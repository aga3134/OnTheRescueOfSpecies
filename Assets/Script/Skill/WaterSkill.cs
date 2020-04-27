using UnityEngine;
using System.Collections;

public class WaterSkill : MonoBehaviour {
    BoxGenerator m_BG;
    public float m_LifeTime = 5;
    public float m_DestroyTime = 0.5f;
    float m_DestroyCountDown = 0;
    public int m_Size = 3;
    float m_CurTime = 0;
    Coord2D m_Coord;
    public bool m_Pause = false;
	// Use this for initialization
	void Start () {
        GameObject gc = GameObject.FindGameObjectWithTag("GameController");
        m_BG = gc.GetComponent<BoxGenerator>();
        //寬度是偶數行時需shift，特效跟消融方塊的區域重疊位置才會一樣
        float offsetX = 0;
        if (m_Size % 2 == 0) {
            offsetX = 0.5f;
        }
        m_Coord = m_BG.PosToCoord(new Vector2(transform.localPosition.x+offsetX, transform.localPosition.y));
        m_DestroyCountDown = m_DestroyTime;
	}
	
	// Update is called once per frame
	void Update () {
        if (m_Pause) return;
        m_CurTime += Time.deltaTime;
        if (m_CurTime >= m_LifeTime){
            DestroyObject(gameObject);
        }
        else {
            m_DestroyCountDown -= Time.deltaTime;
            if (m_DestroyCountDown <= 0)
            {
                int halfSize = m_Size >> 1;
                for (int x = m_Coord.x - halfSize; x < m_Coord.x - halfSize + m_Size; x++)
                {
                    GameObject obj = m_BG.GetElementByCoord(new Coord2D(x, m_Coord.y));
                    if (obj != null)
                    {
                        BoxElementControl control = obj.GetComponent<BoxElementControl>();
                        control.DestroyElement();
                    }
                }
                m_DestroyCountDown = m_DestroyTime;
            }
        }
	}
}
