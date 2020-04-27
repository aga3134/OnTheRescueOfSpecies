using UnityEngine;
using System.Collections;
using System;

public class UserData : MonoBehaviour {
    public static int m_AddLifeTime = 600;    //second
    //basic
    public static int m_Life;
    public static DateTime m_TimeBase;
    public static int m_LevelUnlock;
    public static int m_CurLevel;
    public static int m_HighestScore;
    public static int m_MaterialRes;
    public static int m_DNARes;
    //skill
    public static int m_MetalSkillLevel;
    public static int m_WoodSkillLevel;
    public static int m_WaterSkillLevel;
    public static int m_FireSkillLevel;
    public static int m_EarthSkillLevel;
    public static int m_TimeAccLevel;
    public static int m_MetalSkillInit;
    public static int m_WoodSkillInit;
    public static int m_WaterSkillInit;
    public static int m_FireSkillInit;
    public static int m_EarthSkillInit;

    //species
    public static int[] m_EnabledSpecies = new int[60];
    //option
    public static int m_MusicOn = 1;
    public static int m_SoundOn = 1;
    //level
    public static int[] m_HighestMaterial = new int[5];
    public static int[] m_HighestDNA = new int[5];
    

    void Awake()
    {
        LoadUserData();
    }

    public static void AddLife(int life, bool resetTime) {
        if (m_Life == 5 || life <= 0) return;
        m_Life += life;
        //if(resetTime) m_TimeBase = DateTime.Now;
        if (resetTime) m_TimeBase = m_TimeBase.AddSeconds(m_AddLifeTime * life);
        if (m_Life > 5) m_Life = 5;
        SaveUserData();
    }

    public static void SubLife()
    {
        if (m_Life == 0) return;
        if (m_Life == 5) {
            m_TimeBase = DateTime.Now;
        }
        m_Life--;
        SaveUserData();
    }

    public static void RescueSpecies(int i){
        if (i < 0 || i >= m_EnabledSpecies.Length) return;
        m_EnabledSpecies[i]++;  //記錄共救援幾次
        SaveUserData();
    }

    public static void LoadUserData()
    {
        m_Life = PlayerPrefs.GetInt("Life", 5);
        m_CurLevel = PlayerPrefs.GetInt("CurLevel", 0);
        m_LevelUnlock = PlayerPrefs.GetInt("LevelUnlock", 0);
        m_HighestScore = PlayerPrefs.GetInt("HighestScore", 0);
        m_TimeBase = new DateTime(PlayerPrefs.GetInt("TimeBaseYear"), PlayerPrefs.GetInt("TimeBaseMonth"), PlayerPrefs.GetInt("TimeBaseDay"),
                                  PlayerPrefs.GetInt("TimeBaseHour"), PlayerPrefs.GetInt("TimeBaseMinute"), PlayerPrefs.GetInt("TimeBaseSecond"));

        m_MaterialRes = PlayerPrefs.GetInt("MaterialRes", 0);
        m_DNARes = PlayerPrefs.GetInt("DNARes", 0);

        m_MetalSkillLevel = PlayerPrefs.GetInt("MetalSkillLevel", 0);
        m_WoodSkillLevel = PlayerPrefs.GetInt("WoodSkillLevel", 0);
        m_WaterSkillLevel = PlayerPrefs.GetInt("WaterSkillLevel", 0);
        m_FireSkillLevel = PlayerPrefs.GetInt("FireSkillLevel", 0);
        m_EarthSkillLevel = PlayerPrefs.GetInt("EarthSkillLevel", 0);
        m_TimeAccLevel = PlayerPrefs.GetInt("TimeAccLevel", 0);
        m_MetalSkillInit = PlayerPrefs.GetInt("MetalSkillInit", 0);
        m_WoodSkillInit = PlayerPrefs.GetInt("WoodSkillInit", 0);
        m_WaterSkillInit = PlayerPrefs.GetInt("WaterSkillInit", 0);
        m_FireSkillInit = PlayerPrefs.GetInt("FireSkillInit", 0);
        m_EarthSkillInit = PlayerPrefs.GetInt("EarthSkillInit", 0);

        for (int i = 0; i < m_EnabledSpecies.Length; i++) {
            m_EnabledSpecies[i] = PlayerPrefs.GetInt("EnabledSpecies"+i, 0);
        }

        for (int i = 0; i < m_HighestMaterial.Length; i++) {
            m_HighestMaterial[i] = PlayerPrefs.GetInt("HighestMaterial" + i, 0);
            m_HighestDNA[i] = PlayerPrefs.GetInt("HighestDNA" + i, 0);
        }

        m_MusicOn = PlayerPrefs.GetInt("MusicOn", 1);
        m_SoundOn = PlayerPrefs.GetInt("SoundOn", 1);

        TimeSpan diff = DateTime.Now - m_TimeBase;
        int addLife = (int)Math.Floor(diff.TotalSeconds / m_AddLifeTime);
        AddLife(addLife, true);
    }

    public static void SaveUserData()
    {
        PlayerPrefs.SetInt("Life", m_Life);
        PlayerPrefs.SetInt("LevelUnlock", m_LevelUnlock);
        PlayerPrefs.SetInt("HighestScore", m_HighestScore);
        PlayerPrefs.SetInt("CurLevel", m_CurLevel);

        PlayerPrefs.SetInt("TimeBaseYear", m_TimeBase.Year);
        PlayerPrefs.SetInt("TimeBaseMonth", m_TimeBase.Month);
        PlayerPrefs.SetInt("TimeBaseDay", m_TimeBase.Day);
        PlayerPrefs.SetInt("TimeBaseHour", m_TimeBase.Hour);
        PlayerPrefs.SetInt("TimeBaseMinute", m_TimeBase.Minute);
        PlayerPrefs.SetInt("TimeBaseSecond", m_TimeBase.Second);

        PlayerPrefs.SetInt("MaterialRes", m_MaterialRes);
        PlayerPrefs.SetInt("DNARes", m_DNARes);

        PlayerPrefs.SetInt("MetalSkillLevel", m_MetalSkillLevel);
        PlayerPrefs.SetInt("WoodSkillLevel", m_WoodSkillLevel);
        PlayerPrefs.SetInt("WaterSkillLevel", m_WaterSkillLevel);
        PlayerPrefs.SetInt("FireSkillLevel", m_FireSkillLevel);
        PlayerPrefs.SetInt("EarthSkillLevel", m_EarthSkillLevel);
        PlayerPrefs.SetInt("TimeAccLevel", m_TimeAccLevel);
        PlayerPrefs.SetInt("MetalSkillInit", m_MetalSkillInit);
        PlayerPrefs.SetInt("WoodSkillInit", m_WoodSkillInit);
        PlayerPrefs.SetInt("WaterSkillInit", m_WaterSkillInit);
        PlayerPrefs.SetInt("FireSkillInit", m_FireSkillInit);
        PlayerPrefs.SetInt("EarthSkillInit", m_EarthSkillInit);

        for (int i = 0; i < m_EnabledSpecies.Length; i++){
            PlayerPrefs.SetInt("EnabledSpecies" + i, m_EnabledSpecies[i]);
        }

        for (int i = 0; i < m_HighestMaterial.Length; i++){
            PlayerPrefs.SetInt("HighestMaterial" + i, m_HighestMaterial[i]);
            PlayerPrefs.SetInt("HighestDNA" + i, m_HighestDNA[i]);
        }

        PlayerPrefs.SetInt("MusicOn", m_MusicOn);
        PlayerPrefs.SetInt("SoundOn", m_SoundOn);
    }

    public static void ResetUserData()
    {
        m_Life = 5;
        m_LevelUnlock = 0;
        m_HighestScore = 0;
        m_TimeBase = new DateTime();
        m_MaterialRes = 0;
        m_DNARes = 0;
        m_MetalSkillLevel = 0;
        m_WoodSkillLevel = 0;
        m_WaterSkillLevel = 0;
        m_FireSkillLevel = 0;
        m_EarthSkillLevel = 0;
        m_TimeAccLevel = 0;
        m_MetalSkillInit = 0;
        m_WoodSkillInit = 0;
        m_WaterSkillInit = 0;
        m_FireSkillInit = 0;
        m_EarthSkillInit = 0;
        m_MusicOn = 1;
        m_SoundOn = 1;

        for (int i = 0; i < m_EnabledSpecies.Length; i++) {
            m_EnabledSpecies[i] = 0;
        }

        SaveUserData();
    }
}
