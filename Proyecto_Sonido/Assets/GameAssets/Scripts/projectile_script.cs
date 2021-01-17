using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectile_script : MonoBehaviour
{
    public float speed;

    public Vector3 direction;

    public int damage;

    public float timeToDestroy;
    float time;

    void Start()
    {
        time = timeToDestroy;
    }
    void Update()
    {
        timeToDestroy -= Time.deltaTime;
        if (timeToDestroy <= 0)
            Destroy(this.gameObject);
        this.GetComponent<Rigidbody>().velocity = direction.normalized * speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        //Destroy(this.gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "Player")
            Destroy(this.gameObject);

    }
}
