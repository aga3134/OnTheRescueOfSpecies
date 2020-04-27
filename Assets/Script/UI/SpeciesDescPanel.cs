using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SpeciesDescPanel : MonoBehaviour {
    public Text m_SpeciesCount;
    public Image m_Image;
    public Text m_SpeciesName;
    public Text m_Desc;
    public Text[] m_SkillAmount;
    public Text m_Effect;
    public string m_Link;
    public GridLayoutGroup m_GLG;
    public AudioClip m_AudioClick;
    AudioSource m_AudioSource;

    public float m_MoveTime = 0.3f;
    public Vector2 m_OpenPos, m_ClosePos;
    RectTransform m_RectTransform;
    int m_State = 0;    //0: Static, 1: Opening, 2: Closing
    float m_Time = 0;

	// Use this for initialization
	void Start () {
        m_AudioSource = GetComponent<AudioSource>();
        m_RectTransform = GetComponent<RectTransform>();
        m_RectTransform.anchoredPosition = m_ClosePos;
        int count = 0;
        for (int i = 0; i < UserData.m_EnabledSpecies.Length; i++) {
            if (UserData.m_EnabledSpecies[i] > 0) {
                count++;
            }
        }
        m_SpeciesCount.text = count + " / " + UserData.m_EnabledSpecies.Length;

        //update grid size to fix ui height
        /*var rectTransform = m_GLG.gameObject.GetComponent<RectTransform>();
        float h = rectTransform.rect.height;
        float size = (h - 50*7)/6;
        Debug.Log(size);
        m_GLG.cellSize = Vector2.one*size;
        rectTransform.sizeDelta = new Vector2(size*10+50*11, rectTransform.sizeDelta.y);*/
	}
	
	// Update is called once per frame
	void Update () {
        if(Input.GetKeyUp(KeyCode.Escape)){
            BackToSelectLevel();
        }

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

    public void GoToLink() {
        Application.OpenURL(m_Link);
    }

    public void BackToSelectLevel()
    {
        if (UserData.m_SoundOn == 1)
        {
            m_AudioSource.PlayOneShot(m_AudioClick);
        }
        SceneManager.LoadScene("SelectLevel");
    }

    public void OpenMenu()
    {
        if (m_State == 1) return;
        if (UserData.m_SoundOn == 1)
        {
            m_AudioSource.PlayOneShot(m_AudioClick);
        }
        m_State = 1;
        m_Time = 0;
    }

    public void CloseMenu()
    {
        if (m_State == 2) return;
        if (UserData.m_SoundOn == 1)
        {
            m_AudioSource.PlayOneShot(m_AudioClick);
        }
        m_State = 2;
        m_Time = 0;
    }
}
