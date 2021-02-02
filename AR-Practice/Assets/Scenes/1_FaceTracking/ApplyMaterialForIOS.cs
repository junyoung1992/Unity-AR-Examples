using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyMaterialForIOS : MonoBehaviour
{
    public Material[] materialsForIOS;
    
    // Start is called before the first frame update
    void Start()
    {
        // Materials가 아니라 Material을 사용하면 Material이 여러 개일 때 Element 0 만 교체함
#if UNITY_IOS
        GetComponent<MeshRenderer>().materials = materialsForIOS;
#endif
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
