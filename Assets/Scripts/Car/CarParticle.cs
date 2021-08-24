using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class CarParticle : MonoBehaviourPun
{


 //   [Header("SmokeIdle")]
 //   [SerializeField] private float idle_startSizeConstantMax = 0.2f;
	//[SerializeField] private float idle_rateOverTime = 4f;

 //   [Header("SmokeMove")]
 //   [SerializeField] private float move_startSizeConstantMax = 0.4f;
 //   [SerializeField] private float move_rateOverTime = 24f;

    private CarMovement car;

    [SerializeField] private ParticleSystem smokeParticle_Idle;
    [SerializeField] private ParticleSystem smokeParticle_Moving;



    void Start()
    {
        car = GetComponent<CarMovement>();
    }

    void Update()
    {
        SmokeParticle();
    }

    void SmokeParticle()
    {
        if(car.speed > 12)
        {
            car.photonView.RPC("ChangeSmoke", RpcTarget.All, false, true);
        }
        else if(car.speed < 12)
        {
            car.photonView.RPC("ChangeSmoke", RpcTarget.All, true, false);
        }
    }

    [PunRPC]
    void ChangeSmoke(bool activeSmokeIdle, bool activeSmokeMoving)
    {
        //if(smokeParticle_Idle.activeSelf != activeSmokeIdle)
        //    smokeParticle_Idle.SetActive(activeSmokeIdle);

        //if (smokeParticle_Moving.activeSelf != activeSmokeMoving)
        //    smokeParticle_Moving.SetActive(activeSmokeMoving);


        if(smokeParticle_Idle.emission.enabled != activeSmokeIdle)
        {
            Debug.Log("Change Smoke Idle to: " + activeSmokeIdle);
            var smokeIdle = smokeParticle_Idle.emission;
            smokeIdle.enabled = activeSmokeIdle;
        }
        if (smokeParticle_Moving.emission.enabled != activeSmokeMoving)
        {
            Debug.Log("Change Smoke Moving to: " + activeSmokeMoving);
            var smokeMove = smokeParticle_Moving.emission;
            smokeMove.enabled = activeSmokeMoving;
        }
    }
}
