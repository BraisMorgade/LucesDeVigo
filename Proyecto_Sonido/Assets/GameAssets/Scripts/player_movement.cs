using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_movement : MonoBehaviour
{
    public float[] limit_speed;
    public float jumpHeight;
    public Transform groundCheck;
    public LayerMask groundMask;
    public bool autoMovement;
    public Vector3 autoMovementDir;

    bool grounded;
    public float groundDistance = 0.4f;
    public float speed = 0;
    public CharacterController controller;
    [FMODUnity.EventRef]
    public string jumpEvent = "";
    float strafe;
    Vector3 move;
    Vector3 velocity;
    public Transform cam;

    int currentSpeedLimit;

    FMODUnity.StudioEventEmitter emitter;
    void Start()
    {
        currentSpeedLimit = 0;
        emitter = GetComponent<FMODUnity.StudioEventEmitter>();
        if (autoMovement)
            emitter.SetParameter("speed", limit_speed[currentSpeedLimit]);
        else emitter.Stop();
        velocity = new Vector3(0, 0, 0);
    }
    public float getCurrentMovement()
    {
        return move.magnitude;
    }

    public bool isGrounded()
    {
        return grounded;
    }
    public Vector3 ManualWalk()
    {
        Vector2 input;
        input.x = Input.GetAxis("Horizontal");
        input.y = Input.GetAxis("Vertical");

        Vector2 cameraDirection;
        cameraDirection.x = Mathf.Sin(Mathf.Deg2Rad * cam.eulerAngles.y);
        cameraDirection.y = Mathf.Cos(Mathf.Deg2Rad * cam.eulerAngles.y);

        cameraDirection.Normalize();

        Vector2 strafeVector = Vector2.Perpendicular(cameraDirection);

        strafeVector.Normalize();

        Vector2 direction = cameraDirection * input.y + strafeVector * -input.x;

        direction.Normalize();


        return new Vector3(direction.x, 0, direction.y) * speed * input.magnitude * Time.deltaTime;
    }

    public Vector3 AutoRun()
    {
        strafe = Input.GetAxis("Horizontal");

        Vector2 direction = new Vector2(autoMovementDir.x, autoMovementDir.z);

        Vector2 strafeVector = Vector2.Perpendicular(direction);
        strafeVector.Normalize();

        direction = direction + strafeVector * -strafe;

        speed = limit_speed[currentSpeedLimit];

        direction.Normalize();

        return new Vector3(direction.x, 0, direction.y) * speed * Time.deltaTime;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
            speedUp();

        grounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if(grounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        Vector3 aux = new Vector3(1, 0, 0) * speed * Time.deltaTime;
        if (autoMovement)
            move = AutoRun();
        else
        {
            move = ManualWalk();
            emitter.SetParameter("speed", speed/2 * move.magnitude/aux.magnitude );
        }

        if (Input.GetKeyDown(KeyCode.Space) && grounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * -9.8f);
            FMODUnity.RuntimeManager.PlayOneShot(jumpEvent, transform.position);
        }

        velocity.y += -9.8f * Time.deltaTime;

        controller.Move(move + velocity * Time.deltaTime);

        if(grounded && !emitter.IsPlaying())
        {
            emitter.Play();
            if (autoMovement)
                emitter.SetParameter("speed", limit_speed[currentSpeedLimit]);
            else emitter.SetParameter("speed", speed/2 * move.magnitude / aux.magnitude);
        }

        if (!grounded)
            emitter.Stop();
    }

    void speedUp()
    {
        if (currentSpeedLimit < limit_speed.GetLength(0) - 1)
        {
            currentSpeedLimit++;
            emitter.SetParameter("speed", limit_speed[currentSpeedLimit]);
            if (!emitter.IsPlaying()) emitter.Play();
        }
    }
}
