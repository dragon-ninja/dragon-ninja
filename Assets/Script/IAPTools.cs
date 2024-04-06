using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
//using TK;
//using UGF.Singleton;
//using UGF.UI;
using UnityEngine;
using UnityEngine.Purchasing;

public class IAPTools : MonoSingleton<IAPTools>, IStoreListener
{
    private static IStoreController m_StoreController; // 存储商品信息;
    private static IExtensionProvider m_StoreExtensionProvider; // IAP扩展工具;
    private bool m_PurchaseInProgress = false; // 是否处于付费中;

    private const string C_ITEM_0 = "com.xxx.xxx.productname"; // 注意这里统一小写(IOS和Google Paly 公用);

    public void Init()
    {
        if (m_StoreController == null && m_StoreExtensionProvider == null)
            InitUnityPurchase();
    }

    private bool IsInitialized()
    {
        return m_StoreController != null && m_StoreExtensionProvider != null;
    }

    // 初始化IAP;
    public void InitUnityPurchase()
    {
        if (IsInitialized()) return;
        // 标准采购模块;
        StandardPurchasingModule module = StandardPurchasingModule.Instance();
        // 配置模式;
        ConfigurationBuilder builder = ConfigurationBuilder.Instance(module);

#if UNITY_IOS
        builder.AddProduct("paper_cut_199", ProductType.Consumable);
        builder.AddProduct("paper_cut_1999", ProductType.Consumable);
        builder.AddProduct("paper_cut_299", ProductType.Consumable);
        builder.AddProduct("paper_cut_2999", ProductType.Consumable);
        builder.AddProduct("paper_cut_399", ProductType.Consumable);
        builder.AddProduct("paper_cut_499", ProductType.Consumable);
        builder.AddProduct("paper_cut_4999", ProductType.Consumable);
        builder.AddProduct("paper_cut_699", ProductType.Consumable);
        builder.AddProduct("paper_cut_99", ProductType.Consumable);
        builder.AddProduct("paper_cut_999", ProductType.Consumable);
        builder.AddProduct("paper_cut_9999", ProductType.Consumable);
#else
        //builder.AddProduct("paper_cut_gid_199xxxx", ProductType.Consumable);
        builder.AddProduct("paper_cut_gid_199", ProductType.Consumable);
        builder.AddProduct("paper_cut_gid_1999", ProductType.Consumable);
        builder.AddProduct("paper_cut_gid_299", ProductType.Consumable);
        builder.AddProduct("paper_cut_gid_2999", ProductType.Consumable);
        builder.AddProduct("paper_cut_gid_399", ProductType.Consumable);
        builder.AddProduct("paper_cut_gid_499", ProductType.Consumable);
        builder.AddProduct("paper_cut_gid_4999", ProductType.Consumable);
        builder.AddProduct("paper_cut_gid_699", ProductType.Consumable);
        builder.AddProduct("paper_cut_gid_99", ProductType.Consumable);
        builder.AddProduct("paper_cut_gid_999", ProductType.Consumable);
        builder.AddProduct("paper_cut_gid_9999", ProductType.Consumable);
#endif
        //builder.AddProduct("basic_sleeping_potion", ProductType.Consumable);
        //初始化;
        UnityPurchasing.Initialize(this, builder);
    }

#region Public Func
    // 根据ID给购买商品;
    public void BuyProductByID(string productId)
    {

        if (IsInitialized())
        {
            if (m_PurchaseInProgress == true) return;

            Product product = m_StoreController.products.WithID(productId);
            if (product != null && product.availableToPurchase)
            {
                IAPDebugLog(string.Format("Purchasing product asychronously: '{0}'", product.definition.id));
                m_StoreController.InitiatePurchase(product);
                m_PurchaseInProgress = true;
            }
            else
            {

                UIManager.GetUIMgr().showUIForm("ErrForm");
                MessageMgr.SendMsg("ErrorDesc", new MsgKV("", "BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase"));

                IAPDebugLog("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
            }
        }
        else
        {
            UIManager.GetUIMgr().showUIForm("ErrForm");
            MessageMgr.SendMsg("ErrorDesc", new MsgKV("", "BuyProductID FAIL. Not initialized"));
            IAPDebugLog("BuyProductID FAIL. Not initialized.");
            Init();
        }
    }

    // 确认购买产品成功;
    public void DoConfirmPendingPurchaseByID(string productId)
    {
        Product product = m_StoreController.products.WithID(productId);
        if (product != null && product.availableToPurchase)
        {
            if (m_PurchaseInProgress)
            {
                m_StoreController.ConfirmPendingPurchase(product);
                m_PurchaseInProgress = false;
            }
        }
    }

    // 恢复购买;
    public void RestorePurchases()
    {
        if (!IsInitialized())
        {
            IAPDebugLog("RestorePurchases FAIL. Not initialized.");
            return;
        }
        if (Application.platform == RuntimePlatform.IPhonePlayer ||
            Application.platform == RuntimePlatform.OSXPlayer)
        {
            IAPDebugLog("RestorePurchases started ...");
            var apple = m_StoreExtensionProvider.GetExtension<IAppleExtensions>();
            apple.RestoreTransactions((result) => {
                // 返回一个bool值，如果成功，则会多次调用支付回调，然后根据支付回调中的参数得到商品id，最后做处理(ProcessPurchase); 
                IAPDebugLog("RestorePurchases continuing: " + result + ". If no further messages, no purchases available to restore.");
            });
        }
        else
        {
            IAPDebugLog("RestorePurchases FAIL. Not supported on this platform. Current = " + Application.platform);
        }
    }
#endregion

#region IStoreListener Callback
    // IAP初始化成功回掉函数;
    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        IAPDebugLog("OnInitialized Succ !");
#if UNITY_IOS
        /*UIManager.GetUIMgr().showUIForm("ErrForm");
        MessageMgr.SendMsg("ErrorDesc", new MsgKV("", "支付初始化成功"));*/
#endif
        m_StoreController = controller;
        m_StoreExtensionProvider = extensions;

        // 这里可以获取您在AppStore和Google Play 上配置的商品;
        ProductCollection products = m_StoreController.products;
        Product[] all = products.all;
        for (int i = 0; i < all.Length; i++)
        {
            IAPDebugLog(all[i].metadata.localizedTitle + "|" + all[i].metadata.localizedPriceString + "|" + all[i].metadata.localizedDescription + "|" + all[i].metadata.isoCurrencyCode);
        }

#if UNITY_IOS
        // m_AppleExtensions.RegisterPurchaseDeferredListener(OnDeferred);
#endif
    }
    
    // IAP初始化失败回掉函数（没有网络的情况下并不会调起，而是一直等到有网络连接再尝试初始化）;
    public void OnInitializeFailed(InitializationFailureReason error)
    {
#if UNITY_IOS
        UIManager.GetUIMgr().showUIForm("ErrForm");
        MessageMgr.SendMsg("ErrorDesc", new MsgKV("", "失败回调触发:" + error));
        IAPDebugLog(error.ToString());
        switch (error)
        {
            case InitializationFailureReason.AppNotKnown:
                IAPDebugLogError("Is your App correctly uploaded on the relevant publisher console?");
                MessageMgr.SendMsg("ErrorDesc", new MsgKV("", "您的应用程序是否正确上传到相关发布者控制台？"));
                break;
            case InitializationFailureReason.PurchasingUnavailable:
                IAPDebugLog("Billing disabled! Ask the user if billing is disabled in device settings.");
                MessageMgr.SendMsg("ErrorDesc", new MsgKV("", "计费已禁用！询问用户是否在设备设置中禁用了计费"));
                break;
            case InitializationFailureReason.NoProductsAvailable:
                IAPDebugLog("No products available for purchase! Developer configuration error; check product metadata!");
                MessageMgr.SendMsg("ErrorDesc", new MsgKV("", "没有可供购买的产品！开发人员配置错误；检查产品元数据"));
                break;
        }
#endif
    }

    public void OnInitializeFailed(InitializationFailureReason error, string message)
    {
        UIManager.GetUIMgr().showUIForm("ErrForm");
        MessageMgr.SendMsg("ErrorDesc", new MsgKV("", error.ToString() + "  Payment initialization failed, payment function unavailable:" + message));
        IAPDebugLog(error.ToString() + "  失败回调触发:" + message);
        switch (error)
        {
            case InitializationFailureReason.AppNotKnown:
                //您的应用程序是否正确上传到相关发布者控制台？
                IAPDebugLogError("Is your App correctly uploaded on the relevant publisher console?");
                MessageMgr.SendMsg("ErrorDesc", new MsgKV("", "Is your App correctly uploaded on the relevant publisher console?"));
                break;
            case InitializationFailureReason.PurchasingUnavailable:
                //计费已禁用！询问用户是否在设备设置中禁用了计费。
                IAPDebugLog("Billing disabled! Ask the user if billing is disabled in device settings.");
                MessageMgr.SendMsg("ErrorDesc", new MsgKV("", "Billing disabled! Ask the user if billing is disabled in device settings."));
                break;
            case InitializationFailureReason.NoProductsAvailable:
                //没有可供购买的产品！开发人员配置错误；检查产品元数据
                IAPDebugLog("No products available for purchase! Developer configuration error; check product metadata!");
                MessageMgr.SendMsg("ErrorDesc", new MsgKV("", "No products available for purchase! Developer configuration error; check product metadata!"));
                break;
        }
    }

    // 支付成功处理函数;
    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs e)
    {
        IAPDebugLog("Purchase OK: " + e.purchasedProduct.definition.id);

        // 消息结构 : Receipt: {"Store":"fake","TransactionID":"9c5c16a5-1ae4-468f-806d-bc709440448a","Payload":"{ \"this\" : \"is a fake receipt\" }"};
        IAPDebugLog("Receipt: " + e.purchasedProduct.receipt);

        // 根据不同的id，做对应的处理(这是一种处理方式，当然您可以根据自己的喜好来处理);
        //if (String.Equals(e.purchasedProduct.definition.id, C_ITEM_0, StringComparison.Ordinal))
        {
            JObject obj = (JObject)JsonConvert.DeserializeObject(e.purchasedProduct.receipt);
            PayReturn payReturn = obj.ToObject<PayReturn>();
            //IAPDebugLog("payReturn.TransactionID: " + payReturn.TransactionID+"  === "+ e.purchasedProduct.definition.id);
           /* MessageMgr.SendMsg("PayEnd", 
                new MsgKV("", e.purchasedProduct.definition.id + 
                "|" + payReturn.TransactionID));*/
#if UNITY_IOS
      MessageMgr.SendMsg("PayEnd", 
                new MsgKV("", e.purchasedProduct.definition.id + 
                "|" + e.purchasedProduct.receipt));   
#else
            MessageMgr.SendMsg("PayEnd", 
                    new MsgKV("", e.purchasedProduct.definition.id + 
                    "|" + payReturn.TransactionID));
#endif


        }
        //Messenger.Raise("PurchaseSuccess", e.purchasedProduct.definition.id);

        // 我们自己后台完毕的话，通过代码设置成功(如果是不需要后台设置直接设置完毕，不要设置Pending);
        // return PurchaseProcessingResult.Pending;  
        m_PurchaseInProgress = false;
        return PurchaseProcessingResult.Complete;
    }

    // 支付失败回掉函数;
    public void OnPurchaseFailed(Product item, PurchaseFailureReason p)
    {
        m_PurchaseInProgress = false;

        if (p == PurchaseFailureReason.UserCancelled)
        {
            //用户取消 不提示
        }
        else if (p == PurchaseFailureReason.PurchasingUnavailable)
        {
            UIManager.GetUIMgr().showUIForm("ErrForm");
            MessageMgr.SendMsg("ErrorDesc", new MsgKV("", "Unable to use the system purchase function"));
        }
        else if (p == PurchaseFailureReason.ExistingPurchasePending)
        {
            UIManager.GetUIMgr().showUIForm("ErrForm");
            MessageMgr.SendMsg("ErrorDesc", new MsgKV("", "The previous purchase was in progress when requesting a new purchase"));
        }
        else if (p == PurchaseFailureReason.ProductUnavailable)
        {
            UIManager.GetUIMgr().showUIForm("ErrForm");
            MessageMgr.SendMsg("ErrorDesc", new MsgKV("", "Unable to purchase goods in the store"));
        }
        else if (p == PurchaseFailureReason.SignatureInvalid)
        {
            UIManager.GetUIMgr().showUIForm("ErrForm");
            MessageMgr.SendMsg("ErrorDesc", new MsgKV("", "The signature verification of the purchase receipt failed"));
        }
        else if (p == PurchaseFailureReason.PaymentDeclined)
        {
            UIManager.GetUIMgr().showUIForm("ErrForm");
            MessageMgr.SendMsg("ErrorDesc", new MsgKV("", "There is an issue with the payment."));
        }
        else if (p == PurchaseFailureReason.DuplicateTransaction)
        {
            UIManager.GetUIMgr().showUIForm("ErrForm");
            MessageMgr.SendMsg("ErrorDesc", new MsgKV("", "A duplicate transaction error that occurs when the transaction has been successfully completed."));
        }
        else {
            UIManager.GetUIMgr().showUIForm("ErrForm");
            MessageMgr.SendMsg("ErrorDesc", new MsgKV("", "Common reasons for unidentified purchase issues."));
        }
    }

    // 恢复购买功能执行回掉函数;
    private void OnTransactionsRestored(bool success)
    {
        IAPDebugLog("Transactions restored.");
    }

    // 购买延迟提示(这个看自己项目情况是否处理);
    private void OnDeferred(Product item)
    {
        IAPDebugLog("Purchase deferred: " + item.definition.id);
    }
#endregion

    private void IAPDebugLogError(string arg)
    {
        //GameUtil.ShowToast(arg);

        UIManager.GetUIMgr().showUIForm("ErrForm");
        MessageMgr.SendMsg("ErrorDesc", new MsgKV("", arg));

        Debug.LogError("IAP---DebugLogError---" + arg);
    }
    private void IAPDebugLog(string arg)
    {
        //GameUtil.ShowToast(arg);
        //UIManager.GetUIMgr().showUIForm("ErrForm");
        //MessageMgr.SendMsg("ErrorDesc", new MsgKV("", arg));


        Debug.Log("IAP---DebugLog---" + arg);
    }

   
}


[AutoSingleton(true)]
public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;
    private static object _lock = new object();
    public static T Instance
    {
        get
        {
            Type _type = typeof(T);
            if (_destroyed)
            {
                Debug.LogWarningFormat("[Singleton]【{0}】已被标记为销毁，返 Null！", _type.Name);
                return (T)((object)null);
            }
            lock (_lock)
            {
                if (_instance == null)
                {
                    _instance = (T)FindObjectOfType(_type);
                    if (FindObjectsOfType(_type).Length > 1)
                    {
                        Debug.LogErrorFormat("[Singleton]类型【{0}】存在多个实例.", _type.Name);
                        return _instance;
                    }
                    if (_instance == null)
                    {
                        object[] customAttributes = _type.GetCustomAttributes(typeof(AutoSingletonAttribute), true);
                        AutoSingletonAttribute autoAttribute = (customAttributes.Length > 0) ? (AutoSingletonAttribute)customAttributes[0] : null;
                        if (null == autoAttribute || !autoAttribute.autoCreate)
                        {
                            Debug.LogWarningFormat("[Singleton]欲访问单例【{0}】不存在且设置了非自动创建~", _type.Name);
                            return (T)((object)null);
                        }
                        GameObject go = null;
                        if (string.IsNullOrEmpty(autoAttribute.resPath))
                        {
                            go = new GameObject(_type.Name);
                            _instance = go.AddComponent<T>();
                        }
                        else
                        {
                            go = Resources.Load<GameObject>(autoAttribute.resPath);
                            if (null != go)
                            {
                                go = GameObject.Instantiate(go);
                            }
                            else
                            {
                                Debug.LogErrorFormat("[Singleton]类型【{0}】ResPath设置了错误的路径【{1}】", _type.Name, autoAttribute.resPath);
                                return (T)((object)null);
                            }
                            _instance = go.GetComponent<T>();
                            if (null == _instance)
                            {
                                Debug.LogErrorFormat("[Singleton]指定预制体未挂载该脚本【{0}】，ResPath【{1}】", _type.Name, autoAttribute.resPath);
                            }
                        }
                    }
                }
                return _instance;
            }
        }
    }

    protected virtual void Awake()
    {
        if (_instance != null && _instance.gameObject != gameObject)
        {
            Debug.Log("创造了新的克隆体！");
            if (Application.isPlaying)
            {
                GameObject.Destroy(gameObject);
            }
            else
            {
                GameObject.DestroyImmediate(gameObject);
            }
        }
        else
        {
            _instance = GetComponent<T>();
            if (!transform.parent) //Unity 只允许最最根节点的 游戏对象不销毁加载。
            {
                DontDestroyOnLoad(gameObject);
            }
            OnInit();
        }
    }

    public static void DestroyInstance()
    {
        if (_instance != null)
        {
            GameObject.Destroy(_instance.gameObject);
        }
        _destroyed = true;
        _instance = (T)((object)null);
    }

    /// <summary>
    /// 清除 _destroyed 锁
    /// </summary>
    public static void ClearDestroy()
    {
        DestroyInstance();
        _destroyed = false;
    }

    private static bool _destroyed = false;
    /// <summary>
    /// 当播放停止时，Unity 会以随机顺序销毁对象
    /// 若单例 gameObject 先于其他对象销毁，不排除这个单例再次被调用的可能性。
    /// 故而在编辑器模式下，即便播放停止了，也可能会生成一个 gameObject 对象残留在编辑器场景中。
    /// 所以，此方法中加把锁，避免不必要的单例调用
    /// </summary>
    public void OnDestroy()
    {
        if (_instance != null && _instance.gameObject == base.gameObject)
        {
            _instance = (T)((object)null);
            _destroyed = true;
        }
    }

    /// <summary>Awake 初始化完成之后 </summary>
    public virtual void OnInit()
    {
        Debug.Log("OnInit");
    }

}

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class AutoSingletonAttribute : Attribute
{
    public bool autoCreate; //是否自动创建单例
    public string resPath;  //从指定的预制体路径生成单例

    public AutoSingletonAttribute(bool _autoCreate, string _resPath = "")
    {
        this.autoCreate = _autoCreate;
        this.resPath = _resPath;
    }
}



public class PayReturn
{
    public string Payload;
    public string Store;
    public string TransactionID;
}



