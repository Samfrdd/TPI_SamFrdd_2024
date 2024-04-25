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

    public static MapManager instance;

    public string SaveFolderPath { get => _saveFolderPath; set => _saveFolderPath = value; }
    public ModalWindowSave ModalWindow { get => _modalWindow; set => _modalWindow = value; }

    private void Awake()
    {
        // Assurez-vous d'appeler get_persistentDataPath dans Awake ou Start
        SaveFolderPath = Application.persistentDataPath + "/Maps/";
    }

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

    public void BtnSaveClicked()
    {

        ModalWindow.OpenModal();

        
    }
    public void SaveMap(string mapName, MapData mapData)
    {
        // Créer le dossier de sauvegarde s'il n'existe pas
        if (!Directory.Exists(SaveFolderPath))
            Directory.CreateDirectory(SaveFolderPath);

        string filePath = SaveFolderPath + mapName + ".xml";

        // Sérialiser les données de la carte en XML
        XmlSerializer serializer = new XmlSerializer(typeof(MapData));
        using (StreamWriter streamWriter = new StreamWriter(filePath))
        {
            serializer.Serialize(streamWriter, mapData);
        }

        Debug.Log("Map saved at: " + filePath);
    }


    public string GenerateUniqueMapName()
    {
        // Générez un nom unique basé sur le timestamp actuel
        string timeStamp = DateTime.Now.ToString("yyyyMMddHHmmss");
        return "Map_" + timeStamp;
    }
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

    public MapData LoadMap(string mapName)
    {
        string filePath = SaveFolderPath + mapName;

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
