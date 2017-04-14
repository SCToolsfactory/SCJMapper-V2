using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Drawing;

using SCJMapper_V2.Properties;
using SCJMapper_V2.SC;
using SCJMapper_V2.Table;
using SCJMapper_V2.Keyboard;
using SCJMapper_V2.Mouse;
using SCJMapper_V2.Gamepad;
using SCJMapper_V2.Joystick;
using SCJMapper_V2.Options;

namespace SCJMapper_V2
{
  /// <summary>
  /// Maintains the action tree and its GUI representation, the TreeView
  ///  - the TreeView is managed primary in memory (Master Tree) and copied to the GUI tree via the Filter functions
  /// </summary>
  class ActionTree
  {
    private static readonly log4net.ILog log = log4net.LogManager.GetLogger( System.Reflection.MethodBase.GetCurrentMethod( ).DeclaringType );

    #region Static Parts

    // static fonts to be used instead of newly allocated ones each time (init when the Ctrl is assigned)
    static public Font FontActionmap = null;
    static public Font FontAction = null;
    static public Font FontActionActivated = null;

    #endregion

    public event EventHandler<ActionTreeEventArgs> NodeSelectedEvent;
    private void NodeSelected( string action, string ctrl )
    {
      NodeSelectedEvent?.Invoke( this, new ActionTreeEventArgs( action, ctrl ) );
    }


    public ActionMapsCls ActionMaps { get; set; }   // the Action Maps and Actions

    private TreeView m_MasterTree = new TreeView( ); // the master TreeView (mem only)
    private TreeView m_tvCtrlRef = null; // the TreeView in the GUI - injected from GUI via Ctrl property
    public TreeView Ctrl
    {
      get { return m_tvCtrlRef; }
      set {
        m_tvCtrlRef = value;
        // copy props needed
        m_MasterTree.Font = m_tvCtrlRef.Font; // assign font to master tree as well
        m_MasterTree.ImageList = m_tvCtrlRef.ImageList;

        // define some const fonts for further use
        FontActionmap = new Font( m_tvCtrlRef.Font, FontStyle.Bold );
        FontAction = new Font( m_tvCtrlRef.Font, FontStyle.Regular );
        FontActionActivated = new Font( m_tvCtrlRef.Font, FontStyle.Underline );

        m_tvCtrlRef.AfterSelect += M_ctrl_AfterSelect;
      }
    }



    /// <summary>
    /// If a node was selected, update it from the tree content
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void M_ctrl_AfterSelect( object sender, TreeViewEventArgs e )
    {
      NodeSelected( this.SelectedAction, this.SelectedCtrl );
    }


    private bool m_showJoy = true;
    private bool m_showGameP = true;
    private bool m_showKbd = true;
    private bool m_showMouse = true;
    private bool m_showMappedOnly = false;


    /// <summary>
    /// maintains the change status (gets reset by reloading the complete tree)
    /// </summary>
    public bool Dirty { get; set; }

    /// <summary>
    /// a comma separated list of actionmaps to ignore
    /// </summary>
    public string IgnoreMaps { get; set; }

    private string m_Filter = ""; // the tree content filter


    /// <summary>
    /// ctor
    /// </summary>
    public ActionTree()
    {
      IgnoreMaps = ""; // nothing to ignore
    }


    /// <summary>
    /// Copy return the complete ActionTree while reassigning JsN
    /// </summary>
    /// <param name="newJsList">The JsN reassign list</param>
    /// <returns>The ActionTree Copy with reassigned input</returns>
    public ActionTree ReassignJsN( JsReassingList newJsList )
    {
      var nTree = (ActionTree)this.MemberwiseClone( ); // shallow copy of the members
      // DeepCopy of the tree
      nTree.ActionMaps = this.ActionMaps.ReassignJsN( newJsList ); // creates the deep copy of the actionmaps

      nTree.Dirty = true;
      return nTree;
    }

    #region Properties

    public bool CanAssignBinding
    {
      get {
        if ( Ctrl.SelectedNode == null ) return false;
        else return ( Ctrl.SelectedNode.Level == 1 ) || ( Ctrl.SelectedNode.Level == 2 );
      }
    }

    public bool CanBlendBinding
    {
      get {
        if ( Ctrl.SelectedNode == null ) return false;
        else return ( Ctrl.SelectedNode.Level == 1 );
      }
    }

    public bool CanClearBinding
    {
      get {
        if ( Ctrl.SelectedNode == null ) return false;
        else return ( Ctrl.SelectedNode.Level == 1 ) && IsMappedAction;
      }
    }


    public bool CanAddBinding
    {
      get {
        if ( Ctrl.SelectedNode == null ) return false;
        else return ( Ctrl.SelectedNode.Level == 1 ) && IsMappedAction;
      }
    }

    public bool CanDelBinding
    {
      get {
        if ( Ctrl.SelectedNode == null ) return false;
        else return ( Ctrl.SelectedNode.Level == 2 );
      }
    }

    public bool IsMappedAction
    {
      get {
        if ( ( Ctrl.SelectedNode.Level == 0 ) || ( Ctrl.SelectedNode.Level > 2 ) ) return false; // not on node
        if ( Ctrl.SelectedNode == null ) return false; // no node selected
        if ( Ctrl.SelectedNode.Parent == null ) return false; // ERROR EXIT

        return ( Ctrl.SelectedNode as ActionTreeNode ).IsMappedAction;
      }
    }


    public bool ShowAction( ActionCls.ActionDevice actDev, string input )
    {
      if ( ActionCls.IsBlendedInput( input ) && m_showMappedOnly ) return false;
      switch ( actDev ) {
        case ActionCls.ActionDevice.AD_Gamepad: return m_showGameP;
        case ActionCls.ActionDevice.AD_Joystick: return m_showJoy;
        case ActionCls.ActionDevice.AD_Keyboard: return m_showKbd;
        case ActionCls.ActionDevice.AD_Mouse: return m_showMouse;
        default: return false;
      }
    }


    #endregion

    /// <summary>
    /// Add a new Action Child to the selected node to apply an addtional mapping
    /// </summary>
    public void AddBinding()
    {
      if ( Ctrl.SelectedNode == null ) return;
      if ( Ctrl.SelectedNode.Level != 1 ) return; // can only add to level 1 nodes

      ActionTreeNode matn = FindMasterAction( (ActionTreeNode)Ctrl.SelectedNode );
      ActionCls ac = FindActionObject( matn.Parent.Name, matn.Name );   // the related action
      // make new items
      ActionTreeInputNode matin = new ActionTreeInputNode( "UNDEF" ); matin.ImageKey = "Add";
      matn.Nodes.Add( matin ); // add to master tree
      ActionCommandCls acc = ac.AddCommand( "", matin.Index );
      // show stuff
      FilterTree( );
      FindAndSelectCtrlByName( matn.Name );
      // jump to the latest if a new one was added
      if ( Ctrl.SelectedNode.LastNode != null ) {
        Ctrl.SelectedNode = Ctrl.SelectedNode.LastNode;
      }
    }


    /// <summary>
    /// Delete the selected ActionChild and remove corresponding ActionCommands
    /// </summary>
    public void DelBinding()
    {
      if ( Ctrl.SelectedNode == null ) return;
      if ( Ctrl.SelectedNode.Level != 2 ) return; // can only delete level 2 nodes

      ActionTreeNode matn = FindMasterAction( (ActionTreeNode)Ctrl.SelectedNode.Parent ); // the parent treenode
      ActionCls ac = FindActionObject( matn.Parent.Name, matn.Name );   // the related action
      // delete items
      ac.DelCommand( Ctrl.SelectedNode.Index );
      matn.Nodes.RemoveAt( Ctrl.SelectedNode.Index );
      Dirty = true;
      // show stuff
      FilterTree( );
      FindAndSelectCtrlByName( matn.Name );
    }


    public void BlendBinding()
    {
      UpdateSelectedItem( DeviceCls.BlendedInput, ActionCls.ActionDevice.AD_Unknown, false );
    }

    public void ClearBinding()
    {
      UpdateSelectedItem( "", ActionCls.ActionDevice.AD_Unknown, false );
    }


    /// <summary>
    /// Gets the JS device that is used for one of the Inversion Items supported
    /// </summary>
    /// <param name="item">The Inversion item</param>
    /// <returns>The device used or null</returns>
    private DeviceCls GetActionInstance( OptionsInvert.Inversions item )
    {
      // must get the jsN information used for Options
      string nodeText = "";
      nodeText = FindAction( OptionsInvert.MappedActions[(int)item].Map, OptionsInvert.MappedActions[(int)item].Action );
      if ( !string.IsNullOrWhiteSpace( nodeText ) ) {
        DeviceCls dev = DeviceInst.JoystickListRef.Find_jsN( JoystickCls.JSNum( ActionTreeNode.CommandFromNodeText( nodeText ) ) );
        if ( dev != null ) return dev;
        else {
          // could be a gamepad then
          if ( ActionTreeNode.CommandFromNodeText( nodeText ).Contains( "xi_" ) ) {
            return DeviceInst.GamepadRef;
          } else return null; // nope...
        }
      }
      return null;
    }


    /// <summary>
    /// Dumps the actions to an XML string
    /// </summary>
    /// <returns>A string containing the XML</returns>
    public string toXML( string fileName )
    {
      if ( ActionMaps != null ) {
        return ActionMaps.toXML( fileName ); // just propagate if possible
      } else {
        log.Error( "ActionTree-toXML: Program error - ActionMaps not yet created" );
        return "";
      }
    }

    #region MasterTree Actions

    private void UpdateMasterNode( ActionTreeNode node )
    {
      // copy to master node
      TreeNode[] masterNode = m_MasterTree.Nodes.Find( node.Name, true ); // find the same node in master
      if ( masterNode.Length == 0 ) throw new IndexOutOfRangeException( "ActionTree ERROR - cannot find synched node in master" ); // OUT OF SYNC
      // could return more than one if the action is the same in different actionmaps
      foreach ( ActionTreeNode mtn in masterNode ) {
        if ( mtn.Parent.Name == node.Parent.Name ) {
          mtn.Update( node ); // update from node
        }
      }
    }

    private void UpdateMasterNode( ActionTreeInputNode node )
    {
      // copy to master node
      TreeNode[] masterNode = m_MasterTree.Nodes.Find( node.Name, true ); // find the same node in master
      if ( masterNode.Length == 0 ) throw new IndexOutOfRangeException( "ActionTree ERROR - cannot find synched node in master" ); // OUT OF SYNC
      // could return more than one if the action is the same in different actionmaps
      foreach ( ActionTreeInputNode mtn in masterNode ) {
        if ( mtn.Parent.Name == node.Parent.Name ) {
          mtn.Update( node ); // update from node
        }
      }
    }


    /// <summary>
    /// Find the master element for the given ActionNode
    /// </summary>
    /// <param name="atn">The ActionNode to find</param>
    /// <returns>The sought node or null</returns>
    private ActionTreeNode FindMasterAction( ActionTreeNode atn )
    {
      if ( atn.Level != 1 ) return null; // sanity

      TreeNode[] masterNode = m_MasterTree.Nodes.Find( atn.Name, true ); // find the same node in master
      if ( masterNode.Length == 0 ) throw new IndexOutOfRangeException( "ActionTree ERROR - cannot find synched node in master" ); // OUT OF SYNC
      // could return more than one if the action is the same in different actionmaps
      foreach ( ActionTreeNode mtn in masterNode ) {
        if ( mtn.Parent.Name == atn.Parent.Name ) {
          return mtn;
        }
      }
      return null;
    }



    /// <summary>
    /// Apply the filter to the GUI TreeView
    /// </summary>
    private void ApplyFilter()
    {
      log.Debug( "ApplyFilter - Entry" );

      ActionTreeNode topNode = null; // allow to backup the view - will carry the first node items

      Ctrl.BeginUpdate( );
      Ctrl.Nodes.Clear( ); // start over

      // traverse the master tree and build the GUI tree from it
      foreach ( ActionTreeNode tn in m_MasterTree.Nodes ) {
        ActionTreeNode tnMap = new ActionTreeNode( tn ); Ctrl.Nodes.Add( tnMap ); // copy level 0 nodes
        if ( topNode == null ) topNode = tnMap;

        // have to search nodes of nodes
        bool allHidden = true;
        foreach ( ActionTreeNode stn in tn.Nodes ) {
          if ( ( stn.Tag != null ) && ( (bool)stn.Tag == true ) ) {
            ;  // don't create it i.e hide it - though you cannot hide TreeViewNodes at all...
          } else {
            ActionTreeNode tnAction = new ActionTreeNode( stn ); tnMap.Nodes.Add( tnAction ); // copy level 1 nodes
            foreach ( ActionTreeInputNode istn in stn.Nodes ) {
              ActionTreeInputNode tnActionInput = new ActionTreeInputNode( istn ); tnAction.Nodes.Add( tnActionInput ); // copy level 2 nodes
            }
            allHidden = false;
          }
        }
        // make it tidier..
        if ( allHidden ) tnMap.Collapse( );
        else tnMap.ExpandAll( );
      }
      if ( topNode != null ) Ctrl.TopNode = topNode; // set view to topnode

      Ctrl.EndUpdate( ); // enable GUI update
      NodeSelected( this.SelectedAction, this.SelectedCtrl );
    }


    /// <summary>
    /// Filters the master tree  - only Actions (level 1) and not actionmaps (level 0)
    ///   - Tag gets Bool hidden=true if not to be shown
    /// </summary>
    private void FilterTree()
    {
      bool hidden = !string.IsNullOrEmpty( m_Filter ); // hide only if there is a find string
      foreach ( ActionTreeNode tn in m_MasterTree.Nodes ) {
        // have to search nodes of nodes
        foreach ( ActionTreeNode stn in tn.Nodes ) {
          stn.Tag = null; // default
          if ( ( !m_showJoy ) && stn.IsJoystickAction ) stn.Tag = true;
          if ( ( !m_showGameP ) && stn.IsGamepadAction ) stn.Tag = true;
          if ( ( !m_showKbd ) && stn.IsKeyboardAction ) stn.Tag = true;
          if ( ( !m_showMouse ) && stn.IsMouseAction ) stn.Tag = true;
          if ( m_showMappedOnly && ( !stn.IsMappedAction ) ) stn.Tag = true;
          if ( !stn.Text.Contains( m_Filter ) ) stn.Tag = hidden;
        }
      }
      ApplyFilter( ); // to the GUI tree
    }


    /// <summary>
    /// Filters entries with given criteria
    /// </summary>
    /// <param name="filter">The text snip to filter</param>
    public void FilterTree( string filter )
    {
      m_Filter = filter;
      FilterTree( );
    }

    #endregion

    /// <summary>
    /// Load Mappings into the ActionList and create the Master TreeView 
    /// </summary>
    /// <param name="defaultProfileName">The name of the profile to load (w extension)</param>
    /// <param name="applyDefaults">True if default mappings should be carried on</param>
    public void LoadProfileTree( string defaultProfileName, bool applyDefaults )
    {
      log.Debug( "LoadProfileTree - Entry" );

      ActionTreeNode tn = null;
      ActionTreeNode[] cnl = { };
      ActionTreeNode cn = null;
      ActionTreeNode topNode = null;

      ActionMapCls acm = null;
      ActionCls ac = null;
      ActionCommandCls acc = null;

      ActionMaps = new ActionMapsCls( );
      m_MasterTree.Nodes.Clear( );

      // read the action items into the TreeView
      DProfileReader dpReader = new DProfileReader( ); // we may read a profile
      TextReader txReader = null;

      dpReader.fromXML( SCDefaultProfile.DefaultProfile( defaultProfileName ) );
      if ( dpReader.ValidContent ) {
        txReader = new StringReader( dpReader.CSVMap );
      }

      // we assume no addbind items in the profile
      //  so all actions are shown in the ActionTreeNode and no ActionTreeNode childs must be created here
      //  however we create the ActionCommand for each entry that is supported - even if it is not mapped (input= "")
      using ( TextReader sr = txReader ) {
        string buf = sr.ReadLine( );
        while ( !string.IsNullOrEmpty( buf ) ) {
          string[] elem = buf.Split( new char[] { ';', ',' }, StringSplitOptions.None );
          if ( elem.Length > 1 ) {
            if ( !IgnoreMaps.Contains( "," + elem[0] + "," ) ) {
              // must have 2 elements min
              Array.Resize( ref cnl, 0 );
              acm = new ActionMapCls( ); acm.name = elem[0]; // get actionmap name
              // process items
              for ( int ei = 1; ei < elem.Length; ei += 4 ) { // step 2  - action;defaultBinding;defaultActivationMode;defMultiTap come in as quadrupples
                if ( !string.IsNullOrEmpty( elem[ei] ) ) {
                  // default assignments
                  string action = elem[ei].Substring( 1 );
                  string defBinding = elem[ei + 1];
                  string defActivationModeName = elem[ei + 2];
                  int defMultiTap = int.Parse( elem[ei + 3] );
                  // need to create a ActivationMode here
                  ActivationMode defActivationMode = new ActivationMode( defActivationModeName, defMultiTap );

                  string devID = elem[ei].Substring( 0, 1 );
                  string device = ActionCls.DeviceClassFromTag( devID );

                  // visual item for the action
                  cn = new ActionTreeNode( "UNDEF" ); cn.Name = elem[ei]; cn.Action = action; cn.BackColor = Color.White;  // name with the key it to find it..                
                  cn.ImageKey = devID; cn.BackColor = Color.White; // some stuff does not work properly...
                  if ( ActivationMode.IsDefault( defActivationModeName ) ) {
                    cn.NodeFont = FontAction;
                  } else {
                    cn.NodeFont = FontActionActivated;
                  }
                  Array.Resize( ref cnl, cnl.Length + 1 ); cnl[cnl.Length - 1] = cn;

                  // derive content tree
                  ac = new ActionCls( ); ac.key = cn.Name; ac.name = action; ac.device = device; ac.actionDevice = ActionCls.ADevice( device );
                  ac.defBinding = defBinding; ac.defActivationMode = defActivationMode;
                  acm.Add( ac ); // add to our map
                  cn.ActionDevice = ac.actionDevice; // should be known now
                  // create just an unmapped ActionCommand item 
                  acc = ac.AddCommand( "", -1 ); // profile items are shown in the ActionTreeNode (not in a child)

                  // init and apply the default mappings if requested
                  if ( ac.actionDevice == ActionCls.ActionDevice.AD_Joystick ) {
                    acc.DevID = JoystickCls.DeviceID;
                    int jNum = JoystickCls.JSNum( ac.defBinding );
                    if ( applyDefaults ) {
                      if ( JoystickCls.IsJSValid( jNum ) ) {
                        acc.DevInput = ac.defBinding;
                        cn.Command = ac.defBinding; cn.BackColor = JoystickCls.JsNColor( jNum );
                      }
                    }
                  } else if ( ac.actionDevice == ActionCls.ActionDevice.AD_Gamepad ) {
                    acc.DevID = GamepadCls.DeviceID;
                    if ( applyDefaults ) {
                      if ( !string.IsNullOrEmpty( ac.defBinding ) ) {
                        acc.DevInput = ac.defBinding;
                        cn.Command = ac.defBinding; cn.BackColor = GamepadCls.XiColor( );
                      }
                    }
                  } else if ( ac.actionDevice == ActionCls.ActionDevice.AD_Keyboard ) {
                    acc.DevID = KeyboardCls.DeviceID;
                    if ( applyDefaults ) {
                      if ( !string.IsNullOrEmpty( ac.defBinding ) ) {
                        acc.DevInput = ac.defBinding;
                        cn.Command = ac.defBinding; cn.BackColor = KeyboardCls.KbdColor( );
                      }
                    }
                  } else if ( ac.actionDevice == ActionCls.ActionDevice.AD_Mouse ) {  // 20151220BM: add mouse device (from AC 2.0 defaultProfile usage)
                    acc.DevID = MouseCls.DeviceID;
                    if ( applyDefaults ) {
                      if ( !string.IsNullOrEmpty( ac.defBinding ) ) {
                        acc.DevInput = ac.defBinding;
                        cn.Command = ac.defBinding; cn.BackColor = MouseCls.MouseColor( );
                      }
                    }
                  }
                }
              }//for

              tn = new ActionTreeNode( acm.name, cnl ); tn.Name = acm.name; tn.Action = acm.name; // name it to find it..
              tn.ImageIndex = 0; tn.NodeFont = FontActionmap; // new Font( m_MasterTree.Font, FontStyle.Bold );
              m_MasterTree.BackColor = Color.White; // fix for defect TreeView (cut off bold text)
              m_MasterTree.Nodes.Add( tn ); // add to control
              if ( topNode == null ) topNode = tn; // once to keep the start of list
              ActionMaps.Add( acm ); // add to our map
            }//not ignored

          }// if valid line
          buf = sr.ReadLine( );
        }//while
      }
      // fix for defect TreeView (cut off bold text at last element -despite the BackColor fix) add another and delete it 
      tn = new ActionTreeNode( "DUMMY" ); tn.Name = "DUMMY";
      tn.ImageIndex = 0; tn.NodeFont = FontActionmap; // new Font( m_MasterTree.Font, FontStyle.Bold );
      m_MasterTree.BackColor = m_MasterTree.BackColor; // fix for defect TreeView (cut off bold text)
      m_MasterTree.Nodes.Add( tn ); // add to control
      m_MasterTree.Nodes.RemoveByKey( "DUMMY" );
      // fix for defect TreeView (cut off bold text)

      txReader = null;
      Dirty = false;

      // finally apply the filter and make it visible
      ReloadTreeView( ); // performs the complete job..
    }


    /// <summary>
    /// Find the ActivationModes of the selected item
    /// </summary>
    /// <returns>A ActivationModes list - first element is the default, second the selected Mode</returns>
    public ActivationModes ActivationModeSelectedItem()
    {
      ActivationModes am = new ActivationModes( ActivationMode.Default, ActivationMode.Default );// policy: get the default first, then the attached one - dummy answer

      if ( Ctrl.SelectedNode == null ) return am; // ERROR exit
      if ( ( Ctrl.SelectedNode.Level == 0 ) || ( Ctrl.SelectedNode.Level > 2 ) ) return am;
      if ( Ctrl.SelectedNode.Parent == null ) return am; // ERROR exit

      // has a parent - must be level 1 or 2 
      if ( Ctrl.SelectedNode.Level == 1 ) {
        // this is the main node with Action Cmd
        ActionTreeNode atn = ( Ctrl.SelectedNode as ActionTreeNode );  // the treenode from a level 1
        ActionCls ac = FindActionObject( atn.Parent.Name, atn.Name ); if ( ac == null ) return am; // ERROR exit
        ActionCommandCls acc = ac.FindActionInputObject( ActionTreeNode.CommandFromNodeText( atn.Text ) ); if ( acc == null ) return am; // ERROR exit
        am = new ActivationModes( ac.defActivationMode, acc.ActivationMode ); // policy: get the default first, then the attached one
        return am;
      } else if ( Ctrl.SelectedNode.Level == 2 ) {
        // this is a child of an action with further commands
        ActionTreeNode patn = ( Ctrl.SelectedNode.Parent as ActionTreeNode );  // the parent treenode
        ActionTreeNode atn = ( Ctrl.SelectedNode as ActionTreeNode );  // the treenode from a level 1
        // the related action
        ActionCls ac = FindActionObject( patn.Parent.Name, patn.Name ); if ( ac == null ) return am; // ERROR exit
        ActionCommandCls acc = ac.FindActionInputObject( atn.Index ); if ( acc == null ) return am; // ERROR exit
        am = new ActivationModes( ac.defActivationMode, acc.ActivationMode );// policy: get the default first, then the attached one
        return am;
      }

      return am;
    }


    /// <summary>
    /// Update the ActivationMode of the selected item
    /// </summary>
    public void UpdateActivationModeSelectedItem( string newActivationModeName )
    {
      if ( Ctrl.SelectedNode == null ) return; // ERROR exit
      if ( ( Ctrl.SelectedNode.Level == 0 ) || ( Ctrl.SelectedNode.Level > 2 ) ) return;
      if ( Ctrl.SelectedNode.Parent == null ) return; // ERROR exit

      // has a parent - must be level 1 or 2 
      if ( Ctrl.SelectedNode.Level == 1 ) {
        // this is the main node with Action Cmd
        ActionTreeNode atn = ( Ctrl.SelectedNode as ActionTreeNode );  // the treenode from a level 1
        ActionCls ac = FindActionObject( atn.Parent.Name, atn.Name ); if ( ac == null ) return; // ERROR exit
        ActionCommandCls acc = ac.FindActionInputObject( ActionTreeNode.CommandFromNodeText( atn.Text ) ); if ( acc == null ) return; // ERROR exit
        // new am is either a named one or the Default from Profile (which is the default from the Action due to multiTaps..)
        if ( ActivationMode.IsDefault( newActivationModeName ) ) {
          acc.ActivationMode = new ActivationMode( ActivationMode.Default );
        } else {
          acc.ActivationMode = ActivationModes.Instance.ActivationModeByName( newActivationModeName );
        }
        atn.UpdateAction( acc ); UpdateMasterNode( atn );
        NodeSelected( this.SelectedAction, this.SelectedCtrl ); // virtual event - as the selection does not change
        Dirty = true;

      } else if ( Ctrl.SelectedNode.Level == 2 ) {
        // this is a child of an action with further commands
        ActionTreeNode patn = ( Ctrl.SelectedNode.Parent as ActionTreeNode );  // the parent treenode from a level 2
        ActionTreeNode atn = ( Ctrl.SelectedNode as ActionTreeNode );  // the treenode from a level 2
        // the related action
        ActionCls ac = FindActionObject( patn.Parent.Name, patn.Name ); if ( ac == null ) return; // ERROR exit
        // find it in the sublist 
        ActionCommandCls acc = ac.FindActionInputObject( atn.Index ); if ( acc == null ) return; // ERROR exit
        // new am is either a named one or the Default from Profile (which is the default from the Action due to multiTaps..)
        if ( ActivationMode.IsDefault( newActivationModeName ) ) {
          acc.ActivationMode = new ActivationMode( ActivationMode.Default );
        } else {
          acc.ActivationMode = ActivationModes.Instance.ActivationModeByName( newActivationModeName );
        }
        atn.UpdateAction( acc ); UpdateMasterNode( atn );
        NodeSelected( this.SelectedAction, this.SelectedCtrl ); // virtual event - as the selection does not change
        Dirty = true;
      }
    }


    // input is like  js1_button3 OR keyboard such as lctrl+x (mouse is keyboard too)
    /// <summary>
    /// Apply an update the the treenode
    /// First apply to the GUI tree where the selection happend then copy it over to master tree
    /// </summary>
    /// <param name="input">The new Text property</param>
    public bool UpdateSelectedItem( string input, ActionCls.ActionDevice inKind, bool checkKind )
    {
      if ( Ctrl.SelectedNode == null ) return false; // ERROR exit
      if ( ( Ctrl.SelectedNode.Level == 0 ) || ( Ctrl.SelectedNode.Level > 2 ) ) return false; // ERROR exit
      if ( Ctrl.SelectedNode.Parent == null ) return false; // ERROR exit

      // has a parent - must be level 1 or 2 
      if ( Ctrl.SelectedNode.Level == 1 ) {
        // this is the main node with Action Cmd
        ActionTreeNode atn = ( Ctrl.SelectedNode as ActionTreeNode );  // the treenode from a level 1
        ActionCls ac = FindActionObject( atn.Parent.Name, atn.Name );   // the related action in an actionmap
        if ( ac == null ) return false; // ERROR exit
        if ( checkKind && ( ac.actionDevice != inKind ) ) return false; // ERROR exit
        ActionCommandCls acc = ac.FindActionInputObject( ActionTreeNode.CommandFromNodeText( atn.Text ) );
        if ( acc == null ) return false; // ERROR exit
        // have it - continue
        ac.UpdateCommandFromInput( ActionCls.DevInput( input, inKind ), acc.NodeIndex + 1 );
        atn.UpdateAction( acc ); UpdateMasterNode( atn );
        NodeSelected( this.SelectedAction, this.SelectedCtrl ); // virtual event - as the selection does not change
        Dirty = true;

      } else if ( Ctrl.SelectedNode.Level == 2 ) {
        // this is a child of an action with further commands
        ActionTreeNode patn = ( Ctrl.SelectedNode.Parent as ActionTreeNode );  // the parent treenode from a level 2
        ActionTreeNode atn = ( Ctrl.SelectedNode as ActionTreeNode );  // the treenode from a level 2
        if ( string.IsNullOrEmpty( input ) )
          atn.Action = "UNDEF"; // apply UNDEF
        else
          atn.Action = ""; // remove UNDEF
        ActionCls ac = FindActionObject( patn.Parent.Name, patn.Name );   // the related action in an actionmap
        if ( ac == null ) return false; // ERROR exit
        if ( checkKind && ( ac.actionDevice != inKind ) ) return false; // ERROR exit
        ActionCommandCls acc = ac.FindActionInputObject( atn.Index );
        if ( acc == null ) return false; // ERROR exit
        // have it - continue
        ac.UpdateCommandFromInput( ActionCls.DevInput( input, inKind ), acc.NodeIndex + 1 );
        atn.UpdateAction( acc ); UpdateMasterNode( atn );
        NodeSelected( this.SelectedAction, this.SelectedCtrl ); // virtual event - as the selection does not change
        Dirty = true;
      }
      return true;
    }

    /// <summary>
    /// Find an action with name in a actionmap
    /// </summary>
    /// <param name="actionMap">The actionmap name</param>
    /// <param name="actionKey">The action</param>
    /// <param name="devInput">The input</param>
    /// <returns>An actionCommand or null if not found</returns>
    private ActionCommandCls FindActionInputObject( string actionMap, string actionKey, string devInput )
    {
      //log.Debug( "FindActionInputObject - Entry" );
      // Apply the input to the ActionTree
      ActionCls ac = null; ActionCommandCls acc = null;
      ActionMapCls ACM = ActionMaps.Find( delegate ( ActionMapCls _ACM ) { return _ACM.name == actionMap; } );
      if ( ACM != null ) ac = ACM.Find( delegate ( ActionCls _AC ) { return _AC.key == actionKey; } );
      if ( ac != null ) acc = ac.inputList.Find( delegate ( ActionCommandCls _ACC ) { return _ACC.DevInput == devInput; } );
      if ( acc == null ) {
        log.Error( "FindActionInputObject - Action Input not found in tree" );
        return null;  // ERROR - Action Input not found in tree
      }
      return acc;
    }

    /// <summary>
    /// Find an action with name in a actionmap
    /// </summary>
    /// <param name="actionMap">The actionmap name</param>
    /// <param name="actionKey">The action</param>
    /// <param name="input">The input</param>
    /// <returns>An actionCommand or null if not found</returns>
    private ActionCommandCls FindActionInputObject( ActionTree tree, string actionMap, string actionKey, int index )
    {
      //log.Debug( "FindActionInputObject - Entry" );
      // Apply the input to the ActionTree
      ActionCls ac = null; ActionCommandCls acc = null;
      ActionMapCls ACM = tree.ActionMaps.Find( delegate ( ActionMapCls _ACM ) { return _ACM.name == actionMap; } );
      if ( ACM != null ) ac = ACM.Find( delegate ( ActionCls _AC ) { return _AC.key == actionKey; } );
      if ( ac != null ) acc = ac.inputList.Find( delegate ( ActionCommandCls _ACC ) { return _ACC.NodeIndex == index; } );
      if ( acc == null ) {
        log.Error( "FindActionInputObject - Action Input not found in tree" );
        return null;  // ERROR - Action Input not found in tree
      }
      return acc;
    }




    /// <summary>
    /// Find an action with name in a actionmap
    /// </summary>
    /// <param name="actionMap">The actionmap name</param>
    /// <param name="action">The action</param>
    /// <returns>An action or null if not found</returns>
    private ActionCls FindActionObject( string actionMap, string action )
    {
      //log.Debug( "FindActionObject - Entry" );
      // Apply the input to the ActionTree
      ActionCls ac = null;
      ActionMapCls ACM = ActionMaps.Find( delegate ( ActionMapCls acm ) { return acm.name == actionMap; } );
      if ( ACM != null ) ac = ACM.Find( delegate ( ActionCls _AC ) { return _AC.key == action; } );
      if ( ac == null ) {
        log.Error( "FindActionObject - Action Not found in tree" );
        return null;  // ERROR - Action Not found in tree
      }
      return ac;
    }


    /// <summary>
    /// Defines what to show in the tree
    /// </summary>
    /// <param name="showJoystick">True to show Joystick actions</param>
    /// <param name="showGamepad">True to show Gamepad actions</param>
    /// <param name="showKeyboard">True to show Keyboard actions</param>
    /// <param name="showMappedOnly">True to show mapped actions only </param>
    public void DefineShowOptions( bool showJoystick, bool showGamepad, bool showKeyboard, bool showMouse, bool showMappedOnly )
    {
      m_showJoy = showJoystick;
      m_showGameP = showGamepad;
      m_showKbd = showKeyboard;
      m_showMouse = showMouse;
      m_showMappedOnly = showMappedOnly;
    }


    /// <summary>
    /// Loads the mappings back into the treeview control
    /// Note: this takes a while as the list grows...
    /// </summary>
    public void ReloadTreeView()
    {
      log.Debug( "ReloadTreeView - Entry" );

      foreach ( ActionMapCls acm in ActionMaps ) {
        if ( IgnoreMaps.Contains( "," + acm.name + "," ) ) break; // next
        try {
          ActionTreeNode mtn = (ActionTreeNode)m_MasterTree.Nodes[acm.name]; // get the map node
          // find the item to reload into the treeview
          foreach ( ActionCls ac in acm ) {
            ActionTreeNode matn = (ActionTreeNode)mtn.Nodes[ac.key];  // get the action node
            bool first = true;
            // refresh commands
            foreach ( ActionCommandCls acc in ac.inputList ) {
              try {
                ac.UpdateCommandFromInput( acc.DevInput, acc.NodeIndex + 1 ); // this may apply (un)Blending if needed
                // the first one goes into the node, further must be created if not existing
                if ( first ) {
                  matn.UpdateAction( acc ); UpdateMasterNode( matn );
                  matn.Nodes.Clear( ); // clear add childs - those don't persist from newly loaded actionmaps
                  first = false;
                } else {
                  // have to recreate the action child nodes
                  ActionTreeInputNode matin = new ActionTreeInputNode( "UNDEF" ); matin.ImageKey = "Add";
                  acc.NodeIndex = matin.Index; // assign visual reference
                  matn.Nodes.Add( matin ); // add to master tree
                  matin.UpdateAction( acc ); UpdateMasterNode( matin );
                }
              } catch {
                ; // key not found
              }
              NodeSelected( this.SelectedAction, this.SelectedCtrl );
              Dirty = true;
            } // foreach
          }
        } catch {
          ; // map key not found ??
        }
      }

      // finally apply the filter and make it visible
      FilterTree( );
    }


    /// <summary>
    /// Find a control that contains the string and mark it
    ///   this method is applied to the GUI TreeView only
    /// </summary>
    /// <param name="m_MasterTree">The string to find</param>
    public void FindAndSelectActionKey( string actionmap, string actionKey, int nodeIndex )
    {
      log.Debug( "FindAndSelectActionKey - Entry" );

      foreach ( ActionTreeNode tn in Ctrl.Nodes ) {
        if ( string.IsNullOrEmpty( actionmap ) || ( tn.Text == actionmap ) ) {
          // have to search nodes of nodes
          foreach ( ActionTreeNode stn in tn.Nodes ) {
            if ( stn.Name == actionKey ) {
              if ( nodeIndex < 0 ) {
                if ( Ctrl.SelectedNode == stn ) NodeSelected( this.SelectedAction, this.SelectedCtrl );
                Ctrl.SelectedNode = stn;
                Ctrl.SelectedNode.EnsureVisible( );
              } else {
                // have to search nodes of nodes
                int ni = 0;
                foreach ( ActionTreeInputNode sstn in stn.Nodes ) {
                  if ( ni++ == nodeIndex ) {
                    if ( Ctrl.SelectedNode == sstn ) NodeSelected( this.SelectedAction, this.SelectedCtrl );
                    Ctrl.SelectedNode = sstn;
                    Ctrl.SelectedNode.EnsureVisible( );
                    return; // exit all loops
                  }
                }
              }
              return; // exit all loops
            }
          }
        }
      }
    }


    /// <summary>
    /// Find a control that contains the string and mark it
    ///   this method is applied to the GUI TreeView only
    /// </summary>
    /// <param name="m_MasterTree">The string to find</param>
    public void FindAndSelectCtrl( string ctrl )
    {
      log.Debug( "FindAndSelectCtrl - Entry" );

      foreach ( ActionTreeNode tn in Ctrl.Nodes ) {
        // have to search nodes of nodes
        foreach ( ActionTreeNode stn in tn.Nodes ) {
          if ( stn.Text.Contains( ctrl ) ) {
            if ( Ctrl.SelectedNode == stn ) NodeSelected( this.SelectedAction, this.SelectedCtrl );
            Ctrl.SelectedNode = stn;
            Ctrl.SelectedNode.EnsureVisible( );
            return; // exit all loops
          }
          // have to search nodes of nodes
          foreach ( ActionTreeInputNode sstn in stn.Nodes ) {
            if ( sstn.Text.Contains( ctrl ) ) {
              if ( Ctrl.SelectedNode == sstn ) NodeSelected( this.SelectedAction, this.SelectedCtrl );
              Ctrl.SelectedNode = sstn;
              Ctrl.SelectedNode.EnsureVisible( );
              return; // exit all loops
            }
          }
        }
      }
    }


    /// <summary>
    /// Find a control that contains the string and mark it
    ///   this method is applied to the GUI TreeView only
    /// </summary>
    /// <param name="m_MasterTree">The string to find</param>
    public void FindAndSelectCtrlByName( string ctrlName )
    {
      log.Debug( "FindAndSelectCtrlByName - Entry" );

      foreach ( ActionTreeNode tn in Ctrl.Nodes ) {
        // have to search nodes of nodes
        foreach ( ActionTreeNode stn in tn.Nodes ) {
          if ( stn.Name == ctrlName ) {
            if ( Ctrl.SelectedNode == stn ) NodeSelected( this.SelectedAction, this.SelectedCtrl );
            Ctrl.SelectedNode = stn;
            Ctrl.SelectedNode.EnsureVisible( );
            return; // exit all loops
          }
          // have to search nodes of nodes
          foreach ( ActionTreeInputNode sstn in stn.Nodes ) {
            if ( sstn.Name == ctrlName ) {
              if ( Ctrl.SelectedNode == sstn ) NodeSelected( this.SelectedAction, this.SelectedCtrl );
              Ctrl.SelectedNode = sstn;
              Ctrl.SelectedNode.EnsureVisible( );
              return; // exit all loops
            }
          }
        }
      }
    }


    /// <summary>
    /// Find a control that contains the Action (exact match)
    /// </summary>
    /// <param name="actionmap">The actionmap to find the string</param>
    /// <param name="text">The string to find</param>
    public string FindActionKey( string actionmap, string actionKey )
    {
      log.Debug( "FindActionKey - Entry" );

      foreach ( ActionTreeNode tn in Ctrl.Nodes ) {
        if ( string.IsNullOrEmpty( actionmap ) || ( tn.Text == actionmap ) ) {
          // have to search nodes of nodes
          foreach ( ActionTreeNode stn in tn.Nodes ) {
            if ( stn.Name == actionKey ) {
              return stn.Text;
            }
          }
        }
      }
      return "";
    }

    /// <summary>
    /// Find a control that contains the Action (exact match)
    /// </summary>
    /// <param name="actionmap">The actionmap to find the string</param>
    /// <param name="text">The string to find</param>
    public string FindAction( string actionmap, string action )
    {
      log.Debug( "FindAction - Entry" );

      foreach ( ActionTreeNode tn in Ctrl.Nodes ) {
        if ( string.IsNullOrEmpty( actionmap ) || ( tn.Text == actionmap ) ) {
          // have to search nodes of nodes
          foreach ( ActionTreeNode stn in tn.Nodes ) {
            if ( stn.Action == action ) {
              return stn.Text;
            }
          }
        }
      }
      return "";
    }

    /// <summary>
    /// Find a control that contains the Action
    /// </summary>
    /// <param name="text">The string to find</param>
    public string FindAction( string action )
    {
      return FindAction( "", action );
    }


    /// <summary>
    /// Find a control that contains the Command
    /// </summary>
    /// <param name="actionmap">The actionmap to find the string</param>
    /// <param name="text">The string to find</param>
    public string FindCommand( string actionmap, string command )
    {
      log.Debug( "FindCommand - Entry" );

      foreach ( ActionTreeNode tn in Ctrl.Nodes ) {
        if ( string.IsNullOrEmpty( actionmap ) || ( tn.Text == actionmap ) ) {
          // have to search nodes of nodes
          foreach ( ActionTreeNode stn in tn.Nodes ) {
            foreach ( ActionTreeInputNode istn in stn.Nodes ) {
              if ( istn.Command.Contains( command ) ) {
                return stn.Text + " - " + istn.Text;
              }
            }
          }
        }
      }
      return "";
    }

    /// <summary>
    /// Find a control that contains the Command
    /// </summary>
    /// <param name="text">The string to find</param>
    public string FindCommand( string command )
    {
      return FindCommand( "", command );
    }


    /// <summary>
    /// Find a control the the actionmap that contains the Text
    /// </summary>
    /// <param name="actionmap">The actionmap to find the string</param>
    /// <param name="text">The string to find</param>
    public string FindText( string actionmap, string text )
    {
      log.Debug( "FindText - Entry" );

      foreach ( ActionTreeNode tn in m_MasterTree.Nodes ) {
        if ( string.IsNullOrEmpty( actionmap ) || ( tn.Text == actionmap ) ) {
          // have to search nodes of nodes
          foreach ( ActionTreeNode stn in tn.Nodes ) {
            if ( stn.Text.Contains( text ) ) {
              return stn.Text;
            }
          }
        }
      }
      return "";
    }

    /// <summary>
    /// Find a control that contains the Text
    /// </summary>
    /// <param name="text">The string to find</param>
    public string FindText( string text )
    {
      return FindText( "", text );
    }


    /// <summary>
    /// Returns the Action name of the selected item
    /// </summary>
    public string SelectedAction
    {
      get {
        if ( Ctrl.SelectedNode == null ) return "";
        if ( Ctrl.SelectedNode.Level == 1 ) {
          ActionTreeNode matn = FindMasterAction( (ActionTreeNode)Ctrl.SelectedNode );
          return ActionTreeNode.ActionFromNodeText( matn.Text );
        } else if ( Ctrl.SelectedNode.Level == 2 ) {
          ActionTreeNode matn = FindMasterAction( (ActionTreeNode)Ctrl.SelectedNode.Parent ); // the parent treenode
          return ActionTreeNode.ActionFromNodeText( matn.Text );
        } else return "";
      }
    }

    /// <summary>
    /// Returns the ActionID (DS_ActionMap) of the selected item
    /// </summary>
    public string SelectedActionID
    {
      get {
        if ( Ctrl.SelectedNode == null ) return "";
        if ( ( Ctrl.SelectedNode.Level == 0 ) || ( Ctrl.SelectedNode.Level > 2 ) ) return ""; // ERROR exit
        if ( Ctrl.SelectedNode.Parent == null ) return ""; // ERROR exit

        // has a parent - must be level 1 or 2 
        if ( Ctrl.SelectedNode.Level == 1 ) {
          // this is the main node with Action Cmd
          ActionTreeNode atn = ( Ctrl.SelectedNode as ActionTreeNode );  // the treenode from a level 1
          ActionCls ac = FindActionObject( atn.Parent.Name, atn.Name );   // the related action in an actionmap
          if ( ac == null ) return ""; // ERROR exit
          ActionCommandCls acc = ac.FindActionInputObject( ActionTreeNode.CommandFromNodeText( atn.Text ) );
          if ( acc == null ) return ""; // ERROR exit
          // have it - continue
          string actionID = DS_ActionMap.ActionID( atn.Parent.Name, ac.key, acc.NodeIndex );
          return actionID;

        } else if ( Ctrl.SelectedNode.Level == 2 ) {
          // this is a child of an action with further commands
          ActionTreeNode patn = ( Ctrl.SelectedNode.Parent as ActionTreeNode );  // the parent treenode from a level 2
          ActionTreeNode atn = ( Ctrl.SelectedNode as ActionTreeNode );  // the treenode from a level 2
          ActionCls ac = FindActionObject( patn.Parent.Name, patn.Name );   // the related action in an actionmap
          if ( ac == null ) return ""; // ERROR exit
          ActionCommandCls acc = ac.FindActionInputObject( atn.Index );
          if ( acc == null ) return ""; // ERROR exit
          // have it - continue
          string actionID = DS_ActionMap.ActionID( atn.Parent.Name, ac.key, acc.NodeIndex );
          return actionID;

        } else return "";
      }
    }

    /// <summary>
    /// Returns the dev control of the selected item
    /// </summary>
    public string SelectedCtrl
    {
      get {
        if ( Ctrl.SelectedNode == null ) return "";
        if ( Ctrl.SelectedNode.Level == 1 ) {
          ActionTreeNode matn = FindMasterAction( (ActionTreeNode)Ctrl.SelectedNode );
          return ActionTreeNode.CommandFromNodeText( matn.Text );
        } else if ( Ctrl.SelectedNode.Level == 2 ) {
          ActionTreeNode matn = FindMasterAction( (ActionTreeNode)Ctrl.SelectedNode.Parent ); // the parent treenode
          return ActionTreeNode.CommandFromNodeText( matn.Text );
        } else return "";
      }
    }

    /// <summary>
    /// Update from all edits in the dataset DS_ActionMaps
    /// </summary>/// <param name="dsa">The dataset to update from</param>
    /// <returns>returns a null if no changes have been found</returns>
    public ActionTree UpdateFromDataSet( DS_ActionMaps dsa )
    {
      ActionTree nTree = new ActionTree( ); // just a copy
      // full copy from 'this'
      nTree.m_MasterTree = this.m_MasterTree;
      nTree.m_tvCtrlRef = this.m_tvCtrlRef;
      nTree.IgnoreMaps = this.IgnoreMaps;
      nTree.m_Filter = this.m_Filter;
      nTree.ActionMaps = this.ActionMaps.ReassignJsN( new JsReassingList( ) ); // re-use this method with no reassign for full copy of the tree

      int countChanges = 0;
      foreach ( DS_ActionMaps.T_ActionRow ar in dsa.T_Action ) {
        if ( ar.RowState == System.Data.DataRowState.Modified ) {
          countChanges++;
          ActionCommandCls acc = FindActionInputObject( nTree, DS_ActionMap.ActionMap( ar ), DS_ActionMap.ActionKey( ar ), DS_ActionMap.ActionCommandIndex( ar ) );
          if ( acc != null ) {
            acc.UpdateCommandFromInput( ActionCls.DevInput( DS_ActionMap.DevInput( ar ), ActionCls.ADevice( ar.Device ) ), ActionCls.ADevice( ar.Device ) );
            ar.Usr_Binding = acc.DevInput; // feedback the right one
          } else {
            ; // DEBUG  should not happen...
          }
        }
      }

      // finally if there were any changes
      if ( countChanges > 0 ) {
        dsa.AcceptChanges( );

        NodeSelected( this.SelectedAction, this.SelectedCtrl );
        nTree.Dirty = true;
        return nTree;
      } else {
        return null;
      }

    }



    /// <summary>
    /// Reports a summary list of the mapped items
    /// </summary>
    /// <returns></returns>
    public string ReportActions()
    {
      log.Debug( "FindCtrl - ReportActions" );

      string repList = "";
      // JS assignments
      for ( int i = 0; i < JoystickCls.JSnum_MAX; i++ ) {
        if ( !string.IsNullOrEmpty( ActionMaps.jsN[i] ) ) repList += string.Format( "** js{0} = {1}\n", i + 1, ActionMaps.jsN[i] );
      }
      // now the mapped actions
      const int padAction = 42;
      const int padDevice = 4;
      const int padInput = 25;

      repList += string.Format( "\n" );
      repList += string.Format( " {0}+- {1} _ {2}#-[{4}] {3}\n\n", "Action".PadRight( padAction ), "Dev".PadRight( padDevice ), "Binding".PadRight( padInput ), "Activation", "T" ); // col description line

      foreach ( ActionMapCls acm in ActionMaps ) {
        string rep = string.Format( "*** {0}\n", acm.name );
        repList += rep;
        foreach ( ActionCls ac in acm ) {
          foreach ( ActionCommandCls acc in ac.inputList ) {
            if ( ShowAction( ac.actionDevice, acc.Input ) ) {
              if ( !string.IsNullOrEmpty( acc.Input ) /* && !( acc.Input == DeviceCls.BlendedInput )*/ ) {
                if ( acc.DevInput == ac.defBinding ) {
                  rep = string.Format( " {0} . {1} _ {2}", ac.name.PadRight( padAction ), acc.DevID.PadRight( padDevice ), acc.Input.PadRight( padInput ) );
                } else {
                  rep = string.Format( " {0} + {1} _ {2}", ac.name.PadRight( padAction ), acc.DevID.PadRight( padDevice ), acc.Input.PadRight( padInput ) ); // my binding
                }
                if ( acc.ActivationMode == ActivationMode.Default ) {
                  rep += string.Format( " . [{1}] {0}\n", ac.defActivationMode.Name, ac.defActivationMode.MultiTap );
                } else {
                  rep += string.Format( " # [{1}] {0}\n", acc.ActivationMode.Name, acc.ActivationMode.MultiTap );
                }

                repList += rep;
              }
            }
          }
        }
        repList += string.Format( "\n" ); // actionmap spacer
      }
      return repList;
    }


    /// <summary>
    /// Reports a summary list of the mapped items
    /// </summary>
    /// <returns></returns>
    public string ReportActionsCSV( bool listModifiers )
    {
      log.Debug( "FindCtrl - ReportActions2" );

      string repList = "";
      // JS assignments
      for ( int i = 0; i < JoystickCls.JSnum_MAX; i++ ) {
        if ( !string.IsNullOrEmpty( ActionMaps.jsN[i] ) ) repList += string.Format( "** js{0} = {1}\n", i + 1, ActionMaps.jsN[i] );
      }
      // now the mapped actions

      repList += string.Format( "\n" );

      repList += "actionmap;action"; // col description line
      if ( listModifiers ) {
        repList += ";kbd-tag;kbd-input;kbd-mod-tag;kbd-mod-mode;kbd-mod-multi"; // col description line
        repList += ";mouse-tag;mouse-input;mouse-mod-tag;mouse-mod-mode;mouse-mod-multi";
        repList += ";xpad-tag;xpad-input;xpad-mod-tag;xpad-mod-mode;xpad-mod-multi";
        repList += ";js1-tag;js1-input;js1-mod-tag;js1-mod-mode;js1-mod-multi";
        repList += ";js2-tag;js2-input;js2-mod-tag;js2-mod-mode;js2-mod-multi";
        repList += ";js3-tag;js3-input;js3-mod-tag;js3-mod-mode;js3-mod-multi";
        repList += ";js4-tag;js4-input;js4-mod-tag;js4-mod-mode;js4-mod-multi";
      } else {
        repList += ";kbd-tag;kbd-input"; // col description line
        repList += ";mouse-tag;mouse-input";
        repList += ";xpad-tag;xpad-input";
        repList += ";js1-tag;js1-input";
        repList += ";js2-tag;js2-input";
        repList += ";js3-tag;js3-input";
        repList += ";js4-tag;js4-input";
      }
      repList += string.Format( "\n" );

      string action = "";
      string rep = ""; string kbA = ""; string moA = ""; string xbA = ""; string[] jsA = new string[] { "", "", "", "" };
      foreach ( ActionMapCls acm in ActionMaps ) {

        foreach ( ActionCls ac in acm ) {
          // we get an action for each device class here - sort it out
          if ( ac.name != action ) {
            // dump if not empty
            if ( !string.IsNullOrEmpty( action ) ) {
              // compose one action
              rep += string.Format( "{0};{1};{2};{3};{4};{5};{6}\n", kbA, moA, xbA, jsA[0], jsA[1], jsA[2], jsA[3] ); // should be one line now
              repList += string.Format( "{0}", rep );  // add to list
            }
            // action changed - restart collection
            action = ac.name;
            rep = string.Format( "{0};{1};", acm.name, ac.name ); // actionmap; action
                                                                  // note: don't add trailing semicolons as the are applied in the output formatting
            if ( listModifiers ) {
              kbA = "n.a.;;;;"; // defaults tag;input;mod-tag;mod-name;mod-mult
            } else {
              kbA = "n.a.;"; // defaults tag;input
            }
            moA = kbA; xbA = kbA;
            jsA = new string[] { kbA, kbA, kbA, kbA };
          }

          foreach ( ActionCommandCls acc in ac.inputList ) {
            // this is for add binds
            if ( ShowAction( ac.actionDevice, acc.Input ) ) {
              if ( !string.IsNullOrEmpty( acc.Input ) ) {
                // set modified  - note: don't add trailing semicolons as the are applied in the output formatting
                string aTag = "modified"; //default or modified
                string aMode = string.Format( "modified;{0};{1}", ac.defActivationMode.Name, ac.defActivationMode.MultiTap );
                // change if they are default mappings
                if ( acc.DevInput == ac.defBinding ) aTag = "default";
                if ( acc.ActivationMode == ActivationMode.Default ) aMode = string.Format( "default;{0};{1}", ac.defActivationMode.Name, ac.defActivationMode.MultiTap );
                if ( listModifiers ) {
                  switch ( ActionCls.ADeviceFromDevID( acc.DevID ) ) {
                    case ActionCls.ActionDevice.AD_Keyboard: kbA = string.Format( "{0};{1};{2}", aTag, acc.Input, aMode ); break;
                    case ActionCls.ActionDevice.AD_Mouse: moA = string.Format( "{0};{1};{2}", aTag, acc.Input, aMode ); break;
                    case ActionCls.ActionDevice.AD_Joystick:
                      int jsNum = JoystickCls.JSNum( acc.DevInput ) - 1;
                      if ( jsNum >= 0 ) jsA[jsNum] = string.Format( "{0};{1};{2}", aTag, acc.Input, aMode ); break;
                    case ActionCls.ActionDevice.AD_Gamepad: xbA = string.Format( "{0};{1};{2}", aTag, acc.Input, aMode ); break;
                    default: break;
                  }//switch
                } else {
                  switch ( ActionCls.ADeviceFromDevID( acc.DevID ) ) {
                    case ActionCls.ActionDevice.AD_Keyboard: kbA = string.Format( "{0};{1}", aTag, acc.Input ); break;
                    case ActionCls.ActionDevice.AD_Mouse: moA = string.Format( "{0};{1}", aTag, acc.Input ); break;
                    case ActionCls.ActionDevice.AD_Joystick:
                      int jsNum = JoystickCls.JSNum( acc.DevInput ) - 1;
                      if ( jsNum >= 0 ) jsA[jsNum] = string.Format( "{0};{1}", aTag, acc.Input ); break;
                    case ActionCls.ActionDevice.AD_Gamepad: xbA = string.Format( "{0};{1}", aTag, acc.Input ); break;
                    default: break;
                  }//switch
                }
              }
            }// show
          }// for aCmd
        }// for action
      }
      // add the last one
      // compose one action
      rep += string.Format( "{0};{1};{2};{3};{4};{5};{6}\n", kbA, moA, xbA, jsA[0], jsA[1], jsA[2], jsA[3] ); // should be one line now
      repList += string.Format( "{0}", rep );  // add to list

      return repList;
    }

  }
}
