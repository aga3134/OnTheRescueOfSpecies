using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameControl : MonoBehaviour {
    public float m_SwipeThresh = 0.5f;
    BoxGenerator m_BG;
    Vector3 m_PivotPos;
    GameObject m_TouchElement;
    public bool m_Pause = false;
    InfoBar m_InfoBar;
    ConfirmPanel m_ConfirmPanel;
    public Sprite[] m_LevelSprite;

	// Use this for initialization
	void Start () {
        //UserData.ResetUserData();
        m_BG = GetComponent<BoxGenerator>();
        m_InfoBar = GameObject.FindGameObjectWithTag("InfoBar").GetComponent<InfoBar>();
        m_ConfirmPanel = GameObject.Find("ConfirmPanel").GetComponentInChildren<ConfirmPanel>();
	}
	
	// Update is called once per frame
	void Update () {
        if (m_Pause) return;

        //check gameover
        if (IsReachTop()){
            Debug.Log("GameOver");
            GameOverPanel gop = GameObject.Find("GameOverPanel").GetComponentInChildren<GameOverPanel>();
            gop.OpenMenu();
            //SceneManager.LoadScene("MainMenu");
        }

        CheckEliminate();
        CheckMoveBack();

        if (Input.GetMouseButtonDown(0)) {
            m_PivotPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            m_PivotPos = transform.worldToLocalMatrix * m_PivotPos;
            TouchElement(m_PivotPos);
        }
        else if (Input.GetMouseButtonUp(0)) {
            m_TouchElement = null;
        }
        else if(Input.GetMouseButton(0) && m_TouchElement != null){
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pos = transform.worldToLocalMatrix * pos;

            float diffX = pos.x - m_PivotPos.x;
            float diffY = pos.y - m_PivotPos.y;
            if (Mathf.Abs(diffX) > Mathf.Abs(diffY)){
                if (diffX > m_SwipeThresh) SwipeElement(MoveDir.RIGHT);
                else if (diffX < -m_SwipeThresh) SwipeElement(MoveDir.LEFT);
            }
            else{
                if (diffY > m_SwipeThresh) SwipeElement(MoveDir.UP);
                else if (diffY < -m_SwipeThresh) SwipeElement(MoveDir.DOWN);
            }

        }

        CheckLevelUnlock();
	}

    public void CheckLevelUnlock() {
        string sceneName = SceneManager.GetActiveScene().name;
        switch(UserData.m_LevelUnlock){
            case 0: //農業時代
                if (sceneName == "AgeAgriculture")
                {
                    if (m_InfoBar.m_MaterialNum >= 300) {
                        UserData.m_LevelUnlock++;
                        UserData.SaveUserData();
                        m_ConfirmPanel.m_Title.text = "關卡解鎖";
                        m_ConfirmPanel.m_Image.sprite = m_LevelSprite[UserData.m_LevelUnlock];
                        m_ConfirmPanel.m_Info.text = "工業時代";
                        m_ConfirmPanel.OpenMenu();
                    }
                }
                break;
            case 1: //工業時代
                if (sceneName == "AgeFactory")
                {
                    if (m_InfoBar.m_DNANum >= 100){
                        UserData.m_LevelUnlock++;
                        UserData.SaveUserData();
                        m_ConfirmPanel.m_Title.text = "關卡解鎖";
                        m_ConfirmPanel.m_Image.sprite = m_LevelSprite[UserData.m_LevelUnlock];
                        m_ConfirmPanel.m_Info.text = "資訊時代";
                        m_ConfirmPanel.OpenMenu();
                    }
                }
                break;
            case 2: //資訊時代
                if (sceneName == "AgeNetwork")
                {
                    if (m_InfoBar.m_MaterialNum >= 2000 && m_InfoBar.m_DNANum >= 200)
                    {
                        UserData.m_LevelUnlock++;
                        UserData.SaveUserData();
                        m_ConfirmPanel.m_Title.text = "關卡解鎖";
                        m_ConfirmPanel.m_Image.sprite = m_LevelSprite[UserData.m_LevelUnlock];
                        m_ConfirmPanel.m_Info.text = "核戰時代";
                        m_ConfirmPanel.OpenMenu();
                    }
                }
                break;
            case 3: //核戰時代
                bool allRescue = true;
                for (int i = 0; i < 45; i++) {
                    if (UserData.m_EnabledSpecies[i] == 0) {
                        allRescue = false;
                        break;
                    }
                }
                if (allRescue) {
                    UserData.m_LevelUnlock++;
                    m_ConfirmPanel.m_Title.text = "關卡解鎖";
                    m_ConfirmPanel.m_Image.sprite = m_LevelSprite[UserData.m_LevelUnlock];
                    m_ConfirmPanel.m_Info.text = "寶島時代";
                    m_ConfirmPanel.OpenMenu();
                    UserData.SaveUserData();
                }
                break;
        }
    }

    public bool IsReachTop(){
        int w = m_BG.GetCoordW();
        int h = m_BG.GetCoordH();
        for (int x = 0; x < w; x++){
            GameObject obj = m_BG.GetElementByCoord(new Coord2D(x, h - 1));
            if (obj){
                BoxElementControl bec = obj.GetComponent<BoxElementControl>();
                if (bec.GetState() == BoxState.FIX) return true;   //element fixed at top
            }
        }
        return false;
    }

    public void TouchElement(Vector3 pos){
        if (m_Pause) return;
        Coord2D coord = m_BG.PosToCoord(new Vector2(pos.x, pos.y));
        m_TouchElement = m_BG.GetElementByCoord(coord);
    }

    public void SwipeElement(MoveDir dir){
        if (m_Pause) return;
        if (m_TouchElement == null) return;
        BoxElementControl curControl = m_TouchElement.GetComponent<BoxElementControl>();

        GameObject nbObj = curControl.GetNeighbor(dir);
        if (nbObj == null) return;

        BoxElementControl nbControl = nbObj.GetComponent<BoxElementControl>();
        MoveDir opDir = BoxElementControl.OppositeDir(dir);
        if (curControl.GetState() == BoxState.FIX && nbControl.GetState() == BoxState.FIX) {
            curControl.SwipeTo(dir);
            nbControl.SwipeTo(opDir);
        }
        m_TouchElement = null;
    }

    public void CheckEliminate(){
        int w = m_BG.GetCoordW();
        int h = m_BG.GetCoordH();
        for (int y = 0; y < h; y++){
            for (int x = 0; x < w; x++){
                GameObject obj = m_BG.GetElementByCoord(new Coord2D(x, y));
                if (obj == null) continue;  //no object
                BoxElementControl bec = obj.GetComponent<BoxElementControl>();
                bec.CheckEliminate();
            }
        }
    }

    public void CheckMoveBack() {
        int w = m_BG.GetCoordW();
        int h = m_BG.GetCoordH();
        for (int y = 0; y < h; y++) {
            for (int x = 0; x < w; x++) {
                GameObject obj = m_BG.GetElementByCoord(new Coord2D(x, y));
                if (obj == null) continue;  //no object
                BoxElementControl curControl = obj.GetComponent<BoxElementControl>();
                if (curControl.ReadyToMoveBack()) {
                    MoveDir swipeDir = curControl.GetSwipeDir();
                    MoveDir opDir = BoxElementControl.OppositeDir(swipeDir);
                    GameObject nb = curControl.GetNeighbor(opDir);
                    if (nb == null) continue;
                    BoxElementControl nbControl = nb.GetComponent<BoxElementControl>();
                    if (nbControl.ReadyToMoveBack()){   //no match
                        curControl.MoveBack();
                        nbControl.MoveBack();
                    }
                    else {  //nb match success
                        curControl.DoNotMoveBack();
                    }
                }
            }
        }
    }

    public void SetPause(bool bePause) {
        m_Pause = bePause;
        //nuclear countdown
        NuclearExplode ne = GetComponent<NuclearExplode>();
        if (ne) ne.m_Pause = bePause;

        //box
        BoxGenerator bg = GetComponent<BoxGenerator>();
        if (bg) bg.m_Pause = bePause;
        BoxElementControl[] becArray = GetComponentsInChildren<BoxElementControl>();
        for (int i = 0; i < becArray.Length; i++) {
            becArray[i].m_Pause = bePause;
        }

        //skill
        SkillButton[] sb = GameObject.Find("SkillBar").GetComponentsInChildren<SkillButton>();
        for (int i = 0; i < sb.Length; i++)
        {
            sb[i].m_Pause = bePause;
        }
        DestroyEffect[] de = GetComponentsInChildren<DestroyEffect>();
        for (int i = 0; i < de.Length; i++)
        {
            de[i].m_Pause = bePause;
        }
        MetalSkill[] ms = GetComponentsInChildren<MetalSkill>();
        for (int i = 0; i < ms.Length; i++)
        {
            ms[i].m_Pause = bePause;
        }
        WaterSkill[] ws = GetComponentsInChildren<WaterSkill>();
        for (int i = 0; i < ws.Length; i++)
        {
            ws[i].m_Pause = bePause;
        }
        EarthSkill[] es = GetComponentsInChildren<EarthSkill>();
        for (int i = 0; i < es.Length; i++)
        {
            es[i].m_Pause = bePause;
        }
    }
}
