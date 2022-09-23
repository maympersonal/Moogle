namespace MoogleEngine;

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

//Clase que busca las palabras vacías en los documentos
public class ListadoPalabrasVacias
{	
	
	private static List<string> PalabrasVacias = new List<string>();//listado de palabras vacías
	
	//Crea el listado de las palabras vacías
	public static List<string> CreaListado()
	{ 
		List<string> archivos;//nombre de los archivos en los que se va a buscar
		List<string> listadotemp;//listado temporal de las palabras de cada archivo
		Dictionary<string, int> ContadorPalabras = new();//diccionario para almacenar las palabras y las veces que aparece cada una
		long TotalPalabras = 0;//Total de palabras diferentes encontradas en los documentos
		
		
		
		PalabrasVacias.AddRange(Utiles.Articulos);//adicionamos los artículos
		PalabrasVacias.AddRange(Utiles.Preposiciones);//adicionamos las preposiciones
		PalabrasVacias.AddRange(Utiles.Conjunciones);//adicionamos las conjunciones
		PalabrasVacias.AddRange(Utiles.Pronombres);//adicionamos los pronombres
		PalabrasVacias.AddRange(Utiles.Adverbios);//adicionamos los adverbios
		
		archivos = Utiles.ListaArchivos(Utiles.CaminoDocumentos);//creamos el listado de los documentos
		
		for (int i =0; i< archivos.Count(); i++) {//para cada uno de los documentos
			listadotemp = CargarArchivo(archivos[i]);//cargamos el contenido al listado temporal
			TotalPalabras = TotalPalabras + listadotemp.Count();//incrementamos el total de las palabras
			foreach(string palabra in listadotemp ) {//para cada una de las palabras
				if (ContadorPalabras.ContainsKey(palabra))//si ya esta dentro del diccionario
                	ContadorPalabras[palabra]++;//incrementamos la cantidad de veces que aparece
				else {//si no lo esta
					ContadorPalabras.Add(palabra, 1); //se adiciona al diccionario
				}
			}

		}	
			
		
		var ContadorPalabrasOrdenado = ContadorPalabras.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value);//ordenamos las palabras de menor a mayor de acuerdo con la cantidad de veces que aparece
		
		
		//Para seleccionar las palabras vacías buscamos la mayor cantidad posible de palabras que aparezcan en los documentos y que la suma 
		//de sus apariciones sea menor que la suma de las palabras que más aparecen. Las palabras que no esten dentro de estas que serán también las
		//que más veces aparecen serán las palabras vacías
		string[] palabras = ContadorPalabrasOrdenado.Keys.ToArray();//creamos un arreglo con las palabras
		int[] cantidad = ContadorPalabrasOrdenado.Values.ToArray();//creamos un arreglo con las veces que aparece cada palabra

		
		long contador = cantidad[0];//inicializamos el contador a la cantidad de veces que aparece la palabar que menos aparece
		int posicion = 0;//la posición donde termina el preceso la iniciamos en 0
		while(contador < (TotalPalabras-contador)){//mientras el valor de contador sea menor que la diferencia entre el total de palabras y el contador
			posicion++;//pasamos a la siguente palabra
			contador = contador + cantidad[posicion];//incrementamos el contador en la cantidad de apariciones de la palabra
		}
		//en posición tenemos el indice del arreglo a partir del cual estan las palabras que más aparecen en los documentos
		for (int i = posicion; i<ContadorPalabrasOrdenado.Count()-1; i++){
			if (!PalabrasVacias.Contains(palabras[i]))//si la palabra no esta en la lista
                PalabrasVacias.Add(palabras[i]);//se adiciona la palabra a las palabras vacías
		}
		
		
		return PalabrasVacias;//retornamos el listado de palabras vacías
	}
	//Carga a memoria el contenido de un documento
	//Entrada: Camino en disco del documento
	private static List<string> CargarArchivo(string camino)
	{	List<string> resultado = new List<string>();//creacion listado de palabras
	 	
	 	try
        {
	 		if (File.Exists(camino))//si el documento existe
        	{
	 			resultado = Utiles.SeparaPalabras(File.ReadAllText(camino));//leemeos el contenido del documento y lo separamos en palabras
			}
	 					
		}
        catch (Exception e)//procesamos excepción en caso de que ocurra
        {
            Console.WriteLine("Error: {0}", e.ToString());
        }
        finally {}
	 	return resultado;//regresamos listado de las palabras
	}
	
}
