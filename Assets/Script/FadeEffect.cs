using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FadeEffect : MonoBehaviour
{
    public static FadeEffect Instance;
    public Image fadeImage;
    public float fadeSpeed = 1.5f;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // 처음에는 검은 화면에서 시작
        if (fadeImage != null)
            fadeImage.color = new Color(0, 0, 0, 1);
        StartCoroutine(FadeIn());
    }

    public void FadeToScene(string sceneName)
    {
        StartCoroutine(FadeOut(sceneName));
    }

    IEnumerator FadeOut(string sceneName)
    {
        float alpha = 0f;
        while (alpha < 1f)
        {
            alpha += Time.deltaTime * fadeSpeed;
            if (fadeImage != null)
                fadeImage.color = new Color(0, 0, 0, alpha);
            yield return null;
        }

        SceneManager.LoadScene(sceneName);
    }
    IEnumerator FadeIn()
    {
        float alpha = 1f;
        if (fadeImage != null)
            fadeImage.color = new Color(0, 0, 0, alpha);

        while (alpha > 0f)
        {
            alpha -= Time.deltaTime * fadeSpeed;
            if (fadeImage != null)
                fadeImage.color = new Color(0, 0, 0, alpha);
            yield return null;
        }
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(FadeIn());
    }

    
}
