using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
public class InGameUiPVP : MonoBehaviour
{
    public GameObject WinAcknoledgment;
    public GameObject DefeatAcknowledgment;
 
    // Start is called before the first frame update
    public Text PlayerInGameScoreText;
    public Text BotIngameScoreText;
    public TextMeshProUGUI ScorePopUp;
  
    public GameObject BullseyeObject;
    public Image BullsEyeBulletImage;
    public RectTransform BullsEyeSpark;

    public Text RoundsDisplay;

    public Text WinPlayerscore;
    public Text WinBotScore;

    public Text LosePlayerScore;
    public Text LoseBotScore;

    public TextMeshProUGUI TimerText;

    public static InGameUiPVP instance;

    public GameObject PlayerTurnShowcase;
    public GameObject BotTurnShowCase;

    public Image IngameBotAvatar;
    public Image WinBotAvatar;
    public Image LoseBotAvatar;

    private MainData GameData;

    private bool TutorialShown;
    public bool FirstScreenShown;

    public Image sliderFillImage;
    public Slider timeSlider;
    private float slidervaluetoshow;

    public GameObject RivalTurnHand;
    private void Awake()
    {
        slidervaluetoshow = 1;
        instance = this;
    }

    private void Start()
    {
        GameData = GameManagerPvp.instance.PvPGameData.gameData;
        IngameBotAvatar.sprite = GameData.BotSprites[GameData.selectedImageIndex];
        WinBotAvatar.sprite = GameData.BotSprites[GameData.selectedImageIndex];
        LoseBotAvatar.sprite = GameData.BotSprites[GameData.selectedImageIndex];
    }

    public void ChangeTimerSliderValue(float value)
    {
        if(value <= 0.6f && value > 0.4f)
        {
            sliderFillImage.color = Color.cyan;
        }
        else if(value <=0.3f)
        {
            sliderFillImage.color = Color.red;
        }
        else
        {
            sliderFillImage.color = Color.green;
        }

        slidervaluetoshow = value;
    }

    private void Update()
    {
        timeSlider.value = Mathf.Lerp(timeSlider.value, slidervaluetoshow, 0.25f);
    }
    public void ShowcaseDisable()
    {
        
        PlayerTurnShowcase.SetActive(false);
        BotTurnShowCase.SetActive(false);
        GameManagerPvp.instance.TurnStart = true;
        if (GameManagerPvp.instance.IsPlayerTurn)
        {
            GameManagerPvp.instance.AudSource.PlayOneShot(GameManagerPvp.instance.PlayerTurnClip2);
            GameManagerPvp.instance.IPRig.gameObject.SetActive(true);
            GameManagerPvp.instance.TutorialObj1.SetActive(true);
            if (PlayerPrefs.GetInt("ShowPvpTutorial", 1) == 1 && !TutorialShown)
            {
             //   GameManagerPvp.instance.TutorialObj2.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
                //GameManagerPvp.instance.TutorialObj2.transform.GetChild(0).GetChild(1).gameObject.SetActive(true);
                TutorialShown = true;
            }

        }
        else
        {
           
            if (PlayerPrefs.GetInt("ShowPvpTutorial", 1) == 1 && !TutorialShown)
            {
               // GameManagerPvp.instance.TutorialObj2.transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
              //  GameManagerPvp.instance.TutorialObj2.transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
               // GameManagerPvp.instance.TutorialObj2.SetActive(true);
                FirstScreenShown = true;
            }
        }
    
    }

    void HidePopUps()
    {
        ScorePopUp.gameObject.SetActive(false);
        BullseyeObject.SetActive(false);

        BullsEyeSpark.DOScale(0, 0f);
        BullsEyeSpark.gameObject.SetActive(false);
     
        BullsEyeBulletImage.color= new Color(BullsEyeBulletImage.color.r, BullsEyeBulletImage.color.g, BullsEyeBulletImage.color.b, 0f);  // make color black
        BullsEyeBulletImage.gameObject.SetActive(false);
     
    }
    public void ChangeDisplaySCore(int scoreAdded)
    {
        ScorePopUp.text = scoreAdded.ToString();

        ScorePopUp.gameObject.SetActive(true);


        Invoke("HidePopUps", 4f);

        PlayerInGameScoreText.text = GameManagerPvp.instance.PlayerScore.ToString();

        BotIngameScoreText.text = GameManagerPvp.instance.BotScore.ToString();

        GameManagerPvp.instance.IPRig.gameObject.SetActive(false);

        if (PlayerPrefs.GetInt("ShowPvpTutorial", 1) == 1)
        {
           // GameManagerPvp.instance.TutorialObj1.SetActive(false);
        }
       

      //  GameManagerPvp.instance.Invoke("ChangeTurn", 3);
    }

  
    public void EndGameScreen(int BotScore,int PlayerScore,bool isWin)
    {
        if(isWin)
        {
            WinPlayerscore.text = PlayerScore.ToString();
            WinBotScore.text = BotScore.ToString();
        }
        else
        {
            LosePlayerScore.text = PlayerScore.ToString();
            LoseBotScore.text = BotScore.ToString();
        }
    }

   public void BullsEyeAnimation()
    {
        BullsEyeSpark.gameObject.SetActive(true);
        BullsEyeBulletImage.gameObject.SetActive(true);
        BullseyeObject.SetActive(true);
    }
  
}
