using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIController : MonoBehaviour
{
    [Header("Main")]
    [SerializeField]
    private GameObject mainPenel;

    [Header("InGame")]
    [SerializeField]
    private TextMeshProUGUI textCurrentScore;

    public void GameStart()
    {
        mainPenel.SetActive(false);

        textCurrentScore.gameObject.SetActive(true);
    }

    public void UpdateScore(int score)
    {
        textCurrentScore.text = score.ToString();
    }
}
