/** 
***********************************************************************
Auteur : Sam Freddi
Date : 26.03.2024
Description : ModalWindowSave.cs ce script gère la fenêtre modal pour sauvegarder la carte
version 1.0
***********************************************************************
*/

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;

using System;

using System.IO;
using System.Xml.Serialization;
using Unity.VisualScripting;


public class ModalWindowSave : MonoBehaviour
{
    [SerializeField]
    private GameObject _modalWindow;
    [SerializeField]
    private GameObject _inputField;
    [SerializeField]
    private GameObject _modalWindowChoice;
    [SerializeField]
    private GameObject _lblErreur;
    [SerializeField]
    private ManagerUI _managerUI;

    [SerializeField]
    private MapManager _managerMap;

    public GameObject ModalWindow { get => _modalWindow; set => _modalWindow = value; }
    public GameObject InputField { get => _inputField; set => _inputField = value; }
    public GameObject LblErreur { get => _lblErreur; set => _lblErreur = value; }
    public ManagerUI ManagerUI { get => _managerUI; set => _managerUI = value; }
    public MapManager ManagerMap { get => _managerMap; set => _managerMap = value; }
    public GameObject ModalWindowChoice { get => _modalWindowChoice; set => _modalWindowChoice = value; }

    public void OpenModal()
    {
        if (ModalWindowChoice.activeSelf)
        {
            gameObject.GetComponent<ModalManager>().CloseModal(ModalWindowChoice);
        }
        ModalWindow.SetActive(true);
        LblErreur.SetActive(false);

    }

    public void CloseModal()
    {
        ModalWindow.SetActive(false);
    }

    public void ValidateInput()
    {
        string userInput = this.InputField.GetComponent<InputField>().text;
        Debug.Log("Input validé : " + userInput);
        // Ici, vous pouvez traiter ou utiliser la valeur entrée par l'utilisateur
        string folderPath = Application.persistentDataPath + "/Maps/"; ;
        string mapName;
        MapData mapData = new MapData();
        bool nameValide = true;
        // Ajoutez tous les blocs de la scène à la liste de blocs

        if (userInput == "")
        {
            mapName = ManagerMap.GenerateUniqueMapName();
        }
        else
        {
            mapName = userInput;
        }


        if (Directory.Exists(folderPath))
        {
            string[] fileNames = Directory.GetFiles(folderPath);

            foreach (string fileName in fileNames)
            {
                if ((mapName.ToLower() + ".xml") == Path.GetFileName(fileName).ToLower())
                {
                    nameValide = false;
                }
            }
        }

        if (nameValide)
        {
            ManagerMap.AddBlocksToMapData(mapData);
            // Sauvegardez la carte
            ManagerMap.SaveMap(mapName, mapData);

            StartCoroutine(ManagerUI.MapSavedConfirmed());

            CloseModal(); // Fermez la fenêtre modale après la validationF
        }
        else
        {
            // Erreur nom pas valide ou deja existant
            LblErreur.SetActive(true);
        }
    }

    public void CancelInput()
    {
        Debug.Log("Input annulé.");
        CloseModal(); // Fermez la fenêtre modale
    }
}
