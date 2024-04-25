/** 
***********************************************************************
Auteur : Sam Freddi
Date : 26.03.2024
Description : Enter2_PathfinderNewDirection.cs script du début de l'algorithm pour faire apparaître le premier pathfinder 
version 1.0
***********************************************************************
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enter2_PathfinderNewDirection : MonoBehaviour
{
    [SerializeField]
    private GameObject _prefabPathfinder;

    [SerializeField]
    private GameObject _prefabParent;

    [SerializeField]
    private GameObject _gameManager;


    public GameObject PrefabPathfinder { get => _prefabPathfinder; set => _prefabPathfinder = value; }
    public GameObject PrefabParent { get => _prefabParent; set => _prefabParent = value; }
    public GameObject GameManager { get => _gameManager; set => _gameManager = value; }

    // Start is called before the first frame update

    void Start()
    {
        PrefabParent = GameObject.FindWithTag("IA");
        GameManager = GameObject.FindWithTag("gameManager");
        PrefabParent = GameObject.FindWithTag("IA");
    }

    public void StartPathfinder()
    {
        GameManager.GetComponent<ManagerUI>().ClearMapInfo();
        GameManager.GetComponent<ManagerUI>().ClearAllPathinder();
        GameManager.GetComponent<ManagerUI>().SetTexBoxText("Pathfinder en cours de recherche...");
        SpawnPathfinder();
        GameManager.GetComponent<ManagerUI>().RemoveButtonGeneration();
        GameManager.GetComponent<ManagerUI>().RemoveButtonSave();
        GameManager.GetComponent<ManagerUI>().RemoveButtonStart();
        GameManager.GetComponent<ManagerUI>().StartTimer();
        GameManager.GetComponent<ManagerUI>().SetBtnPause(true);


    }
    // Update is called once per frame
    public void SpawnPathfinder()
    {
        GameObject pathfinder = Instantiate(PrefabPathfinder, transform.position, transform.rotation);
        pathfinder.transform.parent = PrefabParent.transform;
        pathfinder.GetComponent<Pathfinding1>().SetOriginal(true);

    }






}
