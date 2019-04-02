using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(SphereCollider))]


public class BallPhysics : MonoBehaviour {


	[SerializeField]  private Vector3	_velocity;
	[SerializeField]  private Vector3	_angularVelocity;


	[SerializeField] private AudioSource _defaultHit;
    [SerializeField] private AudioSource _racketHit;

	private Rigidbody _ballRigidbody;
    private Vector3   _previousPosition;

    [SerializeField] private GameObject Grid;

	private float     _height;
	private float     _collisionStartTime = 0;
	private bool     _tochedSideB = false; // bool to activate the bot
	private bool     _tochedSideA = false; // bool to activate the bot


	private GameManager _gameManager;
	private NetworkManager _networkManager;

	[SerializeField] private float 
        _airFriction, 
        _defaultFriction, 
        _racketFrictionSideA, 
        _racketFrictionSideB,
        _ballAngularDrag,
        _spinInfluenceCoeficient;

	
	private void Start() {

        _gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
		_networkManager = GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<NetworkManager>();
		_collisionStartTime = 0;
		GetComponent<Collider>().material.bounciness = 0.95f;
        _ballRigidbody = GetComponent<Rigidbody>();
		_ballRigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
		_previousPosition = _ballRigidbody.position;
		_ballRigidbody.angularDrag = _ballAngularDrag;
	}

	private void FixedUpdate () {
		float magnitude;

		_ballRigidbody = GetComponent<Rigidbody>();
#if UNITY_EDITOR
		Debug.DrawLine(_previousPosition, _ballRigidbody.position, Color.Lerp(Color.green, Color.red, _ballRigidbody.velocity.magnitude / 10), 4);
#endif
        _previousPosition = _ballRigidbody.position;

		magnitude = _ballRigidbody.velocity.magnitude;
		if (_ballRigidbody.velocity.magnitude > 15)
			magnitude = 15;
		_ballRigidbody.drag = Mathf.Pow(magnitude, 2) * _airFriction;

		_velocity = _ballRigidbody.velocity;
		_angularVelocity = _ballRigidbody.angularVelocity;


		_ballRigidbody.angularVelocity = Vector3.ClampMagnitude(_ballRigidbody.angularVelocity, 100);


		if (_ballRigidbody.angularVelocity.magnitude < 0.01f)
			_ballRigidbody.angularVelocity = Vector3.zero;
		_ballRigidbody.velocity = Quaternion.Euler(_ballRigidbody.angularVelocity * _spinInfluenceCoeficient) * _ballRigidbody.velocity;

		if (_ballRigidbody.velocity.y <= 0.7f && _ballRigidbody.velocity.y >= -2.1f)
			GetComponent<Collider>().material.bounciness = 0.70f;
		else
			GetComponent<Collider>().material.bounciness = 0.95f;
	}

	private void OnCollisionEnter(Collision collision) {
		if (SceneManager.GetActiveScene().name == "AIScene" || SceneManager.GetActiveScene().name == "OnlineScene")
			_collisionStartTime = Time.time;
		_height = 0;
		_tochedSideB = false;
		_tochedSideA = false;
		if (collision.gameObject.tag == "Racket")
			SteamVR_Controller.Input((int)GameObject.FindGameObjectWithTag("RightController").GetComponent<SteamVR_TrackedController>().controllerIndex).TriggerHapticPulse(1700);
		if (collision.collider.name == "Racket")
			_racketHit.Play();
		else
			_defaultHit.Play(); 
		if (_gameManager.IsArcadeCupsMode())
			return;
		switch (collision.collider.name) {
			case "Grid":
				_ballRigidbody.velocity *= 0.7f;
				_gameManager.GridTouch();
				break;

			case "SideA":
				_tochedSideA = true;
				_gameManager.SideATouch();
				break;

			case "SideB":
				_tochedSideB = true;
                _gameManager.SideBTouch();
				break;

			case "floor_roof":
                _gameManager.GroundTouch();
				break;
			case "Racket":
				//_ballRigidbody.velocity *= _racketFrictionSideA;
				_gameManager.RacketHit(collision.collider.name);
				break;
			default:
                break;
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.name == "AIRacket")
			_gameManager.RacketHit(other.name);
	}

	private void OnCollisionExit(Collision collision)
	{
		Rigidbody _rb = GetComponent<Rigidbody>();

		if (collision.gameObject.name == "Racket")
		{
			_networkManager.SendBallPhysics(_rb.position, _rb.rotation, _rb.velocity, _rb.angularVelocity);
		}
	}

	private void OnCollisionStay(Collision collision)
	{
		if (SceneManager.GetActiveScene().name == "AIScene" || SceneManager.GetActiveScene().name == "OnlineScene")
			if (Time.time - _collisionStartTime > 2)
				_gameManager.GroundTouch();	
	}

	public void ApplyPhysics(Vector3 position, Quaternion rotation, Vector3 velocity, Vector3 angularVelocity)
	{
		//if (transform.parent.name != "_Dynamic")
		//	transform.parent = GameObject.FindGameObjectWithTag("Dynamic").transform;
		GetComponent<Rigidbody>().position = position;
		GetComponent<Rigidbody>().rotation = rotation;
		GetComponent<Rigidbody>().velocity = velocity;
		GetComponent<Rigidbody>().angularVelocity = angularVelocity;
		_gameManager.RacketHit("AIRacket");
	}

	public bool TochedSideB()
	{
		return _tochedSideB;
	}

	public bool TochedSideA()
	{
		return _tochedSideA;
	}
}


