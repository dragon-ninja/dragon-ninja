using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnet : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D Collider)
    {
        if (Collider.gameObject.tag == "player")
        {
            DungeonManager.MagnetFlag = true;
           /* GameObject[] exps = GameObject.FindGameObjectsWithTag("exp");
            foreach (var expObj in exps) {
                expObj.GetComponent<expCrystal>().absorb();
            }*/
            Destroy(this.gameObject);
        }
    }
}
