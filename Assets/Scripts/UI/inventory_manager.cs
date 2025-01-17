using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class inventory_manager : MonoBehaviour
{
    [SerializeField] Image Slot1,Slot2,Slot3,Slot4;
    [SerializeField] GameObject WeezerGun, Gun2, CrossBow, FlintLock;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        print(Slot1.sprite.name);
        print(Slot2.sprite.name);
        print(Slot3.sprite.name);
        print(Slot4.sprite.name);
        print(CrossBow.name+ "_0");
    }

    // Update is called once per frame
    void Update()
    {
        if (Slot1.sprite.name == CrossBow.name + "_0")
        {
            print("yes");
        }
    }
}
