/** 
***********************************************************************
Auteur : Sam Freddi
Date : 26.03.2024
Description. RayCastScript.cs nous donne les valeur du raycast et les donne au pathfinder
version 1.0
***********************************************************************
*/

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RayCastScript : MonoBehaviour
{
    [SerializeField]
    private float _distance;
    [SerializeField]
    private LayerMask _layerMask;

    [SerializeField]
    int _color;

    [SerializeField]
    private int _valeurBit;

    public float Distance { get => _distance; set => _distance = value; }
    public LayerMask LayerMask { get => _layerMask; set => _layerMask = value; }
    public int Color { get => _color; set => _color = value; }
    public int ValeurBit { get => _valeurBit; set => _valeurBit = value; }


    // See Order of Execution for Event Functions for information on FixedUpdate() and Update() related to physics queries
    void FixedUpdate()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 4f, LayerMask))
        {
            if (Color == 1)
            {
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, UnityEngine.Color.yellow);
            }
            else if (Color == 2)
            {
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, UnityEngine.Color.blue);
            }
            else
            {
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, UnityEngine.Color.green);
            }

            if (ValeurBit == 1)
            {
                Distance = 1;
            }
            else if (ValeurBit == 2)
            {
                Distance = 2;
            }
            else if (ValeurBit == 4)
            {
                Distance = 4;
            }
        }
        else
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 3, UnityEngine.Color.red);
            Distance = 0;
        }
    }
}
