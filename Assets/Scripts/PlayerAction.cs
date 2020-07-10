using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerAction : MonoBehaviour
{
    //ui - relevant to this game only
    public Image bar;
    public Image pointZone;

    //these are the parameters that control where the "rightBar" will show up
    //this can be relevant to your game
    private float minRight;
    private float maxRight;

    //this is how big the "rightZone" is -- it is just the difference between minRight and maxRight above
    //in your game, this would be the gap between one plane and another
    private float sizePointZone;   

    //relevant to this game only - whether the bar is moving or not
    private bool isPlaying;

    //this is relevant to this game only - the position where the player stopped the bar
    private float playerHit;

    //relevant to this game only - the speed of the bar
    private float speed;


    /*
        THESE ARE ALL IMPORTANT FOR THE DYNAMIC DIFFICULTY

        //these are the variables that will control the player performance
        
        //points registers how many times the player pressed the button at the right time
        //rounds is how many rounds were played
        //reevaluationCycle is after how many rounds I will reevaluate the difficulty

       
        //the difference between points and totPoints is that "points" refer just for the current cycle
        //and "totPoints" to the whole play session ("points" is zeroed after each cycle).

        //this is "rounds" and "totRounds" is the same as above


        //difficultyThreshold is the main game balancing variable for the game
        //it is the parameter that defines whether the game is too hard or too difficult

         
         */
    private int reevaluationCycle;

    public float difficultyThreshold;

    private int totPoints;

    private int totRounds;

    private int points;

    private int rounds;


    private UIManager ui;

   

    // Start is called before the first frame update
    void Start()
    {        

        ui = this.GetComponent<UIManager>();

        #region gameAndUIRelatedVariablesInit

        //these are variables that are only relevant for this game
        //such as graphics for the bar to work

        isPlaying = false;

        bar.fillAmount = 0f;

        bar.fillMethod = Image.FillMethod.Horizontal;

        playerHit = 0;

        sizePointZone = 0.5f;

        speed = 1f;

        #endregion


        //these are the variables that will control the player performance
        
        //points registers how many times the player pressed the button at the right time
        //rounds is how many rounds were played
        //reevaluationCycle is after how many rounds I will reevaluate the difficulty

       
        //the difference between points and totPoints is that "points" refer just for the current cycle
        //and "totPoints" to the whole play session ("points" is zeroed after each cycle).

        //this is "rounds" and "totRounds" is the same as above

        points = 0;

        rounds = 0;

        reevaluationCycle = 6;

        totPoints = 0;

        totRounds = 0;

        //difficultyThreshold is the main game balancing variable for the game
        //it is the parameter that defines whether the game is too hard or too difficult
        difficultyThreshold = 0.5f;


        /*
           For example

           After 6 rounds, the player got 4 points (4 correct)

           4 / 6 = 0.66666

           0.6 is greater than "difficultyThreshold" (0.66 > 0.5)

           this means the game is too easy, so I have to make it more difficult
       */

        GenerateChallenge();

        ui.UpdateUI(points, totPoints, rounds, totRounds);
    }


    //this is all gameplay related
    void CalculateResults()
    {
        //increase rounds counters
        rounds++;
        totRounds++;

        //checks if the player pressed at the right time (or, more specifically, whether it was in
        //the right position)
        if ((playerHit >= minRight) && (playerHit <= maxRight))
        {
            points++;
            totPoints++;
        }

        //updates UI according to the current results
        ui.UpdateUI(points, totPoints, rounds, totRounds);

    }


    void MakeMoreDifficult()
    {

        //in my case, to make the game more difficult, I make the sizePointZone (the "gap") smaller
        //you might want to make it bigger

        sizePointZone = (sizePointZone * 3) / 4;

        //I also make the bar go faster

        speed = speed * 1.1f;

        //a final point that I could do is change the difficultyThreshold, 
        //making it more difficult for players to get to the next level

        difficultyThreshold += 0.05f;
    }


    void MakeEasier()
    {
        //conversely, to make it easier, I make the sizePointZone bigger,
        //you might want to make it smaller.

        sizePointZone = sizePointZone * 2;

        //I also make the bar go slower

        speed = speed * 0.8f;

        
    }



    void Reevaluate()
    {
        //this immense line generates a percentage of success of the player
        //(it divides the number of points by the number of rounds played)
        //if that percentage is greater than the threshold - set through a variable earlier
        //this means the game is too easy for the player, so we make it more difficult

        //if it is lower, it means the game is too hard - the player is failing
        //so we make it easier
        //see how these functions work above
        if (System.Convert.ToSingle(points)/System.Convert.ToSingle(rounds) >= difficultyThreshold)
        {
            MakeMoreDifficult();
        }
        else
        {
            MakeEasier();
        }

        //as I reevaluated, this means this cycle ended, so I need to reset my counters for this specific
        //cycle

        points = 0;
        rounds = 0;
    }



    void GenerateChallenge()
    {
        //this checks if you have played enough rounds in this cycle
        //if so, it will reevaluate the difficulty
        if (rounds > 0 && (rounds % reevaluationCycle == 0))
        {
            Reevaluate();
        }

        #region setChallengeAndGraphicsForPlayer

        //this is where I set the new "right position"       

        minRight = Random.Range(0, 1 - sizePointZone);

        maxRight = minRight + sizePointZone;
           
        //and here is where I show it as UI
        pointZone.rectTransform.anchorMin = new Vector2(minRight, 0.2f);
        pointZone.rectTransform.anchorMax = new Vector2(maxRight, 0.4f);
        pointZone.rectTransform.sizeDelta = bar.rectTransform.sizeDelta;

        #endregion


        isPlaying = true;

    }


    // Update is called once per frame
    void Update()
    {

        //you do not need to worry about this part of the code
        //this is just making this "game" work 

        if (isPlaying)
        {

            //this makes the bar go left and right after each frame

            bar.fillAmount = Mathf.Abs(Mathf.Sin(Time.fixedTime * speed));

            //if the player clicks when the bar is moving...
            if (Input.GetMouseButtonUp(0))
            {
                //it saves the position of the bar when the player clicked at this variable
                playerHit = bar.fillAmount;
                //changes the value of "isPlaying" to make the bar stop
                isPlaying = false;

                //calculates the result according to the position and the parameters
                //this function is detailed above, but it just checks if the player stopped the bar within
                //the space, if that is the case, the player scores a point
                CalculateResults();                
            }
        }
        else
        {
            //if the bar is not moving (therefore, "isPlaying" is false)
            //and the player clicks
            if (Input.GetMouseButtonUp(0))
            {
                
                //it will restart, first by generating a new challenge
                //(this function is detailed above)
                //and later changing the value of "isPlaying", to make the part above of the code
                //(the one that makes the bar go up and down) happen



                GenerateChallenge();
                isPlaying = true;
            }
        }

    }
}
