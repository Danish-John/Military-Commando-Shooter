using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CoverShooter;
using Cinemachine;
public class EnemyAI : MonoBehaviour
{
    /// <summary>
    ///  THIS SCRIPT SHOULD HELP FILTER OUT THE TARGETS...AND THEN SELECT THEM SO THAT AI CAN SHOOT EM....IT SHOULD ALSO DISABLE AI IF PLAYER IS SHOOTING IN PVP MODE
    ///  
    /// THE AI SHOOTS IN BULLSHIT DIRECTION THE TARGETS ARE SELECTED BY CODE AT RANDOM NOT BY SHOOTING!!!!!!!!!!!
    /// </summary>


    private GameManagerPvp GameManager;
    private AIFire AiFire;
    public AIFire _AiFire { get { return AiFire; } }

    private bool AiShot;

    public bool _AiShot { get { return AiShot; } set { AiShot=value; } }

    public Gun Gun;
    public Targets SelectedTarget;
    public GameObject TargetHitPoint;
    public CinemachineVirtualCamera Vcam;
    [HideInInspector] public Vector3 DefaultCameraPosition;
    [HideInInspector] public float DefaultFov;
    [SerializeField] private Transform ScopedCameraPosition;
    void Start()
    {
        AiFire = GetComponent<AIFire>();
       // AiFire.Distance = 0f;
        GameManager = GameManagerPvp.instance;
        DefaultCameraPosition = Vcam.transform.position;
        DefaultFov = Vcam.m_Lens.FieldOfView;
    }

    void OnEnable()
    {
       
    }

    public void Shoot()
    {
      
        AiFire.enabled = true;

        int index = SelectTarget();

    
        SelectedTarget = GameManagerPvp.instance.BotTargets[index];  // ADD INDEX HERE AFET AFTER AFTERT AFTERNASLGKNG kl

        int aimPointIndex = Random.Range(0, SelectedTarget.BotAimPoints.Length); ;

        TargetHitPoint = SelectedTarget.BotAimPoints[aimPointIndex].transform.gameObject;

        Vcam.LookAt = SelectedTarget.BotAimPoints[aimPointIndex].transform;

      
        // Invoke("ZoomIn", AiFire.Bursts.Wait);

    }
    /*
    public void ZoomIn()
    {
        //Vcam.m_Lens.FieldOfView = GameManagerPvp.instance.PlayerVcam.ZoomFov;

       // GameManagerPvp.instance.ScopeImage.gameObject.SetActive(true);

        int aimPointIndex = Random.Range(0, SelectedTarget.BotAimPoints.Length); ;

        TargetHitPoint = SelectedTarget.BotAimPoints[aimPointIndex].transform.position;

        Vcam.LookAt = SelectedTarget.BotAimPoints[aimPointIndex].transform;

      //  Vcam.Follow = null;

      //  Vcam.transform.position = ScopedCameraPosition.position;
    }
 */
    public int SelectTarget()
    {
        int[] Indices;
        int selectedIndex=0;

        if(GameManager.SelectedLevel.LevelDifficulty==LevelPvP.Difficulty.Easy)
        {

            Indices = new int[] { 4, 6, 8 };
            selectedIndex = Random.Range(0, Indices.Length);
            return Indices[selectedIndex];
        }
        else if(GameManager.SelectedLevel.LevelDifficulty==LevelPvP.Difficulty.Medium)
        {
            Indices = new int[] { 5, 8, 9 };
            selectedIndex = Random.Range(0, Indices.Length);
            return Indices[selectedIndex];
        }
        else
        {
            Indices = new int[] { 7, 8, 9 };
            selectedIndex = Random.Range(0, Indices.Length);
            return Indices[selectedIndex];
        }
      

    
    }
    
    void Update()
    {
      
    }
}
