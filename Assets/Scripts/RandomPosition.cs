using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPosition : MonoBehaviour
{
    public Vector3[] randomPositions = new Vector3[3];

    void Start()
    {
        int randomIndex = Random.Range(-1, randomPositions.Length);

        if (randomIndex > 0) {
            transform.position = randomPositions[randomIndex];
        }
    }

}