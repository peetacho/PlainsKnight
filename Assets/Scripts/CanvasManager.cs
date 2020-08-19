using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CanvasManager : MonoBehaviour
{

    public static Button interactButton;
    public static Button SwitchWeaponButton;
    public Image SwitchWeaponButtonImage;
    public Joystick shootJoystick;
    public GameObject SwitchWeaponButtonEnergy;
    public static TextMeshProUGUI SwitchWeaponButtonEnergyCost;
    SpriteRenderer weaponGFXSR;

    // Start is called before the first frame update
    void Start()
    {
        interactButton = gameObject.transform.Find("Interact").GetComponent<Button>();
        SwitchWeaponButton = gameObject.transform.Find("Switch Weapon").GetComponent<Button>();
        SwitchWeaponButtonImage = SwitchWeaponButton.transform.Find("Image").GetComponent<Image>();
        SwitchWeaponButtonEnergyCost = SwitchWeaponButtonEnergy.GetComponent<TextMeshProUGUI>();

        Invoke("switchImage", 0.01f);
    }

    public void switchImage()
    {
        GetWeapon gw = FindObjectOfType<GetWeapon>();

        // switch weapon index
        gw.switchWeapon();

        List<MainWeapon> ow = GetWeapon.obtainedWeapons;
        int cwIndex = GetWeapon.currentWeaponIndex;

        // sees if current weapon is of type rangedweapons
        if (ow[cwIndex].GetType() == typeof(RangedWeapons))
        {
            RangedWeapons rw = (RangedWeapons)ow[cwIndex];
            // change artwork
            SwitchWeaponButtonImage.sprite = rw.artwork;

            // change text
            SwitchWeaponButtonEnergyCost.text = rw.energyCost.ToString();
        }
        else if (ow[cwIndex].GetType() == typeof(MeleeWeapons))
        {
            MeleeWeapons mw = (MeleeWeapons)ow[cwIndex];
            SwitchWeaponButtonImage.sprite = mw.artwork;

            SwitchWeaponButtonEnergyCost.text = mw.energyCost.ToString();
        }
    }

    public void switchImageOnly()
    {
        List<MainWeapon> ow = GetWeapon.obtainedWeapons;
        int cwIndex = GetWeapon.currentWeaponIndex;

        // sees if current weapon is of type rangedweapons
        if (ow[cwIndex].GetType() == typeof(RangedWeapons))
        {
            RangedWeapons rw = (RangedWeapons)ow[cwIndex];
            // change artwork
            SwitchWeaponButtonImage.sprite = rw.artwork;

            // change text
            SwitchWeaponButtonEnergyCost.text = rw.energyCost.ToString();
        }
        else if (ow[cwIndex].GetType() == typeof(MeleeWeapons))
        {
            MeleeWeapons mw = (MeleeWeapons)ow[cwIndex];
            SwitchWeaponButtonImage.sprite = mw.artwork;

            SwitchWeaponButtonEnergyCost.text = mw.energyCost.ToString();
        }
    }

    private void Update()
    {

    }

    public void toggleInteract(bool isInteract)
    {
        // print("interact button toggled!");
        interactButton.gameObject.SetActive(isInteract);
    }

}
