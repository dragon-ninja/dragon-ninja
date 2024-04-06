using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class DownMenuForm : BaseUIForm
{
    Image myImg;


    List<RectTransform> tras = new List<RectTransform>();

    public override void Awake()
    {
        base.Awake();
        canvasGroup.alpha = 1;

        ui_type.ui_FormType = UIformType.Fixed;
        ui_type.ui_ShowType = UIformShowMode.Normal;
        ui_type.IsClearStack = false;

        //myImg = transform.Find("img").GetComponent<Image>();

        tras.Add(transform.Find("GameObject/Button_OpenShop").GetComponent<RectTransform>());
        tras.Add(transform.Find("GameObject/Button_OpenBackPack").GetComponent<RectTransform>());
        tras.Add(transform.Find("GameObject/Button_OpenDungeon").GetComponent<RectTransform>());
        tras.Add(transform.Find("GameObject/Button_OpenTalent").GetComponent<RectTransform>());
        tras.Add(transform.Find("GameObject/Button_OpenChallenge").GetComponent<RectTransform>());

        //新手引导相关
        MessageMgr.AddMsgListener("Guide_Button_OpenShop", p =>
        {
            OpenForm("ShopForm");
            reset();
            tras[0].SetAsLastSibling();
            tras[0].sizeDelta = new Vector2(280, 280);
            tras[0].Find("Image").GetComponent<RectTransform>().sizeDelta = new Vector2(150, 150);
            tras[0].GetComponent<Image>().sprite = Resources.Load<Sprite>("ui/img/down_menu/select/1");
        });

        MessageMgr.AddMsgListener("Guide_Button_OpenBackPack", p =>
        {
            OpenForm("BackPackForm");
            reset();
            tras[1].SetAsLastSibling();
            tras[1].sizeDelta = new Vector2(280, 280);
            tras[1].Find("Image").GetComponent<RectTransform>().sizeDelta = new Vector2(150, 150);
            tras[1].GetComponent<Image>().sprite = Resources.Load<Sprite>("ui/img/down_menu/select/2");
        });
        MessageMgr.AddMsgListener("Guide_Button_OpenDungeon", p =>
        {
            OpenForm("DungeonForm");
            reset();
            tras[2].SetAsLastSibling();
            tras[2].sizeDelta = new Vector2(280, 280);
            tras[2].Find("Image").GetComponent<RectTransform>().sizeDelta = new Vector2(200, 200);
            tras[2].GetComponent<Image>().sprite = Resources.Load<Sprite>("ui/img/down_menu/select/3");
        });
       



        GetBut(this.transform, "Button_OpenShop").
           onClick.AddListener(() => {
               OpenForm("ShopForm");
               //myImg.sprite = Resources.Load<Sprite>("ui/img/home/down_Shop");
               reset();
               tras[0].SetAsLastSibling();
               tras[0].sizeDelta = new Vector2(280, 280);
               tras[0].Find("Image").GetComponent<RectTransform>().sizeDelta = new Vector2(150, 150);
               tras[0].GetComponent<Image>().sprite = Resources.Load<Sprite>("ui/img/down_menu/select/1");
           });

        GetBut(this.transform, "Button_OpenBackPack").
           onClick.AddListener(() => {
               OpenForm("BackPackForm");
               //myImg.sprite = Resources.Load<Sprite>("ui/img/home/down_BackPack");
               reset();
               tras[1].SetAsLastSibling();
               tras[1].sizeDelta = new Vector2(280, 280);
               tras[1].Find("Image").GetComponent<RectTransform>().sizeDelta = new Vector2(150, 150);
               tras[1].GetComponent<Image>().sprite = Resources.Load<Sprite>("ui/img/down_menu/select/2");
           });

        GetBut(this.transform, "Button_OpenDungeon").
           onClick.AddListener(() => {
               OpenForm("DungeonForm");
               //myImg.sprite = Resources.Load<Sprite>("ui/img/home/down_Dungeon");
               reset();
               tras[2].SetAsLastSibling();
               tras[2].sizeDelta = new Vector2(280, 280);
               tras[2].Find("Image").GetComponent<RectTransform>().sizeDelta = new Vector2(200, 200);
               tras[2].GetComponent<Image>().sprite = Resources.Load<Sprite>("ui/img/down_menu/select/3");
           });

        GetBut(this.transform, "Button_OpenTalent").
            onClick.AddListener(() => {
                OpenForm("TalentForm");
                //myImg.sprite = Resources.Load<Sprite>("ui/img/home/down_Talent");
                reset();
                tras[3].SetAsLastSibling();
                tras[3].sizeDelta = new Vector2(280, 280);
                tras[3].Find("Image").GetComponent<RectTransform>().sizeDelta = new Vector2(150, 150);
                tras[3].GetComponent<Image>().sprite = Resources.Load<Sprite>("ui/img/down_menu/select/4");
            });

        MessageMgr.AddMsgListener("jumpGold", p =>
        {
            jumpGold();
        });
        MessageMgr.AddMsgListener("jumpGem", p =>
        {
            jumpGem();
        });

        tras[2].SetAsLastSibling();
        tras[2].sizeDelta = new Vector2(280, 280);
        tras[2].Find("Image").GetComponent<RectTransform>().sizeDelta = new Vector2(200, 200);
        tras[2].GetComponent<Image>().sprite = Resources.Load<Sprite>("ui/img/down_menu/select/3");
    }

    private void Start()
    {
        //OpenForm("DungeonForm");
    }

    void reset() {

        foreach (RectTransform t in tras) {
            t.sizeDelta = new Vector2(240, 200);
            t.SetAsLastSibling();
            t.Find("Image").GetComponent<RectTransform>().sizeDelta = new Vector2(100,100);
        }
        tras[0].GetComponent<Image>().sprite = Resources.Load<Sprite>("ui/img/down_menu/1");
        tras[1].GetComponent<Image>().sprite = Resources.Load<Sprite>("ui/img/down_menu/2");
        tras[2].GetComponent<Image>().sprite = Resources.Load<Sprite>("ui/img/down_menu/3");
        tras[3].GetComponent<Image>().sprite = Resources.Load<Sprite>("ui/img/down_menu/4");

    }

    void jumpGold() {
        OpenForm("ShopForm");
        //myImg.sprite = Resources.Load<Sprite>("ui/img/home/down_Shop");
        reset();
        tras[0].SetAsLastSibling();
        tras[0].sizeDelta = new Vector2(280, 280);
        tras[0].Find("Image").GetComponent<RectTransform>().sizeDelta = new Vector2(150, 150);
        tras[0].GetComponent<Image>().sprite = Resources.Load<Sprite>("ui/img/down_menu/select/1");
    }

    void jumpGem() {
        OpenForm("ShopForm");
        //myImg.sprite = Resources.Load<Sprite>("ui/img/home/down_Shop");
        reset();
        tras[0].SetAsLastSibling();
        tras[0].sizeDelta = new Vector2(280, 280);
        tras[0].Find("Image").GetComponent<RectTransform>().sizeDelta = new Vector2(150, 150);
        tras[0].GetComponent<Image>().sprite = Resources.Load<Sprite>("ui/img/down_menu/select/1");
    }
}
