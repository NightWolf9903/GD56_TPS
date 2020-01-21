using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class PhotonController : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private string _UserName = "Christian Clavijo";

    [SerializeField]
    private int _SerializationRate = 10;

    private void Awake()
    {
        PhotonNetwork.SendRate = 2 *_SerializationRate;
        PhotonNetwork.SerializationRate = _SerializationRate;
        PhotonNetwork.NickName = _UserName;
        PhotonNetwork.ConnectUsingSettings();
    }

    override public void OnConnectedToMaster()
    {
        //print("OnConnectedToMaster");
        var roomOptions = new RoomOptions()
        {
            MaxPlayers = 20,
            IsVisible = true,
            IsOpen = true
        };

        PhotonNetwork.JoinOrCreateRoom("GD56",roomOptions,TypedLobby.Default);
    }

    override public void OnJoinedRoom()
    {
        print($"OnJoinRoom {PhotonNetwork.CurrentRoom.Name}");
        var clone = PhotonNetwork.Instantiate("Net_Nightshade",transform.position,transform.rotation);
        clone.name = $"Net_{_UserName}";
    }

    override public void OnPlayerEnteredRoom(Player newPlayer)
    {
        print($"{newPlayer.NickName} Joined the Room");
    }
    
    override public void OnMasterClientSwitched(Player newMasterClient)
    {
        print($"{newMasterClient.NickName} is the new MASTER ");
    }
}
