using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private CubeSpawner         cubeSpawner;
    [SerializeField]
    private CameraController    cameraController;
    [SerializeField]
    private UIController        uiController;

    private bool                isGameStart = false;

    // 업데이트로 하지 않은것이 의문
    private IEnumerator Start()
    {
        while (true)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (!isGameStart)
                {
                    isGameStart = true;
                    uiController.GameStart();
                }

                if (cubeSpawner.CurrentCube != null)
                {
                    bool isGameOver = cubeSpawner.CurrentCube.Arrangement();
                    if (isGameOver)
                    {
                        Debug.Log("GameOver");
                    }
                }

                cameraController.MoveOneStep();

                cubeSpawner.SpawnCube();
            }

            yield return null;
        }
    }
}
