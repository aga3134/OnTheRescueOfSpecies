using UnityEngine;
using System.Collections;

public class SpeciesInfo : MonoBehaviour {
    public int m_SpeciesID;
    public string m_SpeciesName;
    public float m_ShowChance;
    public int m_MetalAmount = 0;
    public int m_WoodAmount = 0;
    public int m_WaterAmount = 0;
    public int m_FireAmount = 0;
    public int m_EarthAmount = 0;
    SkillButton[] m_SkillBt = new SkillButton[5];
    int[] m_Amount = new int[5];
    ConfirmPanel m_ConfirmPanel;

	// Use this for initialization
	void Start () {
        m_ConfirmPanel = GameObject.Find("ConfirmPanel").GetComponentInChildren<ConfirmPanel>();

        for (int i = 0; i < m_SkillBt.Length; i++) {
            m_SkillBt[i] = BoxElementControl.GetSkillButton(i);
        }
        m_Amount[0] = m_MetalAmount;
        m_Amount[1] = m_WoodAmount;
        m_Amount[2] = m_WaterAmount;
        m_Amount[3] = m_FireAmount;
        m_Amount[4] = m_EarthAmount;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void SpeciesRescued() {
        for (int i = 0; i < m_SkillBt.Length; i++){
            if (m_SkillBt[i] != null){
                m_SkillBt[i].AddAmount(m_Amount[i]);
            }
        }

        UserData.RescueSpecies(m_SpeciesID);
        int count = UserData.m_EnabledSpecies[m_SpeciesID];
        if (count == 1) {

            m_ConfirmPanel.m_Title.text = "救援成功";
            m_ConfirmPanel.m_Image.sprite = GetComponent<SpriteRenderer>().sprite;
            m_ConfirmPanel.m_Info.text = m_SpeciesName;
            m_ConfirmPanel.OpenMenu();
        }
        else if (count % 10 == 0) {
            m_ConfirmPanel.m_Title.text = "救援 "+count+" 次";
            m_ConfirmPanel.m_Image.sprite = GetComponent<SpriteRenderer>().sprite;
            m_ConfirmPanel.m_Info.text = m_SpeciesName;
            m_ConfirmPanel.OpenMenu();
            //消除跟目前物種同色的方塊
            GameObject gc = GameObject.FindGameObjectWithTag("GameController");
            GameObject box = gameObject.transform.parent.gameObject;
            BoxElementControl bec = box.GetComponent<BoxElementControl>();
            Debug.Log(bec.m_Type);
            gc.GetComponent<BoxGenerator>().EliminateBoxWithType(bec.m_Type);
        }
    }
}
