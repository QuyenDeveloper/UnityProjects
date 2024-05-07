using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FighterStats : MonoBehaviour
{
    public delegate void OnHealthChangedDelegate();
    public OnHealthChangedDelegate onHealthChangedCallback;

    [SerializeField]
    private int  maxHealth, totalMaxHealth;
    [SerializeField]
    private int armor, attackDamage, scoreGainOnKill;
    [SerializeField]
    private float currentHealth, maxSpeed, acceleration = 50, deacceleration = 100, currentSpeed = 0, attackSpeed;

    public UnityEvent<GameObject> OnHitWithReference, OnDeathWithReference;

    [SerializeField]
    private bool isDead = false;
    public Animator _entityAnimator;
    [SerializeField] private InGameMenu _gameMenu;

    public float fadeDuration = 2f; 
    private Renderer objectRenderer;
    private Color originalColor;
    private Color targetColor;
    private float currentFadeTime = 0f;
    private bool isFading = false;

    public float MaxSpeed { get => maxSpeed; set => maxSpeed = value; }
    public float Acceleration { get => acceleration; set => acceleration = value; }
    public float Deacceleration { get => deacceleration; set => deacceleration = value; }
    public float CurrentSpeed { get => currentSpeed; set => currentSpeed = value; }
    public int AttackDamage { get => attackDamage; set => attackDamage = value; }
    public float AttackSpeed { get => attackSpeed; set => attackSpeed = value; }
    public float CurrentHealth { get => currentHealth; set => currentHealth = value; }
    public int MaxHealth { get => maxHealth; set => maxHealth = value; }
    public int TotalMaxHealth { get => totalMaxHealth; set => totalMaxHealth = value; }
    public int ScoreGainOnKill { get => scoreGainOnKill; set => scoreGainOnKill = value; }

    public Material defaultMaterial;
    private void Start()
    {
        objectRenderer = GetComponent<Renderer>();
        originalColor = objectRenderer.material.color;
    }

    private void Update()
    {
        if (isFading)
        {
            if (currentFadeTime < fadeDuration)
            {
                currentFadeTime += Time.deltaTime;

                float t = currentFadeTime / fadeDuration;
                objectRenderer.material.color = Color.Lerp(originalColor, targetColor, t);
            }
            else
            {
                // Fading completed
                Destroy(gameObject);
            }
        }
    }
    public void GetHit(int amount, GameObject sender)
    {
        if (isDead) return;
        if (sender.layer == gameObject.layer) return;

        float damamgeTaken = amount - armor;

        damamgeTaken = Mathf.Clamp(damamgeTaken, 1, damamgeTaken);

        currentHealth -= damamgeTaken/2;

        ClampHealth();

        if (currentHealth > 0)
        {
            OnHitWithReference?.Invoke(sender);
        }
        else
        {
            OnDeathWithReference?.Invoke(sender);
            currentHealth = 0;
            isDead = true;
            OnDeath();
        }
    }


    public void Heal(int health)
    {
        currentHealth += health;
        ClampHealth();
    }

    public void AddHealth()
    {
        if (maxHealth < totalMaxHealth)
        {
            maxHealth += 1;
            currentHealth = maxHealth;

            if (onHealthChangedCallback != null)
                onHealthChangedCallback.Invoke();
        }
    }

    void ClampHealth()
    {
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (onHealthChangedCallback != null)
            onHealthChangedCallback.Invoke();
    }

    public void OnDeath()
    {
        _gameMenu.AddScore = scoreGainOnKill;
        _entityAnimator.SetTrigger("isDead");
        gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        GameObject child = transform.GetChild(0).gameObject;
        Destroy(child);
        Destroy(gameObject.GetComponent<CapsuleCollider2D>());

        FadeOut();
    }
    public void FadeOut()
    {
        targetColor = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);
        currentFadeTime = 0f;
        isFading = true;
    }
}
