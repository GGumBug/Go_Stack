using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("One Step Move Parameters")]
    [SerializeField]
    private float   moveDistance = 0.1f;
    [SerializeField]
    private float   oneStepMoveTime = 0.25f;

    public void MoveOneStep()
    {
        Vector3 start = transform.position;
        Vector3 end = transform.position + Vector3.up * moveDistance;

        StartCoroutine(OnMoveTo(start, end, oneStepMoveTime));
    }

    private IEnumerator OnMoveTo(Vector3 start, Vector3 end, float time)
    {
        float current = 0;
        float percent = 0;

        while (percent < 1)
        {
            current += Time.deltaTime;
            percent = current / time;

            transform.position = Vector3.Lerp(start, end, percent);

            yield return null;
        }
    }
}
