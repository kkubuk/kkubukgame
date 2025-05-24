using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

#nullable disable
public class title : MonoBehaviour
{
    public AudioClip clip;
    private Image fadesr;
    private float ftime;
    private float time;
    public GameObject fadepannel;

    // Start is called before the first frame update
    private void Start()
    {
        this.fadepannel = GameObject.Find("Canvas").transform.Find("fade").gameObject;
    }

    public void OCB()
    {
        UiSoundPlay();
        this.StartCoroutine(this.FadeOut());
        
    } 

    public IEnumerator FadeOut()
    {
        title title = this;
        title.ftime = 0.7f;
        title.time = 0.0f;
        title.fadepannel.gameObject.SetActive(true);
        title.fadepannel.transform.SetAsLastSibling();
        title.fadesr = title.fadepannel.GetComponent<Image>();
        Color alpha = title.fadesr.color;
        while ((double)alpha.a < 1.0)
        {
            title.time += Time.deltaTime / title.ftime;
            alpha.a = Mathf.Lerp(0.0f, 1f, title.time);
            title.fadesr.color = alpha;
            yield return (object)null;
        }
        title.StartCoroutine(title.scene());
    }

    public IEnumerator scene()
    {
        SceneManager.LoadScene("InGame");
        yield return (object)null;
    }
    // Update is called once per frame
    void Update()
    {

    }
    

    public void UiSoundPlay()
    {
        SoundManager.instance.UIPlay("UiClik", clip);
    }
}
