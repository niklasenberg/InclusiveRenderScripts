using System.Collections;
using System.Collections.Generic;
using Unity.Barracuda;
using UnityEngine;

public class ModelAgent : MonoBehaviour
{
    public NNModel modelSource;
    public float feedbackStimulation = 4.5f;
    public float sensorySensitivity = 0.5f;
    public float readingComprehension = 2.5f;
    public float structureNeed = 2.5f;
    public bool IsInitialized { get; private set;}
    public int TypeIndex { get; private set;}

    // Start is called before the first frame update
    void Start()
    {
        // load model from source
        Model model = ModelLoader.Load(modelSource);
        IWorker worker = WorkerFactory.CreateWorker(WorkerFactory.Type.Compute, model);

        // define tensor shape
        int[] shape = new int[] { 1, 4 };

        // create new tensor from user input
        float[] data = new float[4] { feedbackStimulation, sensorySensitivity, readingComprehension, structureNeed };
        Tensor inputTensor = new Tensor(shape, data);

        // run inference
        worker.Execute(inputTensor);
        Tensor outputTensor = worker.PeekOutput();

        int[] predictedIndex = TensorExtensions.ArgMax(outputTensor);

        TypeIndex = predictedIndex[0];

        IsInitialized = true;

        //cleanup
        inputTensor.Dispose();
        outputTensor.Dispose();
        worker.Dispose();
    }

    void Update() { }
}
