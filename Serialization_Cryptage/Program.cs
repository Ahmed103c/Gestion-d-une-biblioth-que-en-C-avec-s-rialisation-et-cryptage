// See https://aka.ms/new-console-template for more information

using System;
using System.IO;
using System.Text.Json;
using System.IO.Compression;
using System.Xml.Serialization;

string chemin = "C:\\Users\\Rzeigui Ahmed\\Documents\\CS\\Gestion-d-une-biblioth-que-en-C-avec-s-rialisation-et-cryptage\\Serialization_Cryptage\\Bibliotheque";
Utilisateur u3 = new Utilisateur("Amine");
SerializationFactory.sauvegarder(chemin, TypeSerialization.binaire, u3,"1234");
Utilisateur u4 = new Utilisateur("Arij");
SerializationFactory.sauvegarder(chemin, TypeSerialization.binaire, u4, "1235");

string chemin1 = "C:\\Users\\Rzeigui Ahmed\\Documents\\CS\\Gestion-d-une-biblioth-que-en-C-avec-s-rialisation-et-cryptage\\Serialization_Cryptage\\Bibliotheque\\Utilisateur_Arij.bjson";
Utilisateur utilisateur = (Utilisateur)SerializationFactory.Charger(chemin1, typeof(Utilisateur), TypeSerialization.binaire);
Console.WriteLine($"Nom : {utilisateur.Nom}");

string chemin2 = "C:\\Users\\Rzeigui Ahmed\\Documents\\CS\\Gestion-d-une-biblioth-que-en-C-avec-s-rialisation-et-cryptage\\Serialization_Cryptage\\Bibliotheque\\Utilisateur_Amine.bjson";
Utilisateur utilisateur2 = (Utilisateur)SerializationFactory.Charger(chemin2, typeof(Utilisateur), TypeSerialization.binaire);
Console.WriteLine($"Nom : {utilisateur2.Nom}");