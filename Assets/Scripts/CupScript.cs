using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CupScript : MonoBehaviour
{
	private bool _isTouched = false;
	private Quaternion _startingRotation;
	private GameManager _gameManager;

	private void Start()
	{
		_startingRotation = transform.rotation;
		_gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
	}

	private void Update()
	{
		if (Mathf.Abs(_startingRotation.x - transform.rotation.x) > 0.3f
			   || Mathf.Abs(_startingRotation.z - transform.rotation.z) > 0.3f
			   || Mathf.Abs(_startingRotation.y - transform.rotation.y) > 0.3f)
		{
			if (!_isTouched)
				_gameManager.CupTouch();
			_isTouched = true;
		}
	}

	public bool IsTouched()
	{
		return _isTouched;
	}
}
