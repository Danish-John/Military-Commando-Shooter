using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CoverShooter;
using UnityEngine.UI;
using Cinemachine;
using UnityEngine.SceneManagement;

public class GameManagerPvp : MonoBehaviour
{
    public GameObject TutorialObj1;
    public GameObject TutorialObj2;
    public ControlFreak2.InputRig IPRig;
    public static GameManagerPvp instance;
   //public Gun WeaponInfo;
    public bool GameEnded;
    [HideInInspector] public int PlayerScore;
    [HideInInspector] public int BotScore;
    [HideInInspector] public int AdCoinMultiplier;
    [HideInInspector] public MouseLock mouseLock;
    public GameObject Player;
    public LevelData PvPGameData;
    public Transform CameraPosition;
    private int SelectedIndex;
    [Tooltip("The Amount of seconds after which the scope will be disabled")]
    public float ShootInterval;

    [SerializeField] private Gun WeaponInfo;
   
    [SerializeField] private LevelPvP [] Levels;
    [SerializeField] private EnemyAI[] Bots;
    [SerializeField] private GameObject[] Guns;
    [SerializeField] private GameObject[] HolsterGuns;
    public float Timer;
    private float StarTingTimerValue;
    public int NumberOfTurns;
    private int CurrentRound;
    public Image ScopeImage;  // CHANGE IN FUTURE BUILD FIX IT
    public bool stopTimer;
    public GameObject FinalCam;
    public LevelPvP SelectedLevel { get 
             {
                   if(PlayerPrefs.GetInt("ShowPvpTutorial",1)==1)
                    {
                        PvPGameData.CurrentLevel = 0;
                    }
                   else
                    {
                        if (PvPGameData.CurrentLevel >= Levels.Length)
                        {
                            PvPGameData.CurrentLevel = 1;
                        }

                    }
                   
               
                    return Levels[PvPGameData.CurrentLevel] 

            ;} 
    }

    public EnemyAI SelectedBot { get { return Bots[SelectedIndex];} }
    private bool isPlayerTurn;

    public bool IsPlayerTurn { get { return isPlayerTurn; } }
    // TIME BETWEEN EACH SHOT
    // TURN BASED  SWAP PLAYERS AFTER EACH SHOT
    // SHOOTER AI
    // OTHER PLAYER IN THIRD PERSON MODE
    public CameraMove PlayerVcam;
    public CinemachineVirtualCamera BotVcam;
    public ThirdPersonInput Input;
    private CharacterInventory inventory;

    

    [HideInInspector] public Targets [] BotTargets;

    [HideInInspector] public bool TurnStart;

    private bool LoadScene;

    private bool adWatched;

    [HideInInspector] public bool TutorialShown;
    public GameObject BotTargetCross;
    public GameObject PlayerCross;
    private GameObject PlayerTarget;

    public Material NightSkyBoxMaterial;
    public GameObject DayMapObject;
    public GameObject NightMapObject;
    public GameObject SnowMapObject;

    public AudioClip PlayerTurnClip;
    public AudioClip PlayerTurnClip2;
    public AudioClip WatforTurnClip;
   [HideInInspector] public AudioSource AudSource;
 
    void Awake()
    {
        Time.timeScale = 1;
        instance = this;
        SelectedIndex = Random.Range(0, Bots.Length);

        for (int i = 0; i < Bots.Length; i++)
        {
            if (i != SelectedIndex)
            {
                Bots[i].gameObject.SetActive(false);
            }
        }

        BotTargets = SelectedLevel.BotTargets;

        BotTargetCross.transform.position = SelectedLevel.BotTargets[9].transform.position;  // THE THE LAST TARGET

        BotTargetCross.transform.parent = SelectedLevel.BotTargets[9].transform;

        PlayerTarget = SelectedLevel.PlayerTarget;
        PlayerCross.transform.position = new Vector3(PlayerTarget.transform.position.x, PlayerTarget.transform.position.y, PlayerTarget.transform.position.z - 0.05f);
        PlayerCross.transform.parent = PlayerTarget.transform;
        PlayerCross.SetActive(false);
 }

    

    public void FixedUpdate()
    {
        if(ControlFreak2.CF2Input.GetMouseButtonDown(0) && !IsPlayerTurn)
        {

            notPlayerTurn();
        }
    }
    void Start()
    {
        StarTingTimerValue = Timer;
        TurnStart = true;
        StartCoroutine(RoundTimer());
      
       
        if (AdsManger_New.Instance != null)
        {
            InGameUi.instance.DoubleRewardButton.onClick.AddListener(() =>
            {
                InGameUi.instance.DoubleRewardButton.interactable = false;
#if !UNITY_EDITOR

                 AdsManger_New.Instance.Show_Rewarded_Ads_Priority("double");

#endif
            });


        }

        if (LoadingBar.instance != null)
        {
            LoadingBar.instance.MainObj.SetActive(false);
        }


#if !UNITY_EDITOR
        int logLevel = PvPGameData.CurrentLevel + 1;
        FirebaseManager.Instance.LogEvent("PvPLevel_Started_"+ logLevel);
#endif

        BotTargets = SelectedLevel.BotTargets;
        CurrentRound = 1;
        isPlayerTurn = true;
        Input = PlayerVcam._Input;
        mouseLock = PlayerVcam.GetComponent<MouseLock>();
        Input.AllowShooting = true;  // DISABLE THIS IF CAUSES ERROR
        inventory = Player.GetComponent<CharacterInventory>();
        PlayerVcam._Controller.InputEquip(inventory.Weapons[0]);
        GameObject loaderInstance = new GameObject("LevelLoader");
        LoadLevelData DataLoader = loaderInstance.AddComponent<LoadLevelData>();
        DataLoader.InitializeData(null, PvPGameData, PvPGameData.data[PvPGameData.CurrentLevel]);

        Levels[PvPGameData.CurrentLevel].gameObject.SetActive(true);

        DataLoader.AssignWeapon(WeaponInfo, Guns, HolsterGuns);
        InGameUiPVP.instance.RoundsDisplay.text =  CurrentRound + "/" + NumberOfTurns;
        AudSource =GetComponent<AudioSource>();
        PlayerVcam.transform.position = CameraPosition.position;
        PlayerVcam.transform.rotation = CameraPosition.rotation;

        if(PlayerPrefs.GetInt("ShowPvpTutorial", 1)==0)
        {
            PlayerVcam.Wind.ChangeWind();
        }
       
        BotVcam.Follow = SelectedBot.transform;


        InGameUiPVP.instance.PlayerTurnShowcase.SetActive(false);

        // InGameUiPVP.instance.Invoke("ShowcaseDisable", 2f);

        ScopeImage.sprite = WeaponInfo.Scope;  // MUST CHAGNE

        AudSource.PlayDelayed(2f);

  
    }

    public void notPlayerTurn()
    {
    
        if(!SelectedBot._AiShot)
        {
            AudSource.PlayOneShot(WatforTurnClip);
        }
        
    }
   
    public void PauseGame()
    {
        mouseLock.enabled = false;

        StartCoroutine(InGameUi.instance.FinalCoinsCounter(0, PvPGameData.gameData.PlayerCoins));
        InGameUi.instance.EnableScreen(InGameUi.ScreenType.Pause);
        TutorialObj1.SetActive(false);
        TutorialObj2.SetActive(false);
    }

  
    public void NextLevel()
    {

        if (!LoadScene)
        {

#if !UNITY_EDITOR
          LoadingBar.instance.LoadScene(SceneManager.GetActiveScene().name);
#endif


#if UNITY_EDITOR
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);

#endif
            LoadScene = true;

            if(!adWatched)
            {
               
            }
           

        }


    }

    public void RestartLevel()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
    }
    IEnumerator RoundTimer()
    {
        while(Timer>=0)
        {
           

            if (Timer == 0)
            {
                ChangeTurn();
            }
            else
            {
              
                InGameUiPVP.instance.TimerText.text = Timer + "s";
            }

            if(TurnStart && !stopTimer)
            {
                Timer--;

                float timerPercentage = Timer / StarTingTimerValue;

                InGameUiPVP.instance.ChangeTimerSliderValue(timerPercentage);
            }
      

            yield return new WaitForSeconds(1f);
        }
    }
    public void ChangeTurn()
    {
        Timer = StarTingTimerValue;

        float timerPercentage = Timer / StarTingTimerValue;

        InGameUiPVP.instance.ChangeTimerSliderValue(timerPercentage);

        stopTimer = false;
        TurnStart = false;
       
        isPlayerTurn = !isPlayerTurn;

        if(CurrentRound-1<NumberOfTurns)
        {
            if (isPlayerTurn)
            {
                SelectedBot._AiShot = false;
                InGameUiPVP.instance.RivalTurnHand.SetActive(false);
                BotTargetCross.SetActive(true);
                PlayerCross.SetActive(false);
                PlayerVcam.Wind.ChangeWind();
                mouseLock.enabled = true;
               
                BotVcam.gameObject.SetActive(false);
                PlayerVcam.gameObject.SetActive(true);
                InGameUiPVP.instance.PlayerTurnShowcase.SetActive(true);

                InGameUiPVP.instance.RoundsDisplay.text = CurrentRound + "/" + NumberOfTurns;
                AudSource.PlayOneShot(PlayerTurnClip);
             
                if(!TutorialShown)
                {
                    TutorialObj1.SetActive(true);
                }
            }
            else
            {
                TutorialObj1.SetActive(false);
                InGameUiPVP.instance.RivalTurnHand.SetActive(true);
                BotTargetCross.SetActive(false);
                PlayerCross.SetActive(true);
                PlayerVcam.ScopeRect.gameObject.SetActive(false);
                mouseLock.enabled = false;
                BotVcam.gameObject.SetActive(true);
                PlayerVcam.gameObject.SetActive(false);
                CurrentRound++;
                InGameUiPVP.instance.BotTurnShowCase.SetActive(true);
                SelectedBot.Invoke(nameof(SelectedBot.Shoot), 4f);

            }
            InGameUiPVP.instance.Invoke("ShowcaseDisable", 2f);

        }
        else
        {
            FinalCam.SetActive(true);
            BotVcam.gameObject.SetActive(false);
            PlayerVcam.gameObject.SetActive(false);
            if (PlayerScore >= BotScore)
            {
                // GameOver(true);// win
                InGameUiPVP.instance.WinAcknoledgment.SetActive(true);

            }
            else if (PlayerScore < BotScore)
            {

                InGameUiPVP.instance.DefeatAcknowledgment.SetActive(true);
            }

          
        }
       
    
    }


    void Reward()
    {

        if(PlayerPrefs.GetInt("ShowPvpTutorial", 1)==1)
        {
            PlayerPrefs.SetInt("ShowPvpTutorial", 0);
        }
      

        
        PvPGameData.CurrentLevel++;
        
        if (PvPGameData.CurrentLevel > PvPGameData.LevelsUnlocked)
        {
            PvPGameData.LevelsUnlocked++;
        }

        if (PvPGameData.LevelsUnlocked >= Levels.Length)
        {
            PvPGameData.LevelsUnlocked = Levels.Length - 1;
        }

        PvPGameData.gameData.PlayerCoins += 500;

        PvPGameData.SavePvpData();

    }

    public void DoubleRewards()
    {

     
        adWatched = true;
       
        int AdReward = 500 * AdCoinMultiplier;

        PvPGameData.gameData.PlayerCoins += AdReward;
       // InGameUi.instance.DoubleRewardButton.gameObject.SetActive(false);
        InGameUi.instance.RewardVideoADAcknowldgment.SetActive(true);

        StartCoroutine(InGameUi.instance.FinalCoinsCounter(AdReward, PvPGameData.gameData.PlayerCoins));

        PvPGameData.SavePvpData();

        FirebaseManager.Instance.LogEvent("PVP_Video_AD_Played");

    }
    public void GameOver(bool hasWon)
    {
        mouseLock.enabled = false;
        IPRig.gameObject.SetActive(false);
     

       // FinalCam.SetActive(true);

        TutorialObj1.SetActive(false);
        TutorialObj2.SetActive(false);

        bool victory=false;
        int logLevel = PvPGameData.CurrentLevel + 1;


        int TotalScore = 0;
        int prevAmount = 0;

        if (hasWon)   //WIN
        {

            TotalScore = 500;

            prevAmount = PvPGameData.gameData.PlayerCoins;

        
            victory = true;
            InGameUi.instance.ScreenToShow = InGameUi.ScreenType.Sucess;

#region FIREBASE AND STUFF
#if UNITY_EDITOR
            InGameUi.instance.EnableScreen(InGameUi.instance.ScreenToShow);
#endif



#if !UNITY_EDITOR
             if( FirebaseManager.Instance!=null)
             {
                FirebaseManager.Instance.LogEvent("PvPLevel_Completed_"+ logLevel);;
             }
          
#endif


#endregion

            Reward();

           
        }
        else if(!hasWon) // LOSE
        {
            victory = false;
            InGameUi.instance.ScreenToShow = InGameUi.ScreenType.Defeat;

#region FIREBASE AND STUFF
#if UNITY_EDITOR
            InGameUi.instance.EnableScreen(InGameUi.instance.ScreenToShow);
#endif

#if !UNITY_EDITOR
              if( FirebaseManager.Instance!=null)
              {
               FirebaseManager.Instance.LogEvent("PvPLevel_Failed_"+logLevel);
              }
           
#endif


            #endregion


        }

        Debug.Log(TotalScore);
        InGameUiPVP.instance.EndGameScreen(BotScore, PlayerScore, victory);
           // StartCoroutine(InGameUi.instance.ShowDefeatVictoryText(victory));
           StartCoroutine(InGameUi.instance.FinalCoinsCounter(TotalScore, prevAmount));
//#if !UNITY_EDITOR
         
             AdsManger_New.Instance.Invoke("ShowUnityAdAdmobWithAdBreak", 5f);
          
//#endif
    }


}


