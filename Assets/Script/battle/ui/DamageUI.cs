using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Random = UnityEngine.Random;

public class DamageUI : MonoBehaviour
{
    //伤害数字级别  需要存放到相应的集合里
    public int lv;

    public bool critFlag;
    public float size = 1.5f;
    public float critSize = 2f;
    public TextMeshProUGUI text;
    float fx;
    //快速消失标记
    public bool desFlag;
    float leftTime;

    //转换艺术字字符
    string zh(int input) {
        List<int> result = new List<int>();
        string s = "";
        while (input>0)
        {
            s = "<sprite="+ (input % 10)+"> "+s;
            input = input / 10;
        }
        return s;
    }

    public void init(HitInfo hf, bool cure = false)
    {
        if (text == null)
        {
            text = GetComponent<TextMeshProUGUI>();
        }
        leftTime = 0;
        desFlag = false;

        transform.position = new Vector2((float)hf.hurtPos.x, (float)hf.hurtPos.y);


        text.text = zh(hf.damage); //hf.damage+"";//format(hf.damage + "");
        text.color = new Color(1,1,0,1);
        text.fontSize = size;

       /* if(cure)
        {
            text.text = hf.cure+""; 
            text.color = new Color(0, 1, 0, 1);
        }
        else*/ if (hf.critFlag)
        {
            //text.color = new Color(1, 0, 0, 1);
            text.fontSize = critSize;
        }

        fx = Random.Range(-0.5f, 0.5f);
        var offsetY = 0.5f + Math.Abs(GetHashCode()) % 100 / 100.0f * 2f;
        transform.position += new Vector3(fx, offsetY, 0);
    }


    string format(string strs) {

        string text = "";
        var charArray = strs.ToCharArray();

        foreach (var cr in charArray) {
            text += $"<sprite=\"num\" name=\"{cr}\">";
        }
        return text;
    }

    private void Update()
    {

        leftTime += (Time.deltaTime + (desFlag ? 0.3f : 0));
        Color c = text.color;


        if (leftTime < 0.2f * GameSceneManage.nowTimeScale)
        {
            if (text.fontSize > 0.3f)
                text.fontSize = text.fontSize - 0.01f;
            transform.position += new Vector3(fx, 0.5f, 0) * 0.075f;
        }
        else if (leftTime > 0.5f * GameSceneManage.nowTimeScale)
        {
            transform.position += new Vector3(fx, -1, 0) * 0.001f;

            c.a -= (0.05f * (desFlag ? 1.5f : 1));
            text.color = c;
            if (c.a <= 0f)
            {
                DamageUIManage.destroyDmgUI(this,lv);
            }
        }
    }
}


//打击信息
public struct HitInfo
{
    //伤害源
    public string skillType;

    //伤害打击点
    public Vector3 hitPos;

    //无 击退 牵引
    public string hitType;
    public float stiffTime;
    public float stiffForce;
    public bool notStiffCover;
    public bool cureFlag;

    //伤害数值
    public int damage;
    //当前剩余伤害  影响穿透效果
    public int surplusDmg;

    //治疗数值
    public int cure;

    //受伤目标坐标
    public Vector2 hurtPos;

    public bool electricShockFlag;
    public int electricShockNum;
    public int electricDmg;

    public bool curseFlag;
    public float cureseDelay;
    public int curseDmg;

    public bool executeFlag;
    public float executeHp;

    public bool critFlag;
    public float critDmg;

    public bool burnFlag;
    public int burnDmg;
    public float burnInterval;
    public float burnTime;

    public bool bleedFlag;
    public int bleedDmg;
    public float bleedInterval;
    public float bleedTime;


    public bool frozenFlag;
    public float frozenTime;

    public HitInfo Clone()
    {
        return (HitInfo)this.MemberwiseClone();
    }
}

