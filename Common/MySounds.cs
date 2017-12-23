using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Media;

namespace SCJMapper_V2.Common
{
  public class MySounds
  {

    /*
      SystemSounds.Asterisk,
      SystemSounds.Beep,
      SystemSounds.Exclamation,
      SystemSounds.Hand,
      SystemSounds.Question
     * 
     *  Asterisk - the sound that is played when a popup alert is displayed, like a warning message.
        Default Beep - this sound is played for multiple reasons, depending on what you do. For example, it will play if you try to select a parent window before closing the active one.
        Exclamation - the sound that is played when you try to do something that is not supported by Windows.
      */

    public static void PlayNotfound( )
    {
      SystemSounds.Beep.Play( );
    }

    public static void PlayCannot( )
    {
      SystemSounds.Beep.Play( );
    }

    public static void PlayError( )
    {
      SystemSounds.Exclamation.Play( );
    }




  }
}
