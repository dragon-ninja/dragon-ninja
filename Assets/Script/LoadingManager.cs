using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingManager : MonoBehaviour
{
    Slider slider;
    float value;

    AsyncOperation ao;

    // Start is called before the first frame update
    void Awake()
    {
        slider =  GameObject.Find("Slider").GetComponent<Slider>();
        slider.value = 0;
        value = 0;
        StartCoroutine(loadScene());
    }

    IEnumerator loadScene()
    {
        yield return new WaitForSeconds(0.1f);
        ao = SceneManager.LoadSceneAsync("battle");
        //�����곡����Ҫ�Զ���ת
        //operation.allowSceneActivation Ĭ��Ϊtrue,��ζ�Զ���ת
        ao.allowSceneActivation = false;
    }



    // Update is called once per frame
    void Update()
    {
        if (ao != null) { 
            slider.value = ao.progress * 0.4f + (value / 1 * 0.6f);
            value += Time.deltaTime;

            if (ao.progress>=0.9f && value >= 1) {
                ao.allowSceneActivation = true;
            }
        }
    }

}
