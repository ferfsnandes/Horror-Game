using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : MonoBehaviour
{
    public GameObject window;
    public GameObject message;
    public float distance = 2f;   

    IEnumerator DeactivateMessage()
    {
        yield return new WaitForSeconds(0.0001f);

        message.SetActive(false);
    }

    void Update()
    {
        if(!GameObject.Find("FinalItem") && Vector3.Distance(transform.position, window.transform.position) < distance)
        {
            message.SetActive(true);

            if (Input.GetKey(KeyCode.E))
            {
                Debug.Log("Escapou!");
                message.SetActive(false);
            }
        }
        else
            StartCoroutine(DeactivateMessage());

    }
}
