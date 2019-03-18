using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class MenuNavigation : MonoBehaviour
{
    //public Quad backgroundQuad
	public void LoadScene(string sceneName){
        SceneManager.LoadScene(sceneName);
    }
	
	
}
