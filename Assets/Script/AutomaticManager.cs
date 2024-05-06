using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AutomaticManager : MonoBehaviour
{

    [SerializeField]
    private ManagerUI _managerUI;
    [SerializeField]
    private RandomGeneration _randomGeneration;
    [SerializeField]
    private List<EntryData> _lstAllEntryData = new List<EntryData>();
    [SerializeField]
    private List<GameObject> _allEntry;

    private List<GameObject> _lstFolderIa = new List<GameObject>();

    private GameObject _currentTraceActive;

    public ManagerUI ManagerUI { get => _managerUI; set => _managerUI = value; }
    public RandomGeneration RandomGeneration { get => _randomGeneration; set => _randomGeneration = value; }
    public List<GameObject> AllEntry { get => _allEntry; private set => _allEntry = value; }
    public List<EntryData> LstAllEntryData { get => _lstAllEntryData; set => _lstAllEntryData = value; }
    public List<GameObject> LstFolderIa { get => _lstFolderIa; set => _lstFolderIa = value; }

    public void StartModeAuto()
    {
        RandomGeneration.ClearAndUpdateAllMapInfo();
        AllEntry = RandomGeneration.LstEntre.ToList(); // Tolist copie la list et ne copie pas seulement la référence mémoire
        StartCoroutine(StartAlgo());
    }

    public IEnumerator StartAlgo()
    {
        LstAllEntryData.Clear();

        for (var i = 0; i < AllEntry.Count; i++)
        {
            bool tourFinish = false;
            bool isStarted = false;
            ManagerUI.ClearNbExitFound();

            RandomGeneration.AjouterEntre(AllEntry[i].transform, 3);

            while (!tourFinish)
            {
                ManagerUI.SetTexBoxText("les bots test l'entrée numéro " + (i + 1));


                yield return new WaitForSeconds(0.02f);
                if (!isStarted) // si le bot n'as pas encore commencé on le fait spawn
                {
                    GameObject.FindWithTag("Enter").GetComponent<Enter2_PathfinderNewDirection>().StartPathfinder();
                    isStarted = true;
                }

                if (ManagerUI.CheckIfAllBotHaveFinish())
                {
                    tourFinish = true;
                }
            }


            Debug.Log("Tour " + i + " finit ");
            EntryData entryData = new EntryData("Entrée No: " + (i + 1), AllEntry[i], ManagerUI.NbExitFound, ManagerUI.GetAllDistanceFromBot(), GetEntryFitness());
            LstAllEntryData.Add(entryData);

            // Save tous les bot de cette entré 
            SaveTrace(i);
            yield return new WaitForSeconds(1f);
            RandomGeneration.ClearMapBorder();

            ManagerUI.ClearAllPathinder();
            ManagerUI.ClearMapInfo();

            RandomGeneration.AllBlock.Clear();
            RandomGeneration.AddAllBlocFromMaze();

            RandomGeneration.LstEntre.Clear();
            RandomGeneration.GetAllBlocNotConnected();
        }

        Debug.Log("Algo 3 finit ");
        AlgoFinish();
    }

    public void SaveTrace(int numero)
    {
        GameObject saveTrace = new GameObject();
        saveTrace.name = "Entrée No: " + (numero + 1);
        LstFolderIa.Add(saveTrace);
        // foreach (Transform botToTransfer in ManagerUI.IAFolder.transform)
        // {
        //     botToTransfer.transform.parent = saveTrace.transform;
        // }
        for (var i = ManagerUI.IAFolder.transform.childCount - 1; i >= 0; i--)
        {
            ManagerUI.IAFolder.transform.GetChild(i).transform.parent = saveTrace.transform;
        }

        HideTrace(saveTrace);
    }

    public void HideTrace(GameObject IAFolder)
    {
        foreach (Transform bot in IAFolder.transform)
        {
            bot.gameObject.SetActive(false);
            bot.GetComponent<Pathfinding1>().enabled = false;
            // bot.gameObject.GetComponent<Pathfinding1>().StopMovement();
            // bot.gameObject.GetComponent<Pathfinding1>().BlockPathfinderForSave();
            // bot.gameObject.GetComponent<Pathfinding1>().BlockDuplication();
        }
    }

    public void AfficherTrace(string name)
    {
        if (_currentTraceActive != null)
        {
            ClearCurrentTrace();
        }

        GameObject IaFolder = GameObject.Find(name);
        _currentTraceActive = IaFolder;
        foreach (Transform bot in IaFolder.transform)
        {
            bot.gameObject.SetActive(true);
            bot.gameObject.GetComponent<TrailRenderer>().enabled = true;
            bot.gameObject.GetComponent<MeshRenderer>().enabled = true;
        }
    }

    public void ClearCurrentTrace()
    {
        foreach (Transform bot in _currentTraceActive.transform)
        {
            bot.gameObject.SetActive(false);
        }
    }

    public void AlgoFinish()
    {
        ManagerUI.SetBtnInformation(true);
        ManagerUI.AlgoEnCours = false;
        ManagerUI.SetTexBoxText("L'algorithme automatique a finis de générer un score pour la carte");
        ManagerUI.BtnRestartGenerator.gameObject.SetActive(true);
        ManagerUI.StopTimer();
        ManagerUI.SetBtnPause(false);
        ManagerUI.OpenModalInformation();
        ManagerUI.SetBtnInformation(true);
        ManagerUI.SetScrollViewBtnEntry(true);
        ManagerUI.SetBtnInScrollView();
        string text = "Nombre de sorti trouvé total: " + GetAllExitFound().ToString();
        ManagerUI.SetPanelTextInformation(text);
        ManagerUI.SetDistanceInfoText("Score total du labyrinthe : " + CalculerFitnessCarte());
    }


    public float GetAllExitFound()
    {
        float allExit = 0;
        foreach (var info in LstAllEntryData)
        {
            allExit += info.NbExit;
        }
        return allExit;
    }
    public float GetEntryFitness()
    {
        float fitness = 0;
        fitness += ManagerUI.GetAllDistanceFromBot() * 1;
        fitness += ManagerUI.NbExitFound * 10;
        fitness += ManagerUI.Chronometre.GetElapsedTime() * 5;
        return fitness;
    }

    public float CalculerFitnessCarte()
    {
        float fitness = 0;
        foreach (var entry in LstAllEntryData)
        {
            fitness += entry.EntryFitness;
        }
        return fitness;
    }

    public void LstFolderIaClear()
    {
        for (int i = 0; i < LstFolderIa.Count; i++)
        {
            GameObject FolderDestroy = GameObject.Find(LstFolderIa[i].name);
            DestroyImmediate(FolderDestroy);
        }
        LstFolderIa.Clear();
    }
}
