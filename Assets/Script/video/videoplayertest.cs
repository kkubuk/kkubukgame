using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class VideoTransitionController : MonoBehaviour
{
    [Header("1) VideoPlayer 컴포넌트")]
    public VideoPlayer videoPlayer;

    [Header("2) 비디오 재생 시 SoundManager로 재생할 AudioClip")]
    public AudioClip extraClip; // 인스펙터에서 원하는 사운드 클립을 드래그

    private void Start()
    {
        // ① 비디오 플레이어 할당 (인스펙터에 연결되지 않았다면 GetComponent로 가져오기)
        if (videoPlayer == null)
            videoPlayer = GetComponent<VideoPlayer>();

        // ② 비디오 재생 완료 시점 콜백 등록
        videoPlayer.loopPointReached += OnVideoFinished;

        // ③ 비디오 재생
        videoPlayer.Play();

        // ④ SoundManager로 효과음/배경음 재생
        //    SoundManager.instance.OnClickSound() 또는 플레이 목적에 맞는 메서드를 호출하세요.
        if (extraClip != null)
        {
            // OnClickSound 대신, 
            // 만약 SoundManager에 “PlayBgm”이나 “PlayOneShot” 같은 메서드가 있으면 그걸로 써도 됩니다.
            SoundManager.instance.OnClickSound(extraClip);
        }
    }

    private void OnVideoFinished(VideoPlayer vp)
    {
        // 1) VideoPlayer가 가지고 있는 내장 오디오는 따로 없으므로 Stop 호출 필요는 없지만,
        //    만약 SoundManager에서 재생 중인 AudioSource를 멈추려면 아래와 같이 호출할 수 있습니다.
        //    SoundManager.instance.StopSound();  // (SoundManager에 해당 기능이 있다면)

        // 2) TransitionManager에 저장된 최종 씬 이름을 가져와서 씬 전환
        string nextScene = TransitionManager.TargetSceneName;
        if (!string.IsNullOrEmpty(nextScene))
        {
            SceneManager.LoadScene(nextScene);
        }
        else
        {
            Debug.LogError("[VideoTransitionController] TargetSceneName이 설정되지 않았습니다!");
        }
    }
}
