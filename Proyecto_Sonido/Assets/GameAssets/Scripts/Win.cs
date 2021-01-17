using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Win : MonoBehaviour
{
    public enemy_behaviour e_b;
    public float time;
    float t;

    void Start()
    {
        t = time;
    }
    void Update()
    {
        if (t <= 0)
        {
            SceneManager.LoadScene("Win");
        }
        if (e_b.isDead())
        {
            t -= Time.deltaTime;
        }
    }
}
