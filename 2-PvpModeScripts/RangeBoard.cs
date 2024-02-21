using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeBoard : MonoBehaviour
{
    public TargetsRange CurrentDifficulty;
    void Start()
    {
        if(CurrentDifficulty!=GameManagerPvp.instance.SelectedLevel.LevelTargetsRange)
        {
            gameObject.SetActive(false);
        }
    }

    
}
