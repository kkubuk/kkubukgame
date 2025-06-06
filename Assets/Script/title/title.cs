using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class title : MonoBehaviour
{
    public AudioClip clip;

    // Start에서는 더 이상 fadepannel을 찾지 않으므로 빈 채워둡니다.
    private void Start()
    {
        // (이전에는 페이드 패널을 찾았지만 지금은 사용하지 않습니다.)
    }

    // 버튼 클릭 시 호출되는 함수
    public void OCB()
    {
        // 클릭 사운드는 재생하고
        SoundManager.instance.OnClickSound(clip);

        // 곧바로 VideoTransitionScene → InGame 으로 넘어가도록 호출
        TransitionManager.LoadSceneWithVideo("InGame");
    }

    // Update는 사용되지 않으므로 비워둡니다.
    void Update() { }

    public void Exit()
    {
        Application.Quit();
    }
}
