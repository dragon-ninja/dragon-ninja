using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatLine
{
    // 创建⼀条两点之间的线
    public  void CreateLine(Vector2 end, Vector2 start, GameObject line)
    {

        RectTransform rect = line.GetComponent<RectTransform>();

        //设置位置和⾓度

        rect.localPosition = GetBetweenPoint(start, end);

        rect.localRotation = Quaternion.AngleAxis(-GetAngle(start, end), Vector3.forward);

        //设置线段图⽚⼤⼩

        var distance = Vector2.Distance(end, start);

        //Debug.Log("rect.sizeDelta.x:" + rect.sizeDelta.x);

        //todo 画线相关
        rect.sizeDelta = new Vector2(rect.sizeDelta.x
            /*Mathf.Max(30, distance/10)*/, Mathf.Max(1, distance));

        //调整显⽰层级
        //rect.localScale = new Vector3(rect.localScale.x * (Random.value > 0.5f ? 1 : -1), rect.localScale.y);

        line.transform.SetAsFirstSibling();

    }


    //获取两个坐标点之间的夹⾓
    public float GetAngle(Vector2 start, Vector2 end)

    {

        var dir = end - start;

        var dirV2 = new Vector2(dir.x, dir.y);

        var angle = Vector2.SignedAngle(dirV2, Vector2.down);

        return angle;

    }


    //获取上下相邻两个坐标点中间的坐标点
    private Vector2 GetBetweenPoint(Vector2 start, Vector2 end)

    {

        //两点之间垂直距离

        float distance = end.y - start.y;

        float y = start.y + distance / 2;

        float x = start.x;

        if (start.x != end.x)

        {

            //斜率值

            float k = (end.y - start.y) / (end.x - start.x);

            //根据公式 y = kx + b ，求b

            float b = start.y - k * start.x;

            x = (y - b) / k;

        }

        Vector2 point = new Vector2(x, y);

        return point;

    }
}
