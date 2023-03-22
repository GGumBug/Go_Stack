using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerfectController : MonoBehaviour
{
    [SerializeField]
    private CubeSpawner cubeSpawner;
    [SerializeField]
    private Transform   perfectEffect;

    private AudioSource audioSource;

    private float   perfectCorrection = 0.01f;
    private float   addedSize = 0.1f;
    private int     perfectCombo = 0;

    private void Awake() {
        audioSource = GetComponent<AudioSource>();
    }

    public bool IsPerfect(float hangOver)
    {
        if (Mathf.Abs(hangOver) <= perfectCorrection)
        {
            EffectProcess();
            SFXProcess();

            perfectCombo ++;

            return true;
        }
        else
        {
            perfectCombo = 0;

            return false;
        }
    }

    private void EffectProcess()
    {
        OnEffectProcess();
    }

    private void OnEffectProcess()
    {
        Vector3 position = cubeSpawner.LastCube.position;
        position.y = cubeSpawner.CurrentCube.transform.position.y - cubeSpawner.CurrentCube.transform.localScale.y * 0.5f;

        Vector3 scale = cubeSpawner.CurrentCube.transform.localScale;
        scale = new Vector3(scale.x + addedSize, perfectEffect.localScale.y, scale.z + addedSize);

        Transform effect    = Instantiate(perfectEffect);
        effect.position     = position;
        effect.localScale   = scale;
    }

    private void SFXProcess()
    {
        int maxCombo            = 5;
        float volumeMin         = 0.3f;
        float volumeAdditive    = 0.15f;
        float pitchMin          = 0.7f;
        float pitchAdditive     = 0.15f;

        if (perfectCombo < maxCombo)
        {
            audioSource.volume  = volumeMin + perfectCombo * volumeAdditive;
            audioSource.pitch   = pitchMin + perfectCombo * pitchAdditive;
        }

        audioSource.Play();
    }
}
