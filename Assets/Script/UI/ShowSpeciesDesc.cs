using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ShowSpeciesDesc : MonoBehaviour
{
    public int m_SpeciesID;
    public string m_SpeciesName;
    public string m_Desc;
    public string m_Link;
    public SpeciesInfo m_Info;
    public string m_Effect;     //特殊功能
    public SpeciesDescPanel m_DescPanel;
    public Text m_Count;
    Image m_Photo;

    public void ShowDescPanel() {
        int count = UserData.m_EnabledSpecies[m_SpeciesID];
        if (count == 0) return;
        m_DescPanel.m_Image.sprite = m_Photo.sprite;
        m_DescPanel.m_SpeciesName.text = m_SpeciesName;
        m_DescPanel.m_Desc.text = m_Desc;
        m_DescPanel.m_Link = m_Link;
        
        m_DescPanel.m_SkillAmount[0].text = m_Info.m_MetalAmount.ToString();
        m_DescPanel.m_SkillAmount[1].text = m_Info.m_WoodAmount.ToString();
        m_DescPanel.m_SkillAmount[2].text = m_Info.m_WaterAmount.ToString();
        m_DescPanel.m_SkillAmount[3].text = m_Info.m_FireAmount.ToString();
        m_DescPanel.m_SkillAmount[4].text = m_Info.m_EarthAmount.ToString();
        
        m_DescPanel.m_Effect.text = "特殊功能: "+m_Effect;
        m_DescPanel.OpenMenu();
    }

    // Use this for initialization
    void Start()
    {
        m_Photo = gameObject.transform.FindChild("Image").GetComponent<Image>();
        int count = UserData.m_EnabledSpecies[m_SpeciesID];
        GetComponent<Button>().interactable = enabled;
        if (count > 0)
        {
            m_Photo.color = Color.white;
            m_Count.text = count.ToString();
        }
        else {
            m_Photo.color = Color.black;
            m_Count.text = "";
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
