using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotonMulti : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnMouseDown() {
		Debug.Log ("se pusho el boton multijugador");
		UnityEngine.SceneManagement.SceneManager.LoadScene ("Menu_multi");
	}
}
