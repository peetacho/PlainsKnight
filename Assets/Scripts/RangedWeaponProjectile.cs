using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Ranged Weapon Projectile", menuName = "Ranged Weapon Projectile")]
public class RangedWeaponProjectile : ScriptableObject
{
    public new string name;
    public int damage;
    public int projectileSpeed;
    public Sprite artwork;

}