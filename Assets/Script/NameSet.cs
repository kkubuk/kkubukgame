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
    public GameObject fadePanel; // ✅ 추가
    private Image fadeImage;

    void Start()
    {
        nicknamePanel.SetActive(true);
        fadeImage = fadePanel.GetComponent<Image>();
    }

    public void OnConfirmButton()
    {
        if (nicknameInputField == null) return;

        string nickname = nicknameInputField.text;

        if (string.IsNullOrEmpty(nickname))
        {
            Debug.Log("닉네임을 입력하세요");
            return;
        }

        PlayerPrefs.SetString("Nickname", nickname);
        PlayerPrefs.Save();

        nicknamePanel.SetActive(false);
        StartCoroutine(FadeOutAndLoad(nextSceneName));
    }

    private IEnumerator FadeOutAndLoad(string sceneName)
    {
        fadePanel.SetActive(true);
        Color alpha = fadeImage.color;
        float time = 0f;
        float duration = 0.7f;

        while (alpha.a < 1f)
        {
            time += Time.deltaTime / duration;
            alpha.a = Mathf.Lerp(0f, 1f, time);
            fadeImage.color = alpha;
            yield return null;
        }

        SceneManager.LoadScene(sceneName);
    }
}
