using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class PlacementIndicator : MonoBehaviour
{
    private ARRaycastManager rayManager;

    [SerializeField] Camera ARCamera;

    [SerializeField] GameObject placement;
    [SerializeField] GameObject obj;

    [SerializeField] GameObject Text;

    private bool fixedPos = false;

    private string text = "";

    void Start()
    {
        // get the components
        rayManager = FindObjectOfType<ARRaycastManager>();
        
        // hide the placement indicator visual
        placement.SetActive(false);
        obj.SetActive(false);
    }

    void Update()
    {
        Text.GetComponent<Text>().text = text;
        List<ARRaycastHit> hits = new List<ARRaycastHit>();

        if (!fixedPos)
        {
            // shoot a raycast from the center of the screen
            rayManager.Raycast(new Vector2(Screen.width / 2, Screen.height / 2), hits, TrackableType.Planes);
            obj.SetActive(false);
            placement.SetActive(true);
        }
        else
        {
            // shoot a raycast from the pointer position
            // rayManager.Raycast(new Vector2(Input.mousePosition.x, Input.mousePosition.y), hits, TrackableType.Planes);
            obj.SetActive(true);
            placement.SetActive(false);
        }

        // if we hit an AR plane surface, update the position and rotation
        if(hits.Count > 0)
        {
            transform.position = hits[0].pose.position;
            transform.rotation = hits[0].pose.rotation;

            // enable the visual if it's disabled
            // if(!placement.activeInHierarchy && !fixedPos)
            // {
            //     obj.SetActive(false);
            //     placement.SetActive(true);
            // }
            // else if (fixedPos)
            // {
            //     obj.SetActive(true);
            //     placement.SetActive(false);
            // }
        }
        
        if (Input.GetMouseButtonUp(0))
        {
            // Ray ray = ARCamera.ScreenPointToRay(Input.mousePosition);
            Ray ray = ARCamera.ScreenPointToRay(ARCamera.transform.position);

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                // text = hit.transform.tag;
                text =  string.Format("{0}, {1}, {2}",
                    hit.transform.position.x, hit.transform.position.y, hit.transform.position.z);
            }
        }
    }

    public void ChangeState()
    {
        fixedPos = !fixedPos;
    }
}
