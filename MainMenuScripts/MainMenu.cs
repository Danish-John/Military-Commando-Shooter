using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Audio;
using DG.Tweening;
using UnityEngine.SceneManagement;
using Hellmade.Net;

public class MainMenu : MonoBehaviour
{
    public Button SaveButton;

    public Button SFXButton;

    public Button BGMButton;

    public Image SFXImage;

    public Image BGMImage;

    public Button RewardAdButton;

    public Button ShopBackBtn;

    public Button LevelSelectionBackButton;

    [HideInInspector] public bool isInMainMenu;

    public static MainMenu instance;

    public Button RateusCloseButton;

    public Button SniperModeButton;

    public Button TargetModeButton;

    public Button WeaponsModeButton;

    public Button ZombieModeButton;

    public GameObject highlightObjectTargetmode;

    public GameObject highlightObjectWeaponmode;

    public GameObject highlightObjectZombiemode;

    public GameObject UnlockAcknowledgment;

    public GameObject RewardAcknowledgment;

    public GameObject MainMenuTutorial;

    public GameObject ModeSelectionTutorial1;

    public GameObject ModeSelectionTutorial2;

    public GameObject MainMenuTutorialhandObject;

    public GameObject ModeSelectionTutorial1HandObj;

    public GameObject ModeSelectionTutorial2HandObj;

    public GameObject ModeSelectionbackButton;

    public GameObject Exitbutton;

    public LevelData Data;

    [SerializeField] private Text Gold;

    public string modeName;

   

    public DOTweenAnimation PlayButtonMainMenu;

    public DOTweenAnimation ModeSelectionNextButton;
    
    public GameObject Button1, Button2;
   
    private void Awake()
    {
        Time.timeScale = 1;
        instance = this;

        if (FirebaseManager.Instance!=null)
        {
            if(!Data.gameData.EnterMainMenuFirebaseEventLogged)
            {
            #if !UNITY_EDITOR
                AdsManger_New.Instance.ShowInterstitial();
            #endif

                Data.gameData.EnterMainMenuFirebaseEventLogged = true;
                FirebaseManager.Instance.LogEvent("Entered_MainMenu");
            }
           
        }
        if(AdsManger_New.Instance!=null)
        {
           
            RewardAdButton.onClick.AddListener(() =>
            {
              //  RewardAdButton.gameObject.SetActive(false);
//#if !UNITY_EDITOR
        AdsManger_New.Instance.Show_Rewarded_Ads_Priority("simpleReward");
//#endif
            });
        }
        
    }



    public void ChangeModeSelectionTutorial()
    {
       if (PlayerPrefs.GetInt("ShowMainMenuTutorial", 1) == 1)
        {
            ModeSelectionTutorial1.SetActive(false);
            ModeSelectionTutorial2.SetActive(true);

            MainMenuTutorialhandObject.SetActive(false);
            ModeSelectionTutorial1HandObj.SetActive(false);
            ModeSelectionTutorial2HandObj.SetActive(true);
        }
     
    }
    private void Start()
    {
        if (LoadingBar.NewGame == true)
        {
            LoadingBar.NewGame = false;
            AdsManger_New.Instance.ShowInterstitial();
            //AdsManger_New.Instance.ShowUnityAdAdmobWithAdBreak();
        }

        
        
        if (PlayerPrefs.GetInt("ShowMainMenuTutorial", 1) == 1)
        {
            MainMenuTutorial.SetActive(true);
            ModeSelectionTutorial1.SetActive(true);
            ModeSelectionTutorial2.SetActive(false);
            MainMenuTutorialhandObject.SetActive(true);
            ModeSelectionTutorial1HandObj.SetActive(true);
            ModeSelectionTutorial2HandObj.SetActive(false);

          

        }
        else
        {
            MainMenuTutorial.SetActive(false);
            ModeSelectionTutorial1.SetActive(false);
            ModeSelectionTutorial2.SetActive(false);
            MainMenuTutorialhandObject.SetActive(false);
            ModeSelectionTutorial1HandObj.SetActive(false);
            ModeSelectionTutorial2HandObj.SetActive(false);

            PlayButtonMainMenu.DORestart();
            ModeSelectionNextButton.DORestart();

        }


#if !UNITY_EDITOR
        AdsManger_New.Instance.ShowSmartBannerTopMid();
#endif
        if (Settings.instance!=null)
        {

            SetButtonSprites();

            SaveButton.onClick.AddListener(() =>
            {
                Settings.instance.SaveSettings();
            });
            SFXButton.onClick.AddListener(() => 
            {
                Settings.instance.TurnSFXOnOff();
                SFXImage.sprite = Settings.instance._SFXSpriteToUse;
            });

            BGMButton.onClick.AddListener(() =>
            {
                Settings.instance.TurnBGMOnOff();
                BGMImage.sprite = Settings.instance._BGMSpriteToUse;
            });
        }

        modeName = "Zombie";

        InGameUi.instance = null;
    }



    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !isInMainMenu)
        {
            ShopBackBtn.onClick.Invoke();
            LevelSelectionBackButton.onClick.Invoke();
        }

        if (PlayerPrefs.GetInt("ShowMainMenuTutorial", 1) == 1)
        {
            ModeSelectionbackButton.SetActive(false);
            Exitbutton.SetActive(false);
        }
    }

    public void LogModeSelection(bool hasEntered)
    {
        if(hasEntered)
        {

            if (FirebaseManager.Instance != null)
            {
                if (!Data.gameData.EnterModeSelectionEventLogged)
                {
                    Data.gameData.EnterModeSelectionEventLogged = true;
                    FirebaseManager.Instance.LogEvent("Entered_ModeSelection");
                }

            }
        }
        else
        {

            if (FirebaseManager.Instance != null)
            {
                if (!Data.gameData.ExitModeSelectionEventLogged)
                {
                    Data.gameData.ExitModeSelectionEventLogged = true;
                    FirebaseManager.Instance.LogEvent("Exit_ModeSelection");
                }

            }
        }



        if (!Data.gameData.ExitMainMenuFirebaseEventLogged)
        {
            Data.gameData.ExitMainMenuFirebaseEventLogged = true;
            FirebaseManager.Instance.LogEvent("Exit_MainMenu");
        }
    }

    public void ModeSelection(string ModeName)
    {
    
        //modeName = ModeName;
        
        if (ModeName == "Target")
        {
            if (Data.gameData.CheckTargetModeUnlock())
            {
                highlightObjectTargetmode.SetActive(true);
                modeName = ModeName;
            }
            else
            {
                UnlockAcknowledgment.SetActive(true);
                highlightObjectTargetmode.SetActive(false);
            }
        }
        else if(ModeName == "Weapons")
        {
           // if (Data.gameData.CheckTargetModeUnlock())
           // {
                highlightObjectWeaponmode.SetActive(true);
                modeName = ModeName;
         //   }
          //  else
           // {
            //    UnlockAcknowledgment.SetActive(true);
            //    highlightObjectTargetmode.SetActive(false);
            //}
        }
        else if (ModeName == "Zombie")
        {
            // if (Data.gameData.CheckTargetModeUnlock())
            // {
            highlightObjectZombiemode.SetActive(true);
            modeName = ModeName;
            //   }
            //  else
            // {
            //    UnlockAcknowledgment.SetActive(true);
            //    highlightObjectTargetmode.SetActive(false);
            //}
        }


        else
        {
            modeName = ModeName;
        }
        
    }

    public void ChangeScreen()
    {


        if (PlayerPrefs.GetInt("ShowMainMenuTutorial", 1) != 1)  
        {

#if !UNITY_EDITOR
        AdsManger_New.Instance.ShowUnityAdAdmobWithAdBreak();
#endif





            //This ad will not play when user will be entering Mode selection panel for the very first time. 
            //After first time, it'll play every time while clicking Mode Selection's Next-btn.

        }

        if (modeName == "Sniper")
        {
            SniperModeButton.onClick.Invoke();
        
        }
        else if (modeName == "Target")
        {
            TargetModeButton.onClick.Invoke();
          
        }

        else if (modeName == "Weapons")
        {
            WeaponsModeButton.onClick.Invoke();
        }

        else if (modeName == "Zombie")
        {
            ZombieModeButton.onClick.Invoke();
        }
    }

        public void SetButtonSprites()
    {

        if (Settings.instance.isBGMOn)
        {

            BGMImage.sprite = Settings.instance.OnoffSprites[0];

        }
        else
        {

            BGMImage.sprite = Settings.instance.OnoffSprites[1];
        }

        if (Settings.instance.isSFXOn)
        {

            SFXImage.sprite = Settings.instance.OnoffSprites[0];
        }
        else
        {
            SFXImage.sprite = Settings.instance.OnoffSprites[1];
        }
        Settings.instance.BGMMixer.SetFloat("volume", Settings.instance.bgmMixerValue);
        Settings.instance.SFXMixer.SetFloat("volume", Settings.instance.sfxMixerValue);
    }
    public void QuitGame()
    {
        Application.Quit();
    }

   

    public void RateValue(float val)
    {
        if(val>=4)
        {
            Application.OpenURL("https://play.google.com/store/apps/details?id=com.xtreme.game.military.commando.shooting.enemy.free&hl=en&gl=US");
            RateusCloseButton.onClick.Invoke();
        }
        if(val>=1)
        {
           
        }
    }

    public void GrantAdReward()
    {
        
        Data.gameData.PlayerCoins += 1000;
        Data.gameData.SaveGameData();
        Gold.text = Data.gameData.PlayerCoins.ToString("n0");

#if !UNITY_EDITOR
        FirebaseManager.Instance.LogEvent("Rew_MainMenu");
#endif
    }


    public void ShowInterStitialAd()
    {
#if !UNITY_EDITOR
            AdsManger_New.Instance.ShowInterstitial();
#endif
    }
    


    public void ExitCalled()
    {
        this.gameObject.GetComponent<ButtonSwapping>().enabled = false;
        Button1.gameObject.SetActive(false);
        Button2.gameObject.SetActive(false);

        //Button1.GetComponent<Image>().enabled = false;
        //Button2.GetComponent<Image>().enabled = false;

    }

    public void ExitCancelled()
    {
        this.gameObject.GetComponent<ButtonSwapping>().enabled = true;
        Button1.gameObject.SetActive(true);
        Button2.gameObject.SetActive(false);

        Button1.GetComponent<Image>().enabled = true;
        Button2.GetComponent<Image>().enabled = true;


    }


}
