using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using Mono.Data.Sqlite;
using System.IO;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class MenuOptions : MonoBehaviour
{
    private string dbName = "URI=file:GameLog6.db";

    public Text scoreGame1;
    int points = 0;
    double time = 0;
    int lastPoints = 0;
    int lastTime = 0;

    // Start is called before the first frame update

    private void Start()
    {
        var scene = SceneManager.GetActiveScene().buildIndex;

        if(scene == 2)
        {
            ReadDB();
            GetCurrentScore();
            scoreGame1.text = $"Zakończyłeś \n poziom pierwszy \n\n Twój wynik: \n\n Czas: {lastTime} \n Uderzenia: {lastPoints}\n Najlepszy czas: {time}\n Najmniej uderzeń: {points}";
        }
    }

    private void ReadDB()
    {
        
        using (var connection = new SqliteConnection(dbName))
        {
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT MIN(time) FROM levelOne;";

                using (IDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        //Debug.Log(reader[0]);
                        //points = int.Parse(reader[0].ToString());
                        time = (double)reader[0];
                    }

                    reader.Close();
                }

                command.CommandText = "SELECT MIN(hits) FROM levelOne;";

                using (IDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        //Debug.Log(reader[0]);
                        points = int.Parse(reader[0].ToString());
                        //time = (double)reader[0];
                    }

                    reader.Close();
                }
            }

            connection.Close();
        }
    }

    private void GetCurrentScore()
    {
        using (var connection = new SqliteConnection(dbName))
        {
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT time FROM levelOne WHERE rowid = (SELECT MAX(rowid)  FROM levelOne); ";

                using (IDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Debug.Log(reader[0]);
                        //points = int.Parse(reader[0].ToString());
                        lastTime = (int)reader[0];
                    }

                    reader.Close();
                }

                command.CommandText = "SELECT hits FROM levelOne WHERE rowid = (SELECT MAX(rowid)  FROM levelOne); ";

                using (IDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Debug.Log(reader[0]);
                        //points = int.Parse(reader[0].ToString());
                        lastPoints = (int)reader[0];
                    }

                    reader.Close();
                }
            }

            connection.Close();
        }
    }
    public void ChangeScene()
    {
        Debug.Log("GUZIK!!!!");
        SceneManager.LoadScene(1);
    }

    public void GoToGame2()
    {
        SceneManager.LoadScene(3);
    }
}
