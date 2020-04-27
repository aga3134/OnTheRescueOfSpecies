using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;

public class UpgradeSystem : MonoBehaviour {
    public Text m_MaterialRes;
    public Text m_DNARes;
    public AudioClip m_AudioClick;
    AudioSource m_AudioSource;

    //upgrade item templates
    public GameObject[] m_MetalSkill;
    public GameObject[] m_WoodSkill;
    public GameObject[] m_WaterSkill;
    public GameObject[] m_FireSkill;
    public GameObject[] m_EarthSkill;
    public GameObject[] m_TimeAcc;
    public GameObject[] m_MetalInit;
    public GameObject[] m_WoodInit;
    public GameObject[] m_WaterInit;
    public GameObject[] m_FireInit;
    public GameObject[] m_EarthInit;

    List<GameObject> m_ItemList = new List<GameObject>();

	// Use this for initialization
	void Start () {
        m_AudioSource = GetComponent<AudioSource>();
        UpdateUpgradeItem(false);

        //UserData.ResetUserData();
        //UserData.m_MaterialRes = 100000;
        //UserData.m_DNARes = 10000;
	}

    public void PlayClickSound() {
        if (UserData.m_SoundOn == 1)
        {
            m_AudioSource.PlayOneShot(m_AudioClick);
        }
    }

    public void UpdateUpgradeItem(bool sound) {
        if(sound) PlayClickSound();

        if(m_ItemList != null){
            for (int i = 0; i < m_ItemList.Count; i++) {
                DestroyObject(m_ItemList[i]);
            }
        }
        m_ItemList.Clear();

        m_MaterialRes.text = UserData.m_MaterialRes.ToString();
        m_DNARes.text = UserData.m_DNARes.ToString();

        int id = UserData.m_MetalSkillLevel;
        if (id >= 0 && id < m_MetalSkill.Length) AddItem(m_MetalSkill[id]);
        else AddItem(m_MetalSkill[m_MetalSkill.Length-1]);

        id = UserData.m_WoodSkillLevel;
        if (id < m_WoodSkill.Length) AddItem(m_WoodSkill[id]);
        else AddItem(m_WoodSkill[m_WoodSkill.Length - 1]);

        id = UserData.m_WaterSkillLevel;
        if (id >= 0 && id < m_WaterSkill.Length) AddItem(m_WaterSkill[id]);
        else AddItem(m_WaterSkill[m_WaterSkill.Length - 1]);

        id = UserData.m_FireSkillLevel;
        if (id >= 0 && id < m_FireSkill.Length) AddItem(m_FireSkill[id]);
        else AddItem(m_FireSkill[m_FireSkill.Length - 1]);

        id = UserData.m_EarthSkillLevel;
        if (id >= 0 && id < m_EarthSkill.Length) AddItem(m_EarthSkill[id]);
        else AddItem(m_EarthSkill[m_EarthSkill.Length - 1]);

        id = UserData.m_TimeAccLevel;
        if (id >= 0 && id < m_TimeAcc.Length) AddItem(m_TimeAcc[id]);
        else AddItem(m_TimeAcc[m_TimeAcc.Length - 1]);

        id = UserData.m_MetalSkillInit;
        if (id >= 0 && id < m_MetalInit.Length) AddItem(m_MetalInit[id]);
        else AddItem(m_MetalInit[m_MetalInit.Length - 1]);

        id = UserData.m_WoodSkillInit;
        if (id >= 0 && id < m_WoodInit.Length) AddItem(m_WoodInit[id]);
        else AddItem(m_WoodInit[m_WoodInit.Length - 1]);

        id = UserData.m_WaterSkillInit;
        if (id >= 0 && id < m_WaterInit.Length) AddItem(m_WaterInit[id]);
        else AddItem(m_WaterInit[m_WaterInit.Length - 1]);

        id = UserData.m_FireSkillInit;
        if (id >= 0 && id < m_FireInit.Length) AddItem(m_FireInit[id]);
        else AddItem(m_FireInit[m_FireInit.Length - 1]);

        id = UserData.m_EarthSkillInit;
        if (id >= 0 && id < m_EarthInit.Length) AddItem(m_EarthInit[id]);
        else AddItem(m_EarthInit[m_EarthInit.Length - 1]);
    }

    void AddItem(GameObject template) {
        GameObject item = Instantiate(template);
        item.transform.SetParent(transform);
        item.transform.localScale = Vector3.one;
        m_ItemList.Add(item);
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            BackToSelectLevel();
        }
	}

    public void BackToSelectLevel(){
        if (UserData.m_SoundOn == 1)
        {
            m_AudioSource.PlayOneShot(m_AudioClick);
        }
        SceneManager.LoadScene("SelectLevel");
    }
}
