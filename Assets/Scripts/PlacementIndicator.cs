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

    [SerializeField] List<GameObject> HideAfterClick;
    [SerializeField] List<GameObject> HideBeforeClick;
    [SerializeField] GameObject obj;

    [SerializeField] GameObject Text;

    private bool placeObject = false;

    private string text = "";

    void Start()
    {
        // Get the components
        rayManager = FindObjectOfType<ARRaycastManager>();
        
        // Hide the placement indicator visual
        SetActiveList(HideAfterClick, false);
        SetActiveList(HideBeforeClick, false);
    }

    void Update()
    {
        Text.GetComponent<Text>().text = text;
        List<ARRaycastHit> hits = new List<ARRaycastHit>();

        if (!placeObject)
        {
            // Shoot a raycast from the center of the screen
            rayManager.Raycast(new Vector2(Screen.width / 2, Screen.height / 2),
                hits,
                TrackableType.Planes
            );
            
            // Hide the gameobject in the hierarchy and show the placement indicator
            SetActiveList(HideAfterClick, true);
            SetActiveList(HideBeforeClick, false);
        }
        else
        {
            // Activate the gameobject we want to display in the hierarchy
            // and hide the placement indicator
            SetActiveList(HideAfterClick, false);
            SetActiveList(HideBeforeClick, true);
        }

        // If we hit an AR plane surface, update the position and rotation
        if(hits.Count > 0)
        {
            transform.position = hits[0].pose.position;
            transform.rotation = hits[0].pose.rotation;
        }
        
        if (Input.GetMouseButtonDown(0))
        {
            text = "Clicked" + '\n';

            // Create a ray from mouse/touch position
            Ray ray = ARCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                // Display selected gameobject tag and the impact position
                text += "Tag: " + hit.transform.tag + '\n';
                text +=  string.Format("{0}, {1}, {2}",
                    hit.point.x,
                    hit.point.y,
                    hit.point.z
                );
            }
        }
    }

    private void SetActiveList(List<GameObject> objects, bool active)
    {
        foreach (GameObject obj in objects)
        {
            obj.SetActive(active);
        }
    }

    public void ChangeState()
    {
        placeObject = !placeObject;
    }
}
