/** 
***********************************************************************
Auteur : Sam Freddi
Date : 26.03.2024
Description : Exit.cs script qui ce place sur le bloc de sortie. Si un pathfinder rentre en contact avec lui cela signifie que nous avons trouvé une sortie possible
version 1.0
***********************************************************************
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField]
    private Material _material;

    public Material Material { get => _material; set => _material = value; }


    /// <summary>
    /// Méthode appelée lorsque le collider de cet objet entre en contact avec un autre collider.
    /// </summary>
    /// <param name="other">Le collider avec lequel cet objet entre en contact.</param>

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("pathfinder"))
        {
            other.gameObject.GetComponent<TrailRenderer>().material = Material;
            other.gameObject.GetComponent<Pathfinding1>().FindExit();
            other.gameObject.GetComponent<Pathfinding1>().StopMovement();
            GameObject.FindWithTag("gameManager").GetComponent<ManagerUI>().NbExitFound++;
        }
    }
}
