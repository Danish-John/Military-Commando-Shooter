using UnityEngine;
using CoverShooter;
using System.Collections.Generic;
public class Level : MonoBehaviour
{

    private float DefaultShootInterval;
    [HideInInspector] public int TargetToShoot;
    [HideInInspector] public int ObjectiveToShoot;
    public float defaultShootInterval { get { return DefaultShootInterval; } }

    [HideInInspector] public Move Player;
    public GameObject[] Enemies;
    public Objective[] Objectives;
    public Waypoint StartingWaypoint;  // The point which player will move to

    [HideInInspector] public int RemainingEnemies;
    [HideInInspector] public int RemainingObjectives;

    [HideInInspector] public LevelManager lvl_manager;


    private Outline[] StaticOutlines;

    private int enemyCount;
    private int ObjectiveCount;

    private List<AISight> AI;

    public List<CharacterMotor> EnemyMotor;

    private List<AIFire> AiFire;

    private bool EnemiesAlarmed;

    public bool _EnemiesAlarmed {get { return EnemiesAlarmed; } set { EnemiesAlarmed = value; } }

    public bool DisableEnemies=true;

    public static Level instance;



    private void Start()
    {
        instance = this;
    }



    public void DisableEnemyAggro()
    {
        for (int i = 0; i < EnemyMotor.Count; i++)
        {
            EnemyMotor[i].enabled = false;
            // AiFire[i].enabled = false;
        }
    }

    public void AlarmEnemies()
    {
        if (!EnemiesAlarmed)
        {
          
            EnemiesAlarmed = true;


            if(AI!=null)
            {
                for (int i = 0; i < AI.Count; i++)
                {
                    AI[i].Distance = 100f;

                }
            }
          
            if(EnemyMotor!=null)
            {
                for (int i = 0; i < EnemyMotor.Count; i++)
                {

                    EnemyMotor[i].enabled = true;

            
                }
            }
           
            if(AiFire!=null)
            {
                for (int i = 0; i < AiFire.Count; i++)
                {

                    AiFire[i].enabled = true;

                }
            }
           
        }
    }
    void AddObjectiveDeathListener()
    {
        RemainingObjectives = Objectives.Length;

        for (int i = 0; i < Objectives.Length; i++)
        {

            Objectives[i].OnDeath.AddListener(() =>

            {

                RemainingObjectives--;
                GameManager.instance.TotalObjectives--;

                ObjectiveCount++;
                if (ObjectiveCount > Objectives.Length - 1)
                {
                    ObjectiveCount = Objectives.Length - 1;
                }
                ObjectiveToShoot = ObjectiveCount;

                if (RemainingEnemies < 1 && RemainingObjectives < 1)
                {

                    GameManager.instance.LastManStanding = true;

                }

              //  CheckForActionCam();
                CheckCompletion();
            });
        }
    }

     void AddEnemyOnDeathListeners()
    {

        RemainingEnemies = Enemies.Length;

        for (int i = 0; i < Enemies.Length; i++)
        { 
            CharacterHealth health;
            AISight Sight;
            AIFire Fire;
            CharacterMotor motor;
            Outline line;
            Enemies[i].TryGetComponent<CharacterHealth>(out health);

            if (Enemies[i].TryGetComponent<Outline>(out line))
            {
                StaticOutlines[i] = line;
                StaticOutlines[i].enabled = false;
            }

            if (Enemies[i].TryGetComponent<AISight>(out Sight))
            {
               AI.Add(Sight);
            }

            if (Enemies[i].TryGetComponent<CharacterMotor>(out motor))
            {
               EnemyMotor.Add(motor);
            }

            if (Enemies[i].TryGetComponent<AIFire>(out Fire))
            {

               AiFire.Add(Fire);
            }


            health.OnDeath.AddListener(() =>
            {
                
                GameManager.instance.PlayerCamera._Input.FirstShotFired = false;
                RemainingEnemies--;

               GameManager.instance.TotalObjectives--;



                if (RemainingEnemies < 1 && RemainingObjectives < 1)
                {

                    GameManager.instance.LastManStanding = true;

                }
                enemyCount++;
                if (enemyCount > Enemies.Length - 1)
                {
                    enemyCount = Enemies.Length - 1;
                }
                TargetToShoot = enemyCount;

                CheckForActionCam();
                CheckCompletion();

            });

            //   GameManager.instance.EnemyMotor[i].enabled = false;



        }

    }
    public void CheckForActionCam()
    {
       
       
        if (lvl_manager.LevelIndex == lvl_manager.Levels.Length-1)
        {
            if(RemainingEnemies<2 && RemainingObjectives == 0)
            {
              
                GameManager.instance.allowactionCam = true;
            }
            else if(RemainingObjectives < 2 && RemainingEnemies == 0)
            {
               
                GameManager.instance.allowactionCam = true;
            }
        
        }
    }
    public void StartMission()
    {
        
        EnemyMotor=new List<CharacterMotor>();
        AI=new List<AISight>();
        AiFire = new List<AIFire>();
        EnemiesAlarmed=false;

        GameManager.instance.LastManStanding = false;

        if (Enemies.Length == 1 && lvl_manager.LevelIndex == lvl_manager.Levels.Length - 1)
        {
         
            GameManager.instance.allowactionCam = true;
          
        }

        Player.SetDestination(StartingWaypoint);

        StaticOutlines = new Outline[Enemies.Length];

        GameManager.instance.EnableOutline.AddListener(() => 
        {
          
            if (StaticOutlines.Length > 0)
            {
                foreach (Outline StaticOutline in StaticOutlines)
                {
                    if (StaticOutline != null)
                        StaticOutline.enabled = true;
                }
            }

        });

        GameManager.instance.DisableOutline.AddListener(() =>
        {
         

            if (StaticOutlines.Length > 0)
            {
                foreach (Outline StaticOutline in StaticOutlines)
                {
                    if (StaticOutline != null)
                        StaticOutline.enabled = false;
                }
            }
        });
     



        if(Enemies.Length>0)
        {
            AddEnemyOnDeathListeners();
        }



        if (Objectives.Length>0)
        {
            AddObjectiveDeathListener();
        }


        if(lvl_manager.LevelIndex<1)
        {
            GameManager.instance.SetTotalEnemies();
        }
       

        InGameUi.instance.SetHUDData();

    }

   
  
    void  CheckCompletion()
    {
       
        GameManager.instance.DisableScope(false);

        if (RemainingEnemies <=0 && RemainingObjectives <1)
        {
            GameManager.instance.PlayerCamera._Input.AllowShooting = false;  /// THIS SHOULD TELL THE GAME MANAGER RATHER 
            //Player.DisableShooting();
        
            lvl_manager.EndMission();
        }
      
    }
   


}
