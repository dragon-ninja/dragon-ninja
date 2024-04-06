using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class TalentInfoPanel : BaseUIPanel
{
    Button myBut;

    public TextMeshProUGUI goldText;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI effectText;
    public Image gold_img;
    public Button unlock_Button;
    public TalentForm form;

    Talent nowTalent;

    protected override void Awake()
    {
        base.Awake();

        gold_img = UIFrameUtil.FindChildNode(this.transform, "gold_Image").GetComponent<Image>();
        goldText = UIFrameUtil.FindChildNode(this.transform, "gold_Text").GetComponent<TextMeshProUGUI>();
        nameText = UIFrameUtil.FindChildNode(this.transform, "name").GetComponent<TextMeshProUGUI>();
        effectText = UIFrameUtil.FindChildNode(this.transform, "effect").GetComponent<TextMeshProUGUI>();
        unlock_Button = UIFrameUtil.FindChildNode(this.transform, "unlock_Button").GetComponent<Button>();

        myBut = GetComponent<Button>();
        myBut.onClick.AddListener(() => {
            Hide();
        });

        unlock_Button.onClick.AddListener(() => {

            AudioSource.PlayClipAtPoint(Resources.Load<AudioClip>("audio/天赋点加点成功"), Vector3.zero, 1.0f);
           
            form.unlockTalent(nowTalent);
            Hide();
        });
    }

    public void Show(Talent t)
    {
        base.Show();

        nowTalent = t;

        nameText.text = t.name;
        effectText.text =t.talentType == "super" ? t.desc : t.desc.Substring(0, t.desc.Length-1) + t.value;
        goldText.text = "x"+t.expend.num;

        int gold = 0;
        if (DataManager.Get().backPackData.backPackItems.Find(x => x.id == "p10001") != null)
            gold = DataManager.Get().backPackData.backPackItems.Find(x => x.id == "p10001").quantity;


        if (gold > t.expend.num)
        {
            gold_img.sprite = Resources.Load<Sprite>("ui/icon/gold");
            unlock_Button.GetComponent<Image>().sprite =
                Resources.Load<Sprite>("ui/img/talent/unlock_Button");
            unlock_Button.interactable = true;
        }
        else {
            //金币不足
            gold_img.sprite = Resources.Load<Sprite>("ui/icon/gold_h");
            unlock_Button.GetComponent<Image>().sprite = 
                Resources.Load<Sprite>("ui/img/talent/unlock_Button_h");
            unlock_Button.interactable = false;
        }


        //显示/隐藏购买按钮
        if (
            //等级达到购买条件
            DataManager.Get().roleAttrData.nowLevel >= t.level &&
            //未解锁该天赋
            (DataManager.Get().roleAttrData.talentList == null 
             || !DataManager.Get().roleAttrData.talentList.Contains(t.id))
            &&
            //前置天赋满足
            (t.lastTalentId == null || t.lastTalentId.Length == 0 ||
            (DataManager.Get().roleAttrData.talentList != null
            && DataManager.Get().roleAttrData.talentList.Contains(t.lastTalentId))))
        {
            unlock_Button.gameObject.SetActive(true);
        }
        else
        {
            unlock_Button.gameObject.SetActive(false);
        }
    }
}
