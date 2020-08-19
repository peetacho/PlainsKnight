using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Ranged Weapon", menuName = "Ranged Weapon")]
public class RangedWeapons : ScriptableObject, MainWeapon
{
    public new string name;
    public int damage;
    public float weaponCriticalChance;
    public float weaponKnockBack;
    public int attackSpeed;
    public Sprite artwork;
    public int energyCost;
    public float shootDelayTime;
    public GameObject rangedWeaponProjectile;

    [Header("Unique script:")]
    public Object uniqueScript;

}