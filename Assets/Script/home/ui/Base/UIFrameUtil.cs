using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFrameUtil : MonoBehaviour
{
    //使用此方法须保证没有同名子孙节点 否则请勿使用  
    public static Transform FindChildNode(Transform parentNode, string childName)
    {
        Transform childNode = parentNode.Find(childName);

        if (childNode == null)
        {
            foreach (Transform childs in parentNode.transform)
            {
                childNode = FindChildNode(childs, childName);
                if (childNode != null)
                {
                    return childNode;
                }
            }
        }

        return childNode;
    }

    //根据十六进制以及透明度获取颜色
    public static Color getitemQualityColor(string colorStr, float alphe = 1)
    {
        Color c = new Color();
        ColorUtility.TryParseHtmlString(colorStr, out c);
        c.a = alphe;
        return c;
    }

    /// <summary>
    /// 返回路径返回UI图片
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static Sprite GetItemIcon(string path)
    {
        return null;
    }
}
