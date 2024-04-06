using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DlySkillBox : MonoBehaviour
{
    public DlySkill dlyskill;
    public int attack = 25;
    public float attackInterval;
    Collider2D box;
    float dmgTime;

    private void Awake()
    {
        box = GetComponent<Collider2D>();
    }

    private void Update()
    {
        /* dmgTime -= Time.deltaTime;

        if (box != null)
        {
            if (!box.enabled)
                box.enabled = true;

            //重启box 重新激活碰撞事件
            if (dmgTime <= 0)
            {
                dmgTime = attackInterval;
                box.enabled = false;
            }
        }*/

        //RaycastHit2D[]  = Physics2D.RaycastAll(transform.position, Vector2.up, 0.5f);
    }

    //常规情况下直接碰撞
    void OnTriggerEnter2D(Collider2D Collider)
    {
        /*if (Collider.gameObject.tag == "enemy")
        {
            Enemy e = Collider.gameObject.GetComponent<Enemy>();
            e.hurt(this.attack);
        }*/

        //碰撞建筑   规则 只有按顺序来才行  可以逆序
        if (Collider.gameObject.tag == "obstacle")
        {
            Obstacle e = Collider.gameObject.GetComponent<Obstacle>();

            //必须从起点或者终点开始激活
            if (dlyskill.startIndex == -1)
            {
                if ((e.index == 0 || e.index == e.maxNum - 1))
                {
                    dlyskill.startIndex = e.index;
                    
                    if (dlyskill.startIndex == 0)
                        dlyskill.nextIndex = e.index + 1;
                    else
                        dlyskill.nextIndex = e.index - 1;
                    
                    e.activeFlag = true;
                }
            }
            else if(dlyskill.startIndex !=-1 && e.index == dlyskill.nextIndex)
            {
                if (dlyskill.startIndex == 0)
                {
                    dlyskill.nextIndex = e.index + 1;
                    e.changeLine();
                    //判定结束
                    if (e.index == e.maxNum-1) {
                        dlyskill.nowDuration = 0;
                    }
                }
                else { 
                    dlyskill.nextIndex = e.index - 1;
                    e.nextObstacle.changeLine();
                    //判定结束
                    if (e.index == 0)
                    {
                        dlyskill.nowDuration = 0;
                    }

                }
                e.activeFlag = true;
            }
        }
    }

}
