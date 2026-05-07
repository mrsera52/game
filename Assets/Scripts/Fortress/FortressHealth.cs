using UnityEngine;

public class FortressHealth : MonoBehaviour
{
    public float maxHealth = 1000f;
    private float current;

    void Start() => current = maxHealth;

    public void TakeDamage(float dmg)
    {
        current -= dmg;
        Debug.Log($"Крепость: {current}/{maxHealth} HP");
        if (current <= 0) { Debug.Log("Победа!"); Destroy(gameObject); }
    }
}