using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BaseUIForm : MonoBehaviour
{
    UIManager umgr;
    public UIType ui_type = new UIType();
    public CanvasGroup canvasGroup;
    protected bool initFlag;

    public virtual void Awake()
    {
        initFlag = true;
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0;
    }

    public virtual void Show()
    {
        if (!initFlag)
            Awake();

        transform.SetAsLastSibling();

        gameObject.SetActive(true);
        DOFade(1, 0.25f);
        StopCoroutine("InActive");
    }

    public virtual void Hide()
    {
        if (!gameObject.activeInHierarchy)
            return;

        DOFade(0, 0.25f);
        StartCoroutine("InActive", 0.25f);
    }

    public virtual void ReShow()
    {
        gameObject.SetActive(true);
        DOFade(1, 0.25f);
    }

    public virtual void Freeze()
    {

    }

    public void DOFade(float alphe, float time)
    {
        //canvasGroup.alpha = alphe;
        canvasGroup.DOFade(alphe, time);
    }

    IEnumerator InActive(float time)
    {
        yield return new WaitForSeconds(time);
        gameObject.SetActive(false);
    }

    //调用简化
    public void OpenForm(string formName)
    {
        UIManager.GetUIMgr().showUIForm(formName);
    }

    public void CloseForm(string formName = null)
    {
        if (formName == null)
        {
            string className = GetType().ToString();
            if (className.LastIndexOf('.') != -1)
                className = className.Substring(className.LastIndexOf('.') + 1);

            formName = className;
        }

        UIManager.GetUIMgr().closeUIForm(formName);
    }

    public Button GetBut(Transform node, string butName)
    {
        return UIFrameUtil.FindChildNode(node, butName).GetComponent<Button>();
    }

    public T GetComponent<T>(Transform node, string path)
    {
        T com = default(T);
        Transform trans = UIFrameUtil.FindChildNode(node, path);
        if (trans != null)
        {
            if (typeof(T) == typeof(Transform))
            {
                com = (T)(object)trans;
            }
            else
            {
                com = trans.GetComponent<T>();
            }
        }
        return com;
    }

    //获取匹配当前语言的字符串内容
    public string GetText(string textName)
    {
        return LanguageMgr.Get().GetText(textName);
    }
}
