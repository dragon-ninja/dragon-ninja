using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;//使用场景管理器
using UnityEngine.Audio;

public class GameSceneManage : MonoBehaviour
{
    public AudioMixer audioMixer;

    public GameObject pausePanel;
    //暂停界面
    public GameObject pausePanel_c0;
    //开局教程
    public GameObject pausePanel_c1;
    //dly教程
    public GameObject pausePanel_c2;

    //public static TextMeshProUGUI killTxt;
    public static TextMeshProUGUI goldTxt;

    //public static int killNum;
    //public static int goldNum;

    public static float nowTimeScale = 1;

    //开局提示
    public static bool courseFlag_1;
    //dly提示
    public static bool courseFlag_2;

    private void Awake()
    {
        //audioMixer.SetFloat("vSound", DataManager.Get().userData.settingData.soundFlag ? 1 : -100f);
        //audioMixer.SetFloat("vMusic", DataManager.Get().userData.settingData.musicFlag ? 1 : -100f);

        //killNum = 0;
        //goldNum = 0;
        //killTxt = GameObject.Find("kill/Text (TMP)").GetComponent<TextMeshProUGUI>();
        //killTxt.text = SpriteNumUtil.zhInt(killNum);

        //old
        /*pausePanel = GameObject.Find("Canvas").transform.Find("暂停").gameObject;
        pausePanel_c0 = pausePanel.transform.Find("TowerBackPack").gameObject;
        pausePanel_c1 = pausePanel.transform.Find("course_1").gameObject;
        pausePanel_c2 = pausePanel.transform.Find("course_2").gameObject;
        */

        pausePanel = GameObject.Find("Canvas").transform.Find("暂停_new").gameObject;
        pausePanel_c0 = pausePanel.transform.Find("TowerBackPack").gameObject;
        pausePanel_c1 = pausePanel.transform.Find("course_1").gameObject;
        pausePanel_c2 = pausePanel.transform.Find("course_2").gameObject;


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
           GameObject.Find("TimePanel").GetComponent<RectTransform>().sizeDelta = new Vector2(GetComponent<RectTransform>().sizeDelta.x, 250);

    }

    private void Start()
    {
        if (!DataManager.Get().userData.settingData.courseFlag_1) {
            DataManager.Get().userData.settingData.courseFlag_1 = true; 
            PauseGame(1);
        }
    }

    public void tryDlyCourse() {
        if (!DataManager.Get().userData.settingData.courseFlag_2)
        {
            DataManager.Get().userData.settingData.courseFlag_2 = true;
            PauseGame(2);
        }
    }

    public void PauseGame(int i=0)
    {
        Time.timeScale = 0;

        pausePanel_c0.SetActive(false);
        pausePanel_c1.SetActive(false);
        pausePanel_c2.SetActive(false);
        
        pausePanel.SetActive(true);
        
        if (i == 0)
        {
            pausePanel_c0.SetActive(true);
        }
        else if (i == 1) {
            pausePanel_c1.SetActive(true);
        }
        else if (i == 2)
        {
            pausePanel_c2.SetActive(true);
        }
    }

    public void EndPauseGame()
    {
        Time.timeScale = GameSceneManage.nowTimeScale;
        pausePanel.SetActive(false);
    }

    public void RestartGame() {
        SceneManager.LoadScene("battle");
    }

    public void BackHome() {
        Time.timeScale = 1;
        SceneManager.LoadScene("home");
    }
}
