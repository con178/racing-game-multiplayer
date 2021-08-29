using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
	[SerializeField] private AudioSource engineSoundSrc;
    [SerializeField] private AudioSource driftSoundSrc;
    public bool canPlayEngineSound = true;
    public bool canPlayDriftSound = true;
    private float soundPitch;
    [SerializeField] private const float minPitch = 1f;
    [SerializeField] private const float maxPitch = 2.64f;
    private float timer = 0f;
    [SerializeField] private float driftAudioLength = 0.5f;

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
        DriftAudio();
    }

    void EngineAudio()
    {
        if (canPlayEngineSound)
        {
            soundPitch = (carMovement.speed / 17f);
            engineSoundSrc.pitch = carDrift.isDrifting ? (soundPitch - 0.2f) : soundPitch;
            engineSoundSrc.pitch = Mathf.Clamp(engineSoundSrc.pitch, minPitch, maxPitch);
        }
    }

    void DriftAudio()
    {
        if(canPlayDriftSound)
        {
            if(!driftSoundSrc.isPlaying && carDrift.isDrifting)
            {
                AudioClip clip = driftSoundSrc.clip;
                driftSoundSrc.PlayOneShot(clip);
            }
            else if(driftSoundSrc.isPlaying && !carDrift.isDrifting)
            {
                timer += Time.deltaTime;
                if (timer > driftAudioLength)
                {
                    timer = 0;
                    driftSoundSrc.Stop();
                }
            }
        }
    }
}
