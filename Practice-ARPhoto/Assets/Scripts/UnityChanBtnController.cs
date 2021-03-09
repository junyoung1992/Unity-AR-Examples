using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnityChanBtnController : BlockBtnController
{
    protected override void ChangeAnimation(string anim)
    {
        Debug.Log(animController);
        Debug.Log(anim);
        animController.SetBool("Rest", false);
        animController.SetFloat("Speed", 0.0f);
        animController.SetBool("Jump", false);
        
        switch (anim)
        {
            case "Rest":
                animController.SetBool("Rest", true);
                break;
            case "Locomotion":
                animController.SetFloat("Speed", 0.2f);
                break;
            case "Jump":
                animController.SetFloat("Speed", 0.2f);
                animController.SetBool("Jump", true);
                break;
        }
    }
    
}
