using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSwappingRewardedGuns : MonoBehaviour
{
    public GameObject[] Buttons;
    int check = 0;

    [HideInInspector] public bool offfunc = true;

    public static ButtonSwappingRewardedGuns instance;
    [HideInInspector] public int selectedsnipergun;

    [HideInInspector] public int index = 0;

    void Start()
    {
        instance = this;
        selectedsnipergun = NewShop.instance.GunNo - 1;
       
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


            if (index > 3)
            {
                index = 0;
            }


            for (int i = 0; i < 4; i++)
            {
                Buttons[i].SetActive(false);
            }

          
            if (index != selectedsnipergun)
            {
                Buttons[index].SetActive(true);
                
                index++;
                yield return new WaitForSeconds(3f);

            }
            else if (index == selectedsnipergun)
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
        for (int i = 0; i < 4; i++)
        {
            Buttons[i].SetActive(false);
        }



    }

}
