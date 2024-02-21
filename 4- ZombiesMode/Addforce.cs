using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CoverShooter;

public class Addforce : MonoBehaviour
{
    
    public int[] arr;

    public Rigidbody[] rigidBody;

    int Val;

    //bool check = true;

    private void Start()
    {
        AddForce();
    }


    void AddForce()
    {
        if (GameManager.instance) //For Sniper Mode
        {
            

            if (Projectile.instance)                                             // For Last Enemy
            {
                Val = 60;

                foreach (Rigidbody rb in rigidBody)
                {
                    //rb.AddForce(this.transform.forward * arr[Val], ForceMode.Impulse);
                    rb.AddForce(Projectile.instance.Direction * Val, ForceMode.Impulse);
                }
                
                this.transform.GetChild(2).gameObject.SetActive(true);
            }
            else                                                                                                         // For Other enemies
            {
                Vector3 rot = GameManager.instance.Player.transform.localEulerAngles;

                foreach (Rigidbody rb in rigidBody)
                {
                    //rb.AddForce(this.transform.forward * arr[Val], ForceMode.Impulse);
                    rb.AddForce(rot * (0), ForceMode.Impulse);
                }
                
                this.transform.GetChild(2).gameObject.SetActive(true);
            }
        }
        else //For WarZone Mode and Zombie Mode
        {
            Val = UnityEngine.Random.Range(0, 2);


            foreach (Rigidbody rb in rigidBody)
            {
                //rb.AddForce(this.transform.forward * arr[Val], ForceMode.Impulse);
                rb.AddForce(Projectile.instance.Direction * arr[Val], ForceMode.Impulse);
            }
            this.transform.GetChild(2).gameObject.SetActive(true);
        }



    }

}
