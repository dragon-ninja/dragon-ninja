using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.Linq;

public class UIManager : MonoBehaviour
{
    private static UIManager uiMgr;
    //ui预设体路径
    private Dictionary<string, string> UIformPathMap;
    //缓存的ui面板
    private Dictionary<string, BaseUIForm> UIformMap;
    //当前显示的ui面板
    private Dictionary<string, BaseUIForm> NowShowUIFormMap;
    //UI栈,存储退回上层类型的ui面板
    private Stack<BaseUIForm> UIFormStack;
    //根节点
    private Transform baseNode;
    //全屏显示节点
    private Transform NormalNode;
    //固定显示节点
    private Transform FixedNode;
    //弹出显示节点
    private Transform PopUpNode;
    //UIManager节点
    private Transform UIMgrNode;

    public static UIManager GetUIMgr()
    {
        if(uiMgr == null) {
            GameObject g = GameObject.Find("UIManager");
            if(g != null)
                uiMgr = g.GetComponent<UIManager>();
        }
        
        return uiMgr;
    }

    private void Awake()
    {
        uiMgr = GameObject.Find("UIManager").GetComponent<UIManager>();
        MessageMgr.init();


        UIformPathMap = new Dictionary<string, string>();
        UIformMap = new Dictionary<string, BaseUIForm>();
        NowShowUIFormMap = new Dictionary<string, BaseUIForm>();
        UIFormStack = new Stack<BaseUIForm>();

        //根节点
        GameObject baseNodeGobj = GameObject.Find("BaseUICanvas");
        if (baseNodeGobj == null)
        {
            baseNode = GameObject.Instantiate(Resources.Load<GameObject>(SysDefine.SYS_BaseUICanvas)).transform;
            baseNode.GetComponent<Canvas>().worldCamera = Camera.main;
        }
        else
        {
            baseNode = baseNodeGobj.transform;
        }
         baseNode.transform.SetAsLastSibling();
        
        baseNode.GetComponent<Canvas>().sortingOrder = 10;

        //全屏显示节点
        NormalNode = baseNode.Find("Normal");
        //固定显示节点
        FixedNode = baseNode.Find("Fixed");
        //弹出显示节点
        PopUpNode = baseNode.Find("PopUp");
        //UIManager节点
        //UIMgrNode = baseNode.Find("UIMgr");
        //this.transform.SetParent(UIMgrNode);
        //DontDestroyOnLoad(baseNode); //todo

        //初始化资源路径
        TextAsset ta = Resources.Load<TextAsset>("UI/UIPath");
        Dictionary<string, string> pathmap = JsonConvert.DeserializeObject<Dictionary<string, string>>(ta.text);
        foreach (string str in pathmap.Keys)
        {
            UIformPathMap.Add(str, pathmap[str]);
        }
    }

    //显示form
    public void showUIForm(string formName)
    {
        if (string.IsNullOrWhiteSpace(formName)) return;

        BaseUIForm uf = loadUIFormToFormMap(formName);

        if (uf == null) return;

        if (uf.ui_type.IsClearStack)
            clearUIStack();

        switch (uf.ui_type.ui_ShowType)
        {
            case UIformShowMode.Normal:
                loadUIFormToNowShow(formName);
                break;
            case UIformShowMode.ReverseChange:
                pushUIFormToStack(formName);
                break;
            case UIformShowMode.HideOther:
                showUIFormAndHideOther(formName);
                break;
            default:
                break;
        }
    }

    //关闭form
    public void closeUIForm(string formName)
    {
        if (string.IsNullOrWhiteSpace(formName) || !UIformMap.ContainsKey(formName))
            return;

        switch (UIformMap[formName].ui_type.ui_ShowType)
        {
            case UIformShowMode.Normal:
                exitUIForm(formName);
                break;
            case UIformShowMode.ReverseChange:
                popUIFormToStack();
                break;
            case UIformShowMode.HideOther:
                exitUIFormAndShowOther(formName);
                break;
            default:
                break;
        }
    }

    public bool checkUIForm(string formName) {
        return UIformMap.ContainsKey(formName);
    }

    //预加载form
    public void preload(string formName) {
        if (string.IsNullOrWhiteSpace(formName)) return;

        BaseUIForm uf = loadUIFormToFormMap(formName);
        uf.Show();
        uf.Hide();
    }

    public void SetCanvasReaderMode(RenderMode renderMode)
    {
      if(baseNode == null)
      {
          return;
      }
      Canvas CurCanvas = baseNode.GetComponent<Canvas>();
      if(CurCanvas == null)
      {
          return;
      }
      CurCanvas.renderMode = renderMode;
      switch(renderMode)
      {
          case RenderMode.ScreenSpaceCamera:
              CurCanvas.worldCamera = Camera.main;
            break;
      }
    }

    #region private
    //获取指定UI窗体
    private BaseUIForm loadUIFormToFormMap(string formName)
    {
        BaseUIForm uf = null;
        UIformMap.TryGetValue(formName, out uf);

        if (uf == null)
        {
            uf = loadUIForm(formName);
        }
        else {
           /* uf.Show();
            uf.Hide();*/
        }
        return uf;
    }

    //读取资源路径加载UI窗体
    private BaseUIForm loadUIForm(string formName)
    {
        BaseUIForm uf = null;

        //先尝试找到场景里的元素  预加载好的资源
        Transform pf = GameObject.Find("UIManager").transform.Find(formName);
        if (pf != null)
        {
            uf = pf.GetComponent<BaseUIForm>();
            uf.Show();
            uf.Hide();
            uf.gameObject.SetActive(true);
        }
        else { 
            uf = GameObject.Instantiate(Resources.Load<GameObject>(UIformPathMap[formName])).GetComponent<BaseUIForm>();
            uf.Show();
            uf.Hide();
        }

          
        if (uf == null)
        {
            Debug.LogError("未获取到资源,检查资源:" + formName + "是否异常");
            return null;
        }

        UIformMap.Add(formName, uf);

        switch (uf.ui_type.ui_FormType)
        {
            case UIformType.Normal:
                uf.transform.SetParent(NormalNode, false);
                break;
            case UIformType.Fixed:
                uf.transform.SetParent(FixedNode, false);
                break;
            case UIformType.PopUp:
                uf.transform.SetParent(PopUpNode, false);
                break;
            default:
                break;
        }

        uf.gameObject.SetActive(false);

        return uf;
    }

    //加载到当前显示中
    private void loadUIFormToNowShow(string formName)
    {
        if (NowShowUIFormMap.ContainsKey(formName))
        {
            return;
        }
        else if (UIformMap.ContainsKey(formName))
        {
            NowShowUIFormMap.Add(formName, UIformMap[formName]);
            UIformMap[formName].Show();
        }
    }

    //从当前显示中移除
    private void exitUIForm(string formName)
    {
        if (NowShowUIFormMap.ContainsKey(formName))
        {
            NowShowUIFormMap[formName].Hide();
            NowShowUIFormMap.Remove(formName);
        }
    }

    //加载到栈显示中
    private void pushUIFormToStack(string formName)
    {
        //若栈中有其他窗体,则进行冻结
        if (UIFormStack.Count > 0)
        {
            UIFormStack.Peek().Freeze();
        }

        if (UIformMap.ContainsKey(formName))
        {
            UIFormStack.Push(UIformMap[formName]);
            UIformMap[formName].Show();
        }
        else
        {
            Debug.LogError(formName + "存在异常");
        }
    }

    //从栈中弹出首个UI窗体
    private void popUIFormToStack()
    {
        if (UIFormStack.Count >= 2)
        {
            UIFormStack.Pop().Hide();
            UIFormStack.Peek().ReShow();
        }
        else if (UIFormStack.Count > 0)
        {
            UIFormStack.Pop().Hide();
        }
    }

    //显示form并隐藏其他
    private void showUIFormAndHideOther(string formName)
    {
        //若已正在显示 不做处理
        if (NowShowUIFormMap.ContainsKey(formName))
            return;

        //将正在显示,栈中的UI全部隐藏
        for (int i = NowShowUIFormMap.Count-1; i >= 0 ;i--) {
            var item = NowShowUIFormMap.ElementAt(i);
            if (item.Key != formName && item.Value.ui_type.ui_FormType 
                != UIformType.Fixed)
            {
                NowShowUIFormMap.Remove(item.Key);
                item.Value.Hide();
            }
        }
        foreach (BaseUIForm uf in UIFormStack)
        {
            uf.Hide();
        }
        //将当前的窗体加入"正在显示窗体"集合中并进行显示
        NowShowUIFormMap.Add(formName, UIformMap[formName]);
        NowShowUIFormMap[formName].Show();
    }

    //退出form并重新显示其他
    private void exitUIFormAndShowOther(string formName)
    {
        //若已不处于显示中 不做处理
        if (NowShowUIFormMap[formName] == null)
            return;

        NowShowUIFormMap[formName].Hide();
        NowShowUIFormMap.Remove(formName);

        //将正在显示,栈中的UI全部隐藏
        foreach (BaseUIForm uf in NowShowUIFormMap.Values)
        {
            uf.ReShow();
        }
        foreach (BaseUIForm uf in UIFormStack)
        {
            uf.ReShow();
        }
    }

    //清空栈
    private void clearUIStack()
    {
        UIFormStack.Clear();
    }
    #endregion

}
