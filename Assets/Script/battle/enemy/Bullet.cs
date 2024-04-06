using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    //初始生成点
    public Vector3 startPoint;
    //飞行向量
    public Vector3 flyDir;
    //目标落点
    public Vector3 pointVec;

    public float speed = 5;

    public int attack;

    public float life;

    float now_duration;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        now_duration += Time.deltaTime;

        if (now_duration > life) {
            Destroy(this.gameObject);
        }

        this.transform.position += flyDir.normalized * Time.deltaTime * speed;
    }

    void OnTriggerEnter2D(Collider2D Collider)
    {
        if (Collider.gameObject.tag == "player")
        {
            Player p = Collider.gameObject.GetComponent<Player>();
            HitInfo ht = new HitInfo();
            ht.hitPos = transform.position;
            ht.damage = this.attack;
            p.hurt(ht);
            Destroy(this.gameObject);
        }
    }
}
