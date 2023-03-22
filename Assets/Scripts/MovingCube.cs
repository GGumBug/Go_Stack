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

        cubeSpawner.LastCube = this.transform;

        return false;
    }
}
