using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TargetsRange { TenMeter, FifteenMeter, TwentyMeter, TwentyFiveMeter, ThirtyMeter }
public class LevelPvP : MonoBehaviour
{
    public enum Difficulty {Easy,Medium,Hard}

  
    public TargetsRange LevelTargetsRange;
    public Difficulty LevelDifficulty;
    [SerializeField] private Transform BotSpawnPoint;
    [SerializeField] private Transform PlayerSpawnPoint;
    public Targets [] BotTargets;
    private GameManagerPvp instance;
    public GameObject PlayerTarget;
    public bool isNightLevel;
    void Start()
    {
        instance = GameManagerPvp.instance;
        instance.SelectedBot.transform.position = BotSpawnPoint.position;
        instance.Player.transform.position = PlayerSpawnPoint.position;
        instance.Player.transform.rotation = PlayerSpawnPoint.rotation;
        SetScopeRange();




        //if (GameManagerPvp.instance.PvPGameData.CurrentLevel == 0 || GameManagerPvp.instance.PvPGameData.CurrentLevel == 2 || GameManagerPvp.instance.PvPGameData.CurrentLevel == 4 || GameManagerPvp.instance.PvPGameData.CurrentLevel == 6)
        //{
        //    NetCheckinLevels();
            
        //}



    }





//    void NetCheckinLevels()
//    {
//        if (AdsManger_New.Instance.IsInternetConnection() == true)
//        {
//#if !UNITY_EDITOR
//            FirebaseManager.Instance.LogEvent("NetOn_PvP_" + GameManagerPvp.instance.PvPGameData.CurrentLevel+1);
//#endif
//        }
//        else if (AdsManger_New.Instance.IsInternetConnection() == false)
//        {
//#if !UNITY_EDITOR
//            FirebaseManager.Instance.LogEvent("NetOff_PvP_" + GameManagerPvp.instance.PvPGameData.CurrentLevel+1);
//#endif
//        }
//    }






    void SetScopeRange()
    {
        switch(LevelTargetsRange)
        {
            case TargetsRange.TenMeter:
                GameManagerPvp.instance.PlayerVcam.ZoomFov = 20f;  //8.5
                break;

            case TargetsRange.FifteenMeter:
                GameManagerPvp.instance.PlayerVcam.ZoomFov = 20f;
                break;

            case TargetsRange.TwentyMeter:
                GameManagerPvp.instance.PlayerVcam.ZoomFov = 22f;  //8
                break;

            case TargetsRange.TwentyFiveMeter:
                GameManagerPvp.instance.PlayerVcam.ZoomFov = 22f; //8
                break;

            case TargetsRange.ThirtyMeter:
                GameManagerPvp.instance.PlayerVcam.ZoomFov = 23f; //6
                break;

        }
    }

}
