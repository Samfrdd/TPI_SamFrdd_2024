/** 
***********************************************************************
Auteur : Sam Freddi
Date : 17.04.2024
Description : connecteur.cs est une classe qui se place sur les blocs de connexion entre chaque blocs
version 1.0
***********************************************************************
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class connecteur : MonoBehaviour
{
    [SerializeField]
    private string _connected;

    [SerializeField]
    private GameObject _blockConnecte;



    public string Connected { get => _connected; set => _connected = value; }
    public GameObject BlockConnecte { get => _blockConnecte; set => _blockConnecte = value; }

    // Start is called before the first frame update
    void Awake()
    {
        _connected = "pasConnecte";
        // _blockConnecte = new GameObject();
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// Déclenché lorsque ce collider entre en collision avec un autre collider.
    /// Gère les interactions entre cet objet et les autres objets lors de la collision.
    /// </summary>
    /// <param name="other">Le collider avec lequel cet objet entre en collision.</param>
    private void OnTriggerEnter(Collider other)
    {

        if (gameObject.CompareTag("mauvais"))
        {
            if (other.gameObject.CompareTag("Connecteur"))
            {
                _connected = "mauvaiseConnection";
            }
            else if (other.gameObject.CompareTag("mauvais"))
            {
                _connected = "connecte";
                _blockConnecte = other.gameObject;
            }
        }
        else
        {
            if (other.gameObject.CompareTag("Connecteur"))
            {
                _connected = "connecte";
                _blockConnecte = other.gameObject;
            }
            else if (other.gameObject.CompareTag("mauvais"))
            {
                _connected = "mauvaiseConnection";
            }
        }

    }



}
