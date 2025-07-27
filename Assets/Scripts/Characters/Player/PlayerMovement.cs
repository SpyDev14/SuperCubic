using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
	[Header("Movement Settings")]
	[SerializeField] private Vector2 _groundCheckArea = new Vector2(1, 1);

	[Header("References")]
	[SerializeField] private LayerMask _groundLayer;
	[SerializeField] private Transform _groundCheck;

	// [SerializeField] private float speed;
	// [SerializeField] private float jumpForce;
	private const float _baseSpeed = 6f;
	private const float _baseJumpForce = 7.4f;
	public float Speed => _baseSpeed;
	public float JumpForce => _baseJumpForce;

	private Rigidbody2D _rb;
	private SpriteRenderer _spriteRenderer;
	private float _moveX;

	private bool _isGrounded;
	private bool _jumpButtonPressed;

	void Start()
	{
		_rb = GetComponent<Rigidbody2D>();
		_spriteRenderer = GetComponent<SpriteRenderer>();

		_rb.gravityScale = 1.3f;
	}

	void FixedUpdate()
	{
		if (_moveX != 0)
		{
			_rb.linearVelocityX = _moveX * Speed;
		}

		if (_jumpButtonPressed)
		{
			_isGrounded = Physics2D.OverlapBox(
				_groundCheck.position,
				_groundCheckArea, 0f,
				_groundLayer
			);

			if (!_isGrounded)
				return;

			_rb.AddForce(Vector2.up * JumpForce, ForceMode2D.Impulse);
		}
	}

	public void OnMove(InputValue value)
	{
		_moveX = value.Get<Vector2>().x;
		if (_moveX != 0)
			_spriteRenderer.flipX = _moveX < 0;
	}

	public void OnJump(InputValue value)
	{
		_jumpButtonPressed = value.Get<float>() > 0;
	}

	public void OnDrawGizmos()
	{
		if (_groundCheck == null)
			return;

		Gizmos.color = Color.green;
		Gizmos.DrawWireCube(
			new Vector3(_groundCheck.position.x, _groundCheck.position.y),
			new Vector3(_groundCheckArea.x, _groundCheckArea.y)
		);
	}
}
