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
        m_MasterTree.Font = m_ctrl.Font;
        m_MasterTree.ImageList = m_ctrl.ImageList;
      }
    }

    /// <summary>
    /// maintains the change status (gets reset by reloading the complete tree)
    /// </summary>
    public Boolean Dirty { get; set; }

    /// <summary>
    /// Blend (insert jsx_reserved") into undmapped Joystick items
    /// </summary>
    public Boolean BlendUnmappedJS { get; set; }
    /// <summary>
    /// Blend (insert xi_reserved") into undmapped Gamepad items
    /// </summary>
    public Boolean BlendUnmappedGP { get; set; }

    /// <summary>
    /// a comma separated list of actionmaps to ignore
    /// </summary>
    public String IgnoreMaps { get; set; }

    private String  m_Filter = ""; // the tree content filter
    private JoystickList m_jsList = null;

    /// <summary>
    /// ctor
    /// </summary>
    public ActionTree( Boolean blendUnmappedJS, Boolean blendUnmappedGP, JoystickList jsList )
    {
      BlendUnmappedJS = blendUnmappedJS;
      BlendUnmappedGP = blendUnmappedGP;

      m_jsList = jsList;

      IgnoreMaps = ""; // nothing to ignore
    }


    /// <summary>
    /// Copy return the complete ActionTree while reassigning JsN
    /// </summary>
    /// <param name="newJsList">The JsN reassign list</param>
    /// <returns>The ActionTree Copy with reassigned input</returns>
    public ActionTree ReassignJsN( Dictionary<int, int> newJsList )
    {
      ActionTree nTree = new ActionTree( BlendUnmappedJS, BlendUnmappedGP, m_jsList );
      // full copy from 'this'
      nTree.m_MasterTree = this.m_MasterTree;
      nTree.m_ctrl = this.m_ctrl;
      nTree.IgnoreMaps = this.IgnoreMaps;
      nTree.m_Filter = this.m_Filter;

      nTree.ActionMaps = this.ActionMaps.ReassignJsN( newJsList );

      nTree.Dirty = true;
      return nTree;
    }


    private void UpdateMasterNode( ActionTreeNode node )
    {
      // copy to master node
      TreeNode[] masterNode = m_MasterTree.Nodes.Find( node.Name, true ); // find the same node in master
      if ( masterNode.Length == 0 ) throw new IndexOutOfRangeException( "ActionTree ERROR - cannot find synched node in master" ); // OUT OF SYNC
      // could return more than one if the action is the same in different actionmaps
      foreach ( ActionTreeNode mtn in masterNode ) {
        if ( mtn.Parent.Name == node.Parent.Name ) {
          mtn.Command = node.Command; mtn.BackColor = node.BackColor;
        }
      }
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
            allHidden = false;
          }
        }
        // make it tidier..
        if ( allHidden ) tnMap.Collapse( );
        else tnMap.Expand( );
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
          if ( !stn.Text.Contains( m_Filter ) ) stn.Tag = hidden;
          else stn.Tag = null;
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


    /// <summary>
    /// Load Mappings into the ActionList and create the Master TreeView 
    /// </summary>
    /// <param name="defaultProfileName">The name of the profile to load (w/o extension)</param>
    /// <param name="applyDefaults">True if default mappings should be carried on</param>
    public void LoadTree( String defaultProfileName, Boolean applyDefaults )
    {
      log.Debug( "LoadTree - Entry" );

      ActionTreeNode tn = null;
      ActionTreeNode[] cnl = { };
      ActionTreeNode cn = null;
      ActionTreeNode topNode = null;

      ActionCls ac = null;
      ActionMapCls acm = null;

      ActionMaps = new ActionMapsCls( m_jsList );
      m_MasterTree.Nodes.Clear( );


      // read the action items into the TreeView
      DProfileReader dpReader = new DProfileReader( ); // we may read a profile
      TextReader txReader = null;

      dpReader.fromXML( SCDefaultProfile.DefaultProfile( defaultProfileName + ".xml" ) );
      if ( dpReader.ValidContent ) {
        txReader = new StringReader( dpReader.CSVMap );
      }

      using ( TextReader sr = txReader ) {
        String buf = sr.ReadLine( );
        while ( !String.IsNullOrEmpty( buf ) ) {
          String[] elem = buf.Split( new char[] { ';', ',', ' ' } );
          if ( elem.Length > 1 ) {
            if ( !IgnoreMaps.Contains( "," + elem[0] + "," ) ) {
              // must have 2 elements min
              Array.Resize( ref cnl, 0 );
              acm = new ActionMapCls( ); acm.name = elem[0]; // get actionmap name
              // process items
              for ( int ei=1; ei < elem.Length; ei += 2 ) { // step 2  - action;defaultBinding come in pairs
                if ( !String.IsNullOrEmpty( elem[ei] ) ) {
                  // default assignments
                  String action = elem[ei].Substring( 1 );
                  String defBinding = elem[ei + 1].Substring( 0 );
                  cn = new ActionTreeNode( "UNDEF" ); cn.Name = elem[ei]; cn.Action = action; cn.BackColor = Color.White; // name with the key it to find it..                
                  String devID = elem[ei].Substring( 0, 1 );
                  String device = ActionCls.DeviceFromID( devID );
                  cn.ImageKey = devID;
                  cn.BackColor = Color.White; // some stuff does not work properly...

                  Array.Resize( ref cnl, cnl.Length + 1 ); cnl[cnl.Length - 1] = cn;
                  ac = new ActionCls( ); ac.key = cn.Name; ac.name = action; ac.device = device; ac.defBinding = defBinding;
                  acm.Add( ac ); // add to our map

                  if ( applyDefaults ) {
                    // apply the default mappings
                    if ( JoystickCls.IsDeviceClass( ac.device ) ) {
                      int jNum = JoystickCls.JSNum( ac.defBinding );
                      if ( JoystickCls.IsJSValid( jNum ) ) {
                        ac.input = ac.defBinding;
                        cn.Command = ac.defBinding; cn.BackColor = JoystickCls.JsNColor( jNum );
                      }
                      else if ( BlendUnmappedJS ) {
                        // jsx_reserved gets here
                        ac.input = JoystickCls.BlendedInput;
                        cn.Command = JoystickCls.BlendedInput; cn.BackColor = MyColors.BlendedColor;
                      }
                    }
                    else if ( GamepadCls.IsDeviceClass( ac.device ) ) {
                      if ( GamepadCls.IsXiValid( ac.defBinding ) ) {
                        ac.input = ac.defBinding;
                        cn.Command = ac.defBinding; cn.BackColor = GamepadCls.XiColor( );
                      }
                      else if ( BlendUnmappedGP ) {
                        // xi_reserved gets here
                        ac.input = GamepadCls.BlendedInput;
                        cn.Command = GamepadCls.BlendedInput; cn.BackColor = MyColors.BlendedColor;
                      }
                    }
                    else if ( KeyboardCls.IsDeviceClass( ac.device ) ) {
                      if ( !String.IsNullOrEmpty( ac.defBinding ) ) {
                        ac.input = ac.defBinding;
                        cn.Command = ac.defBinding; cn.BackColor = KeyboardCls.KbdColor( );
                      }
                    }
                  }
                  // Don't apply defaults - but blend if checked
                  else {
                    // init empty
                    if ( JoystickCls.IsDeviceClass( ac.device ) && BlendUnmappedJS ) {
                      ac.input = JoystickCls.BlendedInput;
                      cn.Command = JoystickCls.BlendedInput; cn.BackColor = MyColors.BlendedColor;
                    }
                    else if ( GamepadCls.IsDeviceClass( ac.device ) && BlendUnmappedGP ) {
                      ac.input = GamepadCls.BlendedInput;
                      cn.Command = GamepadCls.BlendedInput; cn.BackColor = MyColors.BlendedColor;
                    }
                  }
                }
              }//for

              tn = new ActionTreeNode( acm.name, cnl ); tn.Name = acm.name;  // name it to find it..
              tn.ImageIndex = 0; tn.NodeFont = new Font( m_MasterTree.Font, FontStyle.Bold );
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
      tn.ImageIndex = 0; tn.NodeFont = new Font( m_MasterTree.Font, FontStyle.Bold );
      m_MasterTree.BackColor = m_MasterTree.BackColor; // fix for defect TreeView (cut off bold text)
      m_MasterTree.Nodes.Add( tn ); // add to control
      m_MasterTree.Nodes.RemoveByKey( "DUMMY" );
      // fix for defect TreeView (cut off bold text)

      txReader = null;
      Dirty = false;

      // finally apply the filter and make it visible
      FilterTree( );
    }


    // input is like  js1_button3 OR keyboard such as lctrl+x (mouse is keyboard too)
    /// <summary>
    /// Apply an update the the treenode
    /// First apply to the GUI tree where the selection happend then copy it over to master tree
    /// </summary>
    /// <param name="input">The new Text property</param>
    public void UpdateSelectedItem( String input, DeviceCls.InputKind inKind )
    {
      log.Debug( "UpdateSelectedItem - Entry" );

      if ( Ctrl.SelectedNode == null ) return;

      ActionCls ac = FindAction( Ctrl.SelectedNode.Parent.Name, Ctrl.SelectedNode.Name );
      UpdateActionFromInput( input, ac );
      UpdateNodeFromAction((ActionTreeNode)Ctrl.SelectedNode, ac, inKind );
    }

    /// <summary>
    /// Find an action with name in a actionmap
    /// </summary>
    /// <param name="actionMap">The actionmap name</param>
    /// <param name="action">The action</param>
    /// <returns>An action or null if not found</returns>
    private ActionCls FindAction( String actionMap, String action )
    {
      log.Debug( "UpdateAction - Entry" );
      // Apply the input to the ActionTree
      ActionCls ac = null;
      ActionMapCls ACM = ActionMaps.Find( delegate( ActionMapCls acm ) { return acm.name == actionMap; } );
      if ( ACM != null ) ac = ACM.Find( delegate( ActionCls _AC ) { return _AC.key == action; } );
      if ( ac == null ) {
        log.Error( "FindAction - Action Not found in tree" );
        return null;  // ERROR - Action Not found in tree
      }
      return ac;
    }


    /// <summary>
    /// Updates an action with a new input (command)
    /// </summary>
    /// <param name="input">The input command</param>
    /// <param name="action">The action to update</param>
    /// <param name="inKind">The input device</param>
    private void UpdateActionFromInput( String input, ActionCls action )
    {
      log.Debug( "UpdateAction - Entry" );
      if ( action == null ) return;

      // Apply the input to the ActionTree
      if ( String.IsNullOrEmpty( input ) ) {
        // unmapped - handle the blended ones from setting
        if ( JoystickCls.IsDeviceClass( action.device ) && BlendUnmappedJS ) action.input = JoystickCls.BlendedInput;
        else if ( GamepadCls.IsDeviceClass( action.device ) && BlendUnmappedGP ) action.input = GamepadCls.BlendedInput;
        else action.input = "";
      }
      else {
        // mapped ones
        action.input = input;
      }
      Dirty = true;
    }

    /// <summary>
    /// Apply an update from the action to the treenode
    /// First apply to the GUI tree where the selection happend then copy it over to master tree
    /// </summary>
    /// <param name="input">The input command</param>
    /// <param name="node">The TreeNode to update</param>
    /// <param name="action">The action that carries the update</param>
    /// <param name="inKind">The input device</param>
    private void UpdateNodeFromAction( ActionTreeNode node, ActionCls action, DeviceCls.InputKind inKind )
    {
      log.Debug( "UpdateNode - Entry" );
      if ( action == null ) return;

      // applies only to ActionNodes
      if ( node.Level == 1 ) {
        // input is either "" or a valid mapping or a blended mapping
        if ( String.IsNullOrEmpty( action.input ) ) {
          // new unmapped
          node.Command = ""; node.BackColor = MyColors.UnassignedColor;
        }
        // blended mapped ones - can only get a Blend Background
        else if ( JoystickCls.IsDeviceClass( action.device ) && ( action.input == JoystickCls.BlendedInput ) ) {
          node.Command = action.input; node.BackColor = MyColors.BlendedColor;
        }
        else if ( GamepadCls.IsDeviceClass( action.device ) && ( action.input == GamepadCls.BlendedInput ) ) {
          node.Command = action.input; node.BackColor = MyColors.BlendedColor;
        }
        else if ( action.input == DeviceCls.BlendedInput ) {
          // Manually Blended input
          node.Command = action.input;  node.BackColor = MyColors.BlendedColor;
        }
        else {
          // mapped ( regular ones )
          node.Command = action.input;
          // background is along the input 
          if ( inKind == DeviceCls.InputKind.Joystick ) {
            int jNum = JoystickCls.JSNum( action.input );
            node.BackColor = JoystickCls.JsNColor( jNum );
          }
          else if ( inKind == DeviceCls.InputKind.Gamepad ) {
            node.BackColor = GamepadCls.XiColor( );
          }
          else if ( inKind == DeviceCls.InputKind.Kbd ) {
            node.BackColor = KeyboardCls.KbdColor( );
          }
          else {
            // ?? what else
            node.BackColor = MyColors.UnassignedColor;
          }
        }
        UpdateMasterNode( node );

      }
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
          ActionTreeNode amTn = (ActionTreeNode)m_MasterTree.Nodes[acm.name]; // get the map node
          // find the item to reload into the treeview
          foreach ( ActionCls ac in acm ) {
            try {
              ActionTreeNode tnl = (ActionTreeNode)amTn.Nodes[ac.key];
              UpdateActionFromInput(ac.input, ac ); // this may apply (un)Blending if needed
              // input kind priority first
              if ( JoystickCls.IsJsN( ac.input ) ) {
                UpdateNodeFromAction( tnl, ac, DeviceCls.InputKind.Joystick );
              }
              else if ( GamepadCls.IsXiValid( ac.input ) ) {
                UpdateNodeFromAction( tnl, ac, DeviceCls.InputKind.Gamepad );
              }
              // device priority second
              else if ( JoystickCls.IsDeviceClass( ac.device ) ) {
                UpdateNodeFromAction( tnl, ac, DeviceCls.InputKind.Joystick );
              }
              else if ( GamepadCls.IsDeviceClass( ac.device ) ) {
                UpdateNodeFromAction( tnl, ac, DeviceCls.InputKind.Gamepad );
              }
              else if ( KeyboardCls.IsDeviceClass( ac.device ) ) {
                UpdateNodeFromAction( tnl, ac, DeviceCls.InputKind.Kbd );
              }
              else {
                // ?? defaults to unassigned color 
                UpdateNodeFromAction( tnl, ac, DeviceCls.InputKind.Other );
              }

            }
            catch {
              ; // key not found
            }
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
    public void FindCtrl( String ctrl )
    {
      log.Debug( "FindCtrl - Entry" );

      Boolean found = false;
      foreach ( ActionTreeNode tn in Ctrl.Nodes ) {
        // have to search nodes of nodes
        foreach ( ActionTreeNode stn in tn.Nodes ) {
          if ( stn.Text.Contains( ctrl ) ) {
            Ctrl.SelectedNode = stn;
            Ctrl.SelectedNode.EnsureVisible( );
            found = true;
            break;
          }
        }
        if ( found ) break; // exit all loops
      }
    }


    /// <summary>
    /// Find a control that contains the Command
    /// </summary>
    /// <param name="m_MasterTree">The string to find</param>
    public String FindCommand( String ctrl )
    {
      log.Debug( "FindCtrl - Entry" );

      foreach ( ActionTreeNode tn in Ctrl.Nodes ) {
        // have to search nodes of nodes
        foreach ( ActionTreeNode stn in tn.Nodes ) {
          if ( stn.Command.Contains( ctrl ) ) {
            return stn.Text;
          }
        }
      }
      return "";
    }


    /// <summary>
    /// Find a control that contains the Text
    /// </summary>
    /// <param name="m_MasterTree">The string to find</param>
    public String FindText( String text )
    {
      log.Debug( "FindText - Entry" );

      foreach ( ActionTreeNode tn in Ctrl.Nodes ) {
        // have to search nodes of nodes
        foreach ( ActionTreeNode stn in tn.Nodes ) {
          if ( stn.Text.Contains( text ) ) {
            return stn.Text;
          }
        }
      }
      return "";
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
      for ( int i=0; i < JoystickCls.JSnum_MAX; i++ ) {
        if ( !String.IsNullOrEmpty( ActionMaps.jsN[i] ) ) repList += String.Format( "** js{0} = {1}\n", i + 1, ActionMaps.jsN[i] );
      }
      // now the mapped actions
      repList += String.Format( "\n" );
      foreach ( ActionMapCls acm in ActionMaps ) {
        String rep = String.Format( "*** {0}\n", acm.name );
        repList += rep;
        foreach ( ActionCls ac in acm ) {
          if ( !String.IsNullOrEmpty( ac.input ) && !( ac.input == JoystickCls.BlendedInput ) ) {
            rep = String.Format( " {0} - {1} - ({2})\n", ac.name.PadRight( 35 ), ac.input.PadRight( 30 ), ac.device );
            repList += rep;
          }
        }
        repList += String.Format( "\n" );
      }
      return repList;
    }


  }
}
