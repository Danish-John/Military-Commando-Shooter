using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class LevelManagerZombiesMode : MonoBehaviour
{
    [SerializeField] private EnemiesSpawnerZombies[] Levels;
    public EnemiesSpawnerZombies[] _Levels { get { return Levels; } }

    [HideInInspector] public int CurrentLevel;

    public GameObject BulletsText;

    public float FogValue;

    public GameObject GunHolder;

    public GameObject SecondTuTScreen;

    public static LevelManagerZombiesMode instance;



    /// <summary>
    /// /////////////////////////////////////////////////////////////////////////////////
    /// </summary>
    public ZombieGunsBluePrints[] ZombieGunDetails;

    [HideInInspector] public int totalguns;
    
    /// <summary>
    /// /////////////////////////////////////////////////////////////////////////////////////////////
    /// </summary>



    void Start()
    {
        instance = this;

        PlayerPrefs.SetInt("RewardedGun", 0);
        PlayerPrefs.SetInt("RewardedGunIndex", 0);


        foreach (EnemiesSpawnerZombies level in Levels)
        {
            level.Manager = this;
        }
        StartNextLevel();
        RenderSettings.fogDensity = FogValue;
        totalguns = GunHolder.transform.childCount;


        //if (GameManagerZombiesMode.instance.Data.CurrentLevel == 0 || GameManagerZombiesMode.instance.Data.CurrentLevel == 2 || GameManagerZombiesMode.instance.Data.CurrentLevel == 4 || GameManagerZombiesMode.instance.Data.CurrentLevel == 6)
        //{
        //    NetCheckinLevels();
            
        //}
    }


//    void NetCheckinLevels()
//    {
//        if (AdsManger_New.Instance.IsInternetConnection() == true)
//        {
//#if !UNITY_EDITOR
//            FirebaseManager.Instance.LogEvent("NetOn_Zombie_" + GameManagerZombiesMode.instance.Data.CurrentLevel+1);
//#endif

//        }
//        else if (AdsManger_New.Instance.IsInternetConnection() == false)
//        {
//#if !UNITY_EDITOR
//            FirebaseManager.Instance.LogEvent("NetOff_Zombie_" + GameManagerZombiesMode.instance.Data.CurrentLevel+1);
//#endif

//        }
//    }



    public void CutsceneToGameplay()
    {
        GameManagerZombiesMode.instance.PlayerMove.transform.position = _Levels[CurrentLevel].StartingWaypoint.transform.position;
        GameManagerZombiesMode.instance.MainUI.SetActive(true);
        InGameUi.instance.SetHUDData();

        
        if (PlayerPrefs.GetInt("ShowZombieTutorial", 1) == 1)
        {
            SecondTuTScreen.SetActive(true);
        }

    }



    public void StartNextLevel()
    {
        GameManagerZombiesMode.instance.CF2Rig.SetActive(false);

        if (CurrentLevel < Levels.Length)
        {
            if (CurrentLevel > 0)
            {
                Levels[CurrentLevel - 1].gameObject.SetActive(false);
            }
            Levels[CurrentLevel].gameObject.SetActive(true);
            BulletsText.SetActive(false);

            GameManagerZombiesMode.instance.PlayerMove.transform.parent = Levels[CurrentLevel].StartingWaypoint.transform.parent;
            GameManagerZombiesMode.instance.PlayerMove.SetDestination(Levels[CurrentLevel].StartingWaypoint);
        }
        else
        {
            GameManagerZombiesMode.instance.VictoryScreen();
            GameManagerZombiesMode.instance.StartScoreCounter(true);
        }

        // CurrentLevel++;
    }





    public void GiveRewardedGunDemonHunter()
    {
        for (int j = 0; j < totalguns; j++)
        {
            GunHolder.transform.GetChild(j).gameObject.SetActive(false);
        }

        GunHolder.transform.GetChild(3).gameObject.SetActive(true);
        GunHolder.transform.GetChild(3).gameObject.GetComponent<AKMGun>().enabled = true;  //just enabing gun script of the GUN
        GunHolder.transform.GetChild(3).gameObject.transform.GetChild(1).gameObject.SetActive(false); // We're disabling "Spawnstater" which is (child gameobject) of the gun because we don't want to start a new wave.
        GunHolder.transform.GetChild(3).gameObject.transform.GetChild(2).gameObject.SetActive(true); //enabling mesh


#if !UNITY_EDITOR
        FirebaseManager.Instance.LogEvent("Rew_DemonHunter");
#endif


        GameManagerZombiesMode.instance.RewardedAKMNacromencer.gameObject.SetActive(false);
        GameManagerZombiesMode.instance.RewardedAKMDragon.gameObject.SetActive(false);
        GameManagerZombiesMode.instance.RewardedDemonHunter.gameObject.SetActive(false);

        RewardedGunsInGameplay.instance.selectedzombiegun = 2;
        RewardedGunsInGameplay.instance.index = 0;


        Time.timeScale = 1;


        PlayerPrefs.SetInt("RewardedGun", 1);
        PlayerPrefs.SetInt("RewardedGunIndex", 3);


        RewardedGunsInGameplay.instance.StopAllCoroutines();
        RewardedGunsInGameplay.instance.Initialization();
        //RewardedGunsInGameplay.instance.offfunc = false;
        //RewardedGunsInGameplay.instance.StopAllCoroutines();
    }


    public void GiveRewardedGunNacromencer()
    {
        for (int j = 0; j < totalguns; j++)
        {
            GunHolder.transform.GetChild(j).gameObject.SetActive(false);
        }

        GunHolder.transform.GetChild(1).gameObject.SetActive(true);
        GunHolder.transform.GetChild(1).gameObject.GetComponent<AKMGun>().enabled = true;  //just enabing gun script of the GUN
        GunHolder.transform.GetChild(1).gameObject.transform.GetChild(1).gameObject.SetActive(false); // We're disabling "Spawnstater" which is (child gameobject) of the gun because we don't want to start a new wave.
        GunHolder.transform.GetChild(1).gameObject.transform.GetChild(2).gameObject.SetActive(true);  //enabling mesh


#if !UNITY_EDITOR
        FirebaseManager.Instance.LogEvent("Rew_AKM_Nacromencer");
#endif


        GameManagerZombiesMode.instance.RewardedAKMNacromencer.gameObject.SetActive(false);
        GameManagerZombiesMode.instance.RewardedAKMDragon.gameObject.SetActive(false);
        GameManagerZombiesMode.instance.RewardedDemonHunter.gameObject.SetActive(false);

        RewardedGunsInGameplay.instance.selectedzombiegun = 0;
        RewardedGunsInGameplay.instance.index = 1;





        Time.timeScale = 1;

        PlayerPrefs.SetInt("RewardedGun", 1);
        PlayerPrefs.SetInt("RewardedGunIndex", 1);


        RewardedGunsInGameplay.instance.StopAllCoroutines();
        RewardedGunsInGameplay.instance.Initialization();
        //RewardedGunsInGameplay.instance.offfunc = false;
        //RewardedGunsInGameplay.instance.StopAllCoroutines();
    }

    public void GiveRewardedGunDragon()
    {
        for (int j = 0; j < totalguns; j++)
        {
            GunHolder.transform.GetChild(j).gameObject.SetActive(false);
        }

        GunHolder.transform.GetChild(2).gameObject.SetActive(true);
        GunHolder.transform.GetChild(2).gameObject.GetComponent<AKMGun>().enabled = true;  //just enabing gun script of the GUN
        GunHolder.transform.GetChild(2).gameObject.transform.GetChild(1).gameObject.SetActive(false); // We're disabling "Spawnstater" which is (child gameobject) of the gun because we don't want to start a new wave.
        GunHolder.transform.GetChild(2).gameObject.transform.GetChild(2).gameObject.SetActive(true);  //enabling mesh


#if !UNITY_EDITOR
        FirebaseManager.Instance.LogEvent("Rew_AKM_Dragon");
#endif

        GameManagerZombiesMode.instance.RewardedAKMNacromencer.gameObject.SetActive(false);
        GameManagerZombiesMode.instance.RewardedAKMDragon.gameObject.SetActive(false);
        GameManagerZombiesMode.instance.RewardedDemonHunter.gameObject.SetActive(false);

        RewardedGunsInGameplay.instance.selectedzombiegun = 1;
        RewardedGunsInGameplay.instance.index = 2;




        Time.timeScale = 1;

        PlayerPrefs.SetInt("RewardedGun", 1);
        PlayerPrefs.SetInt("RewardedGunIndex", 2);


        RewardedGunsInGameplay.instance.StopAllCoroutines();
        RewardedGunsInGameplay.instance.Initialization();
        //RewardedGunsInGameplay.instance.offfunc = false;
        //RewardedGunsInGameplay.instance.StopAllCoroutines();

    }



}
