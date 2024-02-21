using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunHideShow : MonoBehaviour
{
    public GameObject Player;
    public GameObject Waypoint;
    public GameObject Gun;

    public GameObject BulletsText;
    [HideInInspector] public static bool GunHoldershown = false;

    bool check = false;
    
    // Start is called before the first frame update
    void Start()
    {
        Gun.SetActive(false);
        
    }

    // Update is called once per frame
    void Update()
    {
        if(check == false)
        {
            float dist = Vector3.Distance(Player.transform.position, Waypoint.transform.position);
            if (dist <= 1)
            {
                LevelManagerZombiesMode.instance.GunHolder.GetComponent<GunSelector>().enabled = true;
                Gun.SetActive(true);
                BulletsText.SetActive(true);
                GunHoldershown = true;
                check = true;

                //if (PlayerPrefs.GetInt("RewardedGun") == 1)
                //{
                //    Gun.GetComponent<GunSelector>().enabled = false;
                //    for (int i = 0; i < LevelManagerZombiesMode.instance.totalguns; i++)
                //    {
                //        Gun.transform.GetChild(i).gameObject.SetActive(false);
                //    }
                //    Gun.transform.GetChild(LevelManagerZombiesMode.instance.gunindex).gameObject.SetActive(true);
                //}


            }
        }
        
    }
}
