using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EntryData
{
    // Start is called before the first frame update
    private string _nom;
    private GameObject _entry;
    private int _nbExit;
    private float _distance;

    private float _entryFitness;

    public GameObject Entry { get => _entry; set => _entry = value; }
    public int NbExit { get => _nbExit; set => _nbExit = value; }
    public float Distance { get => _distance; set => _distance = value; }
    public float EntryFitness { get => _entryFitness; set => _entryFitness = value; }
    public string Nom { get => _nom; set => _nom = value; }

    public EntryData(string nom, GameObject entry, int nbExit, float distance, float entryFitness)
    {
        Nom = nom;
        Entry = entry;
        NbExit = nbExit;
        Distance = distance;
        EntryFitness = entryFitness;
    }


}
