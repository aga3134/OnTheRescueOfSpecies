using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using UnityEngine.Advertisements;

public class OutOfResource : MonoBehaviour {
    public float m_Speed = 5;
    public Vector2 m_OpenPos, m_ClosePos;
    RectTransform m_RectTransform;
    public Button[] m_LevelBt;
    public Text m_Countdown;
    public bool m_Pause;

	// Use this for initialization
	void Start () {
        m_Pause = false;
        m_RectTransform = GetComponent<RectTransform>();
        m_RectTransform.anchoredPosition = m_ClosePos;
	}
	
	// Update is called once per frame
	void Update () {
        if (m_Pause) return;
        DateTime d = UserData.m_TimeBase.AddSeconds(UserData.m_AddLifeTime);
        TimeSpan ts = d - DateTime.Now;
        m_Countdown.text = (ts.Minutes+1) + "萬年";

        if (UserData.m_Life == 0){
            for (int i = 0; i < m_LevelBt.Length; i++) {
                m_LevelBt[i].enabled = false;
            }
            m_RectTransform.anchoredPosition = Vector2.Lerp(m_RectTransform.anchoredPosition, m_OpenPos, Time.deltaTime * m_Speed);
        }
        else {
            for (int i = 0; i < m_LevelBt.Length; i++)
            {
                m_LevelBt[i].enabled = true;
            }
            m_RectTransform.anchoredPosition = Vector2.Lerp(m_RectTransform.anchoredPosition, m_ClosePos, Time.deltaTime * m_Speed);
        }
       
	}

    
    public void ShowRewardedAd(){
        if (Advertisement.IsReady("rewardedVideo")){
            var options = new ShowOptions { resultCallback = HandleShowResult };
            Advertisement.Show("rewardedVideo", options);
        }
    }

    private void HandleShowResult(ShowResult result){
        switch (result){
            case ShowResult.Finished:
                UserData.AddLife(3,false);
                break;
            case ShowResult.Skipped:
                break;
            case ShowResult.Failed:
                break;
        }
    }

}
