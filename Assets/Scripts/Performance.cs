using UnityEngine;
using TMPro;

public class Performance : MonoBehaviour {
    [SerializeField]
    TextMeshProUGUI display;

    [SerializeField, Range(0.1f, 2f)]
    float sampleDuration = 1f; // calculate average FPS over this sample duration

    int frames;
    float duration, bestDuration = float.MaxValue, worstDuration; // set bestDuration to worstest possible

    public enum DisplayMode { FPS, MS } // Display frame-per-second or frame duration in ms
    public DisplayMode mode = DisplayMode.FPS;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        float frameDuration = Time.unscaledDeltaTime; // cannot use Time.deltaTime to measure since it may impact by game mechanism
        frames += 1;
        duration += frameDuration;
        if (frameDuration < bestDuration) {
            bestDuration = frameDuration;
        }
        if (frameDuration > worstDuration) {
            worstDuration = frameDuration;
        }
        if (duration >= sampleDuration) {
            if (mode == DisplayMode.FPS) {
                display.SetText("FPS: {0:0}\nBest: {1:0}\nWorst: {2:0}",
                frames / duration, 1f / bestDuration, 1f / worstDuration);
            } else {
                display.SetText("Duration: {0:1}ms\nBest: {1:1}ms\nWorst: {2:1}ms",
                    1000f * duration / frames, 1000f * bestDuration, 1000f * worstDuration);
            }
            frames = 0;
            duration = 0f;
            bestDuration = float.MaxValue;
            worstDuration = 0f;
        }
    }
}
