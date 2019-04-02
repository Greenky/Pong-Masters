using UnityEngine;

[RequireComponent(typeof(PhotonView))]

public class BallLerp : Photon.MonoBehaviour, IPunObservable
{


    private Vector3 _latestCorrectPos;
    private Quaternion _latestCorrectRot;

    private Vector3 _onUpdatePos;
    private Quaternion _onUpdateRot;

    private bool _localPaddle = false;


    private float _fraction;

    public void Start()
    {
        _latestCorrectPos = GetComponent<Rigidbody>().position;
        _latestCorrectRot = GetComponent<Rigidbody>().rotation;
        _onUpdatePos = transform.position;
        _onUpdateRot = transform.rotation;
    }


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting && _localPaddle)
        {
            Vector3 pos = GetComponent<Rigidbody>().position;
            Quaternion rot = GetComponent<Rigidbody>().rotation;
            stream.Serialize(ref pos);
            stream.Serialize(ref rot);
            _localPaddle = false;
        }
        else
        {
          
            Vector3 pos = Vector3.zero;
            Quaternion rot = Quaternion.identity;

            stream.Serialize(ref pos);
            stream.Serialize(ref rot);

            _latestCorrectPos = pos;             
            _latestCorrectRot = rot;
            _onUpdatePos = GetComponent<Rigidbody>().position; 
            _onUpdateRot = GetComponent<Rigidbody>().rotation;
            _fraction = 0;

            _fraction = _fraction + Time.deltaTime * 9;
            transform.localPosition = Vector3.Lerp(_onUpdatePos, _latestCorrectPos, _fraction);
            transform.localRotation = Quaternion.Lerp(_onUpdateRot, _latestCorrectRot, _fraction);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //TODO check if my paddle then change state _localPaddle to true 
    }
   
}
