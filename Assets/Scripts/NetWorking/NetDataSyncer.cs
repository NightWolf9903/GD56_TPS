using UnityEngine;
using Photon.Pun;

public class NetDataSyncer : MonoBehaviourPun, IPunObservable
{
    [SerializeField]
    private float _SmoothFactor = 8f;
    private Vector3 _NetPosition;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else
        {
            _NetPosition = (Vector3) stream.ReceiveNext();
            transform.rotation = (Quaternion) stream.ReceiveNext();
        }
    }

    private void Update()
    {
        if(photonView.IsMine) return;
        transform.position = Vector3.Lerp(transform.position,_NetPosition, _SmoothFactor * Time.deltaTime);
    }

    [PunRPC]
    private void RPC_BroadcastChat(string msg, PhotonMessageInfo info)
    {
        print($"[{info.Sender.NickName}] : {msg}");
    }

    [SerializeField]
    private string _MesgToSend = "Hello";

    [ContextMenu("Send Chat")]
    private void SendChatMsg()
    {
        photonView.RPC("RPC_BroadcastChat",RpcTarget.OthersBuffered, _MesgToSend);
    }
}
