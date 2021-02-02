using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxController : MonoBehaviour
{
    private const float CameraDistance = 7.5f;
    public float positionY = 0.4f;
    public GameObject[] prefab;

    protected Camera mainCamera;
    protected GameObject HoldingObject;
    protected Vector3 InputPosition;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        // 시작하자마자 어떤 오브젝트를 만듬
        Reset();
    }

    // Update is called once per frame
    void Update()
    {
#if !UNITY_EDITOR
        if (Input.touchCount == 0) return;
#endif
        InputPosition = TouchHelper.TouchPosition;
        if (TouchHelper.Touch2)
        {
            Reset();
            return;
        }

        if (HoldingObject)
        {
            if (TouchHelper.IsUP)
            {
                OnPut(InputPosition);
                HoldingObject = null;
                return;
            }
            Move(InputPosition);
            return;
        }

        if (!TouchHelper.IsDown) return;
        // 클릭한 물체를 선택
        if (Physics.Raycast(mainCamera.ScreenPointToRay(InputPosition), out var hits, mainCamera.farClipPlane)) {
            if (hits.transform.gameObject.tag.Equals("Player"))
            {
                HoldingObject = hits.transform.gameObject;
                OnHold();
            }
        }
    }

    private void Move(Vector3 pos)
    {
        pos.z = mainCamera.nearClipPlane * CameraDistance;
        // 그냥 Vector3 위치 지정하는 것보다 Lerp가 보기에 자연스러움
        // Time.deltaTime * 7.0f는 강사가 여러번 테스트를 해 얻은 자연스러운 값 
        HoldingObject.transform.position = Vector3.Lerp(
            HoldingObject.transform.position,
            mainCamera.ScreenToWorldPoint(pos),
            Time.deltaTime * 7.0f);
    }

    // protected virtual -> 하위 클래스에서 상위 클래스의 함수를 재정의 가p
    protected virtual void OnHold()
    {
        HoldingObject.GetComponent<Rigidbody>().useGravity = false;
        HoldingObject.transform.SetParent(mainCamera.transform);
        HoldingObject.transform.rotation = Quaternion.identity;
        HoldingObject.transform.position = mainCamera.ViewportToWorldPoint(
            new Vector3(0.5f, positionY, mainCamera.nearClipPlane * CameraDistance));
    }

    protected virtual void OnPut(Vector3 pos)
    {
        HoldingObject.GetComponent<Rigidbody>().useGravity = true;
        HoldingObject.transform.SetParent(null);
    }

    private void Reset()
    {
        var pos = mainCamera.ViewportToWorldPoint(new Vector3(
            0.5f, positionY, mainCamera.nearClipPlane * CameraDistance));
        var index = Random.Range(0, prefab.Length);
        // Instantiate(객체, 위치, 각도, 부모)
        var obj = Instantiate(prefab[index], pos, Quaternion.identity, mainCamera.transform);
        var rigidbody = obj.GetComponent<Rigidbody>();
        rigidbody.useGravity = false;
        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;
    }
}
