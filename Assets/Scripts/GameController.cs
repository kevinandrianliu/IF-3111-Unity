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
    private int points;
    private int total_points;
    private int round;
    private int turn;

    public Text scoreText;
    public Text totalScoreText;

    // Start is called before the first frame update
    void Start()
    {
        var pins = GameObject.FindGameObjectsWithTag("Pins");
        knockedPins = new Stack<GameObject>();
        pinPositions = new List<Vector3>();
        pinRotations = new List<Quaternion>();
        points = 0;
        total_points = 0;
        scoreText.text = "Score: " + points;
        totalScoreText.text = "Total Score: " + total_points;
        foreach (var pin in pins)
        {
            pinPositions.Add(pin.transform.position);
            pinRotations.Add(pin.transform.rotation);
        }

        ballPosition = GameObject.FindGameObjectWithTag("Ball").transform.position;
    }

     void Update() {

     	//reset for new round
        if (Input.GetKeyUp(KeyCode.R))
        {
        	var pins = GameObject.FindGameObjectsWithTag("Pins");

        	 for (int i = 0; i < pins.Length; i++)
            {
                
                var pinPhysics = pins[i].GetComponent<Rigidbody>();

                if(pinPhysics.rotation != Quaternion.identity) 
                {
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

            for (int i = 0; i < pins.Length; i++)
            {   
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


        //next round
         if (Input.GetKeyUp(KeyCode.B))
        {

            var ball = GameObject.FindGameObjectWithTag("Ball");

            ball.GetComponent<Rigidbody>().velocity = Vector3.zero;
            ball.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            ball.transform.position = ballPosition;
            
            var pins = GameObject.FindGameObjectsWithTag("Pins");

            for (int i = 0; i < pins.Length; i++)
            {
                
                var pinPhysics = pins[i].GetComponent<Rigidbody>();

                if(pinPhysics.rotation != Quaternion.identity) 
                {
                	 knockedPins.Push(pins[i]);
                	 pins[i].SetActive(false);
                }
                else {

                	pinPhysics.velocity = Vector3.zero;
                	pinPhysics.position = pinPositions[i];
                	pinPhysics.rotation = pinRotations[i];
                	pinPhysics.velocity = Vector3.zero;
                	pinPhysics.angularVelocity = Vector3.zero;

                }

            }

            points = knockedPins.Count;

            scoreText.text = "Score: " + points;
			totalScoreText.text = "Total Score: " + total_points;

        }


        if (Input.GetKeyUp(KeyCode.Backspace))
        {
            SceneManager.LoadScene("MainMenu");
        }

        //quit
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            Application.Quit();
        }

    }

}
