using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallEffect : MonoBehaviour
{
    public ParticleSystem particle;

    private void OnCollisionEnter(Collision other)
    {
        // 바닥에 떨어질 때만 폭발
        if (other.gameObject.tag.Equals("Player")) return;
        Instantiate(particle, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
