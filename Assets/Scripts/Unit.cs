using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
public abstract class Unit : MonoBehaviour
{
    [Header("Unit")]
    public int TeamNumber = 0;

    [SerializeField]
    private Renderer _Renderer;

    protected Rigidbody _RB;
    protected Animator _Anim;

    protected abstract void UnitAwake();

    protected void Awake()
    {
        _RB = GetComponent<Rigidbody>();
        _Anim = GetComponent<Animator>();

        UnitAwake();
    }

    protected void Start()
    {
        SetTeam(TeamNumber);
    }

    private void SetTeam(int teamNumber)
    {
        TeamNumber = teamNumber;
        var teamColor = GameManager.Instance.TeamColors[TeamNumber];

        if (_Renderer == null) return;

        _Renderer.material.color = teamColor;
    }


}
