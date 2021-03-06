﻿using UnityEngine;
using System.Collections;

public class CameraLock : MonoBehaviour {

	public static CameraLock cameraLock;

	public enum TargetType { player, staticObject, movingObject }
	public TargetType targetType = TargetType.staticObject;

	public GameObject target;
	public float playerXOffset = 7f;
	public float playerYOffset = 0f;
	public float smoothTime = 0.5f;
	private Vector3 velocity = Vector3.zero;

	void Awake()
	{          
		if(cameraLock == null)           //If manager doesn't exist, create one
		{
			DontDestroyOnLoad(gameObject);
			cameraLock = this;
		} else if(cameraLock != null)    //if manager does exist, destroy this copy
		{
			Destroy(gameObject);
		}
		cameraLock = this;
	}

	void FixedUpdate ()
	{
		if(target != null)
			PositionCamera();
	}

	public void SetCameraTargetPlayer(GameObject newTarget)
	{
		target = newTarget;
		targetType = TargetType.player;
	}

	public void SetCameraTarget(GameObject newTarget, TargetType newTargetType)
	{
		target = newTarget;
		targetType = newTargetType;
	}

	private void PositionCamera()
	{
		switch(targetType)
		{
		case TargetType.player:
			TargetTypePlayer();
			break;
		case TargetType.movingObject:
			break;
		case TargetType.staticObject:
		default:
			if(transform.position != Vector3.zero)
				transform.position = Vector3.zero;
			break;
		}
	}

	private void TargetTypePlayer()
	{
		Vector3 playerPos = Vector3.zero;
		//float cameraYOffset = 0f;
		
		if(target.GetComponent<Rigidbody2D>().velocity.y < 0 && target.GetComponent<Rigidbody2D>().velocity.y < -10)
			playerYOffset = Mathf.Lerp(playerYOffset, -6, Time.fixedDeltaTime*7);
		else
			playerYOffset = Mathf.Lerp(playerYOffset, 1, Time.fixedDeltaTime*10);
		
		playerPos.x = target.transform.position.x + playerXOffset;
		playerPos.y = target.transform.position.y + playerYOffset;
		playerPos.z = -10f;
		
		transform.position = Vector3.SmoothDamp(transform.position, playerPos, ref velocity, smoothTime);
	}
}
