// See https://aka.ms/new-console-template for more information
using System.Globalization;

Console.WriteLine("Bienvenue sur la bibliothèque Enligne");
//Utilisateur u1 = new Utilisateur
//{
//    Nom = "Ahmed",
//    Prenom = "Rzeigui",
//    Email = "ahmedRzeigui@gmail.com",
//    DateInscription = DateTime.ParseExact("10/02/2025", "dd/MM/yyyy", CultureInfo.InvariantCulture)
//};

//// Affichage des informations
//Console.WriteLine($"Nom : {u1.Nom}");
//Console.WriteLine($"Prénom : {u1.Prenom}");
//Console.WriteLine($"Email : {u1.Email}");
//Console.WriteLine($"Date d'inscription : {u1.DateInscription:dd/MM/yyyy}");

string cheminFichier = "C:\\Users\\Rzeigui Ahmed\\Documents\\CS\\Gestion-d-une-biblioth-que-en-C-avec-s-rialisation-et-cryptage\\GestionBibliothequeRzeiguiAhmed\\Livres.csv";

//if (File.Exists(cheminFichier))
//{
//    Console.WriteLine("Le fichier existe !");
//    Console.WriteLine("Contenu du fichier :");
//    Console.WriteLine(File.ReadAllText(cheminFichier));
//}
//else
//{
//    Console.WriteLine("Fichier introuvable !");
//}

// Liste qui contiendra tous les livres
List<Livre> livres = new List<Livre>();



// Lecture ligne par ligne
try
{
    using (StreamReader reader = new StreamReader(cheminFichier))
    {
        string ligne;
        bool isHeader = true; // Pour ignorer la première ligne (en-têtes)

        while ((ligne = reader.ReadLine()) != null)
        {

            if (isHeader)
            {
                isHeader = false;
                continue; // Ignorer l'en-tête
            }

            // Séparer les colonnes
            string[] colonnes = ligne.Split(';');

            if (colonnes.Length != 6)
            {
                Console.WriteLine($"Ligne incorrecte : {ligne}");
                continue;
            }

            // Extraction des champs
            string titre = colonnes[0];
            string auteur = colonnes[1];
            DateTime dateDePublication = DateTime.ParseExact(colonnes[2], "dd/MM/yyyy", CultureInfo.InvariantCulture);
            long isbn = long.Parse(colonnes[3]);
            string categorieNom = colonnes[4];
            DateTime dateAjout = DateTime.ParseExact(colonnes[5], "dd/MM/yyyy", CultureInfo.InvariantCulture);

            // Création de l'objet `Categorie`
            Categorie categorie = new Categorie { Nom = categorieNom };

            // Création de l'objet `Livre`
            Livre livre = new Livre
            {
                Titre = titre,
                Auteur = auteur,
                DateDePublication = dateDePublication,
                ISBN = isbn,
                Categorie = categorie,
                DateAjout = dateAjout
            };

            // Ajout du livre à la liste
            livres.Add(livre);
        }
    }

    // Affichage des livres pour vérification
    foreach (var livre in livres)
    {
        Console.WriteLine($"Titre: {livre.Titre}, Auteur: {livre.Auteur}, ISBN: {livre.ISBN}, Categorie: {livre.Categorie.Nom}, Date d'ajout: {livre.DateAjout:dd/MM/yyyy}");
    }
}
catch (Exception ex)
{
    Console.WriteLine($"Erreur : {ex.Message}");
}
    