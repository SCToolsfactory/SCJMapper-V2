using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms.DataVisualization.Charting;
using System.Windows.Forms;
using OpenTK;

namespace SCJMapper_V2.OGL
{
  public class BezierPointCollection : List<DataPoint>
  {

  }


  /// <span class="code-SummaryComment"><summary>
  /// You can use this Series like any other series in the MS web charting control
  /// http://www.codeproject.com/Articles/169962/Bezier-curve-for-the-Microsoft-Web-Chart-control
  /// <span class="code-SummaryComment"></summary>
  public class BezierSeries : Series
  {
    #region fields

    private int _pointsOnCurve = 50;
    private BezierPointCollection _bezierPoints = new BezierPointCollection();

    #endregion

    #region properties

    /// <span class="code-SummaryComment"><summary>
    /// Defines how many points the resulting curve will have;
    /// min = 2, must be an even number
    /// <span class="code-SummaryComment"></summary>
    public int PointsOnCurve
    {
      get { return _pointsOnCurve; }
      set
      {
        //min value is 2
        if ( value < 2 ) {
          value = 2;
        }

        //it must be an even number
        if ( value % 2 == 1 ) {
          value++;
        }

        _pointsOnCurve = value;
      }
    }

    /// <span class="code-SummaryComment"><summary>
    /// Points that should be used to calculate the bezier graph
    /// <span class="code-SummaryComment"></summary>
    public BezierPointCollection BezierPoints
    {
      get
      {
        return _bezierPoints;
      }
      set
      {
        this.Points.Clear( );
        if ( value != null ) {
          _bezierPoints = value;

          float[] cout = new float[PointsOnCurve*2];
          CalcCurve( cout );
          //bezier curve points
          this.ChartType = SeriesChartType.Line;

          for ( int i = 0; i < cout.Length; i = i + 2 ) {
            this.Points.AddXY( cout[i], cout[i + 1] );
          }
        }
      }
    }

    public void Invalidate( Control owner)
    {
      this.Points.Clear( );

      float[] cout = new float[PointsOnCurve*2];
      CalcCurve( cout );
      //bezier curve points
      this.ChartType = SeriesChartType.Line;

      for ( int i = 0; i < cout.Length; i = i + 2 ) {
        this.Points.AddXY( cout[i], cout[i + 1] );
      }

      owner.Invalidate( );
    }


    private void CalcCurve( float[] cout )
    {
      // fix issue #26 don't calc if we have no _bezierPoints
      if ( _bezierPoints.Count == 0 ) return; // the input as initialized

      Vector2[] ptList = new Vector2[_bezierPoints.Count];
      //convert bezier points to flat list
      int pIdx = 0;
      foreach ( DataPoint point in _bezierPoints ) {
        ptList[pIdx++] = new Vector2( ( float )point.XValue, ( float )point.YValues[0] );
      }

      //bezier curve calculation            
      int resolution = PointsOnCurve - 1;
      CalcCurve bc = new BezierInterpolation( ptList, PointsOnCurve );

      pIdx = 0;
      Vector2 pos = Vector2.One;
      for ( int p = 0; p <= resolution; p++ ) {
        pos = bc.CalculatePoint( ( float )p / ( float )resolution );
        cout[p * 2] = pos.X; cout[p * 2 + 1] = pos.Y;
      }
    }


    #endregion

  }
}
