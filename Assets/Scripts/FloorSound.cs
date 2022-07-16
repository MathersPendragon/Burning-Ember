using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorSound : MonoBehaviour
{
    public AudioClip[] SoundStep;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        {
            EnterFloor();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        {
            ExitFloor();
        }
    }
    void EnterFloor()
    {
        GameManager.Instance.player.EnterAreaStep(SoundStep);
    }

    void ExitFloor()
    {
        GameManager.Instance.player.ExitAreaStep();
    }
}
