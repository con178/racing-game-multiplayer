using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
	[SerializeField] private AudioSource engineSoundSrc;
	public bool canPlaySFX = true;
    [SerializeField] private float soundPitch;
    [SerializeField] private const float minPitch = 0.69f;
    [SerializeField] private const float maxPitch = 1.4f;
    private CarDrift carDrift;
    private CarMovement carMovement;

    void Start()
    {
        carDrift = GetComponent<CarDrift>();
        carMovement = GetComponent<CarMovement>();
    }

    void Update()
    {
        EngineAudio();
    }

    void EngineAudio()
    {
        if (canPlaySFX)
        {
            soundPitch = (carMovement.speed / 31.818f);
            engineSoundSrc.pitch = carDrift.isDrifting ? (soundPitch - 0.08f) : soundPitch;
            engineSoundSrc.pitch = Mathf.Clamp(engineSoundSrc.pitch, minPitch, maxPitch);
        }
    }
}
