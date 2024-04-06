using System;
using UnityEngine;
using UnityEngine.Purchasing;

public class IAPT2 : MonoSingleton<IAPTools>, IStoreListener
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
        builder.AddProduct("com.manhuang.tk.1", ProductType.Consumable);
        builder.AddProduct("com.manhuang.tk.2", ProductType.Consumable);
        builder.AddProduct("com.manhuang.tk.3", ProductType.Consumable);
        builder.AddProduct("com.manhuang.tk.4", ProductType.Consumable);
        builder.AddProduct("com.manhuang.tk.5", ProductType.Consumable);
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
                IAPDebugLog("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
            }
        }
        else
        {
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

    // 支付成功处理函数;
    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs e)
    {
        IAPDebugLog("Purchase OK: " + e.purchasedProduct.definition.id);

        // 消息结构 : Receipt: {"Store":"fake","TransactionID":"9c5c16a5-1ae4-468f-806d-bc709440448a","Payload":"{ \"this\" : \"is a fake receipt\" }"};
        IAPDebugLog("Receipt: " + e.purchasedProduct.receipt);

        // 根据不同的id，做对应的处理(这是一种处理方式，当然您可以根据自己的喜好来处理);
        if (String.Equals(e.purchasedProduct.definition.id, C_ITEM_0, StringComparison.Ordinal))
        {
            // TODO::

        }
        //Messenger.Raise("PurchaseSuccess", e.purchasedProduct.definition.id);

        // 我们自己后台完毕的话，通过代码设置成功(如果是不需要后台设置直接设置完毕，不要设置Pending);
        // return PurchaseProcessingResult.Pending;  
        m_PurchaseInProgress = false;
        return PurchaseProcessingResult.Complete;
    }

    // 支付失败回掉函数;
    public void OnPurchaseFailed(Product item, PurchaseFailureReason r)
    {
        m_PurchaseInProgress = false;
       // UIFloatingManager.Instance.Show("支付失败");
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