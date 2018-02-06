using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCJMapper_V2.RTF
{
  /// <summary>
  /// A simple class to support RTF formatting for the RTF control
  /// </summary>
  class RTFformatter
  {
    private const string c_Header = @"\rtf1\ansi\ansicpg437\deff0\deflang9"; // plain ANSI with old IBM codepage, def font use 0, lang is Gen English
    private const string c_Leader = @"\viewkind4\uc1\b0\f0\cf0 "; // 4 Normal view, Unicode 1 byte (ANSI)

    #region Font

    private const int c_FontSize = 11;
    private string FontSize_low( int ptSize )
    {
      int ps = ( ptSize == 0 ) ? c_FontSize : ptSize;
      return @"\fs" + ( ps * 2 ).ToString( ).Trim( );
    }
    public void FontSize( int ptSize )
    {
      m_rTFtextLine += FontSize_low( ptSize );
    }

    private string m_fmtFont = @"\f0";
    private const string c_Fonts = @"{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset0 Courier New;}}"; // some fonts (Sans, Mono)
    public enum ERFont
    {
      ERF_Sans = 0,
      ERF_Mono,
    }

    private ERFont m_rFont = ERFont.ERF_Sans;
    public ERFont RFont
    {
      get { return m_rFont; }
      set {
        m_rFont = value;
        m_fmtFont = string.Format( @"\f{0} ", ( (int)m_rFont ).ToString( ).Trim( ) );
        m_rTFtextLine += m_fmtFont;
      }
    }

    private string m_fmtBold = @"\f0 ";
    private bool m_rBold = false;
    public bool RBold
    {
      get { return m_rBold; }
      set {
        m_rBold = value;
        m_fmtBold = ( m_rBold ) ? @"\b " : @"\b0 ";
        m_rTFtextLine += m_fmtBold;
      }
    }

    private string m_fmtULine = @"\ulnone ";
    private bool m_rULine = false;
    public bool RULine
    {
      get { return m_rULine; }
      set {
        m_rULine = value;
        m_fmtULine = ( m_rULine ) ? @"\ul " : @"\ulnone ";
        m_rTFtextLine += m_fmtULine;
      }
    }
    #endregion


    #region Text Color

    private const string c_Colors =
      @"{\colortbl ;\red255\green0\blue0;"      // Red
                + @"\red0\green176\blue80;"     // MidGreen
                + @"\red0\green77\blue187;"     // Blue
                + @"\red173\green255\blue47;"   // Green
                + @"\red0\green100\blue0;"      // DarkGreen
                + @"\red220\green220\blue220;"  // Gainsborow
                + "}"; 
    public enum ERColor
    {
      ERC_Black = 0,
      ERC_Red,
      ERC_MidGreen,
      ERC_Blue,
      ERC_Green,
      ERC_DarkGreen,
      ERC_Gainsborow,
    }

    private string m_fmtColor = @"\cf0";
    private ERColor m_rColor = ERColor.ERC_Black;
    public ERColor RColor
    {
      get { return m_rColor; }
      set {
        m_rColor = value;
        m_fmtColor = string.Format( @"\cf{0} ", ( (int)m_rColor ).ToString( ).Trim( ) );
        m_rTFtextLine += m_fmtColor;
      }
    }

    private string m_fmtHLight = @"\highlight0 ";
    private ERColor m_rHighlightColor = ERColor.ERC_Black;
    public ERColor RHighlightColor
    {
      get { return m_rHighlightColor; }
      set {
        m_rHighlightColor = value;
        m_fmtHLight = string.Format( @"\highlight{0} ", ( (int)m_rHighlightColor ).ToString( ).Trim( ) );
        m_rTFtextLine += m_fmtHLight;
      }
    }

    #endregion


    #region RTF text

    private const string c_NL = @"\par";
    private string LineFormatter( string input )
    {
      string ret = @"\pard\sl0\slmult1" + TabHeader + " "; // reset, line size determined by char, spacing 1

      return ret + input + c_NL;
    }

    private string m_rTFtextLine = "";
    private List<string> m_rTFtext = new List<string>( );
    public string RTFtext
    {
      get {
        // Build the stuff...
        string ret = "{" + string.Format( "{0}\n{1}\n{2}\n{3}\n", c_Header, c_Fonts, c_Colors, c_Leader );
        foreach ( string s in m_rTFtext )
          ret += string.Format( "{0}\n", s );
        ret += "}";
        return ret;
      }
    }
    #endregion


    #region Tabs

    private List<int> m_tabs = new List<int>( );
    private string TabHeader
    {
      get {
        string ret = "";
        foreach ( int i in m_tabs ) {
          ret += @"\tx" + i.ToString( );
        }
        return ret;
      }
    }

    public void ClearTabs()
    {
      m_tabs = new List<int>( );
    }

    /// <summary>
    /// Set a tab in twips (1/20pt) this is RTF style..
    /// </summary>
    /// <param name="tabPos">Tab position in twips</param>
    public void SetTab( int tabPos )
    {
      m_tabs.Add( tabPos );
    }
    #endregion


    #region Input

    public void Clear()
    {
      m_rTFtext.Clear( );
      m_rTFtextLine = "";
    }

    public void WriteTab( string input )
    {
      m_rTFtextLine += @"\tab " + input;
    }

    public void Write( string text )
    {
      m_rTFtextLine += text;
    }

    public void WriteLn( string text )
    {
      m_rTFtextLine += text;
      WriteLn( );
    }

    public void WriteLn()
    {
      m_rTFtext.Add( LineFormatter( m_rTFtextLine ) );
      m_rTFtextLine = "";
    }

    #endregion


  }
}
