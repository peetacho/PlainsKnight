using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "Enemy")]
public class EnemyScriptObj : ScriptableObject
{

    [Header("Stats:")]
    public float maxHealth;
    public string enemyName;
    public int meleeDamage;
    public int rangedDamage;
    public float rangedAttackSpeed;
    public float attackDelayTime;
    public float attackRange;
    public float movementSpeed;
    public float colliderRadius;
    public GameObject projectile;
    public bool enemyIsRanged;
    public float enemyViewRange;

    [Header("Unique script:")]
    public Object uniqueScript;


    [Header("Spirte and Animator Controller:")]
    public Sprite artwork;
    public RuntimeAnimatorController controller;

}