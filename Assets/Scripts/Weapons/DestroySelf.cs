using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine;

public class DestroySelf : MonoBehaviour
{
    [SerializeField] private float destroyDelay = 5f;

    void Start()
    {
        
        Destroy(gameObject, destroyDelay);
    }

}
