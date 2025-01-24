using NUnit.Framework.Constraints;
using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    bool playingGunSound = false;

    public IEnumerator StartPlayingGunSoundLoop(float pShootingTimeOut)
    {
        string pSound = "GunShootSound";

        playingGunSound = true;
        while (playingGunSound)
        {
            yield return new WaitForSecondsRealtime(pShootingTimeOut);
            GameObject sound = Instantiate((GameObject)Resources.Load($"prefabs/{pSound}"));
            if (gameObject.transform.parent.parent.parent.gameObject.CompareTag("Player"))
            {
                sound.GetComponent<AudioSource>().spatialBlend = 0;
            }
        }
    }
    public IEnumerator StartPlayingCrossbowSoundLoop(float pShootingTimeOut)
    {
        string pSound = "CrossbowShootSound";
        GameObject sound = Instantiate((GameObject)Resources.Load($"prefabs/{pSound}"));

        playingGunSound = true;
        while (playingGunSound)
        {
            yield return new WaitForSecondsRealtime(pShootingTimeOut);
            Instantiate(sound);
        }
    }


    public void StopPlayingSoundLoop()
    {
        playingGunSound = false;
    }

    public void PlaySound(string pSound)
    {
        GameObject sound = Instantiate((GameObject)Resources.Load($"prefabs/{pSound}"), this.transform);
        if (gameObject.transform.parent.parent.parent.gameObject.CompareTag("Player"))
        {
            sound.GetComponent<AudioSource>().spatialBlend = 0;
        }
    }
}
