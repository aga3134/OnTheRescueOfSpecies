using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class LifeUpdate : MonoBehaviour {
    public Text m_LifeText;
    public Text m_TimeText;
    public Image m_TimeIcon;

	// Use this for initialization
	void Start () {
        //UserData.SubLife();
	}
	
	// Update is called once per frame
	void Update () {
        int life = UserData.m_Life;
        m_LifeText.text = "X " + life;

        if (life < 5)
        {
            //m_TimeIcon.enabled = true;
            DateTime d = UserData.m_TimeBase.AddSeconds(UserData.m_AddLifeTime);
            TimeSpan ts = d - DateTime.Now;
            m_TimeText.text = ts.Minutes.ToString("D2") + ":" + ts.Seconds.ToString("D2");
            if (ts.TotalSeconds <= 0)
            {
                UserData.AddLife(1, true);
            }
        }
        else {
            //m_TimeIcon.enabled = false;
            m_TimeText.text = "00:00";
        }
	}
}
