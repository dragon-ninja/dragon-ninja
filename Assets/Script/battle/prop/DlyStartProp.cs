using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DlyStartProp : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D Collider)
    {
        if (Collider.gameObject.tag == "player")
        {
            DlySkill DlySkill = Collider.gameObject.GetComponent<DlySkill>();
            DlySkill.dlyAckStart();
            Destroy(this.gameObject);
        }
    }
}
