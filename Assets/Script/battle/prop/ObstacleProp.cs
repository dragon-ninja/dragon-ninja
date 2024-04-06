using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleProp : MonoBehaviour
{
    bool hide;

    void OnTriggerEnter2D(Collider2D Collider)
    {
        if (Collider.gameObject.tag == "player" && !hide)
        {
            hide = true;
            transform.Find("sprite").gameObject.SetActive(false);
            StartCoroutine(DungeonManager.creatObstacle("obstacle_0",this.gameObject));
        }
    }

}
