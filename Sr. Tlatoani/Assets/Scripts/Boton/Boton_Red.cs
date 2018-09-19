using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boton_Red : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	void OnMouseDown() {
		UnityEngine.SceneManagement.SceneManager.LoadScene ("Menu_Red");
	}
}
