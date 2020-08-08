using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHandler : MonoBehaviour
{

    // weapon prefab
    public GameObject weapon;

    // newly created weapons
    SpriteRenderer weaponSR;
    GameObject newWeapon;
    string weaponCloneName;

    // weapon point
    private GameObject weaponPoint;

    private float boxOffsetX;

    private void Start()
    {
        // Get weapon point location
        weaponPoint = gameObject.transform.Find("Weapon Point").gameObject;
        weaponCloneName = weapon.name + "(Clone)";

    }

    private void Update()
    {
        // allows only one object to be instantiated.
        if (!gameObject.transform.Find(weaponCloneName))
        {
            SetParent();
        }

        if (gameObject.transform.Find(weaponCloneName))
        {
            flipWeapon();
        }
    }

    void flipWeapon()
    {
        // weapon point to right
        if (CharacterController2D.movementDirection.x > 0f)
        {
            weaponSR.flipX = false;

        }
        // weaponpoint to left
        else if (CharacterController2D.movementDirection.x < 0f)
        {
            weaponSR.flipX = true;
        }
    }

    //Invoked when a button is pressed.
    public void SetParent()
    {
        // // allows only one object to be instantiated.
        // if (!gameObject.transform.Find(weaponCloneName))
        // {
        newWeapon = Instantiate(weapon, weaponPoint.transform.position, Quaternion.Euler(transform.rotation.eulerAngles));
        weaponSR = newWeapon.transform.Find("WeaponGFX").GetComponent<SpriteRenderer>();
        //Makes the GameObject "newParent" the parent of the GameObject "player".
        newWeapon.transform.parent = gameObject.transform;
        // }
    }

    // public void DetachFromParent()
    // {
    //     // Detaches the transform from its parent.
    //     weapon.transform.parent = null;
    // }
}
