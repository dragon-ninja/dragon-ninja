using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class BaseSlot : MonoBehaviour, IPointerEnterHandler
{
    //槽位序号
    public int index;
    //管理者
    public Object mgr;

    //初始化标记
    protected bool initFlag;
    //槽位底图
    protected Image background;
    //槽位显示图标
    protected Image icon;

    public Button myBut;

    protected virtual void Awake()
    {
        initFlag = true;
        if (transform.Find("icon") != null)
            icon = transform.Find("icon").GetComponent<Image>();
        myBut = GetComponent<Button>();
        background = GetComponent<Image>();
    }

    public virtual void Show()
    {
        if (!initFlag)
            Awake();

        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {

    }
}
