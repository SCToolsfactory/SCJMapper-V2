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
    /// Blend (insert jsx_reserved") into undmapped items
    /// </summary>
    public Boolean BlendUnmapped { get; set; }

    /// <summary>
    /// a comma separated list of actionmaps to ignore
    /// </summary>
    public String IgnoreMaps { get; set; }

    private String  m_Filter = ""; // the tree content filter


    /// <summary>
    /// ctor
    /// </summary>
    public ActionTree( Boolean blendUnmapped )
    {
      BlendUnmapped = blendUnmapped;
      IgnoreMaps = ""; // nothing to ignore
    }


    /// <summary>
    /// Copy return the complete ActionTree while reassigning JsN
    /// </summary>
    /// <param name="newJsList">The JsN reassign list</param>
    /// <returns>The ActionTree Copy with reassigned input</returns>
    public ActionTree ReassignJsN( Dictionary<int, int> newJsList )
    {
      ActionTree nTree = new ActionTree( BlendUnmapped );
      // full copy from 'this'
      nTree.m_MasterTree = this.m_MasterTree;
      nTree.m_ctrl = this.m_ctrl;
      nTree.IgnoreMaps = this.IgnoreMaps;
      nTree.m_Filter = this.m_Filter;

      nTree.ActionMaps = this.ActionMaps.ReassignJsN( newJsList );

      nTree.Dirty = true;
      return nTree;
    }

    /// <summary>
    /// Instantiates a copy of the node - copies only the needed properties
    /// </summary>
    /// <param name="srcNode">A source node</param>
    /// <returns>A new TreeNode</returns>
    private TreeNode TNCopy( TreeNode srcNode )
    {
      if ( srcNode == null ) return null;

      TreeNode nn = new TreeNode( );
      nn.Name = srcNode.Name;
      nn.Text = srcNode.Text;
      nn.BackColor = srcNode.BackColor;
      nn.ForeColor = srcNode.ForeColor;
      nn.NodeFont = srcNode.NodeFont;
      nn.ImageKey = srcNode.ImageKey;
      return nn;
    }


    /// <summary>
    /// Apply the filter to the GUI TreeView
    /// </summary>
    private void ApplyFilter( )
    {
      log.Debug( "ApplyFilter - Entry" );

      TreeNode topNode = null; // allow to backup the view - will carry the first node items

      Ctrl.BeginUpdate( );
      Ctrl.Nodes.Clear( ); // start over

      // traverse the master tree and build the GUI tree from it
      foreach ( TreeNode tn in m_MasterTree.Nodes ) {
        TreeNode tnMap = TNCopy( tn ); Ctrl.Nodes.Add( tnMap ); // copy level 0 nodes
        if ( topNode == null ) topNode = tnMap;

        // have to search nodes of nodes
        Boolean allHidden = true;
        foreach ( TreeNode stn in tn.Nodes ) {
          if ( ( stn.Tag != null ) && ( ( Boolean )stn.Tag == true ) ) {
            ;  // don't create it i.e hide it - though you cannot hide TreeViewNodes at all...
          }
          else {
            TreeNode tnAction = TNCopy( stn ); tnMap.Nodes.Add( tnAction ); // copy level 1 nodes
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
      foreach ( TreeNode tn in m_MasterTree.Nodes ) {
        // have to search nodes of nodes
        foreach ( TreeNode stn in tn.Nodes ) {
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

      TreeNode tn = null;
      TreeNode[] cnl = { };
      TreeNode cn = null;
      TreeNode topNode = null;

      ActionCls ac = null;
      ActionMapCls acm = null;

      ActionMaps = new ActionMapsCls( );
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
                  String action = elem[ei].Substring( 1 );
                  String defBinding = elem[ei + 1].Substring( 0 );
                  cn = new TreeNode( action ); cn.Name = elem[ei]; cn.BackColor = Color.White; // name with the key it to find it..                
                  String devID = elem[ei].Substring( 0, 1 );
                  String device = ActionCls.DeviceFromID( devID );
                  cn.ImageKey = devID;
                  cn.BackColor = Color.White; // some stuff does not work properly...

                  Array.Resize( ref cnl, cnl.Length + 1 ); cnl[cnl.Length - 1] = cn;
                  ac = new ActionCls( ); ac.key = cn.Name; ac.name = action; ac.device = device; ac.defBinding = defBinding;
                  acm.Add( ac ); // add to our map


                  if ( applyDefaults ) {
                    // right now this application only works with joysticks
                    if ( JoystickCls.IsJoystick( ac.device ) ) {
                      int jNum = JoystickCls.JSNum( ac.defBinding );
                      if ( JoystickCls.IsJSValid( jNum ) ) {
                        ac.input = ac.defBinding;
                        cn.Text += " - " + ac.defBinding;
                        cn.BackColor = JoystickCls.JColor[jNum - 1]; // color list is 0 based
                      }
                      else {
                        if ( BlendUnmapped ) {
                          ac.input = JoystickCls.BlendedJsInput;
                          cn.Text += " - " + JoystickCls.BlendedJsInput;
                        }
                      }
                    }
                  }
                  else {
                    // init empty
                    if ( JoystickCls.IsJoystick( ac.device ) && BlendUnmapped ) {
                      ac.input = JoystickCls.BlendedJsInput;
                      cn.Text += " - " + JoystickCls.BlendedJsInput;
                    }
                  }
                }
              }//for

              tn = new TreeNode( acm.name, cnl ); tn.Name = acm.name;  // name it to find it..
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
      tn = new TreeNode( "DUMMY" ); tn.Name = "DUMMY";
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



    // input is like  js1_button3
    /// <summary>
    /// Apply an update the the treenode
    /// First apply to the GUI tree where the selection happend then copy it over to master tree
    /// </summary>
    /// <param name="input">The new Text property</param>
    public void UpdateSelectedItem( String input )
    {
      log.Debug( "UpdateSelectedItem - Entry" );

      if ( Ctrl.SelectedNode == null ) return;

      // Apply the input to the ActionTree
      ActionCls ac = null;
      ActionMapCls ACM = ActionMaps.Find( delegate( ActionMapCls acm ) { return acm.name == Ctrl.SelectedNode.Parent.Name; } );
      if ( ACM != null ) {
        ac = ACM.Find( delegate( ActionCls _AC ) { return _AC.key == Ctrl.SelectedNode.Name; } );
        if ( ac != null ) {
          if ( String.IsNullOrEmpty( input ) ) {
            // unmapped
            if ( JoystickCls.IsJoystick( ac.device ) && BlendUnmapped ) ac.input = JoystickCls.BlendedJsInput;
            else ac.input = "";
          }
          else {
            // mapped
            ac.input = input;
          }
          Dirty = true;
        }
        else {
          log.Error( "UpdateSelectedItem - Action Not found in tree" );
          return;  // ERROR - Action Not found in tree
        }
      }

      // applies only to ActionNodes
      if ( Ctrl.SelectedNode.Level == 1 ) {
        String[] elements = Ctrl.SelectedNode.Text.Split( );
        if ( String.IsNullOrEmpty( input ) ) {
          // unmapped
          if ( JoystickCls.IsJoystick( ac.device ) && BlendUnmapped ) Ctrl.SelectedNode.Text = elements[0] + " - " + JoystickCls.BlendedJsInput;
          else Ctrl.SelectedNode.Text = elements[0];
          Ctrl.SelectedNode.BackColor = Color.White;
        }
        else {
          // mapped
          Ctrl.SelectedNode.Text = elements[0] + " - " + input;
          int jNum = JoystickCls.JSNum( input );
          Ctrl.SelectedNode.BackColor = JoystickCls.JColor[jNum - 1]; // color list is 0 based
        }

        // copy to master node
        TreeNode[] masterNode = m_MasterTree.Nodes.Find( Ctrl.SelectedNode.Name, true ); // find the same node in master
        if ( masterNode.Length == 0 ) throw new IndexOutOfRangeException( "ActionTree ERROR - cannot find synched node in master" ); // OUT OF SYNC
        masterNode[0].Text = Ctrl.SelectedNode.Text;
        masterNode[0].BackColor = Ctrl.SelectedNode.BackColor;


      }
    }


    /// <summary>
    /// Loads the mappings back into the treeview control
    /// Note: this takes a while as the list grows...
    /// </summary>
    public void ReloadCtrl( )
    {
      log.Debug( "ReloadCtrl - Entry" );

      foreach ( ActionMapCls acm in ActionMaps ) {
        if ( IgnoreMaps.Contains( "," + acm.name + "," ) ) break; // next
        try {
          TreeNode amTn = m_MasterTree.Nodes[acm.name]; // get the map node
          // find the item to reload into the treeview
          foreach ( ActionCls ac in acm ) {
            try {
              TreeNode tnl = amTn.Nodes[ac.key];
              String[] elements = tnl.Text.Split( );
              if ( String.IsNullOrEmpty( ac.input ) || ( ac.input == JoystickCls.BlendedJsInput ) ) {
                // grabed input is not mapped
                if ( JoystickCls.IsJoystick( ac.device ) && BlendUnmapped ) {
                  ac.input = JoystickCls.BlendedJsInput;
                  tnl.Text = elements[0] + " - " + JoystickCls.BlendedJsInput;
                }
                else {
                  ac.input = "";
                  tnl.Text = elements[0];
                }
                tnl.BackColor = Color.White;
              }
              else {
                // mapped
                int jNum = JoystickCls.JSNum( ac.input );
                tnl.Text = elements[0] + " - " + ac.input;
                tnl.BackColor = JoystickCls.JColor[jNum - 1]; // color list is 0 based
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
      foreach ( TreeNode tn in Ctrl.Nodes ) {
        // have to search nodes of nodes
        foreach ( TreeNode stn in tn.Nodes ) {
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
    /// Reports a summary list of the mapped items
    /// </summary>
    /// <returns></returns>
    public String ReportActions( )
    {
      log.Debug( "FindCtrl - ReportActions" );

      String repList = "";
      // JS assignments
      for ( int i=0; i < JoystickCls.JSnum_MAX; i++ ) {
        if ( !String.IsNullOrEmpty( ActionMaps.jsN[i] ) ) repList += String.Format( "** js{0} = {1}\n", i+1, ActionMaps.jsN[i] );
      }
      // now the mapped actions
      repList += String.Format( "\n" );
      foreach ( ActionMapCls acm in ActionMaps ) {
        String rep = String.Format( "*** {0}\n", acm.name );
        repList += rep;
        foreach ( ActionCls ac in acm ) {
          if ( !String.IsNullOrEmpty( ac.input ) && !( ac.input == JoystickCls.BlendedJsInput ) ) {
            rep = String.Format( " {0} - {1} - ({2})\n", ac.name.PadRight( 35 ), ac.input.PadRight( 20 ), ac.device );
            repList += rep;
          }
        }
        repList += String.Format( "\n" );
      }
      return repList;
    }


  }
}
