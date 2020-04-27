using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public struct Coord2D
{
    public Coord2D(int xv, int yv) { x = xv; y = yv; }
    public static bool operator ==(Coord2D c1, Coord2D c2){
        return (c1.x == c2.x) && (c1.y == c2.y);
    }
    public static bool operator !=(Coord2D c1, Coord2D c2){
        return (c1.x != c2.x) || (c1.y != c2.y);
    }
    public bool InRange(int minX, int maxX, int minY, int maxY) {
        if (x >= minX && x < maxX && y >= minY && y < maxY) return true;
        else return false;
    }
    public int x, y;
}

public class BoxGenerator : MonoBehaviour {
    public GameObject[] m_BoxElement;
    public GameObject[] m_Species;
    float m_ChanceSum;
    public Image m_PlayArea;
    public int m_BoxNumH = 16, m_BoxNumW = 10;
    public float m_GenerateCountDown = 1;
    public float m_FallingTime = 1;
    public bool m_Halt = false;     //for nuclear war warning
    public bool m_Pause = false;    //for menu & other game state
    float m_CountDown;
    Vector2 m_Offset;
    GameObject[] m_ElementArray;
    Coord2D m_TouchCoord;
    int m_LastX;
    [HideInInspector]
    public EarthSkill m_EarthSkill;

	// Use this for initialization
	void Start () {
        float h = 2f * Camera.main.orthographicSize;
        float w = h * Camera.main.aspect;
        //Debug.Log(w+","+h);
        float scaleX = w / m_BoxNumW;
        float scaleY = (h-4) / m_BoxNumH;
        transform.localScale = new Vector3(scaleX, scaleY, 1);

        m_Offset = new Vector2(-(m_BoxNumW-1) * 0.5f, -(m_BoxNumH-1)*0.5f);
        m_CountDown = m_GenerateCountDown;
        m_ElementArray = new GameObject[m_BoxNumW*m_BoxNumH];
        m_LastX = -1;

        m_ChanceSum = 0;
        for (int i = 0; i < m_Species.Length; i++) {
            m_ChanceSum += m_Species[i].GetComponent<SpeciesInfo>().m_ShowChance;
        }

        /*for (int x = 0; x < m_BoxNumW; x++)
        {
            for (int y = 0; y < m_BoxNumH; y++) {
                Vector3 pos = new Vector3(x+m_Offset.x, y+m_Offset.y, 0);
                GameObject obj = (GameObject)Instantiate(m_BoxElement);
                obj.transform.parent = this.transform;
                obj.transform.localPosition = pos;
                obj.transform.localScale = Vector3.one;
            }
        }*/
    }
	
	// Update is called once per frame
	void Update () {
        if (m_Halt || m_Pause) return;
        m_CountDown -= Time.deltaTime;
        if (m_CountDown <= 0){
            m_CountDown = m_GenerateCountDown;

            int x = GetRandomPosX();//Random.Range(0, m_BoxNumW);
            if (x == m_LastX) { //reduce burst death
                x = (x + 1) % m_BoxNumW;
            }
            m_LastX = x;

            int index = Random.Range(0, m_BoxElement.Length);
            Coord2D coord = new Coord2D(x, m_BoxNumH-1);
            Vector2 pos = CoordToPos(coord);

            GameObject obj = (GameObject)Instantiate(m_BoxElement[index]);
            obj.transform.parent = this.transform;
            obj.transform.localPosition = new Vector3(pos.x, pos.y, transform.localPosition.z); ;
            obj.transform.localScale = Vector3.one;

            //random generate species by probabilities
            float rn = Random.Range(0.0f, 1.0f);
            if (rn > 0.95f) {
                AttachRandomSpecies(obj);
            }
        }
	}

    int GetRandomPosX() {
        //同一行空格越多越有機會中
        int[] emptyY = new int[m_BoxNumW];
        for (int x = 0; x < m_BoxNumW; x++) {
            emptyY[x] = 10; //初始機會，設越大越不容易受空格數影響
            for (int y = m_BoxNumH-1; y >= 0; y--) {
                GameObject obj = GetElementByCoord(new Coord2D(x, y));
                if (obj == null)
                {
                    emptyY[x]++;
                }
                else continue;
            }
        }

        float emptySum = 0;
        for (int x = 0; x < m_BoxNumW; x++) {
            emptySum += emptyY[x];
        }
        float rn = Random.Range(0, emptySum);
        int acc = 0;
        for (int x = 0; x < m_BoxNumW; x++) {
            acc += emptyY[x];
            if (acc > rn) {
                return x;
            }
        }
        return 0;
    }

    public void AttachRandomSpecies(GameObject obj) {
        float value = Random.Range(0, m_ChanceSum);
        float acc = 0;
        for (int i = 0; i < m_Species.Length; i++)
        {
            acc += m_Species[i].GetComponent<SpeciesInfo>().m_ShowChance;
            if (acc >= value)
            {
                obj.GetComponent<BoxElementControl>().AttachSpecies(m_Species[i]);
                break;
            }
        }
    }

    public int GetCoordW() { return m_BoxNumW; }
    public int GetCoordH() { return m_BoxNumH; }

    public Coord2D PosToCoord(Vector2 pos){
        int x = Mathf.RoundToInt(pos.x - m_Offset.x);
        int y = Mathf.RoundToInt(pos.y - m_Offset.y);
        if (x >= 0 && x < m_BoxNumW && y >= 0 && y < m_BoxNumH) {
            return new Coord2D(x, y);
        }
        else return new Coord2D(-1, -1); ;
    }

    public Vector2 CoordToPos(Coord2D coord){
        if (coord.InRange(0, m_BoxNumW, 0, m_BoxNumH)){
            return new Vector2(coord.x + m_Offset.x, coord.y + m_Offset.y);
        }
        else return new Vector2(float.NaN, float.NaN);
    }

    public GameObject GetElementByCoord(Coord2D coord){
        if (coord.InRange(0, m_BoxNumW, 0, m_BoxNumH)) return m_ElementArray[coord.y * m_BoxNumW + coord.x];
        else return null;
    }
    public void SetElementByCoord(Coord2D coord, GameObject obj){
        if (coord.InRange(0, m_BoxNumW, 0, m_BoxNumH)){
            m_ElementArray[coord.y * m_BoxNumW + coord.x] = obj;
        }
    }

    public Coord2D GetNextCoord(Coord2D curCoord){
        Coord2D nextCoord = new Coord2D(curCoord.x, curCoord.y - 1);
        if (!nextCoord.InRange(0, m_BoxNumW, 0, m_BoxNumH)){
            return curCoord;
        }
        else if (GetElementByCoord(nextCoord) != null){
            return curCoord;
        }
        else{
            return nextCoord;
        }
    }

    public void EliminateBoxWithType(int type) {
        for (int i = 0; i < m_ElementArray.Length; i++) {
            if (m_ElementArray[i] == null) continue;
            BoxElementControl bec = m_ElementArray[i].GetComponent<BoxElementControl>();
            if (bec.m_Type == type) {
                bec.MarkEliminate();
            }
        }
    }
}
