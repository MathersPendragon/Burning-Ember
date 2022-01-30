using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortOrderY_Particle : MonoBehaviour
{
    public bool onUpdate = false;
    private ParticleSystemRenderer particle;
    void Start()
    {
        particle = GetComponent<ParticleSystemRenderer>();
        particle.sortingOrder = Mathf.RoundToInt(transform.position.y * 100f) * -1;
    }
    void Update()
    {
        if (onUpdate)
        {
            particle.sortingOrder = Mathf.RoundToInt(transform.position.y * 100f) * -1;
        }
    }
}
