using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComportamientoComponente : MonoBehaviour {
	private ComponenteInsulto componente=null;
	public ComponenteInsulto Componente{
		get
		{
			return componente;
		}
		set
		{
			componente = value;
			if(componente!=null)GetComponent<Text> ().text=componente.componente;
			else GetComponent<Text> ().text="null";
		}
	}
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public bool Presionado(int id) {
		Partida p = GameObject.Find ("Main Camera").GetComponent<Partida> ();
		if (p.ComponentePulsado (componente, gameObject.tag, id)) {
			gameObject.SetActive (false);
			return true;
		}
		return false;
	}
	public bool PresionadoRED(int id) {
		Partida p = GameObject.Find ("Main Camera").GetComponent<Partida> ();
		if (p.ComponentePulsadoRed (componente, gameObject.tag, id)) {
			gameObject.SetActive (false);
			return true;
		}
		return false;
	}
}
