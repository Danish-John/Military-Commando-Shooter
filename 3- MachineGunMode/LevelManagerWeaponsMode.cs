using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LevelManagerWeaponsMode : MonoBehaviour
{
    [SerializeField] private EnemiesSpawner[] Levels;
    public EnemiesSpawner[] _Levels { get { return Levels; } }

   [HideInInspector]public int CurrentLevel;

   

    //public GameObject BulletsText;
    void Start()
    {
        foreach(EnemiesSpawner level in Levels)
        {
            level.Manager = this;
        }
        GameManagerWeaponsMode.instance.PlayerMove.transform.position = _Levels[CurrentLevel].StartingWaypoint.transform.position;
        StartNextLevel();
        InGameUi.instance.GameplayHUD.SetActive(true);




        //if (GameManagerWeaponsMode.instance.Data.CurrentLevel == 0 || GameManagerWeaponsMode.instance.Data.CurrentLevel == 2 || GameManagerWeaponsMode.instance.Data.CurrentLevel == 4 || GameManagerWeaponsMode.instance.Data.CurrentLevel == 6)
        //{
        //    NetCheckinLevels();

        //}



    }




//    void NetCheckinLevels()
//    {
//        if (AdsManger_New.Instance.IsInternetConnection() == true)
//        {
//#if !UNITY_EDITOR
//            FirebaseManager.Instance.LogEvent("NetOn_WarZone_" + GameManagerWeaponsMode.instance.Data.CurrentLevel+1);
//#endif
//        }
//        else if (AdsManger_New.Instance.IsInternetConnection() == false)
//        {
//#if !UNITY_EDITOR
//            FirebaseManager.Instance.LogEvent("NetOff_WarZone_" + GameManagerWeaponsMode.instance.Data.CurrentLevel+1);
//#endif
//        }
//    }










    public void StartNextLevel()
    {
        GameManagerWeaponsMode.instance.CF2Rig.SetActive(false);

        if (CurrentLevel < Levels.Length)
        {
            if(CurrentLevel>0)
            {
                Levels[CurrentLevel-1].gameObject.SetActive(false);
            }
            Levels[CurrentLevel].gameObject.SetActive(true);
            //BulletsText.SetActive(false);
            
            GameManagerWeaponsMode.instance.PlayerMove.transform.parent = Levels[CurrentLevel].StartingWaypoint.transform.parent;
            GameManagerWeaponsMode.instance.PlayerMove.SetDestination(Levels[CurrentLevel].StartingWaypoint);
        }
        else
        {  
            GameManagerWeaponsMode.instance.VictoryScreen();
        }

       // CurrentLevel++;
    }
}
