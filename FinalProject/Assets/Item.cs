using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField]
    private float targetDetectionRange = 2f;

    [SerializeField]
    private LayerMask playerLayerMask;

    [SerializeField]
    private float increaseAmount = 2;

    [SerializeField]
    private float attractionSpeed = 2f;

    [SerializeField]
    private bool increaseMaxHP = false;

    [SerializeField]
    private int increaseScore = 100;

    [SerializeField] 
    private InGameMenu _gameMenu;

    [SerializeField]
    private int itemWeight = 0;

    private Transform playerTransform;
    private FighterStats playerStats;

    public int ItemWeight { get => itemWeight; set => itemWeight = value; }
    public InGameMenu GameMenu { get => _gameMenu; set => _gameMenu = value; }

    private void Update()
    {
        DetectPlayer();
        MoveTowardsPlayer();
    }

    private void DetectPlayer()
    {
        Collider2D playerCollider = Physics2D.OverlapCircle(transform.position, targetDetectionRange, playerLayerMask);
        if (playerCollider != null)
        {
            playerTransform = playerCollider.transform;
            playerStats = playerCollider.GetComponent<FighterStats>();
        }
        else
        {
            playerTransform = null;
        }
    }

    private void MoveTowardsPlayer()
    {
        if (playerTransform != null)
        {
            transform.position = Vector2.MoveTowards(transform.position, playerTransform.position, attractionSpeed * Time.deltaTime);
        }
    }

    public void PickedUp()
    {
        if (playerTransform != null)
        {
            if (increaseMaxHP)
            {
                playerStats.AddHealth(increaseAmount);
            }
            else
            {
                playerStats.Heal(increaseAmount);
            }
            _gameMenu.AddScore = increaseScore;
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")){
            PickedUp();
        }
    }
}
