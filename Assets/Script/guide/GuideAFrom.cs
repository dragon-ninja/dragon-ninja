using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using TMPro;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Threading.Tasks;
using UnityEngine.UI;

public class GuideAFrom : BaseUIForm
{
    int index = 0;
    GameObject panel_1;
    GameObject panel_2;

    GameObject img_1;
    GameObject img_2;
    GameObject img_3;
    GameObject img_4;

    TextMeshProUGUI desc;
    GameObject but;

    //防止重复点击
    bool towerButFlag;

    public override void Awake()
    {
        base.Awake();
        ui_type.ui_FormType = UIformType.Fixed;
        ui_type.ui_ShowType = UIformShowMode.Normal;
        ui_type.IsClearStack = false;



        panel_1 = UIFrameUtil.FindChildNode(this.transform, "Panel").gameObject;
        panel_2 = UIFrameUtil.FindChildNode(this.transform, "Panel_2").gameObject;

        img_1 = UIFrameUtil.FindChildNode(this.transform, "Image_1").gameObject;
        img_2 = UIFrameUtil.FindChildNode(this.transform, "Image_2").gameObject;
        img_3 = UIFrameUtil.FindChildNode(this.transform, "Image_3").gameObject;
        img_4 = UIFrameUtil.FindChildNode(this.transform, "Image_4").gameObject;

        desc = UIFrameUtil.FindChildNode(this.transform, "desc").GetComponent<TextMeshProUGUI>();
        but = UIFrameUtil.FindChildNode(this.transform, "button").gameObject;
        GetComponent<Button>().onClick.AddListener(() => {
            if (notClikeTiime <= 0)
            {
                notClikeTiime = 0.5f;
                next();
            }
        });

        GetBut(this.transform, "button")
          .onClick.AddListener(async () => {

              //先校验
              if (towerButFlag)
                  return;
              DataManager.Get().now_chapterIndex = 0;
              towerButFlag = true;
              //string str = await NetManager.get(ConfigCheck.publicUrl + "/data/pub/battleFlow/flowIn", DataManager.Get().getHeader());
              //Debug.Log(str);

              //JObject obj = (JObject)JsonConvert.DeserializeObject(str);
              //NetData NetData = obj.ToObject<NetData>();

              //if ((bool)NetData.data)
              {
                  //todo 爬塔
                  towerButFlag = false;
                  SceneManager.LoadScene("tower");
              }
              /* else
               {
                    //显示资源不够
                    UIManager.GetUIMgr().showUIForm("ErrForm");
                   MessageMgr.SendMsg("ErrorDesc", new MsgKV("", "Insufficient physical strength"));
                   towerButFlag = false;
               }*/
          });

        notClikeTiime = 0.5f;
        img_1.GetComponent<CanvasGroup>().DOFade(1, 1f);
    }

    float notClikeTiime = 0;

    void next()
    {

        index += 1;

        if (index == 1)
        {
            //img_2.SetActive(true);
            img_2.GetComponent<CanvasGroup>().DOFade(1, 1f);
        }
        if (index == 2)
        {
            //img_3.SetActive(true);
            img_3.GetComponent<CanvasGroup>().DOFade(1, 1f);
        }
        if (index == 3)
        {
            //img_4.SetActive(true);
            img_4.GetComponent<CanvasGroup>().DOFade(1, 1f);
        }

        if (index == 4)
        {


            desc.text = "What's going on here? In order to go back, we must take risks!";
            panel_1.SetActive(false);
            panel_2.SetActive(true);
            //强制刷新布局
            LayoutRebuilder.ForceRebuildLayoutImmediate
               (desc.GetComponent<RectTransform>());
        }


        if (index == 5)
        {
            desc.text = "The entrance appears, start fighting!";
            but.SetActive(true);
            //强制刷新布局
            LayoutRebuilder.ForceRebuildLayoutImmediate
               (desc.GetComponent<RectTransform>());
        }
    }

    private void Update()
    {
        if (notClikeTiime > 0)
            notClikeTiime -= Time.deltaTime;
    }
}