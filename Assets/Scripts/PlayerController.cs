using UnityEngine;

public class PlayerController : Unit
{
    [Space, Header("Player Controller")]

    [SerializeField]
    private float _MoveSpeed = 8f;

    [SerializeField]
    private float _RunSpeedMult = 5f;

    [SerializeField]
    private float _JumpSpeed = 12f;

    [SerializeField]
    private Transform _CameraPivot;

    private float _XInput;
    private float _ZInput;
    private bool _JumpPressed;
    private Transform _Camera;

    protected override void UnitAwake()
    {
        _Camera = Camera.main.transform;
    }

    private void Update()
    {
        TimeDebug();
        ReadInputs();
        ApplyAnimValues();
        MouseRotations();
        CameraZoom();
    }

    private void TimeDebug()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) Time.timeScale = 1f;
        if (Input.GetKeyDown(KeyCode.Alpha2)) Time.timeScale = 2f;
        if (Input.GetKeyDown(KeyCode.Alpha4)) Time.timeScale = 4f;
        if (Input.GetKeyDown(KeyCode.Alpha8)) Time.timeScale = 8f;
    }

    private void ApplyAnimValues()
    {
        _Anim.SetFloat("Vertical", _ZInput);
        _Anim.SetFloat("Horizontal", _XInput);
    }

    private void CameraZoom()
    {
        var newZoom = _Camera.transform.localPosition;
        newZoom.z += Input.mouseScrollDelta.y;
        newZoom.z = Mathf.Clamp(newZoom.z, -32f, 0f);
        _Camera.localPosition = newZoom;
    }

    private void MouseRotations()
    {
        var mouseX = Input.GetAxis("Mouse X");
        var mouseY = Input.GetAxis("Mouse Y");

        transform.Rotate(0f, mouseX, 0f);
        _CameraPivot.Rotate(-mouseY, 0f, 0f);
    }

    private void FixedUpdate()
    {
        ApplyPhysics();
    }

    private void ReadInputs()
    {
        _XInput = Input.GetAxis("Horizontal");
        _ZInput = Input.GetAxis("Vertical");

        if (Input.GetKeyDown(KeyCode.Space))
        {
            _JumpPressed = true;
            _Anim.SetTrigger("Jump");
        }
    }

    private void ApplyPhysics()
    {
        var speedMult = Input.GetKey(KeyCode.LeftShift) ? _RunSpeedMult : 1f;
        _Anim.SetFloat("RunSpeed", speedMult);
        var newVel = new Vector3(_XInput, 0f, _ZInput) * _MoveSpeed * speedMult;
        newVel = transform.TransformVector(newVel);

        newVel.y = _JumpPressed ? _JumpSpeed : _RB.velocity.y;

        _RB.velocity = newVel;

        _JumpPressed = false;
    }

}
