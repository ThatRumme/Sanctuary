using UnityEngine;

public class PlayerIngame : Player
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public Gun gun; //Gun

    public float netRange = 3f;

    [Header("Health")]
    public int maxHealth = 20;
    public float timeUntilCanRegenHealth = 4;
    public float secondsForEachHeartRegen = 0.4f;
    public static int currentHealth = 20;
    float regenTimer = 0;
    float canRegenTimer = 0;


    Creature markedCreature;

    protected override void Start()
    {
        base.Start();
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        RegenerateHealth();
        UpdateMarkedCreature();
    }

    void UpdateMarkedCreature()
    {

        //Raycast for layer creature and return first hit
        RaycastHit hit;
        Debug.DrawRay(cam.transform.position, cam.transform.forward * netRange, Color.red);
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, netRange, 1 << LayerMask.NameToLayer("Creature")))
        {
            if (hit.transform.GetComponent<Creature>() != null)
            {
                markedCreature = hit.transform.GetComponent<Creature>();
            }
        }
        else
        {
            markedCreature = null;
        }
    }

    void RegenerateHealth()
    {
        if (currentHealth == maxHealth) return;


        if (canRegenTimer >= timeUntilCanRegenHealth)
        {
            if (regenTimer >= secondsForEachHeartRegen)
            {
                regenTimer -= secondsForEachHeartRegen;
                currentHealth = Mathf.Min(currentHealth + 1, maxHealth);
            }
            else
            {
                regenTimer += Time.deltaTime;
            }
        }
        else
        {
            canRegenTimer += Time.deltaTime;
        }
    }

    void ResetCanRegenTimer()
    {
        canRegenTimer = 0;
    }

    public void TakeDamage(int damage)
    {
        ResetCanRegenTimer();
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            EventManager.OnPlayerHealthUpdated(currentHealth);
        }
    }

    private void Die()
    {
        currentHealth = maxHealth;
        EventManager.OnPlayerDied();
        EventManager.OnPlayerHealthUpdated(currentHealth);

    }

    protected override void Fire()
    {
        base.Fire();
        if (gun != null)
        {
            gun.Shoot();
        }
    }

    protected override void Interact()
    {
        base.Interact();

        if (markedCreature != null)
        {
            if (markedCreature.PickUp())
            {
                //Add to bag
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("KillTrigger"))
        {
            Die();
        }
    }
}
