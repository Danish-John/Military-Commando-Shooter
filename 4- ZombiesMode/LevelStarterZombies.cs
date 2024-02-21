using UnityEngine.Events;
using UnityEngine;

public class LevelStarterZombies : MonoBehaviour
{
    public UnityEvent OnPlayerEnter;

    GameObject LevelPart;

    private void Start()
    {
        LevelPart = GameObject.FindGameObjectWithTag("LevelPart");
    }

    private void Update()
    {
        LevelPart = GameObject.FindGameObjectWithTag("LevelPart");
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            gameObject.SetActive(false);
            GameManagerZombiesMode.instance.CF2Rig.SetActive(true);
            other.transform.gameObject.GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;
            GameObject.FindGameObjectWithTag("HolderGun").GetComponent<MeshRenderer>().enabled = false;
            GameObject.FindGameObjectWithTag("HolderGun").transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;

            
            OnPlayerEnter.Invoke();
            if (!LevelPart.activeSelf)
            {
                LevelPart = GameObject.FindGameObjectWithTag("LevelPart");

            }
            
            LevelPart.GetComponent<EnemiesSpawnerZombies>().StartWave();

        }
    }



}
