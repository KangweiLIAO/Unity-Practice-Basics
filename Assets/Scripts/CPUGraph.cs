using UnityEngine;

public class CPUGraph : MonoBehaviour {
    [SerializeField]
    GameObject pointPrefab;
    [SerializeField, Range(10, 200)]
    int resolution = 50;
    [SerializeField, Min(0f)]
    float functionDuration = 1f, transitionDuration = 1f;

    public WaveFunctionLibrary.WaveType wave = WaveFunctionLibrary.WaveType.SineWave;

    Transform[] points;
    float duration; // record the time lapsed for a wave function
    private bool transitioning;
    WaveFunctionLibrary.WaveType transitionFunction;

    // Awake is called when the script's first loaded
    void Awake() {
        points = new Transform[resolution * resolution];
        for (int i = 0; i < points.Length; i++) {
            Transform point = points[i] = Instantiate(pointPrefab).transform; // instantiate a point & store in points[]
            point.SetParent(gameObject.transform, false); // why false?
        }
        transitioning = false;
    }

    // Update is called once per frame
    void Update() {
        duration += Time.deltaTime;  // deltaTime = the time last framed used
        if (transitioning) {
            UpdateWaveTransition();
        } else {
            WaveUpdate();
        }
        if (transitioning) {
            if (duration >= transitionDuration) {
                duration -= transitionDuration;
                transitioning = false;
            }
        } else if (duration >= functionDuration) {
            duration -= functionDuration;
            transitioning = true;
            transitionFunction = wave;
            wave = WaveFunctionLibrary.GetNextWave(wave);
        }
    }

    void WaveUpdate() {
        WaveFunctionLibrary.WaveFunction f = WaveFunctionLibrary.GetWaveFunction(wave); // delegate function
        float time = Time.time;
        float step = 2f / resolution;
        float v = 0.5f * step - 1f;
        Vector3 scale = Vector3.one * 2f / resolution;
        for (int i = 0, x = 0, z = 0; i < points.Length; i++, x++) {
            if (x == resolution) {
                x = 0;
                z += 1;
                v = (z + 0.5f) * step - 1f;
            }
            float u = (x + 0.5f) * step - 1f;
            points[i].localPosition = f(u, v, time); // using delegate
            points[i].localScale = scale;
        }
    }

    void UpdateWaveTransition() {
        WaveFunctionLibrary.WaveFunction
            from = WaveFunctionLibrary.GetWaveFunction(transitionFunction),
            to = WaveFunctionLibrary.GetWaveFunction(wave);
        float progress = duration / transitionDuration;
        float time = Time.time;
        float step = 2f / resolution;
        float v = 0.5f * step - 1f;
        Vector3 scale = Vector3.one * 2f / resolution;
        for (int i = 0, x = 0, z = 0; i < points.Length; i++, x++) {
            if (x == resolution) {
                x = 0;
                z += 1;
                v = (z + 0.5f) * step - 1f;
            }
            float u = (x + 0.5f) * step - 1f;
            points[i].localPosition = WaveFunctionLibrary.Morph(
                u, v, time, from, to, progress
            );
            points[i].localScale = scale;
        }
    }
}
