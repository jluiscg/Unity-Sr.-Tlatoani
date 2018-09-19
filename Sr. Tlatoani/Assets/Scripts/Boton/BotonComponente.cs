using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BotonComponente : MonoBehaviour {
	public Text padre;
	public int id;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void OnMouseDown() {
		if (padre.GetComponent<ComportamientoComponente> ().Presionado (id)) {
			gameObject.SetActive (false);
		}
	}
	public void OnMouseDownRED() {
		if (padre.GetComponent<ComportamientoComponente> ().PresionadoRED (id)) {
			gameObject.SetActive (false);
		}
	}
}
