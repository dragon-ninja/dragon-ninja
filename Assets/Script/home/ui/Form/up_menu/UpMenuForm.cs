using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpMenuForm : BaseUIForm
{
    public Image levelImg;
    public TextMeshProUGUI userNameText;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI goldText;
    public TextMeshProUGUI gemText;
    public TextMeshProUGUI strengthText;

    public override void Awake()
    {
        base.Awake();
        canvasGroup.alpha = 1;

        ui_type.ui_FormType = UIformType.Fixed;
        ui_type.ui_ShowType = UIformShowMode.Normal;
        ui_type.IsClearStack = false;

        userNameText = UIFrameUtil.FindChildNode(this.transform, "userName/Text (TMP)").GetComponent<TextMeshProUGUI>();
        levelImg = UIFrameUtil.FindChildNode(this.transform, "exp/expValue").GetComponent<Image>();
        levelText = UIFrameUtil.FindChildNode(this.transform, "exp/value").GetComponent<TextMeshProUGUI>();
        goldText = UIFrameUtil.FindChildNode(this.transform, "gold/value").GetComponent<TextMeshProUGUI>();
        gemText = UIFrameUtil.FindChildNode(this.transform, "gem/value").GetComponent<TextMeshProUGUI>();
        strengthText = UIFrameUtil.FindChildNode(this.transform, "strength/value").GetComponent<TextMeshProUGUI>();
       
        bool isHaveLiuhai = false;
#if UNITY_IPHONE
  		 //通过设备型号判断是否刘海屏
         if (SystemInfo.deviceModel.Contains("iPhone10,3") 
          || SystemInfo.deviceModel.Contains("iPhone10,6")
          || SystemInfo.deviceModel.Contains("iPhone11,2")
          || SystemInfo.deviceModel.Contains("iPhone11,6")
          || SystemInfo.deviceModel.Contains("iPhone11,8"))
        {
            isHaveLiuhai = true;
        }
        //通过屏幕比例判断是否刘海屏
        if ((float)Screen.width / Screen.height > 2)
        {
            isHaveLiuhai = true;
        }
#endif
        if (Screen.height - Screen.safeArea.yMax > 0)
            isHaveLiuhai = true;
        if (isHaveLiuhai)
            GetComponent<RectTransform>().sizeDelta = 
                new Vector2(GetComponent<RectTransform>().sizeDelta.x, 250);




        GetBut(this.transform, "userName").onClick.AddListener(() => {
            OpenForm("SettingForm");
        });

        GetBut(this.transform, "gold").onClick.AddListener(() => {
            MessageMgr.SendMsg("jumpGold",null);
        });
        GetBut(this.transform, "gem").onClick.AddListener(() => {
            MessageMgr.SendMsg("jumpGem", null);
        });
        GetBut(this.transform, "strength").onClick.AddListener(() => {
            OpenForm("StrengthForm");
        });


        MessageMgr.AddMsgListener("UpMenuRefresh", p =>
        {
            Refresh();
        });

        Refresh();
    }

    public async void Refresh() {

        await DataManager.Get().refreshRoleAttributeStr();
        await DataManager.Get().refreshBackPack();
        await NetManager.get(ConfigCheck.publicUrl + "/data/pub/hungUp/push", DataManager.Get().getHeader());

        int gold = 0;
        int gem = 0;
        EquipmentData goldD = DataManager.Get().backPackData.backPackItems.Find(x => x.id == "p10001");
        if(goldD!=null)
            gold = goldD.quantity;

        EquipmentData gemD = DataManager.Get().backPackData.backPackItems.Find(x => x.id == "p10000");
        if (gemD != null)
            gem = gemD.quantity;


        userNameText.text = DataManager.Get().nickName; //DataManager.Get().loginData.data.user;
        levelText.text = DataManager.Get().roleAttrData.nowLevel + "";


        //Debug.Log();

        levelImg.fillAmount = (DataManager.Get().roleAttrData.nowLevelExp  + 0.0f)
            / DataManager.Get().roleAttrData.nextLevelNeedExp;

        goldText.text = NumUtil.getNumk(gold) ;
        gemText.text = NumUtil.getNumk(gem);
        strengthText.text = DataManager.Get().roleAttrData.strength.strength + "/"+ DataManager.Get().roleAttrData.strength.maxStrength;

        MessageMgr.SendMsg("RefreshStrength", null);
    }

    float time;
    private void Update()
    {
        time += Time.deltaTime;
        if (time > 300) {
            time = 0;
            Refresh();
        }
    }
}
