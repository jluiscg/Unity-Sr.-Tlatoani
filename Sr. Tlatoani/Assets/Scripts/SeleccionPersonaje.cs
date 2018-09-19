using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SeleccionPersonaje : MonoBehaviour {
	public Text texto_seleccion;
	PreferenciasPartida prefs;
	// Use this for initialization
	void Start () {
		//de forma temporal en Start pues no hay escena de seleccion de personaje aun
		GameObject tmp = GameObject.FindGameObjectWithTag ("Prefs");
		prefs = tmp.GetComponent<PreferenciasPartida>();
		//decision de como seleccionara personajes, automatico un jugador y multijugador local
		//conexion con el otro cliente para seleccion en modo multijugador en red
		/*switch (prefs.tipoPartida) {
		case Constantes.SINGLE:
			prefs.personajes [0] = Constantes.AMLO;
			prefs.personajes [1] = Constantes.MEADE;
			break;
		case Constantes.MULTI_LOCAL:
			prefs.personajes [0] = Constantes.AMLO;
			prefs.personajes [1] = Constantes.MEADE;
			break;
		case Constantes.MULTI_RED_HOST:
			prefs.personajes [0] = Constantes.MEADE; // host siempre es el 1
			//prefs.server.SendBytesTo (1, (byte[])prefs.personajes [0], 4, 0);
			prefs.server.Enviar (prefs.personajes [0].ToString ());
			prefs.esperado = Constantes.SELECCION_JUGADOR;
			//aqui es donde debera esperar por la seleccion dle cliente
			break;
		case Constantes.MULTI_RED_CLI:
			//solo configura su personaje
			prefs.personajes [1] = Constantes.ANAYA; // el cliente siempre es el jugador 2
			break;
		}
		if(prefs.tipoPartida!=Constantes.MULTI_RED_HOST && prefs.tipoPartida!=Constantes.MULTI_RED_CLI)
			UnityEngine.SceneManagement.SceneManager.LoadScene ("Partida");*/
	}
	public void IconoPresionado(int personaje){
		if (prefs.tipoPartida == Constantes.SINGLE) {
			prefs.personajes [0] = personaje;
			prefs.personajes [1] = Constantes.AI;
			UnityEngine.SceneManagement.SceneManager.LoadScene ("Partida");
		} else if (prefs.tipoPartida == Constantes.MULTI_LOCAL) {
			if (prefs.personajes [0] == 0) {
				prefs.personajes [0] = personaje;
				texto_seleccion.text = "Seleccion jugador 2";
			} else {
				prefs.personajes [1] = personaje;
				UnityEngine.SceneManagement.SceneManager.LoadScene ("Partida");
			}
		} else if (prefs.tipoPartida == Constantes.MULTI_RED_HOST) {
			if (prefs.personajes [0] == 0) {
				prefs.personajes [0] = personaje;
				prefs.server.Enviar (prefs.personajes [0].ToString ());
				prefs.esperado = Constantes.SELECCION_JUGADOR;
				texto_seleccion.text = "Seleccion jugador 2";
			} else
				return;
		} else if (prefs.tipoPartida == Constantes.MULTI_RED_CLI) {
			if (prefs.personajes [0] == 0)
				return;
			else {
				prefs.personajes [1] = personaje;
				prefs.client.Enviar (prefs.personajes [1].ToString ());
				prefs.esperado = Constantes.ID_COMPONENTE;
				UnityEngine.SceneManagement.SceneManager.LoadScene ("Partida");
			}
		}
	}
		
	public void RegistrarSeleccionPersonaje(int per){
		if (prefs.tipoPartida == Constantes.MULTI_RED_CLI) {
			//el cliente recibe y posteriormente envia
			prefs.personajes [0] = per;
			texto_seleccion.text = "Seleccion jugador 2";
			/*prefs.client.Enviar (prefs.personajes [1].ToString ());
			prefs.esperado = Constantes.ID_COMPONENTE;*/
		} else {
			//es el host, solo recibe y guarda el dato
			prefs.personajes [1] = per;
			UnityEngine.SceneManagement.SceneManager.LoadScene ("Partida");
			//prefs.esperado = Constantes.NINGUNO;
		}
		//UnityEngine.SceneManagement.SceneManager.LoadScene ("Partida");
	}
	// Update is called once per frame
	void Update () {
		
	}
}
