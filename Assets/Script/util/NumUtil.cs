using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumUtil
{
    public static string getNumk(int num)
    {
        if (num > 999999) {
            if (num % 1000000 / 10000 > 0)
            {
                return num / 1000000 + "." +
                    num % 1000000 / 10000 + "M";
            }
            return num / 1000000 + "M";
        }


        if (num > 9999)
        {
            if (num % 1000 / 100 > 0)
            {
                return num / 1000 + "." +
                    num % 1000 / 100 + "k";
            }
            return num / 1000 + "k";
        }




        return num + "";
    }


    public static string getTime(int s) {

        string time = null;


        if (s >= 3600)
        {
            time = (s / 3600) + "h " + ((s % 3600) / 60) + "m " + ((s % 3600) % 60) + "s";
        }
        else if (s >= 60)
        {
            time = (s / 60) + "m " + (s % 60) + "s";
        }
        else {
            time = s+"s";
        }

        return time;
    }
}
