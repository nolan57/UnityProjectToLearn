using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Speech : MonoBehaviour
{
    [SerializeField]private Canvas canvas;
    [SerializeField]private TextMeshPro textMeshPro;
    private CanvasGroup canvasGroup;
    public float fadeInDuration = 2.0f;
    public float fadeOutDuration = 2.0f;
    private float timerOfIn = 0.0f;
    private float timerOfOut = 0.0f;
    private bool isFadingIn = false;
    private bool isFadingOut = false;
    // Start is called before the first frame update
    void Start()
    {
        transform.Rotate(0, 180, 0);
        canvas.enabled = false;
        canvasGroup = canvas.gameObject.GetComponent<CanvasGroup>();
        if (canvasGroup == null){
            Debug.Log("No Canvas Group!");
            return;
        }
        canvasGroup.alpha = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (isFadingIn)
        {
            timerOfIn = Mathf.Clamp(timerOfIn + Time.deltaTime, 0.0f, fadeInDuration);
            float alpha = timerOfIn / fadeInDuration;
            canvasGroup.alpha = alpha;

            if (timerOfIn >= fadeInDuration)
            {
                isFadingIn = false;
            }
        }
        if (isFadingOut)
        {
            timerOfOut = Mathf.Clamp(timerOfOut + Time.deltaTime, 0.0f, fadeOutDuration);
            float alpha = 1.0f - (timerOfOut / fadeOutDuration);
            canvasGroup.alpha = alpha;

            if (timerOfOut >= fadeOutDuration)
            {
                isFadingOut = false;
                canvasGroup.interactable = false;
                canvasGroup.blocksRaycasts = false;
            }
        }
    }
    public void toSpeech(string words)
    {
        textMeshPro.text = words;
        canvas.enabled = true;
        StartFadeIn();
        Invoke("SpeechNoWord",2f);
    }
    private void SpeechNoWord()
    {
        textMeshPro.text = "";
        StartFadeOut();
        canvas.enabled = false;
    }
    private void StartFadeIn()
    {
        isFadingIn = true;
        timerOfIn = 0.0f;
        canvasGroup.alpha = 0.0f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }
    private void StartFadeOut()
    {
        isFadingOut = true;
        timerOfOut = 0.0f;
        canvasGroup.alpha = 1.0f;
    }
}
