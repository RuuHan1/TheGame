using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerController : MonoBehaviour
{

    [SerializeField] private float _moveSpeed = 5f;
    private Vector2 _moveInput;
    private Rigidbody2D _rb;
    private TrailRenderer _trail;


    public void SetMoveSpeed(float value)
    {
        _moveSpeed = value;
    }
    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _trail = GetComponent<TrailRenderer>();
    }
    private void OnEnable()
    {
        //GameEvents.PlayerPosition?.Invoke(transform);
        //Debug.Log("Invoked");
    }
    private void Start()
    {
        GameEvents.PlayerPosition?.Invoke(transform);

    }

    private void OnMove(InputValue value)
    {
        _moveInput = value.Get<Vector2>();
        FlipSprite(_moveInput);
    }

    private void FlipSprite(Vector2 value)
    {
        if (value.x < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (value.x > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
    }
    private void FixedUpdate()
    {
        _rb.MovePosition(_rb.position + _moveInput * _moveSpeed * Time.fixedDeltaTime);
        UpdateTrailEffect();
    }
    private void UpdateTrailEffect()
    {
        bool isMoving = _moveInput.magnitude > 0.1f;
        if (_trail != null)
        {
            _trail.emitting = isMoving;
        }
    }
}
