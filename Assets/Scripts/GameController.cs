using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private CubeSpawner cubeSpawner;

    // 업데이트로 하지 않은것이 의문
    private IEnumerator Start()
    {
        while (true)
        {
            if (Input.GetMouseButtonDown(0))
            {
                cubeSpawner.SpawnCube();
            }

            yield return null;
        }
    }
}
