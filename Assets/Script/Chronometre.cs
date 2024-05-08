/** 
***********************************************************************
Auteur : Sam Freddi
Date : 17.04.2024
Description : Chronometre.cs est une classe qui me permets de gérer le chronometre
version 1.0
***********************************************************************
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chronometre : MonoBehaviour
{
    [SerializeField]
    private float _elapsedTime = 0f;
    private bool _chronometerRunning = false;

    public float ElapsedTime { get => _elapsedTime; set => _elapsedTime = value; }
    public bool ChronometerRunning { get => _chronometerRunning; set => _chronometerRunning = value; }

    /// <summary>
    ///   Fonction pour commencer le chronomètre 
    /// </summary>
    public void StartChronometer()
    {
        ChronometerRunning = true;
    }

    /// <summary>
    ///   Fonction pour arrêter le chronomètre 
    /// </summary>
    public void StopChronometer()
    {
        ChronometerRunning = false;
    }

    /// <summary>
    ///   Fonction pour reset le chronomètre 
    /// </summary>
    public void ResetChronometer()
    {
        ElapsedTime = 0f;
    }

    /// <summary>
    /// Fonction pour récupérer le temps écoulé en millisecondes
    /// </summary>
    public float GetElapsedTime()
    {
        return ElapsedTime;
    }

    /// <summary>
    /// Formate un temps donné en secondes en une représentation sous forme de chaîne de caractères d'heures, minutes et secondes.
    /// </summary>
    /// <param name="timeInSeconds">Le temps en secondes à formater.</param>
    /// <returns>Une chaîne de caractères représentant le temps formaté selon le format "hh:mm:ss.ss".</returns>
    public string FormatTime(float timeInSeconds)
    {
        int hours = Mathf.FloorToInt(timeInSeconds / 3600);
        int minutes = Mathf.FloorToInt((timeInSeconds % 3600) / 60);
        float seconds = timeInSeconds % 60;

        return string.Format("{1:00}:{2:00.00}", hours, minutes, seconds);
    }

    void Update()
    {
        // Vérifier si le chronomètre est en cours d'exécution
        if (ChronometerRunning)
        {
            // Mettre à jour le temps écoulé
            ElapsedTime += Time.deltaTime;
            gameObject.GetComponent<ManagerUI>().SetTextTimer(FormatTime(ElapsedTime));
            gameObject.GetComponent<ManagerUI>().SetPanelInformationTime(FormatTime(ElapsedTime));
        }
    }
}
