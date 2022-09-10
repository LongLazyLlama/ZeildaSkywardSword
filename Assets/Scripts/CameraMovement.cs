using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMovement : MonoBehaviour
{
    public GameObject Target;

    public Transform HorizontalRotation;
    public Transform VerticalRotation;

    public float CameraMoveSpeed = 0.05f;
    public Vector2 TurnSpeed = new Vector2(0.1f, 0.1f);

    Vector2 _mouseMovement;

    void Update()
    {
        if (Target == null)
        {
            Debug.LogWarning("No target found: " + Target);
        }
        else
        {
            HorizontalRotation.transform.position = Target.transform.position;
            Camera.main.transform.LookAt(Target.transform);
        }

        //var _currentposition = HorizontalRotation.transform.position;
        //var _destination = Target.transform.position;
        //HorizontalRotation.transform.position = Vector3.Lerp(_currentposition, _destination, CameraMoveSpeed);

    }

    public void OnMouseMovement(InputAction.CallbackContext context)
    {
        _mouseMovement = context.ReadValue<Vector2>();
        //Debug.Log(_mouseMovement);

        HorizontalRotation.transform.Rotate(Vector3.up, _mouseMovement.x * TurnSpeed.x);
        VerticalRotation.transform.Rotate(Vector3.left, _mouseMovement.y * TurnSpeed.y);
    }

}
