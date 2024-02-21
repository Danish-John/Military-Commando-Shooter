using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class GameManagerZombiesMode : MonoBehaviour
{
    public GameObject CF2Rig;

    public GameObject PlayerCam;

    public static GameManagerZombiesMode instance;

    public Move PlayerMove;

    public GameObject MainUI;

    public Text TotalCoinText;

    public GameObject Haathi;

    MainData main;

    
    public LevelManagerZombiesMode[] Levels;

    [HideInInspector] public int AdCoinMultiplier;


    [HideInInspector] public int TotalScore;
    [HideInInspector] public int AdReward;
    [HideInInspector] public int DoubleVal;

    
    [HideInInspector] public int Enemie_Counter;
    public LevelData Data;
    public int _totalScore { get { return TotalScore; } }



    public Button RewardedAKMNacromencer;
    public Button RewardedAKMDragon;
    public Button RewardedDemonHunter;
    










    void Awake()
    {
        instance = this;


        
        Enemie_Counter = 0;
        GameObject loaderInstance = new GameObject("LevelLoader");

        LoadLevelData DataLoader = loaderInstance.AddComponent<LoadLevelData>();


        if (PlayerPrefs.GetInt("ShowZombieTutorial", 1) == 1)
        {
            DataLoader.InitializeData(null, Data, Data.data[0]);
            Data.CurrentLevel = 0;
            Levels[0].gameObject.SetActive(true);
            
#if !UNITY_EDITOR
             FirebaseManager.Instance.LogEvent("Zombie_Lvl_Start_TuTorialLevel");
#endif
        }
        else
        {
            if (Data.CurrentLevel >= Levels.Length-1)
            {
                Data.CurrentLevel = 0;
            }
            
            DataLoader.InitializeData(null, Data, Data.data[Data.CurrentLevel+1]);

            Levels[Data.CurrentLevel+1].gameObject.SetActive(true);

            int levelToDisplay = Data.CurrentLevel + 1;

#if !UNITY_EDITOR
             FirebaseManager.Instance.LogEvent("Zombie_Lvl_Start_"+ levelToDisplay);
#endif

        }

    }

    private void Start()
    {
        Time.timeScale = 1;
        if (LoadingBar.instance != null)
        {
            LoadingBar.instance.MainObj.SetActive(false);
        }


        if (PlayerPrefs.GetInt("ShowZombieTutorial", 1) == 1)
        {
            Haathi.SetActive(true);
        }
        else
        {
            Haathi.SetActive(false);
        }


        InGameUi.instance.SetHUDData();
        MainUI.SetActive(false);



        InGameUi.instance.DoubleRewardButton.onClick.AddListener(() =>
        {
            InGameUi.instance.DoubleRewardButton.interactable = false;
            //#if !UNITY_EDITOR
            AdsManger_New.Instance.Show_Rewarded_Ads_Priority("double");
            //#endif
        });


        RewardedDemonHunter.onClick.AddListener(() =>
        {
            //#if !UNITY_EDITOR
            AdsManger_New.Instance.Show_Rewarded_Ads_Priority("freegunDemonHunter");
            //#endif
        });


        RewardedAKMNacromencer.onClick.AddListener(() =>
        {
            //#if !UNITY_EDITOR
            AdsManger_New.Instance.Show_Rewarded_Ads_Priority("freegunNacromencer");
            //#endif
        });



        RewardedAKMDragon.onClick.AddListener(() =>
        {
            //#if !UNITY_EDITOR
            AdsManger_New.Instance.Show_Rewarded_Ads_Priority("freegunDragon");
            //#endif
        });


    }



    public void ScoreUp(int AmountToScoreUp)
    {
        TotalScore += AmountToScoreUp;

        InGameUi.instance.ScoreUpTextChange(AmountToScoreUp);

        InGameUi.instance.SetHUDData();
    }

    public void VictoryScreen()
    {
        int TotalEnemies = 0;

        int currLevel = Data.CurrentLevel;

        int LevelCompleteAmount = (currLevel * 50) + 100;

        LevelManagerZombiesMode onGoingLevel = Levels[Data.CurrentLevel];

        for (int i = 0; i < onGoingLevel._Levels.Length; i++)
        {
            for (int j = 0; j < onGoingLevel._Levels[i].EnemiesPerWave.Length; j++)
            {
                TotalEnemies += onGoingLevel._Levels[i].EnemiesPerWave[j];
            }
        }

        InGameUi.instance.TargetsText.text = TotalEnemies + " X 10";

        InGameUi.instance.LevelCompletedCoins.text = LevelCompleteAmount.ToString("n0");
        InGameUi.instance.TotalScoreText.text = (LevelCompleteAmount + (TotalEnemies * 10)).ToString();

        TotalScore += LevelCompleteAmount;

        StartCoroutine(InGameUi.instance.ShowDefeatVictoryText(true));

        //    StartCoroutine(InGameUi.instance.EndGameDisplay(0, TotalScore, InGameUi.instance.TotalScoreText));

        // InGameUi.instance.EnableScreen(InGameUi.ScreenType.Sucess);

        Data.gameData.PlayerCoins += TotalScore;

        InGameUi.instance.ScreenToShow = InGameUi.ScreenType.Sucess;
        CF2Rig.SetActive(false);
        //GameObject.FindGameObjectWithTag("Player").GetComponent<AKMGun>().enabled = false;

#if !UNITY_EDITOR
             FirebaseManager.Instance.LogEvent("Zombie_Lvl_Comp_"+ (Data.CurrentLevel+1));;
#endif

        //#if !UNITY_EDITOR
        AdsManger_New.Instance.Invoke("ShowUnityAdAdmobWithAdBreak", 5f);
        //#endif

    }


    public void SetData()
    {
        if (PlayerPrefs.GetInt("ShowZombieTutorial", 1) != 1)
        {
            Data.CurrentLevel++;

            if (Data.CurrentLevel > Data.LevelsUnlocked)
            {
                Data.LevelsUnlocked++;

                if (Data.LevelsUnlocked > Levels.Length)
                {
                    Data.LevelsUnlocked = Levels.Length;
                }
            }

            Data.SaveZombieModeData();
        }
    }

    public void NewLevelStart()
    {
        PlayerPrefs.SetInt("ShowZombieTutorial", 0);
///////////////////////////////////////////////////////////////    AD  BEFORE STARTING  EVEN-NO  LEVELS       ////////////////////////////////////////////////////////////////////////////////////////////////////////////
        
        if (GameManagerZombiesMode.instance.Data.CurrentLevel % 2 != 0)
        {
//#if !UNITY_EDITOR
            AdsManger_New.Instance.ShowInterstitial();
//#endif
        }

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
    }

    public void Restart()
    {
        SceneManager.LoadScene("ZombieMode");
    }

    public void PauseGame()
    {
        CF2Rig.SetActive(false);
        Settings.instance.TurnSFXOnOff();
        InGameUi.instance.EnableScreen(InGameUi.ScreenType.Pause);

    }


    public void StoryToCutScene()
    {
        GameObject.FindGameObjectWithTag("CutScene").transform.GetChild(0).gameObject.SetActive(true);
        
    }


    public void StartScoreCounter(bool hasWon)
    {
        int score = Data.gameData.PlayerCoins;
        if (hasWon)
        {



            StartCoroutine(InGameUi.instance.EndGameDisplay(score, TotalScore, InGameUi.instance.TotalScoreText));
        }
        else
        {

            StartCoroutine(InGameUi.instance.EndGameDisplay(score, TotalScore, InGameUi.instance.LoseTotalScore));
        }

    }

    public void Enemey_Counter(int TotalEnemies)
    {
        Enemie_Counter = TotalEnemies;

    }



    public void DoubleRewards()
    {

#if !UNITY_EDITOR
        FirebaseManager.Instance.LogEvent("Zombie_Video_AD_Played");
#endif


        //Debug.Log("Double Reward done !!!");

        InGameUi.instance.DoubleRewardButton.gameObject.SetActive(false);
        int TotalEnemies = 0;

        LevelManagerZombiesMode onGoingLevel = Levels[Data.CurrentLevel];

        for (int i = 0; i < onGoingLevel._Levels.Length; i++)
        {
            for (int j = 0; j < onGoingLevel._Levels[i].EnemiesPerWave.Length; j++)
            {
                TotalEnemies += onGoingLevel._Levels[i].EnemiesPerWave[j];
            }
        }


        AdReward = TotalScore * AdCoinMultiplier;




        int currLevel = Data.CurrentLevel;
        int LevelCompleteAmount = (currLevel * 50) + 100;
        InGameUi.instance.TotalScoreText.text = (LevelCompleteAmount + (TotalEnemies * 10)).ToString();

        // FOR COUNTER COIN DISPLAy

        InGameUi.instance.RewardVideoADAcknowldgment.SetActive(true);


        int a = Data.gameData.PlayerCoins;
        //Debug.Log("Coins before ad  = " + a);

        DoubleVal = TotalScore * (AdCoinMultiplier - 1);
        Data.gameData.PlayerCoins = Data.gameData.PlayerCoins + DoubleVal; 
        //Debug.Log("Coins after ad = " + Data.gameData.PlayerCoins);
        Data.SaveZombieModeData();


        StartCoroutine(InGameUi.instance.EndGameDisplay(a, Data.gameData.PlayerCoins, InGameUi.instance.TotalScoreText));

    }


}
