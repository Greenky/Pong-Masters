using UnityEngine;
using UnityEditor.Animations;

public class HierarchyRecorder : MonoBehaviour
{
	public AnimationClip clip;
	public bool record = false;

	private GameObjectRecorder m_Recorder;
	[SerializeField]
	private GameObject _rightController;

	private void Start()
	{
		_rightController.GetComponent<SteamVR_TrackedController>().TriggerClicked += OnTriggerClicked;
		_rightController.GetComponent<SteamVR_TrackedController>().TriggerUnclicked += OnTriggerUnclicked;
		m_Recorder = new GameObjectRecorder(gameObject);
		m_Recorder.BindComponentsOfType<Transform>(gameObject, true);
	}

	private void OnTriggerClicked(object sender, ClickedEventArgs e)
	{
		record = true;
	}

	private void OnTriggerUnclicked(object sender, ClickedEventArgs e)
	{
		record = false;
	}

	private void LateUpdate()
	{
		if (clip == null)
			return;

		if (record)
			m_Recorder.TakeSnapshot(Time.deltaTime);
		else if (m_Recorder.isRecording)
		{
			m_Recorder.SaveToClip(clip);
			m_Recorder.ResetRecording();
		}
	}
}
