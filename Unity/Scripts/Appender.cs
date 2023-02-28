using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Appender : MonoBehaviour
{
    //Script logic
    private float maxRenderDistance = 5.0f;
    private GameObject player;
    private GameObject[] taggedObjects;
    private List<GameObject> appendedObjects;
    private GameObject predictedObject;
    private bool staticRendered;

    //User defined
    public string TAG;
    public bool mirrored;
    public float xOffset;
    public float yOffset;
    public float zOffset;
    public GameObject pictoObject;
    public GameObject pictoAndTextObject;
    public GameObject staticObject;
    public GameObject textObject;

    // Start is called before the first frame update
    void Start()
    {
        taggedObjects = GameObject.FindGameObjectsWithTag(TAG);
        player = GameObject.FindGameObjectWithTag("Player");
        appendedObjects = new List<GameObject>();

        predictedObject = getPredictedObject();

        foreach (GameObject taggedObject in taggedObjects){
            AppendObject(taggedObject, xOffset, yOffset);
        }
    }

    GameObject getPredictedObject(){
        int index = -1;
        new WaitUntil(() => GameObject.FindGameObjectWithTag("ModelAgent").GetComponent<ModelAgent>().IsInitialized);
        index = GameObject.FindGameObjectWithTag("ModelAgent").GetComponent<ModelAgent>().getTypeIndex();
        switch (index)
        {
            case 0: 
            Debug.Log("pictogram");
            return pictoObject;
                case 1: 
                    Debug.Log("pictogram and text");
                    return pictoAndTextObject;
                case 2: 
                    Debug.Log("static");
                    staticRendered = true;
                    return staticObject;
                case 3: 
                    Debug.Log("text");
                    return textObject;
        }
        Debug.Log("no prediced object yet");
        return null;
    }

    void AppendObject(GameObject taggedObject, float xOffset, float yOffset){
        GameObject newObject = Instantiate(predictedObject, taggedObject.transform);
        newObject.transform.localPosition = new Vector3(xOffset, yOffset, zOffset);
        newObject.transform.localRotation = Quaternion.identity;
        appendedObjects.Add(newObject);

        if(mirrored){
        GameObject newObject2 = Instantiate(predictedObject, taggedObject.transform);
        newObject2.transform.localPosition = new Vector3(-xOffset, yOffset, zOffset);
        newObject2.transform.localRotation = Quaternion.identity;
        appendedObjects.Add(newObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(!staticRendered){
            // Update the renderer based on distance from the player
        foreach (GameObject appendedObject in appendedObjects)
        {
            Renderer renderer = appendedObject.GetComponent<Renderer>();
            if (renderer != null)
            {
                float distance = Vector3.Distance(appendedObject.transform.position, player.transform.position);
                if (distance <= maxRenderDistance)
                {
                    renderer.enabled = true;
                }
                else
                {
                    renderer.enabled = false;
                }
            }
        }
        }
    }
}
