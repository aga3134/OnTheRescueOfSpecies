using UnityEngine;
using System.Collections;

public enum BoxState { FALL, FIX, TOUCH, ELIMINATE };
public enum MoveDir { NONE, UP, RIGHT, DOWN, LEFT};

public class BoxElementControl : MonoBehaviour {
    public int m_Type;
    public float m_RemoveCountDown = 0.5f;
    public GameObject m_RemoveEffect;
    InfoBar m_InfoBar;
    float m_RemoveTime = 0;
    BoxGenerator m_BG;
    Vector2 m_CurPos, m_TargetPos;
    Coord2D m_CurCoord, m_TargetCoord;
    float m_CurTime = 0;
    BoxState m_State;
    MoveDir m_SwipeDir;
    GameObject m_Species;
    SkillButton m_SkillBt;
    public bool m_Pause = false;

	// Use this for initialization
	void Start () {
        m_State = BoxState.FALL;
        m_CurTime = 0;
        GameObject gc = GameObject.FindGameObjectWithTag("GameController");
        m_BG = gc.GetComponent<BoxGenerator>();

        m_SkillBt = GetSkillButton(m_Type);

        //init coordinate
        Vector2 pos = new Vector2(transform.localPosition.x, transform.localPosition.y);
        Coord2D coord = m_BG.PosToCoord(pos);
        SetCoord(coord);

        m_InfoBar = GameObject.FindGameObjectWithTag("InfoBar").GetComponent<InfoBar>();
	}

	// Update is called once per frame
    void Update(){
        if (m_Pause) return;

        UpdatePos();
        switch (m_State){
            case BoxState.FALL:
                m_CurTime += Time.deltaTime;
                if (m_CurTime >= m_BG.m_FallingTime)
                {
                    GoToNextCoord();
                }
                break;
            case BoxState.FIX:
                GoToNextCoord();
                break;
            case BoxState.TOUCH:
                m_CurTime += Time.deltaTime;
                if (m_CurTime >= m_BG.m_FallingTime)
                {
                    SetFixed();
                }
                break;
            case BoxState.ELIMINATE:
                m_RemoveTime += Time.deltaTime;
                if (m_RemoveTime >= m_RemoveCountDown){
                    if (m_BG.m_EarthSkill != null) {
                        m_BG.m_EarthSkill.EliminateRow(m_TargetCoord.y);
                    }
                    DestroyElement();
                }
                break;
        }
    }

    public static SkillButton GetSkillButton(int type) {
        GameObject obj = null;
        switch (type) { 
            case 0:
                obj = GameObject.FindGameObjectWithTag("MetalSkillBt");
                break;
            case 1:
                obj = GameObject.FindGameObjectWithTag("WoodSkillBt");
                break;
            case 2:
                obj = GameObject.FindGameObjectWithTag("WaterSkillBt");
                break;
            case 3:
                obj = GameObject.FindGameObjectWithTag("FireSkillBt");
                break;
            case 4:
                obj = GameObject.FindGameObjectWithTag("EarthSkillBt");
                break;
        }
        if(obj == null) return null;
        return obj.GetComponent<SkillButton>();
    }

    void UpdatePos() {
        Vector2 pos = Vector2.Lerp(m_CurPos, m_TargetPos, m_CurTime / m_BG.m_FallingTime);
        transform.localPosition = new Vector3(pos.x, pos.y, transform.localPosition.z);
    }

    void SetFixed() {
        m_CurCoord = m_TargetCoord;
        m_CurPos = m_TargetPos;
        m_State = BoxState.FIX;
        m_CurTime = m_BG.m_FallingTime;
        m_BG.SetElementByCoord(m_CurCoord, gameObject);
    }

    public void SetCoord(Coord2D coord){
        m_CurCoord = coord;
        m_TargetCoord = coord;
        m_CurTime = m_BG.m_FallingTime;
        m_State = BoxState.FALL;
        m_BG.SetElementByCoord(coord, gameObject);
        m_CurPos = m_BG.CoordToPos(m_CurCoord);
        m_TargetPos = m_CurPos;
    }

    public void GoToCoord(Coord2D coord){
        m_State = BoxState.FALL;
        m_CurPos = m_BG.CoordToPos(m_TargetCoord);
        m_TargetCoord = coord;
        m_CurTime = 0;
        m_BG.SetElementByCoord(m_CurCoord, null);
        m_BG.SetElementByCoord(coord, gameObject);
        m_TargetPos = m_BG.CoordToPos(m_TargetCoord);
    }

    public void GoToNextCoord(){
        m_CurCoord = m_TargetCoord;
        m_CurPos = m_BG.CoordToPos(m_TargetCoord);
        m_TargetCoord = m_BG.GetNextCoord(m_CurCoord);
        m_TargetPos = m_BG.CoordToPos(m_TargetCoord);
        m_CurTime = 0;
        if (m_CurCoord == m_TargetCoord){
            SetFixed();
        }
        else {
            m_State = BoxState.FALL;
            m_BG.SetElementByCoord(m_CurCoord, null);
            m_BG.SetElementByCoord(m_TargetCoord, gameObject);
        }
    }

    public void SwipeTo(MoveDir dir){
        if (m_Pause) return;
        if (m_State != BoxState.FIX) return;

        m_SwipeDir = dir;
        Coord2D coord = new Coord2D(m_CurCoord.x, m_CurCoord.y);
        switch (dir) { 
            case MoveDir.UP:
                coord.y++;
                break;
            case MoveDir.RIGHT:
                coord.x++;
                break;
            case MoveDir.DOWN:
                coord.y--;
                break;
            case MoveDir.LEFT:
                coord.x--;
                break;
        }
        m_State = BoxState.TOUCH;
        m_TargetCoord = coord;
        m_CurTime = 0;
        m_BG.SetElementByCoord(coord, gameObject);
        m_TargetPos = m_BG.CoordToPos(m_TargetCoord);
    }

    public bool ReadyToMoveBack() {
        return (m_State == BoxState.FIX) && (m_SwipeDir != MoveDir.NONE);
    }

    public void DoNotMoveBack() {
        m_SwipeDir = MoveDir.NONE;
    }

    public void MoveBack() {
        if (!ReadyToMoveBack()) return;

        Coord2D coord = m_TargetCoord;
        switch (m_SwipeDir){
            case MoveDir.UP:
                coord.y--;
                break;
            case MoveDir.RIGHT:
                coord.x--;
                break;
            case MoveDir.DOWN:
                coord.y++;
                break;
            case MoveDir.LEFT:
                coord.x++;
                break;
        }
        m_SwipeDir = MoveDir.NONE;
        m_State = BoxState.FALL;
        m_CurCoord = m_TargetCoord;
        m_CurPos = m_BG.CoordToPos(m_CurCoord);
        m_TargetCoord = coord;
        m_CurTime = 0;
        m_BG.SetElementByCoord(coord, gameObject);
        m_TargetPos = m_BG.CoordToPos(m_TargetCoord);
    }

    public bool CheckEliminate() {
        return CheckEliminate(MoveDir.UP) || CheckEliminate(MoveDir.RIGHT) || 
            CheckEliminate(MoveDir.DOWN) || CheckEliminate(MoveDir.LEFT);
    }

    public bool CheckEliminate(MoveDir dir){
        if (m_State != BoxState.FIX) return false;
        int w = m_BG.GetCoordW();
        int h = m_BG.GetCoordH();
        int matchNum = 1;
        switch (dir) { 
            case MoveDir.UP:
                for (int y = m_CurCoord.y + 1; y < h; y++){
                    if (IsMatch(new Coord2D(m_CurCoord.x, y))){
                        matchNum++;
                    }
                    else break;
                }
                if (matchNum >= 3)
                {
                    for (int y = 0; y < matchNum; y++)
                    {
                        MarkEliminate(new Coord2D(m_CurCoord.x, m_CurCoord.y + y));
                    }
                    return true;
                }
                else return false;
                //break;
            case MoveDir.RIGHT:
                for (int x = m_CurCoord.x + 1; x < w; x++){
                    if (IsMatch(new Coord2D(x, m_CurCoord.y))){
                        matchNum++;
                    }
                    else break;
                }
                if (matchNum >= 3)
                {
                    for (int x = 0; x < matchNum; x++)
                    {
                        MarkEliminate(new Coord2D(m_CurCoord.x + x, m_CurCoord.y));
                    }
                    return true;
                }
                else return false;
                //break;
            case MoveDir.DOWN:
                for (int y = m_CurCoord.y - 1; y >= 0; y--){
                    if (IsMatch(new Coord2D(m_CurCoord.x, y))){
                        matchNum++;
                    }
                    else break;
                }
                if (matchNum >= 3)
                {
                    for (int y = 0; y < matchNum; y++)
                    {
                        MarkEliminate(new Coord2D(m_CurCoord.x, m_CurCoord.y - y));
                    }
                    return true;
                }
                else return false;
                //break;
            case MoveDir.LEFT:
                for (int x = m_CurCoord.x - 1; x >= 0; x--){
                    if (IsMatch(new Coord2D(x, m_CurCoord.y))){
                        matchNum++;
                    }
                    else break;
                }
                if (matchNum >= 3)
                {
                    for (int x = 0; x < matchNum; x++)
                    {
                        MarkEliminate(new Coord2D(m_CurCoord.x - x, m_CurCoord.y));
                    }
                    return true;
                }
                else return false;
                //break;
            default:
                return false;
        }

    }

    bool IsMatch(Coord2D coord) {
        GameObject nb = m_BG.GetElementByCoord(coord);
        if (nb == null) return false;
        BoxElementControl nbControl = nb.GetComponent<BoxElementControl>();
        
        if (nbControl.m_Type == m_Type && 
            (nbControl.m_State == BoxState.FIX || nbControl.m_State == BoxState.ELIMINATE)) return true;
        else return false;
    }

    public void MarkEliminate() {
        if (m_State == BoxState.ELIMINATE) return;
        m_SwipeDir = MoveDir.NONE;
        m_State = BoxState.ELIMINATE;
        m_RemoveTime = 0;
        m_CurTime = m_BG.m_FallingTime;
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        sr.color = Color.Lerp(sr.color, Color.white, 0.5f);
    }

    public void MarkEliminate(Coord2D coord){
        GameObject obj = m_BG.GetElementByCoord(coord);
        if (obj == null) return;
        BoxElementControl objControl = obj.GetComponent<BoxElementControl>();
        objControl.MarkEliminate();
    }

    public void DestroyElement() {
        if (m_RemoveEffect != null){
            GameObject effect = (GameObject)Instantiate(m_RemoveEffect, transform.position, transform.rotation);
            DestroyEffect de = effect.GetComponent<DestroyEffect>();
            de.SetRescue(m_Species != null);
        }

        m_InfoBar.AddMaterial(1);
        DestroyObject(gameObject);
        m_BG.SetElementByCoord(m_TargetCoord, null);
        
        if (m_SkillBt != null) {
            m_SkillBt.AddAmount(0.2f);
        }
        if (m_Species != null){
            SpeciesInfo si = m_Species.GetComponent<SpeciesInfo>();
            si.SpeciesRescued();
            m_InfoBar.AddDNANum(1);
        }
        
    }

    public BoxState GetState() { return m_State; }
    public Coord2D GetTargetCoord() { return m_TargetCoord; }
    public MoveDir GetSwipeDir() { return m_SwipeDir; }
    public static MoveDir OppositeDir(MoveDir dir){
        switch(dir){
            case MoveDir.UP:
                return MoveDir.DOWN;
            case MoveDir.RIGHT:
                return MoveDir.LEFT;
            case MoveDir.DOWN:
                return MoveDir.UP;
            case MoveDir.LEFT:
                return MoveDir.RIGHT;
            default:
                return MoveDir.NONE;
        }
    }

    public GameObject GetNeighbor(MoveDir dir) {
        Coord2D coord = new Coord2D(m_TargetCoord.x, m_TargetCoord.y);
        switch (dir){
            case MoveDir.UP:
                coord.y++;
                break;
            case MoveDir.RIGHT:
                coord.x++;
                break;
            case MoveDir.DOWN:
                coord.y--;
                break;
            case MoveDir.LEFT:
                coord.x--;
                break;
        }
        return m_BG.GetElementByCoord(coord);
    }

    public void AttachSpecies(GameObject species) {
        if (m_Species != null) {
            DestroyObject(m_Species);
        }
        m_Species = (GameObject)Instantiate(species);
        m_Species.transform.parent = transform;
        m_Species.transform.localPosition = new Vector3(0,0,-1);
        m_Species.transform.localScale = Vector3.one;
    }

    public GameObject GetSpecies() { return m_Species; }
}
