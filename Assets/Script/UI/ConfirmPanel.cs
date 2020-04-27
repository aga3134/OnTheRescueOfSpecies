using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ConfirmPanel : MonoBehaviour {
    public Text m_Title;
    public Image m_Image;
    public Text m_Info;
    public float m_MoveTime = 0.3f;
    public Vector2 m_OpenPos, m_ClosePos;
    RectTransform m_RectTransform;
    int m_State = 0;    //0: Static, 1: Opening, 2: Closing
    float m_Time = 0;
    GameControl m_GameControl;

	// Use this for initialization
	void Start () {
        m_RectTransform = GetComponent<RectTransform>();
        m_RectTransform.anchoredPosition = m_ClosePos;
        m_GameControl = GameObject.Find("GameContainer").GetComponent<GameControl>();
	}
	
	// Update is called once per frame
	void Update () {
        m_Time += Time.deltaTime;
        float alpha = m_Time / m_MoveTime;
        switch (m_State)
        {
            case 0: //static
                break;
            case 1: //opening
                if (alpha >= 1)
                {
                    m_State = 0;
                    alpha = 1;
                }
                m_RectTransform.anchoredPosition = Vector2.Lerp(m_ClosePos, m_OpenPos, alpha);
                break;
            case 2: //closing
                if (alpha >= 1)
                {
                    m_State = 0;
                    alpha = 1;
                }
                m_RectTransform.anchoredPosition = Vector2.Lerp(m_OpenPos, m_ClosePos, alpha);
                break;
        }
	}

    public void OpenMenu()
    {
        if (m_State == 1) return;
        m_State = 1;
        m_Time = 0;
        m_GameControl.SetPause(true);
    }

    public void CloseMenu()
    {
        if (m_State == 2) return;
        m_State = 2;
        m_Time = 0;
        m_GameControl.SetPause(false);
    }
}
