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
    private Button _btnInformation; // Btn qui permet de lancer le pathfinder    
    [SerializeField]
    private Button _btnPause; // Btn qui permet de lancer le pathfinder
    [SerializeField]
    private Button _btnRestartGenerator; // Btn regénerer qui est dans la scene
    [SerializeField]
    private GameObject _textBox; // textbox qui est en haut de la scene
    [SerializeField]
    private GameObject _timer; // textbox qui est en haut de la scene
    [SerializeField]
    private GameObject _scrollViewBtnEntry; // textbox qui est en haut de la scene
    [SerializeField]
    private GameObject _scrollViewBtnEntryContent; // textbox qui est en haut de la scene
    [SerializeField]
    private GameObject _prefabBtnScrollView; // textbox qui est en haut de la scene

    [SerializeField]
    private GameObject _lblConfirmationSave; // textbox qui est en haut de la scene

    [SerializeField]
    private GameObject _infoPathfinder; // textbox qui est en haut de la scene
    [SerializeField]
    private GameObject _lblEntryParent; // textbox qui est en haut de la scene   
    [SerializeField]
    private GameObject _lblEntry; // textbox qui est en haut de la scene   
    [SerializeField]
    private GameObject _lblDistance; // textbox qui est en haut de la scene  
    [SerializeField]
    private GameObject _lblNbExit; // textbox qui est en haut de la scene    
    [SerializeField]
    private GameObject _lblFitness; // textbox qui est en haut de la scene

    [SerializeField]
    private GameObject _iAFolder; // textbox qui est en haut de la scene

    [SerializeField]
    private RandomGeneration _managerGeneration; // textbox qui est en haut de la scene
    [SerializeField]
    private AutomaticManager _automaticManager; // textbox qui est en haut de la scene
    [SerializeField]
    private ModalManager _managerModal; // textbox qui est en haut de la scene
    [SerializeField]
    private GameObject _modalChoice;
    [SerializeField]
    private GameObject _modalInformation;
    [SerializeField]
    private GameObject _lblTrouve;
    [SerializeField]
    private GameObject _lblDistanceInfo;
    [SerializeField]
    private GameObject _lblTemps;
    [SerializeField]
    private GameObject _allBloc;
    [SerializeField]
    private Chronometre _chronometre;
    [SerializeField]
    private Material _materialYellow;
    [SerializeField]
    private List<GameObject> _lstEntry;
    [SerializeField]
    private List<GameObject> _lstBtnScrollView;
    private int _nbPathfinder;
    [SerializeField]
    private int _modeEnCours;
    private bool _algoEnCours;
    private int _nbExitFound = 0;




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
    public int ModeEnCours { get => _modeEnCours; set => _modeEnCours = value; }
    public bool AlgoEnCours { get => _algoEnCours; set => _algoEnCours = value; }
    public int NbExitFound { get => _nbExitFound; set => _nbExitFound = value; }
    public GameObject LblDistanceInfo { get => _lblDistanceInfo; set => _lblDistanceInfo = value; }
    public List<GameObject> LstEntry { get => _lstEntry; set => _lstEntry = value; }
    public Button BtnInformation { get => _btnInformation; set => _btnInformation = value; }
    public AutomaticManager AutomaticManager { get => _automaticManager; set => _automaticManager = value; }
    public GameObject ScrollViewBtnEntry { get => _scrollViewBtnEntry; set => _scrollViewBtnEntry = value; }
    public GameObject ScrollViewBtnEntryContent { get => _scrollViewBtnEntryContent; set => _scrollViewBtnEntryContent = value; }
    public GameObject PrefabBtnScrollView { get => _prefabBtnScrollView; set => _prefabBtnScrollView = value; }
    public GameObject LblEntry { get => _lblEntry; set => _lblEntry = value; }
    public GameObject LblDistance { get => _lblDistance; set => _lblDistance = value; }
    public GameObject LblNbExit { get => _lblNbExit; set => _lblNbExit = value; }
    public GameObject LblFitness { get => _lblFitness; set => _lblFitness = value; }
    public GameObject LblEntryParent { get => _lblEntryParent; set => _lblEntryParent = value; }
    public List<GameObject> LstBtnScrollView { get => _lstBtnScrollView; set => _lstBtnScrollView = value; }




    // Start is called before the first frame update


    public void Start()
    {
        Debug.Log(" random Gen player : " + PlayerPrefs.HasKey("nameMap"));
        AlgoEnCours = false;
        if (PlayerPrefs.HasKey("nameMap"))
        {
            string nameMap = PlayerPrefs.GetString("nameMap"); // Récupérez le paramètre de PlayerPrefs
            Debug.Log("Paramètre récupéré : " + nameMap);
            gameObject.GetComponent<MapManager>().SetFolderPath(PlayerPrefs.GetString("folder"));
            MapData _mapToLoad = gameObject.GetComponent<MapManager>().LoadMap(nameMap, PlayerPrefs.GetString("folder"));
            Debug.Log(_mapToLoad);
            gameObject.GetComponent<LoadMap>().GenerateMapFromSave(_mapToLoad.Blocks);
            SetTexBoxText("Map telechargé : " + nameMap);
            PlayerPrefs.DeleteKey("nameMap");
            PlayerPrefs.DeleteKey("folder");
            RemoveButtonStart();
            SetBtnInformation(false);
        }
        else
        {
            SetBtnInformation(false);
            SetBtnChoice(false);
            ManagerGeneration.StartGeneation();
        }
    }

    void Update()
    {
        if (ModeEnCours == 2 && AlgoEnCours)
        {
            if (CheckIfAllBotHaveFinish())
            {
                AlgoEnCours = false;
                SetTexBoxText("Les pathFinder ont trouvé toutes les sorties !");
                BtnRestartGenerator.gameObject.SetActive(true);
                SetBtnStart(2);
                StopTimer();
                SetBtnPause(false);
                OpenModalInformation();
                SetBtnInformation(true);
                Debug.Log("... " + NbExitFound);
                string text = "Nombre de sorti trouvé : " + NbExitFound.ToString();
                SetPanelTextInformation(text);
                SetDistanceInfoText("Les bots ont parcouru " + GetAllDistanceFromBot().ToString() + " mètre pour trouvé les sorties");
            }
        }

    }
    public void UpdateView()
    {


    }

    public void ClearNbExitFound()
    {
        NbExitFound = 0;
    }
    public void RemoveButtonGeneration()
    {
        BtnRestartGenerator.gameObject.SetActive(false);
    }

    public void AddEntry(GameObject entry)
    {
        LstEntry.Add(entry);
    }

    public void ClearListEntry()
    {
        LstEntry.Clear();
    }

    public void ClearInfo()
    {
        NbPathfinder = 0;
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

    public void SetBtnStart(int mode)
    {
        BtnStart.onClick.RemoveAllListeners();
        BtnStart.onClick.AddListener(() => GameObject.FindWithTag("Enter").GetComponent<Enter2_PathfinderNewDirection>().StartPathfinder());
        BtnStart.gameObject.SetActive(true);
        // clear toute les info de la carte
        // passage du pathfinder
        // pathfinder
        ModeEnCours = mode;
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

    public void SetBtnInformation(bool value)
    {
        BtnInformation.gameObject.SetActive(value);
    }


    public void StartMode1()
    {
        // -------------------------- variable
        ModeEnCours = 1;
        NbExitFound = 0;
        // -------------------------- Affichage

        SetBtnChoice(false);
        RemoveButtonSave();
        RemoveButtonStart();
        RemoveButtonGeneration();
        SetBtnInformation(false);
        SetScrollViewBtnEntry(false);
        ClearAllInfoModeAuto();

        ManagerModal.CloseModal(ModalChoice);

        // Mode 1. On place 1 entré et 1 sortie et le bot trouve le plus rapide
        ManagerGeneration.GenerateMode1();

    }

    public void StartMode2()
    {
        // -------------------------- variable
        ModeEnCours = 2;
        NbExitFound = 0;
        // -------------------------- Affichage

        SetBtnChoice(false);
        RemoveButtonSave();
        RemoveButtonStart();
        RemoveButtonGeneration();
        SetBtnInformation(false);
        SetScrollViewBtnEntry(false);
        ClearAllInfoModeAuto();

        ManagerModal.CloseModal(ModalChoice);

        // Mode 2. On place 1 entré et le bot trouve toute les sortie possible
        ManagerGeneration.GenerateMode2();
    }

    public void StartMode3()
    {
        // -------------------------- variable
        NbExitFound = 0;
        ModeEnCours = 3;
        // -------------------------- Affichage
        SetBtnChoice(false);
        RemoveButtonSave();
        RemoveButtonStart();
        RemoveButtonGeneration();
        SetBtnInformation(false);
        SetScrollViewBtnEntry(false);
        ClearAllInfoModeAuto();
        ManagerModal.CloseModal(ModalChoice);
        // Mode 3. le bot trouve avec toute les entrés toute les sorties possible
        AutomaticManager.StartModeAuto();

    }

    public void OpenModalInformation()
    {
        ManagerModal.OpenModal(ModalInformation);
    }

    public void CloseModalInformation()
    {
        ManagerModal.CloseModal(ModalInformation);
    }

    public void SetPanelTextInformation(string info)
    {
        LblTrouve.GetComponent<Text>().text = info;
    }

    public void SetDistanceInfoText(string info)
    {
        LblDistanceInfo.GetComponent<Text>().text = info;
    }

    public void SetPanelInformationTime(string time)
    {
        LblTemps.GetComponent<Text>().text = "Temps : " + time;
    }

    public void SetNewExit()
    {
        if (ModeEnCours == 2 || ModeEnCours == 3)
        {
            Debug.Log("Action non dispobible dans ce mode de jeux");
        }
        else
        {
            ClearAllPathinder();
            SetBtnInformation(false);
            RemoveButtonSave();
            RemoveButtonStart();
            RemoveButtonGeneration();
            SetScrollViewBtnEntry(false);
            ManagerModal.CloseModal(ModalInformation);
            ClearMapInfo();
            // // Clear tous les bord
            ManagerGeneration.ClearMapBordersWithoutEntry();
            // // Placer bouton 
            ManagerGeneration.GenerateBtnExit(1);
            // // Tout refermé
        }


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
        GameObject parent = null;

        if (!child.GetComponent<Pathfinding1>().IsOriginal)
        {
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

    public bool CheckIfAllBotHaveFinish()
    {
        bool isFinish = true;
        foreach (Transform child in IAFolder.transform)
        {
            if (child.GetComponent<Pathfinding1>().Blocked || child.GetComponent<Pathfinding1>().Trouve)
            {
                // Si il a trouve la solution ou est mort dans un mur
            }
            else
            {
                isFinish = false;
            }
        }

        return isFinish;
    }

    public float GetAllDistanceFromBot()
    {

        float distance = 0;
        foreach (Transform child in IAFolder.transform)
        {
            if (child.GetComponent<Pathfinding1>().Trouve)
            {
                distance += child.GetComponent<Pathfinding1>().RealDistance;
            }
        }
        return distance;
    }

    public void SetBtnInScrollView()
    {
        Debug.Log("creation");
        float ContentHeight = 0;


        foreach (var Entry in AutomaticManager.LstAllEntryData)
        {
            // Instancier le bouton à partir du prefab
            GameObject buttonGO = Instantiate(PrefabBtnScrollView, new Vector3(0, 0, 0), Quaternion.identity, ScrollViewBtnEntryContent.transform);


            // D�finir le parent du bouton
            buttonGO.transform.SetParent(ScrollViewBtnEntryContent.transform, false); // Ne pas conserver la rotation et l'échelle du parent
            LstBtnScrollView.Add(buttonGO);

            Text buttonText = buttonGO.GetComponentInChildren<Text>();
            if (buttonText != null)
            {
                buttonText.text = Entry.Nom + " : " + Entry.EntryFitness.ToString();

                // Ajuster la taille du bouton pour correspondre à la taille du texte
                RectTransform buttonRect = buttonGO.GetComponent<RectTransform>();
                buttonRect.sizeDelta = new Vector2(buttonText.preferredWidth + 20, buttonText.preferredHeight + 20);
                ContentHeight += buttonRect.sizeDelta.y + 15;

            }
            // Acc�der au composant Button du bouton et ajouter une fonction à appeler avec un paramètre
            Button button = buttonGO.GetComponent<Button>();
            if (button != null)
            {
                // Ajouter un écouteur d'évènement au bouton avec une méthode à appeler et un paramètre
                button.onClick.AddListener(() => SetInfoEntry(Entry));
            }
        }

        Vector2 currentSize = ScrollViewBtnEntryContent.GetComponent<RectTransform>().sizeDelta;

        // Modifier la hauteur
        currentSize.y = ContentHeight;

        // Appliquer la nouvelle taille
        ScrollViewBtnEntryContent.GetComponent<RectTransform>().sizeDelta = currentSize;
    }

    public void SetActiveInfoEntryParent(bool value)
    {
        LblEntryParent.SetActive(value);
    }
    public void SetInfoEntry(EntryData entryData)
    {
        Debug.Log("coucou : " + entryData.Distance + " " + entryData.NbExit + " " + entryData.EntryFitness);
        SetActiveInfoEntryParent(true);
        LblEntry.GetComponent<Text>().text = entryData.Nom;
        LblDistance.GetComponent<Text>().text = "Distance : " + entryData.Distance.ToString();
        LblNbExit.GetComponent<Text>().text = "Nb sortie : " + entryData.NbExit.ToString();
        LblFitness.GetComponent<Text>().text = "Fitness : " + entryData.EntryFitness.ToString();

        // Affiché le bon tracé en vert
        AutomaticManager.AfficherTrace(entryData.Nom);
    }

    public void SetScrollViewBtnEntry(bool value)
    {
        ScrollViewBtnEntry.SetActive(value);
    }

    public void ClearAllInfoModeAuto()
    {
        SetActiveInfoEntryParent(false);
        SetScrollViewBtnEntry(false);
        AutomaticManager.LstFolderIaClear();
        // CLear la list des boutons
        ClearScrollViewContent();
    }

    public void ClearScrollViewContent()
    {
        foreach (GameObject button in LstBtnScrollView)
        {
            Destroy(button.gameObject);
        }

        LstBtnScrollView.Clear();
    }
}

