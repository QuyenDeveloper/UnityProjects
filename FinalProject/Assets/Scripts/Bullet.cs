
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private int damageLayer = 6;
    private int bulletDamage;
    public int DamageLayer { get => damageLayer; set => damageLayer = value; }
    public int BulletDamage { get => bulletDamage; set => bulletDamage = value; }

    public GameObject bulletPrefab;

    public int bulletCount = 0;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == damageLayer)
        {
            FighterStats enemyStats = collision.GetComponent<FighterStats>();

            if (enemyStats != null)
            {
                enemyStats.GetHit(bulletDamage, gameObject);
            }
            Destroy(gameObject);
        }
    }

    public void RedirectBullet(Vector2 redirectDirection, float bulletSpeed)
    {
        damageLayer = 7;
        GetComponent<Rigidbody2D>().velocity = redirectDirection * bulletSpeed;
        StartCoroutine(NormalProjectileLifeSpan(3, gameObject));
    }

    public void InitBulletPrefab(Vector2 direction, Vector3 pos, int damage, float bulletSpeed, int userType)
    {
        bulletCount++;

        if (bulletCount == 5 && userType == 1)
        {
            bulletCount = 0;
            BurstProjectile(direction,pos,damage,bulletSpeed);
        }
        else
        {
            NormalProjectile(direction, pos, damage, bulletSpeed);
        }
    }

    private void BurstProjectile(Vector2 direction, Vector3 pos, int damage, float projectileSpeed)
    {
        GameObject bullet = Instantiate(bulletPrefab, pos, Quaternion.identity);
        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
        bulletRb.velocity = direction * projectileSpeed;
        bullet.GetComponent<Bullet>().BulletDamage = damage;
        bullet.transform.localScale *= 1.5f;

        StartCoroutine(BurstProjectileLifeSpan(2, bullet, damage, projectileSpeed));
    }
    private IEnumerator BurstProjectileLifeSpan(float lifeSpan, GameObject bullet, int damage, float projectileSpeed)
    {
        yield return new WaitForSeconds(lifeSpan); 
        if(bullet != null)
        {
            for (int i = 0; i < 8; i++)
            {
                Vector2 randomDirection = Random.insideUnitCircle.normalized;
                NormalProjectile(randomDirection, bullet.transform.position, damage, projectileSpeed);
            }
            Destroy(bullet);
        }
    }

    private void NormalProjectile(Vector2 direction, Vector3 pos, int damage, float bulletSpeed)
    {
        GameObject bullet = Instantiate(bulletPrefab, pos, Quaternion.identity);
        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
        bulletRb.velocity = direction * bulletSpeed;
        bullet.GetComponent<Bullet>().BulletDamage = damage;

        StartCoroutine(NormalProjectileLifeSpan(3, bullet));
    }
    private IEnumerator NormalProjectileLifeSpan(float lifeSpan, GameObject bullet)
    {
        yield return new WaitForSeconds(lifeSpan);
        Destroy(bullet);
    }
}
