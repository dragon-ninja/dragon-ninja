using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using TMPro;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Threading.Tasks;
using UnityEngine.UI;
using System.Collections;

public class GuideBFrom : BaseUIForm
{
    int index = 0;
    GameObject panel_1;
    GameObject panel_2;
    GameObject panel_2_2;
    GameObject panel_3;
    GameObject panel_4;
    GameObject panel_5;
    GameObject panel_6;

    //GameObject panel_2_s_1;
    //GameObject panel_2_s_2;

    TextMeshProUGUI desc;
    GameObject but;


    public override async void Awake()
    {
        base.Awake();
        ui_type.ui_FormType = UIformType.Fixed;
        ui_type.ui_ShowType = UIformShowMode.Normal;
        ui_type.IsClearStack = false;



        panel_1 = UIFrameUtil.FindChildNode(this.transform, "Panel_1").gameObject;
        panel_2 = UIFrameUtil.FindChildNode(this.transform, "Panel_2").gameObject;
        panel_2_2 = UIFrameUtil.FindChildNode(this.transform, "Panel_2_2").gameObject;
        panel_3 = UIFrameUtil.FindChildNode(this.transform, "Panel_3").gameObject;
        panel_4 = UIFrameUtil.FindChildNode(this.transform, "Panel_4").gameObject;
        panel_5 = UIFrameUtil.FindChildNode(this.transform, "Panel_5").gameObject;
        panel_6 = UIFrameUtil.FindChildNode(this.transform, "Panel_6").gameObject;

        //panel_2_s_1 = UIFrameUtil.FindChildNode(panel_2.transform, "手1").gameObject;
        //panel_2_s_2 = UIFrameUtil.FindChildNode(panel_2.transform, "手2").gameObject;

        //desc = UIFrameUtil.FindChildNode(this.transform, "desc").GetComponent<TextMeshProUGUI>();
        //but = UIFrameUtil.FindChildNode(this.transform, "button").gameObject;


      

        UIFrameUtil.FindChildNode(this.transform, "Button_OpenShop_Guide").GetComponent<Button>().onClick.AddListener(async () => {
            MessageMgr.SendMsg("Guide_Button_OpenShop", null);
            panel_1.SetActive(false);
            panel_2.SetActive(true);

            int gem = 0;
            EquipmentData goldD = DataManager.Get().backPackData.backPackItems.Find(x => x.id == "p10000");
            if (goldD != null)
                gem = goldD.quantity;

            if (gem < 80)
            {
                string str = await NetManager.get(ConfigCheck.publicUrl + "/data/pub/mall/dailyShop", DataManager.Get().getHeader());
                NetDailyShopData dailyShopdata = JsonUtil.ReadData<NetDailyShopData>(str);
                if (dailyShopdata.dailyInfoList[0].payedNum < dailyShopdata.dailyInfoList[0].buyCount)
                {
                    GameObject pf = GameObject.Find("DailyShopList/Image (1)");

                    LayoutRebuilder.ForceRebuildLayoutImmediate
                        (pf.transform.parent.GetComponent<RectTransform>());
                    LayoutRebuilder.ForceRebuildLayoutImmediate
                        (pf.transform.parent.parent.GetComponent<RectTransform>());


                    GameObject node_0 = Instantiate(pf, panel_2.transform);


                    Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(null, pf.transform.position);
                    RectTransform rt = panel_2.GetComponent<RectTransform>();
                    Vector3 globalMousePos;
                    RectTransformUtility.ScreenPointToWorldPointInRectangle(rt, screenPoint, null, out globalMousePos);
                    node_0.transform.position = globalMousePos;

                    Transform shou = UIFrameUtil.FindChildNode(this.transform, "Panel_2/手");
                    shou.parent = node_0.transform;
                    shou.gameObject.SetActive(true);
                    shou.localPosition = new Vector3(0, 100);
               
                    Debug.Log("dailyShop:" + str);
                    node_0.GetComponent<DailyShopSlot>().Refresh(dailyShopdata.dailyInfoList[0]);

                    node_0.GetComponent<Button>().onClick.AddListener(() =>
                    {
                        node_0.gameObject.SetActive(false);
                    });
                }
                else {
                    this.gameObject.SetActive(false);
                }
            }
            else {
                panel_2.SetActive(false);
                panel_2_2.SetActive(true);

                MessageMgr.SendMsg("Guide_OpenBox", null);
                GameObject pf = GameObject.Find("Supply box/panel/box1");

                LayoutRebuilder.ForceRebuildLayoutImmediate
                    (pf.transform.parent.GetComponent<RectTransform>());
                LayoutRebuilder.ForceRebuildLayoutImmediate
                    (pf.transform.parent.parent.GetComponent<RectTransform>());


                GameObject node_1 = Instantiate(pf, panel_2_2.transform);


                Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(null, pf.transform.position);
                RectTransform rt = panel_2_2.GetComponent<RectTransform>();
                Vector3 globalMousePos;
                RectTransformUtility.ScreenPointToWorldPointInRectangle(rt, screenPoint, null, out globalMousePos);
                node_1.transform.position = globalMousePos;

                Transform shou = UIFrameUtil.FindChildNode(this.transform, "Panel_2_2/手");
                shou.parent = node_1.transform;
                shou.gameObject.SetActive(true);
                shou.localPosition = new Vector3(70, 80);

                node_1.transform.Find("BuyBut").GetComponent<Button>().onClick.AddListener(() => {
                    OpenForm("ShopConfirmForm");
                    MessageMgr.SendMsg("openBox", new MsgKV("sc001", 1));
                    node_1.gameObject.SetActive(false);
                });
            }
        });

        //跳转到装备页面
        UIFrameUtil.FindChildNode(this.transform, "Button_OpenBackPack_Guide").GetComponent<Button>().onClick.AddListener(async () => {
            MessageMgr.SendMsg("Guide_Button_OpenBackPack", null);
            panel_3.SetActive(false);
            panel_4.SetActive(true);


            //如果网络不好 不一定刷出来 怎么做?

            if (GameObject.Find("itemList/item") == null) {
                //等待
                StartCoroutine(awaitBackPack());
                return;
            }

            GameObject pf = GameObject.Find("itemList/item");
 
                LayoutRebuilder.ForceRebuildLayoutImmediate
                    (pf.transform.parent.GetComponent<RectTransform>());
                LayoutRebuilder.ForceRebuildLayoutImmediate
                    (pf.transform.parent.parent.GetComponent<RectTransform>());


            GameObject node_1 = Instantiate(pf, panel_4.transform);


            Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(null, pf.transform.position);
            RectTransform rt = panel_4.GetComponent<RectTransform>();
            Vector3 globalMousePos;
            RectTransformUtility.ScreenPointToWorldPointInRectangle(rt, screenPoint, null, out globalMousePos);
            node_1.transform.position = globalMousePos;


            Transform shou = UIFrameUtil.FindChildNode(this.transform, "Panel_4/手");
            shou.parent = node_1.transform;
            shou.gameObject.SetActive(true);
            shou.localPosition = new Vector3(0, 150);

            //UIFrameUtil.FindChildNode(this.transform, "Panel_4/手").transform.position = globalMousePos + new Vector3(0, 100);


            node_1.GetComponent<BackPackSlot>().Refresh(pf.GetComponent<BackPackSlot>().eqData,
                pf.GetComponent<BackPackSlot>().eqAtr);



            node_1.GetComponent<Button>().onClick.AddListener(() => {
                panel_4.SetActive(false);
                panel_5.SetActive(true);

                LayoutRebuilder.ForceRebuildLayoutImmediate
                    (GameObject.Find("ItemInfoPanel/Image/Equip").transform.parent.GetComponent<RectTransform>());
                LayoutRebuilder.ForceRebuildLayoutImmediate
                    (GameObject.Find("ItemInfoPanel/Image/Equip").transform.parent.parent.GetComponent<RectTransform>());
                Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(null, GameObject.Find("ItemInfoPanel/Image/Equip").transform.position);
                RectTransform rt = panel_1.GetComponent<RectTransform>();
                Vector3 globalMousePos;
                RectTransformUtility.ScreenPointToWorldPointInRectangle(rt, screenPoint, null, out globalMousePos);
                UIFrameUtil.FindChildNode(this.transform, "Button_Equip_Guide").transform.position = globalMousePos;
            });
        });

        UIFrameUtil.FindChildNode(this.transform, "Button_Equip_Guide").GetComponent<Button>().onClick.AddListener(() => {

            if (!this.gameObject.activeInHierarchy)
                return;

            MessageMgr.SendMsg("GuideB_Equip",null);
            panel_5.SetActive(false);
            panel_6.SetActive(true);

            LayoutRebuilder.ForceRebuildLayoutImmediate
                (GameObject.Find("GameObject/Button_OpenDungeon").transform.parent.GetComponent<RectTransform>());
            LayoutRebuilder.ForceRebuildLayoutImmediate
                (GameObject.Find("GameObject/Button_OpenDungeon").transform.parent.parent.GetComponent<RectTransform>());
            Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(null, GameObject.Find("GameObject/Button_OpenDungeon").transform.position);
            RectTransform rt = panel_1.GetComponent<RectTransform>();
            Vector3 globalMousePos;
            RectTransformUtility.ScreenPointToWorldPointInRectangle(rt, screenPoint, null, out globalMousePos);
            UIFrameUtil.FindChildNode(this.transform, "Button_OpenDungeon_Guide").transform.position = globalMousePos;
           
        });

        UIFrameUtil.FindChildNode(this.transform, "Button_OpenDungeon_Guide").GetComponent<Button>().onClick.AddListener(() => {
            this.gameObject.SetActive(false);
            MessageMgr.SendMsg("Guide_Button_OpenDungeon", null);
        });

        MessageMgr.AddMsgListener("GuideB_OpenBoxEnd", p =>
        {
            panel_2_2.SetActive(false);
            panel_3.SetActive(true);


            LayoutRebuilder.ForceRebuildLayoutImmediate
                (GameObject.Find("GameObject/Button_OpenBackPack").transform.parent.GetComponent<RectTransform>());
            LayoutRebuilder.ForceRebuildLayoutImmediate
                (GameObject.Find("GameObject/Button_OpenBackPack").transform.parent.parent.GetComponent<RectTransform>());
            Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(null, GameObject.Find("GameObject/Button_OpenBackPack").transform.position);
            RectTransform rt = panel_1.GetComponent<RectTransform>();
            Vector3 globalMousePos;
            RectTransformUtility.ScreenPointToWorldPointInRectangle(rt, screenPoint, null, out globalMousePos);
            UIFrameUtil.FindChildNode(this.transform, "Button_OpenBackPack_Guide").transform.position = globalMousePos;
        });


        MessageMgr.AddMsgListener("GuideB_ShopGem", p =>
        {
            if (!this.gameObject.activeInHierarchy)
                return;

            panel_2.SetActive(false);
            panel_2_2.SetActive(true);
            MessageMgr.SendMsg("Guide_OpenBox", null);
            GameObject pf = GameObject.Find("Supply box/panel/box1");

            LayoutRebuilder.ForceRebuildLayoutImmediate
                (pf.transform.parent.GetComponent<RectTransform>());
            LayoutRebuilder.ForceRebuildLayoutImmediate
                (pf.transform.parent.parent.GetComponent<RectTransform>());


            GameObject node_1 = Instantiate(pf, panel_2_2.transform);


            Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(null, pf.transform.position);
            RectTransform rt = panel_2_2.GetComponent<RectTransform>();
            Vector3 globalMousePos;
            RectTransformUtility.ScreenPointToWorldPointInRectangle(rt, screenPoint, null, out globalMousePos);
            node_1.transform.position = globalMousePos;


            Transform shou = UIFrameUtil.FindChildNode(this.transform, "Panel_2_2/手");
            shou.parent = node_1.transform;
            shou.gameObject.SetActive(true);
            shou.localPosition = new Vector3(70, 80);

           
            node_1.transform.Find("BuyBut").GetComponent<Button>().onClick.AddListener(() => {
                OpenForm("ShopConfirmForm");
                MessageMgr.SendMsg("openBox", new MsgKV("sc001", 1));
                node_1.gameObject.SetActive(false);
            });

        });
    }

    private void Start()
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate
          (GameObject.Find("GameObject/Button_OpenShop").transform.parent.GetComponent<RectTransform>());
        LayoutRebuilder.ForceRebuildLayoutImmediate
            (GameObject.Find("GameObject/Button_OpenShop").transform.parent.parent.GetComponent<RectTransform>());
        Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(null, GameObject.Find("GameObject/Button_OpenShop").transform.position);
        RectTransform rt = panel_1.GetComponent<RectTransform>();
        Vector3 globalMousePos;
        RectTransformUtility.ScreenPointToWorldPointInRectangle(rt, screenPoint, null, out globalMousePos);
        UIFrameUtil.FindChildNode(this.transform, "Button_OpenShop_Guide").transform.position = globalMousePos;
        //UIFrameUtil.FindChildNode(this.transform, "Panel_1/手").transform.position = globalMousePos + new Vector3(0, 200);


    }



    IEnumerator awaitBackPack()
    {
        while (GameObject.Find("itemList/item") == null) { 
            yield return null;
        }

        GameObject pf = GameObject.Find("itemList/item");

        LayoutRebuilder.ForceRebuildLayoutImmediate
            (pf.transform.parent.GetComponent<RectTransform>());
        LayoutRebuilder.ForceRebuildLayoutImmediate
            (pf.transform.parent.parent.GetComponent<RectTransform>());


        GameObject node_1 = Instantiate(pf, panel_4.transform);


        Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(null, pf.transform.position);
        RectTransform rt = panel_4.GetComponent<RectTransform>();
        Vector3 globalMousePos;
        RectTransformUtility.ScreenPointToWorldPointInRectangle(rt, screenPoint, null, out globalMousePos);
        node_1.transform.position = globalMousePos;


        Transform shou = UIFrameUtil.FindChildNode(this.transform, "Panel_4/手");
        shou.parent = node_1.transform;
        shou.gameObject.SetActive(true);
        shou.localPosition = new Vector3(0, 150);

        //UIFrameUtil.FindChildNode(this.transform, "Panel_4/手").transform.position = globalMousePos + new Vector3(0, 100);


        node_1.GetComponent<BackPackSlot>().Refresh(pf.GetComponent<BackPackSlot>().eqData,
            pf.GetComponent<BackPackSlot>().eqAtr);



        node_1.GetComponent<Button>().onClick.AddListener(() => {
            panel_4.SetActive(false);
            panel_5.SetActive(true);

            LayoutRebuilder.ForceRebuildLayoutImmediate
                (GameObject.Find("ItemInfoPanel/Image/Equip").transform.parent.GetComponent<RectTransform>());
            LayoutRebuilder.ForceRebuildLayoutImmediate
                (GameObject.Find("ItemInfoPanel/Image/Equip").transform.parent.parent.GetComponent<RectTransform>());
            Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(null, GameObject.Find("ItemInfoPanel/Image/Equip").transform.position);
            RectTransform rt = panel_1.GetComponent<RectTransform>();
            Vector3 globalMousePos;
            RectTransformUtility.ScreenPointToWorldPointInRectangle(rt, screenPoint, null, out globalMousePos);
            UIFrameUtil.FindChildNode(this.transform, "Button_Equip_Guide").transform.position = globalMousePos;
        });
    }

}
