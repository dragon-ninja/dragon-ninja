using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DamagePanel : MonoBehaviour
{
    public List<DamagePanelSlot> slots = new List<DamagePanelSlot>();

    // Start is called before the first frame update：
    void Awake()
    {
        GetComponent<Button>().onClick.AddListener(() => {
            gameObject.SetActive(false);
        });

        UIFrameUtil.FindChildNode(this.transform, "returnBut").
            GetComponent<Button>().onClick.AddListener(() => {
                gameObject.SetActive(false);
            });


        Transform listTra = UIFrameUtil.FindChildNode(this.transform, "list");
        for (int i = 0; i < listTra.childCount; i++)
        {
            slots.Add(listTra.GetChild(i).GetComponent<DamagePanelSlot>());
        }
    }

    private void OnEnable()
    {
        Refresh();
    }

    void Refresh() {
        int index = 0;
        float alldmg = 0;
        foreach (var item in DamageMeters.damageMap) {
            alldmg += item.Value;
        }
        foreach (var item in DamageMeters.damageMap)
        {
            float v = 0;
            if (alldmg > 0)
                v = item.Value / alldmg;
            slots[index++].Refresh(item.Key,v,item.Value);
        }
    }


}
