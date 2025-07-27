using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
	[Header("Movement Settings")]
	// TODO: возможно, стоит сделать const
	[SerializeField] private float _groundCheckRadius = 0.2f;

	[Header("References")]
	[SerializeField] private LayerMask _groundLayer;
	[SerializeField] private Transform _groundCheck;

	private const float _baseSpeed = 6f;
	private const float _baseJumpForce = 5f;
	public float Speed => _baseSpeed;
	public float JumpForce => _baseJumpForce;

	private Rigidbody2D _rb;
	private SpriteRenderer _spriteRenderer;
	private float _moveX;
	private bool _isGrounded; // Обновляется в OnJump
	private bool _jumpButtonHolded;

	void Start()
	{
		_rb = GetComponent<Rigidbody2D>();
		_spriteRenderer = GetComponent<SpriteRenderer>();
	}

	void FixedUpdate()
	{
		if (_moveX != 0)
		{
			_rb.linearVelocityX = _moveX * Speed;
			Debug.Log($"Walking into {(_moveX > 0 ? "right" : "left")} side");
		}
	}

	// Вызывается при Move, как заданно в PlayerInput
	//InputAction.CallbackContext
	public void OnMove(InputValue value)
	{
		_moveX = value.Get<Vector2>().x;
		if (_moveX != 0)
			_spriteRenderer.flipX = _moveX < 0;
	}

	// Вызывается при Jump, как заданно в PlayerInput
	//InputAction.CallbackContext
	public void OnJump()
	{
		// Перенести в FixedUpdate, если будет использоваться где-то ещё
		_isGrounded = Physics2D.OverlapCircle(
			_groundCheck.position,
			_groundCheckRadius,
			_groundLayer
		);

		if (!_isGrounded)
			return;

		_rb.AddForce(Vector2.up * JumpForce, ForceMode2D.Impulse);
		Debug.Log($"Jumping");
	}

	public void OnDrawGizmos()
	{
		if (_groundCheck == null)
			return;

		Gizmos.color = Color.green;
		Gizmos.DrawWireSphere(_groundCheck.position, _groundCheckRadius);
		// Gizmos.DrawWireCube()
	}
}
