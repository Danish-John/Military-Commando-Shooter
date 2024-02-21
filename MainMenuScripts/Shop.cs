using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using DG.Tweening;
using Firebase.Analytics;

public class Shop : MonoBehaviour
{
    [SerializeField] private SliderMenu Slidermenu;
    [SerializeField] private Transform ScrollParent;
    [SerializeField] private GameObject MainSlotPrefab;
    [SerializeField] private LevelData Data;

    [SerializeField] private Slider Damage;
    [SerializeField] private Slider Magazine;
    [SerializeField] private Slider Stability;

    [SerializeField] private Text Gold;
    [SerializeField] private Text Cost;
    [SerializeField] private Button PurchaseBtn;
    [SerializeField] private Button SelectBtn;
    [SerializeField] private GameObject SelectedText;
                     public GameObject PurchaseAcknowledgment;
                     public GameObject Toast;
                     public Text PurchasedGunName;
                     public Image PurchasedGunImage;
                   
                     public RectTransform SlidesTransform;
                     public Text GunNameDisplay;
    [SerializeField] private GameObject[] WeaponModels;

    public GameObject GunDisplay;
                
    
    private Weapon [] guns;

    private Weapon CurrentGun;

    private Button [] weapnBtns;

    private bool inShop;

    public LevelSelection lvlSelection;
    
    private Dictionary <string, GameObject> WpnDictionary;

    public GameObject BackButton;

    public GameObject ShoptutObject;
    public GameObject ShopTutHandObject;

    public DOTweenAnimation PlayButton;
    public void DisplaySelectedWeponSlide()
    {
        int slelectedWeaponIndex = 0;
        foreach (KeyValuePair<string, GameObject> wpn in WpnDictionary)
        {
            
            if (Data.gameData.EquippedGunName!=wpn.Key)
            {
                slelectedWeaponIndex++;
            }
            else
            {
                break;
            }

        }


        float positionX = slelectedWeaponIndex * -350;
       

          SlidesTransform.anchoredPosition = new Vector2(positionX, 0f);
        Debug.Log("DisplaySelectedWeponSlide Called");
      //  SlidesTransform.DOMoveX(positionX, 0.125f);
    }

    
    void Start()
    {
        if (PlayerPrefs.GetInt("ShowMainMenuTutorial", 1) == 1)
        {
            ShopTutHandObject.SetActive(true);
            ShoptutObject.SetActive(true);

        }
        else
        {
            PlayButton.DORestart();
            ShopTutHandObject.SetActive(false);
            ShoptutObject.SetActive(false);
        }
        weapnBtns = new Button[WeaponModels.Length];

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
        }

        guns = Data.gameData.Guns;
        InitializeStore();
        PurchaseBtn.gameObject.SetActive(false);
        SelectBtn.gameObject.SetActive(false);
        Gold.text = Data.gameData.PlayerCoins.ToString("n0");

        //  Data.gameData.ReadData();

        AssignWeapon(Data.gameData.EquippedGunName);
    }

    private void FixedUpdate()
    {
        ShowWeapon(Slidermenu.CurrentIndex);

        if (PlayerPrefs.GetInt("ShowMainMenuTutorial", 1) == 1)
        {
            BackButton.SetActive(false);

        }

        if(CurrentGun.isUnlocked == true)
        { 
            PurchaseBtn.gameObject.SetActive(false);
        }

        // DisplaySelectedWeponSlide();
    }



    void InitializeStore()
    {
       // Slidermenu.SlidesInView = guns.Length;

        for(int i=0;i<guns.Length;i++)
        {
            GameObject instance = Instantiate(MainSlotPrefab);
            instance.transform.parent = ScrollParent;
            instance.name = guns[i].GunName;

           
            weapnBtns[i] = instance.GetComponent<Button>();
            // instance.GetComponentInChildren<Image>().sprite = guns[i].GunImage;

            instance.transform.GetChild(0).GetComponent<Image>().sprite= guns[i].GunImage;
            instance.transform.GetChild(2).GetComponent<Text>().text = guns[i].GunName;


            if (!guns[i].isUnlocked)
            {
                weapnBtns[i].interactable = false;
            }
            else
            {
                instance.transform.GetChild(1).gameObject.SetActive(false);
            }

            weapnBtns[i].onClick.AddListener(()=> 
            {
                if(lvlSelection.insInLevelSelection)
                {
                    lvlSelection.StartGame();
                }
                
            });

            instance.SetActive(true);
            Slidermenu.Slides.Add(instance);
            
        }
    }

    public void ShowWeapon(int index)
    {
        Debug.Log("ShowWeapon Called");
        CurrentGun      = guns[index];
        Damage.value    = CurrentGun.Power;
        Magazine.value  = CurrentGun.Magazine;
        Stability.value = CurrentGun.Stability;
        GunNameDisplay.text = CurrentGun.GunName;

 

        if (!CurrentGun.isUnlocked)
        {
            Cost.text = CurrentGun.Cost.ToString("n0");
        }
        else
        {
            // ADD THE CODE TO SELECT HERE
            Cost.text = "Purchased";
        }
        

        if(inShop)
        {
          
            AssignWeapon(CurrentGun.GunName);
        }
        else
        {
           
            AssignWeapon(Data.gameData.EquippedGunName);
        }
        

        if (CurrentGun.GunName!=Data.gameData.EquippedGunName)
        {
            SelectedText.SetActive(false);
            if (CurrentGun.isUnlocked)
            {
                PurchaseBtn.gameObject.SetActive(false);
                // SelectBtn.gameObject.SetActive(true);

                SelectWeapon();
            }
            else if (!CurrentGun.isUnlocked)
            {
                PurchaseBtn.gameObject.SetActive(true);
                SelectBtn.gameObject.SetActive(false);
            }
        }
        else
        {
            PurchaseBtn.gameObject.SetActive(false);
            SelectBtn.gameObject.SetActive(false);
            SelectedText.SetActive(true);

        }
      
       
    }

    public void PurchaseWeapon()
    {
        if(Data.gameData.PlayerCoins >= CurrentGun.Cost)
        {
            CurrentGun.isUnlocked = true;
            Data.gameData.EquippedGunName = CurrentGun.GunName;
            Data.gameData.PlayerCoins -= CurrentGun.Cost;
            Gold.text = Data.gameData.PlayerCoins.ToString("n0");
            //weapnBtns[Slidermenu.CurrentIndex].interactable = true;
            //weapnBtns[Slidermenu.CurrentIndex].transform.GetChild(1).gameObject.SetActive(false);

            PurchasedGunName.text = CurrentGun.GunName;
            PurchasedGunImage.sprite = CurrentGun.GunImage;
            PurchaseAcknowledgment.SetActive(true);
            SaveGame();
            
            GunDisplay.SetActive(false);
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

        if(FirebaseManager.Instance!=null)
        {
            FirebaseManager.Instance.LogEvent(CurrentGun.GunName + "_Gun_Selected");
        }
       

    }

    private void AssignWeapon(string WpnName)
    {
        WpnDictionary[WpnName].SetActive(true);
        foreach(KeyValuePair<string,GameObject> wpn in WpnDictionary)
        {
            if(wpn.Key!=WpnName)
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
            if(!Data.gameData.LeaveWeaponSelectionEventLogged)
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
