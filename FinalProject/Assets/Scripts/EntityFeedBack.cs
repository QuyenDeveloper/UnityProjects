using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class EntityFeedBack : MonoBehaviour
{
    [Header("Toggle")]
    [SerializeField]
    private bool KnockbackToggle = true;

    [SerializeField]
    private Rigidbody2D rb;

    [SerializeField]
    private Animator animator;

    [SerializeField]
    private float strength = 16;



    public UnityEvent OnBegin, OnDone;
    public void PlayFeedBack(GameObject sender)
    {
        StopAllCoroutines();
        OnBegin?.Invoke();
        TakeDamageFeedBack();
        KnockbackFeedBack(sender);

        StartCoroutine(Reset());
    }

    private IEnumerator Reset()
    {
        float elapsedTime = 0;
        float duration = 0.2f;
        Vector2 originalVelocity = rb.velocity;

        while (elapsedTime < duration)
        {
            rb.velocity = Vector2.Lerp(originalVelocity, Vector2.zero, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        rb.velocity = Vector2.zero;

        OnDone?.Invoke();
    }

    private void TakeDamageFeedBack()
    {
        animator.SetTrigger("isHit");
    }

    private void KnockbackFeedBack(GameObject sender)
    {
        if (!KnockbackToggle) return;
        Vector2 direction = (transform.position - sender.transform.position).normalized;

        rb.AddForce(direction * strength, ForceMode2D.Impulse);
    }


}
