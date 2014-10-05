﻿using UnityEngine;
using System.Collections;

public class CameraLock : MonoBehaviour {

	public GameObject Player;
	public float fltCameraOffset = 7;
	public float fltSmoothTime = 0.5f;
	public float cameraSize = 20.0f;
	public float growthSpeed = 10.0f;
	public bool isPlaying = false;

	private Vector3 PlayerPos;
	private Vector3 CameraPos;
	private Vector3 velocity = Vector3.zero;
	
	void Start () 
	{
		PlayerPos.z = transform.position.z;
	}
	void FixedUpdate ()
	{
		if (isPlaying) 
		{
            if (camera.orthographicSize < cameraSize)
            {
                if(transform.localScale.y < 1)
                {
                    transform.localScale += new Vector3((growthSpeed/8) * Time.fixedDeltaTime, (growthSpeed/8) * Time.fixedDeltaTime, 0);
                }
                camera.orthographicSize += growthSpeed * Time.fixedDeltaTime;
            }
		}

		PlayerPos.x = Player.transform.position.x + fltCameraOffset;
		PlayerPos.y = Player.transform.position.y;
		
		CameraPos = transform.position;
		CameraPos.z = PlayerPos.z;
		
		transform.position = Vector3.SmoothDamp(CameraPos, PlayerPos, ref velocity, fltSmoothTime);

		if(PlayerPos.x + 5 > CameraPos.x)
			Player.GetComponent<PlayerScript>().blnPlayerActive = true;
	}
}
