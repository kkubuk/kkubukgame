using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FadeEffect : MonoBehaviour
{
    public static FadeEffect Instance;
    public GameObject fadeobj;      // 검정색 이미지
    public float fadeSpeed = 1.5f;

    public GameObject fadeManager;

    void Start()
    {
        
    }


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        if (!fadeManager.gameObject.activeSelf)
            fadeManager.gameObject.SetActive(true);
    }

    public void FadeToScene(string sceneName, int fadeindex)
    {
        switch (fadeindex)
        {
            case 0:
                StartCoroutine(FadeIn(sceneName));
                break;
            case 1:
                StartCoroutine(FadeOut(sceneName));
                break;
            
        }
    }

    IEnumerator FadeIn(string sceneName)
    {
        Image fadeimage = fadeobj.GetComponent<Image>();
        float alpha = 1f;
        while (alpha > 0f)
        {
            alpha -= Time.deltaTime * fadeSpeed;
            fadeimage.color = new Color(0, 0, 0, alpha);
            yield return null;
        }

        SceneManager.LoadScene(sceneName);
    }

    IEnumerator FadeOut(string sceneName)
    {
        Image fadeimage = fadeobj.GetComponent<Image>();
        float alpha = 0f;
        while (alpha < 1f)
        {
            alpha += Time.deltaTime * fadeSpeed;
            fadeimage.color = new Color(0, 0, 0, alpha);
            yield return null;
        }

        SceneManager.LoadScene(sceneName);
    }
}

