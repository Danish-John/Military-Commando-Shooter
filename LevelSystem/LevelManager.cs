using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    [Range(1f,20f)]
    public int NumberOfBullets;

    private float StartMissionDelay;
    public bool isNight;
    public Level [] Levels;

    private int levelIndex;


    /// <summary>
    /// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// </summary>
    /// 

    public static LevelManager instance;

    LevelData Data;
    int gunindex;

    LoadLevelData LevelGunData;
    public Image Scope;

    /// <summary>
    /// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// </summary>






    public int LevelIndex { 
        get 
        { 
            if(levelIndex< Levels.Length)
            {
                return levelIndex;
            }
            else
            {
                return levelIndex = Levels.Length - 1;
            }
           
        } }


    [HideInInspector] public Move Player;

    [HideInInspector] public bool isLastLevel;

    private Level CurrentPart;
    public Level currentPart { get { return CurrentPart; } }

    public Transform SpawnPoint;

    public int LevelZoomAMount=20;

    public GameObject Cutscene;

    [TextArea]
    public string ObjectiveText;


    private int TotalEnemies;

    public int _TotalEnemies { get { return TotalEnemies; } }

    private int Totallevels;
    
    
    private void Start()
    {
        PlayerPrefs.SetInt("GunInSniperScene", 0);
        instance = this;


        Totallevels = Levels.Length;

        for (int i = 0; i < Totallevels; i++)
        {


            TotalEnemies += Levels[i].Enemies.Length;

            TotalEnemies += Levels[i].Objectives.Length;
            

        }

      
        Cutscene.SetActive(true);

        if(InGameUi.instance!=null)
        {
            InGameUi.instance.SetCutsceneText(ObjectiveText);

            InGameUi.instance.StartGameBtn.onClick.AddListener(StartMission);

            InGameUi.instance.SKipCutsceneBtn.onClick.AddListener(StartMission);
        }

        BulletAssignment();


        if(GameManager.instance.Data.CurrentLevel ==1 || GameManager.instance.Data.CurrentLevel == 3 || GameManager.instance.Data.CurrentLevel == 5 || GameManager.instance.Data.CurrentLevel == 7)
        {
            NetCheckinLevels();
            
        }


    }


    void NetCheckinLevels()
    {
        if (AdsManger_New.Instance.IsInternetConnection() == true)
        {
#if !UNITY_EDITOR
            FirebaseManager.Instance.LogEvent("NetOn_Sniper_" + GameManager.instance.Data.CurrentLevel);
#endif

        }
        else if (AdsManger_New.Instance.IsInternetConnection() == false)
        {
#if !UNITY_EDITOR
            FirebaseManager.Instance.LogEvent("NetOff_Sniper_" + GameManager.instance.Data.CurrentLevel);
#endif

        }
    }




    private void Update()
    {
        //if (PlayerPrefs.GetInt("GunInSniperScene") == 0)
        //{
        //    //InGameUi.instance.FreeGunBtn.gameObject.SetActive(true);
        //    //RewardedGunShowUI();
        //}



    }



    void DisableLevels()
    {
        for (int i = 0; i < Levels.Length; i++)
        {
            Levels[i].lvl_manager = this;

            if (i > 0)
            {
                Levels[i].gameObject.SetActive(false);
            }
        }

    }
    IEnumerator  NextObjective()
    {
        yield return new WaitForSeconds(StartMissionDelay);

        if (levelIndex > 0)
        {
            Levels[levelIndex - 1].gameObject.SetActive(false);      
        }

        Levels[levelIndex].lvl_manager = this;
        Levels[levelIndex].Player = Player;
        Levels[levelIndex].gameObject.SetActive(true);
        Levels[levelIndex].StartMission();
      
        CurrentPart = Levels[levelIndex];

        if(CurrentPart.DisableEnemies && GameManager.instance!=null)
        {

            CurrentPart.Invoke("DisableEnemyAggro", 0.5f);
        }
        

     }



    public void StartMission()
    {
       GameManager.instance.StartLog();

       Cutscene.SetActive(false);

       InGameUi.instance.EndCutscene();

       StartMissionDelay = 0f;

        //  GameManager.instance.SetWeaponData(NumberOfBullets);

       
       Player.transform.position = SpawnPoint.position;

       Invoke("DisableLevels", 2f);
       
       StartCoroutine(NextObjective());

    }
    public void EndMission()  
    {
       
        levelIndex++;

        
        if (levelIndex>=Levels.Length)
        {
            //Invoke("MissionSucess", 1f);
            MissionSucess();
        }
        else
        {
            StartMissionDelay = 2f;

            StartCoroutine(NextObjective());
        }

        if(levelIndex==Levels.Length-1)
        {
            isLastLevel = true;
        }
       
        

    }
    
    public void MissionSucess()
    {
        if(GameManager.instance!=null)
        {
            GameManager.instance.hasFailed = false;
        }
     
        
    }




    void BulletAssignment()
    {
        //GameManager.instance.WeaponInfo.Bullet = GameManager.instance.Data.gameData.EquippedGunName;
        string Gunname = GameManager.instance.Data.gameData.EquippedGunName;

        switch (Gunname)
        {
            case "CheyTacM200_2":
                GameManager.instance.WeaponInfo.Bullet = GameManager.instance.BulletPrefabs[0];
                break;

            case "Kar-5":
                GameManager.instance.WeaponInfo.Bullet = GameManager.instance.BulletPrefabs[1];
                break;

            case "M1-10-4":
                GameManager.instance.WeaponInfo.Bullet = GameManager.instance.BulletPrefabs[2];
                break;

            case "M24_world-3":
                GameManager.instance.WeaponInfo.Bullet = GameManager.instance.BulletPrefabs[3];
                break;

            case "Sniper-4":
                GameManager.instance.WeaponInfo.Bullet = GameManager.instance.BulletPrefabs[4];
                break;

        }
    }





    //private void RewardedGunShowUI()
    //{
    //    bool DisplayGunBtn = false;

    //    int i;
    //    for (i = 0; i < GameManager.instance.Guns.Length; i++)
    //    {

    //        Debug.Log("Guns Length = " + GameManager.instance.Guns.Length);
    //        Debug.Log("Guns Name = " + GameManager.instance.Guns[i].name);
    //        Debug.Log("Data equippedgun = " + GameManager.instance.Data.gameData.EquippedGunName);   //  LevelGunData.WpnDictionary[Data.gameData.EquippedGunName].name.ToString()

    //        if (GameManager.instance.Guns[i].name.ToString() != GameManager.instance.Data.gameData.EquippedGunName.ToString())
    //        {
    //            InGameUi.instance.FreeGunBtn.gameObject.SetActive(true);
    //            //InGameUi.instance.FreeGunBtn.GetComponent<Image>().sprite = GunSprites[i];
    //            InGameUi.instance.FreeGunBtn.transform.GetChild(5).GetComponent<Text>().text = GameManager.instance.Guns[i].name.ToString();
    //            InGameUi.instance.FreeGunBtn.transform.GetChild(i).gameObject.SetActive(true);
    //            if (i > 0)
    //            {
    //                InGameUi.instance.FreeGunBtn.transform.GetChild(i - 1).gameObject.SetActive(false);

    //            }

    //            gunindex = i;
    //            DisplayGunBtn = true;
    //        }
    //   }




    //    if (DisplayGunBtn == false)
    //    {
    //        InGameUi.instance.FreeGunBtn.gameObject.SetActive(false);
    //    }

    //}


    public void GiveRewardedGunKar()
    {
        PlayerPrefs.SetInt("GunInSniperScene", gunindex);
        
        
        int j;
        for (j = 0; j < GameManager.instance.Guns.Length; j++)
        {
            
            GameManager.instance.Guns[j].SetActive(false);
            GameManager.instance.HolsterGuns[j].SetActive(false);  
        }

        GameManager.instance.Guns[1].SetActive(true);
        GameManager.instance.HolsterGuns[1].SetActive(true);
        GameManager.instance.WeaponInfo.Scope = GameManager.instance.Data.gameData.Guns[1].ScopeImage;
        Scope.GetComponent<Image>().sprite = GameManager.instance.Data.gameData.Guns[1].ScopeImage;
        GameManager.instance.WeaponInfo.Bullet = GameManager.instance.BulletPrefabs[1];
        //CameraMove.instance.BulletAssign();


#if !UNITY_EDITOR
            FirebaseManager.Instance.LogEvent("Rew_KAR");
#endif

        Time.timeScale = 1;




        ButtonSwappingRewardedGuns.instance.selectedsnipergun = 0;
        ButtonSwappingRewardedGuns.instance.StopAllCoroutines();
        ButtonSwappingRewardedGuns.instance.Initialization();
        //ButtonSwappingRewardedGuns.instance.HideGunBtns();



    }


    public void GiveRewardedGunM10()
    {
        PlayerPrefs.SetInt("GunInSniperScene", gunindex);


        int j;
        for (j = 0; j < GameManager.instance.Guns.Length; j++)
        {
            GameManager.instance.Guns[j].SetActive(false);
            GameManager.instance.HolsterGuns[j].SetActive(false);
        }

        GameManager.instance.Guns[2].SetActive(true);
        GameManager.instance.HolsterGuns[2].SetActive(true);
        GameManager.instance.WeaponInfo.Scope = GameManager.instance.Data.gameData.Guns[2].ScopeImage;
        Scope.GetComponent<Image>().sprite = GameManager.instance.Data.gameData.Guns[2].ScopeImage;
        GameManager.instance.WeaponInfo.Bullet = GameManager.instance.BulletPrefabs[2];
        //CameraMove.instance.BulletAssign();

#if !UNITY_EDITOR
            FirebaseManager.Instance.LogEvent("Rew_M1_10");
#endif

        Time.timeScale = 1;




        ButtonSwappingRewardedGuns.instance.selectedsnipergun = 1;
        ButtonSwappingRewardedGuns.instance.StopAllCoroutines();
        ButtonSwappingRewardedGuns.instance.Initialization();
        //ButtonSwappingRewardedGuns.instance.HideGunBtns();



    }



    public void GiveRewardedGunM24()
    {
        PlayerPrefs.SetInt("GunInSniperScene", gunindex);


        int j;
        for (j = 0; j < GameManager.instance.Guns.Length; j++)
        {
            GameManager.instance.Guns[j].SetActive(false);
            GameManager.instance.HolsterGuns[j].SetActive(false);
        }

        GameManager.instance.Guns[3].SetActive(true);
        GameManager.instance.HolsterGuns[3].SetActive(true);
        GameManager.instance.WeaponInfo.Scope = GameManager.instance.Data.gameData.Guns[3].ScopeImage;
        Scope.GetComponent<Image>().sprite = GameManager.instance.Data.gameData.Guns[3].ScopeImage;
        GameManager.instance.WeaponInfo.Bullet = GameManager.instance.BulletPrefabs[3];
        //CameraMove.instance.BulletAssign();

#if !UNITY_EDITOR
            FirebaseManager.Instance.LogEvent("Rew_M24");
#endif

        Time.timeScale = 1;


        ButtonSwappingRewardedGuns.instance.selectedsnipergun = 2;
        ButtonSwappingRewardedGuns.instance.StopAllCoroutines();
        ButtonSwappingRewardedGuns.instance.Initialization();
        //ButtonSwappingRewardedGuns.instance.HideGunBtns();


    }






    public void GiveRewardedGunSniper4()
    {
        PlayerPrefs.SetInt("GunInSniperScene", gunindex);


        int j;
        for (j = 0; j < GameManager.instance.Guns.Length; j++)
        {
            GameManager.instance.Guns[j].SetActive(false);
            GameManager.instance.HolsterGuns[j].SetActive(false);
        }

        GameManager.instance.Guns[4].SetActive(true);
        GameManager.instance.HolsterGuns[4].SetActive(true);
        GameManager.instance.WeaponInfo.Scope = GameManager.instance.Data.gameData.Guns[4].ScopeImage;
        Scope.GetComponent<Image>().sprite = GameManager.instance.Data.gameData.Guns[4].ScopeImage;
        GameManager.instance.WeaponInfo.Bullet = GameManager.instance.BulletPrefabs[4];
        //CameraMove.instance.BulletAssign();

#if !UNITY_EDITOR
            FirebaseManager.Instance.LogEvent("Rew_Sniper_4");
#endif

        Time.timeScale = 1;



        ButtonSwappingRewardedGuns.instance.selectedsnipergun = 3;
        ButtonSwappingRewardedGuns.instance.StopAllCoroutines();
        ButtonSwappingRewardedGuns.instance.Initialization();
        //ButtonSwappingRewardedGuns.instance.HideGunBtns();




    }





}
