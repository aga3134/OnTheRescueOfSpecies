using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverPanel : MonoBehaviour {
    public Text m_GameMaterial, m_GameDNA;  //game over panel的resource
    public Text m_HighestMaterial, m_HighestDNA;
    public float m_MoveTime = 0.3f;
    public Vector2 m_OpenPos, m_ClosePos;
    RectTransform m_RectTransform;
    GameControl m_GameControl;
    Text m_SourceMaterial, m_SourceDNA; //game topbar的resource
    int m_State = 0;    //0: Static, 1: Opening, 2: Closing
    float m_Time = 0;
    public AudioClip m_AudioClick;
    public AudioClip m_AudioGameOver;
    AudioSource m_AudioSource;
    AudioSource m_BgMusic;

	// Use this for initialization
	void Start () {
        m_RectTransform = GetComponent<RectTransform>();
        m_GameControl = GameObject.Find("GameContainer").GetComponent<GameControl>();
        m_RectTransform.anchoredPosition = m_ClosePos;
        m_SourceMaterial = GameObject.Find("MaterialText").GetComponent<Text>();
        m_SourceDNA = GameObject.Find("DNAText").GetComponent<Text>();
        m_AudioSource = GetComponent<AudioSource>();
        m_BgMusic = GameObject.Find("BgMusic").GetComponent<AudioSource>();
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
                    m_GameControl.SetPause(false);
                }
                m_RectTransform.anchoredPosition = Vector2.Lerp(m_OpenPos, m_ClosePos, alpha);
                break;
        }
	}

    public void OpenMenu()
    {
        if (UserData.m_SoundOn == 1)
        {
            m_AudioSource.PlayOneShot(m_AudioGameOver);
        }
        if (UserData.m_MusicOn == 1) {
            m_BgMusic.Stop();
        }

        Button bt = GameObject.Find("InfoBar").GetComponentInChildren<Button>();
        bt.enabled = false;

        //update game score
        m_GameMaterial.text = m_SourceMaterial.text;
        m_GameDNA.text = m_SourceDNA.text;
        int srcMaterial = int.Parse(m_SourceMaterial.text);
        int srcDNA = int.Parse(m_SourceDNA.text);
        UserData.m_MaterialRes += srcMaterial;
        UserData.m_DNARes += srcDNA;

        //update highest score
        int levelIndex = SceneManager.GetActiveScene().buildIndex-2;
        if (srcMaterial > UserData.m_HighestMaterial[levelIndex])
        {
            UserData.m_HighestMaterial[levelIndex] = srcMaterial;
        }
        if (srcDNA > UserData.m_HighestDNA[levelIndex])
        {
            UserData.m_HighestDNA[levelIndex] = srcDNA;
        }
        m_HighestMaterial.text = UserData.m_HighestMaterial[levelIndex].ToString();
        m_HighestDNA.text = UserData.m_HighestDNA[levelIndex].ToString();

        
        //check level unlock
        if (UserData.m_CurLevel == UserData.m_LevelUnlock) {
            switch (UserData.m_LevelUnlock)
            {
                case 0: //農業 -> 工業
                    if (srcMaterial >= 300) UserData.m_LevelUnlock++;
                    break;
                case 1: //工業 -> 資訊
                    if (srcDNA >= 200) UserData.m_LevelUnlock++;
                    break;
                case 2: //資訊 -> 核戰
                    if (srcMaterial >= 1500 && srcDNA >= 200) UserData.m_LevelUnlock++;
                    break;
                case 3: //核戰 -> 寶島
                    break;
            }
        }

        UserData.SaveUserData();
        m_State = 1;
        m_Time = 0;
        m_GameControl.SetPause(true);
    }

    public void CloseMenu()
    {
        m_State = 2;
        m_Time = 0;
    }

    public void ToSelectLevel()
    {
        if (UserData.m_SoundOn == 1)
        {
            m_AudioSource.PlayOneShot(m_AudioClick);
        }
        SceneManager.LoadScene("SelectLevel");
    }
}
