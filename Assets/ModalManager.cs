using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModalManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _modalWindow;


    [SerializeField]
    private ManagerUI _managerUI;



    public GameObject ModalWindow { get => _modalWindow; set => _modalWindow = value; }
    public ManagerUI ManagerUI { get => _managerUI; set => _managerUI = value; }


   public void OpenModal(GameObject modal)
    {
        modal.SetActive(true);


    }

    public void CloseModal(GameObject modal)
    {
        modal.SetActive(false);
    }
}
