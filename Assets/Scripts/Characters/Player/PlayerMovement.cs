using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
	[Header("References")]
	[SerializeField] private LayerMask _groundLayer;
	[SerializeField] private Transform _groundCheck;
	[SerializeField] private Transform _leftWallCheck;
	[SerializeField] private Transform _rightWallCheck;

	[Header("Movement Settings")]
	[SerializeField] private Vector2 _groundCheckArea = new Vector2(1f, 0.1f);
	[SerializeField] private Vector2 _wallCheckArea = new Vector2(0.1f, 0.8f);
	[SerializeField] private float _baseSpeed = 6f;
	[SerializeField] private float _baseJumpForce = 7.4f;
	public float Speed => _baseSpeed;
	public float JumpForce => _baseJumpForce;

	private Rigidbody2D _rb;
	private SpriteRenderer _spriteRenderer;
	private float _moveX;

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
			bool isTryWalkInWall = (
				(_moveX > 0 && IsTouchingRightWall()) ||
				(_moveX < 0 && IsTouchingLeftWall())
			);

			if (!isTryWalkInWall)
				_rb.linearVelocityX = _moveX * Speed;
		}

		if (_jumpButtonPressed)
		{
			bool isGrounded;
			const float correctionThreshold = 0.1f;

			isGrounded = Physics2D.OverlapBox(
				_groundCheck.position,
				_groundCheckArea, 0f,
				_groundLayer
			) && Math.Abs(_rb.linearVelocityY) < correctionThreshold;

			if (isGrounded)
				_rb.AddForce(Vector2.up * JumpForce, ForceMode2D.Impulse);
		}
	}

	private bool IsTouchingLeftWall() => Physics2D.OverlapBox(
		_leftWallCheck.position,
		_wallCheckArea, 0f,
		_groundLayer
	);

	private bool IsTouchingRightWall() => Physics2D.OverlapBox(
		_rightWallCheck.position,
		_wallCheckArea, 0f,
		_groundLayer
	);

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

		Gizmos.color = Color.green;

		if (_groundCheck != null)
			Gizmos.DrawWireCube(
				new Vector3(_groundCheck.position.x, _groundCheck.position.y),
				new Vector3(_groundCheckArea.x, _groundCheckArea.y)
			);

		// Wall checkers
		if (_leftWallCheck != null)
			Gizmos.DrawWireCube(
				new Vector3(_leftWallCheck.position.x, _leftWallCheck.position.y),
				new Vector3(_wallCheckArea.x, _wallCheckArea.y)
			);

		// if (_rightWallCheck != null)
			Gizmos.DrawWireCube(
				new Vector3(_rightWallCheck.position.x, _rightWallCheck.position.y),
				new Vector3(_wallCheckArea.x, _wallCheckArea.y)
			);
	}
}
