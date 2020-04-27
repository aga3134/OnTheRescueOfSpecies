using UnityEngine;
using System.Collections;

public class WoodSkill : MonoBehaviour {
    BoxGenerator m_BG;
    public int m_Count = 10;
	// Use this for initialization
	void Start () {
        GameObject gc = GameObject.FindGameObjectWithTag("GameController");
        m_BG = gc.GetComponent<BoxGenerator>();

        Coord2D coord = m_BG.PosToCoord(new Vector2(transform.localPosition.x, transform.localPosition.y));
        if (coord.x != -1 && coord.y != -1){
            GameObject element = m_BG.GetElementByCoord(coord);
            if (element != null) {
                var type = element.GetComponent<BoxElementControl>().m_Type;
                m_BG.AttachRandomSpecies(element);
                var count = 1;

                //尋找同類方塊，加上物種
                for (int y = 0; y < m_BG.m_BoxNumH; y++){
                    for (int x = 0; x < m_BG.m_BoxNumW; x++) {
                        Coord2D targetCoord = new Coord2D(x, y);
                        GameObject target = m_BG.GetElementByCoord(targetCoord);
                        if (target != null)
                        {
                            BoxElementControl bec = target.GetComponent<BoxElementControl>();
                            if (bec.m_Type == type && bec.GetSpecies() == null) {
                                m_BG.AttachRandomSpecies(target);
                                count++;
                                if (count >= m_Count) return;
                            }
                        }
                    }
                }
            }
            /*int halfSize = m_Size >> 1;
            for (int x = coord.x - halfSize; x < coord.x - halfSize + m_Size; x++){
                for (int y = coord.y - halfSize; y < coord.y - halfSize + m_Size; y++){
                    Coord2D targetCoord = new Coord2D(x, y);
                    GameObject element = m_BG.GetElementByCoord(targetCoord);
                    if (element != null) {
                        m_BG.AttachRandomSpecies(element);
                    }
                }
            }*/
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
