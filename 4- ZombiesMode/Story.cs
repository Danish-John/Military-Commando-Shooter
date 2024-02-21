using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Story : MonoBehaviour
{
    public Text StoryText;
    public string Text;

    private void Awake()
    {
        StoryText.text = Text;
    }

}
