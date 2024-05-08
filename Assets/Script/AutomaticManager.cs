using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;
using System.IO;
using Unity.VisualScripting;

public class AutomaticManager : MonoBehaviour
{

    [SerializeField]
    private ManagerUI _managerUI;
    [SerializeField]
    private RandomGeneration _randomGeneration;

    [SerializeField]
    private MapManager _mapManager;
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
    public MapManager MapManager { get => _mapManager; set => _mapManager = value; }

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

        // Récuperer les 10 meileurs carte
        // Regardeer si la notre est dans le top 10
        // si top 10 = save notre carte et jeter l'ancienne 10ème
        CheckFitnessTop10(CalculerFitnessCarte());
    }

    public void CheckFitnessTop10(float fitness)
    {
        List<MapData> listMaze = new List<MapData>();
        listMaze = GetAllTop10Maze();
        MapData mapData = new MapData();
        if (listMaze.Count < 10)
        {
            MapManager.AddBlocksToMapData(mapData);
            mapData.SetFitness(CalculerFitnessCarte());
            // Si la liste est moins de 10, simplement ajouter le nouveau labyrinthe
            listMaze.Add(mapData);
            SortAndSave(listMaze);
            return;
        }

        // Comparer la fitness du nouveau labyrinthe avec les labyrinthes actuels
        foreach (var labyrinthe in listMaze)
        {
            MapManager.AddBlocksToMapData(mapData);
            mapData.SetFitness(CalculerFitnessCarte());
            if (mapData.Fitness > labyrinthe.Fitness)
            {

                // Remplacer le labyrinthe actuel par le nouveau labyrinthe
                listMaze.Remove(labyrinthe);
                listMaze.Add(mapData);
                SortAndSave(listMaze);
                return;
            }
        }
    }

    private void SortAndSave(List<MapData> lstMazeToSave)
    {
        DeleteAllFilesInFolder();
        lstMazeToSave.Sort((x, y) => y.Fitness.CompareTo(x.Fitness));
        // Code pour sauvegarder la liste des 10 meilleurs labyrinthes
        int i = 0;
        Debug.Log("COUNT " + lstMazeToSave.Count);
        foreach (var maze in lstMazeToSave)
        {

            SaveMapTop10("Top " + (i + 1) + " - " + maze.Fitness, maze);
            i++;
        }
    }

    /// <summary>
    /// Supprime tous les fichiers présents dans le dossier spécifié, s'il existe.
    /// </summary>
    public void DeleteAllFilesInFolder()
    {
        if (Directory.Exists(Application.persistentDataPath + "/Top10/"))
        {
            string[] files = Directory.GetFiles(Application.persistentDataPath + "/Top10/");
            foreach (string file in files)
            {
                File.Delete(file);
                Debug.Log("Fichier supprimé : " + file);
            }
        }
        else
        {
            Debug.LogWarning("Le dossier n'existe pas : " + Application.persistentDataPath + "/Top10/");
        }
    }
    public void AjouterLabyrinthe(float nouveauLabyrinthe)
    {

    }

    public void SaveMapTop10(string name, MapData mapData)
    {
        string folderPath = "top";
        MapManager.SaveMap(name, mapData, folderPath);
    }

    public List<MapData> GetAllTop10Maze()
    {
        List<MapData> listMaze = new List<MapData>();
        string folderPath = Application.persistentDataPath + "/Top10/";
        if (Directory.Exists(folderPath))
        {
            string[] fileNames = Directory.GetFiles(folderPath);

            Debug.Log("get");
            foreach (string fileName in fileNames)
            {
                // Récuper la fitness des carte
                MapData _mapToLoad = gameObject.GetComponent<MapManager>().LoadMap(Path.GetFileName(fileName), folderPath);
                listMaze.Add(_mapToLoad);
            }
        }
        else
        {
            Debug.LogError("Le dossier spécifié n'existe pas : " + folderPath);
        }

        return listMaze;
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
