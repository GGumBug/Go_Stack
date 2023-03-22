using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerfectController : MonoBehaviour
{
    [SerializeField]
    private CubeSpawner cubeSpawner;
    [SerializeField]
    private Transform   perfectEffect;
    [SerializeField]
    private Transform   perfectComboEffect;
    [SerializeField]
    private Transform   perfectRecoveryEffect;

    private AudioSource audioSource;

    [SerializeField]
    private int     recoveryCombo = 5;
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
        Vector3 position = cubeSpawner.LastCube.position;
        position.y = cubeSpawner.CurrentCube.transform.position.y - cubeSpawner.CurrentCube.transform.localScale.y * 0.5f;

        Vector3 scale = cubeSpawner.CurrentCube.transform.localScale;
        scale = new Vector3(scale.x + addedSize, perfectEffect.localScale.y, scale.z + addedSize);

        OnEffectProcess(position, scale);

        if (perfectCombo > 0 && perfectCombo < recoveryCombo)
        {
            StartCoroutine(OnPerfectComboEffect(position, scale));
        }
        else if (perfectCombo > recoveryCombo)
        {
            OnPerfectRecoveryEffect();
        }
    }

    private void OnEffectProcess(Vector3 position, Vector3 scale)
    {
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

    private IEnumerator OnPerfectComboEffect(Vector3 position, Vector3 scale)
    {
        int currentCombo    = 0;
        float beginTime     = Time.time;
        float duration      = 0.15f;

        // true 인 동안 증감식이 발동한 횟수만큼 반복
        while ( currentCombo < perfectCombo)
        {
            //beginTime에서 duration 시간만큼 지나야 생성되는 방식
            float t = (Time.time - beginTime) / duration;

            if ( t >= 1)
            {
                Transform effect    = Instantiate(perfectComboEffect);
                effect.position     = position;
                effect.localScale   = scale;

                beginTime = Time.time;

                currentCombo ++;
            }

            yield return null;
        }
    }

    private void OnPerfectRecoveryEffect()
    {
        Transform effect = Instantiate(perfectRecoveryEffect);

        effect.position = cubeSpawner.CurrentCube.transform.position;

        // 반경 반지름과 두께 설정
        var shape = effect.GetComponent<ParticleSystem>().shape;
        float radius = cubeSpawner.CurrentCube.transform.localScale.x > cubeSpawner.CurrentCube.transform.localScale.z ? cubeSpawner.CurrentCube.transform.localScale.x : cubeSpawner.CurrentCube.transform.localScale.z;
        shape.radius = radius;
        shape.radiusThickness = radius * 0.5f;

        // 이동 큐브의 방향에 따라 일부분 회복
        cubeSpawner.CurrentCube.RecoveryCube();
    }
}
