using UnityEngine;

public class fire_gun : MonoBehaviour
{
    public Transform camera;
    public GameManager manager;
    public FMODUnity.StudioEventEmitter shotEmitter;
    public GameObject OnBeatText;
    public float timebtwShots;
    public int damage;
    public float range;
    float counter;

    public ParticleSystem muzzleFlash;

    public float beatTextTimeActive;
    float beatTextTime;
    void Start()
    {
        counter = timebtwShots;
        beatTextTime = 0;
    }
    void Update()
    {
        counter -= Time.deltaTime;

        if(beatTextTime <= 0)
        {
            OnBeatText.SetActive(false);
            beatTextTime = 0;
        }
        
        if (Input.GetButtonDown("Fire1"))
        {
            if (counter <= 0)
            {
                Shoot();
                counter = timebtwShots;
            }
        }

        beatTextTime -= Time.deltaTime;
    }

    void Shoot()
    {
        muzzleFlash.Play();
        shotEmitter.Play();
        RaycastHit hit;
        Debug.DrawRay(camera.transform.position, camera.transform.forward, Color.green);

        if(manager.onBeat())
        {
            OnBeatText.SetActive(true);
            beatTextTime = beatTextTimeActive;
        }

        int layerMask = LayerMask.GetMask("Enemy");

        if (Physics.Raycast(camera.transform.position, camera.transform.forward, out hit, range, layerMask))
        {
            enemy_behaviour enemy = hit.transform.GetComponent<enemy_behaviour>();

            if (enemy != null)
                enemy.TakeDamage(damage);
        }
    }
}
