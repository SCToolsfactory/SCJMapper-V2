using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Drawing;

namespace SCJMapper_V2
{
  class ActionTree
  {
    public ActionMapsCls ActionMaps { get; set; }
    public TreeView Ctrl { get; set; }
    public Boolean Dirty { get; set; }


    // Load MappingVars.csv into the ActionList and create the Control TreeView
    public void LoadTree( )
    {
      TreeNode tn = null;
      TreeNode[] cnl = { };
      TreeNode cn = null;
      TreeNode topNode = null;

      ActionCls ac = null;
      ActionMapCls acm = null;

      ActionMaps = new ActionMapsCls( );
      Ctrl.Nodes.Clear( );


      // read the action items into the TreeView
      DProfileReader dpReader = new DProfileReader( ); // we may read a profile
      TextReader txReader = null;

      dpReader.fromXML( SCDefaultProfile.DefaultProfile( ) );
      if ( dpReader.ValidContent ) {
        txReader = new StringReader( dpReader.CSVMap );
      }

      using ( TextReader sr = txReader ) {
        String buf = sr.ReadLine( );
        while ( !String.IsNullOrEmpty( buf ) ) {
          String[] elem = buf.Split( new char[] { ';', ',', ' ' } );
          if ( elem.Length > 1 ) {
            // must have 2 elements min
            Array.Resize( ref cnl, 0 );
            acm = new ActionMapCls( ); acm.name = elem[0]; // get actionmap name
            // process items
            for ( int ei=1; ei < elem.Length; ei++ ) {
              if ( !String.IsNullOrEmpty( elem[ei] ) ) {
                String action = elem[ei].Substring( 1 );
                cn = new TreeNode( action ); cn.Name = elem[ei];  // name with the key it to find it..
                String devID = elem[ei].Substring( 0, 1 );
                String device = ActionCls.DeviceFromID( devID );
                cn.ImageKey = devID;

                Array.Resize( ref cnl, cnl.Length + 1 ); cnl[cnl.Length - 1] = cn;
                ac = new ActionCls( ); ac.key = cn.Name; ac.name = action; ac.device = device;
                acm.Add( ac ); // add to our map
              }
            }//for
            tn = new TreeNode( acm.name, cnl ); tn.Name = acm.name;  // name it to find it..
            tn.ImageIndex = 0; tn.NodeFont = new Font( Ctrl.Font, FontStyle.Bold );
            Ctrl.BackColor = Ctrl.BackColor; // fix for defect TreeView (cut off bold text)
            Ctrl.Nodes.Add( tn ); // add to control
            if ( topNode == null ) topNode = tn; // once to keep the start of list
            ActionMaps.Add( acm ); // add to our map
          }// if valid line
          buf = sr.ReadLine( );
        }//while
      }
      // fix for defect TreeView (cut off bold text at last element -despite the BackColor fix) add another and delete it 
      tn = new TreeNode( "DUMMY" ); tn.Name = "DUMMY";
      tn.ImageIndex = 0; tn.NodeFont = new Font( Ctrl.Font, FontStyle.Bold );
      Ctrl.BackColor = Ctrl.BackColor; // fix for defect TreeView (cut off bold text)
      Ctrl.Nodes.Add( tn ); // add to control
      Ctrl.Nodes.RemoveByKey( "DUMMY" );
      // fix for defect TreeView (cut off bold text)

      txReader = null;

      Ctrl.ExpandAll( );
      if ( topNode != null ) Ctrl.TopNode = topNode;
      Dirty = false;
    }


    // input is like  js1_button3
    public void UpdateSelectedItem( String input )
    {
      if ( Ctrl.SelectedNode == null ) return;
      if ( Ctrl.SelectedNode.Level == 1 ) {
        String[] elements = Ctrl.SelectedNode.Text.Split( );
        if ( String.IsNullOrEmpty( input ) ) {
          Ctrl.SelectedNode.Text = elements[0];
          Ctrl.SelectedNode.BackColor = Color.White;
        }
        else {
          Ctrl.SelectedNode.Text = elements[0] + " - " + input;
          int jNum = JoystickCls.JSNum( input );
          Ctrl.SelectedNode.BackColor = MyColors.JColor[jNum - 1]; // color list is 0 based
        }
        ActionMapCls ACM = ActionMaps.Find( delegate( ActionMapCls acm ) {
          return acm.name == Ctrl.SelectedNode.Parent.Name;
        } );

        if ( ACM != null ) {
          ActionCls AC = ACM.Find( delegate( ActionCls ac ) {
            return ac.key == Ctrl.SelectedNode.Name;
          } );
          if ( AC != null ) {
            AC.input = input;
            Dirty = true;
          }
        }
      }
    }


    /// <summary>
    /// Loads the mappings back into the treeview control
    /// Note: this takes a while as the list grows...
    /// </summary>
    public void ReloadCtrl( )
    {
      Ctrl.BeginUpdate( );
      foreach ( ActionMapCls acm in ActionMaps ) {
        try {
          TreeNode amTn = Ctrl.Nodes[acm.name]; // get the map node
          // find the item to reload into the treeview
          foreach ( ActionCls ac in acm ) {
            try {
              TreeNode tnl = amTn.Nodes[ac.key];
              String[] elements = tnl.Text.Split( );
              if ( String.IsNullOrEmpty( ac.input ) ) {
                // grabed input is not mapped
                tnl.Text = elements[0];
                tnl.BackColor = Color.Transparent;
              }
              else {
                int jNum = JoystickCls.JSNum( ac.input );
                tnl.Text = elements[0] + " - " + ac.input;
                tnl.BackColor = MyColors.JColor[jNum - 1]; // color list is 0 based
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
      Ctrl.EndUpdate( );
    }

    public void FindCtrl( String ctrl )
    {
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


    public String ReportActions( )
    {
      String repList = "";

      if ( !String.IsNullOrEmpty( ActionMaps.js1 ) ) repList += String.Format( "** js1 = {0}\n", ActionMaps.js1 );
      if ( !String.IsNullOrEmpty( ActionMaps.js2 ) ) repList += String.Format( "** js2 = {0}\n", ActionMaps.js2 );
      if ( !String.IsNullOrEmpty( ActionMaps.js3 ) ) repList += String.Format( "** js3 = {0}\n", ActionMaps.js3 );
      if ( !String.IsNullOrEmpty( ActionMaps.js4 ) ) repList += String.Format( "** js4 = {0}\n", ActionMaps.js4 );
      if ( !String.IsNullOrEmpty( ActionMaps.js5 ) ) repList += String.Format( "** js5 = {0}\n", ActionMaps.js5 );
      if ( !String.IsNullOrEmpty( ActionMaps.js6 ) ) repList += String.Format( "** js6 = {0}\n", ActionMaps.js6 );
      if ( !String.IsNullOrEmpty( ActionMaps.js7 ) ) repList += String.Format( "** js7 = {0}\n", ActionMaps.js7 );
      if ( !String.IsNullOrEmpty( ActionMaps.js8 ) ) repList += String.Format( "** js8 = {0}\n", ActionMaps.js8 );
      repList += String.Format( "\n" );
      foreach ( ActionMapCls acm in ActionMaps ) {
        String rep = String.Format( "*** {0}\n", acm.name );
        repList += rep;
        foreach ( ActionCls ac in acm ) {
          if ( !String.IsNullOrEmpty( ac.input ) ) {
            rep = String.Format( " {0} - {1} - {2}\n", ac.name.PadRight( 35 ), ac.device.PadRight( 10 ), ac.input );
            repList += rep;
          }
        }
        repList += String.Format( "\n" );
      }
      return repList;
    }


  }
}
