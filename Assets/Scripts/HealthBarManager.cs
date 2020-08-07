using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarManager : MonoBehaviour
{

    public float maxHealth;
    public float currentHealth;
    public float UI_SPACING = 65.0f;
    public Image heart0;
    public Image heart1;
    public Image heart2;
    public Image heart3;
    public Image heart4;

    private Image newHeart;

    // Start is called before the first frame update
    void Start()
    {
        if (!PlayerPrefs.HasKey("maxHealth"))
        {
            PlayerPrefs.SetInt("maxHealth", 4);
        }
        maxHealth = PlayerPrefs.GetInt("maxHealth");
        currentHealth = maxHealth;
        getHearts("heal", 0.0f);
    }

    public void getHearts(string heal_damage, float value)
    {
        if (heal_damage == "heal")
        {
            currentHealth += value;
        }
        else if (heal_damage == "damage")
        {
            currentHealth -= value;
        }

        if (currentHealth < 0.25)
        {
            // makes sure current health does not pass 0, and ends game
            currentHealth = 0;
            print("Endgame");
        }

        if (currentHealth > maxHealth)
        {
            // makes sure current health does not pass max
            currentHealth = maxHealth;
        }

        int max = (int)(maxHealth);
        int curFloor = (int)(Mathf.Floor(currentHealth));
        float decMax = Mathf.Repeat(maxHealth, 1.0f);

        float difference = maxHealth - currentHealth;

        for (var i = 1; i <= curFloor; i++)
        {
            SetParent(heart0, getNewLocation(i));
        }

        if (currentHealth == maxHealth)
        {
            return;
        }

        float decCur = Mathf.Repeat(currentHealth, 1.0f);

        switch (decCur)
        {
            case 0.0f:
                SetParent(heart4, getNewLocation(curFloor + 1));
                break;
            case 0.25f:
                SetParent(heart3, getNewLocation(curFloor + 1));
                break;
            case 0.5f:
                SetParent(heart2, getNewLocation(curFloor + 1));
                break;
            case 0.75f:
                SetParent(heart1, getNewLocation(curFloor + 1));
                break;
        }

        for (var j = curFloor + 2; j <= maxHealth; j++)
        {
            SetParent(heart4, getNewLocation(j));
        }
    }

    Vector3 getNewLocation(int index)
    {

        return new Vector3(transform.position.x + (UI_SPACING * index), transform.position.y, transform.position.z);
    }

    public void SetParent(Image heart, Vector3 heartPos)
    {
        newHeart = Instantiate(heart, heartPos, Quaternion.identity);
        newHeart.transform.SetParent(gameObject.transform);

        // change size
        newHeart.transform.localScale = new Vector3(2, 2, 2);
    }

    // Update is called once per frame
    // private void Update()
    // {
    //     if (Input.touchCount > 0)
    //     {
    //         getHearts("heal", 0.25f);
    //     }
    // }

    public void resetHearts(string heal_damage, float value)
    {
        foreach (Transform t in gameObject.transform)
        {
            Destroy(t.gameObject);
        }
        getHearts(heal_damage, value);
    }
}
