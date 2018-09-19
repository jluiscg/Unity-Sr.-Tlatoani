using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotonSeleccionPersonaje : MonoBehaviour {
	public int id;
	public SeleccionPersonaje sel;
	// Use this for initialization
	void Start () {
		sel = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<SeleccionPersonaje> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	void OnMouseDown(){
		sel.IconoPresionado (id);
	}
}
