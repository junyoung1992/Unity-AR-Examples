using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchHelper : MonoBehaviour
{
#if UNITY_EDITOR
    // 손가락 두 개의 입력을 마우스 오른쪽 버튼으로 대체
    public static bool Touch2 => Input.GetMouseButtonDown(1);
    public static bool IsDown => Input.GetMouseButtonDown(0);
    public static bool IsUP => Input.GetMouseButtonUp(0);
    public static Vector2 TouchPosition => Input.mousePosition;
#else
    // 손가락 두 개의 입력의 이벤트를 받음
    public static bool Touch2 => Input.touchCount == 2 && (Input.GetTouch(1).phase == TouchPhase.Began);
    public static bool IsDown => Input.GetTouch(0).phase == TouchPhase.Began;
    public static bool IsUP => Input.GetTouch(0).phase == TouchPhase.Ended;
    public static Vector2 TouchPosition => Input.GetTouch(0).position;
#endif

    public static bool Touch3 => Input.touchCount == 3 && (Input.GetTouch(2).phase == TouchPhase.Began);
}
