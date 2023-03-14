using System.Collections;
using System.Collections.Generic;
using Unity.Barracuda;
using UnityEngine;

public class ModelAgent : MonoBehaviour
{
    public NNModel modelSource;
    private Model model;
    private IWorker worker;

    //[0, 5], [0, 5], [0, 2], [3, 5] = pictogram => predictedIndex = 0
    //[0, 5], [0, 1], [3, 5], [4, 5] = pictogramandtext => predictedIndex = 1
    //[4, 5], [0, 1], [0, 5], [0, 5] = static => predictedIndex = 2
    //[0, 5], [0, 2], [3, 5], [3, 5] = text => predictedIndex = 3
    public float repetitiveBehavior = 4.5f;
    public float sensorySensitivity = 0.5f;
    public float readingComprehension = 2.5f;
    public float structureNeed = 2.5f;
    private int typeIndex = -1;
    public bool IsInitialized { get; private set;}

    // Start is called before the first frame update
    void Start()
    {
        model = ModelLoader.Load(modelSource);
        worker = WorkerFactory.CreateWorker(WorkerFactory.Type.Compute, model);

        // Define the shape of the tensor
        int[] shape = new int[] { 1, 4 };

        // Define the data type of the tensor
        //DataType dtype = DataType.Float;

        // Generate the random data and create a new tensor
        float[] data = new float[4] { repetitiveBehavior, sensorySensitivity, readingComprehension, structureNeed };
        Tensor inputTensor = new Tensor(shape, data);

        // Run the model to get output
        worker.Execute(inputTensor);
        Tensor outputTensor = worker.PeekOutput();

        // Do something with the output tensor, such as print it to the console
        Debug.Log(outputTensor.ToString());

        int[] predictedIndex = TensorExtensions.ArgMax(outputTensor);

        for(int i = 0; i < predictedIndex.Length; i++){
            Debug.Log(predictedIndex[i]);
        }
        
        typeIndex = predictedIndex[0];

        IsInitialized = true;
        // Clean up resources
        inputTensor.Dispose();
        outputTensor.Dispose();
        worker.Dispose();
    }

    // Update is called once per frame
    void Update() { }

    public int getTypeIndex()
    {
        return typeIndex;
    }
}
