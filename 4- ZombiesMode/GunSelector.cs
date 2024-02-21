using UnityEngine;

public class GunSelector : MonoBehaviour
{
    int ZombieGunIndex;
    public GameObject[] ZombieGuns;


    private void Start()
    {
        
       
    }



    private void OnEnable()
    {
        if (PlayerPrefs.GetInt("RewardedGun") == 0)
        {
            if (PlayerPrefs.GetInt("GunInScene") == 0)
            {
                ZombieGunIndex = PlayerPrefs.GetInt("SelectedZombieGunIndex", 0);

                foreach (GameObject ZombieGun in ZombieGuns)
                    ZombieGun.SetActive(false);

                ZombieGuns[ZombieGunIndex].SetActive(true);
            }
            else if (PlayerPrefs.GetInt("GunInScene") != 0)
            {
                ZombieGunIndex = PlayerPrefs.GetInt("GunInScene");

                foreach (GameObject ZombieGun in ZombieGuns)
                    ZombieGun.SetActive(false);

                ZombieGuns[ZombieGunIndex].SetActive(true);
            }
        }


        else if(PlayerPrefs.GetInt("RewardedGun") == 1)
        {
            foreach (GameObject ZombieGun in ZombieGuns)
                ZombieGun.SetActive(false);

            ZombieGuns[PlayerPrefs.GetInt("RewardedGunIndex")].SetActive(true);
        }
       

    }

}
