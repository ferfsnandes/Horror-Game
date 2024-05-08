using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float mouseSensitivity = 200f;
    public Transform playerBody;
    float xRotation = 0f;

    private static bool locked = false;

    public static bool isLocked() { return locked; }

    public static void Lock() { locked = true; }

    public static void Unlock() { locked = false; }

    // Start is called before the first frame update
    void Start()
    {
        //Travando o cursor
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        if (!locked)
        {
            //Funcionamento: Ao movimentar o eixo X do mouse, o corpo inteiro do player rotaciona no eixo Y
            //Para o movimento vertical da câmera o eixo X é rotacionado em um ângulo máximo de 180°
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            playerBody.Rotate(Vector3.up * mouseX);

            xRotation -= mouseY;
            //Limitando o movimento vertical em 180°
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);
            // Aplicando rotação vertical da câmera
            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        }

    }
}
