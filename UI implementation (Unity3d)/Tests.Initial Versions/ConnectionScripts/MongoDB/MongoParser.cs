/**
*Script used to connect test the Unity3D to MongoDB connection.
*This script utilises the open source MongoDB driver v.1.11.
*@author TheHub
*DIT029 H16 Project: Software Architecture for Distributed Systems
*University of Gothenburg, Sweden 2016
*/
using UnityEngine;
using System.Collections;

using System;
using MongoDB.Bson;
using MongoDB.Driver;
using Random = UnityEngine.Random;

public class MongoParser : MonoBehaviour
{
    public class Player
    {
        public int score { get; set; }
    }
    private MongoClient client;
    private MongoServer server;
    private MongoDatabase db;
    private MongoCollection<Player> playercollection;

    private int counter = 0;
    private bool work = true;
	
	//initialise the MongoDB connection
    protected void Start()
    {
        client = new MongoClient(new MongoUrl("mongodb://localhost"));
        server = client.GetServer();
        server.Connect();
        db = server.GetDatabase("test_unity");
        playercollection = db.GetCollection<Player>("player");

        InsertRandomDocument();
    }

    //inserts a document to a MongoDB database
	//it inserts an object in the format of {"score":<(random)int>}
    public void InsertRandomDocument()
    {
        Player player = new Player
        {
            score = Random.Range(0,100),
        };

        playercollection.Insert(player);
    }
}
