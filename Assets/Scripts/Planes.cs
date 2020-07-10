using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class LevelManager : MonoBehaviour
{    
    static public LevelManager lm;
    public GameObject plane;
    Vector3 pos = Vector3.zero;

    public List<GameObject> planes;


    private float minRange;

    private float maxRange;


    private float totalDistanceYouWantToHave;


    public float thresholdToIncreaseDifficulty;

    private void Awake()
    {
        lm = this;
    }



    public void Start()
    {

        totalDistanceYouWantToHave = 20f;

        minRange = 5f;
        maxRange = 15f;
        MakePlane();
    }


    


    public void MakePlane()
    {
        Vector3 initialPosition = new Vector3(0, -20, 1);

        //pos += new Vector3(0, 0, Random.Range(5f, 15f));

        /*
         * Here: you'll need to randomise the position through variables, something like below
         * */


        pos += new Vector3(0, 0, Random.Range(minRange, maxRange));

        Instantiate(planes[Random.Range(0, planes.Count)], pos, plane.transform.rotation);

    }
}