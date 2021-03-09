using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ImageDetectionController : MonoBehaviour
{

    [SerializeField] private Camera arCamera;
    [SerializeField] private ARTrackedImageManager arTrackedImageManager;
    [SerializeField] private GameObject spawnedObjectWrap;
    [SerializeField] private GameObject[] placeablePrefabs;
    private Dictionary<string, GameObject> spawnedPrefabDictionary = new Dictionary<string, GameObject>();

    void Awake()
    {
        foreach (var prefab in placeablePrefabs)
        {
            var newPrefab = Instantiate(prefab, Vector3.zero, Quaternion.identity);
            newPrefab.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            newPrefab.name = prefab.name;
            newPrefab.SetActive(false);
            spawnedPrefabDictionary.Add(prefab.name, newPrefab);
        }
    }

    private void OnEnable() => arTrackedImageManager.trackedImagesChanged += TrackedImageChanged;

    private void OnDisable() => arTrackedImageManager.trackedImagesChanged -= TrackedImageChanged;

    private void TrackedImageChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach (var trackedImage in eventArgs.added)
        {
            UpdateImage(trackedImage);
        }
        
        foreach (var trackedImage in eventArgs.updated)
        {
            // UpdateImage(trackedImage);
        }
        
        foreach (var trackedImage in eventArgs.removed)
        {
            spawnedPrefabDictionary[trackedImage.referenceImage.name].SetActive(false);
        }
    }

    private void UpdateImage(ARTrackedImage trackedImage)
    {
        var prefabName = trackedImage.referenceImage.name;
        var prefabPosition = trackedImage.transform.position;
        Debug.Log("Prefab Name: " + prefabName);
        Debug.Log("Camera Position: " + arCamera.transform.position);
        Debug.Log("Image Position: " + prefabPosition);
        // 카메라 위치와 트래킹된 이미지의 위치가 같음 -> 문제

        var cameraForward = -1 * arCamera.transform.TransformDirection(Vector3.forward);
        var cameraBearing = new Vector3(cameraForward.x, 0.0f, cameraForward.z).normalized;
        
        spawnedPrefabDictionary[prefabName].transform.SetPositionAndRotation(prefabPosition, Quaternion.LookRotation(cameraBearing));
        spawnedPrefabDictionary[prefabName].SetActive(true);
        Debug.Log("Prefab Position: " + spawnedPrefabDictionary[prefabName].transform.position);
        foreach (var go in spawnedPrefabDictionary.Values)
        {
            if (go.name != prefabName)
            {
                go.SetActive(false);
            }
        }
    }
    
}
