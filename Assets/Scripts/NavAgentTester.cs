using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class NavAgentTester : MonoBehaviour
{
    private NavMeshAgent _Agent;
    private Camera _Cam;

    private void Awake()
    {
        _Agent = GetComponent<NavMeshAgent>();
        _Cam = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButton(1))
        {
            var ray = _Cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hit))
            {
                _Agent.SetDestination(hit.point);
                Debug.DrawRay(hit.point, Vector3.up, Color.red, 1f);
            }
        }
    }

}
