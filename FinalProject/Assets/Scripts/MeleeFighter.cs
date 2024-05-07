using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Fighter : MonoBehaviour
{

    private Vector2 movementDirection, pointerDirection;
    private WeaponParent weaponParent;
    private MoveEntity moveEntity;
    public Vector2 PointerDirection { get => pointerDirection; set => pointerDirection = value; }
    public Vector2 MovementDirection { get => movementDirection; set => movementDirection = value; }
    

    private void Awake()
    {
        weaponParent = GetComponentInChildren<WeaponParent>();
        moveEntity = GetComponent<MoveEntity>();
    }
    void Update()
    {
        moveEntity.MovementDirection = movementDirection;
        weaponParent.PointerPosition = pointerDirection;
    }

    public void PerformAttack()
    {
        if (Time.timeScale != 0)
        {
            weaponParent.Attack();
        }
    }
}
