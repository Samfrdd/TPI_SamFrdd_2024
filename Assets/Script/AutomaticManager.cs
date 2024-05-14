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

    /// <summary>
    /// Démarre le mode automatique de l'algorithme.
    /// </summary>
    public void StartModeAuto()
    {
        RandomGeneration.ClearAndUpdateAllMapInfo();
        AllEntry = RandomGeneration.LstEntre.ToList(); // Tolist copie la list et ne copie pas seulement la référence mémoire
        StartCoroutine(StartAlgo());
    }

    /// <summary>
    /// Démarre l'exécution de l'algorithme.
    /// </summary>
    /// <returns>Un énumérateur pour l'exécution asynchrone.</returns>
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

    /// <summary>
    /// Enregistre la trace de l'algorithme pour une entrée spécifique.
    /// </summary>
    /// <param name="numero">Le numéro de l'entrée.</param>
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

    /// <summary>
    /// Masque la trace de l'algorithme en désactivant les objets dans le dossier spécifié.
    /// </summary>
    /// <param name="IAFolder">Le dossier contenant les objets à masquer.</param>
    public void HideTrace(GameObject IAFolder)
    {
        foreach (Transform bot in IAFolder.transform)
        {
            bot.gameObject.SetActive(false);
            bot.GetComponent<Pathfinding1>().enabled = false;
        }
    }


    /// <summary>
    /// Affiche la trace de l'algorithme en activant les objets dans le dossier spécifié.
    /// </summary>
    /// <param name="name">Le nom du dossier contenant les objets à afficher.</param>
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

    /// <summary>
    /// Efface la trace actuellement affichée en désactivant les objets dans le dossier de trace actif.
    /// </summary>
    public void ClearCurrentTrace()
    {
        foreach (Transform bot in _currentTraceActive.transform)
        {
            bot.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Indique que l'algorithme est terminé, effectue diverses actions finales et affiche les informations de fin.
    /// </summary>
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

    /// <summary>
    /// Vérifie si le score du nouveau labyrinthe est dans le top 10 des scores enregistrés. Si oui, le sauvegarde.
    /// </summary>
    /// <param name="fitness">Le score du nouveau labyrinthe.</param>
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

    /// <summary>
    /// Trie la liste des labyrinthes par score de fitness décroissant et sauvegarde les 10 meilleurs.
    /// </summary>
    /// <param name="lstMazeToSave">La liste des labyrinthes à sauvegarder.</param>
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


    /// <summary>
    /// Sauvegarde un labyrinthe dans le dossier des 10 meilleurs avec un nom spécifié.
    /// </summary>
    /// <param name="name">Le nom du labyrinthe.</param>
    /// <param name="mapData">Les données du labyrinthe à sauvegarder.</param>
    public void SaveMapTop10(string name, MapData mapData)
    {
        string folderPath = "top";
        MapManager.SaveMap(name, mapData, folderPath);
    }

    /// <summary>
    /// Récupère et retourne la liste des données des labyrinthes figurant dans le top 10.
    /// </summary>
    /// <returns>La liste des données des labyrinthes du top 10.</returns>
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

    /// <summary>
    /// Calcule et retourne le nombre total de sorties trouvées dans toutes les données d'entrée enregistrées.
    /// </summary>
    /// <returns>Le nombre total de sorties trouvées.</returns>
    public float GetAllExitFound()
    {
        float allExit = 0;
        foreach (var info in LstAllEntryData)
        {
            allExit += info.NbExit;
        }
        return allExit;
    }

    /// <summary>
    /// Calcule et retourne le score de fitness pour une entrée spécifique, en fonction de différents critères.
    /// </summary>
    /// <returns>Le score de fitness de l'entrée.</returns>
    public float GetEntryFitness()
    {
        float fitness = 0;
        fitness += ManagerUI.GetAllDistanceFromBot() * 1;
        fitness += ManagerUI.NbExitFound * 10;
        fitness += ManagerUI.Chronometre.GetElapsedTime() * 5;
        return fitness;
    }


    /// <summary>
    /// Calcule et retourne le score de fitness total de la carte en additionnant les scores de fitness de toutes les entrées.
    /// </summary>
    /// <returns>Le score de fitness total de la carte.</returns>
    public float CalculerFitnessCarte()
    {
        float fitness = 0;
        foreach (var entry in LstAllEntryData)
        {
            fitness += entry.EntryFitness;
        }
        return fitness;
    }

    /// <summary>
    /// Efface tous les dossiers contenant les données des IA.
    /// </summary>
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
