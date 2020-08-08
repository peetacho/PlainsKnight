using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetWeapon : MonoBehaviour
{
    public MeleeWeapons mw;
    SpriteRenderer sr;
    BoxCollider2D box;

    [Header("Weapon Stats:")]
    public string weaponName;
    public static int weaponDamage;
    public static float weaponCriticalChance;
    public int attackSpeed;
    public static float attackRangeX;
    public static float attackRangeY;

    void Awake()
    {
        sr = transform.Find("WeaponGFX").GetComponent<SpriteRenderer>();
        box = GetComponent<BoxCollider2D>();

    }

    // Start is called before the first frame update
    void Start()
    {
        initStats();
    }

    public void initStats()
    {
        sr.sprite = mw.artwork;
        weaponName = mw.name;
        weaponDamage = mw.damage;
        weaponCriticalChance = mw.criticalChance;
        attackSpeed = mw.attackSpeed;
        attackRangeX = mw.rangeX;
        attackRangeY = mw.rangeY;

    }

}
