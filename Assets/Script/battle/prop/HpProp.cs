using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpProp : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D Collider)
    {
        if (Collider.gameObject.tag == "player")
        {
            Player player = Collider.gameObject.GetComponent<Player>();

            Prop p = PropFactory.Get().PropMap["prop_1"];
            player.hp_now += (int)Mathf.Clamp(player.hp_max * (p.effect / 10000.0f), 1, player.hp_max);
            player.hp_now = Mathf.Min(player.hp_now , player.hp_max);

            //Debug.Log("huixue:" +((int)Mathf.Clamp(player.hp_max * (p.effect / 10000.0f), 1, player.hp_max)));
            HitInfo hf = new HitInfo();
            hf.damage = (int)Mathf.Clamp(player.hp_max * (p.effect / 10000.0f),1, player.hp_max);
            hf.hurtPos = transform.position;
            hf.cureFlag = true;
            DamageUIManage.creatureDmgUI(hf);
            Destroy(this.gameObject);
        }
    }
}
