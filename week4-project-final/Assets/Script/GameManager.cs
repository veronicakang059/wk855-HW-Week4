using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // scene change set up
    public static GameManager Instance;
    public bool isGame = true;

    //level information
    public int CurrentLevel;

    //timer set up
    float Timer = 0;
    public float gameTime = 5;
    public Text timerText;

    // files paths
    const string DIR_LOGS = "/Logs";
    const string FILE_HIGH_SCORES = DIR_LOGS + "/highScores.txt";
    string FILE_PATH_HIGH_SCORE;

    // score set up
    public int score = 0;
    private List<int> highScores;

    // property set up
    //public int highScore = -1;
    public int targetScore = 3;
    public int currentLevel = 0;

    public Text ScoreNumber;
    /*public int Score
    {
        get { return score;}
        set
        {
            score = value;

            //Debug.Log("someone set the score!");
            if (score > Highscore)
            {
                Highscore = score;
            }
            
            string fileContents = "";
            if (File.Exists(FILE_PATH_ALL_SCORE)){
                fileContents = File.ReadAllText(FILE_PATH_ALL_SCORE);
            }
            fileContents += score + ",";
            File.WriteAllText(FILE_PATH_ALL_SCORE, fileContents);
        }
    }
    */
    // public int Highscore
    // {
    //  get
    //{
    //    if (highScore < 0)
    //   {
    //highScore = PlayerPrefs.GetInt(PREF_KEY_HIGH_SCORE, 2);
    //       if (File.Exists(FILE_PATH_HIGH_SCORE))
    //       {
    //           string fileContents = File.ReadAllText(FILE_PATH_HIGH_SCORE);
    //          highScore = Int32.Parse(fileContents);
    //       }
    //       else
    //      {
    //           highScore = 3;
    //      }
    //   }
    //   return highScore;
    //  }
    //  set
    // {
    //     highScore = value;
    //  Debug.Log("high score is updated!");
    //Debug.Log("filePath:" + FILE_PATH_HIGH_SCORE);
    //  PlayerPrefs.SetInt(key:PREF_KEY_HIGH_SCORE, highScore);
    //  File.WriteAllText(FILE_PATH_HIGH_SCORE,highScore+"");

    //  if (!File.Exists(FILE_PATH_HIGH_SCORE))
    //  {
    //      Directory.CreateDirectory(Application.dataPath + DIR_LOGS);
    //File.Create(FILE_PATH_HIGH_SCORE);
    //   }
    //  }
    //   }

    // awake to make sure one game manager in scene
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }

    private void Start()
    {
        Timer = 0;
        //PlayerPrefs.DeleteKey("hsKey");
        FILE_PATH_HIGH_SCORE = Application.dataPath + FILE_HIGH_SCORES;
    }

    void UpdateHighScore()
    {
        if (highScores == null)
        {
            highScores = new List<int>();
            string fileContents = File.ReadAllText(FILE_PATH_HIGH_SCORE);
            string[] fileScores = fileContents.Split(',');

            for (var i = 0; i < fileScores.Length-1; i++)
            {
                highScores[i] = Int32.Parse(fileScores[i]);
            }
        }

        // check all the score
        for (var i = 0; i < highScores.Count; i++)
        {
            if (score > highScores[i])
            {
                highScores.Insert(i, score);
                break;
            }
        }

        string saveHighScores = "";
        for (var i = 0; i < highScores.Count; i++)
        {
            saveHighScores += highScores[i] + "/n";
        }
        File.WriteAllText(FILE_PATH_HIGH_SCORE,saveHighScores);


    }

    // Update is called once per frame
    private void Update()
    {
        Timer += Time.deltaTime;

        //text changes
        if (!isGame)
        {
            string highScoreString = "Congrats!\n\n";
            for (var i = 0; i < highScores.Count; i++)
            {
                highScoreString += highScores[i] + "\n";
            }

            timerText.text = highScoreString;
            ScoreNumber.text = " Congrats!";

        }
        else
        {
            timerText.text = "You have " + (int) (gameTime - Timer) + " s left";
            ScoreNumber.text = "Level:" + currentLevel +
                               "\nScore:" + GameManager.Instance.score + " Target:" + targetScore;
        }

        if (score == targetScore)
        {
            currentLevel++;
            SceneManager.LoadScene(currentLevel);
            targetScore *= 2;
        }

        if (gameTime < Timer && isGame)
        {
            SceneManager.LoadScene(3);
            isGame = false;
        }
    }
}