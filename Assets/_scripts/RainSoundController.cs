using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainSoundController : MonoBehaviour
{
    public ParticleSystem raingenerator; // Ya�mur partik�l sisteminizi buraya atay�n.
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        // Ya�mur partik�l sisteminin �al���p �al��mad���n� kontrol edin.
        bool isRaining = raingenerator.isPlaying;

        // Ya�mur ses efektini �al��t�r�n veya durdurun
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
