using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GetWeapon : MonoBehaviour
{
    public MeleeWeapons mw;
    public RangedWeapons rw;
    public static SpriteRenderer sr;
    BoxCollider2D box;

    [Header("Melee Weapon Stats:")]
    public static string weaponNameM;
    public static float weaponDamageM;
    public static float weaponCriticalChanceM;
    public static float weaponKnockBackM;
    public static float attackRangeM;
    public static int attackSpeedM;
    public static int energyCostM;
    public static GameObject weaponProjectileM;
    public static float weaponProjectileDamageM;
    public static float weaponProjectileSpeedM;
    public static float shootDelayTimeM;

    [Header("Ranged Weapon Stats:")]
    public static string weaponNameR;
    public static float weaponDamageR;
    public static float weaponCriticalChanceR;
    public static float weaponKnockBackR;
    public static int attackSpeedR;
    public static int energyCostR;
    public static float shootDelayTimeR;
    public static GameObject weaponProjectileR;

    [Header("Other Important Variables:")]
    public System.Type uniqueScript;
    // this static weapontype variable tells wether the current weapon is ranged or melee
    public static weapontype currentWeaponType;
    public enum weapontype { ranged, melee };
    public static List<MainWeapon> obtainedWeapons;
    public static int currentWeaponIndex = 0;
    public int maxWeaponAmt;

    // function that switches weapon by cycling through index
    public void switchWeapon()
    {
        maxWeaponAmt = obtainedWeapons.Count;

        currentWeaponIndex += 1;

        if (currentWeaponIndex >= maxWeaponAmt)
        {
            currentWeaponIndex = 0;
        }
    }

    void Awake()
    {
        sr = transform.Find("WeaponGFX").Find("SR").GetComponent<SpriteRenderer>();
        box = GetComponent<BoxCollider2D>();
        obtainedWeapons = new List<MainWeapon>();
    }

    // Start is called before the first frame update
    void Start()
    {

        if (rw != null)
        {
            obtainedWeapons.Add(rw);
            initUniqueScript();
        }
        if (mw != null)
        {
            obtainedWeapons.Add(mw);
            initUniqueScript();
        }
    }

    private void FixedUpdate()
    {
        // if there is something inside obtained weapons list
        if (obtainedWeapons.Count > 0)
        {
            initWeapon();
        }
    }

    public static weapontype GetWeapontype()
    {
        return currentWeaponType;
    }

    public void initWeapon()
    {

        var currentWeapon = obtainedWeapons[currentWeaponIndex];
        // print(currentWeapon);

        // sees if current weapon is of type rangedweapons
        if (currentWeapon.GetType() == typeof(RangedWeapons))
        {
            rw = (RangedWeapons)obtainedWeapons[currentWeaponIndex];
            initStatsR();
            currentWeaponType = weapontype.ranged;
        }
        else if (currentWeapon.GetType() == typeof(MeleeWeapons))
        {
            mw = (MeleeWeapons)obtainedWeapons[currentWeaponIndex];
            initStatsM();
            currentWeaponType = weapontype.melee;
        }

    }

    // initializes melee weapon stats
    // when called, function changes the weapon
    public void initStatsM()
    {
        sr.sprite = mw.artwork;
        weaponNameM = mw.name;
        weaponDamageM = mw.damage;
        weaponCriticalChanceM = mw.criticalChance;
        weaponKnockBackM = mw.weaponKnockBack;
        attackSpeedM = mw.attackSpeed;
        energyCostM = mw.energyCost;
        weaponProjectileM = mw.meleeWeaponProjectile;
        weaponProjectileDamageM = mw.meleeWeaponProjectileDamage;
        weaponProjectileSpeedM = mw.meleeWeaponProjectileSpeed;
        shootDelayTimeM = mw.shootDelayTime;
        attackRangeM = mw.meleeRange;
    }

    // initializes melee weapon stats
    // when called, function changes the weapon
    public void initStatsR()
    {
        sr.sprite = rw.artwork;
        weaponNameR = rw.name;
        weaponDamageR = rw.damage;
        weaponCriticalChanceR = rw.weaponCriticalChance;
        weaponKnockBackR = rw.weaponKnockBack;
        attackSpeedR = rw.attackSpeed;
        energyCostR = rw.energyCost;
        weaponProjectileR = rw.rangedWeaponProjectile;
        shootDelayTimeR = rw.shootDelayTime;
    }

    // gets unique script
    void initUniqueScript()
    {
        if (obtainedWeapons[currentWeaponIndex].GetType() == typeof(RangedWeapons) && rw.uniqueScript != null)
        {
            var script = ObjectNames.GetDragAndDropTitle(rw.uniqueScript).Replace(" (MonoScript)", "");
            // print(script);
            uniqueScript = System.Type.GetType(script);

            gameObject.AddComponent(uniqueScript);
        }
        else if (obtainedWeapons[currentWeaponIndex].GetType() == typeof(MeleeWeapons) && mw.uniqueScript != null)
        {
            var script = ObjectNames.GetDragAndDropTitle(mw.uniqueScript).Replace(" (MonoScript)", "");
            // print(script);
            uniqueScript = System.Type.GetType(script);

            gameObject.AddComponent(uniqueScript);
        }
        else
        {
            print("no script!" + gameObject.name);
        }
    }

}
