using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

#nullable disable
public class nameselect : MonoBehaviour
{
    public InputField playerNameInput;
    private string playerName;
    public GameObject background;
    private Image fadesr;
    private float ftime;
    private float time;
    public GameObject fadepannel;
    public GameObject helppannel;
    private bool click1;
    private bool click2;

    // Start is called before the first frame update
    private void Start()
    {
        this.playerName = this.playerNameInput.GetComponent<InputField>().text;
        this.fadepannel = GameObject.Find("Canvas").transform.Find("fade").gameObject;
        this.click1 = false;
        this.click2 = false;
        this.continueButton.SetActive(false);
        this.StartCoroutine(this.FadeIn());
    }

    // Update is called once per frame
    private void Update()
    {
        if (!Input.GetKeyDown(KeyCode.A) || !Input.GetKeyDown(KeyCode.LeftControl) || !((Object)this.playerNameInput != (Object)null))
            return;
        this.playerNameInput.text = "";
    }

    public void OCCB()
    {
        if (this.click1)
            return;
        this.click1 = true;
        this.playerName = this.playerNameInput.text;
        if (this.playerName.Length > 5)
            return;
        if (this.playerName.Length < 1)
        {
            MonoBehaviour.print((object)"이름이 없나요?");
            this.playerName = "황동우";
            Txt.playername = this.playerName;
            this.StartCoroutine(this.Next());
        }
        Txt.playername = this.playerName;
        this.StartCoroutine(this.Next());
    }

    public IEnumerator Next()
    {
        nameselect nameselect = this;
        nameselect.ftime = 0.7f;
        nameselect.time = 0.0f;
        nameselect.fadepannel.gameObject.SetActive(true);
        nameselect.fadepannel.transform.SetAsLastSibling();
        nameselect.fadesr = nameselect.fadepannel.GetComponent<Image>();
        Color alpha = nameselect.fadesr.color;
        while ((double)alpha.a < 1.0)
        {
            nameselect.time += Time.deltaTime / nameselect.ftime;
            alpha.a = Mathf.Lerp(0.0f, 1f, nameselect.time);
            nameselect.fadesr.color = alpha;
            yield return (object)null;
        }
        nameselect.StartCoroutine(Next2());
    }

    public GameObject nameSetPanel;
    public GameObject continueButton;
    public IEnumerator Next2()
    {
        this.ftime = 0.7f;
        this.time = 0.0f;
        nameSetPanel.SetActive(false);
        this.helppannel.gameObject.SetActive(true);
        this.helppannel.transform.SetAsLastSibling();
        this.fadesr = this.helppannel.GetComponent<Image>();
        Color alpha = this.fadesr.color;
        while ((double)alpha.a < 1.0)
        {
            this.time += Time.deltaTime / this.ftime;
            alpha.a = Mathf.Lerp(0.0f, 1f, this.time);
            this.fadesr.color = alpha;
            yield return (object)null;
        }
        continueButton.SetActive(true);
    }

    public void helpclick()
    {
        if (this.click2)
            return;
        this.click2 = true;
        this.StartCoroutine(this.FadeOut());
    }

    public IEnumerator FadeOut()
    {
        nameselect nameselect = this;
        nameselect.ftime = 0.7f;
        nameselect.time = 0.0f;
        nameselect.fadesr = this.fadepannel.GetComponent<Image>();
        Color alpha = this.fadesr.color;
        while ((double)alpha.a > 0.0)
        {
            nameselect.time += Time.deltaTime / nameselect.ftime;
            alpha.a = Mathf.Lerp(1f, 0.0f, nameselect.time);
            nameselect.fadesr.color = alpha;
            yield return (object)null;
        }
        nameselect.helppannel.gameObject.SetActive(false);
        yield return (object)null;
        nameselect.StartCoroutine(nameselect.scene());
    }

    public IEnumerator FadeIn()
    {
        this.ftime = 0.7f;
        this.time = 0.0f;
        this.fadesr = this.fadepannel.GetComponent<Image>();
        Color alpha = this.fadesr.color;
        while ((double)alpha.a > 0.0)
        {
            this.time += Time.deltaTime / this.ftime;
            alpha.a = Mathf.Lerp(1f, 0.0f, this.time);
            this.fadesr.color = alpha;
            yield return (object)null;
        }
        this.fadepannel.gameObject.SetActive(false);
        yield return (object)null;
    }

    public IEnumerator scene()
    {
        SceneManager.LoadScene("MainGame");
        yield return (object)null;
    }

}
