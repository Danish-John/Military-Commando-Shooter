using UnityEngine;
using UnityEngine.UI;

public class ShopZombie : MonoBehaviour
{
   
    public Button PurchaseButton;
    public GameObject Toast;
    public GameObject PurchaseAcknowledgement;
    public Scrollbar HorizontalRect;
    float LiveRectValue;

    public ZombieGunsBluePrints[] ZombieGunDetails;
    public static ShopZombie instance;

    int ZombieGunIndex;
    public GameObject[] ZombieGunModels;
    public GameObject[] ZombieGunSlides;
    public Text GunNameTxt;


    void Start()
    {

        foreach(ZombieGunsBluePrints zombiegun in ZombieGunDetails)
        {
            if (zombiegun.Price == 0)
            {
                zombiegun.isUnlocked = true;
            }
            else
            {
                zombiegun.isUnlocked = PlayerPrefs.GetInt(zombiegun.Name, 0) == 0 ? false: true;
            }
        }

        ZombieGunIndex = PlayerPrefs.GetInt("SelectedZombieGunIndex", 0);

        foreach (GameObject ZombieGun in ZombieGunModels)
            ZombieGun.SetActive(false);

        ZombieGunModels[ZombieGunIndex].SetActive(true);

        LiveRectValue = HorizontalRect.value;
       
    }

    private void Update()
    {
        UpdateUI();
    }


    public void CheckValue()
    {
        LiveRectValue = HorizontalRect.value;
        
        ShowGun();
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
        }
        else
        {
            Toast.SetActive(true);
        }
    }


    public void ShowGun()
    {
        if (LiveRectValue <= 0.33)
        {
            ZombieGunModels[0].SetActive(true);
            ZombieGunModels[1].SetActive(false);
            ZombieGunModels[2].SetActive(false);
            GunNameTxt.text = "AKM Tiger Claw";

            ZombieGunIndex = 0;
            PlayerPrefs.SetInt("SelectedZombieGunIndex", ZombieGunIndex);

        }
        else if (LiveRectValue >= 0.34 && LiveRectValue <= 0.66)
        {
            ZombieGunModels[0].SetActive(false);
            ZombieGunModels[1].SetActive(true);
            ZombieGunModels[2].SetActive(false);
            GunNameTxt.text = "AKM Nacromencer";

            ZombieGunIndex = 1;
            ZombieGunsBluePrints G = ZombieGunDetails[ZombieGunIndex];
            if (!G.isUnlocked)
                return;

            
            PlayerPrefs.SetInt("SelectedZombieGunIndex", ZombieGunIndex);
        }
        else if (LiveRectValue >= 0.67)
        {
            ZombieGunModels[0].SetActive(false);
            ZombieGunModels[1].SetActive(false);
            ZombieGunModels[2].SetActive(true);
            GunNameTxt.text = "Demon Hunter";

            ZombieGunIndex = 2;

            ZombieGunsBluePrints G = ZombieGunDetails[ZombieGunIndex];
            if (!G.isUnlocked)
                return;

            PlayerPrefs.SetInt("SelectedZombieGunIndex", ZombieGunIndex);
        }
    }


    private void UpdateUI()
    {
        ZombieGunsBluePrints G = ZombieGunDetails[ZombieGunIndex];
        if (G.isUnlocked)
        {
            PurchaseButton.gameObject.SetActive(false);
            ZombieGunSlides[ZombieGunIndex].transform.GetChild(1).gameObject.SetActive(false);                                //lock image removal
        }
        else
        {
            PurchaseButton.gameObject.SetActive(true);
            ZombieGunSlides[ZombieGunIndex].transform.GetChild(1).gameObject.SetActive(true);                                 //lock image
            PurchaseButton.transform.GetChild(1).gameObject.GetComponent<Text>().text = G.Price.ToString();

            //if(G.Price <= PlayerPrefs.GetInt("PlayerCoins"))
            //{
            //    PurchaseButton.interactable = true;
            //}
            //else
            //{
            //    PurchaseButton.interactable = false;
            //}
        }
    }



    public void ShowADWithAdBreak()
    {
#if !UNITY_EDITOR
        AdsManger_New.Instance.ShowUnityAdAdmobWithAdBreak();
#endif
    }

}
