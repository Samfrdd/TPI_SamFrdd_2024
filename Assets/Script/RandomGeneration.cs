/** 
***********************************************************************
Auteur : Sam Freddi
Date : 26.03.2024
Description. RandomGeneration.cs sctipt de génération de la carte. Génération visuel
version 1.0
***********************************************************************
*/


using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;

using System;

using System.IO;
using System.Xml.Serialization;
using Unity.VisualScripting;
using UnityEditor;

public class RandomGeneration : MonoBehaviour
{
    // Script de génération de la carte
    #region variable
    public bool debug; // VARIBALE 
    private bool _mapCree = false;
    [SerializeField]
    private bool _pause;

    #endregion

    #region dossierParent

    [SerializeField]
    private Transform _folderBlocParent;
    [SerializeField]
    private Transform _folderIaParent;
    [SerializeField]
    private Canvas _canvas;

    #endregion

    #region mazeBloc

    [SerializeField]
    private List<GameObject> _lstBlockMaze;     //Liste des blocs pour former le terrain
    [SerializeField]
    private GameObject _blockEnter;     //Liste des blocs pour former le terrain
    [SerializeField]
    private GameObject _blockExit; //Liste des blocs pour former le terrain
    [SerializeField]
    private GameObject _blocSensUnique; // ?? Nom anglais 
    [SerializeField]
    private GameObject _blocFermer; // ?? Nom anglais 
    [SerializeField]
    private List<float> _lstRotationBlock; //Liste des rotations possible
    [SerializeField]
    private Vector2 _terrainSize = new Vector2(100f, 100f); // Taille du terrain
    [SerializeField]
    private Vector2 _blockSize = new Vector2(10f, 10f); //Taille des blocs 
    [SerializeField]
    private int _rows = 10; // Nombre de lignes
    [SerializeField]
    private int _columns = 10; // Nombre de colonnes
    [SerializeField]
    private GameObject _entreChoisi; // Entré choisi par l'utilisateur
    [SerializeField]
    private GameObject _sortiChoisi; // Entré choisi par l'utilisateur
    private List<GameObject> _allBlock = new List<GameObject>(); // List de tous les bloc instancié
    [SerializeField]
    private List<GameObject> _lstEntre; // Liste de toute les entré et sorti possible
    [SerializeField]
    private List<GameObject> _lstAllBlocNotConnected; // Une list pour tester tous les blocs qui n'arrive pas a se connecter

    #endregion

    #region displayUser
    [SerializeField]
    private GameObject _btnPrefab; // prefab du button

    [SerializeField]
    private ManagerUI _managerUI;
    #endregion

    public bool MapCree { get => _mapCree; private set => _mapCree = value; }
    public List<GameObject> LstEntre { get => LstEntre1; set => LstEntre1 = value; }
    public bool Pause { get => _pause; set => _pause = value; }
    public Transform FolderBlocParent { get => _folderBlocParent; set => _folderBlocParent = value; }
    public Transform FolderIaParent { get => _folderIaParent; set => _folderIaParent = value; }
    public Canvas Canvas { get => _canvas; set => _canvas = value; }
    public List<GameObject> LstBlockMaze { get => _lstBlockMaze; set => _lstBlockMaze = value; }
    public GameObject BlockEnter { get => _blockEnter; set => _blockEnter = value; }
    public GameObject BlockExit { get => _blockExit; set => _blockExit = value; }
    public GameObject BlocSensUnique { get => _blocSensUnique; set => _blocSensUnique = value; }
    public GameObject BlocFermer { get => _blocFermer; set => _blocFermer = value; }
    public List<float> LstRotationBlock { get => _lstRotationBlock; set => _lstRotationBlock = value; }
    public Vector2 TerrainSize { get => _terrainSize; set => _terrainSize = value; }
    public Vector2 BlockSize { get => _blockSize; set => _blockSize = value; }
    public int Rows { get => _rows; set => _rows = value; }
    public int Columns { get => _columns; set => _columns = value; }
    public GameObject EntreChoisi { get => _entreChoisi; set => _entreChoisi = value; }
    public GameObject SortiChoisi { get => _sortiChoisi; set => _sortiChoisi = value; }
    public List<GameObject> AllBlock { get => _allBlock; set => _allBlock = value; }
    public List<GameObject> LstEntre1 { get => _lstEntre; set => _lstEntre = value; }
    public List<GameObject> LstAllBlocNotConnected { get => _lstAllBlocNotConnected; set => _lstAllBlocNotConnected = value; }
    public GameObject BtnPrefab { get => _btnPrefab; set => _btnPrefab = value; }
    public ManagerUI ManagerUI { get => _managerUI; set => _managerUI = value; }


    void Start()
    {
        // Au start de la scene
        FolderIaParent = GameObject.FindWithTag("IA").transform;
    }

    public void StartGeneation()
    {
        StartCoroutine(GenerationMap());
    }
    IEnumerator GenerationMap()
    {
        ManagerUI.BtnStart.gameObject.SetActive(false);
        ManagerUI.BtnRestartGenerator.enabled = false;
        ManagerUI.BtnRestartGenerator.gameObject.SetActive(false);
        ManagerUI.BtnSave.gameObject.SetActive(false);

        ManagerUI.SetTexBoxText("Génération en cours...");

        float nbBlocTotal = Rows * Columns;
        float numeroBloc = 0;

        // Position de départ des blocs
        float startX = -(TerrainSize.x / 2) + (BlockSize.x / 2);
        float startZ = -(TerrainSize.y / 2) + (BlockSize.y / 2);
        AllBlock = new List<GameObject>();
        LstEntre = new List<GameObject>();

        GameObject previousBlock = null;
        int nbTentative = 0;

        for (int row = 0; row < Rows; row++)
        {
            for (int col = 0; col < Columns; col++)
            {


                bool blockGood = false;
                while (!blockGood)
                {
                    nbTentative++;
                    float x = startX + col * BlockSize.x;
                    float z = startZ + row * BlockSize.y;
                    Vector3 position = new Vector3(x, 0f, z);
                    GameObject block;
                    List<GameObject> lstConnecteur = new List<GameObject>();


                    if (previousBlock != null)
                    {
                        bool ConnecteurAllFalse;
                        List<string> listString = new List<string>();

                        // On instancie le nous block qui va �tre ajout� 
                        GameObject selectedRoadBlockPrefab = LstBlockMaze[UnityEngine.Random.Range(0, LstBlockMaze.Count)];
                        block = Instantiate(selectedRoadBlockPrefab, position, Quaternion.identity);
                        block.transform.parent = FolderBlocParent;
                        block.transform.Rotate(0f, LstRotationBlock[UnityEngine.Random.Range(0, LstRotationBlock.Count)], 0f);
                        block.name = block.name + "_Row:" + row + "_Col:" + col;

                        // Pause de 0.02 seconde pour que les connections ont le temps de s'être faite 
                        yield return new WaitForSeconds(0.02f);

                        // On ajoute les connecteurs dans la liste                                  
                        int childCount = block.transform.childCount;

                        for (int i = 0; i < childCount; i++)
                        {
                            Transform child = block.transform.GetChild(i);
                            if (child.CompareTag("Connecteur") || child.CompareTag("mauvais"))
                            {
                                lstConnecteur.Add(child.gameObject);
                            }
                        }

                        // On ajoute dans la liste toute les variables pour savoir si le bloc est autorisé a être connecté
                        foreach (GameObject connectedCube in lstConnecteur)
                        {
                            if (connectedCube.CompareTag("Connecteur") || connectedCube.CompareTag("mauvais"))
                            {
                                // Ajout de la variable Connected dans la list de string
                                listString.Add(connectedCube.GetComponent<connecteur>().Connected);
                            }

                        }

                        // Test si 1 des connecteurs a une mauvaise connection
                        ConnecteurAllFalse = TestIfBadConnection(listString);

                        // Verifie le bloc est legit on le pose sinon ou le d�truit
                        if (ConnecteurAllFalse)
                        {
                            previousBlock = block;
                            blockGood = true;
                            nbTentative = 0;
                        }
                        else
                        {
                            DestroyImmediate(block);
                        }

                    }
                    else
                    {
                        // Connecter le point de sortie du bloc à un point de sortie en dehors de la zone de jeu
                        GameObject selectedRoadBlockPrefab = LstBlockMaze[0];
                        block = Instantiate(selectedRoadBlockPrefab, position, Quaternion.identity);
                        block.transform.Rotate(0f, 90f, 0f);
                        block.transform.parent = FolderBlocParent;
                        block.name = block.name + "_" + row + "_" + col + " FIRST";
                        previousBlock = block;
                        blockGood = true;
                        yield return new WaitForSeconds(0.02f);
                    }


                }

                numeroBloc++;
                ManagerUI.SetTexBoxText("Génération en cours : " + (numeroBloc / nbBlocTotal * 100f) + "%");
            }

        }


        DestroyAllGameObjectEmpty();

        AddAllBlocFromMaze();

        GetAllBlocNotConnected();

        ManagerUI.SetBtnChoice(true);
        ManagerUI.SetBtnSave();
        // generateBtnEnter();
        ManagerUI.BtnRestartGenerator.enabled = true;
        ManagerUI.BtnRestartGenerator.gameObject.SetActive(true);

        print("La map a été générer !");
    }

    // Fonctions qui ajoute tous les blocs de la map dans la liste AllBlock
    public void AddAllBlocFromMaze()
    {
        int nombreEnfants = FolderBlocParent.childCount;
        for (int i = 0; i < nombreEnfants; i++)
        {
            Transform enfant = FolderBlocParent.GetChild(i);
            AllBlock.Add(enfant.gameObject);
        }
    }

    // Fonctions qui permets de détruire tous les gameObject vide
    public void DestroyAllGameObjectEmpty()
    {
        // Détruire tous les gameObject vide de la scene 
        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();
        foreach (GameObject obj in allObjects)
        {
            if (obj.name == "New Game Object" && obj.transform.childCount == 0)
            {
                Destroy(obj);
            }
        }
    }

    // Fonctions qui nous pemets de récuéperer toute les entrés 
    public void GetAllBlocNotConnected()
    {
        LstEntre.Clear();
        LstAllBlocNotConnected.Clear();
        for (int i = 0; i < AllBlock.Count; i++)
        {
            int childCount = AllBlock[i].transform.childCount;
            for (int y = 0; y < childCount; y++)
            {
                Transform child = AllBlock[i].transform.GetChild(y);
                if (child.CompareTag("Connecteur"))
                {
                    if (child.GetComponent<connecteur>().Connected == "pasConnecte")
                    {
                        LstEntre.Add(child.gameObject);
                        LstAllBlocNotConnected.Add(child.gameObject);
                    }
                }
                else if (child.CompareTag("mauvais"))
                {
                    if (child.GetComponent<connecteur>().Connected == "pasConnecte")
                    {
                        LstAllBlocNotConnected.Add(child.gameObject);
                    }
                }
            }
        }
    }



    // Fonctions qui test si les blocs sont connecté
    private bool TestIfBadConnection(List<string> listString)
    {
        bool ConnecteurAllFalse;

        if (listString.Any(b => b == "mauvaiseConnection"))
        {
            ConnecteurAllFalse = false;
        }
        else
        {

            ConnecteurAllFalse = true;
        }

        return ConnecteurAllFalse;
    }

    // Fonctions qui génère tous les boutons pour placer une entre
    public void generateBtnEnter(int mode)
    {
        ManagerUI.SetTexBoxText("Veuillez sélectionner une entré !");


        int index = 0;

        // Créer et placer les boutons dynamiquement sur les GameObjects existants
        foreach (GameObject targetObject in LstEntre1)
        {
            index++;
            // Convertir la position du GameObject en coordonnées d'écran
            Vector3 screenPos = Camera.main.WorldToScreenPoint(targetObject.transform.position);

            // Instancier le bouton à partir du prefab
            GameObject buttonGO = Instantiate(BtnPrefab, screenPos, Quaternion.identity, this.Canvas.transform);

            // D�finir le parent du bouton
            buttonGO.transform.SetParent(this.Canvas.transform, false); // Ne pas conserver la rotation et l'échelle du parent

            Text buttonText = buttonGO.GetComponentInChildren<Text>();
            if (buttonText != null)
            {
                buttonText.text = "N:" + index;

                // Ajuster la taille du bouton pour correspondre à la taille du texte
                RectTransform buttonRect = buttonGO.GetComponent<RectTransform>();
                buttonRect.sizeDelta = new Vector2(buttonText.preferredWidth + 20, buttonText.preferredHeight + 20);
            }
            // Accéder au composant Button du bouton et ajouter une fonction à appeler avec un paramètre
            Button button = buttonGO.GetComponent<Button>();
            if (button != null)
            {
                // Ajouter un écouteur d'évènement au bouton avec une méthode à appeler et un paramètre
                button.onClick.AddListener(() => AjouterEntre(targetObject.transform, mode));
            }
        }
    }


    // Fonctions qui génère tous les boutons pour placer une sortie
    public void GenerateBtnExit(int mode)
    {
        ManagerUI.SetTexBoxText("Veuillez sélectionner une sortie !");

        int index = 0;
        List<GameObject> _lstExit = LstEntre1;
        _lstExit.Remove(EntreChoisi);


        foreach (GameObject targetObject in _lstExit)
        {
            index++;
            // Convertir la position du GameObject en coordonn�es d'�cran
            Vector3 screenPos = Camera.main.WorldToScreenPoint(targetObject.transform.position);

            // Instancier le bouton � partir du prefab
            GameObject buttonGO = Instantiate(BtnPrefab, screenPos, Quaternion.identity, this.Canvas.transform);

            // D�finir le parent du bouton
            buttonGO.transform.SetParent(this.Canvas.transform, false); // Ne pas conserver la rotation et l'�chelle du parent

            Text buttonText = buttonGO.GetComponentInChildren<Text>();
            if (buttonText != null)
            {
                buttonText.text = "N:" + index;

                // Ajuster la taille du bouton pour correspondre � la taille du texte
                RectTransform buttonRect = buttonGO.GetComponent<RectTransform>();
                buttonRect.sizeDelta = new Vector2(buttonText.preferredWidth + 20, buttonText.preferredHeight + 20);
            }

            // Acc�der au composant Button du bouton et ajouter une fonction � appeler avec un param�tre
            Button button = buttonGO.GetComponent<Button>();
            if (button != null)
            {
                // Ajouter un �couteur d'�v�nement au bouton avec une m�thode � appeler et un param�tre
                button.onClick.AddListener(() => AjouterExit(targetObject.transform, mode));
            }
        }
    }


    //Fonctions qui ajoute l'entré a la carte
    public void AjouterEntre(Transform entre, int mode)
    {
        GameObject block;
        EntreChoisi = entre.gameObject;

        Vector3 position = new Vector3();
        float rotation = 0f;

       
        if (entre.parent.gameObject.transform.position.z == 45 && entre.parent.gameObject.transform.position.x == 45)
        {

            // Obtenez la position de l'objet et du carre parent dans le referentiel mondial
            Vector3 objectPosition = entre.transform.position;
            Vector3 parentPosition = entre.parent.gameObject.transform.position;

            // Calculez la difference de position entre l'objet et le carre parent
            Vector3 difference = objectPosition - parentPosition;

            // Calculez l'angle entre le vecteur de difference et l'axe X (droite)
            float angle = Vector3.SignedAngle(difference, Vector3.right, Vector3.forward);

            // D�terminez la position relative en fonction de l'angle
            if (angle > -45 && angle <= 45)
            {
                // Debug.Log("L'objet est a droite du carre parent.");
                position = parentPosition + new Vector3(10, 0, 0);
                rotation = 90;
            }
            else if (angle > 45 && angle <= 135)
            {
                // Debug.Log("L'objet est en haut du carre parent.");
            }
            else if (angle > 135 || angle <= -135)
            {
                // Debug.Log("L'objet est a gauche du carre parent.");
            }
            else
            {
                // Debug.Log("L'objet est en bas du carre parent.");

                position = parentPosition + new Vector3(0, 0, 10);
                rotation = 0;
            }

        }
        else if (entre.parent.gameObject.transform.position.z == 45 && entre.parent.gameObject.transform.position.x == -45)
        {
            // Obtenez la position de l'objet et du carr� parent dans le r�f�rentiel mondial
            Vector3 objectPosition = entre.transform.position;
            Vector3 parentPosition = entre.parent.gameObject.transform.position;

            // Calculez la diff�rence de position entre l'objet et le carr� parent
            Vector3 difference = objectPosition - parentPosition;

            // Calculez l'angle entre le vecteur de diff�rence et l'axe X (droite)
            float angle = Vector3.SignedAngle(difference, Vector3.right, Vector3.forward);

            // D�terminez la position relative en fonction de l'angle
            if (angle > -45 && angle <= 45)
            {

            }
            else if (angle > 45 && angle <= 135)
            {
                // Debug.Log("L'objet est en haut du carr� parent.");
            }
            else if (angle > 135 || angle <= -135)
            {
                position = parentPosition + new Vector3(-10, 0, 0);
                rotation = -90;
            }
            else
            {
                //  Debug.Log("L'objet est en bas du carr� parent.");

                position = parentPosition + new Vector3(0, 0, 10);
                rotation = 0;
            }
        }
        else if (entre.parent.gameObject.transform.position.z == -45 && entre.parent.gameObject.transform.position.x == -45)
        {
            // Obtenez la position de l'objet et du carr� parent dans le r�f�rentiel mondial
            Vector3 objectPosition = entre.transform.position;
            Vector3 parentPosition = entre.parent.gameObject.transform.position;

            // Calculez la diff�rence de position entre l'objet et le carr� parent
            Vector3 difference = objectPosition - parentPosition;

            // Calculez l'angle entre le vecteur de diff�rence et l'axe X (droite)
            float angle = Vector3.SignedAngle(difference, Vector3.right, Vector3.forward);

            // D�terminez la position relative en fonction de l'angle
            if (angle > -45 && angle <= 45)
            {

            }
            else if (angle > 45 && angle <= 135)
            {
                // Debug.Log("L'objet est en haut du carr� parent.");
                position = parentPosition + new Vector3(0, 0, -10);
                rotation = 180;
            }
            else if (angle > 135 || angle <= -135)
            {
                position = parentPosition + new Vector3(-10, 0, 0);
                rotation = -90;
            }
            else
            {
                // Debug.Log("L'objet est en bas du carr� parent.");
                position = parentPosition + new Vector3(0, 0, -10);
                rotation = 180;

            }
        }
        else if (entre.parent.gameObject.transform.position.z == -45 && entre.parent.gameObject.transform.position.x == 45)
        {
            // Obtenez la position de l'objet et du carr� parent dans le r�f�rentiel mondial
            Vector3 objectPosition = entre.transform.position;
            Vector3 parentPosition = entre.parent.gameObject.transform.position;

            // Calculez la diff�rence de position entre l'objet et le carr� parent
            Vector3 difference = objectPosition - parentPosition;

            // Calculez l'angle entre le vecteur de diff�rence et l'axe X (droite)
            float angle = Vector3.SignedAngle(difference, Vector3.right, Vector3.forward);

            // D�terminez la position relative en fonction de l'angle
            if (angle > -45 && angle <= 45)
            {
                position = parentPosition + new Vector3(10, 0, 0);
                rotation = 90;
            }
            else if (angle > 45 && angle <= 135)
            {
                // Debug.Log("L'objet est en haut du carr� parent.");
                position = parentPosition + new Vector3(0, 0, -10);
                rotation = 180;
            }
            else if (angle > 135 || angle <= -135)
            {
                position = parentPosition + new Vector3(-10, 0, 0);
                rotation = -90;
            }
            else
            {
                //  Debug.Log("L'objet est en bas du carr� parent.");
                position = parentPosition + new Vector3(0, 0, -10);
                rotation = 180;

            }
        }
        else
        {
            Vector3 parentPosition = entre.parent.gameObject.transform.position;


            if (entre.parent.gameObject.transform.position.z == -45) // SUD
            {
                position = parentPosition + new Vector3(0, 0, -10);
                rotation = 180;
            }
            else if (entre.parent.gameObject.transform.position.z == 45) // NORD
            {
                position = parentPosition + new Vector3(0, 0, 10);
                rotation = 0;

            }
            else if (entre.parent.gameObject.transform.position.x == 45) // EST
            {
                position = parentPosition + new Vector3(10, 0, 0);
                rotation = 90;

            }
            else if (entre.parent.gameObject.transform.position.x == -45) // OUEST
            {
                position = parentPosition + new Vector3(-10, 0, 0);
                rotation = -90;

            }
        }


        block = Instantiate(BlockEnter, position, Quaternion.identity);
        block.name = BlockEnter.name;
        block.transform.parent = FolderBlocParent;
        block.transform.Rotate(0f, rotation, 0f);

        RemoveButton();
        if (mode == 1)
        {
            GenerateBtnExit(mode);
        }

        if (mode == 2)
        {
            StartCoroutine(CompleteMap(mode));
        }

    }

    //Foncions qui ajoute la sortie
    public void AjouterExit(Transform exit, int mode = 2)
    {
        GameObject block;
        Vector3 position = new Vector3();
        float rotation = 0f;
        Debug.Log(exit.gameObject.name);

        List<GameObject> _lstExit = LstEntre1;


        SortiChoisi = exit.gameObject;

        _lstExit.Remove(SortiChoisi);

        this.LstEntre1 = _lstExit;

        if (exit.parent.gameObject.transform.position.z == 45 && exit.parent.gameObject.transform.position.x == 45)
        {

            // Obtenez la position de l'objet et du carre parent dans le referentiel mondial
            Vector3 objectPosition = exit.transform.position;
            Vector3 parentPosition = exit.parent.gameObject.transform.position;

            // Calculez la difference de position entre l'objet et le carre parent
            Vector3 difference = objectPosition - parentPosition;

            // Calculez l'angle entre le vecteur de difference et l'axe X (droite)
            float angle = Vector3.SignedAngle(difference, Vector3.right, Vector3.forward);

            // D�terminez la position relative en fonction de l'angle
            if (angle > -45 && angle <= 45)
            {
                // Debug.Log("L'objet est a droite du carre parent.");
                position = parentPosition + new Vector3(10, 0, 0);
                rotation = 90;
            }
            else if (angle > 45 && angle <= 135)
            {
                // Debug.Log("L'objet est en haut du carre parent.");
            }
            else if (angle > 135 || angle <= -135)
            {
                // Debug.Log("L'objet est a gauche du carre parent.");
            }
            else
            {
                // Debug.Log("L'objet est en bas du carre parent.");

                position = parentPosition + new Vector3(0, 0, 10);
                rotation = 0;
            }

        }
        else if (exit.parent.gameObject.transform.position.z == 45 && exit.parent.gameObject.transform.position.x == -45)
        {
            // Obtenez la position de l'objet et du carr� parent dans le r�f�rentiel mondial
            Vector3 objectPosition = exit.transform.position;
            Vector3 parentPosition = exit.parent.gameObject.transform.position;

            // Calculez la diff�rence de position entre l'objet et le carr� parent
            Vector3 difference = objectPosition - parentPosition;

            // Calculez l'angle entre le vecteur de diff�rence et l'axe X (droite)
            float angle = Vector3.SignedAngle(difference, Vector3.right, Vector3.forward);

            // D�terminez la position relative en fonction de l'angle
            if (angle > -45 && angle <= 45)
            {

            }
            else if (angle > 45 && angle <= 135)
            {
                // Debug.Log("L'objet est en haut du carr� parent.");
            }
            else if (angle > 135 || angle <= -135)
            {
                position = parentPosition + new Vector3(-10, 0, 0);
                rotation = -90;
            }
            else
            {
                //  Debug.Log("L'objet est en bas du carr� parent.");

                position = parentPosition + new Vector3(0, 0, 10);
                rotation = 0;
            }
        }
        else if (exit.parent.gameObject.transform.position.z == -45 && exit.parent.gameObject.transform.position.x == -45)
        {
            // Obtenez la position de l'objet et du carr� parent dans le r�f�rentiel mondial
            Vector3 objectPosition = exit.transform.position;
            Vector3 parentPosition = exit.parent.gameObject.transform.position;

            // Calculez la diff�rence de position entre l'objet et le carr� parent
            Vector3 difference = objectPosition - parentPosition;

            // Calculez l'angle entre le vecteur de diff�rence et l'axe X (droite)
            float angle = Vector3.SignedAngle(difference, Vector3.right, Vector3.forward);

            // D�terminez la position relative en fonction de l'angle
            if (angle > -45 && angle <= 45)
            {

            }
            else if (angle > 45 && angle <= 135)
            {
                // Debug.Log("L'objet est en haut du carr� parent.");
                position = parentPosition + new Vector3(0, 0, -10);
                rotation = 180;
            }
            else if (angle > 135 || angle <= -135)
            {
                position = parentPosition + new Vector3(-10, 0, 0);
                rotation = -90;
            }
            else
            {
                // Debug.Log("L'objet est en bas du carr� parent.");
                position = parentPosition + new Vector3(0, 0, -10);
                rotation = 180;

            }
        }
        else if (exit.parent.gameObject.transform.position.z == -45 && exit.parent.gameObject.transform.position.x == 45)
        {
            // Obtenez la position de l'objet et du carr� parent dans le r�f�rentiel mondial
            Vector3 objectPosition = exit.transform.position;
            Vector3 parentPosition = exit.parent.gameObject.transform.position;

            // Calculez la diff�rence de position entre l'objet et le carr� parent
            Vector3 difference = objectPosition - parentPosition;

            // Calculez l'angle entre le vecteur de diff�rence et l'axe X (droite)
            float angle = Vector3.SignedAngle(difference, Vector3.right, Vector3.forward);

            // D�terminez la position relative en fonction de l'angle
            if (angle > -45 && angle <= 45)
            {
                position = parentPosition + new Vector3(10, 0, 0);
                rotation = 90;
            }
            else if (angle > 45 && angle <= 135)
            {
                // Debug.Log("L'objet est en haut du carr� parent.");
                position = parentPosition + new Vector3(0, 0, -10);
                rotation = 180;
            }
            else if (angle > 135 || angle <= -135)
            {
                position = parentPosition + new Vector3(-10, 0, 0);
                rotation = -90;
            }
            else
            {
                //  Debug.Log("L'objet est en bas du carr� parent.");
                position = parentPosition + new Vector3(0, 0, -10);
                rotation = 180;

            }
        }
        else
        {
            Vector3 parentPosition = exit.parent.gameObject.transform.position;


            if (exit.parent.gameObject.transform.position.z == -45) // SUD
            {
                position = parentPosition + new Vector3(0, 0, -10);
                rotation = 180;
            }
            else if (exit.parent.gameObject.transform.position.z == 45) // NORD
            {
                position = parentPosition + new Vector3(0, 0, 10);
                rotation = 0;

            }
            else if (exit.parent.gameObject.transform.position.x == 45) // EST
            {
                position = parentPosition + new Vector3(10, 0, 0);
                rotation = 90;

            }
            else if (exit.parent.gameObject.transform.position.x == -45) // OUEST
            {
                position = parentPosition + new Vector3(-10, 0, 0);
                rotation = -90;

            }
        }

        block = Instantiate(BlockExit, position, Quaternion.identity);
        block.transform.parent = FolderBlocParent;
        block.name = BlockExit.name;
        block.transform.Rotate(0f, rotation, 0f);

        if (mode == 1)
        {
            RemoveButton();
            StartCoroutine(CompleteMap());
        }


    }

    // Fonctions qui ajoute les blocs pour fermé la map
    public void AjouterBlocFermeture(Transform blocNotConnected, GameObject prefab)
    {
        GameObject block;

        Vector3 position = new Vector3();
        float rotation = 0f;

        if (blocNotConnected.parent.gameObject.transform.position.z == 45 && blocNotConnected.parent.gameObject.transform.position.x == 45)
        {

            // Obtenez la position de l'objet et du carr� parent dans le r�f�rentiel mondial
            Vector3 objectPosition = blocNotConnected.transform.position;
            Vector3 parentPosition = blocNotConnected.parent.gameObject.transform.position;

            // Calculez la diff�rence de position entre l'objet et le carr� parent
            Vector3 difference = objectPosition - parentPosition;

            // Calculez l'angle entre le vecteur de diff�rence et l'axe X (droite)
            float angle = Vector3.SignedAngle(difference, Vector3.right, Vector3.forward);

            // D�terminez la position relative en fonction de l'angle
            if (angle > -45 && angle <= 45)
            {
                // Debug.Log("L'objet est � droite du carr� parent.");
                position = parentPosition + new Vector3(10, 0, 0);
                rotation = -90;
            }
            else if (angle > 45 && angle <= 135)
            {
                // Debug.Log("L'objet est en haut du carr� parent.");
            }
            else if (angle > 135 || angle <= -135)
            {
                // Debug.Log("L'objet est � gauche du carr� parent.");
            }
            else
            {
                // Debug.Log("L'objet est en bas du carr� parent.");

                position = parentPosition + new Vector3(0, 0, 10);
                rotation = 180;
            }

        }
        else if (blocNotConnected.parent.gameObject.transform.position.z == 45 && blocNotConnected.parent.gameObject.transform.position.x == -45)
        {
            // Obtenez la position de l'objet et du carr� parent dans le r�f�rentiel mondial
            Vector3 objectPosition = blocNotConnected.transform.position;
            Vector3 parentPosition = blocNotConnected.parent.gameObject.transform.position;

            // Calculez la diff�rence de position entre l'objet et le carr� parent
            Vector3 difference = objectPosition - parentPosition;

            // Calculez l'angle entre le vecteur de diff�rence et l'axe X (droite)
            float angle = Vector3.SignedAngle(difference, Vector3.right, Vector3.forward);

            // D�terminez la position relative en fonction de l'angle
            if (angle > -45 && angle <= 45)
            {

            }
            else if (angle > 45 && angle <= 135)
            {
                // Debug.Log("L'objet est en haut du carr� parent.");
            }
            else if (angle > 135 || angle <= -135)
            {
                position = parentPosition + new Vector3(-10, 0, 0);
                rotation = +90;
            }
            else
            {
                //  Debug.Log("L'objet est en bas du carr� parent.");

                position = parentPosition + new Vector3(0, 0, 10);
                rotation = 180;
            }
        }
        else if (blocNotConnected.parent.gameObject.transform.position.z == -45 && blocNotConnected.parent.gameObject.transform.position.x == -45)
        {
            // Obtenez la position de l'objet et du carr� parent dans le r�f�rentiel mondial
            Vector3 objectPosition = blocNotConnected.transform.position;
            Vector3 parentPosition = blocNotConnected.parent.gameObject.transform.position;

            // Calculez la diff�rence de position entre l'objet et le carr� parent
            Vector3 difference = objectPosition - parentPosition;

            // Calculez l'angle entre le vecteur de diff�rence et l'axe X (droite)
            float angle = Vector3.SignedAngle(difference, Vector3.right, Vector3.forward);

            // D�terminez la position relative en fonction de l'angle
            if (angle > -45 && angle <= 45)
            {

            }
            else if (angle > 45 && angle <= 135)
            {
                // Debug.Log("L'objet est en haut du carr� parent.");
                position = parentPosition + new Vector3(0, 0, -10);
                rotation = 0;
            }
            else if (angle > 135 || angle <= -135)
            {
                position = parentPosition + new Vector3(-10, 0, 0);
                rotation = +90;
            }
            else
            {
                // Debug.Log("L'objet est en bas du carr� parent.");
                position = parentPosition + new Vector3(0, 0, -10);
                rotation = 0;

            }
        }
        else if (blocNotConnected.parent.gameObject.transform.position.z == -45 && blocNotConnected.parent.gameObject.transform.position.x == 45)
        {
            // Obtenez la position de l'objet et du carr� parent dans le r�f�rentiel mondial
            Vector3 objectPosition = blocNotConnected.transform.position;
            Vector3 parentPosition = blocNotConnected.parent.gameObject.transform.position;

            // Calculez la diff�rence de position entre l'objet et le carr� parent
            Vector3 difference = objectPosition - parentPosition;

            // Calculez l'angle entre le vecteur de diff�rence et l'axe X (droite)
            float angle = Vector3.SignedAngle(difference, Vector3.right, Vector3.forward);

            // D�terminez la position relative en fonction de l'angle
            if (angle > -45 && angle <= 45)
            {
                position = parentPosition + new Vector3(10, 0, 0);
                rotation = -90;
            }
            else if (angle > 45 && angle <= 135)
            {
                // Debug.Log("L'objet est en haut du carr� parent.");
                position = parentPosition + new Vector3(0, 0, -10);
                rotation = 0;
            }
            else if (angle > 135 || angle <= -135)
            {
                position = parentPosition + new Vector3(-10, 0, 0);
                rotation = +90;
            }
            else
            {
                //  Debug.Log("L'objet est en bas du carr� parent.");
                position = parentPosition + new Vector3(0, 0, -10);
                rotation = 0;

            }
        }
        else
        {
            Vector3 parentPosition = blocNotConnected.parent.gameObject.transform.position;


            if (blocNotConnected.parent.gameObject.transform.position.z == -45) // SUD
            {
                position = parentPosition + new Vector3(0, 0, -10);
                rotation = 0;
            }
            else if (blocNotConnected.parent.gameObject.transform.position.z == 45) // NORD
            {
                position = parentPosition + new Vector3(0, 0, 10);
                rotation = 180;

            }
            else if (blocNotConnected.parent.gameObject.transform.position.x == 45) // EST
            {
                position = parentPosition + new Vector3(10, 0, 0);
                rotation = -90;

            }
            else if (blocNotConnected.parent.gameObject.transform.position.x == -45) // OUEST
            {
                position = parentPosition + new Vector3(-10, 0, 0);
                rotation = +90;

            }
        }

        block = Instantiate(prefab, position, Quaternion.identity);
        block.name = prefab.name;
        block.transform.parent = FolderBlocParent;
        block.transform.Rotate(0f, rotation, 0f);
    }

    IEnumerator CompleteMap(int mode = 1)
    {
        ManagerUI.SetTexBoxText("Génération terminer ! ");

        yield return new WaitForSeconds(0.02f);

        GetAllBlocNotConnected();

        LstAllBlocNotConnected.Remove(SortiChoisi);

        for (int i = 0; i < LstAllBlocNotConnected.Count; i++)
        {
            if (LstAllBlocNotConnected[i].tag == "mauvais")
            {
                //  Debug.Log("On mets un bloc ferme");
                AjouterBlocFermeture(LstAllBlocNotConnected[i].transform, BlocFermer);
            }
            else
            {
                if (mode == 2 || mode == 3)
                {
                    AjouterExit(LstAllBlocNotConnected[i].transform);
                }
                else
                {
                    AjouterBlocFermeture(LstAllBlocNotConnected[i].transform, BlocSensUnique);
                }
            }
        }

        Debug.Log("Map termnimé");



        ManagerUI.SetBtnStart();

        MapCree = true;
    }



    //Fonctions qui efface la carte de la scene
    public void ClearMap()
    {
        foreach (Transform child in FolderBlocParent)
        {
            Destroy(child.gameObject);
        }

        foreach (Transform child in FolderIaParent)
        {
            Destroy(child.gameObject);
        }
    }


    // Fonctions qui appele toute les fonctions pour régénerer une carte
    public void RestartGeneration()
    {
        RemoveButton();
        ClearMap();
        ManagerUI.ClearInfo();
        StartGeneation();

    }

    // Fonctions qui remove les buttons
    public void RemoveButton()
    {
        List<GameObject> objectsWithTag = new List<GameObject>();

        GameObject[] btnObjects = GameObject.FindGameObjectsWithTag("btn");
        foreach (GameObject btnObject in btnObjects)
        {
            objectsWithTag.Add(btnObject);
        }

        foreach (GameObject obj in objectsWithTag)
        {
            Destroy(obj);
        }

        objectsWithTag.Clear();
    }


    public void GenerateMode1()
    {
        generateBtnEnter(1);
    }

    public void GenerateMode2()
    {
        generateBtnEnter(2);
    }

    public void GenerateMode3()
    {

    }

}
