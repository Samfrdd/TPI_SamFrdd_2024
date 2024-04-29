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

    // Fonction pour démarrer le chronomètre
    public void StartChronometer()
    {
        ChronometerRunning = true;
    }

    // Fonction pour arrêter le chronomètre
    public void StopChronometer()
    {
        ChronometerRunning = false;
    }

    // Fonction pour réinitialiser le chronomètre
    public void ResetChronometer()
    {
        ElapsedTime = 0f;
    }

    // Fonction pour récupérer le temps écoulé en millisecondes
    public float GetElapsedTime()
    {
        return ElapsedTime;
    }

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

            // Vous pouvez ajouter ici du code pour afficher le temps écoulé dans l'interface utilisateur, par exemple
            // Debug.Log("Temps écoulé : " + elapsedTime.ToString("F2")); // Affiche le temps avec deux décimales

            gameObject.GetComponent<ManagerUI>().SetTextTimer(FormatTime(ElapsedTime));
            gameObject.GetComponent<ManagerUI>().SetPanelInformationTime(FormatTime(ElapsedTime));
        }
    }
}
