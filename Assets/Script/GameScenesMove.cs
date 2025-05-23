using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameScenesMove : MonoBehaviour
{
    public FadeEffect fadeEffect;

    public void GameScenesCtrl()
    {
        fadeEffect.FadeToScene("InGame");
    }
}