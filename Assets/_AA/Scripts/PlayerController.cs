using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerController : MonoBehaviour
{

    [SerializeField] float moveSpeed = 5f;
    private Vector2 moveInput;
    private Rigidbody2D rb;




    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void OnEnable()
    {
        GameEvents.PlayerPosition?.Invoke(transform);
    }

    private void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
        FlipSprite(moveInput);
    }

    private void FlipSprite(Vector2 value)
    {
        if (value.x + value.y < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (value.x + value.y > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
    }
    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + moveInput * moveSpeed * Time.fixedDeltaTime);
    }
}
