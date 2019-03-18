using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    private List<Vector3> pinPositions;
    private List<Quaternion> pinRotations;
    private Vector3 ballPosition;
    private Stack<GameObject> knockedPins;
    private List<int> scores;
    private Score[] realScores;

    private int points;
    private int total_points;
    private int round;
    private int turn;
    private short turnState;
    private float timer;
    private bool timerSet;
    private bool ballReset;

    public Text scoreText;
    public Text totalScoreText;
    public float waitTime;

    // Start is called before the first frame update
    void Start()
    {
        var pins = GameObject.FindGameObjectsWithTag("Pins");
        knockedPins = new Stack<GameObject>();
        pinPositions = new List<Vector3>();
        pinRotations = new List<Quaternion>();
        points = 0;
        total_points = 0;
        turnState = 0;
        scoreText.text = "Score: " + points;
        totalScoreText.text = "Total Score: " + total_points;
        foreach (var pin in pins)
        {
            pinPositions.Add(pin.transform.position);
            pinRotations.Add(pin.transform.rotation);
        }

        timerSet = false;
        ballReset = true;

        scores = new List<int>();
        realScores = new Score[10];
        for (int i = 0; i < 10; i++){
            realScores[i] = new Score();
        }

        ballPosition = GameObject.FindGameObjectWithTag("Ball").transform.position;
    }
    
    void Update() {
        if (turn > 10){
            scoreCounter();

            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #else
            Application.Quit();
            #endif
        }

        if (ballReset){
            switch(turnState){
                case 0: //Start new turn
                {
                    round = 0;
                    turn++;
                    startTurn();
                    break;
                }
                case 1: //Same turn, start second round
                {
                    round++;
                    startRound();
                    break;
                }
                default:
                    break;
            }
            ballReset = false;

        }

        if (GameObject.FindGameObjectWithTag("Ball").transform.position.z >= 101){
            timer += Time.deltaTime;
            timerSet = true;
        }

        if (timerSet && (timer >= waitTime)){
            // var pins = GameObject.FindGameObjectsWithTag("Pins");

            // for (int i = 0; i < pins.Length; i++){
            //     var pinPhysics = pins[i].GetComponent<Rigidbody>();

            //     if(pinPhysics.rotation != Quaternion.identity){
            //     	 knockedPins.Push(pins[i]);
            //     	 pins[i].SetActive(false);
            //     } else {
            //     	pinPhysics.velocity = Vector3.zero;
            //     	pinPhysics.position = pinPositions[i];
            //     	pinPhysics.rotation = pinRotations[i];
            //     	pinPhysics.velocity = Vector3.zero;
            //     	pinPhysics.angularVelocity = Vector3.zero;
            //     }
            // }

            points = knockedPins.Count;
            scoreText.text = "Score: " + points;
			totalScoreText.text = "Total Score: " + total_points;

            scoring();

            switch(turnState){
                case 0: //Start new turn
                    if (knockedPins.Count < 10){
                        turnState = 1;
                    } else {
                        turnState = 0;
                    }
                    break;
                case 1: //Same turn, start second round
                    turnState = 0;
                    break;
                default:
                    break;
            }
            timerSet = false;
            ballReset = true;
            timer = 0;
        }
        // //quit
        // if (Input.GetKeyUp(KeyCode.Escape))
        // {
        //     Application.Quit();
        // }

    }

    private void startTurn(){
       	var pins = GameObject.FindGameObjectsWithTag("Pins");
        	for (int i = 0; i < pins.Length; i++){
               
                var pinPhysics = pins[i].GetComponent<Rigidbody>();
                if(pinPhysics.rotation != Quaternion.identity){
                   	knockedPins.Push(pins[i]);
                   	pins[i].SetActive(false);
                }
            }

            total_points += knockedPins.Count;
            points = 0;

        	while(knockedPins.Count>0){
   				var knocked_pin = knockedPins.Pop();
   				knocked_pin.SetActive(true);
			}

			scoreText.text = "Score: " + points;
			totalScoreText.text = "Total Score: " + total_points;

			pins = GameObject.FindGameObjectsWithTag("Pins");

            for (int i = 0; i < pins.Length; i++){   
                var pinPhysics = pins[i].GetComponent<Rigidbody>();
                pinPhysics.velocity = Vector3.zero;
                pinPhysics.position = pinPositions[i];
                pinPhysics.rotation = pinRotations[i];
                pinPhysics.velocity = Vector3.zero;
                pinPhysics.angularVelocity = Vector3.zero;

                var ball = GameObject.FindGameObjectWithTag("Ball");
                ball.transform.position = ballPosition;
                ball.GetComponent<Rigidbody>().velocity = Vector3.zero;
                ball.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            }
    }

    private void startRound(){
        //next round
            var ball = GameObject.FindGameObjectWithTag("Ball");

            ball.GetComponent<Rigidbody>().velocity = Vector3.zero;
            ball.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            ball.transform.position = ballPosition;
        }

    private void scoring(){
        var pins = GameObject.FindGameObjectsWithTag("Pins");

        for (int i = 0; i < pins.Length; i++){
            var pinPhysics = pins[i].GetComponent<Rigidbody>();

            if(pinPhysics.rotation != Quaternion.identity){
            	knockedPins.Push(pins[i]);
            	pins[i].SetActive(false);
            } else {
                pinPhysics.velocity = Vector3.zero;
            	pinPhysics.position = pinPositions[i];
            	pinPhysics.rotation = pinRotations[i];
            	pinPhysics.velocity = Vector3.zero;
            	pinPhysics.angularVelocity = Vector3.zero;
            }
        }

        if (turnState == 0){
            scores.Add(knockedPins.Count);
        } else {
            scores.Add(knockedPins.Count - points);
        }
    }

    private void scoreCounter(){
        int element = 0;
        int[] scoresArray = scores.ToArray();

        for (int i = 0; i < scoresArray.Length; i++){
            int totalPoints;

            if (element > 0){
                totalPoints = realScores[element-1].totalPointsInteger;
            } else {
                totalPoints = 0;
            }
            if (scoresArray[i] == 10){   //strike
                realScores[element].secondRollPoints = "X";
                for (int j = i; j < i+3; j++){
                    if (j >= scoresArray.Length){
                        break;
                    }
                    totalPoints += scores[j];
                }
                realScores[element].totalPointsInteger = totalPoints;
            } else {
                if ((scoresArray[i] + scoresArray[i+1]) == 10){   //spare
                    realScores[element].firstRollPoints = scoresArray[i].ToString();
                    realScores[element].secondRollPoints = "/";

                    for (int j = i; j < i+3; j++){
                        if (j >= scoresArray.Length){
                            break;
                        }
                        totalPoints += scores[j];
                    }
                    realScores[element].totalPointsInteger = totalPoints;
                } else {
                    realScores[element].firstRollPoints = scoresArray[i].ToString();
                    realScores[element].secondRollPoints = scoresArray[i+1].ToString();
                    realScores[element].totalPointsInteger = totalPoints + scoresArray[i] + scoresArray[i+1];
                }
                i++;
            }
            element++;
        }
    }
}

public class Score {
    public string firstRollPoints;
    public string secondRollPoints;
    public int totalPointsInteger;
    public Score(){
        firstRollPoints = "";
        secondRollPoints = "";
        totalPointsInteger = 0;
    }
}