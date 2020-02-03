using Photon.Pun;
using UnityEngine;

public class NetDataSyncer : MonoBehaviourPun, IPunObservable
{
    [SerializeField]
    private float _SmoothFactor = 8f;

    private Vector3 _NetPosition;

    private void Awake()
    {
        _NetPosition = transform.position;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting) //I'm Sending data, im the owner
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else //I'm receiving data, im NOT the owner
        {
            _NetPosition = (Vector3)stream.ReceiveNext();
            transform.rotation = (Quaternion)stream.ReceiveNext();
        }
    }

    private void Update()
    {
        if (photonView.IsMine) return;
        transform.position = Vector3.Lerp(transform.position, _NetPosition, _SmoothFactor * Time.deltaTime);
    }


    [PunRPC]
    private void RPC_BroadcastChat(string msg, PhotonMessageInfo info)
    {
        print($"[{info.Sender.NickName}] : {msg}");
    }

    [SerializeField]
    private string _MsgToSend = "Hello World!";

    [ContextMenu("Send Chat")]
    private void SendChatMsg()
    {
        photonView.RPC("RPC_BroadcastChat", RpcTarget.OthersBuffered, _MsgToSend);
    }

}
