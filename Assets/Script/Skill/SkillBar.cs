using UnityEngine;
using System.Collections;

public class SkillBar : MonoBehaviour {
    public GameObject[] m_MetalSkillTemplate;
    public GameObject[] m_WoodSkillTemplate;
    public GameObject[] m_WaterSkillTemplate;
    public GameObject[] m_FireSkillTemplate;
    public GameObject[] m_EarthSkillTemplate;

	// Use this for initialization
	void Start () {
        //UserData.m_MetalSkillLevel = 1;
        //UserData.m_WoodSkillLevel = 1;
        //UserData.m_WaterSkillLevel = 1;
        //UserData.m_FireSkillLevel = 1;
        //UserData.m_EarthSkillLevel = 1;
        Vector3 pos = new Vector3(-180, 0, 0);
        float offsetX = 90;
        AddSkill(UserData.m_MetalSkillLevel, m_MetalSkillTemplate, pos, UserData.m_MetalSkillInit==1);
        pos.x += offsetX;
        AddSkill(UserData.m_WoodSkillLevel, m_WoodSkillTemplate, pos, UserData.m_WoodSkillInit == 1);
        pos.x += offsetX;
        AddSkill(UserData.m_WaterSkillLevel, m_WaterSkillTemplate, pos, UserData.m_WaterSkillInit == 1);
        pos.x += offsetX;
        AddSkill(UserData.m_FireSkillLevel, m_FireSkillTemplate, pos, UserData.m_FireSkillInit == 1);
        pos.x += offsetX;
        AddSkill(UserData.m_EarthSkillLevel, m_EarthSkillTemplate, pos, UserData.m_EarthSkillInit == 1);
	}

    void AddSkill(int level, GameObject[] template, Vector3 pos, bool setReady) {
        int id = level-1;
        if (id < 0 || id >= template.Length) return;
        GameObject skill = Instantiate(template[id]);
        skill.transform.SetParent(transform);
        skill.transform.localPosition = pos;
        skill.transform.localScale = Vector3.one;

        if (setReady) {
            skill.GetComponent<SkillButton>().SetReadyOnInit();
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
