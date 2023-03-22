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
    private int                 currentScore = 0;

    // 업데이트로 하지 않은것이 의문
    private IEnumerator Start()
    {
        while (true)
        {
            //테스트용 코드
            if (Input.GetMouseButtonDown(1))
            {
                if (cubeSpawner.CurrentCube != null)
                {
                    cubeSpawner.CurrentCube.transform.position = cubeSpawner.LastCube.position + Vector3.up*0.1f;

                    cubeSpawner.CurrentCube.Arrangement();
                    currentScore++;
                    uiController.UpdateScore(currentScore);
                }
                cameraController.MoveOneStep();
                cubeSpawner.SpawnCube();
            }

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
                        //Debug.Log("GameOver");

                        cameraController.GameOverAnimation(cubeSpawner.LastCube.position.y, OnGameOver);

                        yield break;
                    }

                    currentScore ++;
                    uiController.UpdateScore(currentScore);
                }

                cameraController.MoveOneStep();

                cubeSpawner.SpawnCube();
            }

            yield return null;
        }
    }

    private void OnGameOver()
    {
        int highScore = PlayerPrefs.GetInt("HighScore");

        if (currentScore > highScore)
        {
            PlayerPrefs.SetInt("HighScore", currentScore);
            uiController.GameOver(true);
        }
        else
        {
            uiController.GameOver(false);
        }

        StartCoroutine("AfterGameOver");
    }

    private IEnumerator AfterGameOver()
    {
        yield return new WaitForEndOfFrame();

        while (true)
        {
            if (Input.GetMouseButtonDown(0))
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene(0);
            }

            yield return null;
        }
    }
}
