using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Occlusion : MonoBehaviour
{
    private SpriteRenderer sprite;

    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    void OnBecameVisible()
    {
        sprite.enabled = true;
    }
    void OnBecameInvisible()
    {
        sprite.enabled = false;
    }
}
