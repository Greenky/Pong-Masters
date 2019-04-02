using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RobotBall : MonoBehaviour {

    private Vector3 _initCoord;
    private bool _needToThrow;
	private GameObject _newBall;
	
	[SerializeField] private Vector3	 _easyForce = new Vector3(0, -50, 70);
	[SerializeField] private Vector3	_midForce = new Vector3(0, -40, 40);
	[SerializeField] private Vector3	_superForce = new Vector3(0, -45, 45);
	[SerializeField] private GameObject	 _ball;
	[SerializeField] private GameManager _gameManager;
	
	[SerializeField] private float _deley = 2.0f;

	[Header("Simple Attacks")]
	[Space]
	[SerializeField] private Vector3 _forwardAttackForce		= new Vector3(0, -20, 20);
	[SerializeField] private Vector3 _leftAttackForce			= new Vector3(5, -20, 20);
	[SerializeField] private Vector3 _rightAttackForce			= new Vector3(-5, -20, 20);
	
	[Header("Close Attacks")]
	[Space]
	[SerializeField] private Vector3 _forwardCloseAttackForce	= new Vector3(0, -15, 15);
	[SerializeField] private Vector3 _leftCloseAttackForce		= new Vector3(5, -15, 15);
	[SerializeField] private Vector3 _rightCloseAttackForce		= new Vector3(-5, -15, 15);

	[Header("Far Attacks")]
	[Space]
	[SerializeField] private Vector3 _forwardFarAttackForce		= new Vector3(0, -20, 20);
	[SerializeField] private Vector3 _leftFarAttackForce		= new Vector3(5, -20, 20);
	[SerializeField] private Vector3 _rightFarAttackForce		= new Vector3(-5, -20, 20);

	[Space]

	[Header("Left Spin")]
	[Space]
	[SerializeField] private Vector3 _leftSpinAttackForce		= new Vector3(5, -20, -20);
	[SerializeField] private Vector3 _leftSpinAngularVelocity	= new Vector3(0, 20, 0);

	[Header("Right Spin")]
	[Space]
	[SerializeField] private Vector3 _rightSpinAttackForce		= new Vector3(-5, -20, 20);
	[SerializeField] private Vector3 _rightSpinAngularVelocity	= new Vector3(0, -20, 0);

	public int triesCount = 0;

	void Start () {
        _initCoord = _ball.transform.position;
		_newBall = _ball;
		_gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
		_gameManager.EndGame();
		_ball.GetComponent<Rigidbody>().isKinematic = true;
		_needToThrow = true;
		if (SceneManager.GetActiveScene().name == "GameWithCups")
			StartCoroutine(ReloadBall());
	}

	private void Update()
	{
		if (!_gameManager.InGame() && !_gameManager.IsArcadeCupsMode() && _needToThrow)
		{
			StartCoroutine(ReloadBall());
			_needToThrow = false;
		}
	}

	private void RandomEasyForce()
	{
		_easyForce.x = Random.Range(-15f, 15f);
		_easyForce.y = -50f;
		_easyForce.z = Random.Range(-35f, -65f);
	}

    private IEnumerator ReloadBall()
    {
		if (SceneManager.GetActiveScene().name == "GameWithCups")
			yield return new WaitForSeconds(3);
		else
			yield return new WaitForSeconds(1);
		_ball.GetComponent<Rigidbody>().isKinematic = false;
		triesCount++;
		ResetBall();
		if (!_gameManager.IsArcadeCupsMode())
			RandomEasyForce();
		_newBall.GetComponent<Rigidbody>().AddForce(_easyForce);
		if (_gameManager.IsArcadeCupsMode())
			StartCoroutine(ReloadBall());
	}

	private void ResetBall()
	{
		_newBall.transform.position = _initCoord;
		_newBall.GetComponent<Rigidbody>().velocity = Vector3.zero;
		_newBall.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
		_gameManager.StartGame();
		_needToThrow = true;
	}
	
	public void ForwardAttack()
	{
		ResetBall();
		_newBall.GetComponent<Rigidbody>().AddForce(_forwardAttackForce);
	}

	public void LeftAttack()
	{
		ResetBall();
		_newBall.GetComponent<Rigidbody>().AddForce(_leftAttackForce);
	}

	public void RightAttack()
	{
		ResetBall();
		_newBall.GetComponent<Rigidbody>().AddForce(_rightAttackForce);
	}

	// Far Attacks

	public void LeftFarAttack()
	{
		ResetBall();
		_newBall.GetComponent<Rigidbody>().AddForce(_leftFarAttackForce);
	}

	public void RightFarAttack()
	{
		ResetBall();
		_newBall.GetComponent<Rigidbody>().AddForce(_rightFarAttackForce);
	}

	public void ForwardFarAttack()
	{
		ResetBall();
		_newBall.GetComponent<Rigidbody>().AddForce(_forwardFarAttackForce);
	}

	// Close Attacks

	public void LeftCloseAttack()
	{
		ResetBall();
		_newBall.GetComponent<Rigidbody>().AddForce(_leftCloseAttackForce);
	}

	public void RightCloseAttack()
	{
		ResetBall();
		_newBall.GetComponent<Rigidbody>().AddForce(_rightCloseAttackForce);
	}

	public void ForwardCloseAttack()
	{
		ResetBall();
		_newBall.GetComponent<Rigidbody>().AddForce(_forwardCloseAttackForce);
	}

	// Spin Attacks

	public void LeftSpinAttack()
	{
		ResetBall();
		_newBall.GetComponent<Rigidbody>().AddForce(_leftSpinAttackForce);
	}

	public void RightSpinAttack()
	{
		ResetBall();
		_newBall.GetComponent<Rigidbody>().AddForce(_rightSpinAttackForce);
		_newBall.GetComponent<Rigidbody>().angularVelocity = _rightSpinAngularVelocity;
	}
}
