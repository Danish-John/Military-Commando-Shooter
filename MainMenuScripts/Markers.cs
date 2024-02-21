using UnityEngine.UI;
using UnityEngine;

public class Markers : MonoBehaviour
{
    public GameObject TopMarkers;
    public GameObject LockImage;
    public LevelSelection Selection;
    private Image MarkerImage;
    [HideInInspector] public int LevelNumber;


    private void Awake()
    {
        MarkerImage = GetComponent<Image>();
      
    }
    public void LockOrUnlock(bool isLocked)
    {
        if(isLocked)
        {
            TopMarkers.SetActive(false);
        }
        else
        {
          
            MarkerImage.sprite = Selection.clearedImage;
            LockImage.SetActive(false);
        }
    }
}
