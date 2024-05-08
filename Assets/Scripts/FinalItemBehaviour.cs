using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalItemBehaviour : MonoBehaviour
{

    public GameObject finalItem;
    public GameObject message;
    public float distance = 1.5f;
    public Animator animator;
    public float animationTime = 3;

    IEnumerator ActivateMessage()
    {
        yield return new WaitForSeconds(animationTime);

        animationTime = 0;

        message.SetActive(true);

        if (Input.GetKey(KeyCode.E))
        {
            Destroy(finalItem);
            message.SetActive(false);
        }

    }

    IEnumerator DeactivateMessage()
    {
        yield return new WaitForSeconds(0.0001f);

        message.SetActive(false);

    }

    void Update()
    {

        if (animator.GetBool("isOpen") && finalItem && Vector3.Distance(transform.position, finalItem.transform.position) < distance)
            StartCoroutine(ActivateMessage());
        else
            StartCoroutine(DeactivateMessage());

    }
}
