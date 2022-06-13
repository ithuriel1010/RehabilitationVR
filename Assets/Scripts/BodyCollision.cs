using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using Mono.Data.Sqlite;
using System.IO;
using System;
using UnityEngine.SceneManagement;

public class BodyCollision : MonoBehaviour
{
    public AudioSource audioSource;
    private string dbName = "URI=file:GameLog6.db";
    private int hits = 0;
    private float startTime;

    bool keepTiming;
    float timer;
    // Start is called before the first frame update
    void Start()
    {
        CreateDB();

        ReadDB();
    }

    // Update is called once per frame
    void Update()
    {
        if (keepTiming)
        {
            UpdateTime();
        }
    }

    void UpdateTime()
    {
        timer = Time.time - startTime;
        //Debug.Log(timer);
    }

    void StartTimer()
    {
        keepTiming = true;
        startTime = Time.time;
    }

    void StopTimer()
    {
        Debug.Log("Level finished");

        keepTiming = false;

        UpdateDatabase();
        SceneManager.LoadScene(2);
    }

    /*IEnumerator time()
    {
        while (true)
        {
            timeCount();
            yield return new WaitForSeconds(1);
        }
    }
    void timeCount()
    {
        timer += 1;
        Debug.Log("Seconds:" + timer);
    }*/
    private void CreateDB()
    {
        using(var connection = new SqliteConnection(dbName))
        {
            connection.Open();

            using(var command = connection.CreateCommand())
            {
                command.CommandText = "CREATE TABLE IF NOT EXISTS levelOne (hits INT, time INT);";
                command.ExecuteNonQuery();
            }

            connection.Close();
        }
    }

    private void ReadDB()
    {
        int points=0;
        double time = 0;
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
                        Debug.Log(reader[0]);
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
                        Debug.Log(reader[0]);
                        points = int.Parse(reader[0].ToString());
                        //time = (double)reader[0];
                    }

                    reader.Close();
                }
            }

            connection.Close();
        }

        /*using (var connection = new SqliteConnection(dbName))
        {
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT MAX(hits) FROM levelOne;";

                using (IDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Debug.Log(reader[0]);
                        points = int.Parse(reader[0].ToString());
                        //time = (double)reader[0];
                    }

                    reader.Close();
                }
            }

            connection.Close();
        }*/

        Debug.Log("Min time:" + time);
        Debug.Log("Max points:" + points);
    }


    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("BANG");
        audioSource.Play();
        hits += 1;
    }

   /* private void OnApplicationQuit()
    {
        using (var connection = new SqliteConnection(dbName))
        {
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = "INSERT INTO levelOne(hits, time) VALUES (" + hits + ", " + timer +  ");";
                command.ExecuteNonQuery();
            }

            connection.Close();
        }
    }*/

    private void UpdateDatabase()
    {
        using (var connection = new SqliteConnection(dbName))
        {
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                //Debug.Log("INSERT INTO levelOne(hits, time) VALUES (" + hits + ", " + timer + ");");
                command.CommandText = "INSERT INTO levelOne(hits, time) VALUES (" + hits + ", " + timer + ");";
                command.ExecuteNonQuery();
            }

            connection.Close();
        }
    }
}
