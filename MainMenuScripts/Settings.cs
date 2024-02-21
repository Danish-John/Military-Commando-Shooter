using UnityEngine.Audio;
using UnityEngine;

public class Settings : MonoBehaviour
{
    public Sprite [] OnoffSprites;
    private Sprite BGMSpriteToUse;
    public Sprite _BGMSpriteToUse { get { return BGMSpriteToUse; } }

    private Sprite SFXSpriteToUse;
    public Sprite _SFXSpriteToUse { get { return SFXSpriteToUse; } }

    public static Settings instance;
    public AudioMixer SFXMixer;
    public AudioMixer BGMMixer;

    public bool isSFXOn;

    public bool isBGMOn;

    private int SfxValue;
    public int sfxMixerValue;
    private int Bgmvalue;
    public int bgmMixerValue;
    private void Awake()
    {
        if(instance==null)
        {
            instance = this;
            DontDestroyOnLoad(instance.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }



        ReadSettings();
    }


    public void ReadSettings()
    {
        
        if (PlayerPrefs.GetInt("BGMAudio", 1) == 1)
        {
            isBGMOn = true;
            Bgmvalue = 1;
            bgmMixerValue =0;
        }
        else
        {
            isBGMOn = false;
            Bgmvalue = 0;
            bgmMixerValue = -80;
        }

        if (PlayerPrefs.GetInt("SFXAudio", 1) == 1)
        {
            sfxMixerValue = 0;
            SfxValue = 1;
            isSFXOn = true;
        }
        else
        {
            SfxValue = 0;
            sfxMixerValue = -80;
            isSFXOn = false;
        }

       
    }

    public void TurnSFXOnOff()
    {
        if (isSFXOn)
        {
          
            // WHEN THE BUTTON IS CLICKED IF THE AUDIO WAS ON IT TURNS OFF
            SFXSpriteToUse = OnoffSprites[1];
            SfxValue = 0;
            sfxMixerValue = -80;
           // AudioListener.pause = true;
            isSFXOn = false;
        }
        else
        {
            // WHEN THE BUTTON IS CLICKED IF THE AUDIO WAS OFF IT TURNS ON
            SFXSpriteToUse = OnoffSprites[0];
            sfxMixerValue = 0;
            SfxValue = 1;
            isSFXOn = true;



           // AudioListener.pause = false;
        }



        SFXMixer.SetFloat("volume", sfxMixerValue);
    }

    public void TurnBGMOnOff()  /// THIS FUNCTION IS USED FOR BUTTONS ONLY DONT USE IT ELSE WHERE
    {
        if (isBGMOn)
        {

            // WHEN THE BUTTON IS CLICKED IF THE AUDIO WAS ON IT TURNS OFF
            BGMSpriteToUse = OnoffSprites[1];
            Bgmvalue = 0;
            bgmMixerValue = -80;
            isBGMOn = false;
          

        }
        else
        {
            // WHEN THE BUTTON IS CLICKED IF THE AUDIO WAS OFF IT TURNS ON
            BGMSpriteToUse = OnoffSprites[0];
            Bgmvalue = 1;
            bgmMixerValue = 0;
            isBGMOn = true;
          
        }


        BGMMixer.SetFloat("volume", bgmMixerValue);


    }

    public void SaveSettings()
    {
        PlayerPrefs.SetInt("SFXAudio", SfxValue);
        PlayerPrefs.SetInt("BGMAudio", Bgmvalue);
    }
}
