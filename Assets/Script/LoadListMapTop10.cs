/** 
***********************************************************************
Auteur : Sam Freddi
Date : 26.03.2024
Description : LoadListMapSave.cs ce script permet de récuperer tous les fichier situer dans un dossier. Et les afficher sous la forme de bouton 
version 1.0
***********************************************************************
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;
using System.IO;
using UnityEngine.Scripting;
using Unity.VisualScripting;

public class LoadListMapTop10 : MonoBehaviour
{
    private string _folderPath; // Chemin du dossier dont vous voulez récupérer les noms de fichiers

    [SerializeField]
    private GameObject _parentFolder;
    [SerializeField]
    private GameObject _prefabButton;
    private float _contentHeight;
    private FileSystemWatcher fsWatch = new FileSystemWatcher();
    private bool _refresh = false;
    [SerializeField]
    private List<GameObject> _lstMapButton;
    public string FolderPath { get => _folderPath; private set => _folderPath = value; }
    public GameObject ParentFolder { get => _parentFolder; private set => _parentFolder = value; }
    public GameObject PrefabButton { get => _prefabButton; private set => _prefabButton = value; }
    public float ContentHeight { get => _contentHeight; private set => _contentHeight = value; }
    public List<GameObject> LstMapButton { get => _lstMapButton; set => _lstMapButton = value; }
    public bool Refresh { get => _refresh; set => _refresh = value; }

    void Start()
    {
        FolderPath = Application.persistentDataPath + "/Top10/";
        FetchFileNames();
        fsWatch.Path = FolderPath;
        fsWatch.IncludeSubdirectories = true;
        fsWatch.EnableRaisingEvents = true;
        fsWatch.Changed += FsWatch_Changed;
        fsWatch.Created += FsWatch_Created;
        fsWatch.Deleted += FsWatch_Deleted;
        fsWatch.Renamed += FsWatch_Renamed;
    }

    void Update()
    {
        if (Refresh)
        {
            Refresh = false;
            RefreshList();
        }
    }

    /// <summary>
    /// Récupère les noms des fichiers dans un dossier spécifié et génère des boutons pour chaque fichier,
    /// avec des fonctionnalités pour charger une scène associée au fichier.
    /// </summary>
    public void FetchFileNames()
    {
        if (Directory.Exists(FolderPath))
        {
            string[] fileNames = Directory.GetFiles(FolderPath);

            Debug.Log("creation");
            ContentHeight = 0;
            LstMapButton = new List<GameObject>();
            //   var sortedDirectories = fileNames.OrderBy(d => d, new NaturalSortComparer());


            foreach (string fileName in fileNames)
            {
                // Instancier le bouton à partir du prefab
                GameObject buttonGO = Instantiate(PrefabButton, new Vector3(0, 0, 0), Quaternion.identity, ParentFolder.transform);

                // D�finir le parent du bouton
                buttonGO.transform.SetParent(ParentFolder.transform, false); // Ne pas conserver la rotation et l'échelle du parent
                LstMapButton.Add(buttonGO);
                Text buttonText = buttonGO.GetComponentInChildren<Text>();
                if (buttonText != null)
                {
                    buttonText.text = Path.GetFileName(fileName);

                    // Ajuster la taille du bouton pour correspondre à la taille du texte
                    RectTransform buttonRect = buttonGO.GetComponent<RectTransform>();
                    buttonRect.sizeDelta = new Vector2(buttonText.preferredWidth + 20, buttonText.preferredHeight + 20);
                    ContentHeight += buttonRect.sizeDelta.y + 5;
                }
                // Acc�der au composant Button du bouton et ajouter une fonction à appeler avec un paramètre
                Button button = buttonGO.GetComponent<Button>();
                if (button != null)
                {
                    // Ajouter un écouteur d'évènement au bouton avec une méthode à appeler et un paramètre
                    button.onClick.AddListener(() => gameObject.GetComponent<ManagerScene>().LoadScene("Main_Simulation", Path.GetFileName(fileName), "Top10"));
                }

                FileInfo fi = new FileInfo(fileName);
                Debug.Log("info " + fi.LastWriteTime.ToShortDateString());
            }

            Vector2 currentSize = ParentFolder.GetComponent<RectTransform>().sizeDelta;

            // Modifier la hauteur
            currentSize.y = ContentHeight;

            // Appliquer la nouvelle taille
            ParentFolder.GetComponent<RectTransform>().sizeDelta = currentSize;
        }
        else
        {
            Debug.LogError("Le dossier spécifié n'existe pas : " + FolderPath);
        }
    }

    /// <summary>
    /// Supprime un fichier spécifié s'il existe.
    /// </summary>
    /// <param name="filePath">Le chemin du fichier à supprimer.</param>
    public void DeleteFile(string filePath)
    {
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
            Debug.Log("Fichier supprimé : " + filePath);
        }
        else
        {
            Debug.LogWarning("Le fichier n'existe pas : " + filePath);
        }
    }

    /// <summary>
    /// Supprime tous les fichiers présents dans le dossier spécifié, s'il existe.
    /// </summary>
    public void DeleteAllFilesInFolder()
    {
        if (Directory.Exists(FolderPath))
        {
            string[] files = Directory.GetFiles(FolderPath);
            foreach (string file in files)
            {
                File.Delete(file);
                Debug.Log("Fichier supprimé : " + file);
            }
            RefreshList();
        }
        else
        {
            Debug.LogWarning("Le dossier n'existe pas : " + FolderPath);
        }
    }

    /// <summary>
    /// Efface tous les éléments de la liste de boutons associés aux fichiers, et libère la mémoire non utilisée.
    /// </summary>
    public void ClearList()
    {
        for (int i = LstMapButton.Count - 1; i >= 0; i--)
        {
            Destroy(LstMapButton[i].gameObject);
        }
        GC.Collect(); // Garbage collector : Libére la mémoire des objets qui ont été déruit
    }

    /// <summary>
    /// Rafraîchit la liste des fichiers en effaçant tous les éléments existants et en rechargeant la liste.
    /// </summary>
    public void RefreshList()
    {
        Debug.Log("Refresh list");
        ClearList();
        FetchFileNames();
    }

    /// <summary>
    /// Gère l'événement de renommage d'un fichier surveillé par FileSystemWatcher.
    /// </summary>
    /// <param name="sender">L'objet à l'origine de l'événement.</param>
    /// <param name="e">Les données de l'événement RenamedEventArgs.</param>
    private void FsWatch_Renamed(object sender, RenamedEventArgs e)
    {
        Debug.Log($"Fichier {e.OldName} renommé {e.Name}");
        Refresh = true;
    }

    /// <summary>
    /// Gère l'événement de suppression d'un fichier surveillé par FileSystemWatcher.
    /// </summary>
    /// <param name="sender">L'objet à l'origine de l'événement.</param>
    /// <param name="e">Les données de l'événement FileSystemEventArgs.</param>
    private void FsWatch_Deleted(object sender, FileSystemEventArgs e)
    {
        Debug.Log($"Fichier {e.Name} supprimé");
        Refresh = true;
    }

    /// <summary>
    /// Gère l'événement de création d'un fichier surveillé par FileSystemWatcher.
    /// </summary>
    /// <param name="sender">L'objet à l'origine de l'événement.</param>
    /// <param name="e">Les données de l'événement FileSystemEventArgs.</param>
    private void FsWatch_Created(object sender, FileSystemEventArgs e)
    {
        Debug.Log($"Fichier {e.Name} créé");
        Refresh = true;
    }

    /// <summary>
    /// Gère l'événement de modification d'un fichier surveillé par FileSystemWatcher.
    /// </summary>
    /// <param name="sender">L'objet à l'origine de l'événement.</param>
    /// <param name="e">Les données de l'événement FileSystemEventArgs.</param>
    private void FsWatch_Changed(object sender, FileSystemEventArgs e)
    {
        Debug.Log($"Fichier {e.Name} modifié ");
        Refresh = true;
    }


}
