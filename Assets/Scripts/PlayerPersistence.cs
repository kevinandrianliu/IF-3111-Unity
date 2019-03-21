using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPersistence : MonoBehaviour
{
    public static void SaveData(Settings settings){
		PlayerPrefs.SetInt("fullScreen", settings.playerData.fullScreen?1:0);
		PlayerPrefs.SetInt("resolutionIndex", settings.playerData.resolutionIndex);
		PlayerPrefs.SetFloat("musicVolume", settings.playerData.musicVolume);
		PlayerPrefs.SetString("colorBall", settings.playerData.colorBall);
		PlayerPrefs.SetInt("backgroundIndex", settings.playerData.backgroundIndex);
	}

    public static PlayerData LoadData(){
		float volume = PlayerPrefs.GetFloat("musicVolume");
		int resolution_index = PlayerPrefs.GetInt("resolutionIndex");
		string color_ball = PlayerPrefs.GetString("colorBall");
		bool full_screen = PlayerPrefs.GetInt("fullScreen")==1?true:false;
		int background_index = PlayerPrefs.GetInt("backgroundIndex");
		//int times_scene_opened = PlayerPrefs.GetInt("timessceneopened");
		PlayerData playerData = new PlayerData(){
			musicVolume = volume,
			resolutionIndex = resolution_index,
			colorBall = color_ball,
			fullScreen = full_screen,
			backgroundIndex = background_index,
		};
		return playerData;
	}
}
