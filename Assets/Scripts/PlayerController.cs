using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float _MoveSpeed = 8f;

    [SerializeField]
    private float _RunSpeedMult = 5f;

    [SerializeField]
    private float _JumpSpeed = 12f;

    [SerializeField]
    private Transform _CameraPivot;

    private Rigidbody _RB;
    private Animator _Anim;
    private Vector3 _DesirableCamera;
    private float _XInput;
    private float _ZInput;
    private bool _JumpPressed;
    private Transform _Camera;
    private float _CurrentZoom;
    private bool _WallHitted = false;

    private void Awake()
    {
        _RB = GetComponent<Rigidbody>();
        _Anim = GetComponent<Animator>();
        _Camera = Camera.main.transform;
        _DesirableCamera =_Camera.localPosition;
    }

    private void Update()
    {
        ReadInputs();
        ApplyAnimValues();
        MouseRotations();
        CameraZoom();
        CameraTransition();
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
        if(newZoom.z != _Camera.transform.localPosition.z)
        {
            _DesirableCamera = newZoom;
        }
    }


    private void MouseRotations()
    {
        var mouseX = Input.GetAxis("Mouse X");
        var mouseY = Input.GetAxis("Mouse Y");

        transform.Rotate(0f, mouseX, 0f);

        // Check if the rotation don't exceeds the limits in euler angles
        Vector3 newRotation = _CameraPivot.localEulerAngles;
        newRotation.x += -mouseY;

        if((newRotation.x < 180) && (newRotation.x > 0))
        {
            newRotation.x = Mathf.Clamp(newRotation.x,0f,85f);
        }
        else if((newRotation.x < 360) && (newRotation.x > 270))
        {
            newRotation.x = Mathf.Clamp(newRotation.x,275f,360f);
        }

        //Setting the camera rotation using Euler Angles instead of using Rotate
        _CameraPivot.rotation = Quaternion.Euler(newRotation.x,transform.rotation.eulerAngles.y,0f);

    }

    private void FixedUpdate()
    {
        ApplyPhysics();
        DynamicCamera();
    }

    //Use the Raycast to see if a wall is behind the character
    private void DynamicCamera()
    {
        RaycastHit wallHit;
        var newCameraZ = _Camera.transform.localPosition;
        if (Physics.Raycast(_CameraPivot.position,_Camera.transform.forward*-1,out wallHit,5f))
        {
            if(!_WallHitted)
            {
                _CurrentZoom = _Camera.transform.localPosition.z;
            }
            newCameraZ.z = -Mathf.Abs(wallHit.distance+0.2f);
            Debug.DrawLine(_CameraPivot.position,wallHit.point,Color.red);
            _WallHitted = true;
            _DesirableCamera = newCameraZ;
        }
        else
        {
            if(_WallHitted)
            {
                newCameraZ.z = _CurrentZoom;
                _WallHitted = false;
                _DesirableCamera = newCameraZ;
            }
        }
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

        //Change the boolean variable in the animator to trigger the run animation or unable it
        if (Input.GetKey(KeyCode.LeftShift))
        {
            _Anim.SetBool("Running",true);
        }
        else
        {
            _Anim.SetBool("Running",false);
        }
    }
        


    private void ApplyPhysics()
    {
        var speedMult = Input.GetKey(KeyCode.LeftShift) ? _RunSpeedMult : 1f;
        var newVel = Vector3.Normalize(new Vector3(_XInput, 0f, _ZInput)) * _MoveSpeed * speedMult;
        newVel = transform.TransformVector(newVel);

        newVel.y = _JumpPressed ? _JumpSpeed : _RB.velocity.y;

        _RB.velocity = newVel;

        Vector3 locVel = transform.InverseTransformDirection(_RB.velocity);

        locVel.x = Mathf.Clamp(locVel.x,-Mathf.Abs(newVel.x*0.66f),Mathf.Abs(newVel.x*0.66f));
        if(locVel.z<0)
        {
            locVel.z = Mathf.Clamp(locVel.z,-Mathf.Abs(newVel.z*0.33f),0f);
        }

        _RB.velocity = transform.TransformDirection(locVel);

        _JumpPressed = false;
    }

    //Smooth transition in the zoom-in, zoom-out and wall colision 
    private void CameraTransition()
    {
        _Camera.transform.localPosition = Vector3.Lerp(_Camera.transform.localPosition,_DesirableCamera,0.05f);
    }
}
