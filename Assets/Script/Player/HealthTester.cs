using UnityEngine;

public class HealthTester : MonoBehaviour
{
    [SerializeField] private HealthSystem healthSystem;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            healthSystem.TakeDamage(10);
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            healthSystem.Heal(10);
        }
    }
}
