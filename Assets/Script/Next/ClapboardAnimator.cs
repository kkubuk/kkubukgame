using UnityEngine;
using DG.Tweening;

public class SlateDropBothAndMoveBackgroundsSynced : MonoBehaviour
{
    [Header("1) References")]
    [Tooltip("슬레이트 윗판(ClapHinge) RectTransform")]
    public RectTransform clapHinge;

    [Tooltip("슬레이트 밑판(ClapBase) RectTransform")]
    public RectTransform clapBase;

    [Tooltip("Background1 RectTransform (좌측 상단 바깥에서 중앙으로 이동)")]
    public RectTransform background1;

    [Tooltip("Background2 RectTransform (우측 하단 바깥에서 중앙으로 이동)")]
    public RectTransform background2;

    [Header("2) Positions (Anchored)")]
    [Tooltip("ClapHinge 처음 화면 밖(좌측 상단)에 위치")]
    public Vector2 hingeOffscreenPos = new Vector2(-800, 600);

    [Tooltip("ClapHinge 바닥에 닿을 위치")]
    public Vector2 hingeGroundPos = new Vector2(0, -400);

    [Tooltip("ClapHinge 바닥에서 튀어 올라올 최종 위치")]
    public Vector2 hingeHitPos = new Vector2(0, 0);

    [Tooltip("ClapBase 처음 화면 밖(좌측 상단)에 위치")]
    public Vector2 baseOffscreenPos = new Vector2(-800, 800);

    [Tooltip("ClapBase 바닥에 머무를 위치")]
    public Vector2 baseGroundPos = new Vector2(0, -450);

    [Tooltip("Background1 처음 화면 밖(좌측 상단) 위치")]
    public Vector2 bg1OffscreenPos = new Vector2(-1000, 700);

    [Tooltip("Background2 처음 화면 밖(우측 하단) 위치")]
    public Vector2 bg2OffscreenPos = new Vector2(1000, -700);

    [Header("3) Timing Settings")]
    [Tooltip("ClapHinge/ClapBase 대각선 낙하 시간 (초)")]
    public float dropDuration = 0.8f;

    [Tooltip("ClapHinge 바닥에서 튀어오르는 시간 (초)")]
    public float bounceDuration = 0.4f;

    [Tooltip("ClapHinge 닫히는 회전 시간 (초)")]
    public float hingeCloseDuration = 0.6f;

    [Tooltip("Background들이 중앙으로 이동하는 시간 (초)")]
    public float bgMoveDuration = 1.0f;

    [Tooltip("슬레이트가 작아지며 사라지는 시간 (초)")]
    public float shrinkDuration = 0.5f;

    [Header("4) Angles & Scales")]
    [Tooltip("ClapHinge 첫 회전 각도 (degree)")]
    public float hingeStartAngle = 45f;

    [Tooltip("ClapHinge 초기 스케일 (예: 1 = 100%)")]
    public float hingeStartScale = 1f;

    [Tooltip("ClapBase 초기 스케일 (예: 1 = 100%)")]
    public float baseStartScale = 1f;

    [Header("5) Sound Clips")]
    [Tooltip("ClapHinge가 바닥에 닿을 때 재생할 효과음")]
    public AudioClip hitClip;

    [Tooltip("ClapHinge가 닫힐 때 재생할 효과음")]
    public AudioClip closeClip;

    [Tooltip("슬레이트가 작아지며 사라질 때 재생할 효과음 (Optional)")]
    public AudioClip shrinkClip;

    [Tooltip("배경이 offscreen으로 돌아갈 때 재생할 효과음 (Optional)")]
    public AudioClip bgReturnClip;


    private void Start()
    {
        // 0) SoundManager 체크
        if (SoundManager.instance == null)
        {
            Debug.LogError("씬에 SoundManager가 없습니다!");
            return;
        }

        // 1) ClapHinge 초기화 (offscreen 위치, 스케일, 회전)
        if (clapHinge != null)
        {
            clapHinge.anchoredPosition = hingeOffscreenPos;
            clapHinge.localScale       = Vector3.one * hingeStartScale;
            clapHinge.localRotation    = Quaternion.Euler(0f, 0f, hingeStartAngle);
        }

        // 2) ClapBase 초기화 (offscreen 위치, 스케일)
        if (clapBase != null)
        {
            clapBase.anchoredPosition = baseOffscreenPos;
            clapBase.localScale       = Vector3.one * baseStartScale;
            clapBase.localRotation    = Quaternion.identity;
        }

        // 3) Background1 초기화 (offscreen 좌측 상단), Background2 초기화 (offscreen 우측 하단)
        if (background1 != null)
        {
            background1.anchoredPosition = bg1OffscreenPos;
            background1.localScale       = Vector3.one; // 크기는 100%
        }
        if (background2 != null)
        {
            background2.anchoredPosition = bg2OffscreenPos;
            background2.localScale       = Vector3.one;
        }

        // 4) 전체 시퀀스 실행
        PlayFullSequence();
    }

    private void PlayFullSequence()
    {
        Sequence seq = DOTween.Sequence();

        // --- Step A: ClapHinge & ClapBase 대각선 낙하 ---
        seq.Append(
            DOTween.Sequence()
                .Append(clapHinge
                    .DOAnchorPos(hingeGroundPos, dropDuration)
                    .SetEase(Ease.InQuad)
                )
                .Join(clapBase
                    .DOAnchorPos(baseGroundPos, dropDuration)
                    .SetEase(Ease.InQuad)
                )
        );

        // --- Step B: ClapHinge 바닥에서 Bounce ---
        seq.Append(
            clapHinge
                .DOAnchorPos(hingeHitPos, bounceDuration)
                .SetEase(Ease.OutBounce)
                .OnStart(() =>
                {
                    if (hitClip != null)
                        SoundManager.instance.UIPlay("SlateHit", hitClip);
                })
        );

        // --- Step C: ClapHinge 회전 닫기(“탁”) & Background1/2 중앙 이동 ---
        seq.Append(
            DOTween.Sequence()
                // ClapHinge 회전
                .Append(clapHinge
                    .DOLocalRotate(Vector3.zero, hingeCloseDuration)
                    .SetEase(Ease.OutBack)
                    .OnStart(() =>
                    {
                        if (closeClip != null)
                            SoundManager.instance.UIPlay("ClapClose", closeClip);
                    })
                )
                // Background1 중앙 이동
                .Join(background1 != null
                    ? background1
                        .DOAnchorPos(Vector2.zero, bgMoveDuration)
                        .SetEase(Ease.OutQuad)
                    : null
                )
                // Background2 중앙 이동
                .Join(background2 != null
                    ? background2
                        .DOAnchorPos(Vector2.zero, bgMoveDuration)
                        .SetEase(Ease.OutQuad)
                    : null
                )
        );

        // --- Step D: 슬레이트 작아지며 사라짐 & Background1/2 다시 offscreen으로 이동 ---
        seq.AppendCallback(() =>
        {
            // 슬레이트 작아지기 사운드 (Optional)
            if (shrinkClip != null)
                SoundManager.instance.UIPlay("SlateShrink", shrinkClip);
        });

        seq.Append(
            DOTween.Sequence()
                // ClapHinge: 현재 크기 → 0
                .Append(clapHinge
                    .DOScale(Vector3.zero, shrinkDuration)
                    .SetEase(Ease.InQuad)
                )
                // ClapBase: 현재 크기 → 0
                .Join(clapBase
                    .DOScale(Vector3.zero, shrinkDuration)
                    .SetEase(Ease.InQuad)
                )
                // Background1: 중앙 → offscreen 좌측 상단
                .Join(background1 != null
                    ? background1
                        .DOAnchorPos(bg1OffscreenPos, bgMoveDuration)
                        .SetEase(Ease.InQuad)
                    : null
                )
                // Background2: 중앙 → offscreen 우측 하단
                .Join(background2 != null
                    ? background2
                        .DOAnchorPos(bg2OffscreenPos, bgMoveDuration)
                        .SetEase(Ease.InQuad)
                    : null
                )
        ).OnStart(() =>
        {
            // Background 돌아가는 사운드 (Optional)
            if (bgReturnClip != null)
                SoundManager.instance.UIPlay("BgReturn", bgReturnClip);
        });

        // --- 전체 연출 완료 콜백 ---
        seq.OnComplete(() =>
        {
            Debug.Log("슬레이트 낙하→튕김→닫힘 + 배경1/2 중앙 이동 → 슬레이트 사라짐 + 배경원위치 이동 모두 완료!");
            // 필요 시 씬 전환 등 추가 로직
            // SceneManager.LoadScene("NextScene");
        });
    }
}
