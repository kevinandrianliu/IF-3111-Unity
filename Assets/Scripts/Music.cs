using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class Music : MonoBehaviour
{
    //public AudioSource musicSource;
	public static Music instance = null;
	public PlayerData playerData;

	void Awake() {
		/*AudioSource objs = GameObject.FindGameObjectWithTag("music").GetComponent<AudioSource>();
		playerData = PlayerPersistence.LoadData();
		objs.volume = playerData.musicVolume;*/
		//musicSource.volume = playerData.musicVolume;
		//AudioListener.volume = playerData.musicVolume;
		/*AudioSource musicSource = GetComponent<AudioSource>();
		musicSource.volume = playerData.musicVolume;*/ 
		if (instance == null){
            instance = this;
		}
        else if (instance != this){
            Destroy (gameObject);
		}
		DontDestroyOnLoad(gameObject);
	}
 
    void Update(){
        if (SceneManager.GetActiveScene().name == "PlayScene" || SceneManager.GetActiveScene().name == "Quit"){
            Destroy(this.gameObject);
        }
    }
}
