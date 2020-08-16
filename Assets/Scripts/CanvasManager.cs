using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{

    public static Button interactButton;
    public static Button SwitchWeaponButton;
    public Image SwitchWeaponButtonImage;
    public Joystick shootJoystick;
    public Text obtainedWeaponsList;
    SpriteRenderer weaponGFXSR;

    // Start is called before the first frame update
    void Start()
    {
        interactButton = gameObject.transform.Find("Interact").GetComponent<Button>();
        SwitchWeaponButton = gameObject.transform.Find("Switch Weapon").GetComponent<Button>();
        SwitchWeaponButtonImage = SwitchWeaponButton.transform.Find("Image").GetComponent<Image>();

        Invoke("switchImage", 0.01f);
    }

    public void switchImage()
    {
        SwitchWeaponButtonImage.sprite = WeaponHandler.weaponGFXSR.sprite;
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
