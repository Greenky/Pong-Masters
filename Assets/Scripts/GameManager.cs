using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Wacki;

public class GameManager : MonoBehaviour {

	public int playerNumber;


	private GameObject	_player1, _player2;
	private AudioSource _winBeep, _loseBeep;
	private RobotBall	_robot = null;
    private	int			_player1Score, _player2Score, _hitAC, _hitBC, _hitRacketC = 0, _cupsCount = 7;
	[SerializeField] private bool		_gameInProcess	= false;
	[SerializeField] private bool		_gameOver	= false;
	[SerializeField] private bool		_isFirstThrow	= false;
	private bool		_arcadeCupsMode  = false;
	private bool		_greedToched  = false;
	private bool		_groundTouched = false;
	private bool		_player1RacketTouched = false;
	private float		_player1RacketTouchedTime = 0;
	private bool		_player2RacketTouched = false;
	private float		_player2RacketTouchedTime = 0;
	private float		_deltaTouchTime;
	private GameObject _controllerLeft;
	private GameObject _controllerRight;

	private int _countTouch = 0;

	public bool _flagClick;

	private string		_playTurn;

	[SerializeField]
	private TextMeshPro _scoreBoardPlayer1 = null, _scoreBoardPlayer2 = null;


	private int _touchesOnASide;
	private int _touchesOnBSide;

	private string _playerSide;

	// public string playerNumber;

	enum Difficulties { Easy = 3, Medium = 2, Hard = 1}

	private float _difficulty = 1;

	private void Awake()
	{
		_winBeep = GameObject.Find("WinBeep").GetComponent<AudioSource>();
		_loseBeep = GameObject.Find("LoseBeep").GetComponent<AudioSource>();
		_controllerLeft = GameObject.FindGameObjectWithTag("LeftController");
		Time.fixedDeltaTime = 0.01111f;

		if (SceneManager.GetActiveScene().name == "Preloader")
			Invoke("OnLoadGameMenu", 1.0f);
	}

	private void OnLevelWasLoaded(int level)
	{
		_flagClick = false;
		_controllerLeft = GameObject.FindGameObjectWithTag("LeftController");
		_controllerRight = GameObject.FindGameObjectWithTag("RightController");
		if (SceneManager.GetActiveScene().name == "GameWithCups")
		{
			_arcadeCupsMode = true;
			_robot = GameObject.Find("robot").GetComponent<RobotBall>();
		}
		else
			_arcadeCupsMode = false;
		if (SceneManager.GetActiveScene().name == "Lobby")
		{
			EndGame();
			_playTurn = "A side";
		}
			
	}

	private void FixedUpdate()
	{
		if (SceneManager.GetActiveScene().name == "GameWithCups")
			_arcadeCupsMode = true;
		else
			_arcadeCupsMode = false;
		if (GameObject.Find("Score_Player_1") && GameObject.Find("Score_Player_2"))
		{
			_scoreBoardPlayer1 = GameObject.Find("Score_Player_1").GetComponent<TextMeshPro>();
			_scoreBoardPlayer2 = GameObject.Find("Score_Player_2").GetComponent<TextMeshPro>();
			ChangeScoreBoard();
		}
	}


	/*
	 * Starting the game
	 */

	public void ChooseRandomSide()
	{
		if (Random.value >= 0.5)
		{
			if (GameObject.Find("PlayerTurn.Text"))
				GameObject.Find("PlayerTurn.Text").GetComponent<TextMeshPro>().text = "Player Turn";
			_playTurn = "A side";
		}
		else
		{
			if (GameObject.Find("PlayerTurn.Text"))
				GameObject.Find("PlayerTurn.Text").GetComponent<TextMeshPro>().text = "AI Bot Turn";
			_playTurn = "B side";
		}
			
	}

	public void SetDifficulty(string difficulty)
	{
		switch(difficulty.ToLower())
		{
			case "easy":
				_difficulty = (float)Difficulties.Easy;
				break;
			case "medium":
				_difficulty = (float)Difficulties.Medium;
				break;
			case "hard":
				_difficulty = (float)Difficulties.Hard;
				break;
			default:
				_difficulty = (float)Difficulties.Medium;
				break;
		}
	}

	public float GetDifficulty()
	{
		return _difficulty;
	}

	/*
	 * Counting toches in AI, bot game and online game
	 */

	public void StartGame()
	{
		if (GameObject.Find("InGameIndicator.Text"))
			GameObject.Find("InGameIndicator.Text").GetComponent<TextMeshPro>().text = "";
		if (GameObject.Find("PlayerTurn.Text"))
			GameObject.Find("PlayerTurn.Text").GetComponent<TextMeshPro>().text = "";
		if (SceneManager.GetActiveScene().name == "GameWithBot")
			_playTurn = "A side";
		ResetTouches();
		if (!_arcadeCupsMode)
			_isFirstThrow = true;
	}

	public string PlayTurn()
	{
		return _playTurn;
	}

	public void OutOfPlay(string action)
	{
		int points_sum;

		_winBeep.Play();
		points_sum = _player2Score + _player1Score;
		_gameInProcess = false;
		_isFirstThrow = false;
		if (SceneManager.GetActiveScene().name == "GameWithBot")
			_playTurn = "A side";
		else
		{
			if (points_sum != 0 && action == null)
			{
				if (points_sum % 2 == 0 && _playTurn == "A side")
					_playTurn = "B side";
				else if (points_sum % 2 == 0 && _playTurn == "B side")
					_playTurn = "A side";
			}
		}

		// Side panels ----------------------------------------

		if (GameObject.Find("InGameIndicator.Text"))
		{
			if (action == "Rethrow")
				GameObject.Find("InGameIndicator.Text").GetComponent<TextMeshPro>().text = "Rethrow";
			else
				GameObject.Find("InGameIndicator.Text").GetComponent<TextMeshPro>().text = "Not In Game";
		}
		if (GameObject.Find("PlayerTurn.Text"))
		{
			if (_playTurn == "B side")
				GameObject.Find("PlayerTurn.Text").GetComponent<TextMeshPro>().text = "AI Bot Turn";
			else
				GameObject.Find("PlayerTurn.Text").GetComponent<TextMeshPro>().text = "Player Turn";
		}
	}

	public void EndGame()
	{
		_cupsCount = 7;
		ResetTouches();
		_player1Score = 0;
		_player2Score = 0;
		_isFirstThrow = false;
		_gameInProcess = false;
		
	}



	public void RacketHit(string name)
	{
		if (InGame() && name == "Racket")
		{
			_player1RacketTouched = false;
			if ((_player2RacketTouched && Mathf.Abs(Time.time - _player2RacketTouchedTime) > 0.1f) ||
			((!_isFirstThrow || _playTurn == "B side") && _touchesOnASide == 0 && SceneManager.GetActiveScene().name == "AIScene"))
			{
				_player2RacketTouched = false;
				_player1Score++;
				OutOfPlay(null);
			}
			else
			{
				_player2RacketTouchedTime = Time.time;
				_player2RacketTouched = true;
			}
		}
		else if (InGame())
		{
			_player2RacketTouched = false;
			if ((_player1RacketTouched && Mathf.Abs(Time.time - _player1RacketTouchedTime) > 0.1f) ||
			((!_isFirstThrow || _playTurn == "A side") && _touchesOnBSide == 0 && SceneManager.GetActiveScene().name == "AIScene"))
			{
				_player1RacketTouched = false;
				_player2Score++;
				OutOfPlay(null);
			}
			else
			{
				_player1RacketTouchedTime = Time.time;
				_player1RacketTouched = true;
			}
		}
	}

	public void ResetTouches()
	{
		_greedToched = false;
		_groundTouched = false;
		_player1RacketTouched = false;
		_player2RacketTouched = false;
		_gameOver = false;
		_touchesOnASide = 0;
		_touchesOnBSide = 0;
		_countTouch = 0;
	}

	public void SideATouch()
	{
		_touchesOnASide++;
		if (_isFirstThrow)
		{
			if (_greedToched && _touchesOnASide == 1 && _touchesOnBSide == 1)
				OutOfPlay("Rethrow");
			if (_touchesOnASide == 1 && _touchesOnBSide == 1)
			{
				_gameInProcess = true;
				_isFirstThrow = false;
				ResetTouches();
				if (_playTurn == "A side")
					_touchesOnBSide = 1;
				else
					_touchesOnASide = 1;
			}
			else if (_touchesOnASide == 2)
			{
				if (_playTurn == "A side")
					_player1Score++;
				else
					_player2Score++;
				OutOfPlay(null);
			}
			else if (_playTurn == "B side" && _touchesOnBSide == 0)
			{
				_player2Score++;
				OutOfPlay(null);
			}
		}
		else if (_gameInProcess)
		{
			_touchesOnBSide = 0;
			if (_touchesOnASide == 2)
			{
				_player1Score++;
				OutOfPlay(null);
			}
		}
	}

	public void SideBTouch()
	{
		_touchesOnBSide++;
		if (_isFirstThrow)
        {
			
			if (_greedToched && _touchesOnASide == 1 && _touchesOnBSide == 1)
				OutOfPlay("Rethrow");
			else if (_touchesOnASide == 1 && _touchesOnBSide == 1)
			{
				_gameInProcess = true;
				_isFirstThrow = false;
				ResetTouches();
				if (_playTurn == "A side")
					_touchesOnBSide = 1;
				else
					_touchesOnASide = 1;
			}
			else if (_touchesOnBSide == 2)
			{
				if (_playTurn == "A side")
					_player1Score++;
				else
					_player2Score++;
				OutOfPlay(null);
			}
			else if (_playTurn == "A side" && _touchesOnASide == 0)
			{
				_player1Score++;
				OutOfPlay(null);
			}
				
		}
		else if (_gameInProcess)
		{
			_touchesOnASide = 0;
			if (_touchesOnBSide == 2)
			{
				_player2Score++;
				OutOfPlay(null);
			}
		}
	}

	public void GroundTouch()
	{
        if (_isFirstThrow)
        {
			if (_playTurn == "A side")
				_player1Score++;
			else
				_player2Score++;
			OutOfPlay(null);
		}
		else if (_gameInProcess)
		{
			if (_touchesOnASide == 1)
			{
				_player1Score++;
				OutOfPlay(null);
			}
			else if (_touchesOnBSide == 1)
			{
				_player2Score++;
				OutOfPlay(null);
			}
			else if (_playTurn == "A side")
			{
				_player2Score++;
				OutOfPlay(null);
			}
			else
			{
				_player1Score++;
				OutOfPlay(null);
			}
		}
	}

    public void GridTouch()
	{
		_greedToched = true;
	}

	public void hitRacket() {
        if(_hitRacketC >= 1) { _player2Score++; ChangeScoreBoard();  }
        else 
            _hitRacketC++;
    }

    public void hitFloor() {
		// Destroy the ball after some time
	}

	public void InTableArea()
	{
		// Checking if player hit the ball not above the table
		// (in theory)
	}

	public bool InGame()
	{
		if (SceneManager.GetActiveScene().name == "Lobby")
			return false;
		if (!_gameInProcess && !_isFirstThrow)
			return false;
		else
			return true;
	}


	/*
	 * Functions in Cups Mode
	 */

	public void CupTouch()
	{
		_cupsCount--;
		ChangeScoreBoard();
	}

	public bool IsArcadeCupsMode()
	{
		return _arcadeCupsMode;
	}



	/*
	 * Counting points or cups (in cups mode)
	 */

	private void ChangeScoreBoard()
	{
		if (_arcadeCupsMode)
		{
			_scoreBoardPlayer1.fontSize = 1.7f;
			_scoreBoardPlayer1.text = "TRIES";
			_scoreBoardPlayer2.text = _robot.triesCount.ToString();
			if (_cupsCount == 0)
			{
				_scoreBoardPlayer1.text = "YOU WIN";
				_scoreBoardPlayer2.fontSize = 1.5f;
				_scoreBoardPlayer2.text = "WITH " + _robot.triesCount.ToString() + " TRIES";
				_robot.gameObject.SetActive(false);
				_gameOver = true;
			}
		}
		else if (_player1Score >= 11 && SceneManager.GetActiveScene().name != "GameWithBot")
		{
			_scoreBoardPlayer1.fontSize = 1.9f;
			_scoreBoardPlayer1.text = "YOU";
			_scoreBoardPlayer2.fontSize = 1.9f;
			_scoreBoardPlayer2.text = "LOSE";
			_gameOver = true;
		}
		else if (_player2Score >= 11 && SceneManager.GetActiveScene().name != "GameWithBot")
		{
			_scoreBoardPlayer1.fontSize = 1.9f;
			_scoreBoardPlayer1.text = "YOU";
			_scoreBoardPlayer2.fontSize = 1.9f;
			_scoreBoardPlayer2.text = "WIN";
			_gameOver = true;
		}
		else
		{
			_scoreBoardPlayer1.fontSize = 3.5f;
			_scoreBoardPlayer1.text = _player1Score.ToString();
			_scoreBoardPlayer2.text = _player2Score.ToString();
		}
	}

	//private void OnMenuButtonClicked(object sender, ClickedEventArgs e)
	//{
	//	if (SceneManager.GetActiveScene().name != "Lobby" && SceneManager.GetActiveScene().name != "Preloader")
	//	{
	//		if (!_flagClick || _gameOver)
	//		{
	//			Time.timeScale = 0;
	//			GameObject.Find("Racket").transform.parent = _controllerRight.transform;
	//			_flagClick = true;
	//		}
	//		else
	//		{
	//			Time.timeScale = 1;
	//			GameObject.Find("Racket").transform.parent = GameObject.Find("_Dynamic").transform;
	//			_flagClick = false;
	//			_menuPanel.SetActive(false);
	//			if (_controllerLeft)
	//				_controllerLeft.GetComponent<ViveUILaserPointer>().isActiveLaser = false;
	//		}
	//	}
	//}

	private IEnumerator ToLobby()
	{
		yield return new WaitForSeconds(2);
		OnLoadGameMenu();
	}


	/*
	 * Functions on Buttons in menu
	 */

	public void OnLoadGameMenu()
	{
		Time.timeScale = 1;
		SceneManager.LoadScene("Lobby");
	}

	public void OnLoadGameCups()
	{
		Time.timeScale = 1;
		SceneManager.LoadScene("GameWithCups");
	}

	public void OnLoadGameBot()
	{
		Time.timeScale = 1;
		SceneManager.LoadScene("GameWithBot");
	}

	public void OnLoadAI()
	{
		Time.timeScale = 1;
		SceneManager.LoadScene("AIScene");
	}

	public void OnLoadRestart()
	{
		Time.timeScale = 1;
		if (SceneManager.GetActiveScene().name == "GameWithCups")
			SceneManager.LoadScene("GameWithCups");
		if (SceneManager.GetActiveScene().name == "GameWithBot")
			SceneManager.LoadScene("GameWithBot");
		if (SceneManager.GetActiveScene().name == "AIScene")
			SceneManager.LoadScene("AIScene");
	}


}