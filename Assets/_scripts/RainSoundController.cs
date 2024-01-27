using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainSoundController : MonoBehaviour
{
    public ParticleSystem raingenerator; // Yaðmur partikül sisteminizi buraya atayýn.
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        // Yaðmur partikül sisteminin çalýþýp çalýþmadýðýný kontrol edin.
        bool isRaining = raingenerator.isPlaying;

        // Yaðmur ses efektini çalýþtýrýn veya durdurun
        if (isRaining && !audioSource.isPlaying)
        {
            audioSource.Play();
        }
        else if (!isRaining && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }
}
