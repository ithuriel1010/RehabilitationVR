using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using Mono.Data.Sqlite;
using System.IO;
using System;

public class BodyCollision : MonoBehaviour
{
    public AudioSource audioSource;
    private string dbName = "URI=file:GameLog.db";
    private int hits = 0;
    // Start is called before the first frame update
    void Start()
    {
        CreateDB();

        ReadDB();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void CreateDB()
    {
        using(var connection = new SqliteConnection(dbName))
        {
            connection.Open();

            using(var command = connection.CreateCommand())
            {
                command.CommandText = "CREATE TABLE IF NOT EXISTS levelOne (hits INT);";
                command.ExecuteNonQuery();
            }

            connection.Close();
        }
    }

    private void ReadDB()
    {
        int points=0;
        using (var connection = new SqliteConnection(dbName))
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
                    }

                    reader.Close();
                }

            }

            connection.Close();
        }

        Debug.Log("Max hits:" + points);
    }


    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("BANG");
        audioSource.Play();
        hits += 1;
    }

    private void OnApplicationQuit()
    {
        using (var connection = new SqliteConnection(dbName))
        {
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = "INSERT INTO levelOne(hits) VALUES ("+ hits + ");";
                command.ExecuteNonQuery();
            }

            connection.Close();
        }
    }
}
