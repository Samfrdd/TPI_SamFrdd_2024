/** 
***********************************************************************
Auteur : Sam Freddi
Date : 26.03.2024
Description. MapManager.cs gère toute la sauvegarder de la map et son chargement
version 1.0
***********************************************************************
*/


using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using Unity.VisualScripting;
using UnityEngine;


public class MapManager : MonoBehaviour
{
    private string _saveFolderPath; // Chemin où les fichiers seront sauvegardés
    [SerializeField]
    private ModalWindowSave _modalWindow;
    private const string SaveFolderMaps = "/Maps/";
    private const string SaveFolderTop10 = "/Top10/";
    public static MapManager instance;
    public string SaveFolderPath { get => _saveFolderPath; set => _saveFolderPath = value; }
    public ModalWindowSave ModalWindow { get => _modalWindow; set => _modalWindow = value; }



    private void Awake()
    {
        // Assurez-vous d'appeler get_persistentDataPath dans Awake ou Start

    }

    /// <summary>
    /// Définit le chemin du dossier de sauvegarde en fonction du type de données à manipuler.
    /// </summary>
    /// <param name="folder">Le type de données (par exemple, "Maps" ou autre).</param>
    public void SetFolderPath(string folder)
    {
        if (folder == "Maps")
        {
            SaveFolderPath = Application.persistentDataPath + SaveFolderMaps;
        }
        else
        {
            SaveFolderPath = Application.persistentDataPath + SaveFolderTop10;
        }
    }

    /// <summary>
    /// Obtient l'instance unique de la classe MapManager, en la créant si elle n'existe pas encore.
    /// </summary>
    /// <returns>L'instance unique de MapManager.</returns>
    public static MapManager GetInstance()
    {
        if (instance != null)
        {
            return instance;
        }
        else
        {
            return new MapManager();
        }
    }

    /// <summary>
    /// Gère le clic sur le bouton de sauvegarde en ouvrant une fenêtre modale pour la sauvegarde.
    /// </summary>
    public void BtnSaveClicked()
    {
        ModalWindow.OpenModal();
    }

    /// <summary>
    /// Sauvegarde les données de la carte dans un fichier XML.
    /// </summary>
    /// <param name="mapName">Le nom de la carte.</param>
    /// <param name="mapData">Les données de la carte à sauvegarder.</param>
    public void SaveMap(string mapName, MapData mapData, string folderPath)
    {
        // Créer le dossier de sauvegarde s'il n'existe pas
        if (!Directory.Exists(SaveFolderPath))
            Directory.CreateDirectory(SaveFolderPath);

        string filePath = folderPath + mapName + ".xml";

        // Sérialiser les données de la carte en XML
        // En résumé, ce code prend un objet MapData, le sérialise en XML et enregistre le résultat dans un fichier spécifié.
        XmlSerializer serializer = new XmlSerializer(typeof(MapData));
        using (StreamWriter streamWriter = new StreamWriter(filePath))
        {
            serializer.Serialize(streamWriter, mapData);
        }

        Debug.Log("Map saved at: " + filePath);
    }

    /// <summary>
    /// Génère un nom de carte unique basé sur le timestamp actuel.
    /// </summary>
    /// <returns>Un nom de carte unique basé sur le timestamp actuel.</returns>
    public string GenerateUniqueMapName()
    {
        string timeStamp = DateTime.Now.ToString("yyyyMMddHHmmss");
        return "Map_" + timeStamp;
    }

    /// <summary>
    /// Ajoute les données des blocs présents dans la scène à l'objet MapData spécifié.
    /// </summary>
    /// <param name="mapData">L'objet MapData auquel ajouter les données des blocs.</param>
    public void AddBlocksToMapData(MapData mapData)
    {
        // Récupérez tous les blocs de la scène
        GameObject[] blocks = GameObject.FindGameObjectsWithTag("Bloc");
        foreach (GameObject blockObject in blocks)
        {
            // Créez un nouvel objet BlockData pour chaque bloc de la scène
            BlocData blockData = new BlocData(blockObject.transform.position, blockObject.transform.rotation, blockObject.name);
            // Ajoutez le bloc à la liste de blocs de la carte
            mapData.AddBlockData(blockData);
        }
    }

    /// <summary>
    /// Charge les données d'une carte à partir d'un fichier XML.
    /// </summary>
    /// <param name="mapName">Le nom de la carte à charger.</param>
    /// <returns>Les données de la carte chargées depuis le fichier XML.</returns>
    public MapData LoadMap(string mapName, string folderPath)
    {
        string filePath = "";
        if (folderPath == "Maps")
        {
            filePath = Application.persistentDataPath + SaveFolderMaps + mapName;
        }
        else
        {
            filePath = Application.persistentDataPath + SaveFolderTop10 + mapName;
        }

        // Vérifier si le fichier existe
        if (!File.Exists(filePath))
        {
            Debug.LogError("Map file not found: " + filePath);
            return null;
        }

        // Désérialiser les données de la carte depuis le fichier XML
        XmlSerializer serializer = new XmlSerializer(typeof(MapData));
        using (StreamReader streamReader = new StreamReader(filePath))
        {
            return (MapData)serializer.Deserialize(streamReader);
        }
    }


}
