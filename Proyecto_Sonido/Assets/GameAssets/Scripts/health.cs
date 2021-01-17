using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class health : MonoBehaviour
{
    public int maxHitPoints;
    public float heatpenalty;
    public GameManager manager;
    public healthBar hBar;
    public FMODUnity.StudioEventEmitter deathEvent;
    public FMODUnity.StudioEventEmitter hurtEvent;
    public float invulTime;
    float timeSinceDamaged;
    int hitPoints;
    bool dying;
    void Start()
    {
        hitPoints = maxHitPoints;
        timeSinceDamaged = invulTime;
        hBar.SetMaxHealth(maxHitPoints);
        dying = false;
    }

    void Update()
    {
        timeSinceDamaged += Time.deltaTime;
        if(dying && !deathEvent.IsPlaying())
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
    public void TakeDamage(int damage)
    {
        if (timeSinceDamaged < invulTime || dying)
            return;
        hitPoints -= damage;
        manager.AddHeat(-heatpenalty);
        manager.AddScore(-1);
        timeSinceDamaged = 0;
        hBar.SetHealth(hitPoints);
        if (hitPoints <= 0)
        {
            dying = true;
            deathEvent.Play();
        }
        else
        {
            hurtEvent.Play();
        }
    }
}
