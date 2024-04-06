using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpUI : MonoBehaviour
{
    public Player player;
    RectTransform value_rtra;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("role").GetComponent<Player>();
        value_rtra = transform.Find("value").GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        //transform.position = player.transform.position + new Vector3(0.1f,-1,0);
        value_rtra.sizeDelta = new Vector2(1.16f * player.hp_now / player.hp_max, 0.15f);
    }
}



