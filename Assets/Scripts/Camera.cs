using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    public GameObject playerObj;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        SetCameraLocation();
    }

    Vector2 GetPlayerLocation() {
        return playerObj.GetComponent<Transform>().position;
    }

    void SetCameraLocation() {
        gameObject.GetComponent<Transform>().position = new Vector3(GetPlayerLocation().x, GetPlayerLocation().y, gameObject.GetComponent<Transform>().position.z);
    }
}
