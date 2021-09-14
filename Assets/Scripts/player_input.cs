using UnityEngine;
using UnityEngine.InputSystem;


///<summary> 
/// Behaviour of the player
///</summary>
[RequireComponent(typeof(CharacterController))]
public class player_input : MonoBehaviour
{
    #region public variables
    [Header ("References")]
    [SerializeField]
    private InputActionReference movementControl;
    [SerializeField]
    private InputActionReference jumpControl;
    [SerializeField]
    private Animator anim;

    [Header ("Customizable Features")]
    [SerializeField]
    private float playerSpeed = 2.0f;
    [SerializeField]
    private float jumpHeight = 1.0f;
    [SerializeField]
    private float gravityValue = -9.81f;    
    [SerializeField]
    private float rotationSpeed = 4f;
    #endregion

    #region private variables
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private Transform cameraTransform;
    #endregion

    private void OnEnable()
    {
        movementControl.action.Enable();
        jumpControl.action.Enable();
    }

    private void OnDisable()
    {
        movementControl.action.Disable();
        jumpControl.action.Disable();
    }

    private void Start()
    {
        controller = gameObject.GetComponent<CharacterController>();
        cameraTransform = Camera.main.transform;
    }

    void Update()
    {
        // running
        anim.SetBool("Run", false);
        groundedPlayer = controller.isGrounded;

        // jumping
        if (groundedPlayer && playerVelocity.y < 0)
        {
            anim.SetBool("isJumping", false);
            playerVelocity.y = 0f;
        }

        // movement direction
        Vector2 movement = movementControl.action.ReadValue<Vector2>();
        Vector3 move = new Vector3(movement.x, 0, movement.y);
        move = cameraTransform.forward * move.z + cameraTransform.right * move.x;
        move.y = 0f;
        
        controller.Move(move * Time.deltaTime * playerSpeed);

        // Changes the height position of the player
        if (jumpControl.action.triggered && groundedPlayer)
        {
            anim.SetBool("isJumping", true);
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);

        if(move != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(move);
            anim.SetBool("Run", true);
        }
            
    }
}
