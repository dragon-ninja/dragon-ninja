using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DlyProp : MonoBehaviour
{
    // Start is called before the first frame update：
    void OnTriggerEnter2D(Collider2D Collider)
    {
        if (Collider.gameObject.tag == "player")
        {
            //触发dly模式
            //Collider.gameObject.GetComponent<DlySkill>().dlyAckStart();
            Player player = Collider.gameObject.GetComponent<Player>();
            if (!player.superAttackIng) {
                player.superAttackTiredTime = -1;
                Collider.gameObject.GetComponent<Player>().addDlyEs(0, true);
                Destroy(this.gameObject);
            }
        }
    }
}
