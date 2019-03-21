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
    private int scoreOffset = 60;

    public float waitTime;

    public PlayerData playerData;
    public GameObject ball;
    public Image background;

    public Text firstRoundText;
    public Text secondRoundText;
    public Text totalTurnScoreText;
    public GameObject container;

    public Material background1;
	public Material background2;
	public Material background3;

    public GameObject endPanel;
    public InputField name;
    public Text score;
    public Button confirmButton;


    // Start is called before the first frame update
    void Start()
    {

    	setSettings();

        var pins = GameObject.FindGameObjectsWithTag("Pins");
        knockedPins = new Stack<GameObject>();
        pinPositions = new List<Vector3>();
        pinRotations = new List<Quaternion>();
        points = 0;
        total_points = 0;
        turnState = 0;
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
        confirmButton.onClick.AddListener(delegate {onConfirmButtonClick();});
    }
    
    void Update() {
        if (Input.GetKeyUp(KeyCode.Backspace)){
            SceneManager.LoadScene("MainMenu");
        }


        if (turn > 10){
            Vector3 position = endPanel.transform.localPosition;
            position.x = 0;
            endPanel.transform.localPosition = position;
            score.text = realScores[9].totalPointsInteger.ToString();
        } else {
            if (ballReset){
                switch(turnState){
                    case 0: //Start new turn
                    {
                        if (turn > 0)
                            scoreCounter();
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

            if (GameObject.FindGameObjectWithTag("Ball").transform.localPosition.z >= 101){
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
            //quit
            if (Input.GetKeyUp(KeyCode.Escape))
            {
                Application.Quit();
            }

            //restart
            if (Input.GetKeyUp(KeyCode.Backspace))
            {
                SceneManager.LoadScene("MainMenu");
            }
        }

        

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

        foreach (Transform child in container.transform){
            GameObject.Destroy(child.gameObject);
        }

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

        Color color = new Color(0.0f, 0.0f, 0.0f, 1.0f);

        for (int i = 0; i < 10; i++){
            Text firstRound = Instantiate(firstRoundText);
            Text secondRound = Instantiate(secondRoundText);
            Text totalScore = Instantiate(totalTurnScoreText);

            Vector3 firstRoundPosition = firstRound.transform.localPosition;
            Vector3 secondRoundPosition = secondRound.transform.localPosition;
            Vector3 totalScorePosition = totalScore.transform.localPosition;
            firstRoundPosition.x += (scoreOffset * i);
            secondRoundPosition.x += (scoreOffset * i);
            totalScorePosition.x += (scoreOffset * i);

            firstRound.transform.SetParent(container.transform);
            secondRound.transform.SetParent(container.transform);
            totalScore.transform.SetParent(container.transform);

            firstRound.transform.localPosition = firstRoundPosition;
            secondRound.transform.localPosition = secondRoundPosition;
            totalScore.transform.localPosition = totalScorePosition;

            firstRound.text = realScores[i].firstRollPoints.ToString();
            secondRound.text = realScores[i].secondRollPoints.ToString();
            totalScore.text = realScores[i].totalPointsInteger.ToString();

            firstRound.color = color;
            secondRound.color = color;
            totalScore.color = color;
        }

    }

    private void updateScore(){
        int childs = container.transform.childCount;

        foreach (Transform child in container.transform){
            GameObject.Destroy(child.gameObject);
        }
    }

    private void setSettings(){

    	playerData = PlayerPersistence.LoadData();

     	Color c = Color.black;
    	Renderer ball_render = ball.GetComponent<Renderer>();
    	
		if(playerData.colorBall == "red"){ 
			c.r += 0.1f;
		} else if(playerData.colorBall == "green") {
			c.g += 0.1f;
		} else {
			c.b += 0.1f;
		}
		ball_render.material.color = c;


			if(playerData.backgroundIndex == 0) {
					background.material = background1;
				} else if(playerData.backgroundIndex == 1) {
					background.material = background2;
			} else {
					background.material = background3;
			}
    }

    private void onConfirmButtonClick(){
        // Highscore stuffs
        SceneManager.LoadScene("MainMenu");
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
