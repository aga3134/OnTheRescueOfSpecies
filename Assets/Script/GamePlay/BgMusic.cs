using UnityEngine;
using System.Collections;

public class BgMusic : MonoBehaviour {
    AudioSource m_BgMusic;

	// Use this for initialization
	void Start () {
        m_BgMusic = GetComponent<AudioSource>();
        if (UserData.m_MusicOn == 1)
        {
            m_BgMusic.Play();
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
