using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARTrackedImageManager))]
public class ImageTracking : MonoBehaviour
{
    [SerializeField]
    private GameObject[] placeablePrefabs;

    private Dictionary<string, GameObject> spawnPrefabs = new Dictionary<string, GameObject>();
    private ARTrackedImageManager m_ARTrackedImageManager;

    private void Awake()
    {
        m_ARTrackedImageManager = GetComponent<ARTrackedImageManager>();

        foreach(GameObject prefab in placeablePrefabs)
        {
            GameObject newPrefab = Instantiate(prefab, Vector3.zero, Quaternion.identity);
            newPrefab.name = prefab.name;
            newPrefab.SetActive(false);
            spawnPrefabs.Add(prefab.name, newPrefab);
        }
    }

    private void OnEnable()
    {
        m_ARTrackedImageManager.trackedImagesChanged += OnARTrackedImageChanged;
    }

    private void OnDisable()
    {
        m_ARTrackedImageManager.trackedImagesChanged -= OnARTrackedImageChanged;
    }

    void OnARTrackedImageChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach(ARTrackedImage trackedImage in eventArgs.added)
        {
            UpdateImage(trackedImage);
        }

        foreach (ARTrackedImage trackedImage in eventArgs.updated)
        {
            UpdateImage(trackedImage);
        }

        // 화면에 나타나지 않아도 tracking 되고 있음
        // 오랜 시간동안 추적되지 않거나 프로그램을 나갈 때나 실행됨
        foreach (ARTrackedImage trackedImage in eventArgs.removed)
        {
            spawnPrefabs[trackedImage.name].SetActive(false);
        }
    }

    private void UpdateImage(ARTrackedImage trackedImage)
    {
        string name = trackedImage.referenceImage.name;
        Vector3 position = trackedImage.transform.position;

        // 객체가 화면에서 Tracking 될 때만 객체가 나타나게 생성
        if (trackedImage.trackingState == TrackingState.Tracking)
        {
            spawnPrefabs[name].transform.position = position;
            spawnPrefabs[name].SetActive(true);
        }
        else // None, Limited
        {
            spawnPrefabs[name].SetActive(false);
        }

        foreach (GameObject go in spawnPrefabs.Values)
        {
            if (go.name != name)
            {
                go.SetActive(false);
            }
        }
    }
}
