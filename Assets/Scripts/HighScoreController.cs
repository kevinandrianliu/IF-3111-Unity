using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;
using UnityEngine.UI;

public class HighScoreController : MonoBehaviour {
    private readonly int RANKING_TEXT_X_OFFSET = -130;
    private readonly int Y_OFFSET = -100;
    private readonly int NAME_TEXT_X_OFFSET = -40;
    private readonly int SCORE_TEXT_X_OFFSET = 135;
    private readonly string SCORE_FILE_PATH = "Assets/Data/" + "Score.json";
    private ScoreCollection scoreCollection;
    public GameObject rankingParent;
    public GameObject nameParent;
    public GameObject scoreParent;
    public Text rankingBaseText;
    public Text nameBaseText;
    public Text scoreBaseText;

	void Start() {
        int y = 25;
        LoadHighScore();

        for (int i = 0; i < 10; i++){
            Text rankingText = Instantiate(rankingBaseText);
            Text nameText = Instantiate(nameBaseText);
            Text scoreText = Instantiate(scoreBaseText);
        
            rankingText.transform.SetParent(rankingParent.transform);
            nameText.transform.SetParent(nameParent.transform);
            scoreText.transform.SetParent(scoreParent.transform);

            rankingText.transform.localPosition = new Vector3(RANKING_TEXT_X_OFFSET,Y_OFFSET+y,0f);
            nameText.transform.localPosition = new Vector3(NAME_TEXT_X_OFFSET,Y_OFFSET+y,0f);
            scoreText.transform.localPosition = new Vector3(SCORE_TEXT_X_OFFSET,Y_OFFSET+y,0f);

            rankingText.text = (i+1).ToString();
            nameText.text = scoreCollection.scores[i].name;
            scoreText.text = scoreCollection.scores[i].score.ToString();

            y -= 25;
        }
    }

    private void LoadHighScore(){
        using (StreamReader reader = new StreamReader(SCORE_FILE_PATH)){
            string json = reader.ReadToEnd();
            scoreCollection = JsonUtility.FromJson<ScoreCollection>(json);
        }
    }
}

[Serializable]
public class ScoreCollection {
    public ScoreList[] scores;

    public ScoreCollection() { }
}

[Serializable]
public class ScoreList {
    public string name;
    public int score;

    public ScoreList(string name, int score){
        this.name = name;
        this.score = score;
    }
}