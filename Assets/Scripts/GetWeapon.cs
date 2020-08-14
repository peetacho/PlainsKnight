using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetWeapon : MonoBehaviour
{
    public MeleeWeapons mw;
    public RangedWeapons rw;
    SpriteRenderer sr;
    BoxCollider2D box;

    [Header("Melee Weapon Stats:")]
    public string weaponNameM;
    public static float weaponDamageM;
    public static float weaponCriticalChanceM;
    public static float weaponKnockBackM;
    public static int attackSpeedM;
    public static float attackRangeXM;
    public static float attackRangeYM;

    [Header("Ranged Weapon Stats:")]
    public static string weaponNameR;
    public static float weaponDamageR;
    public static float weaponCriticalChanceR;
    public static float weaponKnockBackR;
    public static int attackSpeedR;
    public static int manaCostR;
    public static float shootDelayTimeR;
    public static GameObject weaponProjectileR;



    void Awake()
    {
        sr = transform.Find("WeaponGFX").GetComponent<SpriteRenderer>();
        box = GetComponent<BoxCollider2D>();

    }

    // Start is called before the first frame update
    void Start()
    {

        if (rw != null)
        {
            // print("rw not null");
            initStatsR();
        }

        if (mw != null)
        {
            // print("mw not null");
            initStatsM();
        }

    }

    public void initStatsM()
    {
        sr.sprite = mw.artwork;
        weaponNameM = mw.name;
        weaponDamageM = mw.damage;
        weaponCriticalChanceM = mw.criticalChance;
        weaponKnockBackM = mw.weaponKnockBack;
        attackSpeedM = mw.attackSpeed;
        attackRangeXM = mw.rangeX;
        attackRangeYM = mw.rangeY;
    }
    public void initStatsR()
    {
        sr.sprite = rw.artwork;
        weaponNameR = rw.name;
        weaponDamageR = rw.damage;
        weaponCriticalChanceR = rw.weaponCriticalChance;
        weaponKnockBackR = rw.weaponKnockBack;
        attackSpeedR = rw.attackSpeed;
        manaCostR = rw.manaCost;
        weaponProjectileR = rw.rangedWeaponProjectile;
        shootDelayTimeR = rw.shootDelayTime;

    }

}
