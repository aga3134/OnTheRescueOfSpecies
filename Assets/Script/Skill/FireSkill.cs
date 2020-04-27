using UnityEngine;
using System.Collections;

public class FireSkill : MonoBehaviour {
    BoxGenerator m_BG;
    public int m_Size = 3;
	// Use this for initialization
	void Start () {
        GameObject gc = GameObject.FindGameObjectWithTag("GameController");
        m_BG = gc.GetComponent<BoxGenerator>();

        //寬度是偶數行時需shift，特效跟消融方塊的區域重疊位置才會一樣
        float offsetX = 0, offsetY = 0;
        if (m_Size % 2 == 0)
        {
            offsetX = 0.5f;
            offsetY = 0.5f;
        }
        Coord2D coord = m_BG.PosToCoord(new Vector2(transform.localPosition.x+offsetX, transform.localPosition.y+offsetY));
        if (coord.x != -1 && coord.y != -1) { 
            int halfSize = m_Size>>1;
            for (int x = coord.x - halfSize; x < coord.x - halfSize + m_Size; x++) {
                for (int y = coord.y - halfSize; y < coord.y - halfSize + m_Size; y++) {
                    Coord2D targetCoord = new Coord2D(x,y);
                    GameObject element = m_BG.GetElementByCoord(targetCoord);
                    if (element != null){
                        BoxElementControl control = element.GetComponent<BoxElementControl>();
                        control.DestroyElement();
                    }
                }
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
