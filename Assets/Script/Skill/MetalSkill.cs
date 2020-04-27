using UnityEngine;
using System.Collections;

public class MetalSkill : MonoBehaviour {
    public GameObject m_Particle;
    public int m_ParticleNum = 5;
    GameObject[] m_ParticleArray;
    BoxGenerator m_BG;
    public int m_BounceNum = 5;
    public float m_Speed = 10.0f;
    int m_CurBounce;
    GameObject m_Target;
    Vector3 m_Offset = new Vector3(0,0,-1);
    int m_TargetType = -1;
    Coord2D m_TargetCoord;
    public bool m_Pause = false;

	// Use this for initialization
	void Start () {
        m_ParticleArray = new GameObject[m_ParticleNum];
        for (int i = 0; i < m_ParticleNum; i++) {
            m_ParticleArray[i] = (GameObject)Instantiate(m_Particle);
            m_ParticleArray[i].transform.parent = transform;
            m_ParticleArray[i].transform.localPosition = Vector3.zero;
        }

        GameObject gc = GameObject.FindGameObjectWithTag("GameController");
        m_BG = gc.GetComponent<BoxGenerator>();
        m_CurBounce = 0;

        Vector2 pos = new Vector2(transform.localPosition.x, transform.localPosition.y);
        m_TargetCoord = m_BG.PosToCoord(pos);
        if (m_TargetCoord.x != -1 && m_TargetCoord.y != -1) {
            m_Target = m_BG.GetElementByCoord(m_TargetCoord);

            if (m_Target != null) {
                BoxElementControl targetControl = m_Target.GetComponent<BoxElementControl>();
                m_TargetType = targetControl.m_Type;

                for (int i = 0; i < m_ParticleNum; i++){
                    m_ParticleArray[i].transform.position = m_Target.transform.position + m_Offset;
                }
            }
            else {
                DestroyObject(gameObject);
            }
        }
	}

    bool FindNextTarget() {
        m_CurBounce++;
        if (m_CurBounce >= m_BounceNum) return false;

        int w = m_BG.GetCoordW();
        int h = m_BG.GetCoordH();
        for (int x = 0; x < w; x++){
            for (int y = 0; y < h; y++) {         
                Coord2D coord = new Coord2D((x+m_TargetCoord.x)%w, y);
                GameObject obj = m_BG.GetElementByCoord(coord);
                if (obj == null) continue;
                BoxElementControl objControl = obj.GetComponent<BoxElementControl>();
                if (objControl.m_Type == m_TargetType && objControl.GetState() != BoxState.ELIMINATE) {
                    m_Target = obj;
                    m_TargetCoord = coord;
                    return true;
                }
            }
        }
        return false;
    }

    void TraceTarget() {
        for (int i = m_ParticleNum-1; i > 0; i--) {
            m_ParticleArray[i].transform.position = m_ParticleArray[i-1].transform.position;
        }
        Vector3 diff = m_Target.transform.position + m_Offset - m_ParticleArray[0].transform.position;
        m_ParticleArray[0].transform.position += Time.deltaTime * diff.normalized * m_Speed;
    }
	
	// Update is called once per frame
	void Update () {
        if (m_Pause) return;

        if (m_Target == null) {
            if (!FindNextTarget()) DestroyObject(gameObject);
        }
        else if (Vector3.Distance(m_ParticleArray[0].transform.position, m_Target.transform.position+m_Offset) < 0.5f){
            BoxElementControl control = m_Target.GetComponent<BoxElementControl>();
            control.DestroyElement();
            if (!FindNextTarget()) DestroyObject(gameObject);
        }
        else{
            TraceTarget();
        }
	}
}
