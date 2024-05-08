using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PauseMenu : MonoBehaviour
{

    public GameObject menuUI;
    public int codeQty = 3;
    public TMP_Text keyStatus;
    public TMP_Text codeStatus;
    public TMP_Text finalItemStatus;
    public float cooldown = 0.5f;
    private float lastTime = 0f;
    private bool isCrRunning = false;

    public void Quit()
    {
        Debug.Log("Saiu.");
    }
    
    void LoadInfo()
    {
        if (!GameObject.Find("Chaves"))
            keyStatus.text = "1/1";

        int count = 0;

        for (int i = 0; i < codeQty; i++)
            if (!GameObject.Find("Codigo" + (i + 1).ToString()))
                count++;
       
        codeStatus.text = count.ToString() + "/" + codeQty.ToString();

        if (!GameObject.Find("FinalItem"))
            finalItemStatus.text = "1/1";
    }

    IEnumerator OpenUI()
    {
        isCrRunning = true;

        yield return new WaitForSeconds(cooldown);

        Time.timeScale = 0;

        PlayerMovement.Lock();
        MouseLook.Lock();
        Cursor.lockState = CursorLockMode.Confined;

        LoadInfo();
        menuUI.SetActive(true);
        Cursor.visible = true;

        isCrRunning = false;
    }

    public void CloseUI()
    {
        Time.timeScale = 1;

        PlayerMovement.Unlock();
        MouseLook.Unlock();
        Cursor.lockState = CursorLockMode.Locked;

        menuUI.SetActive(false);
        Cursor.visible = false;
    }

    void Update()
    {
        if (!SafeBehaviour.IsMenuOpen() && !InitialInstructions.IsRunning() && Time.time - SafeBehaviour.GetLastTime() >= cooldown)
            if (!menuUI.activeSelf)
            {
                if (Input.GetKey(KeyCode.Escape) && Time.time - lastTime >= cooldown)
                {
                    lastTime = Time.time;

                    if (!isCrRunning)
                        StartCoroutine(OpenUI());
                    else
                        lastTime -= cooldown * 1.0001f;

                }
            }
            else if (Input.GetKey(KeyCode.Escape) && Time.time - lastTime >= cooldown)
            {
                lastTime = Time.time;

                CloseUI();
            }
    }
}
