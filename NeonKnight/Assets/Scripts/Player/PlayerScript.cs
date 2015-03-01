﻿using UnityEngine;
using System.Collections;

public class PlayerScript: MonoBehaviour 
{
	public enum PlayerMotorState { walking, running, disabled }
	public PlayerMotorState playerMotorState = PlayerMotorState.disabled;

	public enum  JumpState { grounded, jumping, falling, inactive }
	public JumpState jumpState = JumpState.inactive;

	public enum JumpTypeState { normal, super }
	public JumpTypeState jumpTypeState = JumpTypeState.normal;

	public float moveSpeed = 7;
	public float acceleration = 20;
	public float jumpHeight = 11;
	public float superJumpHeight = 17.5f;
	public bool canJump = true;
	//public bool playerActive = true;
	
	[HideInInspector]
	public Animator playerAnimator;
	private float m_JumpHeight;

	void Awake()
	{
		m_JumpHeight = jumpHeight;
		//Animator code
		playerAnimator = transform.GetChild(0).GetComponent<Animator>();
	}

	void Start () 
	{	
		playerAnimator.SetInteger("Movement", 1);

		if (Application.loadedLevelName == "Level 1") 
		{
			canJump = false;
			moveSpeed = 5;
			jumpHeight = 10;
			superJumpHeight = 15;
		} 
		else 
		{
			canJump = true;
			moveSpeed = 7;
			jumpHeight = 11;
			superJumpHeight = 17.5f;
		}
	}

	void Update ()
	{
		HandleInput ();

		if(rigidbody2D.velocity.y < -0.5f)
		{
			jumpState = JumpState.falling;
		}

		if(playerMotorState == PlayerMotorState.disabled)
			playerAnimator.speed = 0;
		else
			playerAnimator.speed = 1;
	}

	void FixedUpdate () 
	{
		MovePlayer();
	}

	void HandleInput ()
	{
		if(playerMotorState != PlayerMotorState.disabled)
		{
			if (Input.GetKeyDown (KeyCode.Space) )
				Jump ();
		}
	}

	public void Jump()
	{
		if(jumpState == JumpState.grounded)
		{
			jumpState = JumpState.jumping;
			m_JumpHeight = GetJumpHeight();
			this.rigidbody2D.velocity = new Vector2 (moveSpeed, m_JumpHeight);
			StartCoroutine(JumpAnimation());
		}
	}

	float GetJumpHeight()
	{
		switch(jumpTypeState)
		{
		case JumpTypeState.super:
			return superJumpHeight;
		case JumpTypeState.normal:
		default:
			return jumpHeight;
		}
	}
	
	void MovePlayer()
	{
		switch(playerMotorState)
		{
		case PlayerMotorState.walking:
			this.rigidbody2D.isKinematic = false;
			this.rigidbody2D.velocity = new Vector2(moveSpeed, this.rigidbody2D.velocity.y);
			break;
		case PlayerMotorState.running:

			break;
		case PlayerMotorState.disabled:
		default:
			//canJump = false;
			this.rigidbody2D.isKinematic = true;
			this.rigidbody2D.velocity = Vector2.zero;
			break; 
		}
	}

	void OnTriggerEnter2D (Collider2D other)
	{
		if(other.CompareTag ("platform"))
			rigidbody2D.gravityScale = 20.0f;
		if(other.CompareTag("JumpPad"))
			jumpTypeState = JumpTypeState.super;
		if(other.tag == "collectibleIcon")
			Destroy (other.gameObject);
	}
	
	void OnTriggerExit2D (Collider2D other)
	{
		if(other.CompareTag ("platform"))
			rigidbody2D.gravityScale = 3.0f;
		if(other.CompareTag("JumpPad"))
			jumpTypeState = JumpTypeState.normal;
	}
	
	void OnCollisionEnter2D(Collision2D collision)
	{
		jumpState = JumpState.grounded;
		if(collision.gameObject.tag == "MasterPlatform")
			this.transform.parent = collision.gameObject.transform;
	}
	
	void OnCollisionExit2D(Collision2D collision)
	{
		this.transform.parent = null;
	}

	IEnumerator JumpAnimation()
	{
		playerAnimator.SetInteger("Movement", 0);
		yield return new WaitForSeconds(0.5f);
		playerAnimator.SetInteger("Movement", 1);
	}
}
