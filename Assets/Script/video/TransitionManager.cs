// 파일명: TransitionManager.cs
using UnityEngine;
using UnityEngine.SceneManagement;

public static class TransitionManager
{
    // 비디오 씬 종료 후 로드할 실제 씬 이름을 저장
    public static string TargetSceneName;

    /// <summary>
    /// 비디오 전환 씬을 로드하기 전에 
    /// TargetSceneName에 실제로 넘어갈 씬 이름을 설정한 뒤, 비디오 씬으로 전환한다.
    /// </summary>
    public static void LoadSceneWithVideo(string nextScene)
    {
        // 1) 전환할 실제 씬 이름 저장
        TargetSceneName = nextScene;

        // 2) 비디오 전환 전용 씬을 불러옴
        //    (Build Settings > Scenes In Build에 "VideoTransitionScene"이 들어 있어야 함)
        SceneManager.LoadScene("Vidionext");
    }
}
