using UnityEngine;
using System.Collections;

public class EarthSkill : MonoBehaviour {
    public float m_LifeTime = 5;
    float m_CurTime = 0;
    BoxGenerator m_BG;
    public bool m_Pause = false;
    Vector3 m_CamPos;

	// Use this for initialization
	void Start () {
        GameObject gc = GameObject.FindGameObjectWithTag("GameController");
        m_BG = gc.GetComponent<BoxGenerator>();
        m_BG.m_EarthSkill = this;
        m_CamPos = Camera.main.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        if (m_Pause) return;

        m_CurTime += Time.deltaTime;
        //shake camera
        float mag = 0.1f;
        float x = mag*(Random.value * 2.0f - 1.0f);
        float y = mag*(Random.value * 2.0f - 1.0f);
        Camera.main.transform.position = m_CamPos + new Vector3(x, y, m_CamPos.z);

        if (m_CurTime >= m_LifeTime) {
            Camera.main.transform.position = m_CamPos;
            m_BG.m_EarthSkill = null;
            DestroyObject(gameObject);
        }
	}

    public void EliminateRow(int row) {
        int w = m_BG.GetCoordW();
        for (int x = 0; x < w; x++) {
            GameObject obj = m_BG.GetElementByCoord(new Coord2D(x,row));
            if (!obj) continue;
            BoxElementControl control = obj.GetComponent<BoxElementControl>();
            control.DestroyElement();
        }
    }
}
