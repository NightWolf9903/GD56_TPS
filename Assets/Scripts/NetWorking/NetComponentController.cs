using UnityEngine;
using Photon.Pun;

public class NetComponentController : MonoBehaviour
{
    [SerializeField]
    private GameObject[] _ObjectsToDelete;

    [SerializeField]
    private Component[] _ComponentsToRemove;

    private void Awake()
    {
        if (PhotonView.Get(this).IsMine == false)
        {
            foreach (var go in _ObjectsToDelete)
            {
                Destroy(go);
            }
            foreach (var comp in _ComponentsToRemove)
            {
                Destroy(comp);
            }
        }
    }
}
