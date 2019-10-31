using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall_Breakable_Script : MonoBehaviour
{
    public float TotalHealth;
    public float CurrentHealth;
    public int requiredPickaxeLevel;

    // Start is called before the first frame update
    void Start() { 
        
    }

    // Update is called once per frame
    void Update() { 
        if (CheckIfDead()) {
            Die();
        }
    }

    bool CheckIfDead() {
        return CurrentHealth <= 0f;
    }

    public float GetRequiredPickaxeLevel() {
        return requiredPickaxeLevel;
    }

    public void TakeDamage(float damage) {
        Debug.Log("Wall took damage");
        CurrentHealth -= damage;
    }

    void Die() {
        Destroy(this.gameObject);
    }
}
