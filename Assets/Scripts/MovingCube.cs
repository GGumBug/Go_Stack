using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingCube : MonoBehaviour
{
    [SerializeField]
    private float           moveSpeed = 1.5f;
    private Vector3         moveDirection;

    private CubeSpawner     cubeSpawner;
    private MoveAxis        moveAxis;

    public void SetUp(CubeSpawner cubeSpawner, MoveAxis moveAxis)
    {
        this.cubeSpawner = cubeSpawner;
        this.moveAxis = moveAxis;

        if (moveAxis == MoveAxis.x) moveDirection = Vector3.left;
        else if (moveAxis == MoveAxis.z) moveDirection = Vector3.back;
    }

    private void Update() {
        transform.position += moveDirection * moveSpeed * Time.deltaTime;

        if (moveAxis == MoveAxis.x)
        {
            if (transform.position.x <= -1.5f) moveDirection = Vector3.right;
            else if (transform.position.x >= 1.5f) moveDirection = Vector3.left;
        }
        else if (moveAxis == MoveAxis.z)
        {
            if (transform.position.z <= -1.5f) moveDirection = Vector3.forward;
            else if (transform.position.z >= 1.5f) moveDirection = Vector3.back;
        }
    }

    public bool Arrangement()
    {
        moveSpeed = 0;

        float hangOver = GetHangOver();

        // 어느쪽 방향으로 튀어 나왔는지 양수인지 음수인지로 판단
        float direction = hangOver >= 0 ? 1 : -1;

        if ( moveAxis == MoveAxis.x)
        {
            SplitCubeOnX(hangOver, direction);
        }
        else if (moveAxis == MoveAxis.z)
        {
            SplitCubeOnZ(hangOver, direction);
        }

        cubeSpawner.LastCube = this.transform;

        return false;
    }

    private float GetHangOver()
    {
        float amount = 0;

        if (moveAxis == MoveAxis.x)
        {
            amount = transform.position.x - cubeSpawner.LastCube.position.x;
        }
        else if (moveAxis == MoveAxis.z)
        {
            amount = transform.position.z - cubeSpawner.LastCube.position.z;
        }

        return amount;
    }

    private void SplitCubeOnX(float hangOver, float direction)
    {
        float newXPosition  = transform.position.x - (hangOver / 2);
        float newXScale     = transform.localScale.x - Mathf.Abs(hangOver);

        transform.position  = new Vector3(newXPosition, transform.position.y, transform.position.z);
        transform.localScale = new Vector3(newXScale, transform.localScale.y, transform.localScale.z);

        // 음수인지 양수인지 판단해서 어느쪽 부분이 절개선 수치인지 판단
        float cubeEbge = transform.position.x + (transform.localScale.x / 2 * direction);
        float fallingBlockSize = Mathf.Abs(hangOver);
        float fallingBlockSizePosition = cubeEbge + fallingBlockSize / 2 * direction;

        SpawnDropCube(fallingBlockSizePosition, fallingBlockSize);
    }

    private void SplitCubeOnZ(float hangOver, float direction)
    {
        float newZPosition = transform.position.z - (hangOver / 2);
        float newZSize = transform.localScale.z - Mathf.Abs(hangOver);

        transform.position = new Vector3(transform.position.x, transform.position.y, newZPosition);
        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, newZSize);

        float cubeEbge = transform.position.z + (transform.localScale.z / 2 * direction);
        float fallingBlockSize = Mathf.Abs(hangOver);
        float fallingBlockSizePosition = cubeEbge + fallingBlockSize / 2 * direction;

        SpawnDropCube(fallingBlockSizePosition, fallingBlockSize);
    }

    private void SpawnDropCube(float fallingBlockSizePosition, float fallingBlockSize)
    {
        // 큐브를 바로 생성하는 코드
        GameObject clone = GameObject.CreatePrimitive(PrimitiveType.Cube);

        if (moveAxis == MoveAxis.x)
        {
            clone.transform.position = new Vector3(fallingBlockSizePosition, transform.position.y, transform.position.z);
            clone.transform.localScale = new Vector3(fallingBlockSize, transform.localScale.y, transform.localScale.z);
        }
        else if (moveAxis == MoveAxis.z)
        {
            clone.transform.position = new Vector3(transform.position.x, transform.position.y, fallingBlockSizePosition);
            clone.transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, fallingBlockSize);
        }

        clone.GetComponent<MeshRenderer>().material.color = GetComponent<MeshRenderer>().material.color;
        clone.AddComponent<Rigidbody>();

        Destroy(clone, 2);
    }

}
