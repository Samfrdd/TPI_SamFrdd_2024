using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class connecteur : MonoBehaviour
{
    [SerializeField]
    private string _connected;

    [SerializeField]
    private GameObject _blockConnecte;



    public string Connected { get => _connected; private set => _connected = value; }
    public GameObject BlockConnecte { get => _blockConnecte; set => _blockConnecte = value; }

    // Start is called before the first frame update
    void Start()
    {
        _connected = "pasConnecte";
        // _blockConnecte = new GameObject();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {

        if (gameObject.CompareTag("mauvais")){
            if (other.gameObject.CompareTag("Connecteur"))
            {
                _connected = "mauvaiseConnection";
            }
            else if (other.gameObject.CompareTag("mauvais"))
            {
                _connected = "connecte";
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
