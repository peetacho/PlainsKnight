using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarManager : MonoBehaviour
{
    private Image newHeart;
    private GameObject player;
    // private Animator playerAnim;
    private SpriteRenderer playerSR;
    private Slider slider;
    private Text healthNumber;

    [Header("Player Health:")]
    public int maxHealth;
    public int currentHealth;

    // [Header("UI Settings:")]
    // public float UI_SPACING = 65.0f;

    // [Header("Sprites and Artwork:")]
    // public Image heart0;
    // public Image heart1;
    // public Image heart2;
    // public Image heart3;
    // public Image heart4;

    // Start is called before the first frame update
    void Start()
    {

        // initialize player
        player = GameObject.FindGameObjectWithTag("Player");
        // playerAnim = player.GetComponent<Animator>();
        playerSR = player.transform.Find("PlayerGFX").GetComponent<SpriteRenderer>();

        if (!PlayerPrefs.HasKey("maxHealth"))
        {
            PlayerPrefs.SetInt("maxHealth", 20);
        }

        slider = GetComponent<Slider>();
        healthNumber = transform.Find("HealthNumber").GetComponent<Text>();

        initHealthBar();
    }

    void initHealthBar()
    {
        maxHealth = PlayerPrefs.GetInt("maxHealth");
        slider.maxValue = maxHealth;
        slider.value = maxHealth;
        currentHealth = maxHealth;
        healthNumber.text = currentHealth + "/" + maxHealth;
    }

    IEnumerator Hurt()
    {
        Color hurtColor = new Vector4(playerSR.color.r, playerSR.color.g, playerSR.color.b, 0.2f);
        playerSR.color = hurtColor;

        yield return new WaitForSeconds(0.45f);

        hurtColor = new Vector4(playerSR.color.r, playerSR.color.g, playerSR.color.b, 1.0f);
        playerSR.color = hurtColor;
    }

    public void getHearts(string heal_damage, int value)
    {
        print(value);
        if (heal_damage == "heal")
        {
            currentHealth += value;
        }
        else if (heal_damage == "damage")
        {
            currentHealth -= value;
            StartCoroutine(Hurt());

            FindObjectOfType<CameraShake>().Shake(0.1f * value, 0.1f);
        }

        if (currentHealth < 0)
        {
            // makes sure current health does not pass 0, and ends game
            currentHealth = 0;
            // print("Endgame");
        }

        if (currentHealth > maxHealth)
        {
            // makes sure current health does not pass max
            currentHealth = maxHealth;
        }

        // print(currentHealth);
        slider.value = currentHealth;
        healthNumber.text = currentHealth + "/" + maxHealth;

    }
}
