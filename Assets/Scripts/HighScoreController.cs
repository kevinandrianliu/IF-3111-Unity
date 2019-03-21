using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;
using UnityEngine.UI;
using Mono.Data.Sqlite;
using System.Data;

public class HighScoreController : MonoBehaviour {
    private readonly int RANKING_TEXT_X_OFFSET = -130;
    private readonly int Y_OFFSET = -100;
    private readonly int NAME_TEXT_X_OFFSET = -40;
    private readonly int SCORE_TEXT_X_OFFSET = 135;
    private List<ScoreList> scoreCollection;
    public GameObject rankingParent;
    public GameObject nameParent;
    public GameObject scoreParent;
    public Text rankingBaseText;
    public Text nameBaseText;
    public Text scoreBaseText;

	void Start() {
        int y = 25;
        scoreCollection = new List<ScoreList>();
        LoadHighScore();

        ScoreList[] scoreCollectionArray = scoreCollection.ToArray();
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

            rankingText.text = scoreCollectionArray[i].rank.ToString();
            nameText.text = scoreCollectionArray[i].name;
            scoreText.text = scoreCollectionArray[i].score.ToString();

            y -= 25;
        }
    }

    private void LoadHighScore(){
        // using (StreamReader reader = new StreamReader(SCORE_FILE_PATH)){
        //     string json = reader.ReadToEnd();
        //     scoreCollection = JsonUtility.FromJson<ScoreCollection>(json);
        // }

        string path = "URI=file:" + Application.persistentDataPath + "/highscore.s3db";

        IDbConnection dbConn = new SqliteConnection(path);
        IDbCommand dbCommand = dbConn.CreateCommand();

        dbConn.Open();

        // dbCommand.CommandText = "DELETE FROM Score";
        // dbCommand.ExecuteNonQuery();

        // dbCommand.CommandText = "INSERT INTO Score VALUES (1,\"A\",0)";
        // dbCommand.ExecuteNonQuery();
        // dbCommand.CommandText = "INSERT INTO Score VALUES (2,\"B\",0)";
        // dbCommand.ExecuteNonQuery();
        // dbCommand.CommandText = "INSERT INTO Score VALUES (3,\"C\",0)";
        // dbCommand.ExecuteNonQuery();
        // dbCommand.CommandText = "INSERT INTO Score VALUES (4,\"D\",0)";
        // dbCommand.ExecuteNonQuery();
        // dbCommand.CommandText = "INSERT INTO Score VALUES (5,\"E\",0)";
        // dbCommand.ExecuteNonQuery();
        // dbCommand.CommandText = "INSERT INTO Score VALUES (6,\"F\",0)";
        // dbCommand.ExecuteNonQuery();
        // dbCommand.CommandText = "INSERT INTO Score VALUES (7,\"G\",0)";
        // dbCommand.ExecuteNonQuery();
        // dbCommand.CommandText = "INSERT INTO Score VALUES (8,\"H\",0)";
        // dbCommand.ExecuteNonQuery();
        // dbCommand.CommandText = "INSERT INTO Score VALUES (9,\"I\",0)";
        // dbCommand.ExecuteNonQuery();
        // dbCommand.CommandText = "INSERT INTO Score VALUES (10,\"J\",0)";
        // dbCommand.ExecuteNonQuery();

        dbCommand.CommandText = "SELECT * FROM Score";
        IDataReader reader = dbCommand.ExecuteReader();
        while (reader.Read()){
            scoreCollection.Add(new ScoreList(reader.GetInt32(0),reader.GetString(1),reader.GetInt32(2)));
        }
        dbConn.Close();
    }
}

public class ScoreList {
    public int rank;
    public string name;
    public int score;

    public ScoreList(int rank, string name, int score){
        this.rank = rank;
        this.name = name;
        this.score = score;
    }
}