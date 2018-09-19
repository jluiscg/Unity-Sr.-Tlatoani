using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponenteInsulto{
	public string componente;
	public int clase;
	public int tipo_componente;
	public int ID;
	public static int id_it = 1;
	public ComponenteInsulto(string comp, int cla, int tip) {
		componente = comp;
		clase = cla;
		tipo_componente = tip;
		ID = id_it++;
	}
}
