using UnityEngine;

public class TempScript : MonoBehaviour
{
    [SerializeField] GameObject Player; // Reference to the player GameObject
    [SerializeField] WinLoseScript WinLoseScript; // Reference to WinLoseScript to handle the win/lose state

    // Update is called once per frame
    void Update()
    {
        // Check if the player's y-position is below a certain threshold (e.g., falls off the map)
        if (Player.transform.position.y <= -60)
        {
            // If the player falls below this threshold, trigger the Lose method in WinLoseScript
            WinLoseScript.Lose();
        }
    }
}
