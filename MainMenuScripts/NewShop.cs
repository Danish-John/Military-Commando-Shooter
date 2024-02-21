using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using DG.Tweening;
using Firebase.Analytics;

public class NewShop : MonoBehaviour
{
    //[SerializeField] private SliderMenu Slidermenu;
    [SerializeField] private Transform ScrollParent;  //Content (Parent Of Gun Buttons)
    [SerializeField] private GameObject MainSlotPrefab;
    [SerializeField] private LevelData Data;

    [SerializeField] private Slider Damage;
    [SerializeField] private Slider Magazine;
    [SerializeField] private Slider Stability;

    [SerializeField] private Text Gold;
    [SerializeField] private Text Cost;
    [SerializeField] private Button PurchaseBtn;

   
    public GameObject PurchaseAcknowledgment;
    public GameObject Toast;
    public Text PurchasedGunName;
    public Image PurchasedGunImage;

    
    public Text GunNameDisplay;
    [SerializeField] private GameObject[] WeaponModels;

    public GameObject GunDisplay;


    private Weapon[] guns;

    private Weapon CurrentGun;

    private bool inShop;

    public LevelSelection lvlSelection;

    private Dictionary<string, GameObject> WpnDictionary;

    public GameObject BackButton;

    public GameObject ShoptutObject;
    public GameObject ShopTutHandObject;

    public DOTweenAnimation PlayButton;

    public Button ClaimBtn;

    ///////////////////////////////////////////////////////////////////////////////// <summary>


    [HideInInspector] public int GunNo;
    public static NewShop instance;

    public Sprite UnlockedBG;
    public Sprite LockedBG;


    /// /////////////////////////////////////////////////////////////////////////////</summary>




    public void SetUi()
    {
        for (int i = 0; i < WeaponModels.Length; i++)
        {

            if (Data.gameData.Guns[i].isUnlocked == true)
            {
                ScrollParent.transform.GetChild(i).gameObject.transform.GetChild(1).gameObject.SetActive(false); // Off Lock image
                //ScrollParent.transform.GetChild(i).GetComponent<Image>().sprite = UnlockedBG;
            }

            else if (Data.gameData.Guns[i].isUnlocked == false)
            {
                ScrollParent.transform.GetChild(i).gameObject.transform.GetChild(1).gameObject.SetActive(true); // Off Lock image
                //ScrollParent.transform.GetChild(i).GetComponent<Image>().sprite = LockedBG;
            }


        }


    }

    public void DisplaySelectedWeponSlide()
    {
        int slelectedWeaponIndex = 0;
        foreach (KeyValuePair<string, GameObject> wpn in WpnDictionary)
        {

            if (Data.gameData.EquippedGunName != wpn.Key)
            {
                slelectedWeaponIndex++;
            }
            else
            {
                break;
            }

        }

        //  SlidesTransform.DOMoveX(positionX, 0.125f);
    }


    void Start()
    {
        instance = this;

        //if (PlayerPrefs.GetInt("ShowMainMenuTutorial", 1) == 1)
        //{
        //    ShopTutHandObject.SetActive(true);
        //    ShoptutObject.SetActive(true);

        //}
        //else
        //{
        //    PlayButton.DORestart();
        //    ShopTutHandObject.SetActive(false);
        //    ShoptutObject.SetActive(false);
        //}
       

        Cursor.lockState = CursorLockMode.None;

        Cursor.visible = true;

        if (LoadingBar.instance != null)
        {
            LoadingBar.instance.MainObj.SetActive(false);
        }



        WpnDictionary = new Dictionary<string, GameObject>();

        for (int i = 0; i < WeaponModels.Length; i++)
        {
            WpnDictionary.Add(Data.gameData.Guns[i].GunName, WeaponModels[i]);
            //if(Data.gameData.Guns[i].isUnlocked == true)
            //{
            //    ScrollParent.transform.GetChild(i).gameObject.transform.GetChild(1).gameObject.SetActive(false); // Off Lock image
            //}
        }
        SetUi();

        

        guns = Data.gameData.Guns;
      
        PurchaseBtn.gameObject.SetActive(false);
       
        Gold.text = Data.gameData.PlayerCoins.ToString("n0");

        //  Data.gameData.ReadData();

        AssignWeapon(Data.gameData.EquippedGunName);

        ClaimBtn.onClick.AddListener(() =>
        {
           
//#if !UNITY_EDITOR
            AdsManger_New.Instance.Show_Rewarded_Ads_Priority("ClaimSniperGun");
//#endif
        });



    }

    private void FixedUpdate()
    {

        if (PlayerPrefs.GetInt("ShowMainMenuTutorial", 1) == 1)
        {
            BackButton.SetActive(false);

        }
        
    }





    public void ClaimSniperGun()
    {
        
        PurchaseBtn.gameObject.SetActive(false);
        ClaimBtn.gameObject.SetActive(false);
        PlayButton.gameObject.SetActive(true);

        CurrentGun = guns[GunNo];
        CurrentGun.isUnlocked = true;

        Data.gameData.EquippedGunName = CurrentGun.GunName;
        
        PurchasedGunName.text = CurrentGun.GunName;
        PurchasedGunImage.sprite = CurrentGun.GunImage;
        PurchaseAcknowledgment.SetActive(true);

        SetUi();
        SaveGame();

        GunDisplay.SetActive(false);


#if !UNITY_EDITOR
        FirebaseManager.Instance.LogEvent("Shop_S_AD_Gun_" + GunNo+1);
#endif

    }



    public void ShowWeapon(int index)
    {
        GunNo = index;
        CurrentGun = guns[index];
        Damage.value = CurrentGun.Power;
        Magazine.value = CurrentGun.Magazine;
        Stability.value = CurrentGun.Stability;
        GunNameDisplay.text = CurrentGun.GunName;



        for (int i = 0; i < WeaponModels.Length; i++)
        {
            ScrollParent.transform.GetChild(i).GetComponent<Image>().sprite = LockedBG;
            WeaponModels[i].SetActive(false);
        }

        ScrollParent.transform.GetChild(index).GetComponent<Image>().sprite = UnlockedBG;
        WeaponModels[index].SetActive(true);

        

        if (!CurrentGun.isUnlocked)
        {
            Cost.text = CurrentGun.Cost.ToString("n0");
        }
        else
        {
            // ADD THE CODE TO SELECT HERE
            Cost.text = "Purchased";
        }


        if (inShop)
        {

            AssignWeapon(CurrentGun.GunName);
        }
        else
        {

            AssignWeapon(Data.gameData.EquippedGunName);
        }


        if (CurrentGun.GunName != Data.gameData.EquippedGunName)
        {
            
            if (CurrentGun.isUnlocked)
            {
                PurchaseBtn.gameObject.SetActive(false);
                ClaimBtn.gameObject.SetActive(false);
                PlayButton.gameObject.SetActive(true);

                SelectWeapon();
            }
            else if (!CurrentGun.isUnlocked)
            {
                PurchaseBtn.gameObject.SetActive(true);
                PlayButton.gameObject.SetActive(false);
                ClaimBtn.gameObject.SetActive(true);
            }
        }
        else
        {
            PurchaseBtn.gameObject.SetActive(false);
            PlayButton.gameObject.SetActive(true);
            ClaimBtn.gameObject.SetActive(false);


        }
    }

    public void PurchaseWeapon()
    {
        if (Data.gameData.PlayerCoins >= CurrentGun.Cost)
        {
            PurchaseBtn.gameObject.SetActive(false);
            CurrentGun.isUnlocked = true;
            
            Data.gameData.EquippedGunName = CurrentGun.GunName;
            Data.gameData.PlayerCoins -= CurrentGun.Cost;
            Gold.text = Data.gameData.PlayerCoins.ToString("n0");
            
            PurchasedGunName.text = CurrentGun.GunName;
            PurchasedGunImage.sprite = CurrentGun.GunImage;
            PurchaseAcknowledgment.SetActive(true);
            SetUi();
            SaveGame();

            GunDisplay.SetActive(false);


#if !UNITY_EDITOR
            FirebaseManager.Instance.LogEvent("Shop_S_Pr_Gun_" + GunNo+1);
#endif

        }
        else
        {
            Toast.SetActive(true);
        }
    }



    public void SelectWeapon()
    {
        SaveGame();
        Data.gameData.EquippedGunName = CurrentGun.GunName;
        //PlayerPrefs.GetString("CurrentWeapon", "CheyTacM200_2");
        AssignWeapon(Data.gameData.EquippedGunName);

        if (FirebaseManager.Instance != null)
        {
            FirebaseManager.Instance.LogEvent(CurrentGun.GunName + "_Gun_Selected");
        }


    }

    private void AssignWeapon(string WpnName)
    {
        WpnDictionary[WpnName].SetActive(true);
        foreach (KeyValuePair<string, GameObject> wpn in WpnDictionary)
        {
            if (wpn.Key != WpnName)
            {
                wpn.Value.SetActive(false);
            }

        }
    }

    public void LeaveShop()
    {
        inShop = false;
        MainMenu.instance.isInMainMenu = true;

        if (FirebaseManager.Instance != null)
        {
            if (!Data.gameData.LeaveWeaponSelectionEventLogged)
            {
                Data.gameData.LeaveWeaponSelectionEventLogged = true;
                FirebaseManager.Instance.LogEvent("Left_WeaponSelection");
            }

        }

    }

    public void EnterShop()
    {
        inShop = true;
        MainMenu.instance.isInMainMenu = false;

        if (FirebaseManager.Instance != null)
        {
            if (!Data.gameData.EnterWeaponSelectionEventLogged)
            {
                Data.gameData.EnterWeaponSelectionEventLogged = true;
                FirebaseManager.Instance.LogEvent("Entered_WeaponSelection");
            }

            if (!Data.gameData.ExitMainMenuFirebaseEventLogged)
            {
                Data.gameData.ExitMainMenuFirebaseEventLogged = true;
                FirebaseManager.Instance.LogEvent("Exit_MainMenu");
            }

        }

    }
    public void SaveGame()
    {
        Data.gameData.SaveGameData();
    }

    public void ShowAdWithAdBreak()
    {
#if !UNITY_EDITOR
        AdsManger_New.Instance.ShowUnityAdAdmobWithAdBreak();
#endif
    }


}
