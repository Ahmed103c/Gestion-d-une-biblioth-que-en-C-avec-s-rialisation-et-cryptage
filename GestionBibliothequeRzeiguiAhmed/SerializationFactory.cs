using System;

using System;
using System.IO;
using System.Text.Json;
using System.IO.Compression;
using System.Xml.Serialization;
using System.Net.NetworkInformation;
using System.Text;
using System.Runtime.CompilerServices;

public enum TypeSerialization
{
	binaire,
	XML 
}


public class SerializationFactory
{
    public static void sauvegarder(string chemin, TypeSerialization typeserialization, object obj, string motDePasse)
    {

        if (typeserialization == TypeSerialization.binaire)
        {
            string json = JsonSerializer.Serialize(obj);
            byte[] jsonBytes = System.Text.Encoding.UTF8.GetBytes(json);


            string hashMotDePasse = Convert.ToBase64String(System.Security.Cryptography.SHA256.HashData(Encoding.UTF8.GetBytes(motDePasse)));
            byte[] hashBytes = Encoding.UTF8.GetBytes(hashMotDePasse);



            chemin = Path.Combine(chemin, $"{obj.GetType().Name}_{obj.GetType().GetProperty("Nom").GetValue(obj)?.ToString()}_{obj.GetType().GetProperty("Prenom").GetValue(obj)?.ToString()}.bjson");
            using (FileStream fileStream = new FileStream(chemin, FileMode.Create))
            using (GZipStream compressionStream = new GZipStream(fileStream, CompressionMode.Compress))
            {
                compressionStream.Write(hashBytes, 0, hashBytes.Length);
                compressionStream.Write(jsonBytes, 0, jsonBytes.Length);
            }

            Console.WriteLine($"{Couleur.CYN}Données sérialisées et compressées dans : {chemin}{Couleur.NOR}");
        }
        else
        {
            XmlSerializer serializer = new XmlSerializer(obj.GetType());
            string chemin1 = Path.Combine(chemin, $"{obj.GetType().Name}_{obj.GetType().GetProperty("Nom").GetValue(obj)?.ToString()}_{obj.GetType().GetProperty("Prenom").GetValue(obj)?.ToString()}.xml");

            using (FileStream fileStream = new FileStream(chemin1, FileMode.Create))
            {
                serializer.Serialize(fileStream, obj);
            }


            Console.WriteLine($"{Couleur.CYN}Données sérialisées en XML dans : {chemin1}{Couleur.NOR}");

            Console.WriteLine("Entrer un mot de passe pour protéger ce fichier XML :");

            // Générer un hash du mot de passe
            string passwordHash = Convert.ToBase64String(System.Security.Cryptography.SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(motDePasse)));

            // Sauvegarder le hash dans un fichier associé
            string hashFilePath = Path.Combine(chemin, $"{obj.GetType().Name}_{obj.GetType().GetProperty("Nom").GetValue(obj)?.ToString()}_{obj.GetType().GetProperty("Prenom").GetValue(obj)?.ToString()}.hash"); ;
            File.WriteAllText(hashFilePath, passwordHash);

            Console.WriteLine($"{Couleur.BLU}Mot de passe hashé et sauvegardé dans : {hashFilePath}{Couleur.NOR}");
        }
    }

    public static object Charger(string chemin2, string chemin, Type typeObjet, TypeSerialization typeserialization)
    {
        if (typeserialization == TypeSerialization.binaire)
        {

            if (!File.Exists(chemin))
            {
                throw new FileNotFoundException($"Fichier introuvable : {chemin}");
            }
            int tentatives = 3;

            while (tentatives > 0)
            {
                Console.WriteLine("Entrer Mot de passe");
                string mdp = Console.ReadLine();
                string hashMdp = Convert.ToBase64String(System.Security.Cryptography.SHA256.HashData(Encoding.UTF8.GetBytes(mdp)));


                using (FileStream fileStream = new FileStream(chemin, FileMode.Open))
                using (GZipStream decompressionStream = new GZipStream(fileStream, CompressionMode.Decompress))
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    // Lire le hash du fichier
                    byte[] hashBytes = new byte[44]; // Taille d'un hash SHA256 encodé en Base64
                    decompressionStream.Read(hashBytes, 0, hashBytes.Length);
                    string hashFichier = Encoding.UTF8.GetString(hashBytes);

                    if (hashMdp == hashFichier)
                    {

                        decompressionStream.CopyTo(memoryStream);
                        byte[] decompressedBytes = memoryStream.ToArray();
                        string json = System.Text.Encoding.UTF8.GetString(decompressedBytes);


                        object obj = JsonSerializer.Deserialize(json, typeObjet);
                        Console.WriteLine($"{Couleur.GRE}Données désérialisées depuis : {chemin}{Couleur.NOR}");
                        return obj;
                    }
                    else
                    {
                        tentatives--;
                        Console.WriteLine($"{Couleur.RED}Mot de passe incorrect. Il vous reste {tentatives} tentatives.{Couleur.NOR}");
                    }
                }
            }
            if (tentatives == 0)
            {
                Console.WriteLine($"{Couleur.RED}3 tentatives échouées. Le fichier sera supprimé pour garantir la sécurité.{Couleur.NOR}");
                File.Delete(chemin);
                throw new UnauthorizedAccessException("Accès refusé. Fichier supprimé.");
            }
            throw new InvalidOperationException("Impossible de charger le fichier. Vérifiez le mot de passe.");
        }
        else
        {
            if (!File.Exists(chemin))
            {
                throw new FileNotFoundException($"Fichier introuvable : {chemin}");
            }
            // Vérifiez si un fichier de hash existe
            string cheminHash = chemin2;
            if (!File.Exists(cheminHash))
            {
                throw new FileNotFoundException($"Fichier de hash introuvable : {cheminHash}");
            }

            // Lire le hash depuis le fichier
            string hashFichier;
            using (StreamReader reader = new StreamReader(cheminHash))
            {
                hashFichier = reader.ReadLine();
            }

            if (string.IsNullOrEmpty(hashFichier))
            {
                throw new InvalidOperationException("Fichier de hash corrompu ou vide.");
            }

            // Demander le mot de passe et vérifier le hash
            int tentatives = 3;
            while (tentatives > 0)
            {
                Console.WriteLine("Entrer le mot de passe :");
                string mdp = Console.ReadLine();

                // Générer le hash du mot de passe entré
                string hashMdp = Convert.ToBase64String(System.Security.Cryptography.SHA256.HashData(Encoding.UTF8.GetBytes(mdp)));

                // Comparer le hash calculé avec celui du fichier
                if (hashMdp == hashFichier)
                {
                    // Si le mot de passe est correct, désérialiser le fichier XML
                    XmlSerializer serializer = new XmlSerializer(typeObjet);
                    using (FileStream fileStream = new FileStream(chemin, FileMode.Open))
                    {
                        object obj = serializer.Deserialize(fileStream);
                        Console.WriteLine($"{Couleur.GRE}Données désérialisées depuis : {chemin}{Couleur.NOR}");
                        return obj;
                    }
                }
                else
                {
                    tentatives--;
                    Console.WriteLine($"{Couleur.RED}Mot de passe incorrect. Il vous reste {tentatives} tentatives.{Couleur.NOR}");
                }
            }

            // Si les tentatives sont épuisées, supprimer le fichier pour des raisons de sécurité
            if (tentatives == 0)
            {
                Console.WriteLine($"{Couleur.RED}3 tentatives échouées. Le fichier sera supprimé pour garantir la sécurité.{Couleur.NOR}");
                File.Delete(chemin);
                File.Delete(cheminHash); // Supprime également le fichier de hash
                throw new UnauthorizedAccessException("Accès refusé. Fichier supprimé.");
            }

            throw new InvalidOperationException("Impossible de charger le fichier. Vérifiez le mot de passe.");
        }
    }

    public static void Modifier(string chemin2, string chemin, object obj, TypeSerialization typeserialization)
    {
        if (typeserialization == TypeSerialization.binaire)
        {
            if (typeserialization == TypeSerialization.binaire)
            {
                if (!File.Exists(chemin))
                {
                    throw new FileNotFoundException($"Fichier introuvable : {chemin}");
                }

                bool a = false;
                int tentatives = 3;
                string mdp = "";

                while (tentatives > 0)
                {
                    Console.WriteLine("Entrer Mot de passe");
                    mdp = Console.ReadLine();
                    string hashMdp = Convert.ToBase64String(System.Security.Cryptography.SHA256.HashData(Encoding.UTF8.GetBytes(mdp)));

                    // Ouvrir le fichier en mode lecture et décompression
                    using (FileStream fileStream = new FileStream(chemin, FileMode.Open, FileAccess.ReadWrite, FileShare.None)) // Empêche l'accès concurrent
                    using (GZipStream decompressionStream = new GZipStream(fileStream, CompressionMode.Decompress))
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        // Lire le hash du fichier
                        byte[] hashBytes = new byte[44]; // Taille d'un hash SHA256 encodé en Base64
                        decompressionStream.Read(hashBytes, 0, hashBytes.Length);
                        string hashFichier = Encoding.UTF8.GetString(hashBytes);

                        if (hashMdp == hashFichier)
                        {
                            a = true;
                            break;
                        }
                        else
                        {
                            tentatives--;
                            Console.WriteLine($"{Couleur.RED}Mot de passe incorrect. Il vous reste {tentatives} tentatives.{Couleur.NOR}");
                            
                        }
                    }

                    if (tentatives == 0)
                    {
                        Console.WriteLine($"{Couleur.RED}3 tentatives échouées. Le fichier sera supprimé pour garantir la sécurité.{Couleur.NOR}");
                        File.Delete(chemin);
                        throw new UnauthorizedAccessException("Accès refusé. Fichier supprimé.");
                    }
                }
                if (a)
                {
                    try
                    {
                        string cheminx = "C:\\Users\\Rzeigui Ahmed\\Documents\\CS\\Gestion-d-une-biblioth-que-en-C-avec-s-rialisation-et-cryptage\\GestionBibliothequeRzeiguiAhmed\\Bibliotheque";
                        File.Delete(chemin);
                        sauvegarder(cheminx, typeserialization, obj, mdp);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Erreur lors de la suppression du fichier : {ex.Message}");
                        return;
                    }
                }


            }
        }
        else
        {
            if (!File.Exists(chemin))
            {
                throw new FileNotFoundException($"Fichier introuvable : {chemin}");
            }
            // Vérifiez si un fichier de hash existe
            string cheminHash = chemin2;
            if (!File.Exists(cheminHash))
            {
                throw new FileNotFoundException($"Fichier de hash introuvable : {cheminHash}");
            }

            // Lire le hash depuis le fichier
            string hashFichier;
            using (StreamReader reader = new StreamReader(cheminHash))
            {
                hashFichier = reader.ReadLine();
            }

            if (string.IsNullOrEmpty(hashFichier))
            {
                throw new InvalidOperationException("Fichier de hash corrompu ou vide.");
            }
            string mdp = "";
            bool a = false;
            // Demander le mot de passe et vérifier le hash
            int tentatives = 3;
            while (tentatives > 0)
            {
                Console.WriteLine("Entrer le mot de passe :");
                mdp = Console.ReadLine();

                // Générer le hash du mot de passe entré
                string hashMdp = Convert.ToBase64String(System.Security.Cryptography.SHA256.HashData(Encoding.UTF8.GetBytes(mdp)));

                // Comparer le hash calculé avec celui du fichier
                if (hashMdp == hashFichier)
                {
                    a = true;
                    break;
                }
                else
                {
                    tentatives--;
                    Console.WriteLine($"{Couleur.RED}Mot de passe incorrect. Il vous reste {tentatives} tentatives.{Couleur.NOR}");
                }
                if (tentatives == 0)
                {
                    Console.WriteLine($"{Couleur.RED}3 tentatives échouées. Le fichier sera supprimé pour garantir la sécurité.{Couleur.NOR}");
                    File.Delete(chemin);
                    throw new UnauthorizedAccessException("Accès refusé. Fichier supprimé.");
                }
            }
            if (a)
            {
                try
                {
                    string cheminx = "C:\\Users\\Rzeigui Ahmed\\Documents\\CS\\Gestion-d-une-biblioth-que-en-C-avec-s-rialisation-et-cryptage\\GestionBibliothequeRzeiguiAhmed\\Bibliotheque";
                    File.Delete(chemin);
                    sauvegarder(cheminx, typeserialization, obj, mdp);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erreur lors de la suppression du fichier : {ex.Message}");
                    return;
                }
            }



        }
    }

}

