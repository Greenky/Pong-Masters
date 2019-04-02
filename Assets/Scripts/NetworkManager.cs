using System;
using Random = UnityEngine.Random;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class NetworkManager : Photon.MonoBehaviour
{
    public string Name { get; private set; }
    public Transform playerPrefab;
    [SerializeField] private GameObject _roomListObject;
    [SerializeField] private GameObject _roomButton;
    [SerializeField] private Transform _spawnPoint_1;
    [SerializeField] private Transform _spawnPoint_2;
    [SerializeField] private string _sceneLobby = "Lobby";
    [SerializeField] private string _sceneName = "OnlineScene";


    private void Start()
    {
	

		PhotonNetwork.automaticallySyncScene = true;
        if (PhotonNetwork.connectionStateDetailed == ClientState.PeerCreated)
        {
            PhotonNetwork.ConnectUsingSettings("0.9");
        }

   
        if (String.IsNullOrEmpty(PhotonNetwork.playerName))
        {
            Name = "Guest" + Random.Range(1, 9999);
            PhotonNetwork.playerName = Name;
        }

	}

    private void OnConnectedToMaster()
    {
     
        PhotonNetwork.JoinLobby(TypedLobby.Default);
    }

    private void OnJoinedLobby()
    {
        
    }
    private void OnMasterClientSwitched(PhotonPlayer newMasterClient)
    {
        SceneManager.LoadScene(_sceneLobby);
    }

    public void OnClick_CreateRoom()
    {

        if(!PhotonNetwork.CreateRoom("Room_"+ PhotonNetwork.playerName, new RoomOptions() { MaxPlayers = 2 }, null))
        {
            Debug.LogError("create room failed to send");
        }
    }

    private void OnPhotonCreateRoomFailed(object[] codeAndMessage)
    {
        Debug.LogError("create room failed: " + codeAndMessage[1]);
    }

    private void OnCreatedRoom()
    {
        PhotonNetwork.LoadLevel(_sceneName);
    }

    private void OnLevelWasLoaded(int level)
    {
        if (SceneManager.GetActiveScene().name == "OnlineScene")
        {
    
            Vector3 spawnPosition = _spawnPoint_1.position;
            Quaternion spawnRotation = _spawnPoint_1.rotation;
            if(PhotonNetwork.playerList.Length == 2)
            {
                spawnPosition = _spawnPoint_2.position;
                spawnRotation = _spawnPoint_2.rotation;

            }
            GameObject player = PhotonNetwork.Instantiate(playerPrefab.name, spawnPosition, spawnRotation, 0);
           
        }
    }

    public void UpdateClientList()
    {
        RoomInfo[] rooms = PhotonNetwork.GetRoomList();
        RemoveOldRooms(_roomListObject);
        foreach (RoomInfo room in rooms)
        {
            RoomReceived(room);
        }
    }

    private void RoomReceived(RoomInfo room)
    {
        GameObject btn = Instantiate(_roomButton,_roomListObject.transform);
		ColorBlock newColor = btn.GetComponent<Button>().colors;

		if (room.PlayerCount == 2)
			newColor.normalColor = Color.red;
		btn.GetComponent<Button>().colors = newColor;

		btn.transform.Find("Rooms.Text").GetComponent<Text>().text = room.Name.ToString() + "  players " + room.PlayerCount.ToString() + "/2"; // TODO if will be use TEXTMESH PRO change component!
        btn.transform.name = room.Name.ToString();
		
        btn.GetComponent<Button>().onClick.AddListener(delegate { TaskOnClick(room.Name.ToString()); });
    }

    private void RemoveOldRooms(GameObject PanelMenu)
    {
        foreach (Transform child in PanelMenu.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }

    private void TaskOnClick(string roomName)
    {
        PhotonNetwork.JoinRoom(roomName);
    }



    public void OnPhotonPlayerConnected(PhotonPlayer player)
    {
        Debug.Log("OnPhotonPlayerConnected: " + player);
    }

	[PunRPC]
	void GetBallPhysics(Vector3 position, Quaternion rotation, Vector3 velocity, Vector3 angularVelocity)
	{
		GameObject.FindGameObjectWithTag("Ball").GetComponent<BallPhysics>().ApplyPhysics(position, rotation, velocity, angularVelocity);
	}

	public void SendBallPhysics(Vector3 position, Quaternion rotation, Vector3 velocity, Vector3 angularVelocity)
	{
		GetComponent<PhotonView>().RPC("GetBallPhysics", PhotonTargets.Others, position, rotation, velocity, angularVelocity);
	}


}
