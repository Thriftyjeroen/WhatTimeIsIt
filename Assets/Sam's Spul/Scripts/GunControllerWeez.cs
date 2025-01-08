using UnityEngine;

public class GunControllerWeez: MonoBehaviour
{
    [SerializeField] Animator animatorWeezGun;
    [SerializeField] Animator animatorGun2;
    [SerializeField] Animator animatorCrossbow;
    private bool canShoot = true;

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && canShoot)
        {
                Shoot();
        }
    }

    void Shoot()
    {
        canShoot = false;
        animatorGun2.SetTrigger("Reload");
        animatorWeezGun.SetTrigger("Reload");
        animatorCrossbow.SetTrigger("Reload");
    }
    public void OnReloadComplete()
    {
        print("can shoot");
        canShoot=true;
    }
}
