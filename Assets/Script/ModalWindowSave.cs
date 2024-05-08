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
    private GameObject _modalWindow; // Fenêtre modal
    [SerializeField]
    private GameObject _inputField; // Input textuel
    [SerializeField]
    private GameObject _modalWindowChoice; // Bouton pour afficher les choix
    [SerializeField]
    private GameObject _lblErreur; // lbl erreur si le nom existe deja
    [SerializeField]
    private ManagerUI _managerUI; // class managerUI
    [SerializeField]
    private MapManager _managerMap; // class MapManager

    public GameObject ModalWindow { get => _modalWindow; set => _modalWindow = value; }
    public GameObject InputField { get => _inputField; set => _inputField = value; }
    public GameObject LblErreur { get => _lblErreur; set => _lblErreur = value; }
    public ManagerUI ManagerUI { get => _managerUI; set => _managerUI = value; }
    public MapManager ManagerMap { get => _managerMap; set => _managerMap = value; }
    public GameObject ModalWindowChoice { get => _modalWindowChoice; set => _modalWindowChoice = value; }

    /// <summary>
    /// Ouvre la fenêtre modale.
    /// </summary>
    public void OpenModal()
    {
        if (ModalWindowChoice.activeSelf)
        {
            gameObject.GetComponent<ModalManager>().CloseModal(ModalWindowChoice);
        }
        ModalWindow.SetActive(true);
        LblErreur.SetActive(false);
    }

    /// <summary>
    /// Ferme la fenêtre modale.
    /// </summary>
    public void CloseModal()
    {
        ModalWindow.SetActive(false);
    }

    /// <summary>
    /// Valide l'entrée de l'utilisateur pour créer ou enregistrer une carte.
    /// </summary>
    public void ValidateInput()
    {
        string userInput = this.InputField.GetComponent<InputField>().text;
        Debug.Log("Input validé : " + userInput);
        string folderPath = Application.persistentDataPath + "/Maps/";
        string mapName;
        MapData mapData = new MapData();
        bool nameValide = true;

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
            ManagerMap.SetFolderPath("Maps");
            ManagerMap.AddBlocksToMapData(mapData);
            // Sauvegardez la carte
            ManagerMap.SaveMap(mapName, mapData, folderPath);

            StartCoroutine(ManagerUI.MapSavedConfirmed());

            CloseModal(); // Fermez la fenêtre modale après la validationF
        }
        else
        {
            // Erreur nom pas valide ou deja existant
            LblErreur.SetActive(true);
        }
    }

    /// <summary>
    /// Annule l'entrée de l'utilisateur et ferme la fenêtre modale.
    /// </summary>
    public void CancelInput()
    {
        Debug.Log("Input annulé.");
        CloseModal(); // Fermez la fenêtre modale
    }
}
