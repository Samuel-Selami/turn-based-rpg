using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DialogueUI : MonoBehaviour
{
    [Header("Dialogue Attributes")]
    [SerializeField] Image rightPortrait;
    [SerializeField] Image leftPortrait;
    [SerializeField] TMP_Text speakerName;
    [SerializeField] TMP_Text dialogueArea;
    DialogueData currentDialogue;
    public bool isDialoguePlaying;
    [SerializeField] float portraitScaleSpeed = 10f;
    Vector3 rightPortraitScale = Vector3.one;
    Vector3 leftPortraitScale = Vector3.one;
    [SerializeField] float letterDelay = 0.02f;
    int index;
    float gamePause = 0f;
    float gameResume = 1f;
    Coroutine typeRoutine;



    void Start()
    {

    }

    void Update()
    {
        rightPortrait.transform.localScale = Vector3.Lerp(rightPortrait.transform.localScale, rightPortraitScale, portraitScaleSpeed * Time.unscaledDeltaTime);
        leftPortrait.transform.localScale = Vector3.Lerp(leftPortrait.transform.localScale, leftPortraitScale, portraitScaleSpeed * Time.unscaledDeltaTime);

        if (!isDialoguePlaying)
            return;

        if (BattleInput.Confirm())
        {
            Next();
        }
    }

    public void Play(DialogueData data)
    {
        currentDialogue = data;
        index = 0;
        isDialoguePlaying = true;
        Time.timeScale = gamePause;
        gameObject.SetActive(true);
        ShowLine();
    }

    public void Next()
    {
        index++;

        if (index >= currentDialogue.lines.Count)
        {
            EndDialogue();
        }
        else
        {
            ShowLine();
        }
    }
    public void EndDialogue()
    {
        isDialoguePlaying=false;
        gameObject.SetActive(false);
        Time.timeScale = gameResume;
    }
    void ShowLine()
    {
        DialogueLine line = currentDialogue.lines[index];
        speakerName.text = line.speakerName;

        if (typeRoutine != null)
            StopCoroutine(typeRoutine);

        typeRoutine = StartCoroutine(TypeText(line.text));

        if (line.isplayerSide)
        {
            rightPortrait.color = Color.white;

            rightPortraitScale = Vector3.one * 1.2f;
            leftPortrait.color = Color.gray;
            leftPortraitScale = Vector3.one;
        }
        else
        {
            rightPortrait.color = Color.gray;
            rightPortraitScale = Vector3.one;
            leftPortrait.color = Color.white;
            leftPortraitScale = Vector3.one * 1.2f;
        }
    }

    IEnumerator TypeText(string fullText)
    {
        for (int i = 0; i < fullText.Length; i++)
        {
            dialogueArea.text = fullText.Substring(0, i + 1);
            yield return new WaitForSecondsRealtime(letterDelay);
        }
    }



}
