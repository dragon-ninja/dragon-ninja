using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//将数字转成艺术字体工具
public class SpriteNumUtil 
{
    //转艺术字字符
    public static string zhInt(int input)
    {
        int i = input;
        string s = "";
        while (i > 0)
        {
            s = "<sprite=" + (i % 10) + "> " + s;
            i = i / 10;
        }
        if (input <= 0)
            s = "<sprite=0>";
        return s;
    }

    //转艺术字时间字符
    public static string zhTime(int input)
    {
        int minute = input / 60;
        int second = input % 60;


        if (second < 10 || second <= 0)
            return minute + ":0" + second;

        return minute + ":" + second;



        //yishuzi
        /* while (minute > 0)
         {
             s = "<sprite=" + (minute % 10) + ">" + s;
             minute = minute / 10;
         }
         if (input / 60 < 10)
             s = "<sprite=0>" + s;
         if (input / 60 <= 0)
             s = "<sprite=0>" + s;


         while (second > 0)
         {
             s2 = "<sprite=" + (second % 10) + ">" + s2;
             second = second / 10;
         }
         if (input % 60 < 10)
             s2 = "<sprite=0>" + s2;
         if (input % 60 <= 0)
             s2 = "<sprite=0>" + s2;

         return s + "<sprite=10> " + s2;*/
    }

    //转艺术字百分比
    public static string zhjindu(int input)
    {
        int i = input;
        string s = "";
        while (i > 0)
        {
            s = "<sprite=" + (i % 10) + "> " + s;
            i = i / 10;
        }
        if (input <= 0)
            s = "<sprite=0>";
        return s + "<sprite=10>";
    }
}
