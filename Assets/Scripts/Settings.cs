using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using System.IO;
using UnityEngine.SceneManagement;

public class Settings : MonoBehaviour
{
    public Toggle fullscreenToggle;
	public Dropdown colorBallDropdown;
	public Dropdown resolutionDropdown;
	public Dropdown backgroundDropdown;
	public AudioMixer audioMixer;
	public Resolution[] resolutions;
	public PlayerData playerData;
	public Button applyButton;
	public Button applyButton2;
	public Material background1;
	public Material background2;
	public Material background3;
	public GameObject ball;

	void OnEnable(){
		//playerData = new PlayerData();
		playerData = PlayerPersistence.LoadData();
		fullscreenToggle.onValueChanged.AddListener(delegate {onFullScreenToggle();});
		resolutionDropdown.onValueChanged.AddListener(delegate {setResolution();});
		colorBallDropdown.onValueChanged.AddListener(delegate {setColorBall();});
		backgroundDropdown.onValueChanged.AddListener(delegate {setBackground();});
		applyButton.onClick.AddListener(delegate {onApplyButtonClick();});
		applyButton2.onClick.AddListener(delegate {onApplyButton2Click();});
		resolutions = Screen.resolutions;
		foreach(Resolution resolution in resolutions){
			resolutionDropdown.options.Add(new Dropdown.OptionData(resolution.ToString()));
		}
		LoadSettings();
	}
	
	public void onFullScreenToggle(){
		playerData.fullScreen = Screen.fullScreen = fullscreenToggle.isOn;
	}
	
	public void setResolution(){
		Screen.SetResolution(resolutions[resolutionDropdown.value].width, resolutions[resolutionDropdown.value].height, Screen.fullScreen);
		playerData.resolutionIndex = resolutionDropdown.value;
	}
	
	public void setBackground(){
		playerData.backgroundIndex = backgroundDropdown.value;
	}
	
	public void setColorBall(){
		switch(colorBallDropdown.value){
			case 0 : playerData.colorBall = "red"; break;
			case 1 : playerData.colorBall = "green"; break;
			case 2 : playerData.colorBall = "blue"; break;
		}
		string color_ball = PlayerPrefs.GetString("colorBall");
		Renderer ball_render = ball.GetComponent<Renderer>();

	}
	
	public void setVolume (float volume){
		audioMixer.SetFloat("volume",volume);
		playerData.musicVolume = volume;
	}
	
	public void LoadScene(string sceneName){
        SceneManager.LoadScene(sceneName);
    }
	
	public void onApplyButtonClick(){
		SaveSettings();
	}

	public void onApplyButton2Click(){
		SceneManager.LoadScene("MainMenu");
	}
	
	public void SaveSettings(){
		/*string jsonData = JsonUtility.ToJson(playerData,true);
		File.WriteAllText(Application.persistentDataPath + "/gamesettings.json", jsonData);*/
		PlayerPersistence.SaveData(this);
	}
	
	public void LoadSettings(){
		//playerData = JsonUtility.FromJson<PlayerData>(File.ReadAllText(Application.persistentDataPath + "/gamesettings.json"));
		
		audioMixer.SetFloat("volume",playerData.musicVolume);
		if(playerData.colorBall == "red"){
			colorBallDropdown.value = 0;
		}
		else if(playerData.colorBall == "green"){
			colorBallDropdown.value = 1;
		}
		else{
			colorBallDropdown.value = 2;
		}
		resolutionDropdown.value = playerData.resolutionIndex;
		backgroundDropdown.value = playerData.backgroundIndex;
		fullscreenToggle.isOn = playerData.fullScreen;
	}
}
