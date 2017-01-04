/**
Script used to pause and exit the game, or exit the application.
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
using UnityEngine.SceneManagement;


public class PauseResumeExitGame : MonoBehaviour {

	public bool paused;

	void Start(){
		paused = false;
	}

	void Update(){

		//close the game
		if (Input.GetKey(KeyCode.Escape)) {
			SceneManager.LoadScene ("Game Options");
		}

		//close the application
		if (Input.GetKey (KeyCode.LeftAlt) && (Input.GetKey( KeyCode.F4))) {
			Application.Quit ();
		}

		//pause the game
		if (Input.GetKeyDown (KeyCode.Space)) {
			paused = !paused;
		}

		//logic for the pause
		if (paused) {
			Time.timeScale = 0;
		} else if (!paused) {
			Time.timeScale = 1;
		}

	}


}
