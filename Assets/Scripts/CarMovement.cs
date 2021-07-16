using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Cinemachine;

public class CarMovement : MonoBehaviourPun
{
    [HideInInspector] public Rigidbody2D rb;
    [SerializeField] private float accelerationForce;
    [SerializeField] private float steeringForce;
    private float steeringValue;
    private float speed;
    private float direction;
    [SerializeField] private SoundManager sound;
    [SerializeField] private CinemachineVirtualCamera vcam;

    [SerializeField] private const float minPitch = 0.69f;
    [SerializeField] private const float maxPitch = 1.4f;

    public TrailRenderer[] trails;

    private bool isDrifting;

    private float vel = 0f;

    [SerializeField] private GameObject playerCanvas;

    public bool canMove;


    void Start()
    {
        canMove = true;
        rb = GetComponent<Rigidbody2D>();
        if (!photonView.IsMine)
        {
            Destroy(vcam.gameObject);
            Destroy(playerCanvas);
        }
        
    }


    void FixedUpdate()
    {
        if (!photonView.IsMine) return;

        if(canMove)
            Movement();

        Sounds();
        Drifting();
        rb.angularVelocity = 0;
    }

    void Movement()
    {
        steeringValue = -Input.GetAxis("Horizontal");
        speed = Mathf.SmoothDamp(speed, (Input.GetAxis("Vertical") * accelerationForce), ref vel, 0.6f);
        speed = Mathf.Clamp(speed, -accelerationForce, accelerationForce);
        direction = Mathf.Sign(Vector2.Dot(rb.velocity, rb.GetRelativeVector(Vector2.up)));
        rb.rotation += steeringValue * steeringForce * rb.velocity.magnitude * direction;

        rb.AddRelativeForce(Vector2.up * speed);

        rb.AddRelativeForce(-Vector2.right * rb.velocity.magnitude * steeringValue / 4);
    }

    void Sounds()
    {
        if(sound.CanPlaySFX)
        {
            sound.audioSource.pitch = isDrifting ? ((speed / 31.818f) - 0.08f) : (speed / 31.818f);
            sound.audioSource.pitch = Mathf.Clamp(sound.audioSource.pitch, minPitch, maxPitch);
        }
    }

    void Drifting()
    {
        if (steeringValue > 0.9f || steeringValue < -0.9f)
        {
            photonView.RPC("DrawDriftingLines", RpcTarget.All, true);
        }
        else if(isDrifting)
        {
            photonView.RPC("DrawDriftingLines", RpcTarget.All, false);
        }
    }
    [PunRPC]
    void DrawDriftingLines(bool isActive)
    {
        if(isActive)
        {
            foreach (TrailRenderer trail in trails)
            {
                trail.emitting = true;
                isDrifting = true;
            }
        }
        else if(!isActive)
        {
            foreach (TrailRenderer trail in trails)
            {
                if (trail.emitting)
                    trail.emitting = isActive;

                isDrifting = isActive;
            }
        }
    }


    public void MoveToStartPos(Vector3 posOfSpawnPoint, Vector3 zero, bool rotation, bool _canMove)
    {
        photonView.RPC("MovePlayersToSpawnPoints", RpcTarget.All, posOfSpawnPoint, zero, rotation, _canMove);
    }

    [PunRPC]
    void MovePlayersToSpawnPoints(Vector3 posOfSpawnPoint, Vector3 zero, bool rotation, bool _canMove)
    {
        this.transform.position = posOfSpawnPoint;
        this.transform.eulerAngles = zero;
        rb.velocity = zero;
        rb.freezeRotation = rotation;
        canMove = _canMove;

        for (int i = 0; i < trails.Length; i++)
        {
            trails[i].Clear();
        }
    }
}
