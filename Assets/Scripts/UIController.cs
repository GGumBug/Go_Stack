using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [Header("Main")]
    [SerializeField]
    private GameObject mainPenel;

    public void GameStart()
    {
        mainPenel.SetActive(false);
    }
}
