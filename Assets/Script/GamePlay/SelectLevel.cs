using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class SelectLevel : MonoBehaviour
{
    public float m_SwipeThresh = 500.0f;
    public float m_MoveDistance = -1000;
    public float m_MoveTime = 0.5f;
    public Button[] m_LevelBt;
    public Image m_LevelBg;
    public GameObject[] m_UnlockBt;
    public Sprite[] m_BgArray;
    int m_CurIndex = 0;
    float m_CurTime = 0;
    bool m_IsMoving = false;
    Vector3 m_CurPos, m_TargetPos;
    Vector3 m_PivotMousePos;
    RectTransform m_RT;
    public OutOfResource m_OOR;
    public AudioClip m_AudioClick;
    AudioSource m_AudioSource;

    // Use this for initialization
    void Start()
    {
        //UserData.m_LevelUnlock = 4;
        //UserData.m_Life = 5;
        for (int i = 0; i < m_UnlockBt.Length; i++) {
            m_UnlockBt[i].SetActive(UserData.m_LevelUnlock <= i);
        }
        m_RT = gameObject.GetComponent<RectTransform>();
        m_PivotMousePos = Input.mousePosition;
        m_CurPos = m_RT.anchoredPosition3D;

        m_CurIndex = UserData.m_CurLevel;
        //Debug.Log(m_CurIndex);
        m_CurTime = 0;
        m_TargetPos = new Vector3(m_CurIndex * m_MoveDistance, 0, 0);
        m_IsMoving = true;

        m_AudioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape)){
            GoToMainMenu();
        }
        else if (Input.GetMouseButtonDown(0)) {
            m_PivotMousePos = Input.mousePosition;
        }

        if (!m_IsMoving)
        {
            if (Input.GetMouseButton(0))
            {
                Vector3 mouseVec = (Input.mousePosition - m_PivotMousePos) / Time.deltaTime;

                if (mouseVec.x > m_SwipeThresh)
                {
                    SwipeRight();
                }
                else if (mouseVec.x < -m_SwipeThresh)
                {
                    SwipeLeft();
                }
            }
        }
        else
        {
            m_CurTime += Time.deltaTime;

            m_RT.anchoredPosition3D = Vector3.Lerp(m_CurPos, m_TargetPos, m_CurTime / m_MoveTime);
            //Debug.Log(transform.position);
            if (m_CurTime >= m_MoveTime)
            {
                m_IsMoving = false;
                m_LevelBg.sprite = m_BgArray[m_CurIndex];
            }
        }
        
    }

    void SwipeLeft()
    {
        if (m_CurIndex < m_LevelBt.Length - 1)
        {
            m_CurIndex++;
            m_CurTime = 0;
            m_CurPos = m_RT.anchoredPosition3D;
            m_TargetPos = new Vector3(m_CurIndex * m_MoveDistance, 0, 0);
            m_IsMoving = true;
        }
        
    }

    void SwipeRight()
    {
        if (m_CurIndex > 0)
        {
            m_CurIndex--;
            m_CurTime = 0;
            m_CurPos = m_RT.anchoredPosition3D;
            m_TargetPos = new Vector3(m_CurIndex * m_MoveDistance, 0, 0);
            m_IsMoving = true;
        }
    }

    public void LoadLevel(string name) {
        if (m_IsMoving) return;
        if (UserData.m_SoundOn == 1)
        {
            m_AudioSource.PlayOneShot(m_AudioClick);
        }
        //Debug.Log("Load "+name);
        m_OOR.m_Pause = true;
        switch (name) {
            case "AgeAgriculture": UserData.m_CurLevel = 0; break;
            case "AgeFactory": UserData.m_CurLevel = 1; break;
            case "AgeNetwork": UserData.m_CurLevel = 2; break;
            case "AgeNuclearWar": UserData.m_CurLevel = 3; break;
            case "AgeFormosa": UserData.m_CurLevel = 4; break;
        }
        UserData.SubLife();
        SceneManager.LoadScene(name);
    }

    public void GoToUpgradeSystem(){
        if (UserData.m_SoundOn == 1)
        {
            m_AudioSource.PlayOneShot(m_AudioClick);
        }
        SceneManager.LoadScene("Upgrade");
    }

    public void GoToMainMenu(){
        if (UserData.m_SoundOn == 1)
        {
            m_AudioSource.PlayOneShot(m_AudioClick);
        }
        SceneManager.LoadScene("MainMenu");
    }

    public void GoToSpecies()
    {
        if (UserData.m_SoundOn == 1)
        {
            m_AudioSource.PlayOneShot(m_AudioClick);
        }
        SceneManager.LoadScene("Species");
    }

}

