using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

//小提示  用于事件和宝箱节点时显示玩家升级
public class TowerLevelUpDescForm : BaseUIForm
{

    public override void Awake()
    {
        base.Awake();
        canvasGroup.alpha = 1;

        ui_type.ui_FormType = UIformType.PopUp;
        ui_type.ui_ShowType = UIformShowMode.Normal;
        ui_type.IsClearStack = false;

        MessageMgr.AddMsgListener("LevelUpDescShow", p =>
        {
            StartCoroutine(open());
        });
    }

    IEnumerator open()
    {
        GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0, 0);

        canvasGroup.DOFade(1, 0.5f);
        
        yield return new WaitForSeconds(0.5f);
        GetComponent<RectTransform>().DOMove(
            GetComponent<RectTransform>().position + new Vector3(0, 200), 1.2f);
       
        
        yield return new WaitForSeconds(0.5f);
        canvasGroup.DOFade(0, 1f);
    }

}