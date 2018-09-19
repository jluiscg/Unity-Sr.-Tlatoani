using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteligenciaAtificial{
	private List<GameObject> ComponentesDisponibles;
	private List<GameObject> MejorSolucion;
	private List<GameObject> SolucionActual;
	private ComponenteInsulto ultimo;
	private ComponenteInsulto penultimo;
	private Partida partida;
	private int dano_max;
	private int count;


	public InteligenciaAtificial(List<GameObject> CDPub, List<GameObject>CDPriv, ComponenteInsulto ult, ComponenteInsulto penult, int co){
		ComponentesDisponibles = new List<GameObject> ();
		/*ComponentesDisponibles.AddRange (CDPub);
		ComponentesDisponibles.AddRange (CDPriv);*/
		foreach (GameObject c in CDPub) {
			if (c.activeSelf) {
				ComponentesDisponibles.Add (c);
			}
		}
		foreach (GameObject c in CDPriv) {
			if (c.activeSelf) {
				ComponentesDisponibles.Add (c);
			}
		}
		MejorSolucion = new List<GameObject> ();
		SolucionActual = new List<GameObject> ();
		ultimo = ult;
		penultimo = penult;
		count = co;
		partida = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<Partida> ();
		dano_max = 0;
	}
	bool QuedanComponentesValidos(List<GameObject> lista, ComponenteInsulto ult, ComponenteInsulto penult, int c){
		foreach (GameObject comp in lista) {
			if(partida.RevisorGramatical(comp.GetComponent<ComportamientoComponente>().Componente,ult,penult,c))
				return true;
		}
		return false;
	}
	/// <summary>
	/// busca en la lista de disponibles si existe
	/// un componente del mismo tipo pero que su clase
	/// sea la debilidad del jugador 1
	/// </summary>
	/// <returns>el componente encontrado
	/// si no encontro uno retorna el mismo
	/// componente recibido de parametro</returns>
	GameObject EncuentraConDebilidad(List<GameObject> lista_disp, GameObject comp){
		int debilidad = partida.Jugadores [0].GetComponent<Jugador> ().debilidad;
		if (comp.GetComponent<ComportamientoComponente> ().Componente.clase == debilidad)
			return comp;
		foreach (GameObject comp_eva in lista_disp) {
			if (comp_eva.GetComponent<ComportamientoComponente> ().Componente.clase == debilidad &&
				comp_eva.GetComponent<ComportamientoComponente> ().Componente.tipo_componente ==
				comp.GetComponent<ComportamientoComponente> ().Componente.tipo_componente) {
				return comp_eva;
			}
		}
		return comp;
	}
	public void TomarDecision (){
		Debug.Log ("La ia comienza su analisis");
		List<GameObject> ComponentesDisponiblesAux = new List<GameObject> ();
		ComponenteInsulto ultimoAux, penultimoAux;
		if(!QuedanComponentesValidos(ComponentesDisponibles,ultimo,penultimo,count)){
			partida.BotonTerminarRed ();
			Debug.Log ("LA IA TERMINA SU TURNO");
			return;
		}
		Debug.Log("AUN QUEDAN COMPONENTES ELEGIBLES");
		for (int i = 0; i < Constantes.GENERACIONES; i++) {
			Debug.Log ("Busqueda de la solucion " + (i + 1));
			ComponentesDisponiblesAux.Clear ();
			ComponentesDisponiblesAux.AddRange (ComponentesDisponibles);
			SolucionActual.Clear ();
			ultimoAux = ultimo;
			penultimoAux = penultimo;
			do{
				GameObject tmp = ComponentesDisponiblesAux[Random.Range(0,ComponentesDisponiblesAux.Count)];
				if(partida.RevisorGramatical(tmp.GetComponent<ComportamientoComponente>().Componente,ultimoAux,penultimoAux,count+SolucionActual.Count)){
					tmp = EncuentraConDebilidad(ComponentesDisponiblesAux,tmp);//priorizar debilidades
					ComponentesDisponiblesAux.Remove(tmp);
					penultimoAux = ultimoAux;
					ultimoAux = tmp.GetComponent<ComportamientoComponente>().Componente;
					SolucionActual.Add(tmp);
				}
			}while(QuedanComponentesValidos(ComponentesDisponiblesAux,ultimoAux,penultimoAux,count+SolucionActual.Count));
			int dano = calcularDano (SolucionActual);
			//Debug.Log ("Componentes en solucion " + (i + 1) + " antes: " + SolucionActual.Count+"\nDano en solucion " + (i + 1) + ": " + dano);
			int tipoUltimo = SolucionActual [SolucionActual.Count - 1].GetComponent<ComportamientoComponente> ().Componente.tipo_componente;
			while (SolucionActual.Count > 0) {
				tipoUltimo = SolucionActual [SolucionActual.Count - 1].GetComponent<ComportamientoComponente> ().Componente.tipo_componente;
				if (tipoUltimo != Constantes.SUJETO && tipoUltimo != Constantes.INSULTO_SIM) {
					SolucionActual.RemoveAt (SolucionActual.Count-1);
				} else
					break;
			}
			dano = calcularDano (SolucionActual);
			//Debug.Log ("Componentes en solucion " + (i + 1) + " despues: " + SolucionActual.Count+"\nDano en solucion " + (i + 1) + ": " + dano);
			if (dano > dano_max) {
				MejorSolucion.Clear ();
				MejorSolucion.AddRange (SolucionActual);
				dano_max = dano;
			}
		}

		if (MejorSolucion.Count > 0)
			EmitirSenalBotonDe (MejorSolucion [0]);
		else {
			partida.BotonTerminarRed ();
			Debug.Log ("LA IA TERMINA SU TURNO");
		}
	}
	void EmitirSenalBotonDe(GameObject componente){
		List<GameObject> listaBotones = new List<GameObject>(GameObject.FindGameObjectsWithTag ("BotonComponente"));
		foreach (GameObject boton in listaBotones) {
			if (boton.GetComponent<BotonComponente> ().padre.GetComponent<ComportamientoComponente>() == componente.GetComponent<ComportamientoComponente>()) {
				boton.GetComponent<BotonComponente> ().OnMouseDownRED ();
				Debug.Log ("Se encontro el boton");
				return;
			}
		}
		Debug.Log ("NO se encontro el boton");
	}
	int calcularDano(List<GameObject> insulto){
		float suma_dano = 0;
		float dano_comp = 0;
		foreach (GameObject comp in insulto) {
			switch (comp.GetComponent<ComportamientoComponente>().Componente.tipo_componente) {
			case Constantes.SUJETO:
				dano_comp = Constantes.DANO_S;
				break;
			case Constantes.INSULTO_CON:
				dano_comp = Constantes.DANO_IC;
				break;
			case Constantes.INSULTO_SIM:
				dano_comp = Constantes.DANO_IS;
				break;
			case Constantes.CONJUNCION:
				dano_comp = Constantes.DANO_C;
				break;
			case Constantes.SER_ESTAR:
				dano_comp = Constantes.DANO_SE;
				break;
			}
			if (comp.GetComponent<ComportamientoComponente>().Componente.clase == partida.Jugadores[0].GetComponent<Jugador>().debilidad) {
				float bonus = dano_comp * (Constantes.BONUS_DEBILIDAD/100);
				dano_comp = dano_comp + bonus;
			}
			suma_dano += dano_comp;
		}
		return (int)suma_dano;
	}

}
