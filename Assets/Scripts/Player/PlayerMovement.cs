using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.EventSystems.EventTrigger;

public class PlayerMovement : MonoBehaviour
{

    #region Fields
    [Header("General")]
    [HideInInspector] public Vector3 vel; //Velocity
    [HideInInspector] public Vector2 vel2D;
    public bool isGrounded; //grounded
    public Player player; //Player

    [Header("Ground")]
    public float maxWalkSpeed; //Max velocity while walking
    public float groundAcceleration; //acceleration speed while on ground
    private bool _landed; //if landed has happened

    [Header("Air")]
    public float airAcceleration; //acceleration speed while in air
    public float gravity; //gravity velocity
    public float maxGravity; //Maximum gravity velocity

    private bool _inAir; //if player is in air
    private float _airTimer; //time while in air

    [Header("Jump")]
    public float initialJumpForce; //jump velocity
    public float continuedJumpForce; //jump velocity when holding space for higher jump
    public float continuedJumpDuration; //time allowed to hold jump button for extra height
    private float _continuedJumpTimer;

    private bool _wishJump; //if the player wishes to jump
    private bool _wishedJumpPerformed;
    public float jumpGap; //amount of time the player can still jump after running off an edge
    private bool _inJump; //If the player is currently doing a jump

    [Header("Misc")]
    public Transform cameraPivot; // Camera pivot

    //Private
    Vector2 _movement; //movement from inputs
    public Vector3 moveVec; //movement in vector3
    public CharacterController _controller; //character controller
    Vector3 oldPos; //position before moving character
    float movedDistance = 0;
    public float stepDistance = 1f;
    private float _speedMultiplier = 1f;

    PlayerInput inputs;


    #endregion

    #region Propeties

    public bool IsGrounded()
    {
        return isGrounded;
    }

    #endregion

    #region Unity Methods (Start, Awake, Update)

    private void OnEnable()
    {

    }

    private void OnDisable()
    {
        inputs.Main.Jump.performed -= JumpInput;
        inputs.Main.Jump.canceled -= JumpInput;
    }


    // Start is called before the first frame update
    void Start()
    {
        //setup
        _controller = GetComponent<CharacterController>();
        player = GetComponent<Player>();
        inputs = GameManager.Instance.inputs;

        inputs.Main.Jump.performed += JumpInput;
        inputs.Main.Jump.canceled += JumpInput;
    }

    // Update is called once per frame
    void Update()
    {

        if (GameManager.Instance.gameActive)
        {
            HandleInputs();
        }

        //update grounded
        isGrounded = _controller.isGrounded;

        vel2D = new Vector2(vel.x, vel.z);

        Jump();

        //Ground and air movement
        if (isGrounded)
        {
            _inJump = false;
            Land();
        }
        else
        {
            CheckWalkOffEdge();
            AirTimer();
            ApplyGravity();
        }

        Move();


        oldPos = transform.position;

        MoveThePlayer();

        CancelVelocity();

    }

    #endregion

    void MoveThePlayer()
    {
        _controller.Move(Time.deltaTime * _speedMultiplier * vel);
    }

    void AirTimer()
    {
        _airTimer += Time.deltaTime;
        _landed = false;

        if (_wishJump && _wishedJumpPerformed)
        {
            _continuedJumpTimer += Time.deltaTime;
        }
    }


    void HandleInputs()
    {
        float forward = inputs.Main.Forward.ReadValue<float>();
        float backwards = inputs.Main.Backwards.ReadValue<float>();
        float right = inputs.Main.Right.ReadValue<float>();
        float left = inputs.Main.Left.ReadValue<float>();

        _movement = new Vector2(right - left, forward - backwards);

        moveVec = new Vector3(_movement.x, 0, _movement.y).normalized;
    }


    void JumpInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _wishJump = true;
            _wishedJumpPerformed = false;
        }
        if (context.canceled)
        {
            _wishJump = false;
        }
    }


    /// <summary>
    /// Ground move 
    /// </summary>
    void Move()
    {
        Vector3 fixedMovement = transform.TransformVector(moveVec).normalized;

        // Move vel vector towards target vel
        Vector3 velXZ = new Vector3(vel.x, 0, vel.z);
        Vector3 newVelXZ = Vector3.MoveTowards(velXZ, maxWalkSpeed * fixedMovement, groundAcceleration * Time.deltaTime);
        vel = new Vector3(newVelXZ.x, vel.y, newVelXZ.z);

        movedDistance += newVelXZ.magnitude * Time.deltaTime;
        if (movedDistance > stepDistance)
        {
            movedDistance -= stepDistance;
        }

    }

    /// <summary>
    /// Jump 
    /// </summary>
    void Jump()
    {
        if (!_wishJump) return;

        if (isGrounded || _airTimer < jumpGap)
        {
            if (vel.y <= initialJumpForce)
            {
                vel.y = initialJumpForce;
                _airTimer = 1;
            }

            _wishedJumpPerformed = true;

            AdjustValuesWhenJumpPerformed();

        }
        // else if (_wishedJumpPerformed && _continuedJumpTimer < continuedJumpDuration && vel.y <= initialJumpForce * 1.5f) //Only if player has less velocity than the highest you can reach from a normal jump
        // {
        //     vel.y = vel.y + (continuedJumpForce * 200 * Time.deltaTime);
        // }
        else
        {
            _wishJump = false;
        }
    }

    void AdjustValuesWhenJumpPerformed()
    {
        isGrounded = false;
        _inAir = true;
        _inJump = true;
        _continuedJumpTimer = 0;
    }

    /// <summary>
    /// Check if the player is walking off an edge and adjust Y velocity depending on that
    /// </summary>
    private void CheckWalkOffEdge()
    {
        //return if we are on the way up as this is only useful for downgoing velocity
        if (vel.y > 0 || _inAir)
        {
            return;
        }

        _inAir = true;

        //Raycast downwards to see if ground is under the player
        float dist = 0.25f;
        var layerMask = 1 << LayerMask.NameToLayer("Default");
        bool rayHit = Physics.Raycast(transform.position, Vector3.down, out RaycastHit _,
            dist + _controller.height * 0.6f, layerMask, QueryTriggerInteraction.Ignore);

        //If there is ground, keep some downgoing velocity, else make it 0.
        vel.y = rayHit ? -4 : -0;
    }

    private void Land()
    {
        if (_landed)
            return;

        _inAir = false;
        _airTimer = 0;
        _wishedJumpPerformed = false;

        if (vel.y < -4 && _airTimer > 0.5f)
        {
            //Landing sound
        }

        vel.y = -2;

        _landed = true;
    }

    void ApplyGravity()
    {
        if (-vel.y < maxGravity)
        {
            vel += gravity * Time.deltaTime * Vector3.down;
        }
    }

    void CancelVelocity()
    {

        if (Mathf.Abs(transform.position.x - oldPos.x) < Mathf.Abs(vel.x * Time.deltaTime * 0.2f * _speedMultiplier))
        {
            vel.x = 0;
        }

        if (Mathf.Abs(transform.position.y - oldPos.y) < Mathf.Abs(vel.y * Time.deltaTime * 0.2f * _speedMultiplier))
        {
            if (vel.y > 0)
            {
                vel.y = 0;
            }
        }

        if (Mathf.Abs(transform.position.z - oldPos.z) < Mathf.Abs(vel.z * Time.deltaTime * 0.2f * _speedMultiplier))
        {
            vel.z = 0;
        }
    }

    public void ApplyForce(Vector3 force, float scale = 1)
    {
        vel += (force * scale);
    }

    public void LimitHorizonalSpeed()
    {
        if (vel2D.magnitude > maxWalkSpeed)
        {
            Vector2 limitedVel = vel2D.normalized * maxWalkSpeed;
            vel.x = limitedVel.x;
            vel.z = limitedVel.y;
        }
    }

}
