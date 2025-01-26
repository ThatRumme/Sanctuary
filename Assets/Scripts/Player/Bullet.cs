using UnityEngine;

public class Bullet : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created


    public float speed = 10f;

    public float despawnTime = 5f;

    public int damage = 5;

    private Vector3 oldPos;
    void Start()
    {
        Destroy(gameObject, despawnTime);
        oldPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
        CalculateHits();
        oldPos = transform.position;
    }

    void CalculateHits()
    {
        RaycastHit hit;
        if (Physics.Raycast(oldPos, transform.position - oldPos, out hit, (transform.position - oldPos).magnitude))
        {

            if (hit.transform.tag == "Creature")
            {
                hit.transform.GetComponent<Creature>().TakeDamage(damage);
            }

            Destroy(gameObject);
        }
    }
}
