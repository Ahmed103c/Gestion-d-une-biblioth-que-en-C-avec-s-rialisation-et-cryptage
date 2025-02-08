using System;
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Xml.Serialization;

public enum TypeSerialization
{
    binaire,
    XML
}

public class CryptageFactory
{
    // Méthode pour générer une clé à partir d'un mot de passe ou du SID de l'utilisateur
    private static byte[] GenerateKey(string key)
    {
        if (string.IsNullOrEmpty(key))
        {
            // Utiliser le SID de l'utilisateur comme clé par défaut
            key = System.Security.Principal.WindowsIdentity.GetCurrent().User.Value;
        }

        using (SHA256 sha256 = SHA256.Create())
        {
            return sha256.ComputeHash(Encoding.UTF8.GetBytes(key));
        }
    }

    // Méthode pour sauvegarder des fichiers avec chiffrement
    public static void sauvegarder(string chemin, TypeSerialization typeSerialization, object obj)
    {
        string key;
        Console.WriteLine("Entrer la clé pour cryptage  à 16 chiffre_et/ou_lettres");
        key=Console.ReadLine();
        byte[] encryptionKey = GenerateKey(key);

        if (typeSerialization == TypeSerialization.binaire)
        {
            string json = JsonSerializer.Serialize(obj);
            byte[] jsonBytes = Encoding.UTF8.GetBytes(json);

            chemin = Path.Combine(chemin, $"{obj.GetType().Name}_{obj.GetType().GetProperty("Nom").GetValue(obj)?.ToString()}.bjson");
            using (FileStream fileStream = new FileStream(chemin, FileMode.Create))
            using (Aes aes = Aes.Create())
            {
                aes.Key = encryptionKey;
                aes.IV = new byte[16]; // Initialiser IV à des zéros (ou générer aléatoirement)
                fileStream.Write(aes.IV, 0, aes.IV.Length); // Écrire l'IV au début du fichier

                using (CryptoStream cryptoStream = new CryptoStream(fileStream, aes.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cryptoStream.Write(jsonBytes, 0, jsonBytes.Length);
                }
            }

            Console.WriteLine($"Données chiffrées et sauvegardées dans : {chemin}");
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

    // Méthode pour charger des fichiers avec déchiffrement
    //public static object Charger(string chemin, Type typeObjet, TypeSerialization typeSerialization)
    //{
    //    string key;
    //    Console.WriteLine("Entrer la clé pour décryptage  à 16 chiffre_et/ou_lettres");
    //    key = Console.ReadLine();
    //    byte[] decryptionKey = GenerateKey(key);

    //    if (typeSerialization == TypeSerialization.binaire)
    //    {
    //        if (!File.Exists(chemin))
    //        {
    //            throw new FileNotFoundException($"Fichier introuvable : {chemin}");
    //        }

    //        using (FileStream fileStream = new FileStream(chemin, FileMode.Open))
    //        using (Aes aes = Aes.Create())
    //        {
    //            byte[] iv = new byte[16];
    //            fileStream.Read(iv, 0, iv.Length); // Lire l'IV depuis le fichier
    //            aes.Key = decryptionKey;
    //            aes.IV = iv;

    //            using (CryptoStream cryptoStream = new CryptoStream(fileStream, aes.CreateDecryptor(), CryptoStreamMode.Read))
    //            using (MemoryStream memoryStream = new MemoryStream())
    //            {
    //                cryptoStream.CopyTo(memoryStream);
    //                byte[] decryptedBytes = memoryStream.ToArray();
    //                string json = Encoding.UTF8.GetString(decryptedBytes);
    //                object obj = JsonSerializer.Deserialize(json, typeObjet);

    //                Console.WriteLine($"Données déchiffrées et chargées depuis : {chemin}");
    //                return obj;
    //            }
    //        }
    //    }
    //    else
    //    {
    //        if (!File.Exists(chemin))
    //        {
    //            throw new FileNotFoundException($"Fichier introuvable : {chemin}");
    //        }

    //        XmlSerializer serializer = new XmlSerializer(typeObjet);

    //        using (FileStream fileStream = new FileStream(chemin, FileMode.Open))
    //        {
    //            object obj = serializer.Deserialize(fileStream);
    //            Console.WriteLine($"Données désérialisées depuis : {chemin}");
    //            return obj;
    //        }
    //    }
    //}

    public static object Charger(string chemin, Type typeObjet, TypeSerialization typeSerialization)
    {
        if (!File.Exists(chemin))
        {
            throw new FileNotFoundException($"Fichier introuvable : {chemin}");
        }

        byte[] decryptionKey = null;
        bool success = false;

        for (int attempt = 1; attempt <= 3; attempt++)
        {
            Console.WriteLine($"Tentative {attempt}/3 - Entrer la clé pour décryptage (16 caractères, lettres ou chiffres) :");
            string key = Console.ReadLine();
            decryptionKey = GenerateKey(key);

            try
            {
                if (typeSerialization == TypeSerialization.binaire)
                {
                    using (FileStream fileStream = new FileStream(chemin, FileMode.Open))
                    using (Aes aes = Aes.Create())
                    {
                        byte[] iv = new byte[16];
                        fileStream.Read(iv, 0, iv.Length); // Lire l'IV depuis le fichier
                        aes.Key = decryptionKey;
                        aes.IV = iv;

                        using (CryptoStream cryptoStream = new CryptoStream(fileStream, aes.CreateDecryptor(), CryptoStreamMode.Read))
                        using (MemoryStream memoryStream = new MemoryStream())
                        {
                            cryptoStream.CopyTo(memoryStream);
                            byte[] decryptedBytes = memoryStream.ToArray();
                            string json = Encoding.UTF8.GetString(decryptedBytes);
                            object obj = JsonSerializer.Deserialize(json, typeObjet);

                            Console.WriteLine($"Données déchiffrées et chargées depuis : {chemin}");
                            success = true;
                            return obj;
                        }
                    }
                }
                else
                {
                    XmlSerializer serializer = new XmlSerializer(typeObjet);

                    using (FileStream fileStream = new FileStream(chemin, FileMode.Open))
                    {
                        object obj = serializer.Deserialize(fileStream);
                        Console.WriteLine($"Données désérialisées depuis : {chemin}");
                        success = true;
                        return obj;
                    }
                }
            }
            catch (CryptographicException)
            {
                Console.WriteLine("Clé incorrecte.");
            }
        }

        if (!success)
        {
            Console.WriteLine("3 tentatives échouées. Suppression du fichier...");
            File.Delete(chemin);
            Console.WriteLine($"Fichier supprimé : {chemin}");
            throw new UnauthorizedAccessException("Échec du décryptage après 3 tentatives.");
        }

        return null;
    }

}
