using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text curPoints;
    public Text curRounds;
    public Text curAcc;

    public Text totPoints;
    public Text totRounds;
    public Text totAcc;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateUI(int points, int tPoints, int rounds, int tRounds)
    {
        curPoints.text = "Points: " + points.ToString();
        curRounds.text = "Rounds: " + rounds.ToString();

        float acc = Mathf.CeilToInt((System.Convert.ToSingle(points) / System.Convert.ToSingle(rounds))*100);

        curAcc.text = "Accuracy: " + acc.ToString();


        totPoints.text = "Points: " + tPoints.ToString();
        totRounds.text = "Rounds: " + tRounds.ToString();

        acc = Mathf.CeilToInt((System.Convert.ToSingle(tPoints) / System.Convert.ToSingle(tRounds)) * 100);

        totAcc.text = "Accuracy: " + acc.ToString();
    }
}
