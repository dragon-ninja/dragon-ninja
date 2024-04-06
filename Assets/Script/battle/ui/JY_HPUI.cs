using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JY_HPUI : MonoBehaviour
{
    public Enemy enemy;
    RectTransform value_rtra;

    // Start is called before the first frame update
    void Awake()
    {
        value_rtra = transform.Find("value").GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = enemy.transform.position + new Vector3(0, 1.8f, 0);
        value_rtra.sizeDelta = new Vector2(1.5f * enemy.hp_now / enemy.hp, 0.2f);
    }
}



