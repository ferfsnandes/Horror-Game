using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class InitialInstructions : MonoBehaviour
{

    public RawImage blackBackground;
    public GameObject dialogBox;
    public GameObject[] texts = new GameObject[3];
    public GameObject[] controls = new GameObject[2];
    public float textTime = 5f;
    public float controlTime = 5f;
    public Animator windowAnimator;
    private static bool isRunning = true;
    private bool isTextActive = false;
    private Coroutine co = null;

    public static bool IsRunning() { return isRunning; }

    IEnumerator ShowControl(int controlIndex)
    {
        if (controlIndex < controls.Length)
        {
            controls[controlIndex].SetActive(true);
            yield return new WaitForSeconds(controlTime);

            controls[controlIndex].SetActive(false);
            co = StartCoroutine(ShowControl(++controlIndex));
        }
        else
        {
            controls[controlIndex - 1].SetActive(false);

            isTextActive = false;
            isRunning = false;

            yield return null;
        }

    }

    IEnumerator ShowText(int textIndex)
    {
        if(textIndex < texts.Length)
        {
            texts[textIndex].SetActive(true);
            yield return new WaitForSeconds(textTime);

            texts[textIndex].SetActive(false);
            co = StartCoroutine(ShowText(++textIndex));
        }
        else
        {
            dialogBox.SetActive(false);
            

            co = StartCoroutine(ShowControl(0));

            yield return null;
        }

    }
    
    IEnumerator FadeOut()
    {
        windowAnimator.SetBool("isOpen", true);

        PlayerMovement.Lock();
        MouseLook.Lock();

        // Depois substituir essa transição por uma genérica que se aplique ao restante dos gameobjects
        for (float i = 1; i >= 0; i -= 0.01f)
        {
            yield return new WaitForSeconds(0.01f);
            blackBackground.color = new Color(0.75f, 0.75f, 0.75f, i);
        }

        PlayerMovement.Unlock();
        MouseLook.Unlock();

        yield return new WaitForSeconds(0.8f);
        dialogBox.SetActive(true);
        isTextActive = true;

        co = StartCoroutine(ShowText(0));
    }

    // Assim, quando fechar o player cancelar o diálogo, não abrirá o menu de pausa
    IEnumerator Stop()
    {
        yield return new WaitForSeconds(0.5f);
        isRunning = false;
    }

    IEnumerator DeactivateTexts()
    {
        yield return new WaitForSeconds(0.0001f);
        
        dialogBox.SetActive(false);

        for(int i=0; i < controls.Length; i++)
            controls[i].SetActive(false);
    }

    void Start()
    {
        StartCoroutine(DeactivateTexts());
        StartCoroutine(FadeOut());
    }

    void Update()
    {
        if (isTextActive && Input.GetKey(KeyCode.Escape))
        {
            isTextActive = false;
            StartCoroutine(DeactivateTexts());
            StartCoroutine(Stop());
            StopCoroutine(co);
        }
    }
}
