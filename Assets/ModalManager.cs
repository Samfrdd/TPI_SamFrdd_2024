// /** 
// ***********************************************************************
// Auteur : Sam Freddi
// Date : 17.04.2024
// Description : ModalManager.cs est une classe qui me permets de gérer l'ouverture des fenêtres modale
// version 1.0
// ***********************************************************************
// */
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class ModalManager : MonoBehaviour
// {
//     [SerializeField]
//     private GameObject _modalWindow;


//     [SerializeField]
//     private ManagerUI _managerUI;

//     [SerializeField]
//     private GameObject _modalWindowSave;

//     [SerializeField]
//     private GameObject _modalWindowInformation;
//     [SerializeField]
//     private GameObject _modalWindowChoice;

//     public GameObject ModalWindow { get => _modalWindow; set => _modalWindow = value; }
//     public ManagerUI ManagerUI { get => _managerUI; set => _managerUI = value; }
//     public GameObject ModalWindowSave { get => _modalWindowSave; set => _modalWindowSave = value; }
//     public GameObject ModalWindowInformation { get => _modalWindowInformation; set => _modalWindowInformation = value; }
//     public GameObject ModalWindowChoice { get => _modalWindowChoice; set => _modalWindowChoice = value; }

//     /// <summary>
//     /// Ouvre une fenêtre modale spécifiée, en fermant préalablement toute autre fenêtre modale ouverte.
//     /// </summary>
//     /// <param name="modal">Le GameObject de la fenêtre modale à ouvrir.</param>
//     public void OpenModal(GameObject modal)
//     {
//         if (ModalWindowSave.activeSelf)
//         {
//             gameObject.GetComponent<ModalWindowSave>().CloseModal();
//         }
//         if (ModalWindowInformation.activeSelf)
//         {
//             CloseModal(ModalWindowInformation);
//         }
//         if (ModalWindowChoice.activeSelf)
//         {
//             CloseModal(ModalWindowChoice);
//         }
//         modal.SetActive(true);
//     }

//     /// <summary>
//     /// Ferme une fenêtre modale spécifiée en désactivant son GameObject.
//     /// </summary>
//     /// <param name="modal">Le GameObject de la fenêtre modale à fermer.</param>
//     public void CloseModal(GameObject modal)
//     {
//         modal.SetActive(false);
//     }
// }
// // 