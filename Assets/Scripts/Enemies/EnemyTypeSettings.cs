using UnityEngine;

// Class to handle enemy type and assign health based on type
public class EnemyTypeSettings : MonoBehaviour
{
    public enum EnemyType { Weak, Normal, Strong }

    [Header("Enemy Settings")]
    public EnemyType enemyType;

    [Header("Health Settings")]
    public int maxHealth;  // Calculated max health based on type

    public void InitializeHealth()
    {
        // Set health based on enemy type
        switch (enemyType)
        {
            case EnemyType.Weak:
                maxHealth = 50;
                break;
            case EnemyType.Normal:
                maxHealth = 100;
                break;
            case EnemyType.Strong:
                maxHealth = 200;
                break;
        }
    }
}
