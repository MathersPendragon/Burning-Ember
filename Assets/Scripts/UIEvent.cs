using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class UIEvent : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler
{
    public AudioSource audioSource;
    public AudioClip[] clips;
    public void OnPointerEnter(PointerEventData ped)
    {
        audioSource.clip = clips[1];
        audioSource.Play();
    }

    public void OnPointerDown(PointerEventData ped)
    {
        audioSource.clip = clips[0];
        audioSource.Play();
    }
}