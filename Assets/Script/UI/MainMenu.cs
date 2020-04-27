using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {
    public float m_WaitTime = 1.0f;
    public float m_MoveTime = 1.0f;
    public Vector2 m_StartPos, m_EndPos;
    RectTransform m_RectTransform;
    int m_State = 0;    //0: wait, 1: move, 2: stop
    float m_Time = 0;
    public AudioClip m_AudioClick;
    AudioSource m_AudioSource;
    
	// Use this for initialization
	void Start () {
        //UserData.LoadUserData();
        m_RectTransform = GetComponent<RectTransform>();
        m_RectTransform.anchoredPosition = m_StartPos;
        m_AudioSource = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
        m_Time += Time.deltaTime;
        switch (m_State)
        {
            case 0: //wait
                if (m_Time >= m_WaitTime) {
                    m_Time = 0;
                    m_State = 1;
                }
                break;
            case 1: //move
                float alpha = m_Time / m_MoveTime;
                if (alpha >= 1)
                {
                    alpha = 1;
                    m_State = 2;
                }
                m_RectTransform.anchoredPosition = Vector2.Lerp(m_StartPos, m_EndPos, alpha);
                break;
            case 2: //stop
                break;
        }
        if (Input.GetKeyUp(KeyCode.Escape)) {
            ExitGame();
        }
	}

    public void StartGame() {
        if (UserData.m_SoundOn == 1)
        {
            m_AudioSource.PlayOneShot(m_AudioClick);
        }
        SceneManager.LoadScene("SelectLevel");
    }

    public void ExitGame() {
        if (UserData.m_SoundOn == 1)
        {
            m_AudioSource.PlayOneShot(m_AudioClick);
        }
        Application.Quit();
    }

    public void GoToProjectPage() {
        Application.OpenURL("http://agawork.tw");
    }
}
