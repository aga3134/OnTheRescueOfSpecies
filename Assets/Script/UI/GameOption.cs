using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameOption : MonoBehaviour {
    public float m_MoveTime = 0.3f;
    public Vector2 m_OpenPos, m_ClosePos;
    public Toggle m_MusicToggle;
    public Toggle m_SoundToggle;
    RectTransform m_RectTransform;
    int m_State = 0;    //0: Static, 1: Opening, 2: Closing
    float m_Time = 0;
    AudioSource m_BgMusic;
    public AudioClip m_AudioClick;
    AudioSource m_AudioSource;

	// Use this for initialization
	void Start () {
        m_RectTransform = GetComponent<RectTransform>();
        m_RectTransform.anchoredPosition = m_ClosePos;
        m_BgMusic = GameObject.Find("BgMusic").GetComponent<AudioSource>();
        m_AudioSource = GetComponent<AudioSource>();
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
        m_MusicToggle.isOn = (UserData.m_MusicOn==1);
        m_SoundToggle.isOn = (UserData.m_SoundOn==1);
        if (UserData.m_SoundOn == 1) {
            m_AudioSource.PlayOneShot(m_AudioClick);
        }
    }

    public void CloseMenu()
    {
        if (m_State == 2) return;
        m_State = 2;
        m_Time = 0;
        if (UserData.m_SoundOn == 1)
        {
            m_AudioSource.PlayOneShot(m_AudioClick);
        }
    }

    public void OnMusicToggle(){
        if (m_MusicToggle.isOn)
        {
            UserData.m_MusicOn = 1;
            m_BgMusic.Play();
        }
        else
        {
            UserData.m_MusicOn = 0;
            m_BgMusic.Stop();
        }
        UserData.SaveUserData();
    }

    public void OnSoundToggle(){
        if (m_SoundToggle.isOn) UserData.m_SoundOn = 1;
        else UserData.m_SoundOn = 0;
        UserData.SaveUserData();
    }
}
