using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioHandler : MonoBehaviour
{
    [Header("Audio Sourse")]
    [SerializeField] private AudioSource attackingAS;
    [SerializeField] private AudioSource movingAS;

    [Header("Audio Clip")]
    [SerializeField] private AudioClip takeDamageAC;
    [SerializeField] private AudioClip movingAC;
    [SerializeField] private AudioClip deathAC;
    [SerializeField] private AudioClip[] attackingAC;
    public void TakeDamgeAudio(AudioSource audioSource)
    {
        if (!takeDamageAC) return;
        audioSource.PlayOneShot(takeDamageAC);
    }

    public void AttackAudio()
    {
        foreach (var item in attackingAC)
        {
            attackingAS.PlayOneShot(item);
        }
    }

    public void MovementAudio(Animator animator)
    {
        if (!movingAC) return;
        if (!movingAS.clip) movingAS.clip = movingAC;

        bool isWalking = animator.GetBool("isWalking");
        if(isWalking && !movingAS.isPlaying) {
            movingAS.Play();
        }else if (!isWalking)
        {
            movingAS.Pause();
        }
    }

    public void DeathAudio()
    {
        if (!deathAC)
        {
            Debug.Log("DeathAC return");
            return;
        }
        movingAS.PlayOneShot(deathAC);
    }
}
