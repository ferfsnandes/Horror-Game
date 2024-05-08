using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CollectibleBehaviour : MonoBehaviour
{

    public bool[] objects = new bool[4];
    public GameObject[] collectible = new GameObject[4];
    public GameObject message;
    public float distance = 1.5f;
    private Outline outline = null;

    IEnumerator DeactivateMessage(int index)
    {
        yield return new WaitForSeconds(0.0001f);

        if (!objects.Contains(true))
        {
            message.SetActive(false);

            if (outline)
                outline.enabled = false;
        }

        objects[index] = false;

    }

    void Update()
    {
        for (int i = 0; i < collectible.Length; i++)
            if (collectible[i] && Vector3.Distance(transform.position, collectible[i].transform.position) < distance)
            {
                outline = collectible[i].GetComponent<Outline>();
                outline.enabled = true;

                message.SetActive(true);

                objects[i] = true;

                if (Input.GetKey(KeyCode.E))
                {
                    Destroy(collectible[i]);
                    message.SetActive(false);
                    objects[i] = false;
                }
            }
            else
                StartCoroutine(DeactivateMessage(i));
    }
}
