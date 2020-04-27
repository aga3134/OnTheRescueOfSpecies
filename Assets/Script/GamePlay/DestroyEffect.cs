using UnityEngine;
using System.Collections;

public class DestroyEffect : MonoBehaviour {
    public float m_LifeTime = 5;
    float m_CurTime = 0;
    public bool m_Pause = false;
    public bool m_Rescue = false;
    public AudioClip m_AudioDestroy;
    public AudioClip m_AudioRescue;
    AudioSource m_AudioSource;

	// Use this for initialization
	void Start () {
        m_AudioSource = GetComponent<AudioSource>();
        if (UserData.m_SoundOn == 1)
        {
            m_AudioSource.PlayOneShot(m_Rescue ? m_AudioRescue : m_AudioDestroy);
        }
	}

    public void SetRescue(bool isRescue) {
        m_Rescue = isRescue;
    }

	// Update is called once per frame
	void Update () {
        if (m_Pause) return;
        m_CurTime += Time.deltaTime;
        if (m_CurTime >= m_LifeTime) {
            DestroyObject(gameObject);
        }
	}
}
