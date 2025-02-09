﻿// See https://aka.ms/new-console-template for more information
using System.Globalization;

Console.WriteLine("Bienvenue sur la bibliothèque Enligne");

/***********************************************************************************************************
 * 
 * 
 * 
 * 
 *              Gestion des livres de la bibliothèques
 * 
 * 
 * 
 * 
 * 
 * 
 * **********************************************************************************************************/
string cheminFichierCSV = "C:\\Users\\Rzeigui Ahmed\\Documents\\CS\\Gestion-d-une-biblioth-que-en-C-avec-s-rialisation-et-cryptage\\GestionBibliothequeRzeiguiAhmed\\Livres.csv";
// livres de la bibliothèques :
List<Livre> livres = new List<Livre>();
// charger les livres par fichier.csv
try
{
    using (StreamReader reader = new StreamReader(cheminFichierCSV))
    {
        string ligne;
        bool isHeader = true; // Pour ignorer la première ligne (en-têtes)

        while ((ligne = reader.ReadLine()) != null)
        {

            if (isHeader)
            {
                isHeader = false;
                continue; 
            }

            string[] colonnes = ligne.Split(';');

            if (colonnes.Length != 6)
            {
                Console.WriteLine($"Ligne incorrecte : {ligne}");
                continue;
            }

            string titre = colonnes[0];
            string auteur = colonnes[1];
            DateTime dateDePublication = DateTime.ParseExact(colonnes[2], "dd/MM/yyyy", CultureInfo.InvariantCulture);
            long isbn = long.Parse(colonnes[3]);
            string categorieNom = colonnes[4];
            DateTime dateAjout = DateTime.ParseExact(colonnes[5], "dd/MM/yyyy", CultureInfo.InvariantCulture);

            Categorie categorie = new Categorie { Nom = categorieNom };

            Livre livre = new Livre
            {
                Titre = titre,
                Auteur = auteur,
                DateDePublication = dateDePublication,
                ISBN = isbn,
                Categorie = categorie,
                DateAjout = dateAjout
            };
            livres.Add(livre);
        }
    }
}
catch (Exception ex)
{
    Console.WriteLine($"Erreur : {ex.Message}");
}
void afficherLivresBibliothèques() {
    foreach (var livre in livres)
    {
        Console.WriteLine($"Titre: {livre.Titre}, Auteur: {livre.Auteur}, ISBN: {livre.ISBN}, Categorie: {livre.Categorie.Nom}, Date d'ajout: {livre.DateAjout:dd/MM/yyyy}");
    }
}
int chercherIndexLivre(string titreLivre)
{
    int i = 0;
    while (livres[i].Titre!=titreLivre)
    {
        i++;
    }
    return i;
}
/***********************************************************************************************************
 * 
 * 
 * 
 * 
 *               Utilisateur 
 * 
 * 
 * 
 * 
 * 
 * 
 * **********************************************************************************************************/


List<Utilisateur> Utilisateurs = new List<Utilisateur>();

int chercherIndexUtilisateur(string nomUtilisateur,string prenomUtilisateur)
{
    int i = 0;
    while ((Utilisateurs[i].Nom != nomUtilisateur) && (Utilisateurs[i].Prenom!=prenomUtilisateur))
    {
        i++;
    }
    return i;
}

/***********************************************************************************************************
 * 
 * 
 * 
 * 
 *                Sérialization  et Désérialization
 * 
 * 
 * 
 * 
 * 
 * 
 * **********************************************************************************************************/





string chemin = "C:\\Users\\Rzeigui Ahmed\\Documents\\CS\\Gestion-d-une-biblioth-que-en-C-avec-s-rialisation-et-cryptage\\GestionBibliothequeRzeiguiAhmed\\Bibliotheque";

void deserialiser(string nomUtilisateur,string prenomUtilisateur,TypeSerialization typeSerialization)
{
    try
    {
        switch (typeSerialization)
        {
            case TypeSerialization.binaire:
                string cheminbjson = $"C:\\Users\\Rzeigui Ahmed\\Documents\\CS\\Gestion-d-une-biblioth-que-en-C-avec-s-rialisation-et-cryptage\\GestionBibliothequeRzeiguiAhmed\\Bibliotheque\\Utilisateur_{nomUtilisateur}_{prenomUtilisateur}.bjson";
                if (!File.Exists(cheminbjson))
                {
                    throw new FileNotFoundException($"le fichier bjson n'existe pas dans ce chemin : {cheminbjson} ");
                }
                Utilisateur utilisateurBjson = (Utilisateur)SerializationFactory.Charger(null, cheminbjson, typeof(Utilisateur), TypeSerialization.binaire);
                utilisateurBjson.afficherUtilisateur();
                break;
            case TypeSerialization.XML:
                string chemin1 = $"C:\\Users\\Rzeigui Ahmed\\Documents\\CS\\Gestion-d-une-biblioth-que-en-C-avec-s-rialisation-et-cryptage\\GestionBibliothequeRzeiguiAhmed\\Bibliotheque\\Utilisateur_{nomUtilisateur}_{prenomUtilisateur}.xml";
                if (!File.Exists(chemin1))
                {
                    throw new FileNotFoundException($"Le fichier XML n'existe pas : {chemin1}");
                }
                string chemin2 = $"C:\\Users\\Rzeigui Ahmed\\Documents\\CS\\Gestion-d-une-biblioth-que-en-C-avec-s-rialisation-et-cryptage\\GestionBibliothequeRzeiguiAhmed\\Bibliotheque\\Utilisateur_{nomUtilisateur}_{prenomUtilisateur}.hash";
                if (!File.Exists(chemin2))
                {
                    throw new FileNotFoundException($"Le fichier hash n'existe pas : {chemin2}");
                }
                Utilisateur utilisateurXml = (Utilisateur)SerializationFactory.Charger(chemin2, chemin1, typeof(Utilisateur), TypeSerialization.XML);
                utilisateurXml.afficherUtilisateur();
                break;
        }
    }
    catch (FileNotFoundException ex)
    {
        Console.WriteLine($"Erreur : {ex.Message}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Une erreur inattendue s'est produite : {ex.Message}");
    }
}



/***********************************************************************************************************
 * 
 * 
 * 
 * 
 *              Interaction avec Utilisateur 
 * 
 * 
 * 
 * 
 * 
 * 
 * **********************************************************************************************************/





Console.WriteLine("Pour affichier la liste des livres taper 1 \n" +
                  "Pour créer un nouveau Utilisateur  taper 2 \n" +
                  "Pour affiher les donnees d'un Utilisateur taper 3 \n"+
                  "Pour affiher tous les Utilisateurs taper 5 \n" +
                  "Pour arreter l'application taper 4 \n");
bool run = true;
while (run)
{
    string demandeUser = Console.ReadLine();
    switch (demandeUser)
    {
        case "1":
            /************************************************************************************
             * 
             * 
             *                              Accés au livres disponibles 
             * 
             * 
             * **********************************************************************************/
            afficherLivresBibliothèques();
            break;

        case "2":
            /************************************************************************************
             * 
             * 
             *                              Creation Utilisateurs 
             * 
             * 
             * **********************************************************************************/
            Console.WriteLine("Insciption d'un Nouveau Utilisateur :) \n");
            Console.WriteLine("Entrer le nom de l'utilisateur : ");
            string nom = Console.ReadLine();
            Console.WriteLine("Entrer le prenom de l'utilisateur : \n");
            string prenom = Console.ReadLine();
            Console.WriteLine("Entrer l'email de l'utilisateur : \n");
            string email = Console.ReadLine();
            Console.WriteLine("Entrer dateInscri de l'utilisateur : \n");
            DateTime dateinscription = DateTime.ParseExact(Console.ReadLine(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
            Console.WriteLine("Entrer le nom du livre a emprunté shouaité \n");
            string nomLivre = Console.ReadLine();
            Utilisateur newUtilisateur = new Utilisateur(nom, prenom, email, dateinscription, livres[chercherIndexLivre(nomLivre)]);
            Utilisateurs.Add(newUtilisateur);

            /************************************************************************************
             * 
             * 
             *                              Sérialization  
             * 
             * 
             * **********************************************************************************/
            Console.WriteLine("Sauvegarde Utilisateur : Choisisser type de sauvegarde de l'utilisateur : \n" +
                              "0 --- Pour ---- Serialisation Binaire (.bjoson) \n" +
                              "1 --- Pour ---- Serialisation XML (.xml) \n");
            int typeserialistion = int.Parse(Console.ReadLine());
            Console.WriteLine("Entrer un mdp de sauvegarde pour l'utilisateur : ");
            string mdp=Console.ReadLine();
            switch (typeserialistion)
            {
                case 0:
                    SerializationFactory.sauvegarder(chemin, TypeSerialization.binaire, newUtilisateur, mdp);
                    break;
                case 1:
                    SerializationFactory.sauvegarder(chemin, TypeSerialization.XML, newUtilisateur, mdp);
                    break;
                default:
                    Console.WriteLine("typeSerialisation incorrect");
                    break;
            }
            break;

        case "3":
            /************************************************************************************
             * 
             * 
             *                              Désérialization 
             * 
             * 
             * **********************************************************************************/
            Console.WriteLine("Acces Utilisateur : Choisisser type d'acces de l'utilisateur : \n" +
                                         "0 --- Pour ---- Serialisation Binaire (.bjoson) \n" +
                                         "1 --- Pour ---- Serialisation XML (.xml) \n");
            int typeserialistion2 = int.Parse(Console.ReadLine());
            Console.WriteLine("Donner nom utilisateur");
            string nom_des=Console.ReadLine();
            Console.WriteLine("Donner prenom utilisateur");
            string preonm_des = Console.ReadLine();
            switch (typeserialistion2)
            {
                case 0:
                    deserialiser(nom_des,preonm_des, TypeSerialization.binaire);
                    break;
                case 1:
                    deserialiser(nom_des, preonm_des, TypeSerialization.XML);
                    break;
                default:
                    Console.WriteLine("typeSerialisation incorrect");
                    break;
            }
            break;

        case "4":
            Console.WriteLine("Toute l'équipe vous exprime sa gratitude pour l'utilisation de notre bibliothèque");
            run = false;
            break;

        case "5":
            if (Utilisateurs.Count != 0)
            {
                foreach (var user in Utilisateurs)
                {
                    user.afficherUtilisateur();
                }
            }
            else
            {
                Console.WriteLine("Aucun Utilisateur est connecté");
            }
            break;
        default:
            break;
    }

}

