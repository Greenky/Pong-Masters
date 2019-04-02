using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]

public class TrackerFollow : MonoBehaviour {

    [SerializeField] private GameObject _tracker;
	[SerializeField] private GameObject _rightController;

    private GameObject _foollowObj;

    private float _customWidth;

    private Vector3 _size;

    private void Awake()
    {
        _size = GetComponent<BoxCollider>().size;
    }

    private void FixedUpdate () {

        _foollowObj = _tracker.activeSelf ? _tracker : _rightController;
 
        GetComponent<Rigidbody>().MovePosition(
			_foollowObj.GetComponent<Transform>().position
        );
	
		GetComponent<Rigidbody>().MoveRotation(
            _foollowObj.GetComponent<Transform>().rotation * Quaternion.Euler(_tracker.activeSelf ? 0f : 90f, 0, 0)
        );

	
		_customWidth = Mathf.Pow(2f, GetComponent<Rigidbody>().velocity.magnitude * 0.02f);


        if (_customWidth > 20)
            _customWidth = 20;

		GetComponent<BoxCollider>().size = new Vector3(_size.x, _size.y, 0.0058f * _customWidth);
	}
}
