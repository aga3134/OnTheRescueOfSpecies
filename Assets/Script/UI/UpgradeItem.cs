using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UpgradeItem : MonoBehaviour {
    public enum ItemType {NONE, METAL_SKILL, WOOD_SKILL, WATER_SKILL, FIRE_SKILL, EARTH_SKILL, TIME_ACC,
        METAL_INIT, WOOD_INIT, WATER_INIT, FIRE_INIT, EARTH_INIT};
    public ItemType m_Type;
    public GameObject m_CostItem;
    public Sprite[] m_CostIcon;
    public int m_MaterialCost = 0;
    public int m_DNACost = 0;
    public int m_MoneyCost = 0;
    UpgradeSystem m_US;
    IAPStore m_IAPStore;

	// Use this for initialization
	void Start () {
        m_US = GameObject.Find("UpgradeSystem").GetComponentInChildren<UpgradeSystem>();
        m_IAPStore = GameObject.Find("UpgradeSystem").GetComponentInChildren<IAPStore>();
        CheckState();

        int posX = 350, posY = -100, offsetX = 200;
        if (m_MaterialCost > 0) {
            AddCostItem(posX, posY, 0, new Color(1,1,0), m_MaterialCost);
            posX += offsetX;
        }
        if (m_DNACost > 0)
        {
            AddCostItem(posX, posY, 1, new Color(1,1,1), m_DNACost);
            posX += offsetX;
        }
        if (m_MoneyCost > 0)
        {
            AddCostItem(posX, posY, 2, new Color(1, 1, 1), m_MoneyCost);
            posX += offsetX;
        }
	}

    void AddCostItem(int posX, int posY, int iconID, Color c, int cost) {
        GameObject costIcon = Instantiate(m_CostItem);
        costIcon.transform.SetParent(transform);
        costIcon.transform.localScale = Vector3.one;
        RectTransform rt = costIcon.GetComponent<RectTransform>();
        Image im = costIcon.GetComponent<Image>();
        im.sprite = m_CostIcon[iconID];
        im.color = c;
        rt.anchoredPosition = new Vector2(posX, posY);
        Text t = costIcon.GetComponentInChildren<Text>();
        t.text = cost.ToString();
    }

    void CheckState() {
        Button upgradeBt = GetComponentInChildren<Button>();
        if (UserData.m_MaterialRes < m_MaterialCost || UserData.m_DNARes < m_DNACost)
        {
            upgradeBt.interactable = false;
        }
        else upgradeBt.interactable = true;

        Text btText = upgradeBt.gameObject.GetComponentInChildren<Text>();
        switch (m_Type){
            case ItemType.METAL_SKILL:
                if (UserData.m_MetalSkillLevel == 3)
                {
                    btText.text = "最高級";
                    upgradeBt.interactable = false;
                }
                break;
            case ItemType.WOOD_SKILL:
                if (UserData.m_WoodSkillLevel == 3)
                {
                    btText.text = "最高級";
                    upgradeBt.interactable = false;
                }
                break;
            case ItemType.WATER_SKILL:
                if (UserData.m_WaterSkillLevel == 3)
                {
                    btText.text = "最高級";
                    upgradeBt.interactable = false;
                }
                break;
            case ItemType.FIRE_SKILL:
                if (UserData.m_FireSkillLevel == 3)
                {
                    btText.text = "最高級";
                    upgradeBt.interactable = false;
                }
                break;
            case ItemType.EARTH_SKILL:
                if (UserData.m_EarthSkillLevel == 3)
                {
                    btText.text = "最高級";
                    upgradeBt.interactable = false;
                }
                break;
            case ItemType.TIME_ACC:
                if (UserData.m_TimeAccLevel == 3)
                {
                    btText.text = "最高級";
                    upgradeBt.interactable = false;
                }
                break;
            case ItemType.METAL_INIT:
                if (UserData.m_MetalSkillInit == 1)
                {
                    btText.text = "已購買";
                    upgradeBt.interactable = false;
                }
                break;
            case ItemType.WOOD_INIT:
                if (UserData.m_WoodSkillInit == 1)
                {
                    btText.text = "已購買";
                    upgradeBt.interactable = false;
                }
                break;
            case ItemType.WATER_INIT:
                if (UserData.m_WaterSkillInit == 1)
                {
                    btText.text = "已購買";
                    upgradeBt.interactable = false;
                }
                break;
            case ItemType.FIRE_INIT:
                if (UserData.m_FireSkillInit == 1)
                {
                    btText.text = "已購買";
                    upgradeBt.interactable = false;
                }
                break;
            case ItemType.EARTH_INIT:
                if (UserData.m_EarthSkillInit == 1)
                {
                    btText.text = "已購買";
                    upgradeBt.interactable = false;
                }
                break;
        }
    }

    public void UpgradeClick() {
        if (m_MoneyCost > 0){   //in app purchase
            m_US.PlayClickSound();
            switch(m_Type){
                case ItemType.METAL_INIT: m_IAPStore.PurchaseMetalInit(); break;
                case ItemType.WOOD_INIT: m_IAPStore.PurchaseWoodInit(); break;
                case ItemType.WATER_INIT: m_IAPStore.PurchaseWaterInit(); break;
                case ItemType.FIRE_INIT: m_IAPStore.PurchaseFireInit(); break;
                case ItemType.EARTH_INIT: m_IAPStore.PurchaseEarthInit(); break;
            }
        }
        else {
            if(UserData.m_MaterialRes >= m_MaterialCost && UserData.m_DNARes >= m_DNACost){
                DoUpgrade();
            }
        }
    }

    void DoUpgrade() {
        UserData.m_MaterialRes -= m_MaterialCost;
        UserData.m_DNARes -= m_DNACost;
        switch (m_Type)
        {
            case ItemType.METAL_SKILL: 
                UserData.m_MetalSkillLevel++;
                break;
            case ItemType.WOOD_SKILL:
                UserData.m_WoodSkillLevel++;
                break;
            case ItemType.WATER_SKILL:
                UserData.m_WaterSkillLevel++;
                break;
            case ItemType.FIRE_SKILL:
                UserData.m_FireSkillLevel++;
                break;
            case ItemType.EARTH_SKILL:
                UserData.m_EarthSkillLevel++;
                break;
            case ItemType.TIME_ACC:
                UserData.m_TimeAccLevel++;
                break;
            case ItemType.METAL_INIT:
                UserData.m_MetalSkillInit++;
                break;
            case ItemType.WOOD_INIT:
                UserData.m_WoodSkillInit++;
                break;
            case ItemType.WATER_INIT:
                UserData.m_WaterSkillInit++;
                break;
            case ItemType.FIRE_INIT:
                UserData.m_FireSkillInit++;
                break;
            case ItemType.EARTH_INIT:
                UserData.m_EarthSkillInit++;
                break;
        }
        UserData.SaveUserData();
        m_US.UpdateUpgradeItem(true);
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
