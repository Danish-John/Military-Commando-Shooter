using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


namespace CoverShooter {

    public class HealthScript : MonoBehaviour
    {
        public Slider HealthBar;

        public GameObject Player;

        private LevelData Data;

        bool fail;

        public Image HitImage;

        public GameObject Controls;
        public GameObject SpawnStarter;

        void Start()
        {
            fail = false;
            Player.GetComponent<Rigidbody>().mass = 1000;
        }

        // Update is called once per frame
        void Update()
        {
            if (!fail)
            {
                if (HealthBar.value == 0)
                {
                    GameManagerZombiesMode.instance.TotalCoinText.text = GameManagerZombiesMode.instance.Data.gameData.PlayerCoins.ToString("n0");
                    Controls.SetActive(false);
                    this.gameObject.GetComponent<AKMGun>().enabled = false;
                    fail = true;
                    //#if !UNITY_EDITOR
                    //FirebaseManager.Instance.LogEvent("Zombie_Lvl_Fail_" + (Data.CurrentLevel + 1)); 
                    //#endif
                    // InGameUi.instance.ScreenToShow = InGameUi.ScreenType.Defeat; // defeat show


                    StartCoroutine(InGameUi.instance.ShowDefeatVictoryText(false)); // defeat panel show
                    GameManagerZombiesMode.instance.StartScoreCounter(false);
                    InGameUi.instance.LoseTargetCount.text = "" + (GameManagerZombiesMode.instance.Enemie_Counter * 10);
//#if !UNITY_EDITOR
        AdsManger_New.Instance.Invoke("ShowUnityAdAdmobWithAdBreak", 5f);
//#endif
                }
            }
        }



        private void OnTriggerStay(Collider other)
        {
            if (other.tag == "Enemy" || other.tag == "RunningEnemy" && other.transform.GetComponentInParent<CharacterHealth>()._isDead == false)
            {
                
                HealthBar.value -= 0.002f;
                HitImage.DOFade(0.7f, 0.3f).OnComplete(() =>
                {
                    HitImage.DOFade(0, 0.3f);
                });
            }
            else
                return;
        }



        private void OnEnable()
        {
            SpawnStarter.SetActive(true);
        }


    }





}


