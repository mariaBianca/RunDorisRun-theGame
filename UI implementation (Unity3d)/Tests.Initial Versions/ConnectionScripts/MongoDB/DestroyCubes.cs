/**
*Script used to connect test the Unity3D to MongoDB connection.
*This script utilises the MongoParser script and the open source MongoDB driver v.1.11.
*@author TheHub
*DIT029 H16 Project: Software Architecture for Distributed Systems
*University of Gothenburg, Sweden 2016
*/

using UnityEngine;
using System.Collections;


public class DestroyCubes : MonoBehaviour
{
    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.name == "CratePink")
        {
            Destroy(col.gameObject);
        }
    }
	
	/*
	*	If the player collides with a game object, an object is added to the mongoDB
	*/
    void OnCollisionEnter2D(Collision2D coll)
    {
        MongoParser mong = gameObject.AddComponent<MongoParser>() as MongoParser;
        if (coll.gameObject.tag == "TestCrate")
        {
            //coll.gameObject.SendMessage("ApplyDamage", 10);
            //Destroy(coll.gameObject);
            mong.InsertRandomDocument();
        }
    }
}