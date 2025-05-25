using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonCont : MonoBehaviour
{

    public Text ResolutionText;

    public Button Fullbtn;
    public Button Windowbtn;

    public Sprite Sprite;
    public Sprite Sprite2;
    public Sprite ChangeSprite;
    public Sprite ChangeSprite2;

    private int i = 5;
    private string[] ResolutionArray = new string[] {"640 X 360", "960 X 540", "1280 X 720", "1366 X 768", "1600 X 900", "1920 X 1080"};
    public void RightArrowClick()
    {
            if(i < 5)
            {
                i++;
                ResolutionText.GetComponent<Text>().text = ResolutionArray[i];
            }
    }

    public void LeftArrowClick()
    {
        if (i > 0)
        {

            i--;
            ResolutionText.GetComponent<Text>().text = ResolutionArray[i];
        }
    }

    public void FullScreenClick()
    {
        this.Fullbtn.GetComponent<Image>().sprite = this.Sprite;
        this.Windowbtn.GetComponent<Image>().sprite = this.Sprite2;
        Screen.fullScreen = true;
    }

    public void WindowedClick()
    {
        this.Fullbtn.GetComponent<Image>().sprite = this.ChangeSprite;
        this.Windowbtn.GetComponent<Image>().sprite = this.ChangeSprite2;
        Screen.fullScreen = false;
    }

    public void ApplyResolution()
    {
        string current = ResolutionArray[i];
        string width = current.Split(" X ")[0];
        string height = current.Split(" X ")[1];

        Screen.SetResolution(Int32.Parse(width), Int32.Parse(height), Screen.fullScreen);
    }
}