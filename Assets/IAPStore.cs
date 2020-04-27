using System;
using UnityEngine;
using UnityEngine.Purchasing;

public class IAPStore : MonoBehaviour, IStoreListener {
    static string m_MetalInitID = "on_the_rescue_of_species_metal_init";
    static string m_WoodInitID = "on_the_rescue_of_species_wood_init";
    static string m_WaterInitID = "on_the_rescue_of_species_water_init";
    static string m_FireInitID = "on_the_rescue_of_species_fire_init";
    static string m_EarthInitID = "on_the_rescue_of_species_earth_init";
    
    IStoreController m_StoreController;
    UpgradeSystem m_US;
    void Awake()
    {
        ConfigurationBuilder builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
        builder.AddProduct(m_MetalInitID, ProductType.NonConsumable);
        builder.AddProduct(m_WoodInitID, ProductType.NonConsumable);
        builder.AddProduct(m_WaterInitID, ProductType.NonConsumable);
        builder.AddProduct(m_FireInitID, ProductType.NonConsumable);
        builder.AddProduct(m_EarthInitID, ProductType.NonConsumable);
        UnityPurchasing.Initialize(this, builder);
    }

    void Start() {
        m_US = GetComponent<UpgradeSystem>();
    }

    public void PurchaseItem(string id) { 
        if (m_StoreController != null)
        {
            Product product = m_StoreController.products.WithID(id);
            if (product != null && product.availableToPurchase)
            {
                m_StoreController.InitiatePurchase(product);
            }
        }
    }

    public void PurchaseMetalInit()
    {
        PurchaseItem(m_MetalInitID);
    }

    public void PurchaseWoodInit()
    {
        PurchaseItem(m_WoodInitID);
    }

    public void PurchaseWaterInit()
    {
        PurchaseItem(m_WaterInitID);
    }

    public void PurchaseFireInit()
    {
        PurchaseItem(m_FireInitID);
    }

    public void PurchaseEarthInit()
    {
        PurchaseItem(m_EarthInitID);
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        m_StoreController = controller;

        //get purchased item and update user data
        Product product;
        product = m_StoreController.products.WithID(m_MetalInitID);
        if (product != null && product.hasReceipt)
        {
            UserData.m_MetalSkillInit = 1;
        }
        product = m_StoreController.products.WithID(m_WoodInitID);
        if (product != null && product.hasReceipt)
        {
            UserData.m_WoodSkillInit = 1;
        }
        product = m_StoreController.products.WithID(m_WaterInitID);
        if (product != null && product.hasReceipt)
        {
            UserData.m_WaterSkillInit = 1;
        }
        product = m_StoreController.products.WithID(m_FireInitID);
        if (product != null && product.hasReceipt)
        {
            UserData.m_FireSkillInit = 1;
        }
        product = m_StoreController.products.WithID(m_EarthInitID);
        if (product != null && product.hasReceipt)
        {
            UserData.m_EarthSkillInit = 1;
        }
        UserData.SaveUserData();
    }

    public void OnInitializeFailed(InitializationFailureReason error) { }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs e)
    {
        if (String.Equals(e.purchasedProduct.definition.id, m_MetalInitID, StringComparison.Ordinal)){
            UserData.m_MetalSkillInit = 1;
        }
        else if (String.Equals(e.purchasedProduct.definition.id, m_WoodInitID, StringComparison.Ordinal)) 
        {
            UserData.m_WoodSkillInit = 1;
        }
        else if (String.Equals(e.purchasedProduct.definition.id, m_WaterInitID, StringComparison.Ordinal))
        {
            UserData.m_WaterSkillInit = 1;
        }
        else if (String.Equals(e.purchasedProduct.definition.id, m_FireInitID, StringComparison.Ordinal))
        {
            UserData.m_FireSkillInit = 1;
        }
        else if (String.Equals(e.purchasedProduct.definition.id, m_EarthInitID, StringComparison.Ordinal))
        {
            UserData.m_EarthSkillInit = 1;
        }
        UserData.SaveUserData();
        m_US.UpdateUpgradeItem(false);

        return PurchaseProcessingResult.Complete;
    }

    public void OnPurchaseFailed(Product item, PurchaseFailureReason r) { }
}
