using System;

using System;
using System.IO;
using System.Text.Json;
using System.IO.Compression;
using System.Xml.Serialization;
using System.Net.NetworkInformation;

public enum TypeSerialization
{
	binaire,
	XML 
}

public class SerializationFactory
{
	public static void sauvegarder(string chemin, TypeSerialization typeserialization,object obj)
	{
		if (typeserialization == TypeSerialization.binaire)
		{
            string json = JsonSerializer.Serialize(obj);
            byte[] jsonBytes = System.Text.Encoding.UTF8.GetBytes(json);
            chemin = Path.Combine(chemin, $"{obj.GetType().Name}_{obj.GetType().GetProperty("Nom").GetValue(obj)?.ToString()}.bjson");
            using (FileStream fileStream = new FileStream(chemin, FileMode.Create))
            using (GZipStream compressionStream = new GZipStream(fileStream, CompressionMode.Compress))
            {
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
    public static object charger(string chemin, Type typeObjet, TypeSerialization typeserialization)
    {
        if (typeserialization == TypeSerialization.binaire)
        {
            // Chemin complet du fichier .bjson
            if (!File.Exists(chemin))
            {
                throw new FileNotFoundException($"Fichier introuvable : {chemin}");
            }
            using (FileStream fileStream = new FileStream(chemin, FileMode.Open))
            using (GZipStream decompressionStream = new GZipStream(fileStream, CompressionMode.Decompress))
            using (MemoryStream memoryStream = new MemoryStream())
            {
                decompressionStream.CopyTo(memoryStream);
                byte[] decompressedBytes = memoryStream.ToArray();
                string json = System.Text.Encoding.UTF8.GetString(decompressedBytes);

                // Désérialiser le JSON en objet du type spécifié
                object obj = JsonSerializer.Deserialize(json, typeObjet);
                Console.WriteLine($"Données désérialisées depuis : {chemin}");
                return obj;
            }
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
