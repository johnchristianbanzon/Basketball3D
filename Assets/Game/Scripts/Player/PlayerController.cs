using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private TriggerBehavior _triggerCheck;
    [SerializeField]
    private GameObject _pickUpText;
    private GameObject _highlightedObject;
    private Rigidbody _heldObjectRigidBody;
    public InputActionAsset InputActions;
    [SerializeField]
    private Transform _playerHead;
    [SerializeField] private Transform holdPoint;
    [SerializeField] private float holdForce = 20f;
    [SerializeField] private float throwForce = 10f;
    [SerializeField]
    private TrajectoryManager _trajectoryManager;
    [SerializeField]
    private PowerManager _powerManager;
    [SerializeField]
    private PlayerMovement _playerMovement;
    [Inject]
    private GameManager _gameManager;
    private InputAction _pickUpActionReference;
    private InputAction _throwActionReference;
    private bool _isHoldingObject;
    private bool _allowPlayerMovement;

    public void AllowPlayerMovement(bool allowPlayerMovement)
    {
        _allowPlayerMovement = allowPlayerMovement;
        _playerMovement.AllowPlayerMovemnet(allowPlayerMovement);
    }

    private void Awake()
    {
        _triggerCheck.SetTriggerEvent(OnHighlightObject, OnDeHighlightObject);
        _throwActionReference = InputActions.FindAction("Shoot");
        _throwActionReference.started += OnStartShoot;
        _throwActionReference.canceled += OnCompleteShoot;
    }

    private void OnCompleteShoot(InputAction.CallbackContext context)
    {
        if (_isHoldingObject)
        {
            _powerManager.OnStop();
            Throw();
        }
        else
        {
            TryPickUpObject();
        }
        
    }

    private void OnStartShoot(InputAction.CallbackContext context)
    {
        if (_isHoldingObject )
        {
            _powerManager.StartPowerMode(OnUpdatePower);
        }
    }

    private void OnUpdatePower(float throwPower)
    {
        throwForce = throwPower;
        _trajectoryManager.SetThrowPowerForce(throwPower);
        _trajectoryManager.DrawTrajectory(_heldObjectRigidBody.transform);
    }

    private void OnDeHighlightObject(GameObject highlightedObject)
    {
        if (_isHoldingObject)
        {
            return;
        }
        _highlightedObject = null;
        _pickUpText.gameObject.SetActive(false);
    }

    private void OnHighlightObject(GameObject highlightedObject)
    {
        if (_isHoldingObject)
        {
            return;
        }
        _highlightedObject = highlightedObject;
        _pickUpText.gameObject.SetActive(true);
    }

    private void TryPickUpObject()
    {
        if (_highlightedObject == null)
        {
            return;
        }
        _isHoldingObject = true;
        _heldObjectRigidBody = _highlightedObject.GetComponent<Rigidbody>();
        Debug.Log("_heldObjectRigidBody :" + _heldObjectRigidBody.name);
        _heldObjectRigidBody.useGravity = false;
        _heldObjectRigidBody.linearDamping = 10f; // reduce wobble
        _heldObjectRigidBody.angularDamping = 10f;
        _heldObjectRigidBody.GetComponent<SphereCollider>().enabled = false;
        _pickUpText.gameObject.SetActive(false);
    }



    private void FixedUpdate()
    {
        if (_heldObjectRigidBody != null && _isHoldingObject)
        {
            Debug.Log("_heldObjectRigidBody :" + _heldObjectRigidBody + " / _isHoldingObject : "+ _isHoldingObject);
            Vector3 direction = holdPoint.position - _heldObjectRigidBody.position;

            _heldObjectRigidBody.linearVelocity = direction * holdForce;
            
        }
    }

 

    private void Throw()
    {
        if (_heldObjectRigidBody == null) return;
        _isHoldingObject = false;
        Debug.Log("_isHoldingObject :" + _isHoldingObject);
        _heldObjectRigidBody.useGravity = true;
        _heldObjectRigidBody.linearDamping = 0.3f;
        _heldObjectRigidBody.angularDamping = 1f;
        // Add forward force
        Vector3 dir = _trajectoryManager.GetShootDirection();

        _heldObjectRigidBody.AddForce(dir * throwForce, ForceMode.Impulse);
        _heldObjectRigidBody.GetComponent<SphereCollider>().enabled = true;
        // Optional: add spin for realism
        _heldObjectRigidBody.AddTorque(UnityEngine.Random.insideUnitSphere * 2f, ForceMode.Impulse);

        _heldObjectRigidBody = null;
        _highlightedObject = null;
        _trajectoryManager.ClearTrajectory();
        _gameManager.CheckBall();

    }
}