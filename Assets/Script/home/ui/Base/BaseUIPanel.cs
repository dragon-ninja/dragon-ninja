using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

//属于Form的子级面板 具有部分类似Form的功能  不会参与全局的Form管理,只会参与同级的Panel管理
public class BaseUIPanel : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public bool initFlag;

    protected virtual void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0;
    }

    public virtual void Show()
    {
        StopCoroutine("InActive");
        gameObject.SetActive(true);
        DOFade(1, 0.25f);
    }

    public virtual void Hide()
    {
        if (!gameObject.activeInHierarchy)
            return;

        DOFade(0, 0.05f);
        StartCoroutine("InActive", 0.05f);
    }

    public virtual void ReShow()
    {
        gameObject.SetActive(true);
    }

    public virtual void Freeze()
    {

    }

    public void DOFade(float alphe, float time)
    {
        canvasGroup.DOFade(alphe, time);
    }

    IEnumerator InActive(float time)
    {
        yield return new WaitForSeconds(time);
        gameObject.SetActive(false);
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

    public void OpenForm(string formName)
    {
        UIManager.GetUIMgr().showUIForm(formName);
    }
}
