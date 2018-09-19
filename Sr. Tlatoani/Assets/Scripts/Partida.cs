using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Partida : MonoBehaviour {
	///listas de objetos de la partida 
	public List<GameObject> Jugadores;
	private List<ComponenteInsulto> Sujetos;
	private List<ComponenteInsulto> InsultosSimples;
	private List<ComponenteInsulto> InsultosContinuados;
	private List<GameObject> ListaCompartida;
	private List<int> ListaRepeticiones;
	private List<GameObject> BotonesComponentes;
	///objeto unico para esos tipos de componente
	private ComponenteInsulto conjuncion;
	private ComponenteInsulto ser_estar;
	///------------------------------------------
	/// archivos de los diccionarios, ligados a traves del inpector de unity (NO son null)
	public TextAsset archivo_sujetos;
	public TextAsset archivo_insultos_simples;
	public TextAsset archivo_insultos_continuados;
	///----------------------------------------------------------------------------------- 
	// imagenes de dialogos
	public GameObject dialogo1;
	public GameObject dialogo2;
	///-----------------------------------------------------------------------------------
	public int turno;//0 o 1 para indicar el turno de los jugadores
	public int numero_ronda;
	public bool juega1;
	public Sprite efecto_pow;
	public AudioClip sonido_golpe;
	public AudioSource audio_src;
	public PreferenciasPartida preferencias;

	public int componentesrecibidos;
	// Use this for initialization
	void Start () {
		turno = 0;
		numero_ronda = 1;
		juega1 = false;
		ListaRepeticiones = new List<int> ();
		Sujetos = new List<ComponenteInsulto> ();
		InsultosSimples = new List<ComponenteInsulto> ();
		InsultosContinuados = new List<ComponenteInsulto> ();

		//gameobject de preferencias de partida
		GameObject tmp = GameObject.FindGameObjectWithTag ("Prefs");
		preferencias = tmp.GetComponent<PreferenciasPartida> ();

		//logs de preferencias
		Debug.Log ("tipo de partida :" + preferencias.tipoPartida);
		Debug.Log ("personaje 1 :" + preferencias.personajes [0]);
		Debug.Log ("personaje 2 :" + preferencias.personajes [1]);

		//obtener los gameobjects pertenecientes a cada jugador
		Jugadores = new List<GameObject> (GameObject.FindGameObjectsWithTag ("Player"));
		//invertir lista pues unity regresa la lista de objetos en orden de
		//el ultimo agregado es el primero en la lista
		Jugadores.Reverse ();
		//carga las animaciones correspondientes a cada jugador
		int i = 0;
		foreach (GameObject jugador in Jugadores) {
			jugador.GetComponent<Jugador> ().CargarPersonaje (preferencias.personajes [i]);
			i++;
		}
		//cargar las listas de diccionarios desde el archivo
		Carga_Diccionarios ();
		//ListasLog ();//log temporal
		//obtener objetos componente de la escena 
		ListaCompartida = new List<GameObject>(GameObject.FindGameObjectsWithTag("ComponentePublico"));
		Jugadores[0].GetComponent<Jugador>().componentesAux = new List<GameObject>(GameObject.FindGameObjectsWithTag("ComponentePrivadoJ1"));
		Jugadores[1].GetComponent<Jugador>().componentesAux = new List<GameObject>(GameObject.FindGameObjectsWithTag("ComponentePrivadoJ2"));
		BotonesComponentes = new List<GameObject>(GameObject.FindGameObjectsWithTag("BotonComponente"));
		//GameObject.FindGameObjectWithTag ("POW1").SetActive (false);
		//GameObject.FindGameObjectWithTag ("POW2").SetActive (false);
		InicializarRonda ();
	}
	
	// Update is called once per frame
	void Update () 	{
		
			
	}
	//carga los diccionarios
	void Carga_Diccionarios() {
		CargaDiccionario (archivo_sujetos, Constantes.SUJETO);
		CargaDiccionario (archivo_insultos_simples, Constantes.INSULTO_SIM);
		CargaDiccionario (archivo_insultos_continuados, Constantes.INSULTO_CON);
		conjuncion = new ComponenteInsulto ("y", Constantes.NEUTRO, Constantes.CONJUNCION);
		ser_estar = new ComponenteInsulto ("es", Constantes.NEUTRO, Constantes.SER_ESTAR);
	}
	void CargaDiccionario(TextAsset archivo, int tipo_componente) {
		char actual;
		int i = 0;
		string buffer_archivo = archivo.text;
		//Debug.Log (buffer_archivo);
		string buffer_componente;
		string componente;
		int clase;
		//leer hasta el final del archivo
		do {
			//lee hasta la primer coma para obtener el componente
			buffer_componente = "";
			do{
				actual = buffer_archivo[i];
				if(actual!=',') buffer_componente += actual;
				i++;
			}while(actual!=',');
			componente=buffer_componente;
			//lee un caracter para sacar la clase
			actual=buffer_archivo[i];
			clase = (int)char.GetNumericValue(actual);//casteado pues GetNumericValue() regresa un double
			if(tipo_componente==Constantes.SUJETO)
				Sujetos.Add(new ComponenteInsulto(componente,clase,Constantes.SUJETO));
			else if(tipo_componente==Constantes.INSULTO_SIM)
				InsultosSimples.Add(new ComponenteInsulto(componente,clase,Constantes.INSULTO_SIM));
			else if(tipo_componente==Constantes.INSULTO_CON)
				InsultosContinuados.Add(new ComponenteInsulto(componente,clase,Constantes.INSULTO_CON));
			i+=3;
		} while(i < buffer_archivo.Length);
		//Debug.Log ("TERMINA LA CARGA DE UN DICCIONARIO");
	}
	void ListasLog() {
		foreach(ComponenteInsulto comp in Sujetos)
			Debug.Log ("Componente: \"" + comp.componente + "\" Clase: " + comp.clase + " Tipo: " + comp.tipo_componente + " ID: " + comp.ID);
		foreach(ComponenteInsulto comp in InsultosSimples)
			Debug.Log ("Componente: \"" + comp.componente + "\" Clase: " + comp.clase + " Tipo: " + comp.tipo_componente + " ID: " + comp.ID);
		foreach(ComponenteInsulto comp in InsultosContinuados)
			Debug.Log ("Componente: \"" + comp.componente + "\" Clase: " + comp.clase + " Tipo: " + comp.tipo_componente + " ID: " + comp.ID);
		
	}
	void InicializarRonda () {
		int indice;
		Jugadores [0].GetComponent<Jugador> ().insulto_terminado = false;
		Jugadores [1].GetComponent<Jugador> ().insulto_terminado = false;
		juega1 = !juega1;
		if (juega1) {
			turno = 0;
		} else {
			turno = 1;
		}
		CambiarColorInsultos ();
		//reactivar objetos
		foreach(GameObject g in BotonesComponentes){
			g.SetActive (true);//boton
			g.GetComponent<BotonComponente> ().padre.gameObject.SetActive (true);//gameobject del texto al que referencia
		}
		//si se es cliente, no se generan los componentes para la ronda
		//se espera por la indicacion del host
		if (preferencias.tipoPartida == Constantes.MULTI_RED_CLI) {
			preferencias.esperado = Constantes.ID_COMPONENTE;
			//preferencias.client.Enviar ("listo");
			return;
		} else if (preferencias.tipoPartida == Constantes.MULTI_RED_HOST) {
			preferencias.esperado = Constantes.NINGUNO;
		}
			
		//LISTA COMPARTIDA (PUBLICA)-----------------------------------------------------------------
		//3 sujetos
		LimpiarListaRepeticiones ();
		for (indice = 0; indice < 3; indice++) {
			ListaCompartida [indice].GetComponent<ComportamientoComponente> ().Componente = Sujetos[RandomSinRepetir(0,Sujetos.Count)];
		}
		LimpiarListaRepeticiones ();
		//3 insultos continuados
		for (; indice < 6; indice++) {
			ListaCompartida [indice].GetComponent<ComportamientoComponente> ().Componente = InsultosContinuados[RandomSinRepetir(0,InsultosContinuados.Count)];
		}
		LimpiarListaRepeticiones ();
		//insulto simple
		ListaCompartida [indice].GetComponent<ComportamientoComponente> ().Componente = InsultosSimples[RandomSinRepetir(0,InsultosSimples.Count)];
		indice++;
		//conjunciones y verbo ser estar
		//solo si es 0 se dara el caso de una conjuncion y un verbo ser/estar probabilidad del 20%
		if (Random.Range(0,5)!=0) {
			// caso de 2 conjunciones
			for (; indice < 9; indice++) {
				ListaCompartida [indice].GetComponent<ComportamientoComponente> ().Componente = conjuncion;
			}
		} else {
			ListaCompartida [7].GetComponent<ComportamientoComponente> ().Componente = conjuncion;
			ListaCompartida [8].GetComponent<ComportamientoComponente> ().Componente = ser_estar;
		}
		//------------------------------------------------------------------------------------------
		//LISTAS PRIVADAS------------------------------------------------------------------------
		foreach (GameObject jugador in Jugadores) {
			foreach (GameObject comp in jugador.GetComponent<Jugador>().componentesAux) {
				//rango de los tipos de componente, se usa directamente Random.Range() pues no es necesario que no se repitan
				switch(Random.Range(0, 7)){
				case Constantes.SUJETO:
					comp.GetComponent<ComportamientoComponente>().Componente = Sujetos[Random.Range(0,Sujetos.Count)];
					break;
				case Constantes.INSULTO_CON:
					comp.GetComponent<ComportamientoComponente>().Componente = InsultosContinuados[Random.Range(0,InsultosContinuados.Count)];
					break;
				case Constantes.INSULTO_SIM:
					comp.GetComponent<ComportamientoComponente>().Componente = InsultosSimples[Random.Range(0,InsultosSimples.Count)];
					break;
				case Constantes.CONJUNCION:
					comp.GetComponent<ComportamientoComponente>().Componente = conjuncion;
					break;
				case Constantes.SER_ESTAR:
					comp.GetComponent<ComportamientoComponente>().Componente = ser_estar;
					break;
				default:
					comp.GetComponent<ComportamientoComponente>().Componente = Sujetos[Random.Range(0,Sujetos.Count)];
					break;
				}
			}
		}
		//-----------------------------------------------------------------------------------------
		if(preferencias.tipoPartida==Constantes.SINGLE && turno==1){
			StartCoroutine (LLamarIA ());
		}
			
		if (preferencias.tipoPartida == Constantes.MULTI_RED_HOST) {
			StartCoroutine (EnviarComponentesDeRondaV2 ());
		}
		CambiarColorInsultos ();
	}
	int RandomSinRepetir(int min, int max){
		int obtenido;
		while(ListaRepeticiones.Contains(obtenido=Random.Range(min,max))){
			Debug.Log("Hubo repeticion");
		}
		ListaRepeticiones.Add (obtenido);
		return obtenido;
	}
	void LimpiarListaRepeticiones(){
		ListaRepeticiones.Clear ();
	}
	public void BotonTerminar(){
		//validacion turno en red
		if (preferencias.tipoPartida == Constantes.MULTI_RED_CLI && turno == 0)
			return;
		else if (preferencias.tipoPartida == Constantes.MULTI_RED_HOST && turno == 1)
			return;
		if (preferencias.tipoPartida == Constantes.SINGLE && turno == 1)
			return;
		int cero = 0;
		if (preferencias.tipoPartida == Constantes.MULTI_RED_HOST) {
			preferencias.server.Enviar (cero.ToString ());
		}else if (preferencias.tipoPartida == Constantes.MULTI_RED_CLI) {
			preferencias.client.Enviar (cero.ToString ());
		}
		BotonTerminarRed ();
	}
	public void BotonTerminarRed(){
		
		Debug.Log ("Se recibio la senal del boton terminar");
		Jugadores [turno].GetComponent<Jugador> ().insulto_terminado = true;
		if (Jugadores [0].GetComponent<Jugador> ().insulto_terminado && Jugadores [1].GetComponent<Jugador> ().insulto_terminado) {
			StartCoroutine(TerminarRonda ());
		} else {
			turno = 1 - turno; // cambiar valor entre 1 y 0
			CambiarColorInsultos ();
			if (preferencias.tipoPartida == Constantes.SINGLE && turno == 1) {
				StartCoroutine (LLamarIA ());
			}
		}
	}
	public bool RevisorGramatical(ComponenteInsulto comp, ComponenteInsulto ultimo, ComponenteInsulto penultimo, int count){
		if (ultimo == null) {
			if (comp.tipo_componente == Constantes.SUJETO) {
				//recien inicia su turno por lo que se necesita un componente de tipo sujeto
				return true;//el insulto fue aceptado
			} else
				return false;
		} else {
			//ya tiene componentes seleccionados, se revisa gramatica
			if (ultimo.tipo_componente == Constantes.SUJETO) {
				if (comp.tipo_componente == Constantes.SUJETO) {
					return false;
				} else if (comp.tipo_componente == Constantes.SER_ESTAR && penultimo!=null) {
					if (penultimo.tipo_componente == Constantes.CONJUNCION) {
						return true;
					} else
						return false;
				} else if ((comp.tipo_componente == Constantes.INSULTO_CON || comp.tipo_componente == Constantes.INSULTO_SIM || comp.tipo_componente == Constantes.SER_ESTAR) && penultimo!=null) {
					int tmp = penultimo.tipo_componente;
					if (tmp == Constantes.INSULTO_CON || tmp == Constantes.INSULTO_SIM || tmp == Constantes.SER_ESTAR) {
						return false;
					} else {
						return true;
					}
				} else {
					return true;
				}
			} else if (ultimo.tipo_componente == Constantes.INSULTO_CON) {
				if (comp.tipo_componente == Constantes.SUJETO) {
					return true;
				} else
					return false;
			} else if (ultimo.tipo_componente == Constantes.INSULTO_SIM) {
				if (comp.tipo_componente == Constantes.CONJUNCION) {
					return true;
				} else
					return false;
			} else if (ultimo.tipo_componente == Constantes.CONJUNCION) {
				if (comp.tipo_componente == Constantes.CONJUNCION) {
					return false;
				} else if (comp.tipo_componente == Constantes.SER_ESTAR || comp.tipo_componente == Constantes.INSULTO_CON || comp.tipo_componente == Constantes.INSULTO_SIM) {
					if (penultimo != null && (penultimo.tipo_componente == Constantes.SUJETO || 
						penultimo.tipo_componente == Constantes.INSULTO_CON ||
						penultimo.tipo_componente == Constantes.INSULTO_SIM || 
						penultimo.tipo_componente == Constantes.SER_ESTAR) && count >= 3) {
						return true;
					} 
					else
						return false;
				} else
					return true;
			} else if (ultimo.tipo_componente == Constantes.SER_ESTAR) {
				if (comp.tipo_componente == Constantes.SUJETO) {
					return true;
				} else
					return false;
			} else
				return false;
		}
	}
	public bool ComponentePulsadoRed(ComponenteInsulto comp, string tag_lista, int id){
		//revisar validez de componente pulsado por lista
		if (turno == 0) {
			//caso jugador 1
			if (tag_lista != "ComponentePublico" && tag_lista != "ComponentePrivadoJ1") {
				Debug.Log ("RECHAZADO POR PROPIEDAD");
				return false;
			}
		} else {
			//caso jugador 2
			if (tag_lista != "ComponentePublico" && tag_lista != "ComponentePrivadoJ2") {
				Debug.Log ("RECHAZADO POR PROPIEDAD");
				return false;
			}
		}
		//revisar primer caso en que el jugador esta iniciando su insulto
		Jugador jugador_act = Jugadores [turno].GetComponent<Jugador> ();
		ComponenteInsulto ultimo;
		ComponenteInsulto penultimo;
		bool aceptado = false;
		if (jugador_act.Insulto.Count == 0) {
			ultimo = null;
			penultimo = null;
		} else if (jugador_act.Insulto.Count == 1) {
			ultimo = jugador_act.Insulto [jugador_act.Insulto.Count - 1];
			penultimo = null;
		} else {
			ultimo = jugador_act.Insulto [jugador_act.Insulto.Count - 1];
			penultimo = jugador_act.Insulto [jugador_act.Insulto.Count - 2];
		}
		aceptado = RevisorGramatical (comp, ultimo, penultimo,jugador_act.Insulto.Count);
		if(aceptado){
			FinalizarTurno (jugador_act, comp);
			if (preferencias.tipoPartida == Constantes.MULTI_RED_HOST) {
				preferencias.server.Enviar (id.ToString ());
				//				preferencias.esperado = Constantes.ID_BOTON_COMPONENTE_ELEGIDO;
			} else if (preferencias.tipoPartida == Constantes.MULTI_RED_CLI) {
				preferencias.client.Enviar (id.ToString ());
				//				preferencias.esperado = Constantes.ID_BOTON_COMPONENTE_ELEGIDO;
			}

		}
		return aceptado;
	}
	public bool ComponentePulsado (ComponenteInsulto comp, string tag_lista, int id)
	{
		//validacion turnos en red
		if (preferencias.tipoPartida == Constantes.MULTI_RED_CLI && turno == 0)
			return false;
		else if (preferencias.tipoPartida == Constantes.MULTI_RED_HOST && turno == 1)
			return false;
		else if (preferencias.tipoPartida == Constantes.SINGLE && turno == 1)
			return false;
		return ComponentePulsadoRed(comp,tag_lista,id);

	}
	void FinalizarTurno(Jugador jug, ComponenteInsulto comp){
		jug.Insulto.Add (comp);
		turno = 1 - turno; // cambiar valor entre 1 y 0
		if(Jugadores[turno].GetComponent<Jugador>().insulto_terminado){
			turno = 1 - turno;//se cambia de nuevo en caso de que el otro jugador haya terminado su turno
		}
		CambiarColorInsultos();
		//revisar si es partida contra la IA
		if (preferencias.tipoPartida == Constantes.SINGLE&&turno==1) {
			StartCoroutine (LLamarIA ());
		}

	}
	void CambiarColorInsultos(){
		if (turno == 0) {
			Jugadores[0].GetComponent<Jugador>().text_insulto.color = Color.black;
			dialogo1.GetComponent<SpriteRenderer> ().color = Color.white;
			Jugadores[1].GetComponent<Jugador>().text_insulto.color = Color.white;
			dialogo2.GetComponent<SpriteRenderer> ().color = Color.grey;
			if (preferencias.tipoPartida == Constantes.MULTI_RED_HOST) {
				preferencias.esperado = Constantes.NINGUNO;
			} else if (preferencias.tipoPartida == Constantes.MULTI_RED_CLI) {
				preferencias.esperado = Constantes.ID_BOTON_COMPONENTE_ELEGIDO;
			}
		} else {
			Jugadores[0].GetComponent<Jugador>().text_insulto.color = Color.white;
			dialogo1.GetComponent<SpriteRenderer> ().color = Color.grey;
			Jugadores[1].GetComponent<Jugador>().text_insulto.color = Color.black;
			dialogo2.GetComponent<SpriteRenderer> ().color = Color.white;
			if (preferencias.tipoPartida == Constantes.MULTI_RED_HOST) {
				preferencias.esperado = Constantes.ID_BOTON_COMPONENTE_ELEGIDO;
			} else if (preferencias.tipoPartida == Constantes.MULTI_RED_CLI) {
				preferencias.esperado = Constantes.NINGUNO;
			}
		}
	}
	//es IEnumerator para poder usar WaitForSecondsRealTime ()y 
	IEnumerator LLamarIA(){
		for (int i = 0; i < 2; i++) {
			Jugadores [turno].GetComponent<SpriteRenderer> ().color = Color.grey;
			yield return new WaitForSeconds (0.45f);
			Jugadores [turno].GetComponent<SpriteRenderer> ().color = Color.white;
			yield return new WaitForSeconds (0.45f);
		}
		Jugador jugador_act = Jugadores [turno].GetComponent<Jugador> ();
		ComponenteInsulto ultimo;
		ComponenteInsulto penultimo;
		if (jugador_act.Insulto.Count == 0) {
			ultimo = null;
			penultimo = null;
		} else if (jugador_act.Insulto.Count == 1) {
			ultimo = jugador_act.Insulto [jugador_act.Insulto.Count - 1];
			penultimo = null;
		} else {
			ultimo = jugador_act.Insulto [jugador_act.Insulto.Count - 1];
			penultimo = jugador_act.Insulto [jugador_act.Insulto.Count - 2];
		}
		InteligenciaAtificial ia = new InteligenciaAtificial (ListaCompartida, jugador_act.componentesAux, ultimo, penultimo, jugador_act.Insulto.Count);
		ia.TomarDecision ();
	}
	IEnumerator efectoGolpe(GameObject efecto){
		efecto.GetComponent<SpriteRenderer> ().sprite = efecto_pow;
		audio_src.PlayOneShot (sonido_golpe, 1.0f);
		yield return new WaitForSecondsRealtime(1.0f);
		efecto.GetComponent<SpriteRenderer> ().sprite = null;
	}
	//es IEnumerator para poder usar WaitForSecondsRealTime ()  para la animacion del texto parpadenado
	IEnumerator TerminarRonda(){
		Jugador atacante;
		Jugador receptor;
		int dano;
		int dano_si_mismo = 0;
		bool mala_finalizacion = false;
		numero_ronda++;
		//calcular cantidad de dano de jugador 1 a jugador 2
		atacante = Jugadores[0].GetComponent<Jugador>();
		receptor = Jugadores[1].GetComponent<Jugador>();
		dano = CalcularDano (atacante, receptor);
		receptor.Reputacion -= dano;
		StartCoroutine (efectoGolpe (GameObject.FindGameObjectWithTag ("POW2")));
		if (atacante.Insulto.Count > 0 &&
			atacante.Insulto [atacante.Insulto.Count - 1].tipo_componente != Constantes.SUJETO &&
			atacante.Insulto [atacante.Insulto.Count - 1].tipo_componente != Constantes.INSULTO_SIM) {
			mala_finalizacion = true;
			dano_si_mismo = Mathf.CeilToInt ((float)dano * (Constantes.PORCENTAJE_DANO_PROPIO/100));
			atacante.Reputacion -= dano_si_mismo;
			StartCoroutine (efectoGolpe (GameObject.FindGameObjectWithTag ("POW1")));
		}
		//texto de dano hecho 
		GameObject.FindGameObjectWithTag("Dano2").GetComponent<Text>().text = "- "+dano;
		if (mala_finalizacion) {
			GameObject.FindGameObjectWithTag("Dano1").GetComponent<Text>().text = "- "+dano_si_mismo;
		}
		//hacer parpadear texto de atacante 
		for (int i = 0; i < 10; i++) {
			atacante.text_insulto.color = Color.green;
			yield return new WaitForSecondsRealtime (0.24f);
			atacante.text_insulto.color = Color.black;
			yield return new WaitForSecondsRealtime (0.24f);
			GameObject.FindGameObjectWithTag ("Dano2").GetComponent<Text> ().transform.Translate(0.0f,10.0f, 0.0f, Space.World);
			if (mala_finalizacion) {
				GameObject.FindGameObjectWithTag("Dano1").GetComponent<Text>().transform.Translate(0.0f,10.0f, 0.0f, Space.World);
			}
		}
		GameObject.FindGameObjectWithTag("Dano2").GetComponent<Text>().text = "";
		GameObject.FindGameObjectWithTag ("Dano2").GetComponent<Text> ().transform.Translate(0.0f,-100.0f, 0.0f, Space.World);

		if (mala_finalizacion) {
			mala_finalizacion = false;
			GameObject.FindGameObjectWithTag("Dano1").GetComponent<Text>().text = "";
			dano_si_mismo = 0;
			GameObject.FindGameObjectWithTag("Dano1").GetComponent<Text>().transform.Translate(0.0f,-100.0f, 0.0f, Space.World);
		}


		//calcular cantidad de dano de jugador 2 a jugador 1
		atacante = Jugadores[1].GetComponent<Jugador>();
		receptor = Jugadores[0].GetComponent<Jugador>();
		dano = CalcularDano (atacante, receptor);
		receptor.reputacion -= dano;
		StartCoroutine (efectoGolpe (GameObject.FindGameObjectWithTag ("POW1")));
		if (atacante.Insulto.Count > 0 &&
			atacante.Insulto [atacante.Insulto.Count - 1].tipo_componente != Constantes.SUJETO &&
			atacante.Insulto [atacante.Insulto.Count - 1].tipo_componente != Constantes.INSULTO_SIM) {
			mala_finalizacion = true;
			dano_si_mismo = Mathf.CeilToInt ((float)dano * (Constantes.PORCENTAJE_DANO_PROPIO/100));
			atacante.Reputacion -= dano_si_mismo;
			StartCoroutine (efectoGolpe (GameObject.FindGameObjectWithTag ("POW2")));
		}
		GameObject.FindGameObjectWithTag("Dano1").GetComponent<Text>().text = "- "+dano;
		if (mala_finalizacion) {
			GameObject.FindGameObjectWithTag("Dano2").GetComponent<Text>().text = "- "+dano_si_mismo;
		}
		//lo mesmo
		for (int i = 0; i < 10; i++) {
			atacante.text_insulto.color = Color.green;
			yield return new WaitForSecondsRealtime (0.24f);
			atacante.text_insulto.color = Color.black;
			yield return new WaitForSecondsRealtime (0.24f);
			GameObject.FindGameObjectWithTag ("Dano1").GetComponent<Text> ().transform.Translate(0.0f,10.0f, 0.0f, Space.World);
			if (mala_finalizacion) {
				GameObject.FindGameObjectWithTag("Dano2").GetComponent<Text>().transform.Translate(0.0f,10.0f, 0.0f, Space.World);
			}
		}
		GameObject.FindGameObjectWithTag("Dano1").GetComponent<Text>().text = "";
		GameObject.FindGameObjectWithTag ("Dano1").GetComponent<Text> ().transform.Translate(0.0f,-100.0f, 0.0f, Space.World);

		if (mala_finalizacion) {
			mala_finalizacion = false;
			GameObject.FindGameObjectWithTag("Dano2").GetComponent<Text>().text = "";
			GameObject.FindGameObjectWithTag("Dano2").GetComponent<Text>().transform.Translate(0.0f,-100.0f, 0.0f, Space.World);
			dano_si_mismo = 0;
		}
		Jugadores [0].GetComponent<Jugador> ().Insulto = new List<ComponenteInsulto>();
		Jugadores [1].GetComponent<Jugador> ().Insulto = new List<ComponenteInsulto>();
		ValidarFinPartida ();
		InicializarRonda ();
	}
	int CalcularDano(Jugador ata, Jugador rec){
		float suma_dano = 0;
		float dano_comp = 0;
		foreach (ComponenteInsulto comp in ata.Insulto) {
			switch (comp.tipo_componente) {
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
			if (comp.clase == rec.debilidad) {
				float bonus = dano_comp * (Constantes.BONUS_DEBILIDAD/100);
				dano_comp = dano_comp + bonus;
			}
			suma_dano += dano_comp;
		}
		return (int)suma_dano;
	}

	void ValidarFinPartida(){
		if (Jugadores [0].GetComponent<Jugador> ().reputacion <= 0 && Jugadores [1].GetComponent<Jugador> ().reputacion <= 0) {
			preferencias.victoria = Constantes.EMPATE;
		} else if (Jugadores [0].GetComponent<Jugador> ().reputacion <= 0) {
			preferencias.victoria = Constantes.VICTORIA_J2;
		} else if (Jugadores [1].GetComponent<Jugador> ().reputacion <= 0) {
			preferencias.victoria = Constantes.VICTORIA_J1;
		}
		else return;
		if (preferencias.tipoPartida == Constantes.MULTI_RED_HOST) {
			preferencias.server.closeServer ();
			Destroy (preferencias.server);
		} else if (preferencias.tipoPartida == Constantes.MULTI_RED_CLI) {
			preferencias.client.closeClient ();
			Destroy (preferencias.client);
		}
		UnityEngine.SceneManagement.SceneManager.LoadScene ("PantallaVictoria");
	}
	ComponenteInsulto BuscaComponente(int id){
		foreach(ComponenteInsulto c in Sujetos){
			if (c.ID == id) {
				//es el componente
				return c;
			}
		}
		//si no se encontro se busca en insultos simples
		foreach(ComponenteInsulto c in InsultosSimples){
			if (c.ID == id) {
				//es el componente
			return c;
			}
		}
		//si no se encontro se busca en insultos continuados
		foreach(ComponenteInsulto c in InsultosContinuados){
			if (c.ID == id) {
				//es el componente
			return c;
			}
		}
		//si no se encontro se revisa si es verbo ser o conjuncion
		if (ser_estar.ID == id) {
			//es el componente
			return ser_estar;
		} else {
			return conjuncion;
		}
	}

	public IEnumerator RecibirComponenteV2(string msg){
		ComponenteInsulto comp;
		foreach(GameObject g in BotonesComponentes){
			g.SetActive (true);//boton
			g.GetComponent<BotonComponente> ().padre.gameObject.SetActive (true);//gameobject del texto al que referencia
		}
		List<GameObject> botones = new List<GameObject>(GameObject.FindGameObjectsWithTag ("BotonComponente"));
		string[] ids = msg.Split (',');
		componentesrecibidos = 1;
		foreach (string id_s in ids) {
			Debug.Log (id_s);
			comp = BuscaComponente (int.Parse (id_s));
			foreach (GameObject go in botones) {
				int aux = go.GetComponent<BotonComponente> ().id;
				if(aux==(componentesrecibidos))
					go.GetComponent<BotonComponente> ().padre.GetComponent<ComportamientoComponente> ().Componente = comp;
			}
			componentesrecibidos++;
		}
		CambiarColorInsultos();
		yield return null;
	}
	public IEnumerator EnviarComponentesDeRondaV2(){
		List<GameObject> botones = new List<GameObject>(GameObject.FindGameObjectsWithTag ("BotonComponente"));
		string mensaje="";
		yield return new WaitForSecondsRealtime(0.1f);
		for (int i = 1; i <= 15; i++) {
			foreach (GameObject go in botones) {
				int aux = go.GetComponent<BotonComponente> ().id;
				if (aux == i) {
					aux = go.GetComponent<BotonComponente> ().padre.GetComponent<ComportamientoComponente> ().Componente.ID;
					mensaje += aux.ToString ();
					if (i < 15)
						mensaje += ",";
				}
			}
		}
		Debug.Log (mensaje);
		preferencias.server.Enviar (mensaje);
		CambiarColorInsultos();
	}

	public void RecibirSeleccionRed(int id){
		List<GameObject> botones = new List<GameObject>(GameObject.FindGameObjectsWithTag ("BotonComponente"));
		foreach (GameObject go in botones) {
			int aux = go.GetComponent<BotonComponente> ().id;
			if (aux == id) {
				go.GetComponent<BotonComponente> ().OnMouseDownRED();
			}
		}
	}
}
