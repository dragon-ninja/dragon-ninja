using System;
using UnityEngine;
using UnityEngine.Purchasing;

public class IAPT2 : MonoSingleton<IAPTools>, IStoreListener
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
        builder.AddProduct("com.manhuang.tk.1", ProductType.Consumable);
        builder.AddProduct("com.manhuang.tk.2", ProductType.Consumable);
        builder.AddProduct("com.manhuang.tk.3", ProductType.Consumable);
        builder.AddProduct("com.manhuang.tk.4", ProductType.Consumable);
        builder.AddProduct("com.manhuang.tk.5", ProductType.Consumable);
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
                IAPDebugLog("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
            }
        }
        else
        {
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
        switch (error)
        {
            case InitializationFailureReason.AppNotKnown:
                IAPDebugLogError("Is your App correctly uploaded on the relevant publisher console?");
                break;
            case InitializationFailureReason.PurchasingUnavailable:
                IAPDebugLog("Billing disabled! Ask the user if billing is disabled in device settings.");
                break;
            case InitializationFailureReason.NoProductsAvailable:
                IAPDebugLog("No products available for purchase! Developer configuration error; check product metadata!");
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
        if (String.Equals(e.purchasedProduct.definition.id, C_ITEM_0, StringComparison.Ordinal))
        {
            // TODO::

        }
        //Messenger.Raise("PurchaseSuccess", e.purchasedProduct.definition.id);

        // �����Լ���̨��ϵĻ���ͨ���������óɹ�(����ǲ���Ҫ��̨����ֱ��������ϣ���Ҫ����Pending);
        // return PurchaseProcessingResult.Pending;  
        m_PurchaseInProgress = false;
        return PurchaseProcessingResult.Complete;
    }

    // ֧��ʧ�ܻص�����;
    public void OnPurchaseFailed(Product item, PurchaseFailureReason r)
    {
        m_PurchaseInProgress = false;
       // UIFloatingManager.Instance.Show("֧��ʧ��");
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
        Debug.LogError("IAP------" + arg);
    }
    private void IAPDebugLog(string arg)
    {
        Debug.Log("IAP------" + arg);
    }

    public void OnInitializeFailed(InitializationFailureReason error, string message)
    {
        throw new NotImplementedException();
    }
}