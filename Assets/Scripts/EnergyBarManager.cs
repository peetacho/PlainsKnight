using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyBarManager : MonoBehaviour
{
    private GameObject player;
    // private Animator playerAnim;
    private SpriteRenderer playerSR;
    private Slider slider;
    private Text energyNumber;

    [Header("Player Energy:")]
    public int maxEnergy;
    public int currentEnergy;
    public bool hasEnergy = true;

    // Start is called before the first frame update
    void Start()
    {

        // initialize player
        player = GameObject.FindGameObjectWithTag("Player");
        // playerAnim = player.GetComponent<Animator>();
        playerSR = player.transform.Find("PlayerGFX").GetComponent<SpriteRenderer>();

        if (!PlayerPrefs.HasKey("maxEnergy"))
        {
            PlayerPrefs.SetInt("maxEnergy", 20);
        }

        slider = GetComponent<Slider>();
        energyNumber = transform.Find("EnergyNumber").GetComponent<Text>();

        initHealthBar();
    }

    void initHealthBar()
    {
        maxEnergy = PlayerPrefs.GetInt("maxEnergy");
        slider.maxValue = maxEnergy;
        slider.value = maxEnergy;
        currentEnergy = maxEnergy;
        energyNumber.text = currentEnergy + "/" + maxEnergy;
    }

    public void getEnergy(string restore_use, int value)
    {

        if (restore_use == "restore")
        {
            currentEnergy += value;
        }
        else if (restore_use == "use")
        {
            currentEnergy -= value;
        }

        if (currentEnergy <= 0)
        {
            // makes sure current energy does not pass 0
            currentEnergy = 0;
            hasEnergy = false;
            // print("out of energy!");
        }
        else if (currentEnergy > 0)
        {
            hasEnergy = true;
        }

        if (currentEnergy > maxEnergy)
        {
            // makes sure current energy does not pass max
            currentEnergy = maxEnergy;
        }

        slider.value = currentEnergy;
        energyNumber.text = currentEnergy + "/" + maxEnergy;
    }
}
