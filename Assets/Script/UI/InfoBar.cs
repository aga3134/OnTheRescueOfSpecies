using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InfoBar : MonoBehaviour {
    public Text m_MaterialText, m_DNAText;

    [HideInInspector]
    public int m_MaterialNum;
    [HideInInspector]
    public int m_DNANum;
	// Use this for initialization
	void Start () {
        m_MaterialNum = 0;
        m_DNANum = 0;
        m_MaterialText.text = m_MaterialNum.ToString();
        m_DNAText.text = m_DNANum.ToString();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void AddMaterial(int num){
        m_MaterialNum += num * (UserData.m_CurLevel+1);
        m_MaterialText.text = m_MaterialNum.ToString();
    }

    public void AddDNANum(int num) {
        m_DNANum += num * (UserData.m_CurLevel+1);
        m_DNAText.text = m_DNANum.ToString();
    }

}
