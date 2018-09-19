using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Jugador : MonoBehaviour {
	public List<GameObject> componentesAux;
	public List<ComponenteInsulto> Insulto; //componentes seleccionados en la ronda
	public bool insulto_terminado;
	public int debilidad;
	public int reputacion;
	public Image barra_reputacion;//no es nulo, esta direccionado en el editor de UNITY
	public Text text_insulto;

	void Start (){
		Insulto = new List<ComponenteInsulto>();
		insulto_terminado = false;
		reputacion = 100;
	}

	void Update (){
		barra_reputacion.fillAmount = (float)reputacion / (float)Constantes.MAX_REPUTACION;
		text_insulto.text = "";
		foreach (ComponenteInsulto comp in Insulto) {
			text_insulto.text += " " + comp.componente;
		}

	}
	//Asigna tambien la debilidad basado en el personaje
	public void CargarPersonaje (int personaje) {
		Animator anim = GetComponent<Animator> ();
		switch (personaje) {
		case Constantes.AMLO:
			anim.CrossFade ("Amlovsky", 0);
			debilidad = Constantes.EDAD;
			break;
		case Constantes.MEADE:
			anim.CrossFade ("Meade-O", 0);
			debilidad = Constantes.ASPECTO;
			break;
		case Constantes.ANAYA:
			anim.CrossFade ("Menonaya", 0);
			debilidad = Constantes.IMAGEN_PUBLICA;
			gameObject.GetComponent<SpriteRenderer> ().flipX = false;
			break;
		case Constantes.AI:
			anim.CrossFade ("AI", 0);
			gameObject.GetComponent<SpriteRenderer> ().flipX = false;
			debilidad = -1;
			break;
		}
	}
	public int Reputacion {
		get
		{
			return reputacion;
		}
		set
		{
			reputacion = value;

		}
	}
}
