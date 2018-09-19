using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boton_Terminar : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	void OnMouseDown() {
		Debug.Log ("se pusho terminar");
		GameObject.Find ("Main Camera").GetComponent<Partida> ().BotonTerminar ();
	}
}
