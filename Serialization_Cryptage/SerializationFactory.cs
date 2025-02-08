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



            chemin = Path.Combine(chemin, $"{obj.GetType().Name}_{obj.GetType().GetProperty("Nom").GetValue(obj)?.ToString()}.bjson");
            using (FileStream fileStream = new FileStream(chemin, FileMode.Create))
            using (GZipStream compressionStream = new GZipStream(fileStream, CompressionMode.Compress))
            {
                compressionStream.Write(hashBytes, 0, hashBytes.Length);
                compressionStream.Write(jsonBytes, 0, jsonBytes.Length);
            }

            Console.WriteLine($"Données sérialisées et compressées dans : {chemin}");
        }
        else
        {
            XmlSerializer serializer = new XmlSerializer(obj.GetType());

            chemin = Path.Combine(chemin, $"{obj.GetType().Name}.xml");
            using (FileStream fileStream = new FileStream(chemin, FileMode.Create))
            {
                serializer.Serialize(fileStream, obj);
            }

            Console.WriteLine($"Données sérialisées en XML dans : {chemin}");
        }
    }
    public static object Charger(string chemin, Type typeObjet, TypeSerialization typeserialization)
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
                        Console.WriteLine($"Données désérialisées depuis : {chemin}");
                        return obj;
                    }
                    else
                    {
                        tentatives--;
                        Console.WriteLine($"Mot de passe incorrect. Il vous reste {tentatives} tentatives.");
                    }
                }
            }
            if (tentatives == 0)
            {
                Console.WriteLine("3 tentatives échouées. Le fichier sera supprimé pour garantir la sécurité.");
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

            XmlSerializer serializer = new XmlSerializer(typeObjet);

            using (FileStream fileStream = new FileStream(chemin, FileMode.Open))
            {
                object obj = serializer.Deserialize(fileStream);
                Console.WriteLine($"Données désérialisées depuis : {chemin}");
                return obj;
            }
        }
    }
}
