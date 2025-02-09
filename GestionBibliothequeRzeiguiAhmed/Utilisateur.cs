using Microsoft.VisualBasic;
using System;
using System.Security.AccessControl;


public class Utilisateur
{
	public string Nom {  get; set; }
	public string Prenom { get; set; }
	public string Email { get; set; }
	public DateTime DateInscription { get; set; }

	public	List<Livre> LivresEmpruntes { get; set; }=new List<Livre>();

	public Utilisateur() { }
    public Utilisateur(string nom_input,string prenom_input,string email_input,DateTime dateinscription_input,Livre livre_input)
    {
		Nom= nom_input;
		Prenom=prenom_input;
		Email=email_input;
		DateInscription=dateinscription_input;
		LivresEmpruntes?.Add(livre_input);	

    }
	public void afficherUtilisateur()
	{
		Console.WriteLine("Info Utilisateur : " + "Nom :" + Nom + "Prenom : " + Prenom + "\n" +
						  "Email : " + Email + "\n" +
						  "Date Inscription : " + DateInscription + "\n"+
						  "Livres impruntes : "+"\n");
		foreach (var livre in LivresEmpruntes) 
		{
			Console.WriteLine("livre : "+livre.Titre + "\n");	
		}
	}

}
