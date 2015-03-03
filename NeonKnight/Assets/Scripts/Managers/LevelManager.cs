﻿using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour {

	public static LevelManager manager;
	public PlayerManager playerManager;

	void Awake()
	{          
		manager = this;

		playerManager = GetComponent<PlayerManager>();
	}

	public GameObject GetPlayerInstance()
	{
		return GetComponent<PlayerManager>().GetPlayer();
	}

	public void EnablePlayer()
	{
		GetComponent<PlayerManager>().EnablePlayer();
	}

	public void DisablePlayer()
	{
		GetComponent<PlayerManager>().DisablePlayer();
	}

	//Everything that needs to be setup when the level is first loaded.
	public void InitializeLevel()
	{
		GetComponent<PlayerManager>().InitializePlayer();
		GetComponent<PlatformManager>().InitializePlatorms();
		GetComponent<CollectibleManager>().InitializeCollectibles();
		GetComponent<MegaByteManager>().LoadCollectedMegaBytes();
	}
	
	//Everything that needs to be reset upon player death 
	public void ResetLevel()
	{
		GetComponent<PlayerManager>().ResetPlayer();
		GetComponent<PlatformManager>().ResetPlatformPositions();
		GetComponent<CollectibleManager>().ResetCollectibles();
		GetComponent<MegaByteManager>().LoadCollectedMegaBytes();
	}
}
