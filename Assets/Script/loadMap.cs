 /** 
***********************************************************************
Auteur : Sam Freddi
Date : 26.03.2024
Description : loadMap.cs ce script permet de charger les cartes sauvegarder
version 1.0
***********************************************************************
*/


using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using Unity.VisualScripting;
using UnityEngine;

public class LoadMap : MonoBehaviour
{
    [SerializeField]
    private MapManager _mapManager;

    [SerializeField]
    private ManagerUI _managerUI;

    [SerializeField]
    private List<GameObject> _lstBlockMaze;

    [SerializeField]
    private Transform _folderBlocParent;

    public MapManager MapManager { get => _mapManager;private set => _mapManager = value; }
    public ManagerUI ManagerUI { get => _managerUI;private set => _managerUI = value; }
    public List<GameObject> LstBlockMaze { get => _lstBlockMaze;private set => _lstBlockMaze = value; }
    public Transform FolderBlocParent { get => _folderBlocParent;private set => _folderBlocParent = value; }


    //manager.instance.LoadMap("Map_20240213144955.xml");

    private void Start()
    {

        Debug.Log(" load playerPref : " + PlayerPrefs.HasKey("nameMap"));

        if (PlayerPrefs.HasKey("nameMap"))
        {
          
            // Faites ce que vous devez faire avec le paramètre
        }
        else
        {
            Debug.LogWarning("Aucun paramètre trouvé !");
        }

    }

    public void GenerateMapFromSave(List<BlocData> map)
    {
        foreach (BlocData blockData in map)
        {
            GameObject desiredElement;
            string fullString = blockData.Type;
            string[] parts = fullString.Split('('); // Diviser la chaîne en fonction de '('
            string extractedString = parts[0]; // Prendre la première partie



            desiredElement = LstBlockMaze.Find(item => item.name == extractedString);
            // Debug.Log(extractedString);

            GameObject block = Instantiate(desiredElement, blockData.Position, blockData.Rotation);

            block.transform.parent = FolderBlocParent;
        }

        ManagerUI.SetBtnChoice(true);
    }
}
