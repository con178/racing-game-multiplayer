using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CarDrift : MonoBehaviourPun
{
    private CarMovement carMovement;
    private SoundManager soundManager;
    public TrailRenderer[] trails;
    [HideInInspector] public bool isDrifting;

    void Start()
    {
        carMovement = GetComponent<CarMovement>();
        soundManager = GetComponent<SoundManager>();
    }
    void Update()
    {
        Drifting();
    }

    void Drifting()
    {
        if (carMovement.steeringValue > 0.9f || carMovement.steeringValue < -0.9f && carMovement.speed > 30f)
        {
            photonView.RPC("DrawDriftingLines", RpcTarget.All, true);
        }
        else if (carMovement.steeringValue < 0.9f || carMovement.steeringValue > -0.9f)
        {
            photonView.RPC("DrawDriftingLines", RpcTarget.All, false);
        }
    }
    [PunRPC]
    void DrawDriftingLines(bool isActive)
    {
        foreach (TrailRenderer trail in trails)
        {
            trail.emitting = isActive;
            isDrifting = isActive;
        }
    }


    public void ClearTrails()
    {
        for (int i = 0; i < trails.Length; i++)
        {
           trails[i].Clear();
        }
    }
}
