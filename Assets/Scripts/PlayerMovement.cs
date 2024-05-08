using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    
    public CharacterController controller;    
    public float speed = 4.5f;
    public int stamina = 100;
    public bool hitKill = false;
    public AudioSource walkSFX;
    public AudioSource runSFX;
    public Transform cameraPos;

    private static bool locked = false;

    public static bool isLocked() { return locked; }

    public static void Lock() { locked = true; }

    public static void Unlock() { locked = false; }

    void Start() {
        walkSFX.enabled = false;
        runSFX.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!locked)
        {
            //Pegando input AWSD por meio do sistema de input do Unity 
            // Horizontal com as teclas A e D (A:-1 | D:1) 
            // Vertical com as teclas W e S (W:1 | S:-1)
            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");

            //correr
            if (Input.GetKey(KeyCode.LeftShift))
            {
                if (stamina > 0)
                {
                    speed = 11f;
                    stamina--;
                    runSFX.enabled = true;
                }
            }
            else
            {
                speed = 4.5f;
                stamina++;
                runSFX.enabled = false;
            }

            if (hitKill) {
                //cena de gameover
            }

            //Código que considera onde o player está olhando e se move junto
            Vector3 move = transform.right * x + transform.forward * z;

            //Função do componente character controller que movimenta o player
            controller.Move(move * speed * Time.deltaTime);

            if ((Input.GetKey("w") || Input.GetKey("a") || Input.GetKey("s") || Input.GetKey("d")) && !Input.GetKey(KeyCode.LeftShift)) {
                walkSFX.enabled = true;
                // StartCoroutine("moveCamera");
                
            } else {
                walkSFX.enabled = false;
            }
        }
    }


    void OnTriggerEnter(Collider collider) {
        if(collider.gameObject.tag == "enemyHand")
            hitKill = true;
    }

    //subir e descer a câmera ao andar
    // IEnumerator moveCamera() {
    //     Vector3 camera = cameraPos.position;
    //     camera.y += .5f;
    //     cameraPos.position = Vector3.Lerp(cameraPos.position, camera, Time.deltaTime * 2);
    //     yield return new WaitForSeconds(0.2f);
    //     camera = cameraPos.position;
    //     camera.y -= .5f;
    //     cameraPos.position = Vector3.Lerp(cameraPos.position, camera, Time.deltaTime * 2);
    //     yield return new WaitForSeconds(.2f);
    // }
}
