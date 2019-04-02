using UnityEngine.SceneManagement;
using UnityEngine;

using Wacki;

public class ActionController : MonoBehaviour {

	[SerializeField] private GameObject  _controller;
	[SerializeField] private GameObject  _tablePrefab;
	private GameObject _controllerRight;
	private GameObject  _ball;

	private bool        _holdBall = false;

    private Vector3     _velocity;
    private float       _speed;
    private Vector3     _oldPose;
    private Vector3     _newPose;
    private GameObject  _tempTable = null;

	private GameManager _gameManager;

    private Vector3     _gripStartPose;
    private Vector3     _gripChangePose;
    private Vector3     _positionDelta;
	private bool        _holdGrip = false;

	void Start() {
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        _controller.GetComponent<SteamVR_TrackedController>().TriggerClicked += HandleTriggerClicked;
        _controller.GetComponent<SteamVR_TrackedController>().TriggerUnclicked += HandleTriggerUnclicked;
        _controller.GetComponent<SteamVR_TrackedController>().Gripped += OnGriped;
        _controller.GetComponent<SteamVR_TrackedController>().Ungripped += OnUngriped;
		if (SceneManager.GetActiveScene().name == "GameWithCups" || SceneManager.GetActiveScene().name == "GameWithBot" || SceneManager.GetActiveScene().name == "AIScene")
		{
			if (_controller != null)
				_controller.GetComponent<ViveUILaserPointer>().pointer.SetActive(false);
			if (_controllerRight != null)
				_controllerRight.GetComponent<ViveUILaserPointer>().pointer.SetActive(false);
		}

	}

    private void Update() {
		_ball = GameObject.Find("Pong Ball");
        if (_ball && _holdBall && !_gameManager.InGame() && _gameManager.PlayTurn() == "A side" &&
		(SceneManager.GetActiveScene().name == "AIScene" || SceneManager.GetActiveScene().name == "Lobby"))
        {
			_ball.GetComponent<Transform>().position = _controller.transform.Find("spawnZone").transform.position;
			_ball.GetComponent<Transform>().rotation = _controller.transform.Find("spawnZone").transform.rotation;
			_ball.GetComponent<Rigidbody>().velocity = Vector3.zero;
			_ball.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
			_newPose = _controller.transform.position - _oldPose;
			_oldPose = _controller.transform.position;
			_speed = _newPose.magnitude / Time.deltaTime;
			_velocity = _newPose.normalized;
		}
		if (_holdGrip)
		{
			if (_tempTable)
			{
				_gripChangePose = _controller.transform.position - _gripStartPose;
				_gripStartPose = _controller.transform.position;
				_tempTable.transform.position = _tempTable.transform.position + _gripChangePose;
			}
		}
	}

	private void HandleTriggerClicked(object sender, ClickedEventArgs e)
	{
		if (_ball && Time.timeScale != 0 && !_gameManager.InGame() && _gameManager.PlayTurn() == "A side" &&
		(SceneManager.GetActiveScene().name == "AIScene" || SceneManager.GetActiveScene().name == "Lobby"))
		{
			_ball.SetActive(true);
			_ball.GetComponent<Rigidbody>().isKinematic = true;
			_holdBall = true;
			_gameManager.ResetTouches();
		}
	}

	private void OnGriped(object sender, ClickedEventArgs e)
	{
		GameObject _tableInScene;

		_tableInScene = GameObject.FindGameObjectWithTag("Pong Table");
		_gripStartPose = _controller.transform.position;
		_tempTable = Instantiate(_tablePrefab, _tableInScene.transform.position, _tableInScene.transform.rotation,  GameObject.Find("Props").transform);
		_holdGrip = true;
	}

	private void OnUngriped(object sender, ClickedEventArgs e)
	{
		Destroy(_tempTable);
		_positionDelta = GameObject.FindGameObjectWithTag("Pong Table").transform.position - _tempTable.transform.position;
		transform.position += _positionDelta;
		_holdGrip = false;
	}

	private void HandleTriggerUnclicked(object sender, ClickedEventArgs e)
	{
		if (_ball && Time.timeScale != 0 && !_gameManager.InGame() && _gameManager.PlayTurn() == "A side" &&
		(SceneManager.GetActiveScene().name == "AIScene" || SceneManager.GetActiveScene().name == "Lobby"))
		{
			_ball.GetComponent<Rigidbody>().isKinematic = false;
			_ball.GetComponent<Rigidbody>().velocity = _velocity * _speed;
			if (SceneManager.GetActiveScene().name != "Lobby")
				_gameManager.StartGame();
			_holdBall = false;
		}
	}
}

