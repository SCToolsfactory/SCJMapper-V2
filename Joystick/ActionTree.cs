﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Drawing;
using SCJMapper_V2.Properties;

namespace SCJMapper_V2
{
  class ActionTree
  {

    private String m_Filter = "";

    public ActionMapsCls ActionMaps { get; set; }
    public TreeView Ctrl { get; set; }
    public Boolean Dirty { get; set; }


    private void ApplyFilter( )
    {
      TreeNode topNode = null;
      Ctrl.BeginUpdate( );
      foreach ( TreeNode tn in Ctrl.Nodes ) {
        if ( topNode == null ) topNode = tn;
        // have to search nodes of nodes
        Boolean allHidden = true;
        foreach ( TreeNode stn in tn.Nodes ) {
          if ( ( stn.Tag != null ) && ( ( Boolean )stn.Tag == true ) ) {
            // hide it - thoug you cannot hide TreeViewNodes at all...
            stn.ForeColor = stn.BackColor;
          }
          else {
            stn.ForeColor = Ctrl.ForeColor;
            allHidden = false;
          }
        }
        // make it tidier..
        if ( allHidden ) tn.Collapse( );
        else tn.Expand( );
      }
      if ( topNode != null ) Ctrl.TopNode = topNode;

      Ctrl.EndUpdate( );
    }

    /// <summary>
    /// Filters the tree 
    /// </summary>
    private void FilterTree( )
    {
      Boolean hidden = ! String.IsNullOrEmpty( m_Filter ); // hide only if there is a find string
      foreach ( TreeNode tn in Ctrl.Nodes ) {
        // have to search nodes of nodes
        foreach ( TreeNode stn in tn.Nodes ) {
          if ( !stn.Text.Contains( m_Filter ) ) stn.Tag = hidden;
          else stn.Tag = null;
        }
      }
      ApplyFilter( );
    }


    /// <summary>
    /// Filters entries with given criteria but not action maps
    /// </summary>
    /// <param name="filter">The text snip to filter</param>
    public void FilterTree( String filter )
    {
      m_Filter = filter;
      FilterTree( );
    }


    /// <summary>
    /// Load MappingVars.csv into the ActionList and create the Control TreeView 
    /// </summary>
    /// <param name="defaultProfileName">The name of the profile to load (w/o extension)</param>
    /// <param name="applyDefaults">True if default mappings should be carried on</param>
    public void LoadTree( String defaultProfileName, Boolean applyDefaults )
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

      dpReader.fromXML( SCDefaultProfile.DefaultProfile( defaultProfileName + ".xml" ) );
      if ( dpReader.ValidContent ) {
        txReader = new StringReader( dpReader.CSVMap );
      }

      Ctrl.BeginUpdate( );
      using ( TextReader sr = txReader ) {
        String buf = sr.ReadLine( );
        while ( !String.IsNullOrEmpty( buf ) ) {
          String[] elem = buf.Split( new char[] { ';', ',', ' ' } );
          if ( elem.Length > 1 ) {
            // must have 2 elements min
            Array.Resize( ref cnl, 0 );
            acm = new ActionMapCls( ); acm.name = elem[0]; // get actionmap name
            // process items
            for ( int ei=1; ei < elem.Length; ei += 2 ) { // step 2  - action;defaultBinding come in pairs
              if ( !String.IsNullOrEmpty( elem[ei] ) ) {
                String action = elem[ei].Substring( 1 );
                String defBinding = elem[ei + 1].Substring( 0 );
                cn = new TreeNode( action ); cn.Name = elem[ei];  // name with the key it to find it..
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
                      cn.BackColor = MyColors.JColor[jNum - 1]; // color list is 0 based
                    }
                  }
                }
              }
            }//for
            tn = new TreeNode( acm.name, cnl ); tn.Name = acm.name;  // name it to find it..
            tn.ImageIndex = 0; tn.NodeFont = new Font( Ctrl.Font, FontStyle.Bold );
            Ctrl.BackColor = Color.White; // fix for defect TreeView (cut off bold text)
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
      Ctrl.EndUpdate( );

      FilterTree( );
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

      FilterTree( );
    }

    /// <summary>
    /// Find a control that contains the string and make it visible
    /// </summary>
    /// <param name="ctrl">The string to find</param>
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


    /// <summary>
    /// Reports a summary list of the mapped items
    /// </summary>
    /// <returns></returns>
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
            rep = String.Format( " {0} - {1} - ({2})\n", ac.name.PadRight( 35 ), ac.input.PadRight( 20 ), ac.device);
            repList += rep;
          }
        }
        repList += String.Format( "\n" );
      }
      return repList;
    }


  }
}
