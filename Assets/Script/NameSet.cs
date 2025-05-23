using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class NameSet : MonoBehaviour
{
    public TMP_InputField nicknameInputField;
    public GameObject nicknamePanel;
    public string nextSceneName = "MainGame";

    void Start()
    {
        nicknamePanel.SetActive(true);
    }

    public void OnConfirmButton()
    {
        string nickname = nicknameInputField.text;

        if (string.IsNullOrEmpty(nickname))
        {
            Debug.Log("닉네임을 입력하세요");
            return;
        }

        PlayerPrefs.SetString("Nickname", nickname);
        PlayerPrefs.Save();

        nicknamePanel.SetActive(false);

        // ✅ 페이드 아웃 후 전환
        FadeEffect.Instance.FadeToScene(nextSceneName);
    }
}