using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniqueBlueSlime : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("slime!");

    }

    // Update is called once per frame
    void Update()
    {
    }

    // IEnumerator Idle()
    // {
    //     // int direction = 1;

    //     while (true)
    //     {
    //         // Move in a direction
    //         float dirX = Random.Range(-1.0f, 1.0f);
    //         float dirY = Random.Range(-1.0f, 1.0f);
    //         // print(dirX.ToString() + "    " + dirY.ToString());
    //         rb.velocity = (new Vector2(idleSpeed * dirX, idleSpeed * dirY));
    //         // print("idle");

    //         float wait = Random.Range(0.7f, 1.5f);

    //         // Wait
    //         yield return new WaitForSeconds(wait);
    //     }
    // }
}
