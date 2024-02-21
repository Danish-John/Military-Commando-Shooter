using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;


public class WindBehaviour : MonoBehaviour
{
    private string[] windDirection = { "LeftUp", "LeftDown", "RightUp", "RightDown"};
    public float[] WindAmount;
    private float currentWind;
    private string currentWindDirection;
    public Text WindIntensityText;
    public GameObject [] ArrowImages; 
    public Vector3 RaycastPosition;
    public DOTweenAnimation WindAnim;
  
    public void ChangeWind()
    {
        WindAnim.DORestart();
        int windAmountIndex = Random.Range(0, WindAmount.Length);
        int windDirectionIndex= Random.Range(0, windDirection.Length);
        currentWind = WindAmount[windAmountIndex];
        currentWindDirection = windDirection[windDirectionIndex];
        RaycastPosition = new Vector3(0.5f,0.5f);

        WindIntensityText.text = currentWind.ToString();

        for (int i=0;i<ArrowImages.Length;i++)
        {
            if(i== windDirectionIndex)
            {
                ArrowImages[i].SetActive(true);
            }
            else
            {
                ArrowImages[i].SetActive(false);
            }
        }
        switch (currentWindDirection)
        {
            case "LeftUp":
                RaycastPosition.x = RaycastPosition.x - currentWind;
                RaycastPosition.y = RaycastPosition.y + currentWind;
              
                break;

            case "LeftDown":
                RaycastPosition.x = RaycastPosition.x - currentWind;
                RaycastPosition.y = RaycastPosition.y - currentWind;
               
                break;

            case "RightUp":
                RaycastPosition.x = RaycastPosition.x + currentWind;
                RaycastPosition.y = RaycastPosition.y + currentWind;
                
                break;

            case "RightDown":
                RaycastPosition.x = RaycastPosition.x + currentWind;
                RaycastPosition.y = RaycastPosition.y - currentWind;
               
                break;
        }
    }
}
