using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    //The public instance of the Player in scene.
    public static PlayerController Player;

    //The controller attached to the Player.
    public CharacterController CC { get { return GetComponent<CharacterController>();} }
    //The input system (new unity input system) component attached to the Player.
    public PlayerInput Controls { get { return GetComponent<PlayerInput>(); } }

    [HideInInspector]
    public GameObject Interactible;

    //The direction in which the Player moves forward.
    public Transform AbsoluteForward;


    //Default PlayerSettings.
    [Space]
    public float Mass = 60.0f;              //Mass in Kg.
    public float Acceleration = 3.0f;       //Velocity increase in m/s.

    public float WalkSpeed = 10.0f;
    public float TurnSpeed = 10.0f;         //How fast the player's model turns when changing move direction.
    public float JumpHeight = 8.5f;         //How high the player can jump in units.
    public float CoyoteTime = 0.1f;         //Extra jumptime after the Player lost ground.

    [Space]
    public bool Gravity = true;             //Enabled/Disable Player gravity.
    public bool CoyoteJump = true;          //Enabled/Disable Coyote time after losing ground.
    public bool InfiniteJump = false;       //Enabled/Disable Infinite jumping.


    //Extra visualised parameters for debugging.
    [Space]
    [SerializeField]
    Vector2 _normalisedMovement;            //The normalised movement input [-1, 0, 1].
    [SerializeField]
    Vector3 _internalVelocity;              //The current velocity applied to the Player.
    [SerializeField]
    Vector3 _externalVelocity = Vector3.zero;
    [SerializeField]
    Vector3 _detectedNormals;               //Shows which normals the Player touches.
    [SerializeField]
    float _CoyoteTimer;                     //The countdown of the coyotetime after losing ground.


    Vector3 _movement;

    Quaternion _targetRotation;

    float _angle;

    bool _isInteracting;
    bool _jump;

    private void Start()
    {
        Player = this;
        Cursor.visible = false;
    }

    private void FixedUpdate()
    {
        CalculateCoyoteTime();
        CalculateForwardDirection();
        ApplyVerticalVelocity();

        CalculateRotationAngle();
        ApplyStepOffset();

        ApplyJump();
        ApplyMovement();
    }

    private void ApplyMovement()
    {
        CC.Move((_internalVelocity + _externalVelocity) * Time.deltaTime);
    }

    private void CalculateCoyoteTime()
    {
        _CoyoteTimer -= Time.deltaTime;
    }

    private void ApplyStepOffset()
    {
        if (CC.isGrounded)
        {
            _internalVelocity.y -= CC.stepOffset * 10;

            //Reset coyote time.
            _CoyoteTimer = CoyoteTime;
        }
    }

    private void ApplyJump()
    {
        if (_jump)
        { 
            _internalVelocity += -Physics.gravity.normalized * Mathf.Sqrt(2 * Physics.gravity.magnitude * JumpHeight);

            //Instantly sets the coyotetimer below 0 to avoid doublejumping while coyotetimer counts down.
            _CoyoteTimer -= CoyoteTime;
            _jump = false;
        }
    }

    private void ApplyVerticalVelocity()
    {
        if (Gravity)
        {
            if (!CC.isGrounded)
            {
                _internalVelocity.y += (Physics.gravity.y * Time.deltaTime) * 2;
                //Debug.Log(_velocity);
            }
            else
            {
                _internalVelocity -= Vector3.Project(_internalVelocity, Physics.gravity.normalized);
            }
        }
    }

    private void CalculateForwardDirection()
    {
        //Takes the x and y component from the forward.
        Vector3 xzAbsoluteForward = Vector3.Scale(AbsoluteForward.forward, new Vector3(1, 0, 1));

        //Returns the rotationvalue from the forward direction.
        Quaternion forwardRotation = Quaternion.LookRotation(xzAbsoluteForward);

        //Rotates the input vector.
        Vector3 relativeMovement = forwardRotation * _movement;

        //Adds the movement to the new direction vector.
        _internalVelocity = new Vector3(relativeMovement.x * WalkSpeed, _internalVelocity.y, relativeMovement.z * WalkSpeed);
    }

    private void CalculateRotationAngle()
    {
        if (_internalVelocity.x != 0 || _internalVelocity.z != 0)
        {
            //Calulates the rotation to the direction the player currently walks in.
            _angle = Mathf.Atan2(_internalVelocity.x, _internalVelocity.z);
            _angle = Mathf.Rad2Deg * _angle;

            Rotate();
        }
    }

    private void Rotate()
    {
        //Adds the rotation.
        _targetRotation = Quaternion.Euler(0, _angle, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, _targetRotation, TurnSpeed * Time.deltaTime);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == 8 && !_isInteracting)
        {
            Interactible = other.gameObject;
            Interactible.GetComponent<Interactible>().ShowInteractionMessage(true);

            Debug.Log("Interactible '" + other.gameObject.name + "' in range !");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == Interactible)
        {
            Interactible.GetComponent<Interactible>().ShowInteractionMessage(false);
            Interactible = null;
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        //Reads movement input whenever the value changes.
        _normalisedMovement = context.ReadValue<Vector2>();
        _movement = new Vector3(_normalisedMovement.x, 0, _normalisedMovement.y);
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.started && _isInteracting)
        {
            Interactible.GetComponent<Interactible>().ExitInteraction();
            Interactible.GetComponent<Interactible>().ShowInteractionMessage(false);

            Interactible = null;
            _isInteracting = false;

            Debug.Log("No longer interacting with '" + Interactible + "'.");
        }
        else if(context.started && !_isInteracting && Interactible != null)
        {
            Interactible.GetComponent<Interactible>().Interact();
            _isInteracting = true;

            Debug.Log("Now interacting with '" + Interactible + "'.");
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        //Debug.Log("Space was pressed!");
        if (InfiniteJump)
        {
            if (context.started)
            {
                Debug.Log("Infinite jump has started!");
                _jump = true;
            }
        }
        else if (CoyoteJump)
        {
            if (context.started && !_jump && _CoyoteTimer >= 0)
            {
                Debug.Log("Coyotejump has started!");
                _jump = true;
            }
        } 
        else
        {
            if (context.started && !_jump && CC.isGrounded)
            {
                Debug.Log("Jump has started!");
                _jump = true;
            }
        }
    }

    public void OnControllerColliderHit(ControllerColliderHit hit)
    {
        _detectedNormals = hit.normal;
        Rigidbody _detectedRigidBody = hit.gameObject.GetComponent<Rigidbody>();

        //If a normal pointing downwards is detected (above the player) the player velocity resets.
        if (_detectedNormals.y == -1)
        {
            Debug.Log("Object above player detected");
            _internalVelocity.y = -0.5f;
        }

        //If an object with rigidbody is detected, take this objects velocity.
        //
        //Example code for velocity on RigidBodies.
        //
        //RB.velocity = this.transform.forward * Speed;
        //RB.position += RB.velocity * Time.deltaTime;
        //
        if (_detectedRigidBody != null)
        {
            //Note: Negative Y velocity will be replaced by gravity.
            _externalVelocity = new Vector3(_detectedRigidBody.velocity.x, Mathf.Max(_detectedRigidBody.velocity.y, 0), 
                _detectedRigidBody.velocity.z);

            Debug.Log("Player is now standing on top of " + hit.gameObject.GetComponent<Rigidbody>().gameObject);
        }
        else { _externalVelocity = Vector3.zero; }
    }
}
