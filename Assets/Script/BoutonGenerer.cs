/** 
***********************************************************************
Auteur : Sam Freddi
Date : 26.03.2024
Description. BoutonGenerer.cs CLass qui me permets de changer de scene
version 1.0
***********************************************************************
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BoutonGenerer : MonoBehaviour
{
    private const string SceneName = "generationMap"; // Le nom de la sc�ne vers laquelle vous souhaitez changer

    /// <summary>
    /// Charge la scène spécifiée par son nom.
    /// </summary>
    public void ChangeScene()
    {
        SceneManager.LoadScene(SceneName);
    }
}
