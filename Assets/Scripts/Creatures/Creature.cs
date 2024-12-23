using UnityEngine;

public class Creature : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created


    public int health = 5;


    public bool easyPickup = false;

    private bool isSleeping;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    public void TakeDamage(int damage)
    {

        health -= damage;

        if (health <= 0)
        {
            Sleep(true);
        }
        //Take damage
    }

    private void Sleep(bool toggle)
    {
        isSleeping = toggle;
        //Sleep
    }


    public bool PickUp()
    {
        if (easyPickup || isSleeping)
        {
            Destroy(gameObject);
            return true;
        }
        return false;
    }
}
