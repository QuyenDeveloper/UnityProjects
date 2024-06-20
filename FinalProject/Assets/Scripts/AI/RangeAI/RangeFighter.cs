using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RangeFighter : MonoBehaviour
{

    private Vector2 movementDirection, pointerDirection;
    private RangeWeaponParent rangeWeaponParent;
    private MoveEntity moveEntity;
    public Vector2 PointerDirection { get => pointerDirection; set => pointerDirection = value; }
    public Vector2 MovementDirection { get => movementDirection; set => movementDirection = value; }
    private void Awake()
    {
        rangeWeaponParent = GetComponentInChildren<RangeWeaponParent>();
        moveEntity = GetComponent<MoveEntity>();
    }
    void Update()
    {
        moveEntity.MovementDirection = movementDirection;
        rangeWeaponParent.PointerPosition = pointerDirection;
    }

    public void PerformAttack()
    {
        rangeWeaponParent.Attack();
    }
}
