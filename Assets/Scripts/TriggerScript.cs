using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerScript : MonoBehaviour
{
    public bool triggerOn;
    private BoxCollider2D bc;
    // Start is called before the first frame update
    void Start()
    {
        bc = gameObject.GetComponent<BoxCollider2D>();
        triggerOn = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool GetTriggerStatus() {
        return triggerOn;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("ENV_BREAKABLE")) {
            triggerOn = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("ENV_BREAKABLE")) {
            triggerOn = false;
        }
    }
}
