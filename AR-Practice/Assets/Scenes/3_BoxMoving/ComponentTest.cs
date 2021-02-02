using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentTest : MonoBehaviour
{
    public Collider Collider;

    // Start is called before the first frame update
    void Start()
    {
        // collider = GetComponent<Collider>();
        var cols = gameObject.GetComponentsInChildren<Collider>();
        GameObject.Find("Cube");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
