using UnityEngine;

public class GunControllerWeez: MonoBehaviour
{
    [SerializeField] Animator animatorWeezGun;
    [SerializeField] Animator animatorGun2;
    [SerializeField] Animator animatorCrossbow;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        animatorGun2.SetTrigger("Reload");
        animatorWeezGun.SetTrigger("Reload");
        animatorCrossbow.SetTrigger("Reload");

    }
}
