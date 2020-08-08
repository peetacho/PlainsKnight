using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Popup : MonoBehaviour
{
    public float disappearTimer;
    public float moveYSpeed = 0.3f;
    Color textColor;

    private TextMeshPro tm;
    // Start is called before the first frame update
    void Awake()
    {
        tm = GetComponent<TextMeshPro>();
    }

    // Update is called once per frame
    void Update()
    {

        transform.position += new Vector3(0, moveYSpeed) * Time.deltaTime;

        disappearTimer -= Time.deltaTime;
        if (disappearTimer < 0)
        {
            float disappearSpeed = 3f;
            textColor.a -= disappearSpeed * Time.deltaTime;
            tm.faceColor = textColor;

            if (textColor.a < 0)
            {
                Destroy(gameObject);
            }
            // print(textColor);
        }
    }

    // static method to create a popup
    public static Popup Create(Vector3 position, float damage, Color32 color)
    {
        // finds and gets popup prefab in resources folder
        Transform newPopup = (Transform)Resources.Load("Prefabs/damagePopup", typeof(Transform));

        // instantiates popup
        Transform popupTransform = Instantiate(newPopup, position, Quaternion.identity);
        Popup popup = popupTransform.GetComponent<Popup>();

        // sets up the pop up
        popup.Setup(damage, color);

        return popup;
    }

    public void Setup(float damage, Color32 color)
    {
        textColor = color;
        tm.faceColor = color;
        tm.SetText(damage.ToString());
        disappearTimer = 1.0f;
    }
}
