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
    private static IStoreController m_StoreController; // �洢��Ʒ��Ϣ;
    private static IExtensionProvider m_StoreExtensionProvider; // IAP��չ����;
    private bool m_PurchaseInProgress = false; // �Ƿ��ڸ�����;

    private const string C_ITEM_0 = "com.xxx.xxx.productname"; // ע������ͳһСд(IOS��Google Paly ����);

    public void Init()
    {
        if (m_StoreController == null && m_StoreExtensionProvider == null)
            InitUnityPurchase();
    }

    private bool IsInitialized()
    {
        return m_StoreController != null && m_StoreExtensionProvider != null;
    }

    // ��ʼ��IAP;
    public void InitUnityPurchase()
    {
        if (IsInitialized()) return;
        // ��׼�ɹ�ģ��;
        StandardPurchasingModule module = StandardPurchasingModule.Instance();
        // ����ģʽ;
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
        //��ʼ��;
        UnityPurchasing.Initialize(this, builder);
    }

#region Public Func
    // ����ID��������Ʒ;
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

    // ȷ�Ϲ����Ʒ�ɹ�;
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

    // �ָ�����;
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
                // ����һ��boolֵ������ɹ�������ε���֧���ص���Ȼ�����֧���ص��еĲ����õ���Ʒid�����������(ProcessPurchase); 
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
    // IAP��ʼ���ɹ��ص�����;
    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        IAPDebugLog("OnInitialized Succ !");
#if UNITY_IOS
        /*UIManager.GetUIMgr().showUIForm("ErrForm");
        MessageMgr.SendMsg("ErrorDesc", new MsgKV("", "֧����ʼ���ɹ�"));*/
#endif
        m_StoreController = controller;
        m_StoreExtensionProvider = extensions;

        // ������Ի�ȡ����AppStore��Google Play �����õ���Ʒ;
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
    
    // IAP��ʼ��ʧ�ܻص�������û�����������²�������𣬶���һֱ�ȵ������������ٳ��Գ�ʼ����;
    public void OnInitializeFailed(InitializationFailureReason error)
    {
#if UNITY_IOS
        UIManager.GetUIMgr().showUIForm("ErrForm");
        MessageMgr.SendMsg("ErrorDesc", new MsgKV("", "ʧ�ܻص�����:" + error));
        IAPDebugLog(error.ToString());
        switch (error)
        {
            case InitializationFailureReason.AppNotKnown:
                IAPDebugLogError("Is your App correctly uploaded on the relevant publisher console?");
                MessageMgr.SendMsg("ErrorDesc", new MsgKV("", "����Ӧ�ó����Ƿ���ȷ�ϴ�����ط����߿���̨��"));
                break;
            case InitializationFailureReason.PurchasingUnavailable:
                IAPDebugLog("Billing disabled! Ask the user if billing is disabled in device settings.");
                MessageMgr.SendMsg("ErrorDesc", new MsgKV("", "�Ʒ��ѽ��ã�ѯ���û��Ƿ����豸�����н����˼Ʒ�"));
                break;
            case InitializationFailureReason.NoProductsAvailable:
                IAPDebugLog("No products available for purchase! Developer configuration error; check product metadata!");
                MessageMgr.SendMsg("ErrorDesc", new MsgKV("", "û�пɹ�����Ĳ�Ʒ��������Ա���ô��󣻼���ƷԪ����"));
                break;
        }
#endif
    }

    public void OnInitializeFailed(InitializationFailureReason error, string message)
    {
        UIManager.GetUIMgr().showUIForm("ErrForm");
        MessageMgr.SendMsg("ErrorDesc", new MsgKV("", error.ToString() + "  Payment initialization failed, payment function unavailable:" + message));
        IAPDebugLog(error.ToString() + "  ʧ�ܻص�����:" + message);
        switch (error)
        {
            case InitializationFailureReason.AppNotKnown:
                //����Ӧ�ó����Ƿ���ȷ�ϴ�����ط����߿���̨��
                IAPDebugLogError("Is your App correctly uploaded on the relevant publisher console?");
                MessageMgr.SendMsg("ErrorDesc", new MsgKV("", "Is your App correctly uploaded on the relevant publisher console?"));
                break;
            case InitializationFailureReason.PurchasingUnavailable:
                //�Ʒ��ѽ��ã�ѯ���û��Ƿ����豸�����н����˼Ʒѡ�
                IAPDebugLog("Billing disabled! Ask the user if billing is disabled in device settings.");
                MessageMgr.SendMsg("ErrorDesc", new MsgKV("", "Billing disabled! Ask the user if billing is disabled in device settings."));
                break;
            case InitializationFailureReason.NoProductsAvailable:
                //û�пɹ�����Ĳ�Ʒ��������Ա���ô��󣻼���ƷԪ����
                IAPDebugLog("No products available for purchase! Developer configuration error; check product metadata!");
                MessageMgr.SendMsg("ErrorDesc", new MsgKV("", "No products available for purchase! Developer configuration error; check product metadata!"));
                break;
        }
    }

    // ֧���ɹ�������;
    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs e)
    {
        IAPDebugLog("Purchase OK: " + e.purchasedProduct.definition.id);

        // ��Ϣ�ṹ : Receipt: {"Store":"fake","TransactionID":"9c5c16a5-1ae4-468f-806d-bc709440448a","Payload":"{ \"this\" : \"is a fake receipt\" }"};
        IAPDebugLog("Receipt: " + e.purchasedProduct.receipt);

        // ���ݲ�ͬ��id������Ӧ�Ĵ���(����һ�ִ���ʽ����Ȼ�����Ը����Լ���ϲ��������);
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

        // �����Լ���̨��ϵĻ���ͨ���������óɹ�(����ǲ���Ҫ��̨����ֱ��������ϣ���Ҫ����Pending);
        // return PurchaseProcessingResult.Pending;  
        m_PurchaseInProgress = false;
        return PurchaseProcessingResult.Complete;
    }

    // ֧��ʧ�ܻص�����;
    public void OnPurchaseFailed(Product item, PurchaseFailureReason p)
    {
        m_PurchaseInProgress = false;

        if (p == PurchaseFailureReason.UserCancelled)
        {
            //�û�ȡ�� ����ʾ
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

    // �ָ�������ִ�лص�����;
    private void OnTransactionsRestored(bool success)
    {
        IAPDebugLog("Transactions restored.");
    }

    // �����ӳ���ʾ(������Լ���Ŀ����Ƿ���);
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
                Debug.LogWarningFormat("[Singleton]��{0}���ѱ����Ϊ���٣��� Null��", _type.Name);
                return (T)((object)null);
            }
            lock (_lock)
            {
                if (_instance == null)
                {
                    _instance = (T)FindObjectOfType(_type);
                    if (FindObjectsOfType(_type).Length > 1)
                    {
                        Debug.LogErrorFormat("[Singleton]���͡�{0}�����ڶ��ʵ��.", _type.Name);
                        return _instance;
                    }
                    if (_instance == null)
                    {
                        object[] customAttributes = _type.GetCustomAttributes(typeof(AutoSingletonAttribute), true);
                        AutoSingletonAttribute autoAttribute = (customAttributes.Length > 0) ? (AutoSingletonAttribute)customAttributes[0] : null;
                        if (null == autoAttribute || !autoAttribute.autoCreate)
                        {
                            Debug.LogWarningFormat("[Singleton]�����ʵ�����{0}���������������˷��Զ�����~", _type.Name);
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
                                Debug.LogErrorFormat("[Singleton]���͡�{0}��ResPath�����˴����·����{1}��", _type.Name, autoAttribute.resPath);
                                return (T)((object)null);
                            }
                            _instance = go.GetComponent<T>();
                            if (null == _instance)
                            {
                                Debug.LogErrorFormat("[Singleton]ָ��Ԥ����δ���ظýű���{0}����ResPath��{1}��", _type.Name, autoAttribute.resPath);
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
            Debug.Log("�������µĿ�¡�壡");
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
            if (!transform.parent) //Unity ֻ����������ڵ�� ��Ϸ�������ټ��ء�
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
    /// ��� _destroyed ��
    /// </summary>
    public static void ClearDestroy()
    {
        DestroyInstance();
        _destroyed = false;
    }

    private static bool _destroyed = false;
    /// <summary>
    /// ������ֹͣʱ��Unity �������˳�����ٶ���
    /// ������ gameObject ���������������٣����ų���������ٴα����õĿ����ԡ�
    /// �ʶ��ڱ༭��ģʽ�£����㲥��ֹͣ�ˣ�Ҳ���ܻ�����һ�� gameObject ��������ڱ༭�������С�
    /// ���ԣ��˷����мӰ��������ⲻ��Ҫ�ĵ�������
    /// </summary>
    public void OnDestroy()
    {
        if (_instance != null && _instance.gameObject == base.gameObject)
        {
            _instance = (T)((object)null);
            _destroyed = true;
        }
    }

    /// <summary>Awake ��ʼ�����֮�� </summary>
    public virtual void OnInit()
    {
        Debug.Log("OnInit");
    }

}

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class AutoSingletonAttribute : Attribute
{
    public bool autoCreate; //�Ƿ��Զ���������
    public string resPath;  //��ָ����Ԥ����·�����ɵ���

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



