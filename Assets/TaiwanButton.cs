using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TaiwanButton : MonoBehaviour {
    public GameObject m_TaiwanPanel;
    public float m_MoveTime = 0.3f;
    public Vector2 m_OpenPos, m_ClosePos;
    public AudioClip m_AudioClick;
    AudioSource m_AudioSource;

    Image m_TaiwanBt;
    float m_Time = 0;
    int m_State = 0;    //0: Static, 1: Opening, 2: Closing
    RectTransform m_RectTransform;

	// Use this for initialization
	void Start () {
        m_AudioSource = GetComponent<AudioSource>();
        m_TaiwanBt = GetComponent<Image>();
        m_RectTransform = m_TaiwanPanel.GetComponent<RectTransform>();
        m_RectTransform.anchoredPosition = m_ClosePos;
	}
	
	// Update is called once per frame
	void Update () {
        float flashRatio = (Mathf.Sin(m_Time*2) + 1) * 0.5f;
        m_TaiwanBt.color = Color.Lerp(new Color(1, 1, 0, 0.7f), new Color(1, 1, 0, 0.2f), flashRatio);

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

    public void OpenTaiwanPanel() {
        if (m_State == 1) return;
        if (UserData.m_SoundOn == 1)
        {
            m_AudioSource.PlayOneShot(m_AudioClick);
        }
        m_State = 1;
        m_Time = 0;
    }

    public void CloseTaiwanPanel() {
        if (m_State == 2) return;
        if (UserData.m_SoundOn == 1)
        {
            m_AudioSource.PlayOneShot(m_AudioClick);
        }
        m_State = 2;
        m_Time = 0;
    }
}
