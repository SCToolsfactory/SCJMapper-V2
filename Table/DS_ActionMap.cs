using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCJMapper_V2.Table
{
  class DS_ActionMap
  {
    static public string ActionMapShown( string actionMap, int amCount )
    {
      string amShown = string.Format("{0,2:00}-{1}", amCount, actionMap);
      return amShown;
    }

    static public string ActionMapFromAMShown( string actionMapShown )
    {
      string ret = actionMapShown.Substring(3); // must match the above formatting
      return ret;
    }



    static public string ActionID( string actionMap, string acKey, int actionCommandIndex )
    {
      string acID = string.Format("{0}.{1}.{2}", actionMap, acKey, actionCommandIndex ); // make a unique key
      return acID;
    }




    static public string ActionMap( string actionID )
    {
      string[] e = actionID.Split( new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
      // we have actionmap.actionkey.nodeindex
      string actionKey = e[0];
      return actionKey;
    }

    static public string ActionMap( DS_ActionMaps.T_ActionRow acr )
    {
      return ActionMap( acr.ID_Action );
    }



    /// <summary>
    ///  decompose the Action from the ActionID string
    ///    matches the composing ActionIDShown
    /// </summary>
    /// <param name="actionID"></param>
    /// <returns></returns>
    static public string Action( string actionID )
    {
      string[] e =actionID.Split( new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
      // we have actionmap.actionkey.nodeindex
      string actionKey = e[1];
      return actionKey.Substring( 1 );
    }

    static public string Action( DS_ActionMaps.T_ActionRow acr )
    {
      return Action( acr.ID_Action );
    }



    static public string ActionKey( string actionID )
    {
      string[] e = actionID.Split( new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
      // we have actionmap.actionkey.nodeindex
      string actionKey = e[1];
      return actionKey;
    }

    static public string ActionKey( DS_ActionMaps.T_ActionRow acr )
    {
      return ActionKey( acr.ID_Action );
    }



    static public int ActionCommandIndex( string actionID )
    {
      string[] e =actionID.Split( new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
      // we have actionmap.actionkey.nodeindex
      int actionCommandIndex = int.Parse( e[2] );
      return actionCommandIndex;
    }

    static public int ActionCommandIndex( DS_ActionMaps.T_ActionRow acr )
    {
      return ActionCommandIndex( acr.ID_Action );
    }



    static public string DevInput ( DS_ActionMaps.T_ActionRow acr )
    {
      if ( acr.Disabled )
        return DeviceCls.BlendedInput;
      else
        return acr.Usr_Binding;
    }

  }
}
