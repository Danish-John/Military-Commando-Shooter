using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManagerWeaponsMode : MonoBehaviour
{
    public GameObject CF2Rig;

    public GameObject PlayerCam;

    public static GameManagerWeaponsMode instance;

    public Move PlayerMove;

    public LevelManagerWeaponsMode [] Levels;


    [HideInInspector] public int AdCoinMultiplier;
    [HideInInspector] public int TotalScore;

    public LevelData Data;
    public int _totalScore { get { return TotalScore; } }
    void Awake()
    {
        instance = this;

     

        if(Data.CurrentLevel>=Levels.Length)
        {
            Data.CurrentLevel = 0;
        }

        GameObject loaderInstance = new GameObject("LevelLoader");

        LoadLevelData DataLoader = loaderInstance.AddComponent<LoadLevelData>();

        DataLoader.InitializeData(null, Data, Data.data[Data.CurrentLevel]);

        Levels[Data.CurrentLevel].gameObject.SetActive(true);

        int levelToDisplay = Data.CurrentLevel + 1;

#if !UNITY_EDITOR

             FirebaseManager.Instance.LogEvent("War_Start_"+ levelToDisplay);

#endif
    }

    private void Start()
    {
        Time.timeScale = 1;
        if (LoadingBar.instance != null)
        {
            LoadingBar.instance.MainObj.SetActive(false);
        }
        InGameUi.instance.ScreenToShow = InGameUi.ScreenType.None;
        InGameUi.instance.EnableScreen(InGameUi.instance.ScreenToShow);
        InGameUi.instance.SetHUDData();
        InGameUi.instance.GameplayHUD.SetActive(true);

        InGameUi.instance.DoubleRewardButton.onClick.AddListener(() =>
          {
            InGameUi.instance.DoubleRewardButton.interactable = false;
//#if !UNITY_EDITOR
            AdsManger_New.Instance.Show_Rewarded_Ads_Priority("double");
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
        int TotalEnemies=0;

        int currLevel = Data.CurrentLevel;

        int  LevelCompleteAmount = (currLevel * 50) + 100;

        LevelManagerWeaponsMode onGoingLevel= Levels[Data.CurrentLevel];

        for (int i=0;i< onGoingLevel._Levels.Length;i++)
        {
            for(int j=0;j < onGoingLevel._Levels[i].EnemiesPerWave.Length;j++)
            {
                TotalEnemies += onGoingLevel._Levels[i].EnemiesPerWave[j];
            }
        }

        InGameUi.instance.TargetsText.text = TotalEnemies+" X 10";

        InGameUi.instance.LevelCompletedCoins.text = LevelCompleteAmount.ToString("n0");

        TotalScore += LevelCompleteAmount;

        StartCoroutine(InGameUi.instance.ShowDefeatVictoryText(true));

        //    StartCoroutine(InGameUi.instance.EndGameDisplay(0, TotalScore, InGameUi.instance.TotalScoreText));

        // InGameUi.instance.EnableScreen(InGameUi.ScreenType.Sucess);

        Data.gameData.PlayerCoins += TotalScore;

        InGameUi.instance.ScreenToShow = InGameUi.ScreenType.Sucess;
        CF2Rig.SetActive(false);
       

#if !UNITY_EDITOR
             FirebaseManager.Instance.LogEvent("War_Complete_"+ Data.CurrentLevel);;
#endif

//#if !UNITY_EDITOR
            AdsManger_New.Instance.Invoke("ShowUnityAdAdmobWithAdBreak", 5f);
//#endif

    }


    public void SetData()
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

        Data.SaveWeaponsModeData();
    }
    public void NewLevelStart()
    {


        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
    }

    public void PauseGame()
    {
        CF2Rig.SetActive(false);
        InGameUi.instance.EnableScreen(InGameUi.ScreenType.Pause);
        InGameUi.instance.GameplayHUD.SetActive(true);
    }

    public void Restart()
    {
        SceneManager.LoadScene("WeaponsMode");
    }



    public void DoubleRewards()
    {

#if !UNITY_EDITOR
        FirebaseManager.Instance.LogEvent("Warzone_Video_AD_Played");
#endif


        //Debug.Log("Double Reward done !!!");

        InGameUi.instance.DoubleRewardButton.gameObject.SetActive(false);
        int TotalEnemies = 0;

        LevelManagerWeaponsMode onGoingLevel = Levels[Data.CurrentLevel];

        for (int i = 0; i < onGoingLevel._Levels.Length; i++)
        {
            for (int j = 0; j < onGoingLevel._Levels[i].EnemiesPerWave.Length; j++)
            {
                TotalEnemies += onGoingLevel._Levels[i].EnemiesPerWave[j];
            }
        }


        int AdReward = TotalScore * AdCoinMultiplier;



        // FOR COUNTER COIN DISPLAy

        InGameUi.instance.RewardVideoADAcknowldgment.SetActive(true);


        int a = Data.gameData.PlayerCoins;
        Debug.Log("Coins before ad  = " + a);


        Data.gameData.PlayerCoins = Data.gameData.PlayerCoins + (TotalScore * (AdCoinMultiplier - 1));
        Debug.Log("Coins after ad = " + Data.gameData.PlayerCoins);
        Data.SaveZombieModeData();


        StartCoroutine(InGameUi.instance.EndGameDisplay(a, Data.gameData.PlayerCoins, InGameUi.instance.TotalScoreText));

    }


}
