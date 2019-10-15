using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.XR;
using UnityEngine.XR.ARFoundation;
public class ARPlacer : MonoBehaviour
{
    ARRaycastManager aRRaycastManager;
    List<ARRaycastHit> aRRaycastHits = new List<ARRaycastHit>();
    public GameObject locationBasedMap;
    private void Awake()
    {
        aRRaycastManager = GetComponent<ARRaycastManager>();
    }
    // Start is called before the first frame update
    void Start()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.touches[0];
            if (aRRaycastManager.Raycast(touch.position, aRRaycastHits, UnityEngine.XR.ARSubsystems.TrackableType.PlaneWithinPolygon)){
                Pose pose = aRRaycastHits[0].pose;
                locationBasedMap.transform.position = pose.position;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
