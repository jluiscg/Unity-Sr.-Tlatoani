using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Regresar_MenuP : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	void OnMouseDown() {
		Debug.Log ("regresando al menu principal");
		UnityEngine.SceneManagement.SceneManager.LoadScene ("Menu");
	}
}
