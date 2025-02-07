using Microsoft.VisualBasic;
using System;
using static System.Net.Mime.MediaTypeNames;

[Serializable]
public class Utilisateur
{
	public string Nom {  get; set; }
	public string Prenom { get; set; }
	public string Email { get; set; }
	public DateFormat DateInscription { get; set; }

	public	List<Livre> LivresEmpruntes { get; set; }

	public Utilisateur()
	{

	}
    public static void main(string[] args)
    {
		Console.WriteLine("hello from Utilisateur");
    }
}

