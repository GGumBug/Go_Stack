using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CameraController : MonoBehaviour
{
    [Header("One Step Move Parameters")]
    [SerializeField]
    private float   moveDistance = 0.1f;
    [SerializeField]
    private float   oneStepMoveTime = 0.25f;

    [Header("Game Over Parameters")]
    [SerializeField]
    private float   GameOverAnimationTime = 1.5f;
    [SerializeField]
    private float   limitMinY = 4;
    private Camera mainCamera;

    private void Awake() {
        mainCamera = Camera.main;
    }

    public void MoveOneStep()
    {
        Vector3 start = transform.position;
        Vector3 end = transform.position + Vector3.up * moveDistance;

        StartCoroutine(OnMoveTo(start, end, oneStepMoveTime));
    }

    public void GameOverAnimation(float lastCubeY, UnityAction action=null)
    {
        if (limitMinY > lastCubeY)
        {
            if (action != null) action.Invoke();
            
            return;
        }

        // 카메라 Y 위치 설정
        Vector3 startPosition = transform.position;
        Vector3 endPosition = new Vector3(transform.position.x, lastCubeY + 1, transform.position.z);
        // 카메라 이동 애니메이션
        StartCoroutine(OnMoveTo(startPosition, endPosition, GameOverAnimationTime));

        //카메라 View 크기 설정
        float startSize = mainCamera.orthographicSize;
        float endSize   = lastCubeY - 1;
        //카메라 View 크기 변경 애니메이션
        StartCoroutine(OnOrthographicSizeTo(startSize, endSize, GameOverAnimationTime, action));
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

    private IEnumerator OnOrthographicSizeTo(float start, float end, float time, UnityAction action)
    {
        float current   = 0;
        float percent   = 0;

        while (percent < 1)
        {
            current += Time.deltaTime;
            percent = current / time;

            mainCamera.orthographicSize = Mathf.Lerp(start, end, percent);

            yield return null;
        }

        if (action != null) action.Invoke();
    }
}
