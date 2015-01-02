﻿using UnityEngine;
using System.Collections;

public class GodPlatform : MonoBehaviour {

	public enum PlatformType { MoveableHorizontal, MoveableVertical, Static }
	public PlatformType platformType = PlatformType.Static;
	public float fltSolutionOffset = 5.0f;
	public float fltSnappingSpeed = 0.5f;
	public bool hasJumpPad = false;
	public bool enableGuideLinesGizmo = false;
	public bool enableJumpDistanceGizmo = false;

	public GameObject goJumpPad;
	public GameObject goPositionDot;
	public GameObject goMotionLine;
	public GameObject platformTexture;
	public Sprite staticTexture;
	public Sprite movableTexture;

	[HideInInspector] public Vector2 startPosition;
	[HideInInspector] public Vector2 centerPosition;
	[HideInInspector] public Vector2 solutionPosition;
	private bool m_positive;
	Vector2 currentPosition;
	public Vector2 GetStartPosition(){ return startPosition; }
	public Vector2 GetSolutionPosition(){ return solutionPosition; }
	private Vector3 velocity = Vector3.zero;
	
//Gizmos Code Holder
#if UNITY_EDITOR 
	void OnDrawGizmosSelected() 
	{

		switch(platformType)
		{
		case PlatformType.Static:
			
			break;
		case PlatformType.MoveableHorizontal:
			HorizontalSelectedGizmo();
			break;
		case PlatformType.MoveableVertical:
		default:
			VerticalSelectedGizmo();
			break;
		}

		//Used to help game design process
		if(enableGuideLinesGizmo) // horizontal and vertical lines that originate from the objects center 
		{
			Gizmos.color = Color.grey;
			Gizmos.DrawLine(new Vector2(transform.position.x - 50, transform.position.y + 0.242f), 
			                new Vector2(transform.position.x + 50, transform.position.y + 0.242f));
			Gizmos.DrawLine(new Vector2(transform.position.x, transform.position.y - 50), 
			                new Vector2(transform.position.x, transform.position.y + 50));
		}

		//Used to help game design process
		if(enableJumpDistanceGizmo)
		{
			Gizmos.color = Color.yellow;
			float jumpDistance = 3.0f; //offset from the plat's right edge as to where the players falrthest juemp will land them
			float gizmoLineLength = 1.30f; //this is actually half the lenght of the line

			//Right jump distance indicator
			Vector2 platformRightEdge = new Vector2(transform.position.x + 3.0f, transform.position.y);
			Gizmos.DrawLine(new Vector2(platformRightEdge.x + jumpDistance, transform.position.y - gizmoLineLength), 
			                new Vector2(platformRightEdge.x + jumpDistance, transform.position.y + gizmoLineLength));

			//Left jump distance indicator
			Vector2 platformLeftEdge = new Vector2(transform.position.x - 3.0f, transform.position.y);
			Gizmos.DrawLine(new Vector2(platformLeftEdge.x - jumpDistance, transform.position.y - gizmoLineLength), 
			                new Vector2(platformLeftEdge.x - jumpDistance, transform.position.y + gizmoLineLength));
		}
	}

	void OnDrawGizmos()
	{
		switch(platformType)
		{
		case PlatformType.Static:

			break;
		case PlatformType.MoveableHorizontal:
			HorizontalGizmo();
			break;
		case PlatformType.MoveableVertical:
		default:
			VerticalGizmo();
			break;
		}

		if(hasJumpPad)
		{
			Gizmos.color = Color.cyan;
			Gizmos.DrawWireCube(new Vector2(transform.position.x + 2.24f, transform.position.y + 0.68f), new Vector3(1.5f,0.75f,0));
		}
	}

	void VerticalGizmo()
	{
		startPosition = transform.position;
		solutionPosition = new Vector2 (startPosition.x, startPosition.y + fltSolutionOffset);
		Gizmos.color = Color.red;
		Gizmos.DrawWireCube(solutionPosition, new Vector3(5.75f,0.4f,0));
	}

	void VerticalSelectedGizmo()
	{
		startPosition = transform.position;
		solutionPosition = new Vector2 (startPosition.x, startPosition.y + fltSolutionOffset);
		
		if(enableJumpDistanceGizmo)
		{
			Gizmos.color = Color.green;
			float jumpDistance = 3.0f; //offset from the plat's right edge as to where the players falrthest juemp will land them
			float gizmoLineLength = 1.30f; //this is actually half the lenght of the line
			
			//Right jump distance indicator
			Vector2 platformRightEdge = new Vector2(transform.position.x + 3.0f, solutionPosition.y);
			Gizmos.DrawLine(new Vector2(platformRightEdge.x + jumpDistance, solutionPosition.y - gizmoLineLength), 
			                new Vector2(platformRightEdge.x + jumpDistance, solutionPosition.y + gizmoLineLength));
			
			//Left jump distance indicator
			Vector2 platformLeftEdge = new Vector2(transform.position.x - 3.0f, solutionPosition.y);
			Gizmos.DrawLine(new Vector2(platformLeftEdge.x - jumpDistance, solutionPosition.y - gizmoLineLength), 
			                new Vector2(platformLeftEdge.x - jumpDistance, solutionPosition.y + gizmoLineLength));
		}

		if(enableGuideLinesGizmo) // horizontal and vertical lines that originate from the objects center 
		{
			Gizmos.color = Color.grey;
			Gizmos.DrawLine(new Vector2(transform.position.x - 50, solutionPosition.y + 0.242f), 
			                new Vector2(transform.position.x + 50, solutionPosition.y + 0.242f));
		}
	}

	void HorizontalGizmo()
	{
		startPosition = transform.position;
		solutionPosition = new Vector2 (startPosition.x + fltSolutionOffset, startPosition.y);
		Gizmos.color = Color.magenta;
		Gizmos.DrawWireCube(solutionPosition, new Vector3(5.75f,0.4f,0));
	}

	void HorizontalSelectedGizmo()
	{
		startPosition = transform.position;
		solutionPosition = new Vector2 (startPosition.x + fltSolutionOffset, startPosition.y);
		
		if(enableJumpDistanceGizmo)
		{
			Gizmos.color = Color.green;
			float jumpDistance = 3.0f; //offset from the plat's right edge as to where the players falrthest juemp will land them
			float gizmoLineLength = 1.30f; //this is actually half the lenght of the line
			
			//Right jump distance indicator
			Vector2 platformRightEdge = new Vector2(solutionPosition.x + 3.0f, solutionPosition.y);
			Gizmos.DrawLine(new Vector2(platformRightEdge.x + jumpDistance, transform.position.y - gizmoLineLength), 
			                new Vector2(platformRightEdge.x + jumpDistance, transform.position.y + gizmoLineLength));
			
			//Left jump distance indicator
			Vector2 platformLeftEdge = new Vector2(solutionPosition.x - 3.0f, solutionPosition.y);
			Gizmos.DrawLine(new Vector2(platformLeftEdge.x - jumpDistance, platformLeftEdge.y - gizmoLineLength), 
			                new Vector2(platformLeftEdge.x - jumpDistance, platformLeftEdge.y + gizmoLineLength));
		}
	}
#endif

	void Start () 
	{
		switch(platformType)
		{
		case PlatformType.Static:
			GetComponent<TBDragToMove>().enabled = false;
			platformTexture.GetComponent<SpriteRenderer>().sprite = staticTexture;
			break;
		case PlatformType.MoveableHorizontal:
			HorizontalPlatformInitialize();
			break;
		case PlatformType.MoveableVertical:
		default:
			VerticalPlatformInitialize();
			break;
		}

		if(hasJumpPad)
		{
			goJumpPad.SetActive(true);
		}
	}
	
	void LateUpdate () 
	{
		switch(platformType)
		{
		case PlatformType.Static:
			
			break;
		case PlatformType.MoveableHorizontal:
			HorizontalPlatformConstraints();
			break;
		case PlatformType.MoveableVertical:
		default:
			VerticalPlatformConstraints();
			break;
		}
	}

	void VerticalPlatformInitialize()
	{
		platformTexture.GetComponent<SpriteRenderer>().sprite = movableTexture;
		startPosition = transform.position;
		solutionPosition = new Vector2 (startPosition.x, startPosition.y + fltSolutionOffset);
		centerPosition = new Vector2 (startPosition.x, (solutionPosition.y + startPosition.y)/2);
		if(solutionPosition.y - startPosition.y > 0)
			m_positive = true;
		else
			m_positive = false;
		
		GameObject motionLine;
		Vector2 startDotPos = new Vector2(startPosition.x + 0.075f, startPosition.y - 0.35f);
		Vector2 solutionDotPos = new Vector2(solutionPosition.x + 0.075f, solutionPosition.y - 0.35f);
		Vector2 centerLinePos = new Vector2 (startDotPos.x - 0.025f, (solutionDotPos.y + startDotPos.y)/2);
		
		Instantiate(goPositionDot, startDotPos, Quaternion.identity);
		Instantiate(goPositionDot, solutionDotPos, Quaternion.identity);
		motionLine = (GameObject)Instantiate(goMotionLine, centerLinePos, Quaternion.identity);
		motionLine.transform.GetChild(0).transform.localScale = new Vector3(Mathf.Abs(fltSolutionOffset), 5, 0);
	}

	void VerticalPlatformConstraints()
	{
		transform.position = new Vector2(startPosition.x, transform.position.y);
		if(m_positive) 
		{
			if(transform.position.y < centerPosition.y - fltSolutionOffset/8)
				transform.position = Vector3.SmoothDamp(transform.position, startPosition, ref velocity, fltSnappingSpeed);
			else
				transform.position = Vector3.SmoothDamp(transform.position, solutionPosition, ref velocity, fltSnappingSpeed);
			
			if(this.transform.position.y >= solutionPosition.y)
				this.transform.position = new Vector2(transform.position.x, solutionPosition.y);
			
			if(this.transform.position.y <= startPosition.y)
				this.transform.position = new Vector2(transform.position.x, startPosition.y);
		}
		else
		{
			if(transform.position.y < centerPosition.y - fltSolutionOffset/8)
				transform.position = Vector3.SmoothDamp(transform.position, solutionPosition, ref velocity, fltSnappingSpeed);
			else
				transform.position = Vector3.SmoothDamp(transform.position, startPosition, ref velocity, fltSnappingSpeed);
			
			if(this.transform.position.y >= startPosition.y)
				this.transform.position = new Vector2(transform.position.x, startPosition.y);
			
			if(this.transform.position.y <= solutionPosition.y)
				this.transform.position = new Vector2(transform.position.x, solutionPosition.y);
		}
	}

	void HorizontalPlatformInitialize()
	{
		platformTexture.GetComponent<SpriteRenderer>().sprite = movableTexture;
		startPosition = transform.position;
		solutionPosition = new Vector2 (startPosition.x + fltSolutionOffset, startPosition.y);
		centerPosition = new Vector2 ((solutionPosition.x + startPosition.x)/2, startPosition.y);
		if(solutionPosition.x - startPosition.x > 0)
			m_positive = true;
		else
			m_positive = false;
		
		GameObject motionLine;
		Vector2 startDotPos = new Vector2(startPosition.x + 0.0775f, startPosition.y - 0.35f);
		Vector2 solutionDotPos = new Vector2(solutionPosition.x + 0.0775f, solutionPosition.y - 0.35f);
		Vector2 centerLinePos = new Vector2 ((startDotPos.x + solutionDotPos.x)/2, startDotPos.y - 0.025f);
		
		Instantiate(goPositionDot, startDotPos, Quaternion.identity);
		Instantiate(goPositionDot, solutionDotPos, Quaternion.identity);
		motionLine = (GameObject)Instantiate(goMotionLine, centerLinePos, Quaternion.Euler(0, 0, 90));
		motionLine.transform.GetChild(0).transform.localScale = new Vector3(Mathf.Abs(fltSolutionOffset), 5, 0);
	}

	void HorizontalPlatformConstraints()
	{
		transform.position = new Vector2(transform.position.x, startPosition.y);
		if(m_positive) 
		{
			if(transform.position.x < centerPosition.x - fltSolutionOffset/8)
				transform.position = Vector3.SmoothDamp(transform.position, startPosition, ref velocity, fltSnappingSpeed);
			else
				transform.position = Vector3.SmoothDamp(transform.position, solutionPosition, ref velocity, fltSnappingSpeed);
			
			if(this.transform.position.x >= solutionPosition.x)
			{
				this.transform.position = new Vector2(solutionPosition.x, transform.position.y);
			}
			if(this.transform.position.x <= startPosition.x)
			{
				this.transform.position = new Vector2(startPosition.x, transform.position.y);
			}
		}
		else
		{
			if(transform.position.x < centerPosition.x - fltSolutionOffset/8)
				transform.position = Vector3.SmoothDamp(transform.position, solutionPosition, ref velocity, fltSnappingSpeed);
			else
				transform.position = Vector3.SmoothDamp(transform.position, startPosition, ref velocity, fltSnappingSpeed);
			
			if(this.transform.position.x >= startPosition.x)
			{
				this.transform.position = new Vector2(startPosition.x, transform.position.y);
			}
			if(this.transform.position.x <= solutionPosition.x)
			{
				this.transform.position = new Vector2(solutionPosition.x, transform.position.y);
			}
		}
	}
}