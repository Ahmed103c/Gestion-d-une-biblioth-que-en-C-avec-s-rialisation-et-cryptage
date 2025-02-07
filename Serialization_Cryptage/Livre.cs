using Microsoft.VisualBasic;
using System;


public class Livre
{ 
	public string Titre		{ get; set; }
	public string Auteur { get; set; }
	public DateFormat DateDePublication { get; set; }
	public int ISBN { get; set; }
	public Categorie Categorie { get; set; }
	public DateFormat DateAjout { get; set; }

	public Livre()
	{

	}
}
