using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OpenTK;

namespace SCJMapper_V2
{
  /// <summary>
  /// contains the x(in) and y(out) points of the nonlin curve for joysticks (MAX 3 interpolation pts)
  /// automatically adds zero/endpoints and goes symmetric around below 0
  /// </summary>
  public class xyPoints
  {
    private int m_maxpts = 10;

    private Vector2[] m_points = null;
    private double[] m_outCurve = null;
    private int m_Npoints = 0;
    private CalcCurve m_curve;

    public xyPoints( int maxPoints )
    {
      m_maxpts = maxPoints;
      m_outCurve = new double[m_maxpts];
    }

    private void Setup( int dimension )
    {
      if ( ( dimension < 1 ) || ( dimension > 3 ) ) return; // just ignore...
      m_Npoints = dimension + 2; // 3,4,5
      Array.Resize( ref m_points, m_Npoints );  // add endPts and zero
      Array.Resize( ref m_points, m_Npoints );  // add endPts and zero
      // not math just static assignment...
      int i=0;
      switch ( dimension ) {
        case 1: {
            m_points[i++] = new Vector2( 0f, 0f );
            m_points[i++] = new Vector2( 0.5f, 0.5f );
            m_points[i++] = new Vector2( 1.0f, 1.0f );
            break;
          }

        case 2: {
            m_points[i++] = new Vector2( 0f, 0f );
            m_points[i++] = new Vector2( 0.333333f, 0.333333f );
            m_points[i++] = new Vector2( 0.666667f, 0.666667f );
            m_points[i++] = new Vector2( 1.0f, 1.0f );
            break;
          }

        case 3: {
            m_points[i++] = new Vector2( 0f, 0f );
            m_points[i++] = new Vector2( 0.25f, 0.25f );
            m_points[i++] = new Vector2( 0.5f, 0.5f );
            m_points[i++] = new Vector2( 0.75f, 0.75f );
            m_points[i++] = new Vector2( 1.0f, 1.0f );
            break;
          }

        default: {
            // does not get here
            break;
          }
      }
    }


    private void GetOutput( float[] input )
    {
      // assuming it is continous...
      m_outCurve[0] = 0.0; // force Zero...
      int idx = 0;
      for ( int i = 1; i < m_maxpts; i++ ) {
        double sense = i / ( double )m_maxpts;
        while ( ( input[idx] < sense ) && ( idx < ( ( m_maxpts - 1 ) * 2 ) ) ) idx += 2;
        m_outCurve[i] = input[idx + 1];
        idx = ( idx < ( m_maxpts * 2 ) ) ? idx : ( m_maxpts - 1 ) * 2; // we shall not overrun...
      }
      m_outCurve[m_maxpts - 1] = 1.0; // force MAX
    }

    public void Curve( )
    {
      Setup( 1 );
      float[] cout = new float[m_maxpts * 2];
      int resolution = m_maxpts; // The number of points in the bezier curve
      m_curve = new BezierInterpolation( m_points, resolution );
      Vector2 pos = Vector2.One;
      for ( int p = 0; p <= resolution; p++ ) {
        pos = m_curve.CalculatePoint( ( float )p / ( float )resolution );
        cout[p * 2] = pos.X; cout[p * 2 + 1] = pos.Y; 
      }
      GetOutput( cout );
    }

    public void Curve( float x1, float y1 )
    {
      Setup( 1 );
      // zero
      int i = 1;
      m_points[i++] = new Vector2(x1, y1);

      float[] cout = new float[m_maxpts * 2];
      int resolution = m_maxpts; // The number of points in the bezier curve
      m_curve = new BezierInterpolation( m_points, resolution );
      Vector2 pos = Vector2.One;
      for ( int p = 0; p <= resolution; p++ ) {
        pos = m_curve.CalculatePoint( ( float )p / ( float )resolution );
        cout[p * 2] = pos.X; cout[p * 2 + 1] = pos.Y;
      }
      GetOutput( cout );
    }

    public void Curve( float x1, float y1, float x2, float y2 )
    {
      Setup( 2 );
      // zero
      int i = 1;
      m_points[i++] = new Vector2( x1, y1 );
      m_points[i++] = new Vector2( x2, y2 );

      float[] cout = new float[m_maxpts * 2];
      int resolution = m_maxpts; // The number of points in the bezier curve
      m_curve = new BezierInterpolation( m_points, resolution );
      Vector2 pos = Vector2.One;
      for ( int p = 0; p <= resolution; p++ ) {
        pos = m_curve.CalculatePoint( ( float )p / ( float )resolution );
        cout[p * 2] = pos.X; cout[p * 2 + 1] = pos.Y;
      }
      GetOutput( cout );
    }

    public void Curve( float x1, float y1, float x2, float y2, float x3, float y3 )
    {
      Setup( 3 );
      // zero
      int i = 1;
      m_points[i++] = new Vector2( x1, y1 );
      m_points[i++] = new Vector2( x2, y2 );
      m_points[i++] = new Vector2( x3, y3 );

      float[] cout = new float[m_maxpts * 2];
      int resolution = m_maxpts-1; // The number of points in the bezier curve
      m_curve = new BezierInterpolation( m_points, resolution );
      Vector2 pos = Vector2.One;
      for ( int p = 0; p <= resolution; p++ ) {
        pos = m_curve.CalculatePoint( ( float )p / ( float )resolution );
        cout[p * 2] = pos.X; cout[p * 2 + 1] = pos.Y;
      }
      GetOutput( cout );
    }


    public double EvalX( int atX )
    {
      int sng = Math.Sign( atX );
      int x = Math.Abs( atX );
      if ( x < m_maxpts ) return m_outCurve[x] * sng;

      // if out of rng return MAX
      return m_outCurve[m_maxpts-1] * sng;
    }



  }
}
