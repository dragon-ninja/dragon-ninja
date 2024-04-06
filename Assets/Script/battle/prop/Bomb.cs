using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D Collider)
    {
        if (Collider.gameObject.tag == "player")
        {
            GameObject[] exps = GameObject.FindGameObjectsWithTag("enemy");
            foreach (var expObj in exps)
            {
                if(LockUtil.isInView(expObj.transform.position))
                    expObj.GetComponent<Enemy>().hurt(500);
            }
            Destroy(this.gameObject);
        }
    }
}
