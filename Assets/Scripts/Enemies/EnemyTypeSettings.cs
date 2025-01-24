using UnityEngine;

// Class to handle enemy type and assign health based on type
public class EnemyTypeSettings : MonoBehaviour
{
    // Enum to define different enemy types
    public enum EnemyType { Weak, Normal, Strong }

    [Header("Enemy Settings")]
    public EnemyType enemyType;  // Type of the enemy

    [Header("Health Settings")]
    public int maxHealth;  // Max health based on the enemy type

    // Initialize the health based on enemy type
    public void InitializeHealth()
    {
        // Set health based on the enemy type
        switch (enemyType)
        {
            case EnemyType.Weak:
                maxHealth = 50;  // Weak enemies have 50 health
                break;
            case EnemyType.Normal:
                maxHealth = 100;  // Normal enemies have 100 health
                break;
            case EnemyType.Strong:
                maxHealth = 200;  // Strong enemies have 200 health
                break;
        }
    }
}
