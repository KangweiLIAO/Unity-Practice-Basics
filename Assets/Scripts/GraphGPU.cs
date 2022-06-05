using UnityEngine;

public class GPUGraph : MonoBehaviour
{
    const int maxResolution = 1000;

    [SerializeField, Range(10, maxResolution)]
    int resolution = 50;

    [SerializeField]
    ComputeShader computeShader;

    [SerializeField]
    Material material;

    [SerializeField]
    Mesh mesh;

    ComputeBuffer positionsBuffer; // used to store positions in GPU

    // obtains identifiers that Unity uses for the properties in computeShader:
    static readonly int
        positionsId = Shader.PropertyToID("_Positions"),
        resolutionId = Shader.PropertyToID("_Resolution"),
        stepId = Shader.PropertyToID("_Step"),
        timeId = Shader.PropertyToID("_Time");

    // OnEnable is called once the game object bind with this script been enabled
    void OnEnable()
    { // hot reload compatibility
        // use compute buffer to store 3D position vectors, which consist of 3 float numbers:
        positionsBuffer = new ComputeBuffer(maxResolution * maxResolution, 3 * 4); // the element size is 3 * 4 bytes
    }

    // Update is called once per frame
    void Update()
    {
        UpdateFunctionOnGPU();
    }

    /// <summary>
    /// Calculates the step size and sets the resolution, step, and time properties of the compute shader
    /// </summary>
    void UpdateFunctionOnGPU()
    {
        float step = 2f / resolution;
        computeShader.SetInt(resolutionId, resolution);
        computeShader.SetFloat(stepId, step);
        computeShader.SetFloat(timeId, Time.time);
        computeShader.SetBuffer(0, positionsId, positionsBuffer); // links the buffer to the kernel 0 (since we only use one kernel)
        int groups = Mathf.CeilToInt(resolution / 8f); // since we have 8x8 group size, the amount of groups we need in the X and Y dimensions is equal to the resolution divided by eight
        computeShader.Dispatch(0, groups, groups, 1); // runs the kernel

        material.SetBuffer(positionsId, positionsBuffer);
        material.SetFloat(stepId, step);

        // var bounds = new Bounds(Vector3.zero, Vector3.one * 2f); // our bound of the graph is a 2x2 cube bound which center is the world origin
        var bounds = new Bounds(Vector3.zero, Vector3.one * (2f + 2f / resolution)); // since our points also have a size, need to increase the bound
        Graphics.DrawMeshInstancedProcedural(mesh, 0, material, bounds, positionsBuffer.count); // procedural drawing
    }


    // OnDisable is called once the game object bind with this script been disabled
    void OnDisable()
    {
        positionsBuffer.Release(); // let the GPU memory claimed by the buffer can be freed immediately
        // if our graph gets disabled or destroyed while in play mode:
        positionsBuffer = null; // makes it possible for the object to be reclaimed by memory GC process the next time it runs
    }
}
