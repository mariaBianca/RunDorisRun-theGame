/**
Script used to create an object of the player.
@author TheHub
DIT029 H16 Project: Software Architecture for Distributed Systems
University of Gothenburg, Sweden 2016

This file is part of "Run Doris Run!" game.
"Run Doris Run!" game is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

Run Doris Runis distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with "Run Doris Run!" game.  If not, see <http://www.gnu.org/licenses/>.
*/

using UnityEngine;
using System.Collections;

public class PlayerObject : MonoBehaviour {

    public string _id { get; set; }
    public new string name { get; set; }
    public string hashedEmail { get; set; }
    public string hashedPass { get; set; }

    public PlayerObject() { }

    public PlayerObject(string _Id, string Name, string HashedEmail, string HashedPass)
    {
        _id = _Id;
        name = Name;
        hashedEmail = HashedEmail;
        hashedPass = HashedPass;
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
