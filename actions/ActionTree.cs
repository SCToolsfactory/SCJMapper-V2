using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Drawing;

using SCJMapper_V2.Properties;
using SCJMapper_V2.SC;
using SCJMapper_V2.Table;
using SCJMapper_V2.Devices;
using SCJMapper_V2.Devices.Keyboard;
using SCJMapper_V2.Devices.Mouse;
using SCJMapper_V2.Devices.Gamepad;
using SCJMapper_V2.Devices.Joystick;
using SCJMapper_V2.Translation;

namespace SCJMapper_V2.Actions
{
  /// <summary>
  /// Maintains the action tree and its GUI representation, the TreeView
  ///  - the TreeView is managed primary in memory (Master Tree) and copied to the GUI tree via the Filter functions
  /// </summary>
  class ActionTree : IDisposable
  {
    private static readonly log4net.ILog log = log4net.LogManager.GetLogger( System.Reflection.MethodBase.GetCurrentMethod( ).DeclaringType );

    #region Static Parts

    // static fonts to be used instead of newly allocated ones each time (init when the Ctrl is assigned)
    static public Font FontActionmap = null;
    static public Font FontAction = null;
    static public Font FontActionActivated = null;

    #endregion

    public event EventHandler<ActionTreeEventArgs> NodeSelectedEvent;
    // call when the items are known.
    private void NodeSelected( string action, string ctrl )
    {
      NodeSelectedEvent?.Invoke( this, new ActionTreeEventArgs( action, ctrl ) );
    }
    // call when a selection is updated (finds the items in the master tree for the currently selected Ctrl)
    private void NodeSelected()
    {
      string action = ""; string ctrl = "";
      SelectedActionCtrl( out action, out ctrl );
      NodeSelected( action, ctrl );
    }


    public ActionMapsCls ActionMaps { get; set; }   // the Action Maps and Actions

    private TreeView m_MasterTree = new TreeView( ); // the master TreeView (mem only)
    private TreeView m_tvCtrlRef = null; // the TreeView in the GUI - injected from GUI via Ctrl property
    /// <summary>
    /// Property TreeView Control
    ///   The class is owning the TreeView Control and manages it
    /// </summary>
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
      NodeSelected( );
    }

    // filters
    private bool m_showJoy = true;
    private bool m_showGameP = true;
    private bool m_showKbd = true;
    private bool m_showMouse = true;
    private bool m_showMappedOnly = false;

    private string m_Filter = ""; // the tree content filter


    /// <summary>
    /// maintains the change status (gets reset by reloading the complete tree)
    /// </summary>
    public bool Dirty { get; set; }

    /// <summary>
    /// a comma separated list of actionmaps to ignore
    /// </summary>
    public string IgnoreMaps { get; set; }

    /// <summary>
    /// ctor
    /// </summary>
    public ActionTree()
    {
      log.DebugFormat( "ctor - Entry {0}", m_MasterTree.GetHashCode( ).ToString( ) );
      IgnoreMaps = ""; // nothing to ignore
    }

    protected virtual void Dispose( bool disposing )
    {
      if ( disposing ) {
        // dispose managed resources
        if ( m_MasterTree != null ) m_MasterTree.Dispose( );
      }
      // free native resources
      m_tvCtrlRef.AfterSelect -= M_ctrl_AfterSelect;
    }

    public void Dispose()
    {
      Dispose( true );
      GC.SuppressFinalize( this );
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

    /// <summary>
    /// Returns if one can Assign a new binding on the selected node
    /// </summary>
    public bool CanAssignBinding
    {
      get {
        if ( Ctrl.SelectedNode == null ) return false;
        else return ( Ctrl.SelectedNode.Level == 1 ) || ( Ctrl.SelectedNode.Level == 2 );
      }
    }

    /// <summary>
    /// Returns if one can Disable (mask) a binding on the selected node
    /// </summary>
    public bool CanDisableBinding
    {
      get {
        if ( Ctrl.SelectedNode == null ) return false;
        else return ( Ctrl.SelectedNode.Level == 1 );
      }
    }

    /// <summary>
    /// Returns true if a binding can be cleared on the selected node
    /// </summary>
    public bool CanClearBinding
    {
      get {
        if ( Ctrl.SelectedNode == null ) return false;
        else return ( ( Ctrl.SelectedNode.Level == 1 ) && ( IsMappedAction || IsDisabledAction ) );
      }
    }

    /// <summary>
    /// Returns if one can Add a binding on the selected node
    /// </summary>
    public bool CanAddBinding
    {
      get {
        if ( Ctrl.SelectedNode == null ) return false;
        else return ( Ctrl.SelectedNode.Level == 1 ) && IsMappedAction;
      }
    }

    /// <summary>
    /// Returns if one can Delete a binding on the selected node
    /// </summary>
    public bool CanDelBinding
    {
      get {
        if ( Ctrl.SelectedNode == null ) return false;
        else return ( Ctrl.SelectedNode.Level == 2 );
      }
    }

    /// <summary>
    /// Returns true if the action is mapped
    /// </summary>
    public bool IsMappedAction
    {
      get {
        if ( ( Ctrl.SelectedNode.Level == 0 ) || ( Ctrl.SelectedNode.Level > 2 ) ) return false; // not on node
        if ( Ctrl.SelectedNode == null ) return false; // no node selected
        if ( Ctrl.SelectedNode.Parent == null ) return false; // ERROR EXIT

        return ( Ctrl.SelectedNode as ActionTreeNode ).IsMappedAction;
      }
    }

    /// <summary>
    /// Returns true if the action is disabled
    /// </summary>
    public bool IsDisabledAction
    {
      get {
        if ( ( Ctrl.SelectedNode.Level == 0 ) || ( Ctrl.SelectedNode.Level > 2 ) ) return false; // not on node
        if ( Ctrl.SelectedNode == null ) return false; // no node selected
        if ( Ctrl.SelectedNode.Parent == null ) return false; // ERROR EXIT

        return ( Ctrl.SelectedNode as ActionTreeNode ).IsDisabledAction;
      }
    }

    /// <summary>
    /// Returns if an Action must be shown (based on device, mapped filters)
    /// </summary>
    /// <param name="actDev">The Action device</param>
    /// <param name="input">The device input string</param>
    /// <returns>True if it must be shown</returns>
    public bool ShowAction( Act.ActionDevice actDev, string input )
    {
      if ( Act.IsDisabledInput( input ) && m_showMappedOnly ) return false;
      switch ( actDev ) {
        case Act.ActionDevice.AD_Gamepad: return m_showGameP;
        case Act.ActionDevice.AD_Joystick: return m_showJoy;
        case Act.ActionDevice.AD_Keyboard: return m_showKbd;
        case Act.ActionDevice.AD_Mouse: return m_showMouse;
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
      ActionTreeInputNode matin = new ActionTreeInputNode( "UNDEF" ) {
        ImageKey = "Add"
      };
      matin.Update( matn );
      matin.Command = "";

      matn.Nodes.Add( matin ); // add to master tree
      ActionCommandCls acc = ac.AddCommand( "", matin.Index );
      // show stuff
      FilterTree( );
      FindAndSelectCtrlByName( matn.Name, ( matn.Parent as ActionTreeNode ).Action );
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
      FindAndSelectCtrlByName( matn.Name, ( matn.Parent as ActionTreeNode ).Action );
    }

    /// <summary>
    /// Disables the binding of the selected node
    /// </summary>
    public void DisableBinding()
    {
      UpdateSelectedItem( DeviceCls.DisabledInput, Act.ActionDevice.AD_Unknown, false );
    }

    /// <summary>
    /// Clears the binding of the selected node
    /// </summary>
    public void ClearBinding()
    {
      UpdateSelectedItem( "", Act.ActionDevice.AD_Unknown, false );
    }

    /// <summary>
    /// Dumps the actions to an XML string
    /// </summary>
    /// <returns>A string containing the XML</returns>
    public string toXML( string fileName )
    {
      if ( ActionMaps != null ) {
        return ActionMaps.toXML( fileName ); // just propagate if possible
      }
      else {
        log.Error( "ActionTree-toXML: Program error - ActionMaps not yet created" );
        return "";
      }
    }

    #region MasterTree Actions

    /// <summary>
    /// Update the master tree with the given node
    /// </summary>
    /// <param name="node">The node to update from</param>
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

    /// <summary>
    /// Update the master tree with the given input node
    /// </summary>
    /// <param name="node">The input node to update from</param>
    private void UpdateMasterNode( ActionTreeInputNode node )
    {
      log.DebugFormat( "UpdateMasterNode - Entry {0}", m_MasterTree.GetHashCode( ).ToString( ) );
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
      log.DebugFormat( "FindMasterAction(ActionTreeNode) - Entry {0}", m_MasterTree.GetHashCode( ).ToString( ) );
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
    /// Find the master element for the given ActionTreeInputNode
    /// </summary>
    /// <param name="atn">The ActionTreeInputNode to find</param>
    /// <returns>The sought node or null</returns>
    private ActionTreeInputNode FindMasterAction( ActionTreeInputNode atn )
    {
      log.DebugFormat( "FindMasterAction(ActionTreeInputNode) - Entry {0}", m_MasterTree.GetHashCode( ).ToString( ) );
      if ( atn.Level != 2 ) return null; // sanity

      TreeNode[] masterNode = m_MasterTree.Nodes.Find( atn.Name, true ); // find the same node in master
      if ( masterNode.Length == 0 ) throw new IndexOutOfRangeException( "ActionTree ERROR - cannot find synched node in master" ); // OUT OF SYNC
      // could return more than one if the action is the same in different actionmaps
      foreach ( ActionTreeInputNode mtn in masterNode ) {
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
          }
          else {
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
      NodeSelected( );
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
          //if ( !stn.Text.Contains( m_Filter ) ) stn.Tag = hidden;
          if ( !stn.Contains( m_Filter ) ) stn.Tag = hidden;
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
    /// Load Mappings from defaultProfile into the ActionList and create the Master TreeView 
    /// </summary>
    /// <param name="applyDefaults">True if default mappings should be used as current mappings</param>
    public void LoadProfileTree( bool applyDefaults )
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

      dpReader.fromXML( SCDefaultProfile.DefaultProfile( ) );
      if ( dpReader.ValidContent ) {
        txReader = new StringReader( dpReader.CSVMap );
      }

      // Input is a CSV formatted defaultprofile

      // actmap;actmaplabel;action;actionlabel;defBinding;defActMode;defActMultitap;
      const int iMap = 0, iMapLabel = 1, iAction = 2;

      // we assume no addbind items in the profile
      //  so all actions are shown in the ActionTreeNode and no ActionTreeNode childs must be created here
      //  however we create the ActionCommand for each entry that is supported - even if it is not mapped (input= "")
      using ( TextReader sr = txReader ) {
        string buf = sr.ReadLine( );
        while ( !string.IsNullOrEmpty( buf ) ) {
          string[] elem = buf.Split( new char[] { ';', ',' }, StringSplitOptions.None );
          if ( elem.Length > iAction ) {
            if ( !IgnoreMaps.Contains( "," + elem[iMap] + "," ) ) {
              // must have 2 elements min
              Array.Resize( ref cnl, 0 );
              acm = new ActionMapCls { MapName = elem[iMap] };
              for ( int eIndex = iAction; eIndex < elem.Length; eIndex += 5 ) {
                // step 2  - action;actionlabel;defaultBinding;defaultActivationMode;defMultiTap come in as 5groups
                if ( !string.IsNullOrEmpty( elem[eIndex] ) ) {
                  // default assignments
                  string action = elem[eIndex].Substring( 1 ); // has a device Tag as first char
                  string actionLabel = elem[eIndex + 1];
                  string defBinding = elem[eIndex + 2];
                  string defActivationModeName = elem[eIndex + 3];
                  int defMultiTap = int.Parse( elem[eIndex + 4] );
                  // need to create a ActivationMode here
                  ActivationMode defActivationMode = new ActivationMode( defActivationModeName, defMultiTap );

                  string devID = elem[eIndex].Substring( 0, 1 );
                  string device = Act.DeviceClassFromTag( devID );

                  // visual item for the action
                  cn = new ActionTreeNode( "UNDEF" ) {
                    Name = elem[eIndex], Action = action, BackColor = Color.White, ImageKey = devID // name with the key it to find it..
                  };
                  cn.BackColor = Color.White; // some stuff does not work properly...
                  if ( ActivationMode.IsDefault( defActivationModeName ) ) {
                    cn.NodeFont = FontAction;
                  }
                  else {
                    cn.NodeFont = FontActionActivated;
                  }
                  Array.Resize( ref cnl, cnl.Length + 1 ); cnl[cnl.Length - 1] = cn;

                  // derive content tree
                  ac = new ActionCls {
                    Key = cn.Name, ActionName = action, Device = device, ActionDevice = Act.ADevice( device ),
                    DefBinding = defBinding, DefActivationMode = defActivationMode
                  };
                  acm.Add( ac ); // add to our map
                  cn.ActionDevice = ac.ActionDevice; // should be known now
                  // create just an unmapped ActionCommand item 
                  acc = ac.AddCommand( "", -1 ); // profile items are shown in the ActionTreeNode (not in a child)

                  // init and apply the default mappings if requested
                  if ( ac.ActionDevice == Act.ActionDevice.AD_Joystick ) {
                    acc.DevID = JoystickCls.DeviceID;
                    int jNum = JoystickCls.JSNum( ac.DefBinding );
                    if ( applyDefaults ) {
                      if ( JoystickCls.IsJSValid( jNum ) ) {
                        acc.DevInput = ac.DefBinding;
                        cn.Command = ac.DefBinding; cn.BackColor = JoystickCls.JsNColor( jNum );
                      }
                    }
                  }
                  else if ( ac.ActionDevice == Act.ActionDevice.AD_Gamepad ) {
                    acc.DevID = GamepadCls.DeviceID;
                    if ( applyDefaults ) {
                      if ( !string.IsNullOrEmpty( ac.DefBinding ) ) {
                        acc.DevInput = ac.DefBinding;
                        cn.Command = ac.DefBinding; cn.BackColor = GamepadCls.XiColor( );
                      }
                    }
                  }
                  else if ( ac.ActionDevice == Act.ActionDevice.AD_Keyboard ) {
                    acc.DevID = KeyboardCls.DeviceID;
                    if ( applyDefaults ) {
                      if ( !string.IsNullOrEmpty( ac.DefBinding ) ) {
                        acc.DevInput = ac.DefBinding;
                        cn.Command = ac.DefBinding; cn.BackColor = KeyboardCls.KbdColor( );
                      }
                    }
                  }
                  else if ( ac.ActionDevice == Act.ActionDevice.AD_Mouse ) {  // 20151220BM: add mouse device (from AC 2.0 defaultProfile usage)
                    acc.DevID = MouseCls.DeviceID;
                    if ( applyDefaults ) {
                      if ( !string.IsNullOrEmpty( ac.DefBinding ) ) {
                        acc.DevInput = ac.DefBinding;
                        cn.Command = ac.DefBinding; cn.BackColor = MouseCls.MouseColor( );
                      }
                    }
                  }
                }
              }//for
              // ActionMap node
              tn = new ActionTreeNode( acm.MapName, cnl ) {
                Name = acm.MapName,
                Action = acm.MapName, // name it to find it..
                ImageIndex = 0, NodeFont = FontActionmap // new Font( m_MasterTree.Font, FontStyle.Bold );
              };
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
      tn = new ActionTreeNode( "DUMMY" ) {
        Name = "DUMMY", ImageIndex = 0, NodeFont = FontActionmap // new Font( m_MasterTree.Font, FontStyle.Bold );
      };
      m_MasterTree.BackColor = m_MasterTree.BackColor; // fix for defect TreeView (cut off bold text)
      m_MasterTree.Nodes.Add( tn ); // add to control
      m_MasterTree.Nodes.RemoveByKey( "DUMMY" );
      // fix for defect TreeView (cut off bold text)

      ActionMaps.CreateNewOptions( );
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
                                                                                                   //        ActionCommandCls acc = ac.FindActionInputObject( ActionTreeNode.CommandFromNodeText( atn.Text ) ); if ( acc == null ) return am; // ERROR exit
        ActionCommandCls acc = ac.FindActionInputObject( atn.Command ); if ( acc == null ) return am; // ERROR exit
        am = new ActivationModes( ac.DefActivationMode, acc.ActivationMode ); // policy: get the default first, then the attached one
        return am;
      }
      else if ( Ctrl.SelectedNode.Level == 2 ) {
        // this is a child of an action with further commands
        ActionTreeNode patn = ( Ctrl.SelectedNode.Parent as ActionTreeNode );  // the parent treenode
        ActionTreeNode atn = ( Ctrl.SelectedNode as ActionTreeNode );  // the treenode from a level 1
        // the related action
        ActionCls ac = FindActionObject( patn.Parent.Name, patn.Name ); if ( ac == null ) return am; // ERROR exit
        ActionCommandCls acc = ac.FindActionInputObject( atn.Index ); if ( acc == null ) return am; // ERROR exit
        am = new ActivationModes( ac.DefActivationMode, acc.ActivationMode );// policy: get the default first, then the attached one
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
        ActionCommandCls acc = ac.FindActionInputObject( atn.Command ); if ( acc == null ) return; // ERROR exit
        // new am is either a named one or the Default from Profile (which is the default from the Action due to multiTaps..)
        if ( ActivationMode.IsDefault( newActivationModeName ) ) {
          acc.ActivationMode = new ActivationMode( ActivationMode.Default );
        }
        else {
          acc.ActivationMode = ActivationModes.Instance.ActivationModeByName( newActivationModeName );
        }
        atn.UpdateAction( acc ); UpdateMasterNode( atn );
        NodeSelected( ); // virtual event - as the selection does not change
        Dirty = true;

      }
      else if ( Ctrl.SelectedNode.Level == 2 ) {
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
        }
        else {
          acc.ActivationMode = ActivationModes.Instance.ActivationModeByName( newActivationModeName );
        }
        atn.UpdateAction( acc ); UpdateMasterNode( atn );
        NodeSelected( ); // virtual event - as the selection does not change
        Dirty = true;
      }
    }


    // input is like  js1_button3 OR keyboard such as lctrl+x (mouse is keyboard too)
    /// <summary>
    /// Apply an update the the treenode
    /// First apply to the GUI tree where the selection happend then copy it over to master tree
    /// </summary>
    /// <param name="input">The new Text property</param>
    public bool UpdateSelectedItem( string input, Act.ActionDevice inKind, bool checkKind )
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
        if ( checkKind && ( ac.ActionDevice != inKind ) ) return false; // ERROR exit
        ActionCommandCls acc = ac.FindActionInputObject( atn.Command ); if ( acc == null ) return false; // ERROR exit
        // have it - continue
        ac.UpdateCommandFromInput( Act.DevInput( input, inKind ), acc.NodeIndex + 1 );
        atn.UpdateAction( acc ); UpdateMasterNode( atn );
        NodeSelected( ); // virtual event - as the selection does not change
        Dirty = true;

      }
      else if ( Ctrl.SelectedNode.Level == 2 ) {
        // this is a child of an action with further commands
        ActionTreeNode patn = ( Ctrl.SelectedNode.Parent as ActionTreeNode );  // the parent treenode from a level 2
        ActionTreeInputNode atn = ( Ctrl.SelectedNode as ActionTreeInputNode );  // the treenode from a level 2
        if ( string.IsNullOrEmpty( input ) )
          atn.Action = "UNDEF"; // apply UNDEF
        else
          atn.Action = patn.Action; // apply the parent Action 
        ActionCls ac = FindActionObject( patn.Parent.Name, patn.Name );   // the related action in an actionmap
        if ( ac == null ) return false; // ERROR exit
        if ( checkKind ) {
          if ( ac.ActionDevice == Act.ActionDevice.AD_Keyboard ) {
            if ( ( inKind != Act.ActionDevice.AD_Keyboard ) && ( inKind != Act.ActionDevice.AD_Mouse ) ) return false; // ERROR exit
          }
          else {
            if ( ac.ActionDevice != inKind ) return false; // ERROR exit
          }
        }
        ActionCommandCls acc = ac.FindActionInputObject( atn.Index );
        if ( acc == null ) return false; // ERROR exit
        // have it - continue
        ac.UpdateCommandFromInput( Act.DevInput( input, inKind ), acc.NodeIndex + 1 );
        atn.UpdateAction( acc ); UpdateMasterNode( atn );
        NodeSelected( ); // virtual event - as the selection does not change
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
      // Apply the input to the ActionTree
      ActionCls ac = null; ActionCommandCls acc = null;
      ActionMapCls ACM = ActionMaps.Find( delegate ( ActionMapCls _ACM ) { return _ACM.MapName == actionMap; } );
      if ( ACM != null ) ac = ACM.Find( delegate ( ActionCls _AC ) { return _AC.Key == actionKey; } );
      if ( ac != null ) acc = ac.InputList.Find( delegate ( ActionCommandCls _ACC ) { return _ACC.DevInput == devInput; } );
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
      // Apply the input to the ActionTree
      ActionCls ac = null; ActionCommandCls acc = null;
      ActionMapCls ACM = tree.ActionMaps.Find( delegate ( ActionMapCls _ACM ) { return _ACM.MapName == actionMap; } );
      if ( ACM != null ) ac = ACM.Find( delegate ( ActionCls _AC ) { return _AC.Key == actionKey; } );
      if ( ac != null ) acc = ac.InputList.Find( delegate ( ActionCommandCls _ACC ) { return _ACC.NodeIndex == index; } );
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
      // Apply the input to the ActionTree
      ActionCls ac = null;
      ActionMapCls ACM = ActionMaps.Find( delegate ( ActionMapCls acm ) { return acm.MapName == actionMap; } );
      if ( ACM != null ) ac = ACM.Find( delegate ( ActionCls _AC ) { return _AC.Key == action; } );
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
      log.DebugFormat( "ReloadTreeView - Entry {0}", m_MasterTree.GetHashCode( ).ToString( ) );

      foreach ( ActionMapCls acm in ActionMaps ) {
        if ( IgnoreMaps.Contains( "," + acm.MapName + "," ) ) break; // next
        try {
          ActionTreeNode mtn = (ActionTreeNode)m_MasterTree.Nodes[acm.MapName]; // get the map node
          // find the item to reload into the treeview
          foreach ( ActionCls ac in acm ) {
            ActionTreeNode matn = (ActionTreeNode)mtn.Nodes[ac.Key];  // get the action node
            bool first = true;
            // refresh commands
            foreach ( ActionCommandCls acc in ac.InputList ) {
              try {
                ac.UpdateCommandFromInput( acc.DevInput, acc.NodeIndex + 1 ); // this may apply (un)Blending if needed
                // the first one goes into the node, further must be created if not existing
                if ( first ) {
                  matn.UpdateAction( acc ); UpdateMasterNode( matn );
                  matn.Nodes.Clear( ); // clear add childs - those don't persist from newly loaded actionmaps
                  first = false;
                }
                else {
                  // have to recreate the action child nodes
                  ActionTreeInputNode matin = new ActionTreeInputNode( ac.ActionName ) { ImageKey = "Add" };
                  matin.Update( matn ); matin.Command = "";
                  acc.NodeIndex = matin.Index; // assign visual reference
                  matn.Nodes.Add( matin ); // add to master tree
                  matin.UpdateAction( acc ); UpdateMasterNode( matin );
                }
              }
              catch {
                ; // key not found
              }
              Dirty = true;
            } // foreach
          }
        }
        catch ( Exception e ) {
          log.DebugFormat( "ReloadTreeView - Exception in loading Treeview\n{0}", e.Message ); // map key not found ??
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
        if ( string.IsNullOrEmpty( actionmap ) || ( tn.Action == actionmap ) ) {
          // have to search nodes of nodes
          foreach ( ActionTreeNode stn in tn.Nodes ) {
            if ( stn.Name == actionKey ) {
              if ( nodeIndex < 0 ) {
                if ( Ctrl.SelectedNode == stn ) NodeSelected( );
                Ctrl.SelectedNode = stn;
                Ctrl.SelectedNode.EnsureVisible( );
              }
              else {
                // have to search nodes of nodes
                int ni = 0;
                foreach ( ActionTreeInputNode sstn in stn.Nodes ) {
                  if ( ni++ == nodeIndex ) {
                    if ( Ctrl.SelectedNode == sstn ) NodeSelected( );
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
    public void FindAndSelectCtrl( string ctrl, string actionmap )
    {
      log.Debug( "FindAndSelectCtrl - Entry" );

      foreach ( ActionTreeNode tn in Ctrl.Nodes ) {
        // have to search nodes of nodes
        if ( string.IsNullOrEmpty( actionmap ) || ( tn.Action == actionmap ) ) {
          foreach ( ActionTreeNode stn in tn.Nodes ) {
            if ( stn.ContainsCtrl( ctrl ) ) {
              if ( Ctrl.SelectedNode == stn ) NodeSelected( );
              Ctrl.SelectedNode = stn;
              Ctrl.SelectedNode.EnsureVisible( );
              return; // exit all loops
            }
            // have to search nodes of nodes
            foreach ( ActionTreeInputNode sstn in stn.Nodes ) {
              if ( sstn.ContainsCtrl( ctrl ) ) {
                if ( Ctrl.SelectedNode == sstn ) NodeSelected( );
                Ctrl.SelectedNode = sstn;
                Ctrl.SelectedNode.EnsureVisible( );
                return; // exit all loops
              }
            }
          }
        }
      }
    }

    /// <summary>
    /// Find a control that contains the string and mark it
    ///   this method is applied to the GUI TreeView only
    /// </summary>
    public void FindAndSelectCtrlByName( string ctrlName, string actionmap )
    {
      log.Debug( "FindAndSelectCtrlByName - Entry" );

      foreach ( ActionTreeNode tn in Ctrl.Nodes ) {
        if ( string.IsNullOrEmpty( actionmap ) || ( tn.Action == actionmap ) ) {
          // have to search nodes of nodes
          foreach ( ActionTreeNode stn in tn.Nodes ) {
            if ( stn.Name == ctrlName ) {
              if ( Ctrl.SelectedNode == stn ) NodeSelected( );
              Ctrl.SelectedNode = stn;
              Ctrl.SelectedNode.EnsureVisible( );
              return; // exit all loops
            }
            // have to search nodes of nodes
            foreach ( ActionTreeInputNode sstn in stn.Nodes ) {
              if ( sstn.Name == ctrlName ) {
                if ( Ctrl.SelectedNode == sstn ) NodeSelected( );
                Ctrl.SelectedNode = sstn;
                Ctrl.SelectedNode.EnsureVisible( );
                return; // exit all loops
              }
            }
          }
        }
      }
    }

    /// <summary>
    /// Find all actions that are mapped to this input 
    /// </summary>
    /// <param name="input">The input string to find</param>
    public List<string> GetAllActions( string input )
    {
      List<string> ret = new List<string>( );
      if ( string.IsNullOrEmpty( input ) ) return ret; // nothing to find here...
      if ( Act.IsDisabledInput( input ) ) return ret; // nothing to find here...

      foreach ( ActionMapCls acm in ActionMaps ) {
        // have to search Actions in Maps
        foreach ( ActionCls ac in acm ) {
          if ( ac.DefBinding == input ) {
            ret.Add( ac.ActionName );
          }
          foreach ( ActionCommandCls acc in ac.InputList ) {
            if ( acc.DevInput == input ) {
              ret.Add( ac.ActionName );
            }
          }
        }
      }
      return ret;
    }

    /// <summary>
    /// Find and pretty list all actions that are mapped to this input
    /// </summary>
    /// <param name="input">The input string to find</param>
    public List<string> ListAllActions( string input )
    {
      List<string> ret = new List<string>( );
      if ( string.IsNullOrEmpty( input ) ) return ret; // nothing to find here...
      if ( Act.IsDisabledInput( input ) ) return ret; // nothing to find here...

      ret.Add( "Actions listed for Input: " + input );
      ret.Add( "" );
      ret.Add( "location - action - actionmap - activation mode" );
      ret.Add( "" );
      string aMode = "";
      foreach ( ActionMapCls acm in ActionMaps ) {
        // have to search Actions in Maps
        foreach ( ActionCls ac in acm ) {
          string l = ""; // return line composition
          if ( ac.DefBinding == input ) {
            ret.Add( "" );
            aMode = string.Format( "{0};{1}", ac.DefActivationMode.Name, ac.DefActivationMode.MultiTap );
            l = string.Format( "{0} - {1} - {2} - {3}", "profile", ac.ActionName, acm.MapName, aMode );
            ret.Add( l );
          }
          foreach ( ActionCommandCls acc in ac.InputList ) {
            if ( acc.DevInput == input ) {
              aMode = string.Format( "modified;{0};{1}", acc.ActivationMode.Name, acc.ActivationMode.MultiTap );
              if ( acc.ActivationMode == ActivationMode.Default )
                aMode = string.Format( "default" );
              l = string.Format( "{0} - {1} - {2} - {3}", "mapped ", ac.ActionName, acm.MapName, aMode );
              ret.Add( l );
            }
          }
        }
      }
      return ret;
    }

    /// <summary>
    /// Find and pretty print all actions that are mapped to this input as RTF text
    /// formatted as RTF text
    /// </summary>
    /// <param name="input">The input string to find</param>
    public void ListAllActionsRTF( string input, RTF.RTFformatter rtf, bool inverse = false )
    {
      if ( string.IsNullOrEmpty( input ) ) return; // nothing to find here...
      if ( Act.IsDisabledInput( input ) ) return; // nothing to find here...
      rtf.FontSize( 12 );
      rtf.Write( Tx.Translate( "mapHeader" ) + " " );
      rtf.RBold = true; rtf.WriteLn( input ); rtf.RBold = false;

      rtf.FontSize( 9 ); // 9pt
      rtf.ClearTabs( ); rtf.SetTab( 1200 ); rtf.SetTab( 4660 ); rtf.SetTab( 7500 );

      rtf.WriteLn( );
      rtf.RULine = true;
      rtf.Write( Tx.Translate( "mapLocation" ) );
      rtf.WriteTab( Tx.Translate( "mapAction" ) );
      rtf.WriteTab( Tx.Translate( "mapActionmap" ) );
      rtf.WriteTab( Tx.Translate( "mapActivationMode" ).PadRight( 40 ) ); rtf.WriteLn( );
      rtf.RULine = false;
      rtf.WriteLn( );
      string aMode = "";
      foreach ( ActionMapCls acm in ActionMaps ) {
        // have to search Actions in Maps
        foreach ( ActionCls ac in acm ) {
          bool used = false;
          foreach ( ActionCommandCls acc in ac.InputList ) {
            if ( acc.DevInput == input ) {
              aMode = string.Format( "{0};{1};{2}", Tx.Translate( "mapModified" ), acc.ActivationMode.Name, acc.ActivationMode.MultiTap );
              if ( acc.ActivationMode == ActivationMode.Default )
                aMode = string.Format( "{0}", Tx.Translate( "mapDefault" ) );
              rtf.RHighlightColor = ( inverse ) ? RTF.RTFformatter.ERColor.ERC_DarkGreen : RTF.RTFformatter.ERColor.ERC_Green;
              rtf.Write( Tx.Translate( "mapMapped" ) );
              rtf.WriteTab( SCUiText.Instance.Text( ac.ActionName ) );
              rtf.WriteTab( SCUiText.Instance.Text( acm.MapName ) );
              rtf.WriteTab( aMode.PadRight( 80 ) ); rtf.WriteLn( );
              rtf.RHighlightColor = RTF.RTFformatter.ERColor.ERC_Black; // background
              rtf.WriteLn( );
              used = true;
            }
            used = !string.IsNullOrEmpty( acc.DevInput );
          }
          if ( ( !used ) && ac.DefBinding == input ) {
            aMode = string.Format( "{0};{1}", ac.DefActivationMode.Name, ac.DefActivationMode.MultiTap );
            rtf.Write( Tx.Translate( "mapProfile" ) ); rtf.WriteTab( ac.ActionName ); rtf.WriteTab( acm.MapName ); rtf.WriteTab( aMode ); rtf.WriteLn( );
            rtf.WriteLn( );
          }
        }
      }
    }

    /// <summary>
    /// Find a control the the actionmap that contains the ActionText
    /// </summary>
    /// <param name="actionmap">The actionmap to find the string, empty string matches all actionmaps</param>
    /// <param name="text">The string to find</param>
    public string FindText( string actionmap, string text )
    {
      log.DebugFormat( "FindText - Entry ({0}, {1})", actionmap, text );

      foreach ( ActionTreeNode tn in m_MasterTree.Nodes ) {
        //        if ( string.IsNullOrEmpty( actionmap ) || ( tn.Text == actionmap ) ) {
        if ( string.IsNullOrEmpty( actionmap ) || ( tn.Action == actionmap ) ) {
          // have to search nodes of nodes
          foreach ( ActionTreeNode stn in tn.Nodes ) {
            //if ( stn.Text.Contains( text ) ) {
            if ( stn.Contains( text ) ) {
              return stn.ActionText;
            }
          }
        }
      }
      return "";
    }

    /// <summary>
    /// Find a control that contains the Text (searches all actionmaps)
    /// </summary>
    /// <param name="text">The string to find</param>
    public string FindText( string text )
    {
      return FindText( "", text );
    }

    /// <summary>
    /// Returns Action and Ctrl of the currently selected node
    /// </summary>
    /// <param name="action">The action or empty</param>
    /// <param name="ctrl">The control or empty</param>
    public void SelectedActionCtrl( out string action, out string ctrl )
    {
      action = ""; ctrl = "";
      if ( Ctrl.SelectedNode == null ) return;

      if ( Ctrl.SelectedNode.Level == 1 ) {
        ActionTreeNode matn = FindMasterAction( (ActionTreeNode)Ctrl.SelectedNode );
        action = matn.Action;
        ctrl = matn.Command;
      }
      else if ( Ctrl.SelectedNode.Level == 2 ) {
        ActionTreeNode matn = FindMasterAction( (ActionTreeInputNode)Ctrl.SelectedNode ); // the parent treenode
        action = matn.Action;
        ctrl = matn.Command;
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
          return matn.Command;
        }
        else if ( Ctrl.SelectedNode.Level == 2 ) {
          ActionTreeNode matn = FindMasterAction( (ActionTreeInputNode)Ctrl.SelectedNode ); // the parent treenode
          return matn.Command;
        }
        else return "";
      }
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
          return matn.Action;
        }
        else if ( Ctrl.SelectedNode.Level == 2 ) {
          ActionTreeNode matn = FindMasterAction( (ActionTreeNode)Ctrl.SelectedNode.Parent ); // the parent treenode
          return matn.Action;
        }
        else return "";
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
          ActionCommandCls acc = ac.FindActionInputObject( atn.Command ); if ( acc == null ) return ""; // ERROR exit
          // have it - continue
          string actionID = DS_ActionMap.ActionID( atn.Parent.Name, ac.Key, acc.NodeIndex );
          return actionID;

        }
        else if ( Ctrl.SelectedNode.Level == 2 ) {
          // this is a child of an action with further commands
          ActionTreeNode patn = ( Ctrl.SelectedNode.Parent as ActionTreeNode );  // the parent treenode from a level 2
          ActionTreeNode atn = ( Ctrl.SelectedNode as ActionTreeNode );  // the treenode from a level 2
          ActionCls ac = FindActionObject( patn.Parent.Name, patn.Name );   // the related action in an actionmap
          if ( ac == null ) return ""; // ERROR exit
          ActionCommandCls acc = ac.FindActionInputObject( atn.Index );
          if ( acc == null ) return ""; // ERROR exit
          // have it - continue
          string actionID = DS_ActionMap.ActionID( atn.Parent.Name, ac.Key, acc.NodeIndex );
          return actionID;

        }
        else return "";
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
            acc.UpdateCommandFromInput( Act.DevInput( DS_ActionMap.DevInput( ar ), Act.ADevice( ar.Device ) ), Act.ADevice( ar.Device ) );
            ar.Usr_Binding = acc.DevInput; // feedback the right one
          }
          else {
            ; // DEBUG  should not happen...
          }
        }
      }

      // finally if there were any changes
      if ( countChanges > 0 ) {
        dsa.AcceptChanges( );

        NodeSelected( );
        nTree.Dirty = true;
        return nTree;
      }
      else {
        return null;
      }

    }

    /// <summary>
    /// Reports a summary list of the mapped items
    /// </summary>
    /// <returns></returns>
    public string ReportActions()
    {
      log.Debug( "ReportActions - Entry" );

      string repList = "";
      // JS assignments
      for ( int i = 0; i < JoystickCls.JSnum_MAX; i++ ) {
        if ( !string.IsNullOrEmpty( ActionMaps.jsN[i] ) ) repList += string.Format( "** js{0} = {1}\n", i + 1, ActionMaps.jsN[i] );
      }
      // now the mapped actions
      const int padAction = 50;
      const int padDevice = 4;
      const int padInput = 25;

      repList += string.Format( "\n" );
      repList += string.Format( " {0}+- {1} _ {2}#-[{4}] {3}\n\n", "Action".PadRight( padAction ),
                                                                    "Dev".PadRight( padDevice ),
                                                                    "Binding".PadRight( padInput ),
                                                                    "Activation", "T" ); // col description line

      foreach ( ActionMapCls acm in ActionMaps ) {
        string rep = string.Format( "*** {0}\n", SCUiText.Instance.Text( acm.MapName ) );
        repList += rep;
        foreach ( ActionCls ac in acm ) {
          foreach ( ActionCommandCls acc in ac.InputList ) {
            if ( ShowAction( ac.ActionDevice, acc.Input ) ) {
              if ( !string.IsNullOrEmpty( acc.Input ) /* && !( acc.Input == DeviceCls.BlendedInput )*/ ) {
                string actionName = SCUiText.Instance.Text( ac.ActionName );
                if ( actionName.Length > padAction ) actionName = actionName.Substring( 0, padAction );
                if ( acc.DevInput == ac.DefBinding ) {
                  rep = string.Format( " {0} . {1} _ {2}", actionName.PadRight( padAction ), acc.DevID.PadRight( padDevice ), acc.Input.PadRight( padInput ) );
                }
                else {
                  rep = string.Format( " {0} + {1} _ {2}", actionName.PadRight( padAction ), acc.DevID.PadRight( padDevice ), acc.Input.PadRight( padInput ) ); // my binding
                }
                if ( acc.ActivationMode == ActivationMode.Default ) {
                  rep += string.Format( " . [{1}] {0}\n", ac.DefActivationMode.Name, ac.DefActivationMode.MultiTap );
                }
                else {
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
    /// Reports a list of the mapped items as XML (not CIG style)
    /// </summary>
    /// <returns>XML string</returns>
    public string ReportActionsXML()
    {
      log.Debug( "ReportActionsXML - Entry" );
      /*
       Format:
       <actions>
        <actionmap name="mapname">
          <action name="actionname">
            <rebind input="devID_command" />
          </action>
        </actionmap>
       </actions>
       e.g.
       <actions>
        <actionmap name="spaceship_movement">
          <action name="v_pitch">
            <rebind input="js1_y" />
            <rebind input="mo1_maxis_y" />
          </action>
        </actionmap>
       </actions>
 
      */
      string repList = "";
      repList = string.Format( "<actions>\n" );
      foreach ( ActionMapCls acm in ActionMaps ) {
        repList += string.Format( "\t<actionmap AMname=\"{0}\">\n", acm.MapName );
        // restart output
        string actionName = "", devRep = "";
        foreach ( ActionCls ac in acm ) {
          if ( ac.ActionName == actionName ) {
            // same as before
            // collect further
          }
          else {
            // new action
            // report
            if ( !string.IsNullOrEmpty( devRep ) ) {
              repList += string.Format( "\t\t<action Aname=\"{0}\">\n", actionName );
              repList += devRep;
              repList += string.Format( "\t\t</action>\n" );
            }
            // and reset
            actionName = ac.ActionName; // new one
            devRep = "";
          }
          foreach ( ActionCommandCls acc in ac.InputList ) {
            if ( !Act.IsDisabledInput( acc.Input ) ) {
              if ( !string.IsNullOrEmpty( acc.Input ) ) {
                devRep += string.Format( "\t\t\t<rebind input=\"{0}_{1}\" />\n", acc.DevID, acc.Input );
              }
            }
          }
        }
        // have to report the last one
        if ( !string.IsNullOrEmpty( devRep ) ) {
          repList += string.Format( "\t\t<action Aname=\"{0}\">\n", actionName );
          repList += devRep;
          repList += string.Format( "\t\t</action>\n" );
        }
        repList += string.Format( "\t</actionmap>\n" );
      }
      repList += string.Format( "</actions>\n" );
      return repList;
    }

    /// <summary>
    /// Reports a summary list of the mapped items
    /// </summary>
    /// <param name="listModifiers">Wether or not listing modifiers</param>
    /// <param name="listNativeNames">Wether or not listing the profilenames (false by default)</param>
    /// <returns>A string containing the CSV listing</returns>
    public string ReportActionsCSV( bool listModifiers, bool listProfileNames = false )
    {
      log.Debug( "ReportActionsCSV - Entry" );

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
      }
      else {
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
          if ( ac.ActionName != action ) {
            // dump if not empty
            if ( !string.IsNullOrEmpty( action ) ) {
              // compose one action
              rep += string.Format( "{0};{1};{2};{3};{4};{5};{6}\n", kbA, moA, xbA, jsA[0], jsA[1], jsA[2], jsA[3] ); // should be one line now
              repList += string.Format( "{0}", rep );  // add to list
            }
            // action changed - restart collection
            action = ac.ActionName;
            if ( listProfileNames ) {
              rep = string.Format( "{0};{1};", acm.MapName, ac.ActionName ); // actionmap; action
            }
            else {
              rep = string.Format( "{0};{1};", SCUiText.Instance.Text( acm.MapName ),
                                               SCUiText.Instance.Text( ac.ActionName ) ); // actionmap; action
            }
            // note: don't add trailing semicolons as the are applied in the output formatting
            if ( listModifiers ) {
              kbA = "n.a.;;;;"; // defaults tag;input;mod-tag;mod-name;mod-mult
            }
            else {
              kbA = "n.a.;"; // defaults tag;input
            }
            moA = kbA; xbA = kbA;
            jsA = new string[] { kbA, kbA, kbA, kbA };
          }

          foreach ( ActionCommandCls acc in ac.InputList ) {
            // this is for add binds
            if ( ShowAction( ac.ActionDevice, acc.Input ) ) {
              if ( !string.IsNullOrEmpty( acc.Input ) ) {
                // set modified  - note: don't add trailing semicolons as the are applied in the output formatting
                string aTag = "modified"; //default or modified
                string aMode = string.Format( "modified;{0};{1}", acc.ActivationMode.Name, acc.ActivationMode.MultiTap );
                // change if they are default mappings
                if ( acc.DevInput == ac.DefBinding ) aTag = "default";
                if ( acc.ActivationMode == ActivationMode.Default ) aMode = string.Format( "default;{0};{1}", ac.DefActivationMode.Name, ac.DefActivationMode.MultiTap );
                if ( listModifiers ) {
                  switch ( Act.ADeviceFromDevID( acc.DevID ) ) {
                    case Act.ActionDevice.AD_Keyboard: kbA = string.Format( "{0};{1};{2}", aTag, acc.Input, aMode ); break;
                    case Act.ActionDevice.AD_Mouse: moA = string.Format( "{0};{1};{2}", aTag, acc.Input, aMode ); break;
                    case Act.ActionDevice.AD_Joystick:
                      int jsNum = JoystickCls.JSNum( acc.DevInput ) - 1;
                      if ( jsNum >= 0 ) jsA[jsNum] = string.Format( "{0};{1};{2}", aTag, acc.Input, aMode ); break;
                    case Act.ActionDevice.AD_Gamepad: xbA = string.Format( "{0};{1};{2}", aTag, acc.Input, aMode ); break;
                    default: break;
                  }//switch
                }
                else {
                  switch ( Act.ADeviceFromDevID( acc.DevID ) ) {
                    case Act.ActionDevice.AD_Keyboard: kbA = string.Format( "{0};{1}", aTag, acc.Input ); break;
                    case Act.ActionDevice.AD_Mouse: moA = string.Format( "{0};{1}", aTag, acc.Input ); break;
                    case Act.ActionDevice.AD_Joystick:
                      int jsNum = JoystickCls.JSNum( acc.DevInput ) - 1;
                      if ( jsNum >= 0 ) jsA[jsNum] = string.Format( "{0};{1}", aTag, acc.Input ); break;
                    case Act.ActionDevice.AD_Gamepad: xbA = string.Format( "{0};{1}", aTag, acc.Input ); break;
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
