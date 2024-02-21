using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;
using System.Collections;
public class InGameUi : MonoBehaviour
{
    
    public enum ScreenType {Defeat,Pause,Sucess,Revive,None}
   [HideInInspector] public ScreenType ScreenToShow;
    public static InGameUi instance;
    public Text QuestDescription;
    public GameObject CutsceneObj;
    public GameObject GameplayHUD;
    public GameObject PauseScreen;
    public GameObject BottomBannerAdBackground;
    public GameObject TopBannerAdBackground;
    public GameObject VictoryText;
    public GameObject DefeatText;
    public GameObject FadeBackgroundVictoryDefeat;
    public GameObject BulletImage;
    public GameObject CoinParticleObject;
    public GameObject RewardVideoADAcknowldgment;
    public GameObject ReviveVideoAdAcknowledgment;
    public GameObject[] PauseScreenObjects;
    public GameObject ReviveScreen;
    public GameObject PauseButton;
    public Text ScoreUpText;
    public Text Score;
    public Text BonusText;
    public Text TimerText;
    public Text TotalCoinText;
    public Text PauseScreenCoins;

    public Text LevelNumber;
    public Text InGameCoinsEarned;
    public Text TotalLevelEnemies;

    public Slider ShootSlider;
    public Text BulletText;
    public Text HeadShotTextIngame;
    public Text TargetsText;
    public Text LevelCompletedCoins;
    public Text CoinRewardVideoText;
    public Text TotalScoreText;
    public Text LoseTargetCount;
    public Text LoseTotalScore;
    public Text LooseTotalAmount;
    public Button StartGameBtn;
    public Button SkipLevelButton;
    public Button SKipCutsceneBtn;
    public Button DoubleRewardButton;
    public Button ReviveButton;
    public Button SFXButton;
    public Button BGMButton;
   
    public Image SFXImage;
    public Image BGMImage;
    public Image headShotImage;
    private bool StopCoinCounter;
    private GameManager gameManager;
    public bool _stopCointCounter { get { return StopCoinCounter; }  set { StopCoinCounter = value; } }

    public GameObject NextLevelBtn;

    public AudioSource AudSource;
    public AudioClip CoinClip;
    private void Awake()
    {
        instance = this;

        
        if (Settings.instance != null)
        {
            if (PlayerPrefs.GetInt("BGMAudio", 1) == 1)
            {
                Settings.instance.isBGMOn = true;
                BGMImage.sprite = Settings.instance.OnoffSprites[0];
            }
            else
            {
                Settings.instance.isBGMOn = false;
                BGMImage.sprite = Settings.instance.OnoffSprites[1];
            }

            if (PlayerPrefs.GetInt("SFXAudio", 1) == 1)
            {
                Settings.instance.isSFXOn = true;
                SFXImage.sprite= Settings.instance.OnoffSprites[0];
            }
            else
            {
                Settings.instance.isSFXOn = false;
                SFXImage.sprite= Settings.instance.OnoffSprites[1];
            }

            SFXButton.onClick.AddListener(() =>
            {
                Settings.instance.TurnSFXOnOff();
                Settings.instance.SaveSettings();
                SFXImage.sprite = Settings.instance._SFXSpriteToUse;
            });

            BGMButton.onClick.AddListener(() =>
            {
                Settings.instance.TurnBGMOnOff();
                Settings.instance.SaveSettings();
                BGMImage.sprite = Settings.instance._BGMSpriteToUse;
            });
        }
        //InteractBTN.SetActive(false);
    }


    private void Start()
    {
        gameManager = GameManager.instance;

        

        if(gameManager!=null)
        {
            SetBulletText(gameManager.Levels[gameManager.Data.CurrentLevel].NumberOfBullets);
        }
        
        if (PlayerPrefs.GetInt("ShowTutorial", 1) == 1 || PlayerPrefs.GetInt("ShowZombieTutorial", 1) == 1)
        {
            PauseButton.SetActive(false);
        }

        if (GameManagerWeaponsMode.instance)
        {
            PauseButton.SetActive(true);
        }
    }

    public void SetHUDData()
    {
       
        int TotalRemaining = 0;
        int CurrentLevel = 0;
        int scoreToShow = 0;
        if (gameManager)
        {
            TotalRemaining = gameManager.TotalObjectives;
            CurrentLevel = gameManager.Data.CurrentLevel;

            int CurrentpartIndex = gameManager.Levels[CurrentLevel].LevelIndex;

            if (CurrentpartIndex < gameManager.Levels[CurrentLevel].Levels.Length)
            {
                 scoreToShow = gameManager.Score + gameManager.Data.gameData.PlayerCoins;
            }
            
        }

        else if (GameManagerWeaponsMode.instance)
        {
            GameManagerWeaponsMode Manager = GameManagerWeaponsMode.instance;

            CurrentLevel = Manager.Data.CurrentLevel +1;

            LevelManagerWeaponsMode ongoingLevel = Manager.Levels[CurrentLevel-1];

           
        

            if (ongoingLevel.CurrentLevel< ongoingLevel._Levels.Length)
            {
                TotalRemaining = ongoingLevel._Levels[ongoingLevel.CurrentLevel]._RemainingEnemies;       
            }
            else
            {
                TotalRemaining = ongoingLevel._Levels[ongoingLevel.CurrentLevel-1]._RemainingEnemies;
            }

          
            scoreToShow = Manager._totalScore + Manager.Data.gameData.PlayerCoins;
           
        }

        else if (GameManagerZombiesMode.instance)
        {
            GameManagerZombiesMode Manager = GameManagerZombiesMode.instance;

            CurrentLevel = Manager.Data.CurrentLevel + 1;

            LevelManagerZombiesMode ongoingLevel = Manager.Levels[CurrentLevel];


            if (PlayerPrefs.GetInt("ShowZombieTutorial", 1) == 1)
            {
                CurrentLevel = Manager.Data.CurrentLevel;

                ongoingLevel = Manager.Levels[CurrentLevel];

                if (ongoingLevel.CurrentLevel < ongoingLevel._Levels.Length)
                {
                    TotalRemaining = ongoingLevel._Levels[ongoingLevel.CurrentLevel]._RemainingEnemies;
                }
                else
                {
                    TotalRemaining = ongoingLevel._Levels[ongoingLevel.CurrentLevel-1]._RemainingEnemies;
                }
            }
            else
            {
                if (ongoingLevel.CurrentLevel < ongoingLevel._Levels.Length)
                {
                    TotalRemaining = ongoingLevel._Levels[ongoingLevel.CurrentLevel]._RemainingEnemies;
                }
                else
                {
                    TotalRemaining = ongoingLevel._Levels[ongoingLevel.CurrentLevel - 1]._RemainingEnemies;
                }
            }

            

            
            scoreToShow = Manager._totalScore + Manager.Data.gameData.PlayerCoins;

        }

        if (GameManager.instance)
        {
            if (CurrentLevel < 10 && PlayerPrefs.GetInt("ShowTutorial", 1) == 0)
            {
                LevelNumber.text = "Level 0" + CurrentLevel;
            }
            else if (PlayerPrefs.GetInt("ShowTutorial", 1) == 0)
            {
                LevelNumber.text = "Level " + CurrentLevel;
            }
            else
            {
                LevelNumber.text = "Tutorial";
            }
        }
        else if(GameManagerZombiesMode.instance)
        {

            int LvlNo = GameManagerZombiesMode.instance.Data.CurrentLevel + 1;

            if (PlayerPrefs.GetInt("ShowZombieTutorial", 1) == 1)
            {
                LevelNumber.text = "Tutorial Level";
            }
            else
            {
                if (CurrentLevel < 10)
                {
                    LevelNumber.text = "Level 0" + LvlNo;
                }
                else if (CurrentLevel >= 10)
                {
                    LevelNumber.text = "Level " + LvlNo;
                }
            }
        }
        else
        {
            if (CurrentLevel < 10 )
            {
                LevelNumber.text = "Level 0" + CurrentLevel;
            }
            else if (CurrentLevel >= 10)
            {
                LevelNumber.text = "Level " + CurrentLevel;
            }
        }
        

        InGameCoinsEarned.text = scoreToShow.ToString();
        TotalLevelEnemies.text = TotalRemaining.ToString();
    }
    public void SkipCutscene(string text)
    {
        StopAllCoroutines();
        QuestDescription.text = text;
        SKipCutsceneBtn.gameObject.SetActive(false);
        StartGameBtn.gameObject.SetActive(true);
    }
    public void EndCutscene()
    {
        CutsceneObj.SetActive(false);

        gameManager.PlayerCamera.gameObject.SetActive(true);

        gameManager.IPRing.gameObject.SetActive(true);

        GameplayHUD.SetActive(true);

    }
    public void SetCutsceneText(string text)
    {
        QuestDescription.text = "";
        GameplayHUD.SetActive(false);
        CutsceneObj.SetActive(true);
        gameManager.PlayerCamera.gameObject.SetActive(false);
        gameManager.IPRing.gameObject.SetActive(false);
        StartCoroutine(StartWriting(text));

    }

    IEnumerator StartWriting(string text)
    {
        for(int i=0;i<=text.Length;i++)
        {
            QuestDescription.text = text.Substring(0,i);

            if(i==text.Length-1)
            {
                SkipCutscene(text);
            }
            yield return new WaitForSeconds (0.07f);
        }
    }

    public IEnumerator ShowDefeatVictoryText(bool victory)
    {
        if(victory)
        {
            VictoryText.SetActive(true);
            yield return new WaitForSeconds(5f);
        }
        else
        {
            DefeatText.SetActive(true);

            yield return new WaitForSeconds(5f);
            EnableScreen(ScreenType.Defeat);
        }

    }


    public void EnableScreen(ScreenType type)
    {
       if(DefeatText!=null)
        {
            DefeatText.gameObject.SetActive(false);
        }
      
       if(VictoryText!=null)
        {
            VictoryText.gameObject.SetActive(false);
        }
       
     

        if(gameManager!=null)
        {
            gameManager.IPRing.gameObject.SetActive(false);
            gameManager.PlayerCamera.ScopeRect.SetActive(false);
        }
        else if(GameManagerPvp.instance != null)
        {
            GameManagerPvp.instance.IPRig.gameObject.SetActive(false);
            GameManagerPvp.instance.PlayerVcam.gameObject.SetActive(false);

            GameManagerPvp.instance.TutorialObj1.SetActive(false);
            GameManagerPvp.instance.TutorialObj2.SetActive(false);
        }
        
        GameplayHUD.SetActive(false);
        ControlFreak2.CFCursor.visible = true;
        ControlFreak2.CFCursor.lockState = CursorLockMode.None;


      
        if(ReviveScreen!=null)
        {
            ReviveScreen.SetActive(false);
        }

        if (PauseScreenObjects[0] != null)
        {

            PauseScreenObjects[0].SetActive(false);
        }

        if (PauseScreenObjects[1] != null)
        {
            PauseScreenObjects[1].SetActive(false);
        }

        if (PauseScreenObjects[2] != null)
        {
            PauseScreenObjects[2].SetActive(false);
        }
       

        switch (type)
        {
            case ScreenType.Sucess:
                PauseScreenObjects[0].SetActive(true);
              
                PauseScreen.transform.DOScale(1f, 0.125f).OnComplete(() => 
                {

                    if(gameManager)
                    {
                        TotalCoinText.text = gameManager.Data.gameData.PlayerCoins.ToString("n0");
                    }
                    else if(GameManagerPvp.instance)
                    {

                    }
                    else if (GameManagerWeaponsMode.instance)
                    {
                        TotalCoinText.text = GameManagerWeaponsMode.instance.Data.gameData.PlayerCoins.ToString("n0");
                        GameManagerWeaponsMode.instance.SetData();
                    }
                    else if (GameManagerZombiesMode.instance)
                    {
                        TotalCoinText.text = GameManagerZombiesMode.instance.Data.gameData.PlayerCoins.ToString("n0");
                        GameManagerZombiesMode.instance.SetData();
                        
                    }

                    // gameManager.EndLevel();
                    //StartCoroutine(gameManager.MissionCompleteUiCounter(Score,BonusText));


                });
                break;

            case ScreenType.Pause:

//#if !UNITY_EDITOR
                AdsManger_New.Instance.ShowInterstitial();
//#endif
                         
                PauseScreenObjects[1].SetActive(true);
              
                PauseScreen.transform.DOScale(1f, 0.125f).OnComplete(() => 
                { 
                    Time.timeScale = 0;
                    ControlFreak2.CFCursor.visible = true;
             
                    ControlFreak2.CFCursor.lockState = CursorLockMode.None;

                    if (gameManager)
                    {
                        PauseScreenCoins.text = gameManager.Data.gameData.PlayerCoins.ToString("n0");
                    }

                   if(GameManagerZombiesMode.instance)
                    {
                        PauseScreenCoins.text = GameManagerZombiesMode.instance.Data.gameData.PlayerCoins.ToString("n0");
                    }
                        

                    // TotalCoinText.text = gameManager.Data.PlayerCoins.ToString("n0");
                });
            
                break;

            case ScreenType.Defeat:

                
                PauseScreenObjects[2].SetActive(true);
                PauseScreen.transform.DOScale(1f, 0.125f).OnComplete(() => { Invoke("DelayedTimeScale", 2f); });
             


                break;

            case ScreenType.Revive:

                
                ReviveScreen.SetActive(true);
              
                PauseScreen.transform.DOScale(1f, 0.125f).OnComplete(() => 
                {
                   
                    
                    Invoke("DelayedTimeScale", 3f); 
                });
                break;
        }

    }

    public void DelayedTimeScale() 
    {
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        
        Time.timeScale = 1;
        //Debug.Log("Timescale at resume" + Time.timeScale );
        Settings.instance.TurnSFXOnOff();

        PauseScreen.transform.DOScale(0f, 0.125f);

        for(int i=0;i<PauseScreenObjects.Length;i++)
        {
            PauseScreenObjects[i].SetActive(false);
        }

   
        ControlFreak2.CFCursor.visible = false;
        ControlFreak2.CFCursor.lockState = CursorLockMode.Locked;

        if (gameManager != null)
        {
            Debug.Log("GameManager found");
            gameManager.mouseLock.enabled = true;
            gameManager.IPRing.gameObject.SetActive(true);
        }
        else if(GameManagerPvp.instance)
        {
            GameManagerPvp.instance.mouseLock.enabled = true;
            GameManagerPvp.instance.IPRig.gameObject.SetActive(true);

           if(InGameUiPVP.instance.FirstScreenShown)
            {
                GameManagerPvp.instance.TutorialObj2.SetActive(true);
            }
           else
            {
                GameManagerPvp.instance.TutorialObj1.SetActive(true);
            }

        }
        else if (GameManagerWeaponsMode.instance)
        {
            GameManagerWeaponsMode.instance.CF2Rig.SetActive(true);
            GameplayHUD.SetActive(true);
        }

        else if(GameManagerZombiesMode.instance)
        {
            GameManagerZombiesMode.instance.CF2Rig.SetActive(true);
        }
           
       
        // gameManager.scope.gameObject.SetActive(true);
        GameplayHUD.SetActive(true);
    }

    public void BackToMenu()
    {
        //ResumeGame();
        //SceneManager.LoadSceneAsync("MainMenu");

#if !UNITY_EDITOR
        AdsManger_New.Instance.ShowUnityAdAdmobWithAdBreak();
#endif

        LoadingBar.instance.LoadScene("MainMenu");
        InGameUi.instance = null;

    }

    public void SetBulletText(int number)
    {
        BulletText.text = "x" + number;
    }
    public void ScoreUpTextChange(int amount)
    {
      
        ScoreUpText.DOFade(1, 0f);
        RectTransform DefaultPos = ScoreUpText.rectTransform;
        ScoreUpText.gameObject.SetActive(true);
        ScoreUpText.text = "+"+amount;
        ScoreUpText.rectTransform.DOMoveY(ScoreUpText.rectTransform.position.y + 30, 1f).OnComplete(()=>
        {
            ScoreUpText.DOFade(0, 1f);
            ScoreUpText.rectTransform.position = DefaultPos.position;
        });
    }


 
    public void ShowSpecialAnimation(string Text)
    {

        // WE CAN PLAY AUDIO HERE

        HeadShotTextIngame.text = Text;
        HeadShotTextIngame.DOFade(1f, 0.001f);
        headShotImage.DOFade(1, 0.001f);
        headShotImage.gameObject.SetActive(true);

        headShotImage.DOFade(0, 2);
        HeadShotTextIngame.DOFade(0, 2);
    }

    public IEnumerator FinalCoinsCounter(int totalamount,int prevAmount)
    {
        //Debug.Log("Total Score in func" + totalamount);
        //Debug.Log("prev Score in func" + prevAmount);


        for (int i = 0; i <= totalamount; i++)
        {
             int newMauAmount = prevAmount + i;

            if(i>=totalamount)
            {
              
            }

            TotalCoinText.text = newMauAmount.ToString("n0");
             yield return new WaitForSeconds(0.0001f);

        }
    }
    public IEnumerator EndGameDisplay(int startingValue,int TotalScore, Text totalScoreText)
    {
      


        int prevCoins = 0;
        if (gameManager != null)
        {
            prevCoins = gameManager.Data.gameData.PlayerCoins - TotalScore;
            //Debug.Log("Prev Coins" + prevCoins);
        }
        else if(GameManagerPvp.instance)
        {
           prevCoins = GameManagerPvp.instance.PvPGameData.gameData.PlayerCoins - TotalScore;  // CHANGE THIS FOR PVP MODE
        }
        else if(GameManagerWeaponsMode.instance)
        {
            prevCoins = GameManagerWeaponsMode.instance.Data.gameData.PlayerCoins - TotalScore;
        }
        else if (GameManagerZombiesMode.instance)
        {
            TotalScore = GameManagerZombiesMode.instance.DoubleVal; //GameManagerZombiesMode.instance.AdReward;
            //Debug.Log("Total Score = " + TotalScore);
            prevCoins = GameManagerZombiesMode.instance.Data.gameData.PlayerCoins + TotalScore; 
            //Debug.Log("Prev Coins = " + prevCoins);
        }

        TotalCoinText.text = prevCoins.ToString("n0");

        
        for (int i= startingValue; i<= TotalScore; i += 5)
        {
            
            if (i >= TotalScore)
            {
                StopCoinCounter = true;
            }

            if (!StopCoinCounter)
            {
                if(totalScoreText!=null)
                {
                    totalScoreText.text = i.ToString();
                }

                if(AudSource!=null)
                {
                    AudSource.volume = 0.3f;
                    AudSource.pitch = 0.7f;
                    AudSource.PlayOneShot(CoinClip);
                }
              
            }
            else
            {
                if (totalScoreText != null)
                {
                    totalScoreText.text = TotalScore.ToString(); // ADD THE START EFFECT HERE
                }
                   
                if(CoinParticleObject!=null)
                {
                    CoinParticleObject.SetActive(true);
                }

                //if (NextLevelBtn != null)
                //{

                //    NextLevelBtn.SetActive(true);
                //}
                //Debug.Log("Total Score" + TotalScore);
                //Debug.Log("Prev Score" + prevCoins);

                StartCoroutine(FinalCoinsCounter(TotalScore, prevCoins));

              //  TotalCoinText.text = gameManager.Data.PlayerCoins.ToString("n0");

                break;
            }

          
              yield return new WaitForSeconds(0.0125f); 
            
        }
    }
   
}
