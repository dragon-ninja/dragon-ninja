using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LuckyBox : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnTriggerEnter2D(Collider2D Collider)
    {
        if (Collider.gameObject.tag == "player")
        {
            Destroy(this.gameObject);
            GameObject.Find("Canvas").transform.Find("LuckyCrystal").
                GetComponent<LuckyCrystal>().show();
        }
    }
}
