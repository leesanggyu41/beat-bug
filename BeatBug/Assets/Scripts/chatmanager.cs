using UnityEngine;
using TMPro;
using DG.Tweening;
using System.Collections.Generic;

public class chatmanager : MonoBehaviour
{
    [Header("player")]
    public Circlerotate circlerotate;

    [Header("UI Elements")]
    public CanvasGroup dialoguePanel;
    
    public TMP_Text dialogueText;

    [Header("Settings")]
    public float fadeDuration = 0.5f;
    public float typingSpeed = 0.03f;

    [System.Serializable]
    public class DialogueCut
    {

        [TextArea(2, 5)] public string dialogueLine;
    }

    [Header("Dialogue Data")]
    public List<DialogueCut> dialogueCuts = new List<DialogueCut>();

    private int currentCutIndex = 0;
    private Tween typingTween;
    private bool isTyping = false;

    void Start()
    {
        dialoguePanel.alpha = 0;
        dialoguePanel.gameObject.SetActive(false);
    }

    public void StartDialogue()
    {
        currentCutIndex = 0;
        dialoguePanel.gameObject.SetActive(true);
        dialoguePanel.DOFade(1, fadeDuration).OnComplete(() =>
        {
            ShowCut(currentCutIndex);
        });
    }

    public void ShowCut(int index)
    {
        if (index >= dialogueCuts.Count)
        {
            EndDialogue();
            return;
        }

        DialogueCut cut = dialogueCuts[index];
        
        StartTyping(cut.dialogueLine);
    }

    void StartTyping(string text)
    {
        isTyping = true;
        dialogueText.text = "";
        typingTween?.Kill();

        typingTween = DOVirtual.Float(0, text.Length, text.Length * typingSpeed, (value) =>
        {
            int count = Mathf.FloorToInt(value);
            dialogueText.text = text.Substring(0, count);
        }).OnComplete(() =>
        {
            isTyping = false;
        });
    }

    public void NextCut()
    {
        if (isTyping)
        {
            // 타이핑 중일 때는 전체 문장 즉시 표시
            typingTween.Kill();
            dialogueText.text = dialogueCuts[currentCutIndex].dialogueLine;
            isTyping = false;
            return;
        }

        currentCutIndex++;
        ShowCut(currentCutIndex);
    }

    void EndDialogue()
    {
        dialoguePanel.DOFade(0, fadeDuration)
            .OnComplete(() => dialoguePanel.gameObject.SetActive(false));

        //이제 게임을 시작하라고 알림
        Invoke("isppp", 2);


    }

    public void isppp()
    {
        circlerotate.isplayy();
    }
}
