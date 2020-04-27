using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameMenu : MonoBehaviour {
    public float m_MoveTime = 0.3f;
    public Vector2 m_OpenPos, m_ClosePos;
    RectTransform m_RectTransform;
    GameControl m_GameControl;
    int m_State = 0;    //0: Static, 1: Opening, 2: Closing
    float m_Time = 0;
    public AudioClip m_AudipClick;
    AudioSource m_AudioSource;

	// Use this for initialization
	void Start () {
        m_RectTransform = GetComponent<RectTransform>();
        m_GameControl = GameObject.Find("GameContainer").GetComponent<GameControl>();
        m_RectTransform.anchoredPosition = m_ClosePos;
        m_AudioSource = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyUp(KeyCode.Escape)) {
            OpenMenu();
        }

        m_Time += Time.deltaTime;
        float alpha = m_Time / m_MoveTime;
        switch(m_State){
            case 0: //static
                break;
            case 1: //opening
                if (alpha >= 1) {
                    m_State = 0;
                    alpha = 1;
                }
                m_RectTransform.anchoredPosition = Vector2.Lerp(m_ClosePos, m_OpenPos, alpha);
                break;
            case 2: //closing
                if (alpha >= 1) {
                    m_State = 0;
                    alpha = 1;
                    m_GameControl.SetPause(false);
                }
                m_RectTransform.anchoredPosition = Vector2.Lerp(m_OpenPos, m_ClosePos, alpha);
                break;
        }

	}

    public void OpenMenu() {
        if (m_State == 1) return;
        m_State = 1;
        m_Time = 0;
        m_GameControl.SetPause(true);
        if (UserData.m_SoundOn == 1)
        {
            m_AudioSource.PlayOneShot(m_AudipClick);
        }
    }

    public void CloseMenu() {
        if (m_State == 2) return;
        m_State = 2;
        m_Time = 0;
        if (UserData.m_SoundOn == 1)
        {
            m_AudioSource.PlayOneShot(m_AudipClick);
        }
    }

    public void ToMainMenu() {
        if (UserData.m_SoundOn == 1)
        {
            m_AudioSource.PlayOneShot(m_AudipClick);
        }
        SceneManager.LoadScene("MainMenu");
    }

    public void ToSelectLevel()
    {
        if (UserData.m_SoundOn == 1)
        {
            m_AudioSource.PlayOneShot(m_AudipClick);
        }
        SceneManager.LoadScene("SelectLevel");
    }
    
}
