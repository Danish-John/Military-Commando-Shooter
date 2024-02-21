using UnityEngine.Events;
using UnityEngine;

public class LevelStarter : MonoBehaviour
{
    public UnityEvent OnPlayerEnter;
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            gameObject.SetActive(false);
            GameManagerWeaponsMode.instance.CF2Rig.SetActive(true);
            InGameUi.instance.GameplayHUD.SetActive(true);
            OnPlayerEnter.Invoke();
        }
    }
}
