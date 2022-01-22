using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortOrderY : MonoBehaviour
{
    public bool onUpdate = false;
    private SpriteRenderer sprite;
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        sprite.sortingOrder = Mathf.RoundToInt(transform.position.y * 100f) * -1;
    }
    void Update()
    {
        if(onUpdate)
        {
            sprite.sortingOrder = Mathf.RoundToInt(transform.position.y * 100f) * -1;
        }
    }
}
