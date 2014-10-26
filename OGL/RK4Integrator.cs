using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;

namespace SCJMapper_V2
{
  /// <summary>
  /// 4th order Runge-Kutta integrator
  /// </summary>                           
  public class RK4Integrator
  {

    private StateModel m_currState;
    private const double _6th = 1.0/6.0;

    public struct StateModel
    {
      public Vector3d a;          // position
      public Vector3d v;          // velocity
    };

    private struct Derivative
    {
      public Vector3d da;          // derivative of position: velocity
      public Vector3d dv;          // derivative of velocity: acceleration
    };

    // ctor
    public RK4Integrator( )
    {
      m_currState.a = Vector3d.Zero;
      m_currState.v = Vector3d.Zero;
    }


    public StateModel State
    {
      get { return m_currState; }
      set { m_currState = value; }
    }
    public Vector3d Position
    {
      get { return m_currState.a; }
      set { m_currState.a = value; }
    }
    public Vector3d Velocity
    {
      get { return m_currState.v; }
      set { m_currState.v = value; }
    }


    private Vector3d Acceleration( StateModel state, double dampK, double dampB )
    {
      return (-dampK * state.a) - (dampB * state.v);
    }

    private Derivative Evaluate( StateModel initial, double dt, Derivative d, double dampK, double dampB )
    {
      StateModel state;
      state.a = initial.a + (d.da * dt);
      state.v = initial.v + (d.dv * dt);

      Derivative output = new Derivative( );
      output.da = state.v;
      output.dv = Acceleration( state, dampK, dampB );
      return output;
    }

    public Vector3d Integrate( double dt, double dampK, double dampB )
    {
      Derivative a = Evaluate( m_currState, 0.0, new Derivative( ), dampK, dampB );
      Derivative b = Evaluate( m_currState, dt * 0.5, a, dampK, dampB );
      Derivative c = Evaluate( m_currState, dt * 0.5, b, dampK, dampB );
      Derivative d = Evaluate( m_currState, dt, c, dampK, dampB );

      Vector3d dadt = _6th * ( a.da + (2.0 * ( b.da + c.da )) + d.da );
      Vector3d dvdt = _6th * ( a.dv + (2.0 * ( b.dv + c.dv )) + d.dv );

      m_currState.a += (dadt * dt);
      m_currState.v += (dvdt * dt);

      return ( dvdt * dt );
    }

  }
}
