//
// A (very) simple network interpolation script, using Lerp().
//
// This will lag-behind, compared to the moving cube on the controlling client.
// Actually, we deliberately lag behing a bit more, to avoid stops, if updates arrive late.
//
//

using UnityEngine;

[RequireComponent(typeof(PhotonView))]
public class LerpPosition : Photon.MonoBehaviour, IPunObservable
{
    private Vector3 _latestCorrectPos;
    private Quaternion _latestCorrectRot;
    private Vector3 _onUpdatePos;
    private Quaternion _onUpdateRot;
    private float _fraction;


    public void Start()
    {
        _latestCorrectPos = transform.position;
        _latestCorrectRot = transform.rotation;
        _onUpdatePos = transform.position;
        _onUpdateRot = transform.rotation;
    }


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            Vector3 pos = transform.localPosition;
            Quaternion rot = transform.localRotation;
            stream.Serialize(ref pos);
            stream.Serialize(ref rot);
        }
        else
        {
            // Receive latest state information
            Vector3 pos = Vector3.zero;
            Quaternion rot = Quaternion.identity;

            stream.Serialize(ref pos);
            stream.Serialize(ref rot);

            _latestCorrectPos = pos;                // save this to move towards it in FixedUpdate()
            _latestCorrectRot = rot;
            _onUpdatePos = transform.localPosition; // we interpolate from here to latestCorrectPos
            _onUpdateRot = transform.localRotation;
            _fraction = 0;                          // reset the fraction we alreay moved. see Update()

          
        }
    }


    public void Update()
    {
        if (this.photonView.isMine)
        {
            return;     // if this object is under our control, we don't need to apply received position-updates 
        }

        // We get 10 updates per sec. Sometimes a few less or one or two more, depending on variation of lag.
        // Due to that we want to reach the correct position in a little over 100ms. We get a new update then.
        // This way, we can usually avoid a stop of our interpolated cube movement.
        //
        // Lerp() gets a fraction value between 0 and 1. This is how far we went from A to B.
        //
        // So in 100 ms, we want to move from our previous position to the latest known. 
        // Our fraction variable should reach 1 in 100ms, so we should multiply deltaTime by 10.
        // We want it to take a bit longer, so we multiply with 9 instead!

        _fraction = _fraction + Time.deltaTime * 9;
        transform.localPosition = Vector3.Lerp(_onUpdatePos, _latestCorrectPos,_fraction); // set our pos between A and B
        transform.localRotation = Quaternion.Lerp(_onUpdateRot, _latestCorrectRot, _fraction); // set our pos between A and B
    }
}