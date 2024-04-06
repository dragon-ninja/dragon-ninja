using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomMask : MonoBehaviour
{
    SpriteRenderer sp;
    public float speed;

    // Start is called before the first frame update：
    void Awake()
    {
        sp = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    private void OnEnable()
    {
        Color c = sp.color;
        c.a = 1;
        sp.color = c;
    }

    private void Update()
    {
        Color c = sp.color;
        c.a -= speed * Time.deltaTime;
        sp.color = c;

        if (c.a <= 0)
        {
            this.gameObject.SetActive(false);
        }
    }
}
