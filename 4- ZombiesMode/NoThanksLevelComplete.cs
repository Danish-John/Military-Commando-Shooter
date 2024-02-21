using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoThanksLevelComplete : MonoBehaviour
{

    public GameObject NoThanks;
    public float Time;

    public GameObject NextBtn;

    readonly  RNGBasedReward instance;


    private void Awake()
    {
        NextBtn.SetActive(false);
    }

    private void Update()
    {
        if (RNGBasedReward.instance.StopMovement == true)
        {
            NoThanks.SetActive(false);
            NextBtn.SetActive(true);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        NextBtn.SetActive(false);

        Invoke("NoThanksDisplay", Time);
    }


    void NoThanksDisplay()
    {
        if(RNGBasedReward.instance.StopMovement == true)
        {
            NoThanks.SetActive(false);
            NextBtn.SetActive(true);
        }
        else
        {
            NoThanks.SetActive(true);
            NextBtn.SetActive(false);
        }

    }
}
