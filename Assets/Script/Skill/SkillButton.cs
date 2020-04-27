using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SkillButton : MonoBehaviour {
    public GameObject m_PatternTemplate;
    public GameObject m_SkillTemplate;
    public GameObject m_ReadyEffect;
    public float m_CoolDownAmount = 5;
    float m_CurAmount = 0;
    Image m_CoolDownMask;
    GameObject m_Pattern;
    GameObject m_GC;
    BoxGenerator m_BG;
    public bool m_Pause = false;
    float m_TimeAcc = 1;
    bool m_ReadyOnInit = false;

	// Use this for initialization
	void Start () {
	    m_GC = GameObject.FindGameObjectWithTag("GameController");
        m_BG = m_GC.GetComponent<BoxGenerator>();
        m_CoolDownMask = transform.GetChild(0).GetComponent<Image>();
        m_CurAmount = 0;
        if (m_ReadyOnInit) AddAmount(m_CoolDownAmount);

        switch (UserData.m_TimeAccLevel) {
            case 0: m_TimeAcc = 1.0f; break;
            case 1: m_TimeAcc = 1.5f; break;
            case 2: m_TimeAcc = 2.0f; break;
            case 3: m_TimeAcc = 2.5f; break;
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (m_Pause) return;
        AddAmount(Time.deltaTime * m_TimeAcc);
        //AddAmount(Time.deltaTime * m_TimeAcc*100);
	}

    public void AddAmount(float amount) {
        if (m_CurAmount >= m_CoolDownAmount) return;
        m_CurAmount += amount;
        if (m_CurAmount >= m_CoolDownAmount) {
            m_CurAmount = m_CoolDownAmount;
            Instantiate(m_ReadyEffect, new Vector3(transform.position.x,transform.position.y,0), Quaternion.identity);
        }
        m_CoolDownMask.fillAmount = 1 - m_CurAmount / m_CoolDownAmount;
    }

    public void SetReadyOnInit() {
        m_ReadyOnInit = true;
    }

    public void BeginDrag(){
        if (m_Pause) return;
        if (m_CurAmount < m_CoolDownAmount) return;
        m_Pattern = (GameObject)Instantiate(m_PatternTemplate);
        m_Pattern.transform.parent = m_GC.transform;
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pos = m_GC.transform.worldToLocalMatrix * pos;
        m_Pattern.transform.localPosition = new Vector3(pos.x, pos.y, -1);
        m_Pattern.transform.localScale = m_PatternTemplate.transform.localScale;
    }

    public void Drag(){
        if (m_Pause)
        {
            DestroyObject(m_Pattern);
            m_Pattern = null;
            return;
        }
        if (m_Pattern) {
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pos = m_GC.transform.worldToLocalMatrix * pos;
            m_Pattern.transform.localPosition = new Vector3(pos.x, pos.y, -1);
        }
    }

    public void EndDrag(){
        if (m_Pause) return;
        if (m_Pattern) {
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pos = m_GC.transform.worldToLocalMatrix * pos;
            Coord2D coord = m_BG.PosToCoord(new Vector2(pos.x, pos.y));
            if (coord.x != -1 && coord.y != -1) {
                GameObject skill = (GameObject)Instantiate(m_SkillTemplate);
                skill.transform.parent = m_BG.gameObject.transform;
                skill.transform.localPosition = new Vector3(pos.x, pos.y, -1);
                skill.transform.localScale = Vector3.one;
                m_CurAmount = 0;
            }

            DestroyObject(m_Pattern);
            m_Pattern = null;
        }
    }
}
