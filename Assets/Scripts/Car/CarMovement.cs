using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Cinemachine;

public class CarMovement : MonoBehaviourPun
{
    [SerializeField] private GameObject[] PhotonIsNotMine_GameObjectsToRemove;
    [SerializeField] private GameObject[] PhotonIsMine_GameObjectsToRemove;

    [HideInInspector] public Rigidbody2D rb;
    [SerializeField] private float accelerationForce;
    [SerializeField] private float steeringForce;
    [HideInInspector] public float steeringValue;
    public float speed;
    private float direction;
    [SerializeField] private CinemachineVirtualCamera vcam;
    private float vel = 0f;
    
    private CarDrift carDrift;
    public bool canMove;

    void Start()
    {
        if (!photonView.IsMine)
        {
            foreach (GameObject go in PhotonIsNotMine_GameObjectsToRemove)
            {
                Destroy(go);
            }
        }
        else
        {
            foreach (GameObject go in PhotonIsMine_GameObjectsToRemove)
            {
                Destroy(go);
            }
        }
        canMove = true;
        rb = GetComponent<Rigidbody2D>();
        carDrift = GetComponent<CarDrift>();
    }


    void FixedUpdate()
    {
        if (!photonView.IsMine) return;

        Movement();

        rb.angularVelocity = 0;
    }

    void Movement()
    {
        if(canMove)
        {
            steeringValue = -Input.GetAxis("Horizontal");
            speed = Mathf.SmoothDamp(speed, (Input.GetAxis("Vertical") * accelerationForce), ref vel, 0.6f);
            speed = Mathf.Clamp(speed, -accelerationForce, accelerationForce);
            direction = Mathf.Sign(Vector2.Dot(rb.velocity, rb.GetRelativeVector(Vector2.up)));
            rb.rotation += steeringValue * steeringForce * rb.velocity.magnitude * direction;

            rb.AddRelativeForce(Vector2.up * speed);

            rb.AddRelativeForce(-Vector2.right * rb.velocity.magnitude * steeringValue / 4);
        }
    }

    

    
    [PunRPC]
    public void MoveToStartPos_RPC(Vector3 posOfSpawnPoint, Vector3 zero, bool rotation, bool _canMove, float speed)
    {
        this.transform.position = posOfSpawnPoint;
        this.transform.eulerAngles = zero;
        rb.velocity = zero;
        rb.freezeRotation = rotation;
        canMove = _canMove;
        this.speed = speed;
        carDrift.ClearTrails();
    }

    [PunRPC]
    public void PlayerStartRace_RPC(bool freezeRot, bool move)
    {
        rb.freezeRotation = freezeRot;
        canMove = move;
    }
}
