using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FighterStats : MonoBehaviour
{
    public delegate void OnHealthChangedDelegate();
    public OnHealthChangedDelegate onHealthChangedCallback;

    [SerializeField]
    private float  maxHealth, totalMaxHealth;
    [SerializeField]
    private int armor, attackDamage, scoreGainOnKill;
    [SerializeField]
    private float currentHealth, maxSpeed, acceleration = 50, deacceleration = 100, currentSpeed = 0, attackSpeed;

    public UnityEvent<GameObject> OnHitWithReference, OnDeathWithReference;

    [SerializeField]
    private bool isDead = false;
    public Animator _entityAnimator;
    [SerializeField] private InGameMenu _gameMenu;
    [SerializeField] private GameObject[] itemPool;

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
    public float MaxHealth { get => maxHealth; set => maxHealth = value; }
    public float TotalMaxHealth { get => totalMaxHealth; set => totalMaxHealth = value; }
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
                Destroy(gameObject);
            }
        }
    }
    public void GetHit(float amount, GameObject sender)
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


    public void Heal(float health)
    {
        currentHealth += health;
        ClampHealth();
    }

    public void AddHealth(float increaseAmount)
    {
        if (maxHealth < totalMaxHealth)
        {
            maxHealth += increaseAmount;
            maxHealth = Mathf.Clamp(maxHealth, 0, totalMaxHealth);
            currentHealth += increaseAmount;
            ClampHealth();
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
        GameObject a = GetRandomItemOrNothing(0.7f);
        if(a != null) Instantiate(a, transform.position, Quaternion.identity);
    }

    private GameObject GetRandomItem()
    {
        int totalWeight = 0;
        foreach (var weightedItem in itemPool)
        {
            Item i = weightedItem.GetComponent<Item>();
            totalWeight += i.ItemWeight;
        }

        int randomValue = Random.Range(0, totalWeight);

        int cumulativeWeight = 0;
        foreach (var weightedItem in itemPool)
        {
            Item i = weightedItem.GetComponent<Item>();
            cumulativeWeight += i.ItemWeight;
            if (randomValue < cumulativeWeight)
            {
                i.GameMenu = _gameMenu;
                return weightedItem;
            }
        }

        return null;
    }

    public GameObject GetRandomItemOrNothing(float probabilityOfNothing)
    {
        if (Random.value < probabilityOfNothing)
        {
            return null;
        }
        else
        {
            return GetRandomItem();
        }
    }
}
