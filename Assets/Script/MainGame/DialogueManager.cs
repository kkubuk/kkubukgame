using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public GameObject dialoguePanel;
    public RectTransform dialoguePanelRect; // 애니메이션용 RectTransform
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;
    public TextAsset csvFile;

    private Queue<string> sentences;
    private Queue<string> names;
    private bool isTyping = false;
    private string currentSentence = "";
    private string currentName = "";

    private bool hasOpened = false;
    private Vector2 hiddenPos = new Vector2(0, -300); // 패널 숨김 위치
    private Vector2 visiblePos = new Vector2(0, 20);   // 패널 보일 위치

    public Image characterImage;
    public List<CharacterImageData> characterImages;

    void Start()
    {
        sentences = new Queue<string>();
        names = new Queue<string>();
        dialoguePanel.SetActive(true); // 처음엔 활성화되지만 위치는 아래로
        dialoguePanelRect.anchoredPosition = hiddenPos;

        LoadCSV();
        StartCoroutine(StartDialogueAutomatically());
    }

    IEnumerator StartDialogueAutomatically()
    {
        yield return new WaitForSeconds(0.5f); // 선택적으로 약간 기다림
        yield return StartCoroutine(SlidePanelUp());
        hasOpened = true;
        DisplayNextSentence();
    }

    void LoadCSV()
    {
        string[] lines = csvFile.text.Split('\n');

        for (int i = 1; i < lines.Length; i++)
        {
            string[] fields = ParseCSVLine(lines[i]);
            if (fields.Length >= 2)
            {
                string rawName = fields[0].Trim().Replace("\"", "");
                string rawSentence = fields[1].Trim().Replace("\"", "");

                string name = rawName.Replace("{name}", Txt.playername);
                string sentence = rawSentence.Replace("{name}", Txt.playername);

                names.Enqueue(name);
                sentences.Enqueue(sentence);
            }
        }
    }

    void Update()
    {
        if (hasOpened && Input.GetMouseButtonDown(0) && dialoguePanel.activeSelf)
        {
            if (isTyping)
            {
                isTyping = false;
                dialogueText.text = currentSentence;
            }
            else
            {
                DisplayNextSentence();
            }
        }
    }

    public void DisplayNextSentence()
    {
        if (isTyping) return;

        if (sentences.Count == 0)
        {
            StartCoroutine(SlidePanelDown()); // 다 끝나면 패널 내리기
            return;
        }

        currentSentence = sentences.Dequeue();
        currentName = names.Dequeue();

        nameText.text = currentName;
        UpdateCharacterImage(currentName);
        StartCoroutine(TypeSentence(currentSentence));
    }

    IEnumerator TypeSentence(string sentence)
    {
        isTyping = true;
        dialogueText.text = "";

        sentence = sentence.Replace("{name}", Txt.playername);

        foreach (char letter in sentence.ToCharArray())
        {
            if (!isTyping)
            {
                dialogueText.text = sentence;
                yield break;
            }

            dialogueText.text += letter;
            yield return new WaitForSeconds(0.03f);
        }

        isTyping = false;
    }

    IEnumerator SlidePanelUp()
    {
        float t = 0f;
        float duration = 0.5f;
        Vector2 startPos = hiddenPos;
        Vector2 endPos = visiblePos;

        while (t < duration)
        {
            t += Time.deltaTime;
            dialoguePanelRect.anchoredPosition = Vector2.Lerp(startPos, endPos, t / duration);
            yield return null;
        }
        dialoguePanelRect.anchoredPosition = endPos;
    }

    IEnumerator SlidePanelDown()
    {
        float t = 0f;
        float duration = 0.5f;
        Vector2 startPos = visiblePos;
        Vector2 endPos = hiddenPos;

        while (t < duration)
        {
            t += Time.deltaTime;
            dialoguePanelRect.anchoredPosition = Vector2.Lerp(startPos, endPos, t / duration);
            yield return null;
        }
        dialoguePanelRect.anchoredPosition = endPos;
        dialoguePanel.SetActive(false);
    }

    string[] ParseCSVLine(string line)
    {
        List<string> result = new List<string>();
        bool inQuotes = false;
        string field = "";

        foreach (char c in line)
        {
            if (c == '"') inQuotes = !inQuotes;
            else if (c == ',' && !inQuotes)
            {
                result.Add(field);
                field = "";
            }
            else
            {
                field += c;
            }
        }
        result.Add(field);
        return result.ToArray();
    }

    void UpdateCharacterImage(string characterName)
    {
        foreach (CharacterImageData data in characterImages)
        {
            if (data.characterName == characterName)
            {
                characterImage.sprite = data.characterSprite;
                characterImage.color = new Color(1, 1, 1, 0); // 투명 시작
                StartCoroutine(FadeInCharacterImage());
                return;
            }
        }

        // 이름이 없으면 숨김
        characterImage.color = new Color(1, 1, 1, 0);
    }
    
    IEnumerator FadeInCharacterImage()
    {
        float duration = 0.5f;
        float t = 0f;

        while (t < duration)
        {
            t += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, t / duration);
            characterImage.color = new Color(1, 1, 1, alpha);
            yield return null;
        }
    }

}
