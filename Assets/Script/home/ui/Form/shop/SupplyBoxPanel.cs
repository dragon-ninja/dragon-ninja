using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class SupplyBoxPanel : MonoBehaviour
{
    string boxType;
    Button AdBut;
    Button BuyBut;

    // Start is called before the first frame update：
    void Start()
    {
        AdBut = UIFrameUtil.FindChildNode(this.transform, "AdBut").GetComponent<Button>();
        BuyBut = UIFrameUtil.FindChildNode(this.transform, "BuyBut").GetComponent<Button>();


        LayoutRebuilder.ForceRebuildLayoutImmediate(
             BuyBut.transform.Find("gold").GetComponent<RectTransform>());


        BuyBut.onClick.AddListener(() => {
            MessageMgr.SendMsg("BuySupplyBox", new MsgKV("", boxType));
        });

    }

    public void Refresh() { 
    }
}
