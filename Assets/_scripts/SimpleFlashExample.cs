using BarthaSzabolcs.Tutorial_SpriteFlash;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleFlashExample : MonoBehaviour
{
    [SerializeField] private SimpleFlash flashEffect;
    [SerializeField] private KeyCode flashKey;
    [SerializeField] private Material flashMaterial;
    [SerializeField] private float duration;
    private SpriteRenderer spriteRenderer;
    private Material originalMaterial;
    private Coroutine flashRoutine;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        originalMaterial = spriteRenderer.material;
    }
    private void Update()
    {


        if (Input.GetKeyDown(flashKey))
        {
            flashEffect.Flash();
        }
    }
}
