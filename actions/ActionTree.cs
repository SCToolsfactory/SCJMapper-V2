using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Drawing;
using SCJMapper_V2.Properties;

namespace SCJMapper_V2
{
  /// <summary>
  /// Maintains the action tree and its GUI representation, the TreeView
  ///  - the TreeView is managed primary in memory (Master Tree) and copied to the GUI tree via the Filter functions
  /// </summary>
  class ActionTree
  {
    private static readonly log4net.ILog log = log4net.LogManager.GetLogger( System.Reflection.MethodBase.GetCurrentMethod( ).DeclaringType );


    // static fonts to be used instead of newly allocated ones each time (init when the Ctrl is assigned)
    static public Font FontActionmap = null;
    static public Font FontAction = null;
    static public Font FontActionActivated = null;


    public ActionMapsCls ActionMaps { get; set; }   // the Action Maps and Actions

    private TreeView m_MasterTree = new TreeView( ); // the master TreeView (mem only)
    private TreeView m_ctrl = null; // the TreeView in the GUI - injected from GUI via Ctrl property
    public TreeView Ctrl
    {
      get { return m_ctrl; }
      set
      {
        m_ctrl = value;
        // copy props needed
        m_MasterTree.Font = m_ctrl.Font; // assign font to master tree as well
        m_MasterTree.ImageList = m_ctrl.ImageList;

        // define some const fonts for further use
        FontActionmap = new Font( m_ctrl.Font, FontStyle.Bold );
        FontAction = new Font( m_ctrl.Font, FontStyle.Regular );
        FontActionActivated = new Font( m_ctrl.Font, FontStyle.Underline );
      }
    }


    private Boolean m_showJoy = true;
    private Boolean m_showGameP = true;
    private Boolean m_showKbd = true;
    private Boolean m_showMouse = true;
    private Boolean m_showMappedOnly = false;


    /// <summary>
    /// maintains the change status (gets reset by reloading the complete tree)
    /// </summary>
    public Boolean Dirty { get; set; }

    /// <summary>
    /// a comma separated list of actionmaps to ignore
    /// </summary>
    public String IgnoreMaps { get; set; }

    private String  m_Filter = ""; // the tree content filter
    private JoystickList m_jsList = null;
    private GamepadCls   m_gamepad = null;

    /// <summary>
    /// ctor
    /// </summary>
    public ActionTree( JoystickList jsList, GamepadCls gamepad )
    {
      m_jsList = jsList;
      m_gamepad = gamepad;

      IgnoreMaps = ""; // nothing to ignore
    }


    /// <summary>
    /// Copy return the complete ActionTree while reassigning JsN
    /// </summary>
    /// <param name="newJsList">The JsN reassign list</param>
    /// <returns>The ActionTree Copy with reassigned input</returns>
    public ActionTree ReassignJsN( JsReassingList newJsList )
    {
      ActionTree nTree = new ActionTree( m_jsList, m_gamepad );
      // full copy from 'this'
      nTree.m_MasterTree = this.m_MasterTree;
      nTree.m_ctrl = this.m_ctrl;
      nTree.IgnoreMaps = this.IgnoreMaps;
      nTree.m_Filter = this.m_Filter;

      nTree.ActionMaps = this.ActionMaps.ReassignJsN( newJsList );

      nTree.Dirty = true;
      return nTree;
    }

    #region Properties

    public Boolean CanAssignBinding
    {
      get
      {
        if ( Ctrl.SelectedNode == null ) return false;
        else return ( Ctrl.SelectedNode.Level == 1 ) || ( Ctrl.SelectedNode.Level == 2 );
      }
    }

    public Boolean CanBlendBinding
    {
      get
      {
        if ( Ctrl.SelectedNode == null ) return false;
        else return ( Ctrl.SelectedNode.Level == 1 );
      }
    }

    public Boolean CanClearBinding
    {
      get
      {
        if ( Ctrl.SelectedNode == null ) return false;
        else return ( Ctrl.SelectedNode.Level == 1 ) && IsMappedAction;
      }
    }


    public Boolean CanAddBinding
    {
      get
      {
        if ( Ctrl.SelectedNode == null ) return false;
        else return ( Ctrl.SelectedNode.Level == 1 ) && IsMappedAction;
      }
    }

    public Boolean CanDelBinding
    {
      get
      {
        if ( Ctrl.SelectedNode == null ) return false;
        else return ( Ctrl.SelectedNode.Level == 2 );
      }
    }

    public Boolean IsMappedAction
    {
      get
      {
        if ( ( Ctrl.SelectedNode.Level == 0 ) || ( Ctrl.SelectedNode.Level > 2 ) ) return false; // not on node
        if ( Ctrl.SelectedNode == null ) return false; // no node selected
        if ( Ctrl.SelectedNode.Parent == null ) return false; // ERROR EXIT

        return ( Ctrl.SelectedNode as ActionTreeNode ).IsMappedAction;
      }
    }


    public Boolean ShowAction( ActionCls.ActionDevice actDev, string input )
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
    public void AddBinding( )
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
      // jump to the latest
      if ( Ctrl.SelectedNode.LastNode != null ) {
        Ctrl.SelectedNode = Ctrl.SelectedNode.LastNode;
      }
    }


    /// <summary>
    /// Delete the selected ActionChild and remove corresponding ActionCommands
    /// </summary>
    public void DelBinding( )
    {
      if ( Ctrl.SelectedNode == null ) return;
      if ( Ctrl.SelectedNode.Level != 2 ) return; // can only delete level 2 nodes

      ActionTreeNode matn = FindMasterAction( ( ActionTreeNode )Ctrl.SelectedNode.Parent ); // the parent treenode
      ActionCls ac = FindActionObject( matn.Parent.Name, matn.Name );   // the related action
      // delete items
      ac.DelCommand( Ctrl.SelectedNode.Index );
      matn.Nodes.RemoveAt( Ctrl.SelectedNode.Index );
      Dirty = true;
      // show stuff
      FilterTree( );
      FindAndSelectCtrlByName( matn.Name );
    }


    public void BlendBinding( )
    {
      UpdateSelectedItem( DeviceCls.BlendedInput, ActionCls.ActionDevice.AD_Unknown, false );
    }

    public void ClearBinding( )
    {
      UpdateSelectedItem( "", ActionCls.ActionDevice.AD_Unknown, false );
    }

    /// <summary>
    /// Assign the GUI Invert Checkboxes for further handling
    /// </summary>
    public List<CheckBox> InvertCheckList
    {
      set
      {
        if ( ActionMaps != null ) ActionMaps.InvertCheckList = value; // just propagate if possible
        else {
          log.Error( "ActionTree-InvertCheckList: Program error - ActionMaps not yet created" );
        }
      }
    }


    /// <summary>
    /// Gets the JS device that is used for one of the Inversion Items supported
    /// </summary>
    /// <param name="item">The Inversion item</param>
    /// <returns>The device used or null</returns>
    private DeviceCls GetActionInstance( OptionsInvert.Inversions item )
    {
      // must get the jsN information used for Options
      String nodeText = "";
      nodeText = FindAction( OptionsInvert.MappedActions[( int )item].Map, OptionsInvert.MappedActions[( int )item].Action );
      if ( !String.IsNullOrWhiteSpace( nodeText ) ) {
        DeviceCls dev = m_jsList.Find_jsN( JoystickCls.JSNum( ActionTreeNode.CommandFromNodeText( nodeText ) ) );
        if ( dev != null ) return dev;
        else {
          // could be a gamepad then
          if ( ActionTreeNode.CommandFromNodeText( nodeText ).Contains( "xi_" ) ) {
            return m_gamepad;
          }
          else return null; // nope...
        }
      }
      return null;
    }

    /// <summary>
    /// Collects and forwards the device information (instances) to the consuming invert Options
    /// </summary>
    private void UpdateDeviceInformation( )
    {
      // must get the jsN information used for Options Inverters
      for ( int item = 0; item < ( int )OptionsInvert.Inversions.I_LAST; item++ ) {
        ActionMaps.Options.Inverter( ( OptionsInvert.Inversions )item ).GameDevice = GetActionInstance( ( OptionsInvert.Inversions )item );
      }
    }


    /// <summary>
    /// Dumps the actions to an XML String
    /// </summary>
    /// <returns>A string containing the XML</returns>
    public String toXML( String fileName )
    {
      if ( ActionMaps != null ) {
        // must update the devices and instances for inversion before dumping the XML
        UpdateDeviceInformation( );
        return ActionMaps.toXML( fileName ); // just propagate if possible
      }
      else {
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
    private void ApplyFilter( )
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
        Boolean allHidden = true;
        foreach ( ActionTreeNode stn in tn.Nodes ) {
          if ( ( stn.Tag != null ) && ( ( Boolean )stn.Tag == true ) ) {
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
    }


    /// <summary>
    /// Filters the master tree  - only Actions (level 1) and not actionmaps (level 0)
    ///   - Tag gets Bool hidden=true if not to be shown
    /// </summary>
    private void FilterTree( )
    {
      Boolean hidden = !String.IsNullOrEmpty( m_Filter ); // hide only if there is a find string
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
    public void FilterTree( String filter )
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
    public void LoadProfileTree( String defaultProfileName, Boolean applyDefaults )
    {
      log.Debug( "LoadProfileTree - Entry" );

      ActionTreeNode tn = null;
      ActionTreeNode[] cnl = { };
      ActionTreeNode cn = null;
      ActionTreeNode topNode = null;

      ActionMapCls acm = null;
      ActionCls ac = null;
      ActionCommandCls acc = null;

      ActionMaps = new ActionMapsCls( m_jsList );
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
        String buf = sr.ReadLine( );
        while ( !String.IsNullOrEmpty( buf ) ) {
          String[] elem = buf.Split( new char[] { ';', ',' }, StringSplitOptions.None );
          if ( elem.Length > 1 ) {
            if ( !IgnoreMaps.Contains( "," + elem[0] + "," ) ) {
              // must have 2 elements min
              Array.Resize( ref cnl, 0 );
              acm = new ActionMapCls( ); acm.name = elem[0]; // get actionmap name
              // process items
              for ( int ei = 1; ei < elem.Length; ei += 4 ) { // step 2  - action;defaultBinding;defaultActivationMode;defMultiTap come in as quadrupples
                if ( !String.IsNullOrEmpty( elem[ei] ) ) {
                  // default assignments
                  String action = elem[ei].Substring( 1 );
                  String defBinding = elem[ei + 1];
                  string defActivationModeName = elem[ei + 2];
                  int defMultiTap = int.Parse( elem[ei + 3] );
                  // need to create a ActivationMode here
                  ActivationMode defActivationMode = new ActivationMode( defActivationModeName, defMultiTap );

                  String devID = elem[ei].Substring( 0, 1 );
                  String device = ActionCls.DeviceClassFromTag( devID );

                  // visual item for the action
                  cn = new ActionTreeNode( "UNDEF" ); cn.Name = elem[ei]; cn.Action = action; cn.BackColor = Color.White;  // name with the key it to find it..                
                  cn.ImageKey = devID; cn.BackColor = Color.White; // some stuff does not work properly...
                  if ( ActivationMode.IsDefault( defActivationModeName ) ) {
                    cn.NodeFont = FontAction;
                  }
                  else {
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
                  }
                  else if ( ac.actionDevice == ActionCls.ActionDevice.AD_Gamepad ) {
                    acc.DevID = GamepadCls.DeviceID;
                    if ( applyDefaults ) {
                      if ( !String.IsNullOrEmpty( ac.defBinding ) ) {
                        acc.DevInput = ac.defBinding;
                        cn.Command = ac.defBinding; cn.BackColor = GamepadCls.XiColor( );
                      }
                    }
                  }
                  else if ( ac.actionDevice == ActionCls.ActionDevice.AD_Keyboard ) {
                    acc.DevID = KeyboardCls.DeviceID;
                    if ( applyDefaults ) {
                      if ( !String.IsNullOrEmpty( ac.defBinding ) ) {
                        acc.DevInput = ac.defBinding;
                        cn.Command = ac.defBinding; cn.BackColor = KeyboardCls.KbdColor( );
                      }
                    }
                  }
                  else if ( ac.actionDevice == ActionCls.ActionDevice.AD_Mouse ) {  // 20151220BM: add mouse device (from AC 2.0 defaultProfile usage)
                    acc.DevID = MouseCls.DeviceID;
                    if ( applyDefaults ) {
                      if ( !String.IsNullOrEmpty( ac.defBinding ) ) {
                        acc.DevInput = ac.defBinding;
                        cn.Command = ac.defBinding; cn.BackColor = MouseCls.MouseColor( );
                      }
                    }
                  }
                }
              }//for

              tn = new ActionTreeNode( acm.name, cnl ); tn.Name = acm.name;  tn.Action = acm.name; // name it to find it..
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
    public ActivationModes ActivationModeSelectedItem( )
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
      }
      else if ( Ctrl.SelectedNode.Level == 2 ) {
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
        }
        else {
          acc.ActivationMode = ActivationModes.Instance.ActivationModeByName( newActivationModeName );
        }
        atn.UpdateAction( acc ); UpdateMasterNode( atn );
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
        Dirty = true;
      }
    }


    // input is like  js1_button3 OR keyboard such as lctrl+x (mouse is keyboard too)
    /// <summary>
    /// Apply an update the the treenode
    /// First apply to the GUI tree where the selection happend then copy it over to master tree
    /// </summary>
    /// <param name="input">The new Text property</param>
    public Boolean UpdateSelectedItem( String input, ActionCls.ActionDevice inKind, Boolean checkKind )
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
        Dirty = true;
      }
      else if ( Ctrl.SelectedNode.Level == 2 ) {
        // this is a child of an action with further commands
        ActionTreeNode patn = ( Ctrl.SelectedNode.Parent as ActionTreeNode );  // the parent treenode from a level 2
        ActionTreeNode atn = ( Ctrl.SelectedNode as ActionTreeNode );  // the treenode from a level 2
        ActionCls ac = FindActionObject( patn.Parent.Name, patn.Name );   // the related action in an actionmap
        if ( ac == null ) return false; // ERROR exit
        if ( checkKind && ( ac.actionDevice != inKind ) ) return false; // ERROR exit
        ActionCommandCls acc = ac.FindActionInputObject( atn.Index );
        if ( acc == null ) return false; // ERROR exit
        // have it - continue
        ac.UpdateCommandFromInput( ActionCls.DevInput( input, inKind ), acc.NodeIndex + 1 );
        atn.UpdateAction( acc ); UpdateMasterNode( atn );
        Dirty = true;
      }
      return true;
    }

    /// <summary>
    /// Find an action with name in a actionmap
    /// </summary>
    /// <param name="actionMap">The actionmap name</param>
    /// <param name="action">The action</param>
    /// <param name="devInput">The input</param>
    /// <returns>An actionCommand or null if not found</returns>
    private ActionCommandCls FindActionInputObject( String actionMap, String action, String devInput )
    {
      log.Debug( "FindActionInputObject - Entry" );
      // Apply the input to the ActionTree
      ActionCls ac = null; ActionCommandCls acc = null;
      ActionMapCls ACM = ActionMaps.Find( delegate( ActionMapCls _ACM ) { return _ACM.name == actionMap; } );
      if ( ACM != null ) ac = ACM.Find( delegate ( ActionCls _AC ) { return _AC.key == action; } );
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
    /// <param name="action">The action</param>
    /// <param name="input">The input</param>
    /// <returns>An actionCommand or null if not found</returns>
    private ActionCommandCls FindActionInputObject( String actionMap, String action, int index )
    {
      log.Debug( "FindActionInputObject - Entry" );
      // Apply the input to the ActionTree
      ActionCls ac = null; ActionCommandCls acc = null;
      ActionMapCls ACM = ActionMaps.Find( delegate( ActionMapCls _ACM ) { return _ACM.name == actionMap; } );
      if ( ACM != null ) ac = ACM.Find( delegate ( ActionCls _AC ) { return _AC.key == action; } );
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
    private ActionCls FindActionObject( String actionMap, String action )
    {
      log.Debug( "FindActionObject - Entry" );
      // Apply the input to the ActionTree
      ActionCls ac = null;
      ActionMapCls ACM = ActionMaps.Find( delegate( ActionMapCls acm ) { return acm.name == actionMap; } );
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
    public void DefineShowOptions( Boolean showJoystick, Boolean showGamepad, Boolean showKeyboard, Boolean showMouse, Boolean showMappedOnly )
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
    public void ReloadTreeView( )
    {
      log.Debug( "ReloadTreeView - Entry" );

      foreach ( ActionMapCls acm in ActionMaps ) {
        if ( IgnoreMaps.Contains( "," + acm.name + "," ) ) break; // next
        try {
          ActionTreeNode mtn = ( ActionTreeNode )m_MasterTree.Nodes[acm.name]; // get the map node
          // find the item to reload into the treeview
          foreach ( ActionCls ac in acm ) {
            ActionTreeNode matn = ( ActionTreeNode )mtn.Nodes[ac.key];  // get the action node
            Boolean first=true;
            // refresh commands
            foreach ( ActionCommandCls acc in ac.inputList ) {
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
                  ActionTreeInputNode matin = new ActionTreeInputNode( "UNDEF" ); matin.ImageKey = "Add";
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
        catch {
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
    public void FindAndSelectCtrl( String ctrl )
    {
      log.Debug( "FindAndSelectCtrl - Entry" );

      foreach ( ActionTreeNode tn in Ctrl.Nodes ) {
        // have to search nodes of nodes
        foreach ( ActionTreeNode stn in tn.Nodes ) {
          if ( stn.Text.Contains( ctrl ) ) {
            Ctrl.SelectedNode = stn;
            Ctrl.SelectedNode.EnsureVisible( );
            return; // exit all loops
          }
          // have to search nodes of nodes
          foreach ( ActionTreeInputNode sstn in stn.Nodes ) {
            if ( sstn.Text.Contains( ctrl ) ) {
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
    public void FindAndSelectCtrlByName( String ctrlName )
    {
      log.Debug( "FindAndSelectCtrlByName - Entry" );

      foreach ( ActionTreeNode tn in Ctrl.Nodes ) {
        // have to search nodes of nodes
        foreach ( ActionTreeNode stn in tn.Nodes ) {
          if ( stn.Name == ctrlName ) {
            Ctrl.SelectedNode = stn;
            Ctrl.SelectedNode.EnsureVisible( );
            return; // exit all loops
          }
          // have to search nodes of nodes
          foreach ( ActionTreeInputNode sstn in stn.Nodes ) {
            if ( sstn.Name == ctrlName ) {
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
    public String FindAction( String actionmap, String action )
    {
      log.Debug( "FindAction - Entry" );

      foreach ( ActionTreeNode tn in Ctrl.Nodes ) {
        if ( String.IsNullOrEmpty( actionmap ) || ( tn.Text == actionmap ) ) {
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
    public String FindAction( String action )
    {
      return FindAction( "", action );
    }


    /// <summary>
    /// Find a control that contains the Command
    /// </summary>
    /// <param name="actionmap">The actionmap to find the string</param>
    /// <param name="text">The string to find</param>
    public String FindCommand( String actionmap, String command )
    {
      log.Debug( "FindCommand - Entry" );

      foreach ( ActionTreeNode tn in Ctrl.Nodes ) {
        if ( String.IsNullOrEmpty( actionmap ) || ( tn.Text == actionmap ) ) {
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
    public String FindCommand( String command )
    {
      return FindCommand( "", command );
    }


    /// <summary>
    /// Find a control the the actionmap that contains the Text
    /// </summary>
    /// <param name="actionmap">The actionmap to find the string</param>
    /// <param name="text">The string to find</param>
    public String FindText( String actionmap, String text )
    {
      log.Debug( "FindText - Entry" );

      foreach ( ActionTreeNode tn in Ctrl.Nodes ) {
        if ( String.IsNullOrEmpty( actionmap ) || ( tn.Text == actionmap ) ) {
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
    public String FindText( String text )
    {
      return FindText( "", text );
    }


    /// <summary>
    /// Returns the Action name of the selected item
    /// </summary>
    public String SelectedAction
    {
      get
      {
        if ( Ctrl.SelectedNode == null ) return "";
        if ( Ctrl.SelectedNode.Level == 1 ) {
          ActionTreeNode matn = FindMasterAction( ( ActionTreeNode )Ctrl.SelectedNode );
          return ActionTreeNode.ActionFromNodeText( matn.Text );
        }
        else if ( Ctrl.SelectedNode.Level == 2 ) {
          ActionTreeNode matn = FindMasterAction( ( ActionTreeNode )Ctrl.SelectedNode.Parent ); // the parent treenode
          return ActionTreeNode.ActionFromNodeText( matn.Text );
        }
        else return "";
      }
    }


    /// <summary>
    /// Reports a summary list of the mapped items
    /// </summary>
    /// <returns></returns>
    public String ReportActions( )
    {
      log.Debug( "FindCtrl - ReportActions" );

      String repList = "";
      // JS assignments
      for ( int i = 0; i < JoystickCls.JSnum_MAX; i++ ) {
        if ( !String.IsNullOrEmpty( ActionMaps.jsN[i] ) ) repList += String.Format( "** js{0} = {1}\n", i + 1, ActionMaps.jsN[i] );
      }
      // now the mapped actions
      const int padAction = 42;
      const int padDevice = 4;
      const int padInput = 25;

      repList += String.Format( "\n" );
      repList += String.Format( " {0}+- {1} _ {2}#-[{4}] {3}\n\n", "Action".PadRight( padAction ), "Dev".PadRight( padDevice ), "Binding".PadRight( padInput ), "Activation", "T" ); // col description line

      foreach ( ActionMapCls acm in ActionMaps ) {
        String rep = String.Format( "*** {0}\n", acm.name );
        repList += rep;
        foreach ( ActionCls ac in acm ) {
          foreach ( ActionCommandCls acc in ac.inputList ) {
            if ( ShowAction( ac.actionDevice, acc.Input ) ) {
              if ( !String.IsNullOrEmpty( acc.Input ) /* && !( acc.Input == DeviceCls.BlendedInput )*/ ) {
                if ( acc.DevInput == ac.defBinding ) {
                  rep = String.Format( " {0} . {1} _ {2}", ac.name.PadRight( padAction ), acc.DevID.PadRight( padDevice ), acc.Input.PadRight( padInput ) );
                }
                else {
                  rep = String.Format( " {0} + {1} _ {2}", ac.name.PadRight( padAction ), acc.DevID.PadRight( padDevice ), acc.Input.PadRight( padInput ) ); // my binding
                }
                if ( acc.ActivationMode == ActivationMode.Default ) {
                  rep += String.Format( " . [{1}] {0}\n", ac.defActivationMode.Name, ac.defActivationMode.MultiTap );
                }
                else {
                  rep += String.Format( " # [{1}] {0}\n", acc.ActivationMode.Name, acc.ActivationMode.MultiTap );
                }

                repList += rep;
              }
            }
          }
        }
        repList += String.Format( "\n" ); // actionmap spacer
      }
      return repList;
    }


  }
}
