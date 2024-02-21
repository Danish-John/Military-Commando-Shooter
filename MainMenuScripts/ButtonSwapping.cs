using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSwapping : MonoBehaviour
{

    public GameObject[] cubes ;
    public bool check;
    public string BusLink;
    public string DinoLink;


    private void Start()
    {
        if (PlayerPrefs.GetInt("ShowMainMenuTutorial", 1) != 1)
        {
            StartCoroutine(OnOff());
        }
    }


    IEnumerator OnOff()
    {
        while (true)
        {

            if (check == true)
            {
                cubes[0].SetActive(false);
                cubes[1].SetActive(true);
                check = false;
            }
            else
            {
                cubes[1].SetActive(false);
                cubes[0].SetActive(true);
                check = true;
            }

            yield return new WaitForSeconds(3);
        }
    }



    public void BusURL()
    {
        Application.OpenURL(BusLink);
    }

    public void DinoURL()
    {
        Application.OpenURL(DinoLink);
    }




}
