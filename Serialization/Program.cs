// See https://aka.ms/new-console-template for more information

using System;
using System.IO;
using System.Text.Json;
using System.IO.Compression;
using System.Xml.Serialization;

string chemin = "C:\\Users\\Rzeigui Ahmed\\Documents\\CS\\Gestion-d-une-biblioth-que-en-C-avec-s-rialisation-et-cryptage\\Serialization\\Bibliotheque";
Utilisateur u3 = new Utilisateur("Amine");
SerializationFactory.sauvegarder(chemin, TypeSerialization.binaire, u3, "1234");
Utilisateur u4 = new Utilisateur("Arij");
SerializationFactory.sauvegarder(chemin, TypeSerialization.binaire, u4, "1235");

string chemin1 = "C:\\Users\\Rzeigui Ahmed\\Documents\\CS\\Gestion-d-une-biblioth-que-en-C-avec-s-rialisation-et-cryptage\\Serialization\\Bibliotheque\\Utilisateur_Arij.bjson";
Utilisateur utilisateur = (Utilisateur)SerializationFactory.Charger(null,chemin1, typeof(Utilisateur), TypeSerialization.binaire);
Console.WriteLine($"Nom : {utilisateur.Nom}");

string chemin2 = "C:\\Users\\Rzeigui Ahmed\\Documents\\CS\\Gestion-d-une-biblioth-que-en-C-avec-s-rialisation-et-cryptage\\Serialization\\Bibliotheque\\Utilisateur_Amine.bjson";
Utilisateur utilisateur2 = (Utilisateur)SerializationFactory.Charger(null,chemin2, typeof(Utilisateur), TypeSerialization.binaire);
Console.WriteLine($"Nom : {utilisateur2.Nom}");



//Utilisateur u6 = new Utilisateur("Arij");
//SerializationFactory.sauvegarder(chemin, TypeSerialization.XML, u6, "1235");

//string chemin3 = "C:\\Users\\Rzeigui Ahmed\\Documents\\CS\\Gestion-d-une-biblioth-que-en-C-avec-s-rialisation-et-cryptage\\Serialization\\Bibliotheque\\Utilisateur_Arij.xml";
//string chemin31 = "C:\\Users\\Rzeigui Ahmed\\Documents\\CS\\Gestion-d-une-biblioth-que-en-C-avec-s-rialisation-et-cryptage\\Serialization\\Bibliotheque\\Utilisateur_Arij.hash";
//Utilisateur utilisateur3 = (Utilisateur)SerializationFactory.Charger(chemin31,chemin3, typeof(Utilisateur), TypeSerialization.XML);
//Console.WriteLine($"Nom : {utilisateur3.Nom}");
