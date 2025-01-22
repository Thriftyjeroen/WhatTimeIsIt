using UnityEngine;

public class TempScript : MonoBehaviour
{
    [SerializeField] GameObject Player;
    [SerializeField] WinLoseScript WinLoseScript;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Player.transform.position.y <= -60)
        {
            WinLoseScript.Lose();
        }
    }
}
