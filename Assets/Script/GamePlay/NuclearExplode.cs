using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NuclearExplode : MonoBehaviour
{
    public string[] m_WarnMsg;
    public Text m_WarnText;
    int m_WarnIndex = 0;
    public float m_Peried = 60;     //核爆來襲間隔
    public float m_WarnTime = 3;    //警告時間
    public float m_Interval = 20;   //核爆持續時間
    public float m_ExplodeDrop = 0.3f;
    public BoxGenerator m_Generataor;
    int m_State = 0;   //0: 正常, 1:警告, 2: 核爆
    float m_CurTime = 0;
    public bool m_Pause = false;
    Image m_InfoBar, m_SkillBar;
    float m_NormalDropTime, m_NormalFallingTime;
    public AudioClip m_AudioWarn;
    AudioSource m_AudioSource;
    public AudioClip m_NormalMusic;
    public AudioClip m_ExplodeMusic;
    AudioSource m_BgMusic;

    // Use this for initialization
    void Start()
    {
        m_InfoBar = GameObject.Find("InfoBar").GetComponent<Image>();
        m_SkillBar = GameObject.Find("SkillBar").GetComponent<Image>();
        m_NormalDropTime = m_Generataor.m_GenerateCountDown;
        m_NormalFallingTime = m_Generataor.m_FallingTime;
        m_AudioSource = GetComponent<AudioSource>();
        m_BgMusic = GameObject.Find("BgMusic").GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_Pause) return;
        m_CurTime += Time.deltaTime;
        switch (m_State)
        {
            case 0: //正常
                if (m_CurTime >= m_Peried) {
                    m_CurTime = 0;
                    m_State = 1;
                    m_Generataor.m_Halt = true;
                    m_WarnText.text = m_WarnMsg[m_WarnIndex];
                    //Debug.Log("Warning");
                    if (UserData.m_SoundOn == 1)
                    {
                        m_AudioSource.PlayOneShot(m_AudioWarn);
                    }
                }
                break;
            case 1: //警告
                //flash infoBar & skillBar
                float alpha = (Mathf.Sin(Mathf.PI * 2 * 2.5f * m_CurTime / m_WarnTime - Mathf.PI) + 1) * 0.5f;
                m_InfoBar.color = Color.Lerp(Color.red, new Color(0.35f,0.35f,0.35f), alpha);
                m_SkillBar.color = Color.Lerp(Color.red, new Color(0.35f, 0.35f, 0.35f), alpha);
                m_WarnText.color = Color.Lerp(Color.red, new Color(0.35f, 0.35f, 0.35f), alpha);
                if (m_CurTime >= m_WarnTime)
                {
                    m_CurTime = 0;
                    m_State = 2;
                    m_Generataor.m_GenerateCountDown = m_ExplodeDrop;
                    m_Generataor.m_FallingTime = m_ExplodeDrop*0.2f;
                    m_Generataor.m_Halt = false;
                    //Debug.Log("Explode");
                    m_InfoBar.color = Color.red;
                    m_SkillBar.color = Color.red;
                    m_WarnText.color = Color.white;

                    if (UserData.m_MusicOn == 1)
                    {
                        m_BgMusic.clip = m_ExplodeMusic;
                        m_BgMusic.Play();
                    }
                }
                break;
            case 2: //核爆
                if (m_CurTime >= m_Interval)
                {
                    m_CurTime = 0;
                    m_State = 0;
                    m_Interval++;   //越來越久
                    m_Generataor.m_GenerateCountDown = m_NormalDropTime;
                    m_Generataor.m_FallingTime = m_NormalFallingTime;
                    //Debug.Log("Normal");
                    m_InfoBar.color = new Color(0.35f, 0.35f, 0.35f);
                    m_SkillBar.color = new Color(0.35f, 0.35f, 0.35f);
                    m_WarnText.color = new Color(0,0,0,0);
                    m_WarnIndex = (m_WarnIndex + 1) % m_WarnMsg.Length;

                    if (UserData.m_MusicOn == 1)
                    {
                        m_BgMusic.clip = m_NormalMusic;
                        m_BgMusic.Play();
                    }
                }
                break;
        }
    }
}
