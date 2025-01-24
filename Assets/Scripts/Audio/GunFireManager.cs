using UnityEngine;

public class GunFireManager : MonoBehaviour
{
    AudioSource source;
    private void Awake()
    {
        
    }
    private void Start()
    {
        source = GetComponent<AudioSource>();
        source.pitch = Random.Range(0.8f, 1.2f);
    }
    void Update()
    {
        if (!source.isPlaying)
        {
            Destroy(gameObject);
        }
    }
}
