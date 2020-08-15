using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Melee Weapon", menuName = "Melee Weapon")]
public class MeleeWeapons : ScriptableObject
{
    public new string name;
    public float damage;
    public float criticalChance;
    public float weaponKnockBack;
    public int attackSpeed;
    public Sprite artwork;
    public GameObject meleeWeaponProjectile;
    public float meleeWeaponProjectileDamage;
    public float shootDelayTime;

    [Header("Range x/y for position of the weapon.")]
    public float rangeX;
    public float rangeY;

}