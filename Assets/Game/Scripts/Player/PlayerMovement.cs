using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private Rigidbody _rigidBody;
    [SerializeField] 
    private Transform _playerHead;
    [SerializeField]
    private float _movementSpeed;
    private InputAction _actionReference;
    public InputActionAsset InputActions;
    private Vector2 _moveInput;
    private bool _movementAllowed;

    public void AllowPlayerMovemnet(bool allow)
    {
        _movementAllowed = allow;
    }

    private void OnEnable()
    {
        InputActions.FindActionMap("Gameplay").Enable();
    }

    private void OnDisable()
    {
        InputActions.FindActionMap("Gameplay").Disable();
    }

    private void Awake()
    {
        _actionReference = InputActions.FindAction("Move");

    }

    private void FixedUpdate()
    {
        if (_movementAllowed==false)
        {
            return;
        }
        _moveInput = _actionReference.ReadValue<Vector2>();

        var headForward = _playerHead.forward;
        var headRight = _playerHead.right;

        headForward.y = 0;
        headRight.y = 0;

        headForward.Normalize();
        headRight.Normalize();

        Vector3 moveDir = headForward * _moveInput.y + headRight * _moveInput.x;
        _rigidBody.linearVelocity = new Vector3(moveDir.x * _movementSpeed,0, moveDir.z * _movementSpeed);
    }
}
