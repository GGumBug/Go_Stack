using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 상태나 페이지를 나눌때 사용하면 좋다.
public enum MoveAxis { x = 0, z}

public class CubeSpawner : MonoBehaviour
{
    [SerializeField]
    private Transform[]         cubeSpawnPoints; //큐브 생성 위치
    [SerializeField]
    private Transform           movingCubePrefab;
    [SerializeField]
    private PerfectController   perfectController;

    [field:SerializeField]
    public Transform        LastCube { set; get;}
    public MovingCube       CurrentCube {set; get;} = null;

    [SerializeField]
    private float           colorWeight = 15.0f; // 색상이 비슷한 정도
    // 완전히 다른 색상으로 변경하기 위한 횟수, 최대 횟수
    private int             currentColorNumberOfTime = 5;
    private int             maxColorNumberOfTime = 5;

    private MoveAxis        moveAxis = MoveAxis.x;

    public void SpawnCube()
    {
        Transform clone = Instantiate(movingCubePrefab);

        if (LastCube == null || LastCube.name.Equals("StartCubeTop"))
        {
            clone.position = cubeSpawnPoints[(int)moveAxis].position;
        }
        else
        {
            // float x = cubeSpawnPoints[(int)moveAxis].position.x;
            // float z = cubeSpawnPoints[(int)moveAxis].position.z;

            float x = moveAxis == MoveAxis.x ? cubeSpawnPoints[(int)moveAxis].position.x : LastCube.position.x;
            float z = moveAxis == MoveAxis.z ? cubeSpawnPoints[(int)moveAxis].position.z : LastCube.position.z;

            float y = LastCube.position.y + movingCubePrefab.localScale.y;

            clone.position = new Vector3(x,y,z);
        }

        clone.localScale = new Vector3(LastCube.localScale.x, movingCubePrefab.localScale.y, LastCube.localScale.z);

        clone.GetComponent<MeshRenderer>().material.color = GetRandomColor();

        clone.GetComponent<MovingCube>().SetUp(this, perfectController, moveAxis);

        // 나머지 활용 0,1 번갈아가며 돌아감
        moveAxis = (MoveAxis)(((int)moveAxis + 1) % cubeSpawnPoints.Length);

        //LastCube = clone;

        CurrentCube = clone.GetComponent<MovingCube>();
    }

    //시작점부터 끝점까지 와이어큐브로 라인을 그리는 함수
    private void OnDrawGizmos() {
        for (int i = 0; i < cubeSpawnPoints.Length; i++)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(cubeSpawnPoints[i].transform.position, movingCubePrefab.localScale);
        }
    }

    private Color GetRandomColor()
    {
        Color color = Color.white;

        if (currentColorNumberOfTime > 0)
        {
            float colorAmount = (1.0f/255.0f) * colorWeight; // color의 색상값은 0~1로 표현되기 때문에 1/255를 해준 뒤 colorWeight만큼 곱해준다.

            color = LastCube.GetComponent<MeshRenderer>().material.color;
            color = new Color(color.r - colorAmount, color.g - colorAmount, color.b - colorAmount);

            currentColorNumberOfTime --;
        }
        else
        {
            color = new Color(Random.value, Random.value, Random.value);

            currentColorNumberOfTime = maxColorNumberOfTime;
        }

        return color;
    }
}
