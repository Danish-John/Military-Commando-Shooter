using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CoverShooter;
using DG.Tweening;
public class HeavyMachineGun : MonoBehaviour
{
   [SerializeField]private LayerMask ObjectsToShoot;

    private float XRotation;

    private float YRotation;
    [Range(0, 1f)]
    public float Sensitivity;


    [HideInInspector] public CharacterMotor Motor;

    public float interpolationPeriod = 0.1f;

    float time;

    private AudioSource Source;

    [SerializeField] private AudioClip ShootingAudio;

    public Animator GunAnimator;

    public ParticleSystem [] MuzzleParticles;

    public GameObject Bullet;

    public Transform ShootingPoint;

    public int WeaponDmg;
    void Start()
    {
        Motor = GameManagerWeaponsMode.instance.PlayerMove.GetComponent<CharacterMotor>();

        Source = GetComponent<AudioSource>();

       
        time = interpolationPeriod + 1;
        
       
    }

    // Update is called once per frame
    void Update()
    {
        
        Shooting();

        Rotation();
     
    }

    void Rotation()
    {
        XRotation = ControlFreak2.CF2Input.GetAxis("Mouse X");
        YRotation = -ControlFreak2.CF2Input.GetAxis("Mouse Y");

        XRotation = Mathf.Clamp(XRotation, -90, 90);
        YRotation = Mathf.Clamp(YRotation, -30, 30);


        //transform.eulerAngles += new Vector3(YRotation, XRotation, 0); 

        //  Quaternion TargetRotation = Quaternion.Euler(transform.eulerAngles.x + YRotation, transform.eulerAngles.y + XRotation, 0f);

        Quaternion TargetRotation = Quaternion.Euler(transform.eulerAngles.x + YRotation, transform.eulerAngles.y + XRotation, 0f);

        transform.rotation = Quaternion.Slerp(transform.rotation, TargetRotation, Sensitivity);

    }

    void Shooting()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, ObjectsToShoot))
        {
           
            Vector3 direction = (hit.point - transform.position).normalized;

           

            var hitStruct = new Hit(hit.point, -direction, WeaponDmg, Motor.gameObject, hit.collider == null ? null : hit.collider.gameObject, HitType.Rifle, 0);       
            
            time += Time.deltaTime;

            if (time >= interpolationPeriod)
            {
                time = time - interpolationPeriod;

                foreach(ParticleSystem system in MuzzleParticles)
                {
                    system.Play();
                }

               
                Source.PlayOneShot(ShootingAudio);

                if(GunAnimator)
                {
                    GunAnimator.SetBool("isShooting", true);
                }
              

                if(Bullet)
                {
                    var bullet = Instantiate(Bullet, ShootingPoint.position, Quaternion.identity);

                    bullet.transform.LookAt(hit.point);

                    var projectile = bullet.GetComponent<Projectile>();

                    var vector = hit.point - ShootingPoint.position;

                    projectile.Distance = vector.magnitude;

                    projectile.Direction = vector.normalized;

                   
                    if (hit.collider != null)
                    {

                        projectile.Target = hit.collider.gameObject;

                        projectile.Hit = hitStruct;

                    }
                }
                else
                {
                    hit.collider.SendMessage("OnHit", hitStruct, SendMessageOptions.DontRequireReceiver);
                }
              //  

                // execute block of code here
            }

        }
        else
        {
            if(GunAnimator)
            {
                GunAnimator.SetBool("isShooting", false);
            }
           
        }
    }
}
