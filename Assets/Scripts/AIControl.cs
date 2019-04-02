using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(Animator))]
public class AIControl : MonoBehaviour
{
	[Header("Close Attacks")]
	[Space]
	[SerializeField] private Vector3 _forwardCloseAttackForce	= new Vector3(0, 20, 35);
	[SerializeField] private Vector3 _leftCloseAttackForce		= new Vector3(-5, 20, 35);
	[SerializeField] private Vector3 _rightCloseAttackForce		= new Vector3(5, 20, -35);

	[Header("Far Attacks")]
	[Space]
	[SerializeField] private Vector3 _forwardFarAttackForce	= new Vector3(0, 25, 40);
	[SerializeField] private Vector3 _leftFarAttackForce	= new Vector3(-5, 25, 40);
	[SerializeField] private Vector3 _rightFarAttackForce	= new Vector3(5, 25, 40);

	[Header("Low Attacks")]
	[Space]
	[SerializeField] private Vector3 _forwardLowAttackForce	= new Vector3(0, 20, 20);
	[SerializeField] private Vector3 _leftLowAttackForce	= new Vector3(-5, 25, 25);
	[SerializeField] private Vector3 _rightLowAttackForce	= new Vector3(5, 20, 20);

	private GameObject		_ball			= null;
	[SerializeField] private GameObject		_AIBody			= null;
	private GameManager	_gameManager	= null;

	[SerializeField] private float _speed = 5.0f;

	private float _leftXBorder = -1.2f;
	private float _rightXBorder = 1.2f;
	private float _upperYBorder = 1.6f;
	private float _lowerYBorder = 0.9f;

	public bool _ballThrowFlag = true;
	public bool _isFirstAttack = false;


	private float _startTime;
	private float journeyLength;
	private Vector3 _startPosition;
	private Quaternion _startRotation;

	private Vector3 _ballStartPosition;

	float leftX = 1f;
	float rightX = -0.3f;

	public float difficulty = 0.5f;

	private Animator _anim;
	private int _omaewaHash = Animator.StringToHash("PlayDead");
	private int _waveHash = Animator.StringToHash("PlayWave");
	private void Start()
	{
		_anim = GetComponent<Animator>();
		_ballThrowFlag = true;
		if (!_ball)
			_ball = GameObject.Find("Pong Ball"); // need to change to unique ball prefab in scene (InitBall or Pong Ball)
		if (!_gameManager)
			_gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
		_startTime = Time.time;
		_gameManager.ChooseRandomSide();
		_startPosition = transform.position;
		_startRotation = transform.rotation;
		_ballStartPosition = _ball.transform.position;
		_gameManager.EndGame();
		_ball.GetComponent<Rigidbody>().isKinematic = true;
		//_gameManager.SetDifficulty("easy");
		//_gameManager.SetDifficulty("medium");
		_gameManager.SetDifficulty("hard");

		_speed /= (_gameManager.GetDifficulty() * 2);
		//if (!_ballThrowFlag)
		//	_anim.SetTrigger(_waveHash);
		//else
		//_anim.SetTrigger(_omaewaHash);

	}

	private void FixedUpdate()
	{
		if (!_gameManager.InGame() && _ballThrowFlag && _gameManager.PlayTurn() == "B side")
		{
			_ballThrowFlag = false;
			StartCoroutine(FirstThrow());
		}
		else
			RegularBehaviour();
	}

	private IEnumerator FirstThrow()
	{
		yield return new WaitForSeconds(3);
		_ball.GetComponent<Rigidbody>().isKinematic = false;
		_ball.GetComponent<Rigidbody>().velocity = Vector3.zero;
		_ball.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
		_ball.transform.position = _ballStartPosition;
		_ballThrowFlag = true;
		_gameManager.StartGame();
		_ball.GetComponent<Rigidbody>().AddForce(0, 35f, -1f);
		_isFirstAttack = true;
	}


	private void RegularBehaviour()
	{
		Vector3 newPosition;

		if (_ball.transform.position.z > _startPosition.z && _ball.transform.position.y > 0.7 && _ball.transform.position.y < 3
		&& _ball.GetComponent<Rigidbody>().velocity.z < 0 && _ball.GetComponent<BallPhysics>().TochedSideB())
		{
			// Racket movement
			float dZ = (_ball.transform.position.z - transform.position.z) / 20f;
			newPosition = new Vector3(_ball.transform.position.x, _ball.transform.position.y, transform.position.z + dZ);

			Vector3 aroundPoint = new Vector3(transform.position.x + 0.13f, transform.position.y - 0.215f, transform.position.z);

			transform.Rotate(new Vector3(0, 0, transform.position.x * (-2)));
			transform.position = Vector3.MoveTowards(transform.position, newPosition, _speed);

			// AIBody movement
			_AIBody.transform.position = Vector3.MoveTowards(_AIBody.transform.position, new Vector3(transform.position.x - 0.25f,
																										_AIBody.transform.position.y,
																										_AIBody.transform.position.z), _speed);
			if ((transform.position - _ball.transform.position).magnitude < 0.2f)
			{
				GetComponent<Animator>().SetTrigger("LeftAttack");
			}
		}
		else
		{
			transform.rotation = Quaternion.Lerp(transform.rotation, _startRotation, _speed * 2);
			transform.position = Vector3.MoveTowards(transform.position, _startPosition, _speed / 10);
			_AIBody.transform.position = Vector3.MoveTowards(_AIBody.transform.position, new Vector3(transform.position.x - 0.25f,
																										_AIBody.transform.position.y,
																										_AIBody.transform.position.z), _speed);
		}
	}

	private void ResetRacket()
	{
		transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.identity, _speed * 2);
		transform.position = Vector3.MoveTowards(transform.position, _startPosition, _speed);
	}

	private void ResetBall(GameObject ball)
	{
		Rigidbody ballRb = ball.GetComponent<Rigidbody>();

		ballRb.velocity = Vector3.zero;
		ballRb.angularVelocity = Vector3.zero;
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Ball")
		{
			ResetBall(other.gameObject);

			float rand = Random.value * 100;
			if (_isFirstAttack)
			{
				ForwardLowAttack();
				_isFirstAttack = false;
			}
			else if (IsRightZone())
			{
				if (rand % 2 == 0)
					ForwardAttack();
				else
					LeftAttack();
			}
			else if (IsLeftZone())
			{
				if (rand % 2 == 0)
					ForwardAttack();
				else
					RightAttack();
			}
			else if (IsFarRight())
				LeftAttack();
			else if (IsFarLeft())
				RightAttack();
			else if (IsCenterZone())
			{
				if (rand % 3 == 0)
					ForwardAttack();
				else if (rand % 2 == 0)
					RightAttack();
				else
					LeftAttack();
			}
		}
	}

	private bool IsLeftZone()
	{
		return (transform.localPosition.x > -3.45f && transform.localPosition.x < -3.15f);
	}

	private bool IsRightZone()
	{
		return (transform.localPosition.x > -2.55f && transform.localPosition.x < -2.25f);
	}

	private bool IsCenterZone()
	{
		return (transform.localPosition.x >= -3.15f && transform.localPosition.x <= -2.55f);
	}

	private bool IsFarLeft()
	{
		return (transform.localPosition.x <= -3.45f);
	}

	private bool IsFarRight()
	{
		return (transform.localPosition.x >= -2.25f);
	}

	private void ForwardAttack()
	{
		if (Mathf.Abs(transform.position.z - _startPosition.z) > 0.6f)
			ForwardCloseAttack();
		else
			ForwardFarAttack();
	}

	private void LeftAttack()
	{
		if (Mathf.Abs(transform.position.z - _startPosition.z) > 0.6f)
			LeftCloseAttack();
		else
			LeftFarAttack();
	}

	private void RightAttack()
	{
		if (Mathf.Abs(transform.position.z - _startPosition.z) > 0.6f)
			RightCloseAttack();
		else
			RightFarAttack();
	}

	public void LeftFarAttack()
	{
		_ball.GetComponent<Rigidbody>().AddForce(_leftFarAttackForce);
	}

	public void RightFarAttack()
	{
		_ball.GetComponent<Rigidbody>().AddForce(_rightFarAttackForce);
	}

	public void ForwardFarAttack()
	{
		_ball.GetComponent<Rigidbody>().AddForce(_forwardFarAttackForce);
	}

	// Close Attacks

	public void LeftCloseAttack()
	{
		_ball.GetComponent<Rigidbody>().AddForce(_leftCloseAttackForce);
	}

	public void RightCloseAttack()
	{
		_ball.GetComponent<Rigidbody>().AddForce(_rightCloseAttackForce);
	}

	public void ForwardCloseAttack()
	{
		_ball.GetComponent<Rigidbody>().AddForce(_forwardCloseAttackForce);
	}

	// Low Attacks

	public void ForwardLowAttack()
	{
		_ball.GetComponent<Rigidbody>().AddForce(_forwardLowAttackForce);
	}

	public void LeftLowAttack()
	{
		_ball.GetComponent<Rigidbody>().AddForce(_leftLowAttackForce);
	}

	public void RightLowAttack()
	{
		_ball.GetComponent<Rigidbody>().AddForce(_rightLowAttackForce);
	}
}
