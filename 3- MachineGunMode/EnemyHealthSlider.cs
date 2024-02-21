using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;
public class EnemyHealthSlider : MonoBehaviour
{
    public Slider HealthSlider;
    public CoverShooter.CharacterHealth HealthScript;
    private bool RunOnce;
    private Camera cam;
    void Start()
    {
        cam = Camera.main;
        if (GameManagerWeaponsMode.instance)
        {
            HealthSlider.minValue = 0;
            HealthSlider.maxValue = HealthScript.MaxHealth;
        }
        else
        {
            gameObject.SetActive(false);
            enabled = false;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
       

        if (HealthScript._isDead && !RunOnce)
        {
            RunOnce = true;
            HealthSlider.transform.DOScale(0, 2f);
        }
        else
        {
            HealthSlider.value = HealthScript.Health;
        }

        transform.rotation = Quaternion.LookRotation(transform.position - cam.transform.position);
    }
}
