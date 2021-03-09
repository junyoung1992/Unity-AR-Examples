using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelController : MonoBehaviour
{
    [SerializeField] private Camera arCamera;
    [SerializeField] private GameObject placementIndicator;
    private pIndicatorStatus placementIndicatorStatus;
    [SerializeField] private GameObject spawnedObjectWrap;
    private GameObject spawnedObject;

    private void Start()
    {
        placementIndicatorStatus = placementIndicator.GetComponent<pIndicatorStatus>();
    }

    void Update()
    {
        if (TouchHelper.IsDown)
        {
            if (Physics.Raycast(arCamera.ScreenPointToRay(TouchHelper.TouchPosition), out var raycastHit, arCamera.farClipPlane))
            {
                if (raycastHit.transform.gameObject.tag.Equals("Indicator"))
                {
                    var cameraForward = -1 * arCamera.transform.TransformDirection(Vector3.forward);
                    var cameraBearing = new Vector3(cameraForward.x, 0, cameraForward.z).normalized;
                    var position = placementIndicator.transform.position;
                    position.y -= 0.01f;

                    placementIndicatorStatus.modelIsSpawned = true;
                    placementIndicator.SetActive(false);
                    spawnedObjectWrap.SetActive(true);
                    spawnedObjectWrap.transform.SetPositionAndRotation(position, Quaternion.LookRotation(cameraBearing));
                }
            }
        }
    }
}
