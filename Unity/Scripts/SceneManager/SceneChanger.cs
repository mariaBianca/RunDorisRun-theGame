/**
Script that serves in changes the scenes within the game.
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
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour {


	//loads the registration scene
    public void LoadUserRegistrationScene()
    {
        SceneManager.LoadScene("userRegistrationView");
    }
    
	//loads the game options scene
    public void LoadOptionsScene()
    {
        SceneManager.LoadScene("Game Options");
    }

	//loads the high score scene
    public void LoadHighScoreScene()
    {
        SceneManager.LoadScene("HighScoreScreen");
    }

	//loads the welcome screne
    public void LoadWelcomeScene()
    {
        SceneManager.LoadScene("Welcome Screen v2");
    }

	//loads the chat scene
	public void LoadChatScene () {
		SceneManager.LoadScene ("ChatScene");

	}

	//loads the game scene
	public void LoadGameScene(){
		SceneManager.LoadScene ("Level 01");
	}


	//loads the game scene
	public void ResetPassword(){
		SceneManager.LoadScene ("PassReset");
	}

	//loads the change password scene
	public void ChangePassword(){
		SceneManager.LoadScene ("PassChange");
	}

	//Quits the game
	public void doExitGame()
	{
		Application.Quit();
	}

}


