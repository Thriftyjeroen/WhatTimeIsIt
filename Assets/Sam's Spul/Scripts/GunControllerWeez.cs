using UnityEngine;

public class GunControllerWeez: MonoBehaviour
{
    [SerializeField] Animator animator;

    private bool hasShot = false;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Reload();
        }
    }

    void Shoot()
    {
        if (!hasShot)
        {
            animator.SetTrigger("Shoot");
            hasShot = true;
        }
    }

    void Reload()
    {
        if (hasShot)
        {
            animator.SetTrigger("Reload");
            hasShot = false; // Reset after reloading
        }
    }
}
