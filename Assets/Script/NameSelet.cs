using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

#nullable disable
public class NameSelet : MonoBehaviour
{
    public TMP_InputField playerNameInput;
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
        this.playerName = thist.playerNameInput.GetComponent<TMP_InputField>().text;
        this.fadepannel = GameObject.Find("Canvas").transform.Find("fade").gameObject;
        this.click1 = false;
        this.click2 = false;
        this.StartCoroutine(this.FadeIn())
    }

    // Update is called once per frame
    private void Update()
    {
        if (!playerNameInput.GetKeyDown(KeyCode.A) || !Input.GetKeyDown(KeyCode.LeftControl) || !((Object)this.playerNameInput != (Object)null))
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
            MonoBehaviour.print((object) "이름이 없나요?");

        }
    }
}
