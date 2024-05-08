using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideMechanic : MonoBehaviour
{
    public Transform door;

    public Vector3 doorOpenedPosition = new Vector3(0,0,0);
    public Vector3 doorClosedPosition = new Vector3(0,0,1);

    private bool onRange;


    public int openCloseSpeed = 2;

    // Update is called once per frame
    void Update()
    {
       if(onRange){
            //aparecer UI
            System.Console.WriteLine("To no range");
           if(Input.GetKeyDown("e")){
                //Desativar movimentação player

                //Abrir porta armário
                door.position = Vector3.Lerp(door.position, doorOpenedPosition, Time.deltaTime * openCloseSpeed);
                //Mover player pra dentro

                //Fechar porta armário
                door.position = Vector3.Lerp(door.position, doorClosedPosition, Time.deltaTime * openCloseSpeed);
           }
       
       }
        
    }


    //Ao entrar no gatilho
    private void OnTriggerEnter(Collider col){
        if(col.tag=="Player"){
            onRange=true;
        }
        }

    private void OnTriggerExit( Collider col){
        if(col.tag=="Player"){
            onRange=false;
        }
    }


}
