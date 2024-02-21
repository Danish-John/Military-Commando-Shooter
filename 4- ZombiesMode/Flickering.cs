using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flickering : MonoBehaviour
{

    Light liight;
    public Material BulbMaterial;
    public bool StreetLight;
    public GameObject Rays;

    private void Start()
    {
        liight = this.gameObject.GetComponent<Light>();
    }

    //void Update()
    //{
    //    if (Random.value > 0.9) //a random chance
    //    {
    //        if (liight.enabled == true) //if the light is on...
    //        {
    //            liight.enabled = false; //turn it off
    //            BulbMaterial.DisableKeyword("_EMISSION");
    //        }
    //        else
    //        {
    //            liight.enabled = true; //turn it on
    //            BulbMaterial.EnableKeyword("_EMISSION");
    //        }
    //    }
    //}


    private void FixedUpdate()
    {
        if (StreetLight)
        {
            if (Random.value > 0.9) //a random chance
            {
                if (liight.enabled == true) //if the light is on...
                {
                    liight.enabled = false; //turn it off
                    BulbMaterial.DisableKeyword("_EMISSION");
                    Rays.SetActive(false);
                }
                else
                {
                    liight.enabled = true; //turn it on
                    BulbMaterial.EnableKeyword("_EMISSION");
                    Rays.SetActive(true);

                }
            }
        }
        else
        {
            if (Random.value > 0.9) //a random chance
            {
                if (liight.enabled == true) //if the light is on...
                {
                    liight.enabled = false; //turn it off
                    BulbMaterial.DisableKeyword("_EMISSION");
                }
                else
                {
                    liight.enabled = true; //turn it on
                    BulbMaterial.EnableKeyword("_EMISSION");
                }
            }
        }

        
    }

}
