using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    private CheckPointSystem cks;

    private void Start()
    {
        cks = GameObject.FindGameObjectWithTag("CKS").GetComponent<CheckPointSystem>();

        
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            cks.lastCheckPointPos = transform.position;
        }
    }
}
