using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardedGunsInGameplay : MonoBehaviour
{
    public GameObject[] Buttons;
    int check = 0;

    [HideInInspector] public bool offfunc = true;

    public static RewardedGunsInGameplay instance;
    [HideInInspector] public int selectedzombiegun;

    [HideInInspector] public int index = 0;

    void Start()
    {
        instance = this;
        selectedzombiegun = PlayerPrefs.GetInt("SelectedZombieGunIndex") - 1;
        

        Initialization();

    }



    public void Initialization()
    {
        //for (int i = 0; i < 3; i++)
        //{
        //    if (i == selectedzombiegun)
        //    {
        //        Buttons[i] = null;
        //    }
        //}


        StartCoroutine(OnOff());
    }



    IEnumerator OnOff()
    {
        //for (int i = 0; i < 3; i++)
        //{
        //    if (i != selectedzombiegun)
        //    {
        //        Buttons[i].SetActive(false);
        //        index = i;
        //    }
            
        //}

        while (offfunc == true)
        {
            

            if (index > 2)
            {
                index = 0;
            }


            for (int i = 0; i < 3; i++)
            {
                Buttons[i].SetActive(false); 
            }



            

            if (index != selectedzombiegun)
            {
                Buttons[index].SetActive(true);
                
                index++;
                yield return new WaitForSeconds(3f);
            }
            else if (index == selectedzombiegun)
            {
                index++;
                yield return new WaitForSeconds(0f);
            }

            
            
        }

    }

    public void HideGunBtns()
    {
        StopCoroutine(OnOff());
        offfunc = false;
        for (int i = 0; i < 3; i++)
        {
            Buttons[i].SetActive(false);
        }

        
    }

}
