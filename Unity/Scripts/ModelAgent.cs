using System.Collections;
using System.Collections.Generic;
using Unity.Barracuda;
using UnityEngine;

public class ModelAgent : MonoBehaviour
{
    public NNModel modelSource;
    public float feedbackStimulation = 2f;
    public float sensorySensitivity = 3f;
    public float readingComprehension = 2f;
    public float structureNeed = 4f;
    public bool IsInitialized { get; private set; }
    public int TypeIndex { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        Model model = ModelLoader.Load(modelSource);
        IWorker worker = WorkerFactory.CreateWorker(WorkerFactory.Type.Compute, model);

        // Define the shape of the tensor
        int[] shape = new int[] { 1, 4 };

        // Generate the random data and create a new tensor
        float[] data = new float[4] { feedbackStimulation, sensorySensitivity, readingComprehension, structureNeed };
        Tensor inputTensor = new Tensor(shape, data);

        // Run the model to get output
        worker.Execute(inputTensor);
        Tensor outputTensor = worker.PeekOutput();

        int[] predictedIndex = TensorExtensions.ArgMax(outputTensor);

        TypeIndex = predictedIndex[0];

        IsInitialized = true;
        // Clean up resources
        inputTensor.Dispose();
        outputTensor.Dispose();
        worker.Dispose();
    }

    // Update is called once per frame
    void Update() { }
}
