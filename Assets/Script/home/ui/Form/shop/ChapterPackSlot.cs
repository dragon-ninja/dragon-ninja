using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class ChapterPackSlot : BaseSlot
{
    TextMeshProUGUI num;

    protected override void Awake()
    {
        initFlag = true;
        base.Awake();
        num = UIFrameUtil.FindChildNode(this.transform, "Text (TMP)").GetComponent<TextMeshProUGUI>();
    }

    public void Refresh(ItemInfo info)
    {
        if (!initFlag)
            Awake();

        //����id�ҵ���Ӧ����Ʒͼ��
        string iconUrl = ItemFactory.Get().itemMap[info.id].icon;
        icon.sprite = null;
        //��ʾͼ�������
        icon.sprite = Resources.Load<Sprite>(iconUrl);
        num.text = "x"+ info.num;
    }

}
