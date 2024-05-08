using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SafeBehaviour : MonoBehaviour
{

    public GameObject safe;
    public Animator animator;
    public Camera cam;
    public GameObject flashlight;
    public float zoom = 3;
    public GameObject openMessage;
    public int maxNumber = 99;
    public int precision = 1;
    public GameObject safeUI;
    public GameObject arrows;
    public GameObject setButton;
    public TMP_Text[] fields = new TMP_Text[3];
    public TMP_Text[] foundNotes = new TMP_Text[3];
    public float maxDistance = 1.5f;
    public float minDistance = 0.5f;
    public int arrowsDisplayTime = 8;
    public float cooldown = 0.5f;
    private static bool isMenuOpen = false;
    private static float lastTimeClosed;
    private int[] correctNumbers;
    private int numbersQty;
    private float lastTime = 0f;
    private int num;
    private int fieldIndex = 0;
    private Vector3 originalCamY;
    private Vector3 newCamY;
    private Vector3 originalFlashlightPos;
    private Vector3 originalSetButton;
    private Coroutine co = null;

    public static bool IsMenuOpen() { return isMenuOpen; }

    public static float GetLastTime() { return lastTimeClosed; }

    IEnumerator SetCorrectNumbers()
    {
        yield return new WaitForSeconds(1);

        numbersQty = fields.Length;
        correctNumbers = new int[numbersQty];
        correctNumbers = randomCode.GetCorrectNumbers();

        for (int i = 0; i < numbersQty; i++)
            Debug.Log(correctNumbers[i]);
    }
    
    void Start()
    {
        StartCoroutine(SetCorrectNumbers());
    }

    void Rotate(int direction)
    {
        GameObject combinationLock = safe.transform.Find("Safe door/combination lock").gameObject;

        combinationLock.transform.Rotate(0f, 0f, direction * 0.5f, Space.Self);

        if (combinationLock.transform.localEulerAngles.z != 0f)
        {
            float angle = 360f - combinationLock.transform.localEulerAngles.z;
            num = (int)Mathf.RoundToInt((maxNumber / (360f / angle)) / precision) * precision;
        }
        else
            num = 0;

        fields[fieldIndex].text = num.ToString("00");
    }

    IEnumerator HideSafeArrows()
    {
        yield return new WaitForSeconds(arrowsDisplayTime);

        arrows.SetActive(false);
    }

    void OpenUI()
    {
        PlayerMovement.Lock();
        MouseLook.Lock();

        openMessage.SetActive(false);

        safeUI.SetActive(true);
        isMenuOpen = true;

        fields[0].text = num.ToString("00");

        for (int i = 0; i < numbersQty; i++)
            if (!GameObject.Find("Codigo" + (i + 1).ToString()))
                foundNotes[i].text = correctNumbers[i].ToString("00");

        arrows.SetActive(true);
        co = StartCoroutine(HideSafeArrows());

        originalSetButton = setButton.transform.position;

        // Ajustando posi��o da c�mera

        originalCamY = cam.transform.position;
        newCamY = new Vector3(safe.transform.position.x, safe.transform.position.y, safe.transform.position.z - 1f);

        originalFlashlightPos = flashlight.transform.position;

        cam.transform.position = newCamY;

        cam.transform.LookAt(safe.transform.position);
        cam.transform.eulerAngles = new Vector3(cam.transform.rotation.eulerAngles.x - 2f,
            cam.transform.rotation.eulerAngles.y, cam.transform.rotation.eulerAngles.z);

        flashlight.transform.position = new Vector3(safe.transform.position.x, originalFlashlightPos.y - 1f, originalFlashlightPos.z - 1f);

        cam.fieldOfView /= zoom;
    }

    void CloseUI()
    {
        PlayerMovement.Unlock();
        MouseLook.Unlock();

        safeUI.SetActive(false);
        isMenuOpen = false;

        fieldIndex = 0;

        for (int i = 0; i < numbersQty; i++)
        {
            fields[i].text = "-";
            
            if(i > 0)
                fields[i].fontStyle = 0;
        }

        fields[0].fontStyle = FontStyles.Bold;

        StopCoroutine(co);
        arrows.SetActive(false);

        setButton.transform.position = originalSetButton;

        // Resetando posi��o da c�mera

        cam.transform.position = originalCamY;

        flashlight.transform.position = originalFlashlightPos;

        cam.fieldOfView *= zoom;

        lastTimeClosed = Time.time;
    }

    void CheckCombination()
    {
        int count = 0;

        for (int i = 0; i < numbersQty; i++)
            if (correctNumbers[i] == int.Parse(fields[i].text))
                count++;

        Debug.Log(count);

        if (count == numbersQty)
            animator.SetBool("isOpen", true);

        CloseUI();

    }
    
    IEnumerator DeactivateopenMessage()
    {
        yield return new WaitForSeconds(0.0001f);
        openMessage.SetActive(false);
    }

    void Update()
    {
        // S� h� intera��o se o cofre ainda foi aberto
        if (!animator.GetBool("isOpen"))
        {
            // Se o player estiver abrindo o cofre
            if(isMenuOpen)
            {
                // Fechar UI do cofre
                if (Input.GetKey(KeyCode.Escape) && Time.time - lastTime >= cooldown)
                {
                    lastTime = Time.time;

                    CloseUI();
                }

                // Girar esquerda
                if (Input.GetKey(KeyCode.D))
                    Rotate(-1);

                // Girar direita
                if (Input.GetKey(KeyCode.A))
                    Rotate(1);
                

                // Definir n�mero selecionado
                if (Input.GetKey(KeyCode.E) && Time.time - lastTime >= cooldown)
                {
                    lastTime = Time.time;

                    if (++fieldIndex == numbersQty)
                        CheckCombination();
                    else
                    {
                        float newSetButtonX;

                        if (fieldIndex == 1)
                            newSetButtonX = 1.2f;
                        else
                            newSetButtonX = 1.1665f;

                        setButton.transform.position = new Vector3(setButton.transform.position.x * newSetButtonX, 
                            setButton.transform.position.y, setButton.transform.position.z);
                        
                        fields[fieldIndex - 1].fontStyle = 0;
                        fields[fieldIndex].fontStyle = FontStyles.Bold;
                        fields[fieldIndex].text = num.ToString("00");
                    }
                }
            }
            // Se o player n�o est� abrindo o cofre, mas est� perto dele, ent�o mostrar mensagem p/ abrir
            else if (Vector3.Distance(transform.position, safe.transform.position) < maxDistance &&
                Vector3.Distance(transform.position, safe.transform.position) >= minDistance)
            {
                openMessage.SetActive(true);

                if (Input.GetKey(KeyCode.E) && Time.time - lastTime >= cooldown)
                {
                    lastTime = Time.time;

                    OpenUI();
                }
            }
            else
                StartCoroutine(DeactivateopenMessage());
        }

    }

}
