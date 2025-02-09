// See https://aka.ms/new-console-template for more information
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
 *               Serialisation  
 * 
 * 
 * 
 * 
 * 
 * 
 * **********************************************************************************************************/





string chemin = "C:\\Users\\Rzeigui Ahmed\\Documents\\CS\\Gestion-d-une-biblioth-que-en-C-avec-s-rialisation-et-cryptage\\GestionBibliothequeRzeiguiAhmed\\Bibliotheque";





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
            afficherLivresBibliothèques();
            break;



        case "2":
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

            Console.WriteLine("Sauvegarde Utilisateur : Choisisser type de sauvegarde de l'utilisateur : \n" +
                              "0 --- Pour ---- Serialisation Binaire (.bjoson) \n" +
                              "1 --- Pour ---- Serialisation XML (.xml) \n");
            int typeserialistion = int.Parse(Console.ReadLine());
            switch (typeserialistion)
            {
                case 0:

                    SerializationFactory.sauvegarder(chemin, TypeSerialization.binaire, newUtilisateur, "1111");
                    string cheminbjson = $"C:\\Users\\Rzeigui Ahmed\\Documents\\CS\\Gestion-d-une-biblioth-que-en-C-avec-s-rialisation-et-cryptage\\GestionBibliothequeRzeiguiAhmed\\Bibliotheque\\Utilisateur_{newUtilisateur.Nom}_{newUtilisateur.Prenom}.bjson";
                    Utilisateur utilisateurBjson = (Utilisateur)SerializationFactory.Charger(null, cheminbjson, typeof(Utilisateur), TypeSerialization.binaire);
                    utilisateurBjson.afficherUtilisateur();
                    break;
                case 1:
                    SerializationFactory.sauvegarder(chemin, TypeSerialization.XML, newUtilisateur, "1111");
                    string chemin1 = $"C:\\Users\\Rzeigui Ahmed\\Documents\\CS\\Gestion-d-une-biblioth-que-en-C-avec-s-rialisation-et-cryptage\\GestionBibliothequeRzeiguiAhmed\\Bibliotheque\\Utilisateur_{newUtilisateur.Nom}_{newUtilisateur.Prenom}.xml";
                    string chemin2 = $"C:\\Users\\Rzeigui Ahmed\\Documents\\CS\\Gestion-d-une-biblioth-que-en-C-avec-s-rialisation-et-cryptage\\GestionBibliothequeRzeiguiAhmed\\Bibliotheque\\Utilisateur_{newUtilisateur.Nom}_{newUtilisateur.Prenom}.hash";
                    Utilisateur utilisateurXml = (Utilisateur)SerializationFactory.Charger(chemin2, chemin1, typeof(Utilisateur), TypeSerialization.XML);
                    utilisateurXml.afficherUtilisateur();
                    break;
                default:
                    Console.WriteLine("typeSerialisation incorrect");
                    break;
            }
            //test Serialisation binaire : 
            //SerializationFactory.sauvegarder(chemin, TypeSerialization.binaire, newUtilisateur, "1111");
            //string chemin1 = $"C:\\Users\\Rzeigui Ahmed\\Documents\\CS\\Gestion-d-une-biblioth-que-en-C-avec-s-rialisation-et-cryptage\\GestionBibliothequeRzeiguiAhmed\\Bibliotheque\\Utilisateur_{newUtilisateur.Nom}_{newUtilisateur.Prenom}.bjson";
            //Utilisateur utilisateur = (Utilisateur)SerializationFactory.Charger(null, chemin1, typeof(Utilisateur), TypeSerialization.binaire);
            //utilisateur.afficherUtilisateur();

            //test Serialisation xml : 
            //SerializationFactory.sauvegarder(chemin, TypeSerialization.XML, newUtilisateur, "1111");
            //string chemin1 = $"C:\\Users\\Rzeigui Ahmed\\Documents\\CS\\Gestion-d-une-biblioth-que-en-C-avec-s-rialisation-et-cryptage\\GestionBibliothequeRzeiguiAhmed\\Bibliotheque\\Utilisateur_{newUtilisateur.Nom}_{newUtilisateur.Prenom}.xml";
            //string chemin2 = $"C:\\Users\\Rzeigui Ahmed\\Documents\\CS\\Gestion-d-une-biblioth-que-en-C-avec-s-rialisation-et-cryptage\\GestionBibliothequeRzeiguiAhmed\\Bibliotheque\\Utilisateur_{newUtilisateur.Nom}_{newUtilisateur.Prenom}.hash";
            //Utilisateur utilisateur = (Utilisateur)SerializationFactory.Charger(chemin2, chemin1, typeof(Utilisateur), TypeSerialization.XML);
            //utilisateur.afficherUtilisateur();



            break;




        case "3":
            Console.WriteLine("Donner nom de l'utilisateur");
            string nom_input = Console.ReadLine();
            Console.WriteLine("Donner prenom de l'utilisateur");
            string prenom_input = Console.ReadLine();
            Utilisateurs[chercherIndexUtilisateur(nom_input, prenom_input)].afficherUtilisateur();
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

