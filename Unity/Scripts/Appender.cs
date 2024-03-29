using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Appender : MonoBehaviour
{
    //Script logic
    private float maxRenderDistance = 2.5f;
    private GameObject player;
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
    public GameObject infographicObject;
    public GameObject staticObject;
    public GameObject textObject;

    // Start is called before the first frame update
    void Start()
    {
        GameObject[] taggedObjects = GameObject.FindGameObjectsWithTag(TAG);
        player = GameObject.FindGameObjectWithTag("Player");
        appendedObjects = new List<GameObject>();

        predictedObject = GetPredictedObject();

        foreach (GameObject taggedObject in taggedObjects)
        {
            AppendObject(taggedObject, xOffset, yOffset);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!staticRendered)
        {
            // Update the renderer based on distance from the player
            foreach (GameObject appendedObject in appendedObjects)
            {
                List<Renderer> renderers = new List<Renderer>();

                renderers.Add(appendedObject.GetComponent<Renderer>());

                for (int i = 0; i < appendedObject.transform.childCount; i++)
                {
                    renderers.Add(appendedObject.transform.GetChild(i).GetComponent<Renderer>());
                }

                if (renderers.Count > 0)
                {
                    foreach (Renderer renderer in renderers)
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

    private GameObject GetPredictedObject()
    {
        int index = -1;
        new WaitUntil(() => GameObject.FindGameObjectWithTag("ModelAgent").GetComponent<ModelAgent>().IsInitialized);
        index = (int)GameObject.FindGameObjectWithTag("ModelAgent").GetComponent<ModelAgent>().TypeIndex;
        switch (index)
        {
            case 0:
                Debug.Log("infographic");
                return infographicObject;
            case 1:
                Debug.Log("pictogram");
                return pictoObject;
            case 2:
                Debug.Log("static");
                staticRendered = true;
                return staticObject;
            case 3:
                Debug.Log("text");
                return textObject;
        }
        Debug.Log("no predicted object yet");
        return null;
    }

    private void AppendObject(GameObject taggedObject, float xOffset, float yOffset)
    {
        GameObject newObject = Instantiate(predictedObject, taggedObject.transform);
        newObject.transform.localPosition = new Vector3(xOffset, yOffset, zOffset);
        //newObject.transform.localRotation = Quaternion.identity;
        appendedObjects.Add(newObject);

        if (mirrored)
        {
            GameObject newObject2 = Instantiate(predictedObject, taggedObject.transform);
            newObject2.transform.localPosition = new Vector3(-xOffset, yOffset, zOffset);
            newObject2.transform.localRotation *= Quaternion.AngleAxis(180, Vector3.forward);
            appendedObjects.Add(newObject2);
        }
    }
}
