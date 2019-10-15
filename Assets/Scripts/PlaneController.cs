using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.UI;
public class PlaneController : MonoBehaviour
{
    ARPlaneManager aRPlaneManager;
    public Text btnText;
    private void Awake()
    {
        aRPlaneManager = GetComponent<ARPlaneManager>();
    }

    public void TogglePlaneDetectionAndVisibality()
    {
        aRPlaneManager.enabled = !aRPlaneManager.enabled;
        if (aRPlaneManager.enabled)
        {
            SetAllPlaneActiveOrDeactive(true);
            GetComponent<ARPlacer>().enabled = true;
            btnText.text = "Disable Plane Detection And Hide Existing";
        }
        else
        {
            SetAllPlaneActiveOrDeactive(false);
            GetComponent<ARPlacer>().enabled = false;
            btnText.text = "Enable Plane Detection And Show Existing";  
        }
    }
    void SetAllPlaneActiveOrDeactive(bool value)
    {
        foreach(var plane in aRPlaneManager.trackables)
        {
            plane.gameObject.SetActive(value);
    
        }
    }
}
