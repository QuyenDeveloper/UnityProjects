using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponParent : MonoBehaviour
{
    public Vector2 PointerPosition {  get; set; }

    private bool attackBlocked;
    public bool IsAttacking {  get; private set; }
    public float radius;

    public SpriteRenderer charaterRenderer, weaponRenderer;
    public Animator weaponAnimator, playerAnimator;
    public Transform circleOrigin;
    private FighterStats fightStats;
    private AudioHandler audioHandler;
    private void Awake()
    {
        fightStats = GetComponentInParent<FighterStats>();
        audioHandler = GetComponentInParent<AudioHandler>();
    }
    public void ResetIsAttacking()
    {
        IsAttacking = false;
    }
    private void Update()
    {
        if (IsAttacking) return;
        Vector2 direction = (PointerPosition - (Vector2)transform.position).normalized;
        transform.right = direction;

        Vector2 scale = transform.localScale;
        if(direction.x < 0)
        {
            scale.y = -1;
            playerAnimator.SetFloat("X", -1);
        }else if(direction.x > 0)
        {
            scale.y = 1;
            playerAnimator.SetFloat("X", 1);
        }

        transform.localScale = scale;

        if(transform.eulerAngles.z > 0 && transform.eulerAngles.z < 180)
        {
            weaponRenderer.sortingOrder = charaterRenderer.sortingOrder - 1;
        }
        else
        {
            weaponRenderer.sortingOrder = charaterRenderer.sortingOrder + 1;
        }
    }

    public void Attack()
    {
        if (attackBlocked || !weaponAnimator) return;
        weaponAnimator.SetTrigger("Attack");
        IsAttacking = true;
        attackBlocked = true;
        audioHandler.AttackAudio();
        StartCoroutine(DelayAttack());
    }

    private IEnumerator DelayAttack()
    {
        yield return new WaitForSeconds(fightStats.AttackSpeed);
        attackBlocked = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Vector3 pos = circleOrigin == null ? Vector3.zero : circleOrigin.position;
        Gizmos.DrawWireSphere(pos, radius);
    }

    public void DetectColliders()
    {
        foreach (Collider2D collider in Physics2D.OverlapCircleAll(circleOrigin.position, radius))
        {
            FighterStats health;
            Bullet bullet;

            if (health = collider.GetComponent<FighterStats>())
            {
                health.GetHit(fightStats.AttackDamage, transform.parent.gameObject);
            }
            if (bullet = collider.GetComponent<Bullet>())
            {
                bullet.RedirectBullet((PointerPosition - (Vector2)transform.position).normalized, 3);
            }
        }
    }
}
