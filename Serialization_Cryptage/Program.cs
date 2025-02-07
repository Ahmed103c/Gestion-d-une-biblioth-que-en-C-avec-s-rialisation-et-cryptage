// See https://aka.ms/new-console-template for more information

using System;
using System.IO;
using System.Text.Json;
using System.IO.Compression;
using System.Xml.Serialization;


Console.WriteLine("Sérialization binaire ");




// Instanciation et initialisation de l'objet 
Utilisateur utilisateur1 = new Utilisateur();



// Sérialisation JSON
string json = JsonSerializer.Serialize(utilisateur1);
byte[] jsonBytes = System.Text.Encoding.UTF8.GetBytes(json);

// Création du flux 
string chemin = "C:\\Users\\Rzeigui Ahmed\\Documents\\CS\\Gestion-d-une-biblioth-que-en-C-avec-s-rialisation-et-cryptage\\Serialization_Cryptage";
chemin = Path.Combine(chemin, "utilisateur1.bjson"); // Ajout du fichier
using (FileStream fileStream = new FileStream(chemin, FileMode.Create))
using (GZipStream compressionStream = new GZipStream(fileStream, CompressionMode.Compress))
{
    compressionStream.Write(jsonBytes, 0, jsonBytes.Length);
}

Console.WriteLine($"Données sérialisées et compressées dans : {chemin}");

Console.WriteLine("Sérialisation XML");


// Création du sérialiseur XML
XmlSerializer serializer = new XmlSerializer(typeof(Utilisateur));
chemin = "C:\\Users\\Rzeigui Ahmed\\Documents\\CS\\Gestion-d-une-biblioth-que-en-C-avec-s-rialisation-et-cryptage\\Serialization_Cryptage";
chemin = Path.Combine(chemin, "utilisateur1.xml");

using (FileStream fileStream = new FileStream(chemin, FileMode.Create))
{
    serializer.Serialize(fileStream, utilisateur1);
}

Console.WriteLine($"Données sérialisées en XML dans : {chemin}");
