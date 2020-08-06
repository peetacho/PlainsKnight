using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetWeapon : MonoBehaviour
{
    public MeleeWeapons meleeWeapons;
    Rigidbody2D rb;
    SpriteRenderer sr;
    BoxCollider2D box;
    // public LayerMask enemyLayers;
    public static float attackRangeX;
    public static float attackRangeY;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        box = GetComponent<BoxCollider2D>();

    }

    // Start is called before the first frame update
    void Start()
    {
        sr.sprite = meleeWeapons.artwork;
        attackRangeX = meleeWeapons.rangeX;
        attackRangeY = meleeWeapons.rangeY;
        // box.offset = meleeWeapons.boxOffset;
        // box.size = meleeWeapons.boxSize;
    }

    // void OnTriggerEnter2D(Collider2D other)
    // {
    //     if (other.tag == "Enemy")
    //     {
    //         Debug.Log("Enemy Hit!");
    //     }
    // }

}
