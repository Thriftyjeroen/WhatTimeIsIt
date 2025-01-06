using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] GameObject gunSound;


    bool playingGunSound = false;

    public IEnumerator StartPlayingGunSoundLoop(float pShootingTimeOut)
    {
        playingGunSound = true;
        while (playingGunSound)
        {
            yield return new WaitForSecondsRealtime(pShootingTimeOut);
            Instantiate(gunSound);
        }
    }

    public void StartPlayingGunLoop(float pShootingTimeOut)
    {
        StartCoroutine(StartPlayingGunSoundLoop(pShootingTimeOut));
    }


    public void StopPlayingGunLoop()
    {
        playingGunSound = false;
    }

    public void PlayGunSound()
    {
        Instantiate(gunSound);
    }
}
