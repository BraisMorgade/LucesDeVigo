using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy_behaviour : MonoBehaviour
{
    public GameManager manager;

    public float RotationSpeed;

    public Transform barrel;
    public Transform barrel2;

    public ParticleSystem p1;
    public ParticleSystem p2;

    public FMODUnity.StudioEventEmitter hurtEmitter;
    public FMODUnity.StudioEventEmitter dialogueEmitter;
    public FMODUnity.StudioEventEmitter deathEmitter;
    public FMODUnity.StudioEventEmitter shotEmitter;

    private Quaternion _lookRotation;
    private Vector3 _direction;

    GameObject Target;

    public projectile_script projectile;

    public int score;

    public int heat;

    public float proj_offset;

    public float proj_speed;

    public int proj_damage;

    public Vector2 dialogTimeInterval;
 
    public int maxHitPoints;

    public Vector2 shotTimeInterval;

    [Range(-1f, 1f)]
    public float Voz;

    public detectTarget detect_target;
    float timeBetweenShots;
    int hitPoints;
    float timeBetweenDialog;

    bool dying;
    void Start()
    {
        hitPoints = maxHitPoints;
        timeBetweenDialog = Random.Range(dialogTimeInterval.x, dialogTimeInterval.y);
        timeBetweenShots = Random.Range(shotTimeInterval.x, shotTimeInterval.y);
        dying = false;
        Target = manager.getPlayer();
    }

    void Update()
    {
        if (!dying)
        {
            hurtEmitter.SetParameter("Voz", Voz);
            deathEmitter.SetParameter("Voz", Voz);
            dialogueEmitter.SetParameter("Voz", Voz);

            timeBetweenDialog -= Time.deltaTime;
            timeBetweenShots -= Time.deltaTime;

            //find the vector pointing from our position to the target
            _direction = (Target.transform.position - transform.position).normalized;

            //create the rotation we need to be in to look at the target
            _lookRotation = Quaternion.LookRotation(_direction);

            //rotate us over time according to speed until we are in the required rotation
            transform.rotation = Quaternion.Slerp(transform.rotation, _lookRotation, Time.deltaTime * RotationSpeed);

            if (timeBetweenDialog <= 0)
            {
                EmitSound(dialogueEmitter);
                timeBetweenDialog = Random.Range(dialogTimeInterval.x, dialogTimeInterval.y);
            }
            if(timeBetweenShots <=0 && detect_target.IsPlayerInRange())
            {
                GameObject instance = Instantiate(projectile.gameObject);
                instance.transform.position = barrel.transform.position + transform.forward * proj_offset;
                instance.GetComponent<projectile_script>().direction = transform.forward;
                instance.GetComponent<projectile_script>().speed = proj_speed;
                instance.GetComponent<projectile_script>().damage = proj_damage;
                p1.Play();

                if (barrel2 != null)
                {
                    GameObject instance2 = Instantiate(projectile.gameObject);
                    instance2.transform.position = barrel2.transform.position + transform.forward * proj_offset;
                    instance2.GetComponent<projectile_script>().direction = transform.forward;
                    instance2.GetComponent<projectile_script>().speed = proj_speed;
                    instance2.GetComponent<projectile_script>().damage = proj_damage;
                    p2.Play();
                }

                shotEmitter.Play();
                timeBetweenShots = Random.Range(shotTimeInterval.x, shotTimeInterval.y);
            }



        }
        else
        {
            if (!anyEventPlaying())
            {
                Destroy(this.gameObject);
            }
        }
    }
    
    public void TakeDamage(int damage)
    {
        if (dying) return;

        int onBeatBonus = 1;

        hitPoints -= damage * onBeatBonus;
        manager.AddHeat(heat);


        if(manager.onBeat())
        {
            onBeatBonus = 2;
        }

        if (hitPoints <= 0)
        {
            EmitSound(deathEmitter);
            dying = true;
            manager.AddScore(score * onBeatBonus);
            manager.AddHeat(score*onBeatBonus + heat*onBeatBonus);
        }
        else
            EmitSound(hurtEmitter);
    }
    private void StopEmitters()
    {
        deathEmitter.Stop();
        hurtEmitter.Stop();
        dialogueEmitter.Stop();
    }

    private void EmitSound(FMODUnity.StudioEventEmitter emitter)
    {
        if (!anyEventPlaying() && dialogueEmitter == emitter || emitter!=dialogueEmitter)
        {
            StopEmitters();
            emitter.Play();
        }
        
    }

    private bool anyEventPlaying()
    {
        return deathEmitter.IsPlaying() || hurtEmitter.IsPlaying() || dialogueEmitter.IsPlaying();
    }

    public bool isDead()
    {
        return dying;
    }
}
