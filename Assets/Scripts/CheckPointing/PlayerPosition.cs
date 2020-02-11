using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerPosition : MonoBehaviour
{
    private CheckPointSystem cks;

    void Start()
    {
        cks = GameObject.FindGameObjectWithTag("CKS").GetComponent<CheckPointSystem>();
        transform.position = cks.lastCheckPointPos;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            // Basically load scene upon death
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
