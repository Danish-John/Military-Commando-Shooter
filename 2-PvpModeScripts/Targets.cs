using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(Objective))]
public class Targets : MonoBehaviour
{
    private DOTweenAnimation movingAnimation;
    public DOTweenAnimation _movingAnimation { get {return movingAnimation; } }

    public bool isPlayerTarget;
    public int ScoreToAdd;
    public GameObject EnableAnimation;
    public GameObject [] BotAimPoints;
    public Objective ObjectiveComponent;
    public AudioSource AudSource;
    public AudioClip AudClip;
    private void Start()
    {
        ObjectiveComponent = GetComponent<Objective>();
       
        EnableAnimation = transform.GetChild(0).gameObject;
        
        
        if(transform.parent!=null)
        {
            movingAnimation = transform.parent.GetComponentInParent<DOTweenAnimation>();
        }
       

       
    }

    void PlayAudio()
    {
        
        AudSource.PlayOneShot(AudClip);
    }
    public void AddScore()
    {
        if(ScoreToAdd==10)
        {
            InGameUiPVP.instance.BullsEyeAnimation();
        }
       
        if (isPlayerTarget)
        {
           GameManagerPvp.instance.PlayerScore += ScoreToAdd;  
        }
        else
        {
            GameManagerPvp.instance.BotScore += ScoreToAdd;
        }

        Invoke("PlayAudio", 0.5f);
        InGameUiPVP.instance.ChangeDisplaySCore(ScoreToAdd);

        if(EnableAnimation!=null)
        {
            EnableAnimation.SetActive(true);
        }
      
        if(movingAnimation!=null)
        {
            movingAnimation.Invoke("DOPlay",5f);
        }    
    }

  
}
