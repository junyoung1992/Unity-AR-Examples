using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class PlaceOnPlane : MonoBehaviour
{
    public ARRaycastManager raycastManager;
    public GameObject prefab;

    // Update is called once per frame
    void Update()
    {
        // 터치 없으면 실행 X
        if (Input.touchCount == 0) return;

        var touch = Input.GetTouch(0);
        // 터치 시점에 동작하기를 바라므로, 터치가 시작한 시점이 아니면 종료
        if (touch.phase != TouchPhase.Began) return;

        var hits = new List<ARRaycastHit>();
        // raycastManager.Raycast는 생성한 오브젝트에 레이저를 쏠 때만 사용
        // 그 외의 경우에는 유니티에서 기본 제공하는 Raycast 사용해야 함
        if (raycastManager.Raycast(touch.position, hits, TrackableType.PlaneWithinBounds))
        {
            var pose = hits[0].pose;
            Instantiate(prefab, pose.position, pose.rotation);
        }
    }
}
