using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using CoverShooter;
using UnityEngine.Events;
using System.Collections.Generic;
using Hellmade.Net;


public class GameManager : MonoBehaviour
{

    public float sliderincreaseValue;

    public ControlFreak2.InputRig IPRing;

    public GameObject Player;

    public Gun WeaponInfo;

    public static GameManager instance;

    [HideInInspector] public int Score;

    private int tempScore;

    private int BonusScore;



    public LevelData Data;

    public LevelManager TutorialStage;
    public LevelManager[] Levels;

    [HideInInspector] public MouseLock mouseLock;

    [HideInInspector] public float ScoreMultiplier;

    [HideInInspector] public float BonusTimer;

    [HideInInspector] public int BonusRewarded;

    [HideInInspector] public int TotalObjectives;

    [HideInInspector] public int HeadShotCount;

    [HideInInspector] public bool LastManStanding;

    [HideInInspector] public bool hasFailed;

    [HideInInspector] public bool isMoving;

    [HideInInspector] public bool InSlowMotion;

    [HideInInspector] public Transform DirectionToLookAt;


    [HideInInspector] public bool isNight;



    [HideInInspector] public int TapCount;

    [HideInInspector] public int AdCoinMultiplier;


    [HideInInspector] public bool GameEnded;

    [HideInInspector] public bool Nothanks = false;


    private bool reviveAdShown;
    private bool AllowActionCam;

    public bool allowactionCam { get { return AllowActionCam; } set { AllowActionCam = value; } }

    private bool IsScoped;

    public bool isScoped { get { return IsScoped; } set { IsScoped = value; } }

    private int ZoomAmount;

    private int LevelCompleteAmount;

    [HideInInspector] public int TotalScore;
    public int zoomAmount { get { return ZoomAmount; } set { ZoomAmount = value; } }

    private int BonusRecieved;

    public float slowDownFactor;
    //public float slowDownLength;

    private Vector3 TargetPosition; // TARGET OF THE BULLET


    public CameraMove PlayerCamera;

    [Tooltip("The Amount of seconds after which the scope will be disabled")]
    public float ShootInterval;

    public GameObject DayLight;

    public GameObject NightLight;

    [HideInInspector] public UnityEvent DisableOutline;

    [HideInInspector] public UnityEvent EnableOutline;

    [HideInInspector] public bool EnemiesAlarmed;

    public CharacterMotor Motor;
    public GameObject[] Guns;
    public GameObject[] HolsterGuns;
    public GameObject[] BulletPrefabs;

    private bool loadScene;

    public Button RewardedKar;
    public Button RewardedM10;
    public Button RewardedM24;
    public Button RewardedSniper4;



    void Awake()
    {

        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        instance = this;

        // DataLoader.LoadLevelData()
        // LoadLevelData DataLoader = new LoadLevelData(Levels[Data.CurrentLevel],Data, Data.data[Data.CurrentLevel]);

        GameObject loaderInstance = new GameObject("LevelLoader");
        LoadLevelData DataLoader = loaderInstance.AddComponent<LoadLevelData>();
        DataLoader.NightLight = NightLight;
        DataLoader.DayLight = DayLight;

        if (PlayerPrefs.GetInt("ShowTutorial", 1) == 1)
        {

            DataLoader.InitializeData(Levels[0], Data, Data.data[0]);
            Data.CurrentLevel = 0;
            Levels[0].Player = Player.GetComponent<Move>();

        }
        else
        {
            if (Data.CurrentLevel == 0 || Data.CurrentLevel >= Levels.Length)
            {
                Data.CurrentLevel = 1;
            }

            DataLoader.InitializeData(Levels[Data.CurrentLevel], Data, Data.data[Data.CurrentLevel]);
            Levels[Data.CurrentLevel].Player = Player.GetComponent<Move>();
        }


        DataLoader.AssignWeapon(WeaponInfo, Guns, HolsterGuns);

        PlayerCamera.CurrentLevel = Levels[Data.CurrentLevel];


        PlayerCamera.ZoomFov = ZoomAmount;
    }





    void Start()
    {

        if (LoadingBar.instance != null)
        {
            LoadingBar.instance.MainObj.SetActive(false);
        }



        if (AdsManger_New.Instance != null)
        {
            

//#if !UNITY_EDITOR
                 AdsManger_New.Instance.ShowSmartBannerTopMid();
//#endif

            InGameUi.instance.DoubleRewardButton.onClick.AddListener(() =>
            {
                InGameUi.instance.DoubleRewardButton.interactable = false;
//#if !UNITY_EDITOR
                 AdsManger_New.Instance.Show_Rewarded_Ads_Priority("double");
//#endif
            });

            InGameUi.instance.ReviveButton.onClick.AddListener(() =>
            {
                
//#if !UNITY_EDITOR
                    AdsManger_New.Instance.Show_Rewarded_Ads_Priority("revive");
                    //AdsManager.Instance.ShowRewardVideo("revive");
//#endif

            });


            InGameUi.instance.SkipLevelButton.onClick.AddListener(() =>
            {

//#if !UNITY_EDITOR
                 AdsManger_New.Instance.Show_Rewarded_Ads_Priority("skipLVL");
//#endif
            });


            RewardedKar.onClick.AddListener(() =>
            {  
//#if !UNITY_EDITOR
                AdsManger_New.Instance.Show_Rewarded_Ads_Priority("freegunKar");
//#endif
            });


            RewardedM10.onClick.AddListener(() =>
            {
//#if !UNITY_EDITOR
                AdsManger_New.Instance.Show_Rewarded_Ads_Priority("freegunM10");
//#endif
            });



            RewardedM24.onClick.AddListener(() =>
            {
//#if !UNITY_EDITOR
                AdsManger_New.Instance.Show_Rewarded_Ads_Priority("freegunM24");
//#endif
            });



            RewardedSniper4.onClick.AddListener(() =>
            {
//#if !UNITY_EDITOR
                AdsManger_New.Instance.Show_Rewarded_Ads_Priority("freegunSniper4");
//#endif
            });





        }

        Time.timeScale = 1;

        Time.fixedDeltaTime = 0.02f;

        mouseLock = PlayerCamera.GetComponent<MouseLock>();

        hasFailed = true;

        GameEnded = false;

        CutsceneLog();

    }

    public void CutsceneLog()
    {
        if (PlayerPrefs.GetInt("ShowTutorial", 1) == 1)
        {
#if !UNITY_EDITOR
            FirebaseManager.Instance.LogEvent("Entered_ObjectivePanel");
#endif
        }
    }
    public void StartLog()
    {

        if (PlayerPrefs.GetInt("ShowTutorial", 1) == 1)
        {
#if !UNITY_EDITOR
            FirebaseManager.Instance.LogEvent("SniperLevel_Started_0");
#endif
        }
        else
        {
            int logLevel = Data.CurrentLevel;


#if !UNITY_EDITOR
         FirebaseManager.Instance.LogEvent("SniperLevel_Started_" + logLevel);
#endif


        }

    }

    public void SetTotalEnemies()
    {


        for (int i = 0; i < Levels[Data.CurrentLevel].Levels.Length; i++)
        {
            TotalObjectives += Levels[Data.CurrentLevel].Levels[i].Enemies.Length + Levels[Data.CurrentLevel].Levels[i].Objectives.Length;

        }


    }


    void FixedUpdate()
    {


        if (isMoving)
        {
            InGameUi.instance.ShootSlider.gameObject.SetActive(false);
            DisableScope(true);
            TapCount = 0;
        }


    }

    public void RevivePlayer()
    {
        Time.timeScale = 1;
        //Debug.Log("TimeScale 1 = " + Time.timeScale);
        reviveAdShown = true;

        if (PlayerCamera._Controller.ScopeInput)
        {
            PlayerCamera.LockOnTarget();

        }
        hasFailed = true;
        GameEnded = false;

        InGameUi.instance.FadeBackgroundVictoryDefeat.SetActive(false);
        InGameUi.instance.BulletText.gameObject.SetActive(true);
        InGameUi.instance.BulletImage.SetActive(true);

        InGameUi.instance.ReviveButton.gameObject.SetActive(false);
        WeaponInfo.LoadedBullets = 5;


        InGameUi.instance.SetBulletText(WeaponInfo.LoadedBullets);
        //Debug.Log("TimeScale 2 = " + Time.timeScale);


        InGameUi.instance.ResumeGame();
        


        //Debug.Log("TimeScale 3 = " + Time.timeScale);



        IPRing.gameObject.SetActive(true);

        PlayerCamera.ScopeRect.SetActive(true);

        mouseLock.enabled = true;
        PlayerCamera._Input.enabled = true;



#if !UNITY_EDITOR
        FirebaseManager.Instance.LogEvent("Video_AD_Played");
#endif
        Time.timeScale = 1;
        //Debug.Log("TimeScale 4 = " + Time.timeScale);

    }


    public void NoThanksRevive()
    {
        Nothanks = true;
    }

    public void CheckGameOver()
    {
        int totalEnemies = 0;
        if (Data.CurrentLevel != 0) // MEANING NOT TUTORIAL
        {
            totalEnemies = Levels[Data.CurrentLevel]._TotalEnemies;


        }
        else
        {
            totalEnemies = Levels[0]._TotalEnemies;
        }



        if (WeaponInfo.LoadedBullets < 1 && hasFailed)   // DEFEAT

        {
            GameEnded = true;
            InGameUi.instance.LoseTargetCount.text = "10 x " + 0;
            InGameUi.instance.LooseTotalAmount.text = PlayerPrefs.GetInt("PlayerCoins").ToString();


            if (reviveAdShown)
            {
                //Debug.Log("ReviveadShown Masla Spotted 1 ");
                
                InGameUi.instance.ScreenToShow = InGameUi.ScreenType.Defeat;

            }
            else
            {
                //Debug.Log("ReviveadShown Masla Spotted 2 ");
                InGameUi.instance.DefeatText.SetActive(true);
                InGameUi.instance.ScreenToShow = InGameUi.ScreenType.Revive;
                
            }


            if (Nothanks == true)
            {
                Debug.Log("ReviveadShown Masla Spotted 3 ");


                StartCoroutine(InGameUi.instance.ShowDefeatVictoryText(false));
                InGameUi.instance.DefeatText.SetActive(true);
                LevelCompleteAmount = 0;


                #region FIREBASE AND STUFF
#if UNITY_EDITOR
                InGameUi.instance.EnableScreen(InGameUi.instance.ScreenToShow);
#endif
                int logLevel = Data.CurrentLevel;


                if (PlayerPrefs.GetInt("ShowTutorial", 1) == 1)
                {
#if !UNITY_EDITOR
                     FirebaseManager.Instance.LogEvent("SniperLevel_Failed_0");
#endif
                }
                else
                {
#if !UNITY_EDITOR
                     FirebaseManager.Instance.LogEvent("SniperLevel_Failed_"+logLevel);
#endif
                }
                #endregion

                InGameUi.instance.LoseTargetCount.text = "10 x " + 0;
                InGameUi.instance.LooseTotalAmount.text = PlayerPrefs.GetInt("PlayerCoins").ToString();
                Nothanks = false;
            }


        }

        /* EndScreenTotalScoreText = InGameUi.instance.LoseTotalScore;

         TotalEnemiesTextEnd = InGameUi.instance.LoseTargetCount;*/


        else if (!hasFailed)  // VICTORY

        {

            GameEnded = true;

            InGameUi.instance.ScreenToShow = InGameUi.ScreenType.Sucess;

            StartCoroutine(InGameUi.instance.ShowDefeatVictoryText(true));


            int currLevel = Data.CurrentLevel + 1;
            LevelCompleteAmount = (currLevel * 50) + 100;


            /*  EndScreenTotalScoreText = InGameUi.instance.TotalScoreText;
              TotalEnemiesTextEnd = InGameUi.instance.TargetsText;*/

            #region FIREBASE AND STUFF
#if UNITY_EDITOR
            // InGameUi.instance.EnableScreen(InGameUi.instance.ScreenToShow);
#endif

            int logLevel = Data.CurrentLevel;

            if (PlayerPrefs.GetInt("ShowTutorial", 1) == 1)
            {
#if !UNITY_EDITOR
            FirebaseManager.Instance.LogEvent("SniperLevel_Completed_0");
#endif
            }
            else
            {
#if !UNITY_EDITOR
             FirebaseManager.Instance.LogEvent("SniperLevel_Completed_"+ logLevel);;
#endif
            }

            #endregion

            Rewards();
            InGameUi.instance.TargetsText.text = "10 x " + totalEnemies;

        }


        if (GameEnded)   /* THIS IF STATMENT IS HERE TO ENSURE THAT THE THE FUNCTION IS NOT CALLED EVERYTIME. ONLY CALLED WHEN
                             EITHER THE BULLETS ARE ZERO OR THE ENEMIES HAVE ALL DIED
                         */
        {
            PlayerCamera.ScopeRect.SetActive(false);
            IPRing.gameObject.SetActive(false);
            mouseLock.enabled = false;
            PlayerCamera._Input.enabled = false;
            InGameUi.instance.BulletText.gameObject.SetActive(false);
            InGameUi.instance.BulletImage.SetActive(false);

            

            TotalScore = Score + LevelCompleteAmount;


            Data.gameData.PlayerCoins += TotalScore;


            Data.SaveNormalModeData();

            InGameUi.instance.LevelCompletedCoins.text = LevelCompleteAmount.ToString("n0");



            //StartCoroutine(InGameUi.instance.EndGameDisplay(totalEnemies, 0, LevelCompleteAmount, TotalScore, EndScreenTotalScoreText, TotalEnemiesTextEnd));
//#if !UNITY_EDITOR
            AdsManger_New.Instance.Invoke("ShowUnityAdAdmobWithAdBreak", 5f);
//#endif
        }


    }

    public void StartScoreCounter(bool hasWon)
    {

        if (hasWon)
        {
            StartCoroutine(InGameUi.instance.EndGameDisplay(0, TotalScore, InGameUi.instance.TotalScoreText));
        }
        else
        {

            StartCoroutine(InGameUi.instance.EndGameDisplay(0, TotalScore, InGameUi.instance.LoseTotalScore));
        }

    }


    public void SkipLevel()
    {
        InGameUi.instance.SkipLevelButton.gameObject.SetActive(false);
        Rewards();
        Restart();
    }

    private void Rewards()
    {

        if (PlayerPrefs.GetInt("ShowTutorial", 1) == 1)
        {
            //   InGameUi.instance.DoubleRewardButton.gameObject.SetActive(false);
        }

        if (PlayerPrefs.GetInt("ShowTutorial", 1) == 0)
        {

            Data.CurrentLevel++;
        }



        if (Data.CurrentLevel > Data.LevelsUnlocked)
        {
            Data.LevelsUnlocked++;
        }

        if (Data.LevelsUnlocked >= Levels.Length)
        {
            Data.LevelsUnlocked = Levels.Length - 1;
        }


        Data.gameData.PlayerCoins += TotalScore;

        Data.SaveNormalModeData();

    }

    public void DoubleRewards()
    {


        InGameUi.instance.DoubleRewardButton.gameObject.SetActive(false);
        int TotalEnemies = 0;
        if (Data.CurrentLevel > 0)
        {
            TotalEnemies = Levels[Data.CurrentLevel - 1]._TotalEnemies;
        }
        else
        {
            TotalEnemies = Levels[0]._TotalEnemies;
        }


        /* int newScoreAmount = Score * 2;
         Data.PlayerCoins += Score;
         Data.SaveData();*/

        int AdReward = TotalScore * AdCoinMultiplier;
        int newScoreAmount = Score + AdReward;
        Data.gameData.PlayerCoins += AdReward;
        Data.SaveNormalModeData();

        // FOR COUNTER COIN DISPLAy

        InGameUi.instance.RewardVideoADAcknowldgment.SetActive(true);

       
        StartCoroutine(InGameUi.instance.EndGameDisplay(Score, TotalScore, InGameUi.instance.TotalScoreText));


#if !UNITY_EDITOR
        FirebaseManager.Instance.LogEvent("Video_AD_Played");
#endif


    }

    public void ScoreUp(int AmountToScoreUp)
    {

        if (AmountToScoreUp > 0)
        {

            tempScore += AmountToScoreUp;

        }

    }
    public void scoreMultiply()
    {

        if (tempScore > 0)
        {

            int scoreUpAm = (int)ScoreMultiplier + tempScore;

            InGameUi.instance.ScoreUpTextChange(scoreUpAm);


            Score += scoreUpAm;

            tempScore = 0;

            ScoreMultiplier = 0;

            InGameUi.instance.SetHUDData();
        }

    }



    public void FinishActionSequence()  // CALL THIS WHEN THE BULLET HITS
    {
        InSlowMotion = false;

        IPRing.gameObject.SetActive(true);

        PlayerCamera.gameObject.SetActive(true);

    }

    public void slowMotion()
    {

        Time.timeScale = slowDownFactor;
        Time.fixedDeltaTime = Time.timeScale * 0.005f;

    }


    public void ActionCameraEnable()
    {
        InSlowMotion = true;
        DisableScope(true);
        Levels[Data.CurrentLevel].Levels[Levels[Data.CurrentLevel].LevelIndex].DisableEnemyAggro();
        IPRing.gameObject.SetActive(false);
        PlayerCamera.gameObject.SetActive(false);
        slowMotion();

    }


    public void DisableScope(bool disableInstant)
    {
        if (disableInstant)
        {
            unScope();
        }

        else
        {
            int disableTimer = 1;
            if (LastManStanding)    // IF THIS CHECK IS REMOVED THEN WHEN THE LAST ENEMY OF A LEVEL PART IS KILLED THE CAMERA WILL LOOK AT WALLS DO NOT REMOVE THIS
            {
                disableTimer = 2;
            }
            Invoke("unScope", disableTimer);
        }

        //  InGameUi.instance.BulletText.gameObject.SetActive(false);
        //  InGameUi.instance.BulletImage.SetActive(false);
    }

    void unScope()
    {
        PlayerCamera._Controller.ZoomInput = false;
        PlayerCamera._Controller.ScopeInput = false;
    }
    public void NextLevel()
    {
        /*
         When the next button is clicked in the VICTORY OR DEFEAT SCREEN....First the COIN COUNTER
         Will Stop ...then on second click the SCENE Loading Will Start
         */
        /* if(InGameUi.instance._stopCointCounter && !loadScene)
         {
             */
        PlayerPrefs.SetInt("ShowTutorial", 0);

        if (GameManager.instance.Data.CurrentLevel >= 4)
        {
            if (GameManager.instance.Data.CurrentLevel % 2 == 0)
            {
                //#if !UNITY_EDITOR
                AdsManger_New.Instance.ShowInterstitial();
                //#endif
            }
        }


#if !UNITY_EDITOR
         LoadingBar.instance.LoadScene(SceneManager.GetActiveScene().name);
#endif


#if UNITY_EDITOR
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);

#endif
        loadScene = true;

        //}
        /*  else
          {
              InGameUi.instance._stopCointCounter = true;
          }*/

        Data.SaveNormalModeData();

    }

    public void PauseGame()
    {
        mouseLock.enabled = false;
        InGameUi.instance.EnableScreen(InGameUi.ScreenType.Pause);

    }


    public void Restart()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
    }


}
