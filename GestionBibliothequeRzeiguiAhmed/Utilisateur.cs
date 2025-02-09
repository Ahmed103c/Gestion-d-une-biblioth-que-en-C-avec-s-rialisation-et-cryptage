using Microsoft.VisualBasic;
using System;


public class Utilisateur
{
	public string Nom {  get; set; }
	public string Prenom { get; set; }
	public string Email { get; set; }
	public DateTime DateInscription { get; set; }

	public	List<Livre> LivresEmpruntes { get; set; }

	public Utilisateur()
	{

	}
}
