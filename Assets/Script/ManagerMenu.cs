/** 
***********************************************************************
Auteur : Sam Freddi
Date : 26.03.2024
Description : ManagerMenu.cs permets de gérer l'affichage du menu
version 1.0
***********************************************************************
*/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject _mainWindows;
    [SerializeField]
    private GameObject _mainContent;
    
    [SerializeField]
    private GameObject _loadWindows;
        [SerializeField]
    private GameObject _loadContent;

    public GameObject MainWindows { get => _mainWindows;private set => _mainWindows = value; }
    public GameObject MainContent { get => _mainContent;private set => _mainContent = value; }
    public GameObject LoadWindows { get => _loadWindows;private set => _loadWindows = value; }
    public GameObject LoadContent { get => _loadContent;private set => _loadContent = value; }

    private void Start() {
        MainWindows.SetActive(true);
        LoadWindows.SetActive(true);


        MainContent.SetActive(true);
        LoadContent.SetActive(false);
    }

    public void AfficherLoadList(){
        MainContent.SetActive(false);
        LoadContent.SetActive(true);
    }

        public void AfficherMain(){
        MainContent.SetActive(true);
        LoadContent.SetActive(false);
    }
}
