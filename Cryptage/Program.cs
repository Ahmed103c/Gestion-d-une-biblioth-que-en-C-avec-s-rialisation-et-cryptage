// See https://aka.ms/new-console-template for more information

Console.WriteLine("Hello, World! Cryptage");




string chemin = "C:\\Users\\Rzeigui Ahmed\\Documents\\CS\\Gestion-d-une-biblioth-que-en-C-avec-s-rialisation-et-cryptage\\Cryptage\\Bibliotheque";
Utilisateur u3 = new Utilisateur("Amine");
CryptageFactory.sauvegarder(chemin, TypeSerialization.binaire, u3);


string chemin2 = "C:\\Users\\Rzeigui Ahmed\\Documents\\CS\\Gestion-d-une-biblioth-que-en-C-avec-s-rialisation-et-cryptage\\Cryptage\\Bibliotheque\\Utilisateur_Amine.bjson";
Utilisateur utilisateur2 = (Utilisateur)CryptageFactory.Charger(chemin2, typeof(Utilisateur), TypeSerialization.binaire);
Console.WriteLine($"Nom : {utilisateur2.Nom}");