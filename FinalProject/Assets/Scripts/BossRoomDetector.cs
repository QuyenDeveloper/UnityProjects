using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BossRoomDetector : MonoBehaviour
{
    public UnityEvent<Transform> OnPlayerEnter;
    public AudioSource AudioSource;
    public AudioClip AudioClip;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            OnPlayerEnter?.Invoke(collision.transform);
            AudioSource.clip = AudioClip;
            AudioSource.Play();
        }
    }
}
