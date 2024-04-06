using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerNodeSlot : BaseSlot
{

    public TowerMapNodeData data;
    public List<Image> lineList;


    protected override void Awake()
    {
        base.Awake();


        this.GetComponent<Button>().onClick.AddListener(() => {
                MessageMgr.SendMsg("selectMapNode",
                    new MsgKV("", data));
        });

    }

    public void Refresh(bool flag = false) {
        myBut.interactable = flag;
    }
}
