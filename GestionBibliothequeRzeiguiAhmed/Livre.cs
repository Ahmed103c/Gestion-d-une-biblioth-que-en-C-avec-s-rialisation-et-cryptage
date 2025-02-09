using Microsoft.VisualBasic;
using System;


public class Livre
{ 
	public string Titre		{ get; set; }
	public string Auteur { get; set; }
	public DateTime DateDePublication { get; set; }
	public long ISBN { get; set; }
	public Categorie Categorie { get; set; }
	public DateTime DateAjout { get; set; }

	public Livre()
	{

	}
}
