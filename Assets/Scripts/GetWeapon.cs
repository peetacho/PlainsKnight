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
    public static List<MainWeapon> ow; // obtained weapons
    public static int cwIndex = 0; // current weapon index
    public int maxWeaponAmt;
    public List<string> scriptNames;

    // function that switches weapon by cycling through index
    public void switchWeapon()
    {
        maxWeaponAmt = ow.Count;

        cwIndex += 1;

        if (cwIndex >= maxWeaponAmt)
        {
            cwIndex = 0;
        }
    }

    // returns the next main weapon
    public MainWeapon getNextWeapon()
    {
        int max = maxWeaponAmt;
        int count = cwIndex;

        count += 1;

        if (count >= max)
        {
            count = 0;
        }
        return ow[count];
    }

    void Awake()
    {
        sr = transform.Find("WeaponGFX").Find("SR").GetComponent<SpriteRenderer>();
        box = GetComponent<BoxCollider2D>();
        ow = new List<MainWeapon>();
        scriptNames = new List<string>();
    }

    // Start is called before the first frame update
    void Start()
    {

        if (rw != null)
        {
            ow.Add(rw);
            initUniqueScript();
        }
        if (mw != null)
        {
            ow.Add(mw);
            initUniqueScript();
        }
    }

    private void FixedUpdate()
    {
        // if there is something inside obtained weapons list
        if (ow.Count > 0)
        {
            initWeapon();
            initUniqueScript();
        }
    }

    public static weapontype GetWeapontype()
    {
        return currentWeaponType;
    }

    public void initWeapon()
    {

        var currentWeapon = ow[cwIndex];
        // print(currentWeapon);

        // sees if current weapon is of type rangedweapons
        if (currentWeapon.GetType() == typeof(RangedWeapons))
        {
            rw = (RangedWeapons)ow[cwIndex];
            initStatsR();
            currentWeaponType = weapontype.ranged;
        }
        else if (currentWeapon.GetType() == typeof(MeleeWeapons))
        {
            mw = (MeleeWeapons)ow[cwIndex];
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

    // gets unique script and adds it as component
    // called in update function
    public void initUniqueScript()
    {
        if ((ow[cwIndex].GetType() == typeof(RangedWeapons)) && (rw != null))
        {
            var newRw = (RangedWeapons)ow[cwIndex];
            if (newRw.uniqueScript != null)
            {
                var script = ObjectNames.GetDragAndDropTitle(newRw.uniqueScript).Replace(" (MonoScript)", "");
                // print(script);
                var thisScript = this.GetComponent(System.Type.GetType(script));

                // if this object does not have this script, it will add the script
                if (!thisScript)
                {
                    // add to script names list
                    scriptNames.Add(script);
                    uniqueScript = System.Type.GetType(script);
                    var comp = gameObject.AddComponent(uniqueScript);
                    Behaviour b = (Behaviour)comp;
                    // b.enabled = false;
                    toggleOtherUniqueWeapon();
                }
            }
        }
        else if (ow[cwIndex].GetType() == typeof(MeleeWeapons) && mw != null)
        {
            var newMw = (MeleeWeapons)ow[cwIndex];
            if (newMw.uniqueScript != null)
            {
                var script = ObjectNames.GetDragAndDropTitle(newMw.uniqueScript).Replace(" (MonoScript)", "");
                //print(script);
                var thisScript = this.GetComponent(System.Type.GetType(script));

                if (!thisScript)
                {
                    // add to script names list
                    scriptNames.Add(script);
                    uniqueScript = System.Type.GetType(script);
                    var comp = gameObject.AddComponent(uniqueScript);
                    Behaviour b = (Behaviour)comp;
                    // b.enabled = false;
                    toggleOtherUniqueWeapon();
                }
            }
        }
    }

    // switches between unique scripts by disable/enable
    // called when switch button is pressed
    public void switchUniqueScripts()
    {

        if ((ow[cwIndex].GetType() == typeof(RangedWeapons)) && (rw != null))
        {
            var newRw = (RangedWeapons)ow[cwIndex];
            if (newRw.uniqueScript != null)
            {
                // var script = ObjectNames.GetDragAndDropTitle(newRw.uniqueScript).Replace(" (MonoScript)", "");
                toggleOtherUniqueWeapon();
            }
        }
        else if (ow[cwIndex].GetType() == typeof(MeleeWeapons) && mw != null)
        {
            var newMw = (MeleeWeapons)ow[cwIndex];
            if (newMw.uniqueScript != null)
            {
                // var script = ObjectNames.GetDragAndDropTitle(newMw.uniqueScript).Replace(" (MonoScript)", "");
                toggleOtherUniqueWeapon();
            }
        }
    }

    public void toggleOtherUniqueWeapon()
    {

        string currentWeaponName = ow[cwIndex].ToString();
        // print("currentWeaponName " + currentWeaponName);
        string currentWeaponScript = "";

        if (ow[cwIndex].GetType() == typeof(MeleeWeapons) && mw != null)
        {
            var newMw = (MeleeWeapons)ow[cwIndex];
            if (newMw.uniqueScript != null)
            {
                currentWeaponScript = ObjectNames.GetDragAndDropTitle(newMw.uniqueScript).Replace(" (MonoScript)", "");
            }
        }
        else if (ow[cwIndex].GetType() == typeof(RangedWeapons) && rw != null)
        {
            var newRw = (RangedWeapons)ow[cwIndex];
            if (newRw.uniqueScript != null)
            {

                currentWeaponScript = ObjectNames.GetDragAndDropTitle(newRw.uniqueScript).Replace(" (MonoScript)", "");
            }
        }

        foreach (string s in scriptNames)
        {
            if (s == currentWeaponScript)
            {
                toggleUniqueWeapon(s, true);
            }
            else
            {
                toggleUniqueWeapon(s, false);
            }


            // weapons in obtained weapons list
            // if the current looped item is not the current weapon, then disable it
            // if (w.ToString().Contains(currentWeaponName))
            // {
            //     toggleUniqueWeapon(w, true);
            // }
            // else
            // {
            //     toggleUniqueWeapon(w, false);
            // }
        }
        // }
    }

    // toggles the mainweapon cw (inidividual)
    public void toggleUniqueWeapon(string s, bool value)
    {
        var newComp = gameObject.GetComponent(System.Type.GetType(s));
        Behaviour newB = (Behaviour)newComp;

        newB.enabled = value;


        // if (cw.GetType() == typeof(MeleeWeapons) && mw != null)
        // {
        //     var newMw = (MeleeWeapons)cw;
        //     if (newMw.uniqueScript != null)
        //     {
        //         var newScript = ObjectNames.GetDragAndDropTitle(newMw.uniqueScript).Replace(" (MonoScript)", "");

        //         var newComp = gameObject.GetComponent(System.Type.GetType(newScript));
        //         Behaviour newB = (Behaviour)newComp;
        //         newB.enabled = value;
        //     }
        // }
        // else if (cw.GetType() == typeof(RangedWeapons) && rw != null)
        // {
        //     var newRw = (RangedWeapons)cw;
        //     if (newRw.uniqueScript != null)
        //     {

        //         var newScript = ObjectNames.GetDragAndDropTitle(newRw.uniqueScript).Replace(" (MonoScript)", "");

        //         var newComp = gameObject.GetComponent(System.Type.GetType(newScript));
        //         Behaviour newB = (Behaviour)newComp;

        //         newB.enabled = value;
        //     }
        // }
    }

}
