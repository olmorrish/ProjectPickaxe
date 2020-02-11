using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointSystem : MonoBehaviour
{
    private static CheckPointSystem instance;
    public Vector2 lastCheckPointPos;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;

            // Object presists between scenes
            DontDestroyOnLoad(instance);
        }
        else
        {
            // If there is already a CheckPointSystem Destroy this object
            Destroy(gameObject);
        }
    }
}
