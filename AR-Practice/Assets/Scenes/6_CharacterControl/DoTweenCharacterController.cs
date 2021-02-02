using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DoTweenCharacterController : CharacterController
{
    protected override void Rotate()
    {
        // 목적지 - 현재 위치 => 방향 벡터
        var direction = Destination.position - transform.position;
        direction.y = 0;
        // 0.5초간 원하는 각도로 회전
        transform.DORotateQuaternion(Quaternion.LookRotation(direction), 0.5f);
        
    }

    protected override void MoveTo(Vector3 target)
    {
        // 1초에 0.5m 이동
        const float speed = 0.5f;
        // Speed = distance / time
        // time = distance / speed
        var distance = Vector3.Distance(transform.position, target);
        // time 동안 target으로 이
        transform.DOMove(target, distance / speed);
    }
}
