using UnityEngine;
using UnityEngine.UI;

public class NewShopZombie : MonoBehaviour
{

    public GameObject GunButtons;
    public Button PurchaseButton;
    public GameObject Toast;
    public GameObject PurchaseAcknowledgement;
    public Slider Magzine;
    public Slider Damage;
    public Slider Stability;
    public Button ClaimBtn;
    public Button PlayBtn;
    public Sprite UnlockedBG;
    public Sprite LockedBG;
    public Text PurchasedGunName;
    public Image PurchasedGunImage;
    public GameObject GunDisplay;
    public GameObject ShopTutHandObject;
    public GameObject ShoptutObject;



    public static NewShopZombie instance;
    public ZombieGunsBluePrints[] ZombieGunDetails;
    

    int ZombieGunIndex;
    public GameObject[] ZombieGunModels;
    
    public Text GunNameTxt;


    void Start()
    {
        instance = this;


        if (PlayerPrefs.GetInt("ShowMainMenuTutorial", 1) == 1)
        {
            ShopTutHandObject.SetActive(true);
            ShoptutObject.SetActive(true);

        }
        else
        {
            ShopTutHandObject.SetActive(false);
            ShoptutObject.SetActive(false);
        }



        foreach (ZombieGunsBluePrints zombiegun in ZombieGunDetails)
        {
            if (zombiegun.Price == 0)
            {
                zombiegun.isUnlocked = true;
            }
            else
            {
                zombiegun.isUnlocked = PlayerPrefs.GetInt(zombiegun.Name, 0) == 0 ? false : true;
            }
        }

        ZombieGunIndex = PlayerPrefs.GetInt("SelectedZombieGunIndex", 0);

        foreach (GameObject ZombieGun in ZombieGunModels)
            ZombieGun.SetActive(false);

        ZombieGunModels[ZombieGunIndex].SetActive(true);

        UpdateUI();
        
        ClaimBtn.onClick.AddListener(() =>
        {

//#if !UNITY_EDITOR
            AdsManger_New.Instance.Show_Rewarded_Ads_Priority("ClaimZombieGun");
//#endif
        });


    }



    public void ClaimZombieGun()
    {
        ZombieGunsBluePrints G = ZombieGunDetails[ZombieGunIndex];

        PlayerPrefs.SetInt(G.Name, 1);
        PlayerPrefs.SetInt("SelectedZombieGunIndex", ZombieGunIndex);
        G.isUnlocked = true;
        
        
        PurchaseAcknowledgement.SetActive(true);
        PurchasedGunName.text = ZombieGunDetails[ZombieGunIndex].Name;
        PurchasedGunImage.sprite = GunButtons.transform.GetChild(ZombieGunIndex).gameObject.transform.GetChild(0).GetComponent<Image>().sprite;
        GunDisplay.SetActive(false);

       

#if !UNITY_EDITOR
        
        FirebaseManager.Instance.LogEvent("Shop_Z_AD_Gun_" + ZombieGunIndex);
        
#endif

        ShowGun(ZombieGunIndex);



    }



    public void UnlockZombieGun()
    {
        ZombieGunsBluePrints G = ZombieGunDetails[ZombieGunIndex];

        if (G.Price <= PlayerPrefs.GetInt("PlayerCoins"))
        {
            PlayerPrefs.SetInt(G.Name, 1);
            PlayerPrefs.SetInt("SelectedZombieGunIndex", ZombieGunIndex);
            G.isUnlocked = true;
            PlayerPrefs.SetInt("PlayerCoins", PlayerPrefs.GetInt("PlayerCoins", 0) - G.Price);
            PurchaseAcknowledgement.SetActive(true);
            PurchasedGunName.text = ZombieGunDetails[ZombieGunIndex].Name;
            PurchasedGunImage.sprite = GunButtons.transform.GetChild(ZombieGunIndex).GetComponent<Image>().sprite;
            GunDisplay.SetActive(false);



#if !UNITY_EDITOR
            FirebaseManager.Instance.LogEvent("Shop_Z_Pr_Gun_" + ZombieGunIndex);
#endif


            UpdateUI();
        }
        else
        {
            Toast.SetActive(true);
        }
    }


    public void ShowGun(int index)
    {

        for (int i=0; i<ZombieGunDetails.Length; i++)
        {
            ZombieGunModels[i].SetActive(false);
            GunButtons.transform.GetChild(i).GetComponent<Image>().sprite = LockedBG;
        }

        GunButtons.transform.GetChild(index).GetComponent<Image>().sprite = UnlockedBG;

        ZombieGunModels[index].SetActive(true);
        ZombieGunIndex = index;
        GunNameTxt.text = ZombieGunDetails[index].Name;
        Magzine.value = ZombieGunDetails[index].Mag;
        Damage.value = ZombieGunDetails[index].Damage;
        Stability.value = ZombieGunDetails[index].Stability;
        //UpdateUI();



        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


        ZombieGunsBluePrints G = ZombieGunDetails[index];
        if (G.isUnlocked) //&& PlayerPrefs.GetInt(G.Name) == 1
        {
            PurchaseButton.gameObject.SetActive(false);
            ClaimBtn.gameObject.SetActive(false);
            PlayBtn.gameObject.SetActive(true);
            GunButtons.transform.GetChild(index).gameObject.transform.GetChild(1).gameObject.SetActive(false);                   // Turn-Off lock image 

        }
        else
        {
            PurchaseButton.gameObject.SetActive(true);
            ClaimBtn.gameObject.SetActive(true);
            PlayBtn.gameObject.SetActive(false);

            GunButtons.transform.GetChild(index).gameObject.transform.GetChild(1).gameObject.SetActive(true);                    // Turn-On lock image
          
            PurchaseButton.transform.GetChild(1).gameObject.GetComponent<Text>().text = G.Price.ToString();
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////




        if (index == 0)
        {
            PlayerPrefs.SetInt("SelectedZombieGunIndex", ZombieGunIndex);
        }

        
        if (!G.isUnlocked)
            return;
        PlayerPrefs.SetInt("SelectedZombieGunIndex", ZombieGunIndex);

       
    }


   


    public void UpdateUI()
    {
        for (int j=0; j < ZombieGunDetails.Length; j++)
        {
            ZombieGunsBluePrints G = ZombieGunDetails[j];
            if (G.isUnlocked) //&& PlayerPrefs.GetInt(G.Name) == 1
            {
                PurchaseButton.gameObject.SetActive(false);
                ClaimBtn.gameObject.SetActive(false);
                PlayBtn.gameObject.SetActive(true);
                GunButtons.transform.GetChild(j).gameObject.transform.GetChild(1).gameObject.SetActive(false);                   // Turn-Off lock image 
               
            }
            else
            {
                PurchaseButton.gameObject.SetActive(true);
                ClaimBtn.gameObject.SetActive(true);
                PlayBtn.gameObject.SetActive(false);

                GunButtons.transform.GetChild(j).gameObject.transform.GetChild(1).gameObject.SetActive(true);                    // Turn-On lock image
              //GunButtons.transform.GetChild(ZombieGunIndex).GetComponent<Image>().sprite = LockedBG;

                PurchaseButton.transform.GetChild(1).gameObject.GetComponent<Text>().text = G.Price.ToString();
            }
        }
        
    }



    public void ShowADWithAdBreak()
    {
#if !UNITY_EDITOR
        AdsManger_New.Instance.ShowUnityAdAdmobWithAdBreak();
#endif
    }

}
