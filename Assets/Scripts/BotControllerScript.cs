using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BotControllerScript : MonoBehaviour {

	[SerializeField] private GameObject _ball;
	[Header("Close Attacks")]
	[Space]
	[SerializeField] private Vector3 _forwardCloseAttackForce = new Vector3(0, -15, -15);
	[SerializeField] private Vector3 _leftCloseAttackForce = new Vector3(5, -15, -15);
	[SerializeField] private Vector3 _rightCloseAttackForce = new Vector3(-5, -15, 15);

	[Header("Far Attacks")]
	[Space]
	[SerializeField] private Vector3 _forwardFarAttackForce = new Vector3(0, -20, -20);
	[SerializeField] private Vector3 _leftFarAttackForce = new Vector3(5, -20, -20);
	[SerializeField] private Vector3 _rightFarAttackForce = new Vector3(-5, -20, 20);


	void Start () {
		
	}
	
	void Update () {
		

		if(_isBallOnMySide())
		{
			if (_checkBallX() && _checkBallY())
			{
				//GetComponent<Rigidbody>().MovePosition(new Vector3(_ball.transform.position.x,
				//										_ball.transform.position.y,
				//										transform.position.z));
				//transform.Translate(_ball.transform.position.x,
				//					_ball.transform.position.y,
				//					_ball.transform.position.z);
				transform.position = new Vector3(	_ball.transform.position.x,
													_ball.transform.position.y,
													transform.position.z);

//				if (_isBallClose())
//				{
//					Debug.Log("Hello");
//					GetComponent<Rigidbody>().MovePosition(transform.position + _ball.GetComponent<Rigidbody>().velocity);
//					//					transform.Translate(_ball.GetComponent<Rigidbody>().velocity);
//					GetComponent<Rigidbody>().MovePosition(transform.position - _ball.GetComponent<Rigidbody>().velocity);
////					GetComponent<Rigidbody>().AddForce(_ball.GetComponent<Rigidbody>().velocity * -10);
//					//transform.rotation = Quaternion.Euler(_ball.GetComponent<Rigidbody>().velocity * -10);
//				}
			}
		}
		
	}

	private bool _isBallClose()
	{
		return ((_ball.transform.position - transform.position).magnitude < 0.2);
	}

	private bool _isBallOnMySide()
	{
		return (_ball.transform.position.z < 2.6);
	}

	private bool _checkBallX()
	{
		return (_ball.transform.position.x > -1.5 && _ball.transform.position.x < 1.5);
	}

	private bool _checkBallY()
	{
		return (_ball.transform.position.y < 1.5 && _ball.transform.position.y > 0.5);
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.transform.name == "InitBall")
		{
			ResetObject(collision.gameObject);
			collision.gameObject.GetComponent<Rigidbody>().AddForce(_forwardFarAttackForce);
		}
	}

	private void ResetObject(GameObject obj)
	{
		//_ball.transform.position = _initCoord;
		obj.GetComponent<Rigidbody>().velocity = Vector3.zero;
		obj.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
		obj.transform.rotation = Quaternion.Euler(Vector3.zero);
	}

	// Far Attacks

	public void LeftFarAttack()
	{
		_ball.GetComponent<Rigidbody>().isKinematic = false;
		_ball.GetComponent<Rigidbody>().AddForce(_leftFarAttackForce);
	}

	public void RightFarAttack()
	{
		_ball.GetComponent<Rigidbody>().isKinematic = false;
		_ball.GetComponent<Rigidbody>().AddForce(_rightFarAttackForce);
	}

	public void ForwardFarAttack()
	{
		_ball.GetComponent<Rigidbody>().isKinematic = false;
		_ball.GetComponent<Rigidbody>().AddForce(_forwardFarAttackForce);
	}

	// Close Attacks

	public void LeftCloseAttack()
	{
		_ball.GetComponent<Rigidbody>().isKinematic = false;
		_ball.GetComponent<Rigidbody>().AddForce(_leftCloseAttackForce);
	}

	public void RightCloseAttack()
	{
		_ball.GetComponent<Rigidbody>().isKinematic = false;
		_ball.GetComponent<Rigidbody>().AddForce(_rightCloseAttackForce);
	}

	public void ForwardCloseAttack()
	{
		_ball.GetComponent<Rigidbody>().isKinematic = false;
		_ball.GetComponent<Rigidbody>().AddForce(_forwardCloseAttackForce);
	}
}
