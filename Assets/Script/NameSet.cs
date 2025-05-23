using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
public class NameSet : MonoBehaviour
{
    public TMP_InputField nicknameInputField; // 또는 TMP_InputField 사용 가능
    public GameObject nicknamePanel;      // 닉네임 UI 전체 패널
    public GameObject fadeManager;        // 페이드 이미지가 있는 오브젝트
    public FadeEffect fadeEffect;         // 페이드 효과 스크립트
    public string nextSceneName = "MainGame";

    public void OnConfirmNickname()
    {
        if (nicknameInputField == null)
        {
            Debug.LogError("nicknameInputField가 연결되지 않았습니다!");
            return;
        }

        string nickname = nicknameInputField.text;

        if (string.IsNullOrEmpty(nickname))
        {
            Debug.Log("닉네임을 입력하세요.");
            return;
        }

        PlayerPrefs.SetString("Nickname", nickname);
        PlayerPrefs.Save();

        nicknamePanel.SetActive(false); // UI 닫기

        if (fadeManager != null && !fadeManager.activeInHierarchy)
        {
            fadeManager.SetActive(true); // 꺼져 있다면 켜주기
        }

        if (fadeEffect != null)
        {
            fadeEffect.FadeToScene(nextSceneName, 0);
        }
        else
        {
            Debug.LogError("fadeEffect가 연결되지 않았습니다!");
        }
        Debug.Log("fadeEffect is null? " + (fadeEffect == null));
    }
}
