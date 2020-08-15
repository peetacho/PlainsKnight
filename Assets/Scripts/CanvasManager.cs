using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{

    public static Button interactButton;
    public static Button SwitchWeaponButton;
    public Joystick shootJoystick;

    // Start is called before the first frame update
    void Start()
    {
        interactButton = gameObject.transform.Find("Interact").GetComponent<Button>();
        SwitchWeaponButton = gameObject.transform.Find("Switch Weapon").GetComponent<Button>();
    }

    public void toggleInteract(bool isInteract)
    {
        // print("interact button toggled!");
        interactButton.gameObject.SetActive(isInteract);
    }

}
