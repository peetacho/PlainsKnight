using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Melee Weapon", menuName = "Melee Weapon")]
public class MeleeWeapons : ScriptableObject
{
    public new string name;
    public int damage;
    public float criticalChance;
    public int attackSpeed;
    public Sprite artwork;

    [Header("Range x/y for position of the weapon.")]
    public float rangeX;
    public float rangeY;

}