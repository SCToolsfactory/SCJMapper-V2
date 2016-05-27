using System;
using OpenTK;
using System.Collections.Generic;

namespace SCJMapper_V2.OGL
{
  public abstract class CalcCurve
  {

    protected List<Vector2> m_points;
    protected int m_resolution;

    /// <summary>
    /// Constructs a new OpenTK.BezierCurve.
    /// </summary>
    /// <param name="points">The points.</param>
    protected CalcCurve( IEnumerable<Vector2> points, int resolution )
    {
      if ( points == null )
        throw new ArgumentNullException( "points", "Must point to a valid list of Vector2 structures." );

      this.m_points = new List<Vector2>( points );
      this.m_resolution = resolution;
    }

    /// <summary>
    /// Gets the points of this curve.
    /// </summary>
    public IList<Vector2> Points
    {
      get { return m_points; }
    }

    /// <summary>
    /// Calculates the point with the specified t.
    /// </summary>
    /// <param name="t">The t value, between 0.0f and 1.0f.</param>
    /// <returns>Resulting point.</returns>
    abstract public Vector2 CalculatePoint( float t );

  }


  /// <summary>
  /// BezierCurve from OpenTK  (Wrapper)
  /// 
  /// </summary>
  public class Bezier : CalcCurve
  {

    private BezierCurve m_curve;

    /// <summary>
    /// Constructs a new OpenTK.BezierCurve.
    /// </summary>
    /// <param name="points">The points.</param>
    public Bezier( IEnumerable<Vector2> points, int resolution )
      : base( points, resolution )
    {
      m_curve = new BezierCurve( m_points );
      // resolution is not used here
    }


    /// <summary>
    /// Calculates the point with the specified t.
    /// </summary>
    /// <param name="t">The t value, between 0.0f and 1.0f.</param>
    /// <returns>Resulting point.</returns>
    override public Vector2 CalculatePoint( float t )
    {
      return m_curve.CalculatePoint( t );
    }


  }


  /// <summary>
  /// Interpolating cubic B-spline
  /// 
  /// See http://www.ibiblio.org/e-notes/Splines/b-int.html
  /// code http://www.ibiblio.org/e-notes/Splines/b-interpolate.js
  /// </summary>
  public class BezierInterpolation : CalcCurve
  {
    private int nPts = 2;
    private int n1Pts = 3;
    private int segments = 0;
    private int segPts = 26;

    private double[] B0;
    private double[] B1;
    private double[] B2;
    private double[] B3;

    private Vector2[] d; // tangents
    private Vector2[] cout; // out array

    /// <summary>
    /// Constructs a new OpenTK.BezierCurve.
    /// </summary>
    /// <param name="points">The points.</param>
    public BezierInterpolation( IEnumerable<Vector2> points, int resolution )
      : base( points, resolution )
    {
      // tangets via user points
      nPts = m_points.Count - 2;
      // tangents not by user
      nPts = m_points.Count;

      n1Pts = nPts + 1;

      segments = nPts - 1;            // number of curve segments
      segPts = (int)Math.Ceiling( ( double )resolution / ( double )segments );   // pts per curve segment @ resolution

      d = new Vector2[m_points.Count];
      cout = new Vector2[segments * segPts];

      // init table 0.0 .. 1.0 with 0.04 inc
      B0 = new double[segPts];
      B1 = new double[segPts];
      B2 = new double[segPts];
      B3 = new double[segPts];


      double t = 0; double increment = 1.0 / ( segPts - 1 );
      for ( int i= 0; i < segPts; i++ ) {
        double t1 = 1.0 - t;
        double t12 = t1 * t1;
        double t2 = t * t;

        B0[i] = t1 * t12; B1[i] = 3.0 * t * t12; B2[i] = 3.0 * t2 * t1; B3[i] = t * t2;

        t += increment;
      }

      findCPoints( );
      CalcCurve( );
    }


    private void findCPoints( )
    {
      /*
      // tangets via user points
      d[0].X = m_points[nPts].X - m_points[0].X;
      d[0].Y = m_points[nPts].Y - m_points[0].Y;
      
      d[nPts - 1].X = -( m_points[n1Pts].X - m_points[nPts - 1].X );
      d[nPts - 1].Y = -( m_points[n1Pts].Y - m_points[nPts - 1].Y );
      */
      // tangents not by user
      d[0].X = ( m_points[1].X - m_points[0].X ) / 3f;
      d[0].Y = ( m_points[1].Y - m_points[0].Y ) / 3f;
      d[nPts - 1].X = ( m_points[nPts - 1].X - m_points[nPts - 2].X ) / 3f;
      d[nPts - 1].Y = ( m_points[nPts - 1].Y - m_points[nPts - 2].Y ) / 3f;

      Vector2[] A = new Vector2[m_points.Count];
      float[] Bi = new float[m_points.Count];

      Bi[1] = -0.25f;
      A[1].X = ( m_points[2].X - m_points[0].X - d[0].X ) / 4f;
      A[1].Y = ( m_points[2].Y - m_points[0].Y - d[0].Y ) / 4f;

      for ( int i = 2; i < nPts - 1; i++ ) {
        Bi[i] = -1.0f / ( 4.0f + Bi[i - 1] );
        A[i].X = -( m_points[i + 1].X - m_points[i - 1].X - A[i - 1].X ) * Bi[i];
        A[i].Y = -( m_points[i + 1].Y - m_points[i - 1].Y - A[i - 1].Y ) * Bi[i];
      }
      for ( var i = nPts - 2; i > 0; i-- ) {
        d[i].X = A[i].X + d[i + 1].X * Bi[i];
        d[i].Y = A[i].Y + d[i + 1].Y * Bi[i];
      }
    }

    private void CalcCurve( )
    {
      // curve segments
      int segIdx = 0;
      for ( int i = 0; i < segments; i++ ) {
        // resolution per segment
        for ( int k = 0; k < segPts; k++ ) {
          cout[segIdx].X = ( float )( m_points[i].X * B0[k] + ( m_points[i].X + d[i].X ) * B1[k] + ( m_points[i + 1].X - d[i + 1].X ) * B2[k] + m_points[i + 1].X * B3[k] );
          cout[segIdx].Y = ( float )( m_points[i].Y * B0[k] + ( m_points[i].Y + d[i].Y ) * B1[k] + ( m_points[i + 1].Y - d[i + 1].Y ) * B2[k] + m_points[i + 1].Y * B3[k] );
          segIdx++;
        }
      }
    }

    /// <summary>
    /// Calculates the point with the specified t.
    /// </summary>
    /// <param name="t">The t value, between 0.0f and 1.0f.</param>
    /// <returns>Resulting point.</returns>
    override public Vector2 CalculatePoint( float t )
    {
      int pt = ( int )(t * m_resolution); // get an index within resolution of the out array
      // sanity checks
      pt = ( pt < 0 ) ? 0 : pt;
      pt = ( pt >= m_resolution ) ? m_resolution - 1 : pt;

      return cout[pt];
    }
  }


}

