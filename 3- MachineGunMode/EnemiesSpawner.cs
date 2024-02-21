using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CoverShooter;
using DG.Tweening;
public class EnemiesSpawner : MonoBehaviour
{
    [HideInInspector] public LevelManagerWeaponsMode Manager;

    private int TotalWaves;

    public Waypoint StartingWaypoint;

    private int CurrentWaveNumber;
    public int _currentWaveNumber { get { return CurrentWaveNumber; } }

    private int CurrentEnemyCount;

    private int WaveEnemyNoIndex;

    private int DeathCount;

    private int AirWaypointIndex;

    private int TankWaypointIndex;

    public int[] EnemiesPerWave;

    private int TotalEnemies;

    private int RemainingEnemies;
    public int _RemainingEnemies { get { return RemainingEnemies; } }

    public CharacterHealth [] EnemiesPrefabs;

    public Waypoint[] enemyWaypoints;

    public Waypoint[] EnemyAirWayPoints;

    public Waypoint[] TankWaypoints;

    private int NormalWaypointIndex;

    public bool MG;

    public GameObject HolderRPG;







    private void Awake()
    {

        TotalWaves = EnemiesPerWave.Length;

        for (int i = 0; i < EnemiesPerWave.Length; i++)
        {
            RemainingEnemies += EnemiesPerWave[i];
        }
       
    }

    void Start()
    {
       for(int i=0; i<EnemiesPrefabs.Length;i++)
        {
            AIFire fire;

            AISight sight;

            if (EnemiesPrefabs[i].TryGetComponent(out fire))
            {
                fire.enabled = true;
            }
            if (EnemiesPrefabs[i].TryGetComponent(out sight))
            {
                sight.Distance = 1000;
            }
            
           // EnemiesPrefabs[i].OnDeath.AddListener(CheckForLastEnemy);
        }


        if (MG)
        {
            HolderRPG.SetActive(false);
        }
        else if (!MG)
        {
            HolderRPG.SetActive(true);
        }

    }


  
   public void StartWave()  // Start When PlayerEnterCover
    {
    
        if (CurrentWaveNumber <= TotalWaves)
        {
            TotalEnemies = EnemiesPerWave[WaveEnemyNoIndex];
            StartCoroutine(SpawnEnemy());
        }
        else
        {
            Manager.CurrentLevel++;        
        }
    }

    IEnumerator SpawnEnemy()
    {
       
        while(CurrentEnemyCount <= EnemiesPerWave[WaveEnemyNoIndex])
        {
            int SpawnDelay = 0 ;
          
            if (CurrentEnemyCount >= EnemiesPerWave[WaveEnemyNoIndex])
            {
           
                CurrentEnemyCount = 0;
                yield break;
            }
            else
            {
               

                int EnemiesPrefabsIndex = Random.Range(0, EnemiesPrefabs.Length);

                GameObject instance = Instantiate(EnemiesPrefabs[EnemiesPrefabsIndex].gameObject);             

                EnemyWaypoint point = instance.AddComponent<EnemyWaypoint>();

                point.health = instance.GetComponent<CharacterHealth>();

               
              
                instance.GetComponent<CharacterHealth>().OnDeath.AddListener(()=> 
                {
                    GameManagerWeaponsMode.instance.ScoreUp(10);

                    CheckForLastEnemy();

                    //instance.SetActive(false);

                    RemainingEnemies--;

                    InGameUi.instance.SetHUDData();
                 
                    DOTween.Sequence().SetDelay(3f).Append(instance.transform.DOLocalMoveY(-100f,3f));


                });

              
                if(instance.tag =="AirEnemy")
                {
                    SpawnDelay = 4;

                    point._MoveSpeed = 0.04f;

                    point._CurrentWaypoint = EnemyAirWayPoints[AirWaypointIndex];

                    instance.transform.position = EnemyAirWayPoints[AirWaypointIndex].transform.position;

                    AirWaypointIndex++;

                    if (AirWaypointIndex >= EnemyAirWayPoints.Length)
                    {
                        AirWaypointIndex = 0;
                    }

                }
              
                else if(instance.tag=="Tank")
                {
                    SpawnDelay = 4;

                    point._MoveSpeed = 0.03f;

                    point.rotationSpeed = 0.7f;

                    point._CurrentWaypoint = TankWaypoints[TankWaypointIndex];

                    instance.transform.position = TankWaypoints[TankWaypointIndex].transform.position;

                    TankWaypointIndex++;

                    
                    if (TankWaypointIndex >= TankWaypoints.Length)
                    {
                        TankWaypointIndex = 0;
                    }
                }
                else
                {
                    SpawnDelay = 2;

                    point._MoveSpeed = 0.04f;

                    point._CurrentWaypoint = enemyWaypoints[NormalWaypointIndex];

                    instance.transform.position = enemyWaypoints[NormalWaypointIndex].transform.position;


                    NormalWaypointIndex++;

                    if (NormalWaypointIndex >= enemyWaypoints.Length)
                    {
                        NormalWaypointIndex = 0;
                    }
                }

                yield return new WaitForSeconds(SpawnDelay);

            }

            CurrentEnemyCount++;
        }
        

    }

    void CheckForLastEnemy()
    {
      
        DeathCount++;
      
        if (DeathCount>= EnemiesPerWave[WaveEnemyNoIndex])
        {
            CurrentWaveNumber++;
            if (CurrentWaveNumber >= TotalWaves)
            {
                Manager.CurrentLevel++;
                Manager.StartNextLevel();
            }
            else
            {
                WaveEnemyNoIndex++;
                Invoke(nameof(StartWave), 2f);
            }
          
            DeathCount = 0;
        }
      
    }
}
