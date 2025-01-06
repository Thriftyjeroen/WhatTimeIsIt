using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamPosScript : MonoBehaviour
{
    [SerializeField] Transform CamPos;
    [SerializeField] playercam PlayerCam;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.position = CamPos.position;

    }
}
