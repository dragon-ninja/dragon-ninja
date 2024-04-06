using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockUtil
{
    public static Transform lockTarget(Transform self, float size, bool randomFlag = false)
    {
        GameObject[] enemys = GameObject.FindGameObjectsWithTag("enemy");
        float minDis = size;
        GameObject target = null;
        List<GameObject> inViewEnemys = new List<GameObject>();

        //剔除屏幕外的目标
        foreach (var enemy in enemys) {
            if (isInView(enemy.transform.position)) {
                inViewEnemys.Add(enemy);
            }
        }

        if (inViewEnemys.Count > 0) {
            if (randomFlag)
            {
                target = inViewEnemys[Random.Range(0, inViewEnemys.Count)];
            }
            else {
                foreach (var enemy in inViewEnemys)
                {
                    //找到最近的目标
                    float dis = Vector2.Distance(self.position, enemy.transform.position);
                    if (dis < minDis)
                    {
                        minDis = dis;
                        target = enemy;
                    }
                }
            }
        }

        if(target == null)
            return null;
        else
            return target.transform;
    }


    //找到屏幕内目标点最近的x个目标 不重复
    public static List<GameObject> lockTargets(Transform self, float size,int num = 1)
    {
        GameObject[] enemys = GameObject.FindGameObjectsWithTag("enemy");
        List<GameObject> inViewEnemys = new List<GameObject>();

        //剔除屏幕外的目标
        foreach (GameObject enemy in enemys)
        {
            if (isInView(enemy.transform.position))
            {
                inViewEnemys.Add(enemy);
            }
        }

        List<GameObject> targets = new List<GameObject>();

        string fs1 = "";
        string fs2 = "";

        if (inViewEnemys.Count > 0)
        {
            foreach (GameObject enemy in inViewEnemys)
            {
                if (enemy == self.gameObject)
                    continue;

                if (Vector2.Distance(self.position, enemy.transform.position)> size)
                    continue;


                if (targets.Count < num)
                {
                    targets.Add(enemy);
                }
                else {
                    //挑出距离最远的一个替换
                    float maxDis = 0;
                    int index = 0;
                    foreach (GameObject t in targets) {
                        float dis = Vector2.Distance(self.position, t.transform.position);
                        if (dis > maxDis) {
                            maxDis = dis;
                            index = targets.IndexOf(t);
                        }
                    }
                    if (maxDis != 0) {
                        float dis = Vector2.Distance(self.position, enemy.transform.position);
                        if (dis < maxDis) {
                            targets.RemoveAt(index);
                            targets.Add(enemy);
                        }
                    }
                }
            }
        }

        /*Debug.Log("==========================================start:");
        foreach (var t in inViewEnemys)
        {
            fs1 += " | " + Vector2.Distance(self.position, t.transform.position);
        }
        foreach (var t in targets) { 
            fs2+= " | " + Vector2.Distance(self.position, t.transform.position);
        }
        Debug.Log(fs1);
        Debug.Log(fs2);

        Debug.Log("==========================================end:");*/
        return targets;
    }

    public static List<Transform> lockAll(Transform self, float size)
    {
        GameObject[] enemys = GameObject.FindGameObjectsWithTag("enemy");
        List<Transform> Enemys = new List<Transform>();
        foreach (var enemy in enemys)
        {
            //找到附近的目标
            float dis = Vector2.Distance(self.position, enemy.transform.position);
            if (dis < size)
            {
                Enemys.Add(enemy.transform);
            }
        }
        return Enemys;
    }



    public static bool isInView(Vector3 worldPos)
     {
        Transform camTransform = Camera.main.transform;
        Vector2 viewPos = Camera.main.WorldToViewportPoint(worldPos);
        Vector3 dir = (worldPos - camTransform.position).normalized;
        float dot = Vector3.Dot(camTransform.forward, dir);//判断物体是否在相机前面

        if (dot > 0 && viewPos.x >= -0.1f && viewPos.x <= 1 && viewPos.y >= -0.1f && viewPos.y <= 1) return true;
        else return false;
     }



    //计算点a绕着点b旋转x角度后的坐标  用于计算转盘位置
    //pos1中心点   pos2旋转原点    ARotate角度   返回旋转后的目标点
    public static Vector2 RotateAngle(Vector2 pos1, Vector2 pos2, float ARotate)
    {
        float Rad = 0;
        Rad = ARotate * Mathf.Acos(-1) / 180;
        float XAfter = (pos2.x - pos1.x) * Mathf.Cos(Rad) - (pos2.y - pos1.y) * Mathf.Sin(Rad) + pos1.x;
        float YAfter = (pos2.y - pos1.y) * Mathf.Cos(Rad) + (pos2.x - pos1.x) * Mathf.Sin(Rad) + pos1.y;
        return  new Vector2(XAfter , YAfter);
      
    }
    


}
