using UnityEngine;
using static UnityEngine.Mathf;


public static class WaveFunctionLibrary {
    public delegate Vector3 WaveFunction(float u, float v, float t); // wave delegate function

    public enum WaveType {
        SineWave,
        MultiWave,
        Ripple,
        SineWaveZ,
        MultiWaveZ,
        RippleZ,
        Sphere,
        Torus
    };

    /// <summary>
    /// Return a corresponding wave function
    /// </summary>
    /// <param name="wt">Type of wave</param>
    /// <returns>Function that calculates the y values given u and t value</returns>
    public static WaveFunction GetWaveFunction(WaveType wt) {
        switch (wt) {
            case WaveType.MultiWave:
                return MultiWave;
            case WaveType.Ripple:
                return Ripple;
            case WaveType.SineWaveZ:
                return SineWave3D;
            case WaveType.MultiWaveZ:
                return MultiWave3D;
            case WaveType.RippleZ:
                return Ripple3D;
            case WaveType.Sphere:
                return Sphere;
            case WaveType.Torus:
                return Torus;
            default:
                return SineWave; ;
        }
    }

    public static WaveType GetNextWave(WaveType current) {
        return current < WaveType.Torus ? current + 1 : WaveType.SineWave;
    }

    /// <summary>
    /// Lerp from one wave function to another
    /// </summary>
    public static Vector3 Morph(float u, float v, float t, WaveFunction from, WaveFunction to, float progress) {
        // use Lerpunclamped() since SmoothStep() already clamp the value from 0-1
        return Vector3.LerpUnclamped(from(u, v, t), to(u, v, t), SmoothStep(0f, 1f, progress));
    }

    public static Vector3 SineWave(float u, float v, float t) {
        Vector3 position = new Vector3();
        position.x = u;
        position.y = Sin(PI * (u + t));
        position.z = v;
        return position;
    }

    public static Vector3 MultiWave(float u, float v, float t) {
        Vector3 position = new Vector3();
        position.x = u;
        position.y = Sin(PI * (u + t));
        position.y += 0.5f * Sin(2f * PI * (u + t));
        position.y *= (2f / 3f);
        position.z = v;
        return position;
    }

    public static Vector3 Ripple(float u, float v, float t) {
        Vector3 position = new Vector3();
        position.x = u;
        position.y = Sin(PI * (4f * Abs(u) + t));
        position.y /= (1f + 10f * Abs(u));
        position.z = v;
        return position;
    }

    public static Vector3 SineWave3D(float u, float v, float t) {
        Vector3 position = new Vector3();
        position.x = u;
        position.y = Sin(PI * (u + v + t));
        position.z = v;
        return position;
    }

    public static Vector3 MultiWave3D(float u, float v, float t) {
        Vector3 position = new Vector3();
        position.x = u;
        position.y = Sin(PI * (u + 0.5f * t));
        position.y += 0.5f * Sin(2f * PI * (v + t));
        position.y += Sin(PI * (u + v + 0.25f * t));
        position.y *= (1f / 2.5f);
        position.z = v;
        return position;
    }

    public static Vector3 Ripple3D(float u, float v, float t) {
        float d = Sqrt(u * u + v * v); // Pythagorean theorem
        Vector3 position = new Vector3();
        position.x = u;
        position.y = Sin(PI * (4f * d - t));
        position.y /= (1f + 10f * d);
        position.z = v;
        return position;
    }

    public static Vector3 Sphere(float u, float v, float t) {
        float r = 0.9f + 0.1f * Sin(PI * (6f * u + 4f * v + t));
        float s = r * Cos(0.5f * PI * v);
        Vector3 position;
        position.x = s * Sin(PI * u);
        position.y = r * Sin(0.5f * PI * v);
        position.z = s * Cos(PI * u);
        return position;
    }

    public static Vector3 Torus(float u, float v, float t) {
        float r1 = 0.7f + 0.1f * Sin(PI * (6f * u + 0.5f * t));
        float r2 = 0.15f + 0.05f * Sin(PI * (8f * u + 4f * v + 2f * t));
        float s = r1 + r2 * Cos(PI * v);
        Vector3 p;
        p.x = s * Sin(PI * u);
        p.y = r2 * Sin(PI * v);
        p.z = s * Cos(PI * u);
        return p;
    }
}
