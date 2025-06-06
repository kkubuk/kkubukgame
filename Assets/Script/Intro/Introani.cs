using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;    // Image 컴포넌트를 사용하려면 반드시 추가해야 합니다.
using DG.Tweening;

public class Introani : MonoBehaviour
{
    [Header("Intro 스케일 애니메이션 설정")]
    public float minScale = 0.5f;         // 처음 오브젝트를 원래 크기의 몇 배로 작게 보낼지
    public float expandDuration = 0.3f;   // 작아진 상태 → 원래 크기로 커지는 데 걸리는 시간 (초)
    public int loopCount = 1;             // Yoyo 반복 횟수 (1이면 한 사이클만 재생)

    [Header("애니메이션이 끝난 뒤 대기 시간 (초)")]
    public float delayBeforeFade = 0.5f;  // Intro 애니메이션이 끝나고, 페이드가 시작되기 전 대기할 시간

    [Header("페이드 아웃에 걸릴 시간 (초)")]
    public float fadeOutDuration = 0.7f;  

    [Header("Intro 애니메이션이 끝난 뒤 로드할 씬 이름")]
    public string mainSceneName = "Main"; 

    [Header("페이드에 사용할 검은 패널 Image (인스펙터 연결)")]
    public Image blackPanelImage;         

    // 내부에서 사용할 변수들
    private AudioSource audioSource;
    private bool hasPlayedSound = false;
    private bool isFading = false;

    private void Awake()
    {
        // AudioSource 컴포넌트를 가져옵니다. (이 스크립트가 붙은 오브젝트에 AudioSource가 있어야 함)
        audioSource = GetComponent<AudioSource>();

        // blackPanelImage가 Inspector에서 연결되어 있는지 체크
        if (blackPanelImage == null)
        {
            Debug.LogError("[Introani] blackPanelImage가 할당되지 않았습니다. 인스펙터에서 연결해주세요.");
            return;
        }

        // 씬이 시작될 때 BlackPanel은 투명 상태(A=0)여야 하므로 강제로 세팅
        Color c = blackPanelImage.color;
        c.a = 0f;
        blackPanelImage.color = c;
        // BlackPanel 오브젝트는 활성화된 상태여도, Alpha가 0이므로 “실제로는 보이지 않는” 상태가 됩니다.
    }

    private void Start()
    {
        // 1) 현재 Transform의 원래 크기를 저장
        Vector3 originalScale = transform.localScale;

        // 2) 씬 진입 시 오브젝트를 minScale 만큼 작아진 상태로 세팅
        transform.localScale = originalScale * minScale;

        // 3) DOTween Sequence를 만들어서 “작아진 상태 → 원래 크기” 효과를 준 뒤, Yoyo로 한 사이클 실행
        Sequence seq = DOTween.Sequence();

        // 3-1) Scale Tween: minScale → originalScale
        Tween scaleTween = transform
            .DOScale(originalScale, expandDuration)
            .SetEase(Ease.OutQuad)
            // OnStart: Tween이 재생되기 시작할 때(커지기 직전) 효과음을 한 번 재생
            .OnStart(() =>
            {
                if (!hasPlayedSound && audioSource != null)
                {
                    audioSource.Play();
                    hasPlayedSound = true;
                }
            });

        // 3-2) Sequence에 스케일 트윈 추가
        seq.Append(scaleTween);

        // 3-3) Yoyo 반복 횟수 설정 (min → original → min)
        seq.SetLoops(loopCount, LoopType.Yoyo);
        //    loopCount = 1이면: minScale → originalScale → minScale (한 사이클만)
        //    loopCount = -1이면: 무한 반복 (min → orig → min → orig → ...)

        // 3-4) Sequence 전체가 끝나면(스케일 + Yoyo가 모두 완료) 호출되는 콜백
        seq.OnComplete(() =>
        {
            // Intro 애니메이션이 끝난 순간에, "delayBeforeFade" 초 만큼 기다린 뒤 실제 페이드 아웃 시작
            StartCoroutine(WaitAndFade());
        });
    }

    // 4) Intro 애니메이션이 끝난 뒤, delayBeforeFade만큼 기다렸다가 FadeOutAndLoad() 호출
    private IEnumerator WaitAndFade()
    {
        // Intro가 끝나고 바로 페이드 아웃을 시작하지 말고, delayBeforeFade 초 만큼 대기
        yield return new WaitForSeconds(delayBeforeFade);

        // 2초(예) 대기 후, 실제로 페이드 아웃을 진행
        StartCoroutine(FadeOutAndLoad());
    }

    // 5) 실제 “검은 화면으로 페이드 아웃 → Main 씬 로드” 코루틴
    private IEnumerator FadeOutAndLoad()
    {
        if (isFading) yield break;  // 이미 페이드 중이면 중복 실행 방지
        isFading = true;

        // BlackPanel이 UI 상에서 가장 위(Z 순서)로 오도록 설정 (다른 UI보다 앞에 표시하기 위함)
        blackPanelImage.transform.SetAsLastSibling();

        // ① Image 알파를 현재 0에서 1로 바꿔가며 페이드 아웃
        float elapsed = 0f;
        Color c = blackPanelImage.color;
        while (elapsed < fadeOutDuration)
        {
            elapsed += Time.deltaTime;
            float a = Mathf.Clamp01(elapsed / fadeOutDuration);
            c.a = a;
            blackPanelImage.color = c;
            yield return null;
        }

        // 확실히 알파를 1(완전 검정)으로 맞춘 뒤
        c.a = 1f;
        blackPanelImage.color = c;

        // ② 검은 화면이 다 덮이면, Main 씬으로 이동
        SceneManager.LoadScene(mainSceneName);
    }
}
