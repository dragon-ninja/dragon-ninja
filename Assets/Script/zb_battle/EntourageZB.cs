using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntourageZB : MonoBehaviour
{

    Player player;
    // 队形偏移
    public Vector2 formationOffset;

    public int index;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("role").GetComponent<Player>();
        player.EntourageList.Add(this);
        index = player.EntourageList.Count;
    }

    public void Update()
    {
        Vector2 v2 =new Vector2(-player.facingDirection, -player.updownDirection /*player.transform.position.y> transform.position.y?-1:1*/);
        // 计算队形偏移
        Vector3 offset = formationOffset * index * v2;
        float jl = Vector2.Distance(transform.position, player.transform.position + offset);
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position + offset, 3 * (jl/5) * Time.deltaTime);
    }




}
/*public class Leader : MonoBehaviour
{
    // 领导者的移动速度
    public float speed = 5f;

    // 领导者的目标位置
    public Vector3 targetPosition;

    // 队形偏移
    public Vector3 formationOffset;

    void Update()
    {
        // 计算队形偏移
        Vector3 offset = formationOffset * (transform.childCount - 1);

        // 领导者根据目标位置和队形偏移进行移动
        transform.position = Vector3.MoveTowards(transform.position, targetPosition + offset, speed * Time.deltaTime);

        // 更新跟随者的目标位置
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform follower = transform.GetChild(i);
            follower.GetComponent<Follower>().targetPosition = targetPosition + formationOffset * i;
        }
    }
}*/