using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weapon_bob : MonoBehaviour
{
    public GameObject Weapon;
    public Vector2 bob_interval;
    public float bob_speed;

    float speed;
    float x = 0;
    void Update()
    {
        Weapon.GetComponent<Transform>().localPosition = new Vector3(
            Weapon.transform.localPosition.x,
            Weapon.transform.localPosition.y, bob_interval.x + ((bob_interval.y - bob_interval.x) / 2) + Mathf.Sin(Mathf.Deg2Rad * x) * ((bob_interval.y - bob_interval.x) / 2)
        );

        speed = this.GetComponent<player_movement>().getCurrentMovement();

        if(this.GetComponent<player_movement>().isGrounded())
            x += speed * bob_speed * Time.deltaTime;
    }
}
