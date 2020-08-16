using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniqueSwordSlash : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        // destroys enemy projectile with sword projectile
        if (other.gameObject.tag == "Projectile" && other.gameObject.layer == 9)
        {
            gameObject.SetActive(false);
        }
    }
}
