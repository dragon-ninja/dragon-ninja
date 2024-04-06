using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    bool flag;
    float time_now = 0;
    float time_max = 2;

    Transform c;

    private void Awake()
    {
        c = transform.GetChild(0);
    }

    private void Update()
    {
        if (flag) {
            time_now += Time.deltaTime;

            float f = Mathf.Min(1, time_now / time_max);

            c.localScale = new Vector3(f, f,1);

            if (time_now >= time_max) {
                flag = false;
                DungeonManager.bossEnd();
            }
        }
    }


    void OnTriggerEnter2D(Collider2D Collider)
    {
        if (Collider.gameObject.tag == "player")
        {
            flag = true;
            time_now = 0;
        }
    }

    void OnTriggerExit2D(Collider2D Collider)
    {
        if (Collider.gameObject.tag == "player")
        {
            flag = false;
            time_now = 0;
            c.localScale = new Vector3(0, 0, 1);
        }
    }
}
