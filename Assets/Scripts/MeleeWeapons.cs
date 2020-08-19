using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Melee Weapon", menuName = "Melee Weapon")]

public class MeleeWeapons : ScriptableObject, MainWeapon
{
    public new string name;
    public float damage;
    public float criticalChance;
    public float weaponKnockBack;
    public float meleeRange;
    public int attackSpeed;
    public Sprite artwork;
    public int energyCost;
    public GameObject meleeWeaponProjectile;
    public float meleeWeaponProjectileDamage;
    public float meleeWeaponProjectileSpeed;

    public float shootDelayTime;

}
public interface MainWeapon
{

}