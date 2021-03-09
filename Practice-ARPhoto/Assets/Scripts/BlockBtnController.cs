using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockBtnController : MonoBehaviour
{

    private Camera arCamera;
    private GameObject spawnedObject;
    protected Animator animController;
    private List<GameObject> animBtn = new List<GameObject>();

    void Start()
    {
        arCamera = GameObject.Find("AR Camera").GetComponent<Camera>();
        spawnedObject = transform.GetChild(1).gameObject;
        animController = spawnedObject.GetComponent<Animator>();
        
        var animBtnWrap = transform.GetChild(3);
        Debug.Log(animBtnWrap.gameObject.name);
        for (var i = 0; i < animBtnWrap.childCount; i++)
        {
            animBtn.Add(animBtnWrap.GetChild(i).gameObject);
        }
    }

    void Update()
    {
        if (TouchHelper.IsDown)
        {
            if (Physics.Raycast(arCamera.ScreenPointToRay(TouchHelper.TouchPosition), out var raycastHit, arCamera.farClipPlane))
            {
                if (raycastHit.transform.gameObject.name.Equals("RemoveBtn"))
                {
                    gameObject.SetActive(false);
                }
                
                foreach (var btn in animBtn)
                {
                    if (raycastHit.transform.gameObject.name.Equals(btn.name))
                    {
                        Debug.Log(animController);
                        ChangeAnimation(btn.name);
                    }
                }
            }
        }
    }

    protected virtual void ChangeAnimation(string anim) { }

}
