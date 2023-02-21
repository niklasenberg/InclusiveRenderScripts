using System.Collections;
using System.Collections.Generic;
using Unity.Barracuda;
using UnityEngine;

public class ModelAgent : MonoBehaviour
{
    public NNModel modelSource;
    private Model model;
    private IWorker worker;

    // Start is called before the first frame update
    void Start()
    {
        model = ModelLoader.Load(modelSource);
        worker = WorkerFactory.CreateWorker(WorkerFactory.Type.Compute, model);
    }

    // Update is called once per frame
    void Update(){}

    
}
