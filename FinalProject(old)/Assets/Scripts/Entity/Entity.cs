using System;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [SerializeField] private bool blocksMovement;
    public bool BlocksMovement { get => blocksMovement; set => blocksMovement = value; }

    public void Move(Vector2 direction)
    {
        transform.position += (Vector3)direction;
    }
    public void AddToGameManager()
    {
        GameManager.instance.Entities.Add(this);
    }
}
