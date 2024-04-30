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

    [SerializeField]
    private GameObject _modalWindowInformation;
    [SerializeField]
    private GameObject _modalWindowChoice;

    public GameObject ModalWindow { get => _modalWindow; set => _modalWindow = value; }
    public ManagerUI ManagerUI { get => _managerUI; set => _managerUI = value; }
    public GameObject ModalWindowSave { get => _modalWindowSave; set => _modalWindowSave = value; }
    public GameObject ModalWindowInformation { get => _modalWindowInformation; set => _modalWindowInformation = value; }
    public GameObject ModalWindowChoice { get => _modalWindowChoice; set => _modalWindowChoice = value; }

    public void OpenModal(GameObject modal)
    {
        if (ModalWindowSave.activeSelf)
        {
            gameObject.GetComponent<ModalWindowSave>().CloseModal();
        }
        if (ModalWindowInformation.activeSelf)
        {
            CloseModal(ModalWindowInformation);
        }
        if (ModalWindowChoice.activeSelf)
        {
            CloseModal(ModalWindowChoice);
        }
        modal.SetActive(true);
    }

    public void CloseModal(GameObject modal)
    {
        modal.SetActive(false);
    }
}
