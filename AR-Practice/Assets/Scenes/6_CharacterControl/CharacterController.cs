using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class CharacterController : MonoBehaviour
{
    // 캐릭터는 추후에 인스턴스화를 통해 생성되므로, RaycastManager를 public으로 받을 수 없음
    private ARRaycastManager _raycastManager;
    private Animator _anim;
    protected Transform Destination;

    private Vector3 _lastPosition;
    private float _restTime;

    void Start()
    {
        _anim = GetComponent<Animator>();
        _anim.speed = 1.5f;
        Destination = GameObject.FindWithTag("Player").transform;
        _raycastManager = GameObject.Find("AR Session Origin").GetComponent<ARRaycastManager>();
    }

    void Update()
    {
        if (Input.touchCount == 0)
        {
            // deltaTime: 이전 업데이트에서 지금 업데이트 사이의 시간
            _restTime += Time.deltaTime;
            if (_restTime < 3) return;
            _restTime = 0;
            _anim.SetBool("Rest", true);

            return;
        }

        _restTime = 0;
        _anim.SetBool("Rest", true);

        var hits = new List<ARRaycastHit>();
        _raycastManager.Raycast(TouchHelper.TouchPosition, hits, TrackableType.PlaneWithinBounds);
        if (hits.Count == 0) return;
        Destination.transform.position = hits[0].pose.position;
        Rotate();
        MoveTo(hits[0].pose.position);
    }

    void LateUpdate()
    {
        var delta = Vector3.Distance(transform.position, _lastPosition);
        _lastPosition = transform.position;
        _anim.SetFloat("Speed", delta * 100);
    }

    protected virtual void Rotate()
    {
        // 목적지 - 현재 위치 => 방향 벡터
        var direction = Destination.position - transform.position;
        direction.y = 0;
        // 현재 바라보는 방향 = 달려가려는 방향
        transform.rotation = Quaternion.LookRotation(direction);
    }

    protected virtual void MoveTo(Vector3 target)
    {
        transform.position =
            Vector3.Lerp(transform.position, Destination.position, Time.deltaTime);
    }
}
