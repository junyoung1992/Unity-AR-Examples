using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class PlaceController : MonoBehaviour
{
    public Camera mainCamera;
    public ARRaycastManager raycastManager;
    public GameObject placementIndicator;
    public GameObject[] prefab;
    // 생성한 오브젝트 모두 딕셔너리에 저장
    private Dictionary<int, GameObject> _instancedPrefab = new Dictionary<int, GameObject>();

    void Update()
    {
        var hits = new List<ARRaycastHit>();
        // Viewport를 Screen 좌표로... (0 ~ 1) => 따라서 0.5f는 중
        var center = mainCamera.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        raycastManager.Raycast(center, hits, TrackableType.PlaneWithinBounds);
        placementIndicator.SetActive(hits.Count > 0);
        if (hits.Count == 0) return;

        var cameraForward = mainCamera.transform.TransformDirection(Vector3.forward);
        var cameraBearing = new Vector3(cameraForward.x, 0, cameraForward.z).normalized;
        var pose = hits[0].pose;
        pose.rotation = Quaternion.LookRotation(cameraBearing);
        placementIndicator.transform.SetPositionAndRotation(pose.position, pose.rotation);

        if (TouchHelper.Touch3)
        {
            var index = Random.Range(0, prefab.Length);
            var obj = Instantiate(prefab[index], pose.position, pose.rotation, transform);
            obj.SetActive(true);
            // 생성되면 딕셔너리에 저
            _instancedPrefab[obj.GetInstanceID()] = obj;
            // 오브젝트 생성하면 선택 갱신
            RefreshSelection(obj);
        }

        if (Input.touchCount == 0) return;

        if (TouchHelper.IsDown)
        {
            if (Physics.Raycast(mainCamera.ScreenPointToRay(TouchHelper.TouchPosition),
                                out var raycastHits, mainCamera.farClipPlane))
            {
                if (raycastHits.transform.gameObject.tag.Equals("Player"))
                {
                    RefreshSelection(raycastHits.transform.gameObject);
                }
            }
        }
    }

    private void RefreshSelection(GameObject select)
    {
        foreach (var obj in _instancedPrefab)
        {
            var furniture = obj.Value.GetComponent<Furniture>();
            if (furniture)
            {
                furniture.IsSelected = obj.Key == select.GetInstanceID();
            }
        }
    }
}
