using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ButtonOff : MonoBehaviour
{

    public GameObject Button1, Button2;


    // Start is called before the first frame update
    void Start()
    {
        

    }

    // Update is called once per frame
    void Update()
    {
        Button1.SetActive(false);
        Button2.SetActive(false);

       

    }
}
