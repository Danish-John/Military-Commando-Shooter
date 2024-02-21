using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CoverShooter;

public class RagdollManager : MonoBehaviour
{
    public GameObject RagdollMesh;

    

    public void InstantiateRagdoll()
    {

        GameObject instance = Instantiate(RagdollMesh, transform);
        instance.transform.position = this.transform.position;
        instance.transform.rotation = this.transform.rotation;

        this.transform.GetChild(0).gameObject.SetActive(false);
        this.transform.GetChild(1).gameObject.SetActive(false);


        MonoBehaviour[] comps = GetComponents<MonoBehaviour>();
        foreach (MonoBehaviour c in comps)
        {
            c.enabled = false;
            this.gameObject.GetComponent<Animator>().enabled = true;
        }
        this.gameObject.GetComponent<Animator>().enabled = false;
        this.gameObject.GetComponent<CharacterMotor>().enabled = false;
        this.gameObject.GetComponent<CapsuleCollider>().enabled = false;


    }
}
