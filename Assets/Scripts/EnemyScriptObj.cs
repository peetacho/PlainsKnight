using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "Enemy")]
public class EnemyScriptObj : ScriptableObject
{

    [Header("Important Info:")]
    public new string name;
    public int maxHealth;
    public float meleeDamage;
    public float rangedDamage;
    public int attackSpeed;
    public int movementSpeed;

    public float colliderRadius;


    [Header("Spirte and Animator Controller:")]
    public Sprite artwork;
    public RuntimeAnimatorController controller;

}