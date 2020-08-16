using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHandler : MonoBehaviour
{

    // weapon prefab
    public GameObject weapon;
    public static GameObject weaponRangedPos;
    public static GameObject weaponGFX;
    public Transform weaponGFXTransform;

    // newly created weapons
    public static SpriteRenderer weaponGFXSR;
    GameObject newWeapon;
    string weaponCloneName;

    // weapon point
    private GameObject weaponPoint;

    private float boxOffsetX;

    private void Start()
    {
        // Get weapon point location
        weaponPoint = gameObject.transform.Find("PlayerGFX").Find("Weapon Point").gameObject;
        weaponCloneName = weapon.name + "(Clone)";
        fromAngle = new Vector2(1f, 1f);

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
            rotateWeapon();
        }

    }
    Vector2 fromAngle;
    float angle;
    Vector2 shootDirection;

    void rotateWeapon()
    {
        shootDirection = CharacterController2D.shootDirection;
        if (shootDirection != Vector2.zero)
        {
            weaponGFXSR.flipX = true;
            weaponGFXTransform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        }
        else
        {
            weaponGFXSR.flipX = false;
        }
        // determines the angle of the weapon relative to the shoot direction
        angle = Vector2.SignedAngle(fromAngle, shootDirection);
        weaponGFXTransform.transform.eulerAngles = new Vector3(0, 0, angle);
    }

    void flipWeapon()
    {
        // character to right
        if (CharacterController2D.movementDirection.x > 0f)
        {
            weaponGFXTransform.localScale = new Vector3(-0.5f, 0.5f, 0.5f);
        }
        // character to left
        else if (CharacterController2D.movementDirection.x < 0f)
        {
            weaponGFXTransform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        }
    }

    //Invoked when a button is pressed.
    public void SetParent()
    {

        // create new weapon
        newWeapon = Instantiate(weapon, weaponPoint.transform.position, Quaternion.Euler(transform.rotation.eulerAngles));
        weaponGFX = newWeapon.transform.Find("WeaponGFX").gameObject;
        weaponRangedPos = weaponGFX.transform.Find("RangedPos").gameObject;
        weaponGFXTransform = weaponGFX.transform;
        weaponGFXSR = weaponGFXTransform.Find("SR").GetComponent<SpriteRenderer>();

        //Makes the GameObject "newParent" the parent of the GameObject "player".
        newWeapon.transform.parent = gameObject.transform;
        weaponGFXTransform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

    }

    // public void DetachFromParent()
    // {
    //     // Detaches the transform from its parent.
    //     weapon.transform.parent = null;
    // }
}
