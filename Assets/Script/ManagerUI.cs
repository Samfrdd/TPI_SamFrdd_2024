/** 
***********************************************************************
Auteur : Sam Freddi
Date : 26.03.2024
Description : ManagerScene.cs permets de manager tous l'affichage de notre scene
version 1.0
***********************************************************************
*/

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

using Button = UnityEngine.UI.Button;

public class ManagerUI : MonoBehaviour
{

    [SerializeField]
    private GameObject _btnPrefab; // prefab du button
    [SerializeField]
    private Button _btnSave; // Btn save qui est dans la scene
    [SerializeField]
    private Button _btnStart; // Btn qui permet de lancer le pathfinder
    [SerializeField]
    private Button _btnChoice; // Btn qui permet de lancer le pathfinder    
    [SerializeField]
    private Button _btnPause; // Btn qui permet de lancer le pathfinder
    [SerializeField]
    private Button _btnRestartGenerator; // Btn regénerer qui est dans la scene
    [SerializeField]
    private GameObject _textBox; // textbox qui est en haut de la scene
    [SerializeField]
    private GameObject _timer; // textbox qui est en haut de la scene

    [SerializeField]
    private GameObject _lblConfirmationSave; // textbox qui est en haut de la scene

    [SerializeField]
    private GameObject _infoPathfinder; // textbox qui est en haut de la scene

    [SerializeField]
    private GameObject _iAFolder; // textbox qui est en haut de la scene

    [SerializeField]
    private RandomGeneration _managerGeneration; // textbox qui est en haut de la scene
    [SerializeField]
    private ModalManager _managerModal; // textbox qui est en haut de la scene
    [SerializeField]
    private GameObject _modalChoice;
    [SerializeField]
    private GameObject _modalInformation;
    [SerializeField]
    private GameObject _lblTrouve;
    [SerializeField]
    private GameObject _lblTemps;
    [SerializeField]
    private GameObject _allBloc;
    [SerializeField]
    private Chronometre _chronometre;
    [SerializeField]
    private Material _materialYellow;
    private int _nbPathfinder;


    public Button BtnRestartGenerator { get => _btnRestartGenerator; set => _btnRestartGenerator = value; }
    public GameObject BtnPrefab { get => _btnPrefab; set => _btnPrefab = value; }
    public Button BtnSave { get => _btnSave; set => _btnSave = value; }
    public Button BtnStart { get => _btnStart; set => _btnStart = value; }
    public GameObject TextBox { get => _textBox; set => _textBox = value; }
    public GameObject InfoPathfinder { get => _infoPathfinder; set => _infoPathfinder = value; }
    public int NbPathfinder { get => _nbPathfinder; set => _nbPathfinder = value; }
    public GameObject LblConfirmationSave { get => _lblConfirmationSave; set => _lblConfirmationSave = value; }
    public GameObject IAFolder { get => _iAFolder; set => _iAFolder = value; }
    public GameObject AllBloc { get => _allBloc; set => _allBloc = value; }
    public Chronometre Chronometre { get => _chronometre; set => _chronometre = value; }
    public GameObject Timer { get => _timer; set => _timer = value; }
    public Button BtnPause { get => _btnPause; set => _btnPause = value; }
    public RandomGeneration ManagerGeneration { get => _managerGeneration; set => _managerGeneration = value; }
    public Button BtnChoice { get => _btnChoice; set => _btnChoice = value; }
    public ModalManager ManagerModal { get => _managerModal; set => _managerModal = value; }
    public GameObject ModalChoice { get => _modalChoice; set => _modalChoice = value; }
    public GameObject ModalInformation { get => _modalInformation; set => _modalInformation = value; }
    public GameObject LblTrouve { get => _lblTrouve; set => _lblTrouve = value; }
    public GameObject LblTemps { get => _lblTemps; set => _lblTemps = value; }
    public Material MaterialYellow { get => _materialYellow; set => _materialYellow = value; }



    // Start is called before the first frame update


    public void Start()
    {
        Debug.Log(" random Gen player : " + PlayerPrefs.HasKey("nameMap"));

        if (PlayerPrefs.HasKey("nameMap"))
        {
            string nameMap = PlayerPrefs.GetString("nameMap"); // Récupérez le paramètre de PlayerPrefs
            Debug.Log("Paramètre récupéré : " + nameMap);
            MapData _mapToLoad = gameObject.GetComponent<MapManager>().LoadMap(nameMap);
            Debug.Log(_mapToLoad);
            gameObject.GetComponent<LoadMap>().GenerateMapFromSave(_mapToLoad.Blocks);
            SetTexBoxText("Map telechargé : " + nameMap);
            PlayerPrefs.DeleteKey("nameMap");
        }
        else
        {
            ManagerGeneration.StartGeneation();
        }
    }
    public void UpdateView()
    {
        InfoPathfinder.GetComponent<Text>().text = " Nombre de pathfinder : " + _nbPathfinder;

    }
    public void RemoveButtonGeneration()
    {
        BtnRestartGenerator.gameObject.SetActive(false);
    }

    public void ClearInfo()
    {
        NbPathfinder = 0;
        InfoPathfinder.GetComponent<Text>().text = "";
        SetBtnChoice(false);
        UpdateView();

    }
    public void RemoveButtonSave()
    {
        BtnSave.gameObject.SetActive(false);

    }

    public void SetBtnSave()
    {
        BtnSave.gameObject.SetActive(true);
    }

    public void RemoveButtonStart()
    {
        BtnStart.gameObject.SetActive(false);

    }

    public IEnumerator MapSavedConfirmed()
    {
        LblConfirmationSave.SetActive(true);
        RemoveButtonSave();
        yield return new WaitForSeconds(5f);
        LblConfirmationSave.SetActive(false);

    }

    public void SetTextTimer(string time)
    {
        Timer.GetComponent<Text>().text = time.ToString();
    }

    public void SetTexBoxText(string text)
    {
        TextBox.GetComponent<Text>().text = text;
    }

    public void SetBtnStart()
    {
        BtnStart.onClick.RemoveAllListeners();
        BtnStart.onClick.AddListener(() => GameObject.FindWithTag("Enter").GetComponent<Enter2_PathfinderNewDirection>().StartPathfinder());
        BtnStart.gameObject.SetActive(true);

        // clear toute les info de la carte
        // passage du pathfinder
        // pathfinder

    }

    public void Paused()
    {
        if (BtnPause.GetComponentInChildren<Text>().text == "Pause")
        {
            BtnPause.GetComponentInChildren<Text>().text = "Reprendre";
            StopTimer();
            StopAllPathinder();
        }
        else
        {
            BtnPause.GetComponentInChildren<Text>().text = "Pause";
            Chronometre.StartChronometer();
            StartAllPathfinder();
        }
    }
    public void StartTimer()
    {
        Chronometre.ResetChronometer();
        Chronometre.StartChronometer();
    }

    public void SetBtnPause(bool info)
    {
        BtnPause.gameObject.SetActive(info);
    }

    public void StopAllPathinder()
    {
        foreach (Transform child in IAFolder.transform)
        {
            child.gameObject.GetComponent<Pathfinding1>().ChangeSpeed(0);
        }
    }

    public void StartAllPathfinder()
    {
        foreach (Transform child in IAFolder.transform)
        {
            child.gameObject.GetComponent<Pathfinding1>().ChangeSpeed(30);
        }
    }

    public void StopTimer()
    {
        Chronometre.StopChronometer();
    }

    public void ClearMapInfo()
    {
        foreach (Transform bloc in AllBloc.transform)
        {
            bloc.gameObject.GetComponent<CheckAlreadyPass>().ClearInfo();
        }
    }

    public void ClearAllPathinder()
    {
        foreach (Transform child in IAFolder.transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void SetBtnChoice(bool value)
    {
        BtnChoice.gameObject.SetActive(value);
    }

    public void StartMode1()
    {
        SetBtnChoice(false);
        RemoveButtonSave();
        ManagerModal.CloseModal(ModalChoice);
        // Mode 1. On place 1 entré et 1 sortie et le bot trouve le plus rapide

        ManagerGeneration.GenerateMode1();

    }

    public void StartMode2()
    {
        SetBtnChoice(false);
        RemoveButtonSave();
        ManagerModal.CloseModal(ModalChoice);
        // Mode 2. On place 1 entré et le bot trouve toute les sortie possible
        ManagerGeneration.GenerateMode2();
    }

    public void StartMode3()
    {
        // Mode 3. le bot trouve avec toute les entrés toute les sorties possible
    }

    public void OpenModalInformation()
    {
        ManagerModal.OpenModal(ModalInformation);
    }

    public void SetPanelTextInformation(string info)
    {
        LblTrouve.GetComponent<Text>().text = info;
    }

    public void SetPanelInformationTime(string time)
    {
        LblTemps.GetComponent<Text>().text = "Temps : " + time;
    }

    public void SetNewExit()
    {
        // ClearAllPathinder();
        // ManagerModal.CloseModal(ModalInformation);
        // ClearMapInfo();
        // // Clear tous les bord
        // ManagerGeneration.ClearMapBorders();
        // // Placer bouton 
        // ManagerGeneration.GenerateBtnExit(3);
        // // Tout refermé

    }

    public IEnumerator FindNearestExitBot()
    {
        yield return new WaitForSeconds(0.5f);

        GameObject theBot = new GameObject();
        float bestDistance = 9999999;
        foreach (Transform child in IAFolder.transform)
        {
            float distance = Vector3.Distance(child.transform.position, ManagerGeneration.SortiChoisi.transform.position);
            Debug.Log(theBot.name + " : " + distance);
            if (distance < bestDistance)
            {
                bestDistance = distance;
                theBot = child.gameObject;
            }
        }
        theBot.GetComponent<MeshRenderer>().enabled = true;
        theBot.GetComponent<TrailRenderer>().enabled = true;
        theBot.GetComponent<TrailRenderer>().material = MaterialYellow;
        ChangeParentTrace(theBot);
        Debug.Log("le plus proche est : " + theBot.name);
    }
    public void ChangeParentTrace(GameObject child)
    {
        GameObject parent;

        parent = child.GetComponent<Pathfinding1>().Parent;
        parent.GetComponent<MeshRenderer>().enabled = true;
        parent.GetComponent<TrailRenderer>().enabled = true;
        parent.GetComponent<TrailRenderer>().material = MaterialYellow;

        if (!parent.GetComponent<Pathfinding1>().IsOriginal)
        {
            ChangeParentTrace(parent);
        }
    }
}

