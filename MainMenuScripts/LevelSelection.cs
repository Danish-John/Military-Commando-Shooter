using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class LevelSelection : MonoBehaviour
{
    [SerializeField] private LevelData LvlData;
    [SerializeField] private LevelData PvpLvlData;
    [SerializeField] private LevelData WeaponsModeData;
    [SerializeField] private LevelData ZombieModeData;

    [SerializeField] private Button [] Markers;
    [SerializeField] private Button[] WeaponsModeMarkers;
    [SerializeField] private Button[] ZombieModeMarkers;

    [HideInInspector] public bool insInLevelSelection;
    public Sprite clearedImage;
    public GameObject CurrentLevelMarker;
   
    public GameObject LevelSelectionTutorial1;
    public Button PlayButton;
    public Button WeaponsModePlayButton;
    public Transform ScrollContent;
    public Transform WeaponsModeScrollContent;

    public Button ZombieModePlayButton;
    public Transform ZombieModeScrollContent;


    public GameObject backButton;
    public GameObject TytorialHand;
    public DOTweenAnimation NextButton;

    private string Scenename;
    public GameObject Snipershop;
    public GameObject ZombieShop;


    void Start()
    {
       LvlData.ReadNormalModeData();
       PvpLvlData.ReadPvpData();
       WeaponsModeData.ReadWeaponsModeData();
       ZombieModeData.ReadZombieModeData();
        


        if (PlayerPrefs.GetInt("ShowMainMenuTutorial", 1) == 1)
        {
            LevelSelectionTutorial1.SetActive(true);
            TytorialHand.SetActive(true);
        }
        else
        {
            NextButton.DORestart();
            LevelSelectionTutorial1.SetActive(false);
            TytorialHand.SetActive(false);
        }



        InitializeSniperModeMarkers();
        InitializeWeaponsModeMarkers();
        InitializeZombieModeMarkers();
    }





    void InitializeWeaponsModeMarkers()
    {
        int count=0;
        int textcount = 0;
        while (count < WeaponsModeMarkers.Length)
        {
           

            Button btn = WeaponsModeMarkers[count];

            if (count < WeaponsModeData.LevelsUnlocked)
            {

                WeaponsModeMarkers[count].image.sprite = clearedImage;
                WeaponsModeMarkers[count].GetComponent<Markers>().LockOrUnlock(false);
            }
            else
            {
                WeaponsModeMarkers[count].GetComponent<Markers>().LockOrUnlock(true);  /// MEANING LOCK EM UP
                btn.interactable = false;
            }

            WeaponsModeMarkers[count].onClick.AddListener(() =>
            {
                Scenename = "WeaponsMode";
                StartGame();
            });


           
            if (count == WeaponsModeData.LevelsUnlocked)
            {
               
                GameObject instance = Instantiate(CurrentLevelMarker, WeaponsModeScrollContent);

                instance.transform.position = WeaponsModeMarkers[count].transform.position;
                instance.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
                instance.SetActive(true);
                btn.gameObject.SetActive(false);
                textcount = count + 1;

              
                instance.GetComponent<Button>().onClick.AddListener(() =>
                {
                    Scenename = "WeaponsMode";

                    LoadWeaponSelectionLevel(textcount - 1);
                    StartGame();
                });


                WeaponsModePlayButton.onClick.AddListener(() =>
                {

                    Scenename = "WeaponsMode";

                    LoadWeaponSelectionLevel(textcount - 1);
                    StartGame();
                });

                if (count < 10)
                {
                    instance.GetComponentInChildren<Text>().text = "0" + textcount + "\nLvl";
                }
                else
                {
                    instance.GetComponentInChildren<Text>().text = textcount + "\nLvl";
                }
            }

            count++;
        }
    }

    void InitializeZombieModeMarkers()
    {
        int count = 0;
        int textcount = 0;
        
        
        while (count < ZombieModeMarkers.Length)
        {


            Button btn = ZombieModeMarkers[count];

            if (count < ZombieModeData.LevelsUnlocked)
            {

                ZombieModeMarkers[count].image.sprite = clearedImage;
                ZombieModeMarkers[count].GetComponent<Markers>().LockOrUnlock(false);
            }
            else
            {
                ZombieModeMarkers[count].GetComponent<Markers>().LockOrUnlock(true);  /// MEANING LOCK EM UP
                btn.interactable = false;
            }

            ZombieModeMarkers[count].onClick.AddListener(() =>
            {
                Scenename = "ZombieMode";
            });


            //if(count==LvlData.CurrentLevel && LvlData.LevelsUnlocked!=LvlData.data.Length-1)
            if (count == ZombieModeData.LevelsUnlocked)
            {

                GameObject instance = Instantiate(CurrentLevelMarker, ZombieModeScrollContent);

                //  instance.GetComponent<Button>().enabled = false;
                instance.transform.position = ZombieModeMarkers[count].transform.position;
                instance.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
                instance.SetActive(true);
                btn.gameObject.SetActive(false);
                textcount = count + 1;


                instance.GetComponent<Button>().onClick.AddListener(() =>
                {
                    Scenename = "ZombieMode";
                    ZombieShop.GetComponent<DOTweenAnimation>().DORestart();
                    NewShopZombie.instance.ShowGun(0);
                    //NewShopZombie.instance.UpdateUI();
                    
                    NewShopZombie.instance.GunDisplay.SetActive(true);
                    
                    LoadZombieSelectionLevel(textcount -1);
                });


                ZombieModePlayButton.onClick.AddListener(() =>
                {

                    Scenename = "ZombieMode";

                    LoadZombieSelectionLevel(textcount -1);
                });

                if (count < 10)
                {
                    instance.GetComponentInChildren<Text>().text = "0" + textcount + "\nLvl";
                }
                else
                {
                    instance.GetComponentInChildren<Text>().text = textcount + "\nLvl";
                }
            }

            count++;
        }
    }



    void InitializeSniperModeMarkers()
    {
        int count=0;
        int textcount = 0;
        while (count < Markers.Length)
        {
            Button btn = Markers[count];

            if (count < LvlData.LevelsUnlocked) 
            {

                Markers[count].image.sprite = clearedImage;
                Markers[count].GetComponent<Markers>().LockOrUnlock(false);
            }
            else
            {
                Markers[count].GetComponent<Markers>().LockOrUnlock(true);  /// MEANING LOCK EM UP
                btn.interactable = false;
            }


            Markers[count].onClick.AddListener(() =>
            {
                Scenename = "SniperMain";

           
            });

            //if(count==LvlData.CurrentLevel && LvlData.LevelsUnlocked!=LvlData.data.Length-1)
            if (count == LvlData.LevelsUnlocked)
            {

                GameObject instance = Instantiate(CurrentLevelMarker, ScrollContent);

                //  instance.GetComponent<Button>().enabled = false;
                instance.transform.position = Markers[count].transform.position;
                instance.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
                instance.SetActive(true);
                //Markers[count].GetComponent<Button>().enabled = false;
                btn.gameObject.SetActive(false);
                textcount = count + 1;

             

                instance.GetComponent<Button>().onClick.AddListener(() =>
                {
                    Scenename = "SniperMain";
                    Snipershop.GetComponent<DOTweenAnimation>().DORestart();

                    NewShop.instance.EnterShop();
                    NewShop.instance.ShowWeapon(0);
                    NewShop.instance.SetUi();
                    NewShop.instance.GunDisplay.SetActive(true);
                    
                    
                    LoadLevel(textcount - 1);
                });

                PlayButton.onClick.AddListener(() =>
                {
                    Scenename = "SniperMain";
                    LoadLevel(textcount - 1);
                });

                if (count < 10)
                {
                    instance.GetComponentInChildren<Text>().text = "0" + textcount + "\nLvl";
                }
                else
                {
                    instance.GetComponentInChildren<Text>().text = textcount + "\nLvl";
                }
            }



            count++;
        }

    }





    public void EnterLevelSelection()
    {
        MainMenu.instance.isInMainMenu = false;
        insInLevelSelection = true;
        if (FirebaseManager.Instance != null)
        {
            if(!LvlData.gameData.EnterLevelSelectionEvenLogged)
            {
                LvlData.gameData.EnterLevelSelectionEvenLogged = true;
                FirebaseManager.Instance.LogEvent("Entered_Level_Selection");
            }


            if (!LvlData.gameData.ExitMainMenuFirebaseEventLogged)
            {
                LvlData.gameData.ExitMainMenuFirebaseEventLogged = true;
                FirebaseManager.Instance.LogEvent("Exit_MainMenu");
            }

        }
            
    }

    public void LeaveLevelSelection()
    {
        MainMenu.instance.isInMainMenu = true;
        insInLevelSelection = false;
        if (FirebaseManager.Instance != null)
        {
            if (!LvlData.gameData.LeaveLevelSelectionEvenLogged)
            {
                LvlData.gameData.LeaveLevelSelectionEvenLogged = true;
                FirebaseManager.Instance.LogEvent("Exit_Level_Selection");
            }
        }
       
    }

    public void StartGame()
    {

#if !UNITY_EDITOR
        AdsManger_New.Instance.ShowUnityAdAdmobWithAdBreak();
#endif

        if (LoadingBar.instance != null)
        {
             LoadingBar.instance.LoadScene(Scenename);
        }
        else
        {
        #if UNITY_EDITOR
             SceneManager.LoadSceneAsync(Scenename);
        #endif
        }

        PlayerPrefs.SetInt("ShowMainMenuTutorial", 0);
        PlayerPrefs.SetInt("GunInZombieScene", 0);
        PlayerPrefs.SetInt("GunInSniperScene", 0);


        PlayerPrefs.SetInt("RewardedGun", 0);
        PlayerPrefs.SetInt("RewardedGunIndex", 0);


    }
    public void LoadLevel(int level)
    {
       
     LvlData.CurrentLevel = level;
           
    }

    public void LoadWeaponSelectionLevel(int level)
    {
        WeaponsModeData.CurrentLevel = level;

    }

    public void LoadZombieSelectionLevel(int level)
    {
        ZombieModeData.CurrentLevel = level;
        

    }


    void Update()
    {
        if (PlayerPrefs.GetInt("ShowMainMenuTutorial", 1) == 1)
        {
            backButton.SetActive(false);
        }
    }

}
