using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class PlacementController : MonoBehaviour
{
    [SerializeField] private Camera arCamera;
    [SerializeField] private ARPlaneManager arPlaneManager;
    [SerializeField] private ARRaycastManager arRaycastManager;
    
    [SerializeField] private GameObject virtualFloor;
    private List<ARPlane> arPlaneList = new List<ARPlane>();
    private ARPlane currentPlane;
    
    [SerializeField] private GameObject placementIndicator;

    private void Start()
    {
        arPlaneManager.planesChanged += PlaneChanged;
    }
    
    private void Update()
    {
        if (Physics.Raycast(arCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f)), out var raycastHit,
                arCamera.farClipPlane, 1 << LayerMask.NameToLayer("VirtualFloor")))
        {
            placementIndicator.SetActive(true);
            var hitPosition = raycastHit.point;
            hitPosition.y += 0.01f;
            placementIndicator.transform.position = hitPosition;
        }
        else
        {
            placementIndicator.SetActive(false);
        }
    }

    private void PlaneChanged(ARPlanesChangedEventArgs eventArgs)
    {
        foreach (var trackedPlane in eventArgs.added)
        {
            arPlaneList.Add(trackedPlane);
            AddPlane(trackedPlane);
        }
        foreach (var trackedPlane in eventArgs.updated)
        {
            UpdatePlane(trackedPlane);
        }
        foreach (var trackedPlane in eventArgs.removed)
        {
            arPlaneList.Remove(trackedPlane);
            RemovePlane(trackedPlane);
        }
    }

    // 가장 낮은 위치의 지면에 virtualFloor 설치
    private void AddPlane(ARPlane trackedPlane)
    {
        if (arPlaneList.Count == 1)
        {
            currentPlane = arPlaneList[0];
            SetVirtualFloor();
        }
        else
        {
            if (trackedPlane.transform.position.y < currentPlane.transform.position.y)
            {
                currentPlane = trackedPlane;
                SetVirtualFloor();
            }
        }
    }
    
    private void UpdatePlane(ARPlane trackedPlane)
    { 
        if (currentPlane == trackedPlane)
        {
            arPlaneList.Sort((x, y) => 
                x.transform.position.y.CompareTo(y.transform.position.y));

            if (arPlaneList[0].transform.position.y < currentPlane.transform.position.y)
            {
                currentPlane = arPlaneList[0];
                SetVirtualFloor();
            }
        }
        else
        {
            if (trackedPlane.transform.position.y < currentPlane.transform.position.y)
            {
                currentPlane = trackedPlane;
                SetVirtualFloor();
            }
        }
    }

    private void RemovePlane(ARPlane trackedPlane)
    {
        if (arPlaneList.Count == 0)
        {
            virtualFloor.SetActive(false);
            return;
        }

        if (trackedPlane == currentPlane)
        {
            arPlaneList.Sort((x, y) => 
                x.transform.position.y.CompareTo(y.transform.position.y));
            currentPlane = arPlaneList[0];
            SetVirtualFloor();
        }
    }

    private void SetVirtualFloor()
    {
        virtualFloor.SetActive(true);
        var position = arCamera.transform.position;
        position.y = currentPlane.transform.position.y;
        virtualFloor.transform.position = position;
        virtualFloor.transform.localScale = new Vector3(100.0f, 100.0f, 1.0f);
    }
    
}
