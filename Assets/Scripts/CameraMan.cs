using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMan : MonoBehaviour
{
    public GameObject playerObj;

    private bool followPlayer; //when true, will move ea frame to follow player

    // Start is called before the first frame update
    void Start()
    {
        followPlayer = true;
    }

    // Update is called once per frame
    void Update(){

        if (followPlayer) {
            SnapCamera(GetPlayerLocation());
        }
        
    }

    private Vector2 GetPlayerLocation() {
        return playerObj.GetComponent<Transform>().position;
    }

    void SnapCamera(Vector2 location) {
        gameObject.GetComponent<Transform>().position = new Vector3(location.x, location.y, this.transform.position.z); //will ignore z and keep the same
    }
}
