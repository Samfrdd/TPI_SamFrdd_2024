using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModalManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _modalWindow;


    [SerializeField]
    private ManagerUI _managerUI;

    [SerializeField]
    private GameObject _modalWindowSave;

    public GameObject ModalWindow { get => _modalWindow; set => _modalWindow = value; }
    public ManagerUI ManagerUI { get => _managerUI; set => _managerUI = value; }
    public GameObject ModalWindowSave { get => _modalWindowSave; set => _modalWindowSave = value; }

    public void OpenModal(GameObject modal)
    {
        if (ModalWindowSave.activeSelf)
        {
            gameObject.GetComponent<ModalWindowSave>().CloseModal();
        }
        modal.SetActive(true);
    }

    public void CloseModal(GameObject modal)
    {
        modal.SetActive(false);
    }
}
