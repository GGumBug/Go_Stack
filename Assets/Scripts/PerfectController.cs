using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerfectController : MonoBehaviour
{
    [SerializeField]
    private CubeSpawner cubeSpawner;
    [SerializeField]
    private Transform   perfectEffect;

    private float   perfectCorrection = 0.01f;
    private float   addedSize = 0.1f;
    private int     perfectCombo = 0;

    public bool IsPerfect(float hangOver)
    {
        if (Mathf.Abs(hangOver) <= perfectCorrection)
        {
            EffectProcess();

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
}
