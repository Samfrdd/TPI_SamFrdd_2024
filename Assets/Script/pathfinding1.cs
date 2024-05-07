/**
Auteur : Sam Freddi
Date : 19.03.2024
Description : Script pathfinding1, script qui se place sur le pathfinder et qui lui permets de se déplacer dans le maze. Il gère également les duplications
Version 2.0.0
**/


using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Pathfinding1 : MonoBehaviour
{

    [SerializeField]
    private float _forward;
    [SerializeField]
    private float _right;
    [SerializeField]
    private float _left;

    [SerializeField]
    private float _state = 0;
    [SerializeField]
    private float _distance = 0;

    private float _realDistance = 0;

    [SerializeField]
    private bool _blocked = false;

    [SerializeField]
    private float _speed = 0f;
    [SerializeField]
    private float _currentSpeed = 0f;

    [SerializeField]
    private bool _isMoving = false;

    [SerializeField]
    private List<GameObject> _allChildren;

    [SerializeField]
    private GameObject _parent;
    [SerializeField]
    private GameObject _currentBloc;

    [SerializeField]
    private bool _trouve = false;

    [SerializeField]
    private List<Material> _allMaterial;

    [SerializeField]
    private GameObject _prefabPathfinder;

    [SerializeField]
    private GameObject _dossierIA;

    [SerializeField]
    private bool _isOriginal = false;

    [SerializeField]
    private bool _noPathFound = false;

    private bool _algoFinished = false;

    [SerializeField]
    private bool _hasDuplicate = false;

    [SerializeField]
    private bool _canDuplicate = false;

    [SerializeField]
    private bool _blockChangeState = false;

    private ManagerUI _managerUI;
    private Vector3 _lastPosition; // Position du GameObject lors de la dernière frame


    [SerializeField]
    private RayCastScript _scriptLayerFrontal;
    [SerializeField]
    private RayCastScript _scriptLayerLeft;
    [SerializeField]
    private RayCastScript _scriptLayerRight;

    public bool Blocked { get => _blocked; private set => _blocked = value; }
    public bool Trouve { get => _trouve; private set => _trouve = value; }
    public GameObject Parent { get => _parent; private set => _parent = value; }
    public bool IsOriginal { get => _isOriginal; private set => _isOriginal = value; }
    public bool IsMoving { get => _isMoving; private set => _isMoving = value; }
    public float Forward { get => _forward; private set => _forward = value; }
    public float Right { get => _right; private set => _right = value; }
    public float Left { get => _left; private set => _left = value; }
    public float State { get => _state; private set => _state = value; }
    public float Distance { get => _distance; private set => _distance = value; }
    public float Speed { get => _speed; private set => _speed = value; }
    public float CurrentSpeed { get => _currentSpeed; private set => _currentSpeed = value; }
    public List<GameObject> AllChildren { get => _allChildren; private set => _allChildren = value; }
    public GameObject CurrentBloc { get => _currentBloc; private set => _currentBloc = value; }
    public List<Material> AllMaterial { get => _allMaterial; private set => _allMaterial = value; }
    public GameObject PrefabPathfinder { get => _prefabPathfinder; private set => _prefabPathfinder = value; }
    public GameObject DossierIA { get => _dossierIA; private set => _dossierIA = value; }
    public bool NoPathFound { get => _noPathFound; private set => _noPathFound = value; }
    public bool AlgoFinished { get => _algoFinished; private set => _algoFinished = value; }
    public bool HasDuplicate { get => _hasDuplicate; private set => _hasDuplicate = value; }
    public bool CanDuplicate { get => _canDuplicate; private set => _canDuplicate = value; }
    public bool BlockChangeState { get => _blockChangeState; private set => _blockChangeState = value; }
    public Vector3 LastPosition { get => _lastPosition; private set => _lastPosition = value; }
    public RayCastScript ScriptLayerFrontal { get => _scriptLayerFrontal; private set => _scriptLayerFrontal = value; }
    public RayCastScript ScriptLayerLeft { get => _scriptLayerLeft; private set => _scriptLayerLeft = value; }
    public RayCastScript ScriptLayerRight { get => _scriptLayerRight; private set => _scriptLayerRight = value; }
    public ManagerUI ManagerUI { get => _managerUI; set => _managerUI = value; }
    public float RealDistance { get => _realDistance; set => _realDistance = value; }

    private void Start()
    {
        IsMoving = true;
        LastPosition = transform.position;
        Distance = 0;
        RealDistance = 0;
        CanDuplicate = false;
        HasDuplicate = false;
        Blocked = false;
        DossierIA = GameObject.FindWithTag("IA");
        State = 6;
        CurrentSpeed = Speed;
        ManagerUI = GameObject.FindWithTag("gameManager").gameObject.GetComponent<ManagerUI>();
    }

    /// <summary>
    /// Méthode appelée lorsque le collider attaché à cet objet entre en collision avec une autre zone de déclenchement de collider.
    /// Elle vérifie les balises de l'objet en collision et effectue des actions appropriées.
    /// </summary>
    /// <param name="other">Le collider de l'autre objet impliqué dans l'événement de déclenchement.</param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Bloc")
        {
            CurrentBloc = other.gameObject;
        }
        if (other.gameObject.tag == "pathfinder")
        {
            if (other.gameObject.GetComponent<Pathfinding1>().CurrentSpeed == 0f)
            {
                CanDuplicate = false;
                // gameObject.GetComponent<Pathfinding1>().Blocked = true;
            }
        }
    }
    /// <summary>
    /// Méthode appelée à chaque frame pour mettre à jour le comportement de l'objet.
    /// Elle vérifie si la fin est atteinte, récupère les distances des capteurs de mouvement, et effectue les calculs nécessaires.
    /// Elle met à jour les variables Forward, Left et Right avec les distances des capteurs.
    /// </summary>
    void Update()
    {
        CheckIfFinish();
        float right = ScriptLayerRight.Distance;
        float forward = ScriptLayerFrontal.Distance;
        float left = ScriptLayerLeft.Distance;

        Forward = forward;
        Left = left;
        Right = right;

        // Afficher la distance parcourue
        //  Debug.Log("Distance parcourue : " + distance);
        if (!BlockChangeState)
        {
            CalculterState();
        }

        CalculateDistance();


        if (Distance > 4)
        {
            CanDuplicate = true;
            BlockChangeState = false;
        }
        else
        {
            CanDuplicate = false;
        }

        if (IsMoving)
        {
            Move();
        }
    }

    /// <summary>
    /// Calcule l'état actuel de l'objet en combinant les distances des capteurs frontal, gauche et droit.
    /// </summary>
    public void CalculterState()
    {
        State = Forward + Left + Right;
    }

    /// <summary>
    /// Calcule la distance parcourue pendant la frame actuelle en comparant la position actuelle de l'objet avec sa dernière position.
    /// Met à jour la distance totale parcourue et la distance réelle parcourue.
    /// Met également à jour la position précédente pour la prochaine frame.
    /// </summary>
    public void CalculateDistance()
    {
        float distanceFrame = Vector3.Distance(transform.position, LastPosition);

        // Mise à jour de la distance totale parcourue
        Distance += distanceFrame;
        RealDistance += distanceFrame;
        // Mettre à jour la position précédente pour la prochaine frame
        LastPosition = transform.position;
    }

    /// <summary>
    /// Vérifie si les conditions de fin de jeu sont remplies.
    /// Si l'objet est l'original et est bloqué et qu'aucun chemin n'a été trouvé, déclenche le blocage du pathfinder.
    /// Si l'objet est l'original, a trouvé la sortie et que l'algorithme est terminé, déclenche la fin du jeu.
    /// </summary>
    public void CheckIfFinish()
    {
        if (IsOriginal && _blocked && !NoPathFound)
        {
            NoPathFound = true;
            StartCoroutine(BlockPathfinder());
            NoPathFoundFunction();
        }

        if (IsOriginal && Trouve && !AlgoFinished)
        {
            AlgoFinished = true;
            ExitFound();
        }
    }

    /// <summary>
    /// Déplace l'objet en fonction de son état et des conditions environnementales.
    /// </summary>
    public void Move()
    {
        if (!Blocked) // Si on a pas atteint un sens unique
        {

            switch (State)
            {
                case 0:  // Croisement

                    if (CurrentBloc != null)
                    {
                        if (!HasDuplicate && CanDuplicate && CheckIfFirstPathfinder())
                        {
                            transform.position = CurrentBloc.transform.position + new Vector3(0, 1, 0);
                            StopMovement();
                            DuplicationForward();
                            DuplicationRight();
                            DuplicateLeft();
                        }
                    }

                    break;
                case 1: // Bloqué devant mais droite et gauche libre
                    if (!HasDuplicate && CanDuplicate && CheckIfFirstPathfinder())
                    {
                        transform.position = CurrentBloc.transform.position + new Vector3(0, 1, 0);
                        StopMovement();
                        DuplicateLeft();
                        DuplicationRight();
                    }
                    break;
                case 2: // Rien devant et a droite
                    if (!HasDuplicate && CanDuplicate && CheckIfFirstPathfinder())
                    {

                        transform.position = CurrentBloc.transform.position + new Vector3(0, 1, 0);
                        StopMovement();

                        DuplicationForward();
                        DuplicationRight();
                    }

                    break;
                case 3:   // Virage a droite 


                    transform.position = CurrentBloc.transform.position + new Vector3(0, 1, 0);

                    if (!HasDuplicate && CanDuplicate && CheckIfFirstPathfinder())
                    {
                        BlockChangeState = true;
                        Distance = 0;
                        // StopMovement();
                        // DuplicationRight();
                        transform.Rotate(Vector3.up, 90f);
                        CurrentSpeed = Speed;
                        State = 6;

                    }
                    break;
                case 4: // Devant et gauche libre

                    if (!HasDuplicate && CanDuplicate && CheckIfFirstPathfinder())
                    {

                        transform.position = CurrentBloc.transform.position + new Vector3(0, 1, 0);
                        StopMovement();
                        DuplicationForward();
                        DuplicateLeft();
                    }

                    break;
                case 5: // Virage gauche
                    transform.position = CurrentBloc.transform.position + new Vector3(0, 1, 0);

                    transform.position = CurrentBloc.transform.position + new Vector3(0, 1, 0);

                    if (!HasDuplicate && CanDuplicate && CheckIfFirstPathfinder())
                    {
                        BlockChangeState = true;
                        Distance = 0;
                        transform.Rotate(Vector3.up, -90f);
                        CurrentSpeed = Speed;
                        State = 6;

                    }

                    break;
                case 6:  // Rien devant 
                    CurrentSpeed = Speed;
                    break;
                case 7: // Bloqué

                    transform.position = CurrentBloc.transform.position + new Vector3(0, 1, 0);
                    StartCoroutine(BlockPathfinder());
                    // IsMoving = false;
                    break;
                default:
                    break;
            }
        }
        else
        {
            StopMovement();
            CanDuplicate = false;
        }

        Vector3 movement = Vector3.forward * CurrentSpeed * Time.deltaTime;
        transform.Translate(movement);
    }

    /// <summary>
    /// Arrête le mouvement de l'objet en mettant sa vitesse actuelle à zéro et en désactivant son indication de mouvement.
    /// </summary>
    public void StopMovement()
    {
        CurrentSpeed = 0f;
        IsMoving = false;
    }

    /// <summary>
    /// Vérifie si l'objet est le premier pathfinder à passer par le bloc actuel.
    /// </summary>
    /// <returns>Retourne vrai si l'objet est le premier pathfinder à passer par le bloc, sinon retourne faux.</returns>
    public bool CheckIfFirstPathfinder()
    {
        if (CurrentBloc.GetComponent<CheckAlreadyPass>().Pathfinder == this.gameObject)
        {
            return true;
        }
        else
        {
            StartCoroutine(BlockPathfinder());
            return false;
        }
    }

    /// <summary>
    /// Duplique l'objet pathfinder dans la direction avant s'il n'y a pas d'obstacle devant.
    /// </summary>
    public void DuplicationForward()
    {
        CalculterState();

        if (Forward != 1)
        {

            HasDuplicate = true;

            Vector3 newPos = transform.position;

            GameObject pathfinderClone = Instantiate(PrefabPathfinder, newPos, transform.rotation);
            pathfinderClone.GetComponent<Pathfinding1>().BLockChangeState();
            pathfinderClone.GetComponent<Pathfinding1>().SetState(6);
            pathfinderClone.GetComponent<Pathfinding1>().SetOriginal(false);
            pathfinderClone.GetComponent<Pathfinding1>().SetParent(this.gameObject);
            pathfinderClone.GetComponent<Pathfinding1>().ClearListChildren();
            pathfinderClone.GetComponent<Pathfinding1>().SetFolderParent(pathfinderClone);
            this.gameObject.GetComponent<Pathfinding1>().AddChildren(pathfinderClone);
        }
    }

    /// <summary>
    /// Duplique l'objet pathfinder dans la direction droite.
    /// </summary>
    public void DuplicationRight()
    {
        HasDuplicate = true;

        Vector3 newPos = transform.position;

        GameObject pathfinderClone = Instantiate(PrefabPathfinder, newPos, transform.rotation);
        pathfinderClone.GetComponent<Pathfinding1>().BLockChangeState();
        pathfinderClone.GetComponent<Pathfinding1>().SetState(6);
        pathfinderClone.GetComponent<Pathfinding1>().SetParent(this.gameObject);
        pathfinderClone.GetComponent<Pathfinding1>().ClearListChildren();
        pathfinderClone.GetComponent<Pathfinding1>().SetFolderParent(pathfinderClone);
        this.gameObject.GetComponent<Pathfinding1>().AddChildren(pathfinderClone);
        pathfinderClone.GetComponent<Pathfinding1>().SetOriginal(false);
        pathfinderClone.transform.Rotate(Vector3.up, 90f);
    }

    /// <summary>
    /// Duplique l'objet pathfinder dans la direction gauche.
    /// </summary>
    public void DuplicateLeft()
    {
        HasDuplicate = true;

        Vector3 newPos = transform.position;

        GameObject pathfinderClone = Instantiate(PrefabPathfinder, newPos, transform.rotation);
        pathfinderClone.GetComponent<Pathfinding1>().BLockChangeState();
        pathfinderClone.GetComponent<Pathfinding1>().SetState(6);
        pathfinderClone.GetComponent<Pathfinding1>().SetParent(this.gameObject);
        pathfinderClone.GetComponent<Pathfinding1>().ClearListChildren();
        pathfinderClone.GetComponent<Pathfinding1>().SetFolderParent(pathfinderClone);
        pathfinderClone.GetComponent<Pathfinding1>().SetOriginal(false);
        this.gameObject.GetComponent<Pathfinding1>().AddChildren(pathfinderClone);
        pathfinderClone.transform.Rotate(Vector3.up, -90f);
    }

    /// <summary>
    /// Définit le parent de l'objet.
    /// </summary>
    /// <param name="parent">Le GameObject qui deviendra le parent de cet objet.</param>
    public void SetParent(GameObject parent)
    {
        this.Parent = parent;
    }

    /// <summary>
    /// Coroutine qui bloque le pathfinder en activant son rendu de traînée, en changeant son matériau, en désactivant son rendu de maillage, et en notifiant le parent si ce n'est pas l'original.
    /// Attend ensuite pendant une courte durée avant de désactiver le rendu de traînée.
    /// </summary>
    /// <returns>Retourne une coroutine.</returns>
    IEnumerator BlockPathfinder()
    {
        gameObject.GetComponent<TrailRenderer>().enabled = true;
        gameObject.GetComponent<TrailRenderer>().material = AllMaterial[2];
        gameObject.GetComponent<MeshRenderer>().enabled = false;
        Blocked = true;
        if (!IsOriginal)
        {
            Parent.GetComponent<Pathfinding1>().CheckIfAllChildrenBlocked();
        }

        yield return new WaitForSeconds(0.5f);

        // Enelever tout
        if (gameObject.GetComponent<TrailRenderer>().material != AllMaterial[3])
        {
            gameObject.GetComponent<TrailRenderer>().enabled = false;
        }
    }

    /// <summary>
    /// Marque le pathfinder comme ayant trouvé la sortie, change son matériau de traînée et notifie le parent, s'il existe, qu'il a trouvé la sortie.
    /// </summary>
    public void FindExit()
    {
        Trouve = true;
        gameObject.GetComponent<TrailRenderer>().material = AllMaterial[0];
        if (Parent != null)
        {
            Parent.GetComponent<Pathfinding1>().FindExit();
        }
    }

    /// <summary>
    /// Ajoute un enfant à la liste des enfants de l'objet.
    /// </summary>
    /// <param name="child">Le GameObject qui deviendra un enfant de cet objet.</param>
    public void AddChildren(GameObject child)
    {
        AllChildren.Add(child);
    }

    /// <summary>
    /// Efface tous les éléments de la liste des enfants.
    /// </summary>
    public void ClearListChildren()
    {
        AllChildren.Clear();
    }

    /// <summary>
    /// Définit le parent d'un objet pour le regrouper sous un dossier spécifique.
    /// </summary>
    /// <param name="pathfinder">L'objet à déplacer sous le dossier.</param>
    public void SetFolderParent(GameObject pathfinder)
    {
        pathfinder.transform.parent = DossierIA.transform;
    }

    /// <summary>
    /// Vérifie si tous les enfants sont bloqués.
    /// Si oui, lance une coroutine pour bloquer le chemin du pathfinder.
    /// </summary>
    public void CheckIfAllChildrenBlocked()
    {
        bool oneIsBloked = true;

        for (int i = 0; i < AllChildren.Count; i++)
        {
            if (!AllChildren[i].GetComponent<Pathfinding1>().Blocked)
            {
                oneIsBloked = false;
            }
        }

        if (oneIsBloked)
        {
            StartCoroutine(BlockPathfinder());
        }
    }

    /// <summary>
    /// Définit si cet élément est original ou non.
    /// </summary>
    /// <param name="info">La valeur booléenne qui indique si cet élément est original.</param>
    public void SetOriginal(bool info)
    {
        IsOriginal = info;
    }

    /// <summary>
    /// Bloque le changement d'état.
    /// </summary>
    public void BLockChangeState()
    {
        BlockChangeState = true;
    }

    /// <summary>
    /// Définit l'état actuel de l'objet.
    /// </summary>
    /// <param name="state">La nouvelle valeur d'état à définir.</param>
    public void SetState(int state)
    {
        State = state;
    }

    /// <summary>
    /// Fonction appelée lorsqu'aucun chemin n'est trouvé.
    /// </summary>
    public void NoPathFoundFunction()
    {
        if (ManagerUI.ModeEnCours != 3)
        {
            ManagerUI.SetTexBoxText("Aucun chemin trouvé !");
            ManagerUI.BtnRestartGenerator.gameObject.SetActive(true);
            ManagerUI.StopTimer();
            ManagerUI.SetBtnPause(false);
            ManagerUI.SetBtnInformation(true);
            ManagerUI.OpenModalInformation();
            ManagerUI.SetBtnStart(1);
            ManagerUI.SetDistanceInfoText("");
            ManagerUI.SetPanelTextInformation("Sorti non trouvé");
            if (ManagerUI.ModeEnCours == 1)
            {
                StartCoroutine(ManagerUI.FindNearestExitBot());
            }
        }
    }

    /// <summary>
    /// Fonction appelée lorsque la sortie est trouvée par le pathfinder.
    /// </summary>
    public void ExitFound()
    {
        if (ManagerUI.ModeEnCours == 1)
        {
            ManagerUI.AlgoEnCours = false;

            ManagerUI.SetTexBoxText("Le pathFinder a trouvé la sortie !");
            ManagerUI.BtnRestartGenerator.gameObject.SetActive(true);
            ManagerUI.StopTimer();
            ManagerUI.SetBtnPause(false);
            ManagerUI.SetBtnStart(1);
            ManagerUI.SetBtnInformation(true);
            ManagerUI.OpenModalInformation();
            ManagerUI.SetPanelTextInformation("Sorti trouvé !");
            ManagerUI.SetDistanceInfoText("Le bot a parcouru " + ManagerUI.GetAllDistanceFromBot().ToString() + " mètres pour trouvé la sortie");
        }
    }

    /// <summary>
    /// Bloque le pathfinder pour enregistrer son état.
    /// </summary>
    public void BlockPathfinderForSave()
    {
        Blocked = true;
    }

    /// <summary>
    /// Modifie la vitesse du pathfinder.
    /// </summary>
    /// <param name="speed">La nouvelle vitesse du pathfinder.</param>
    public void ChangeSpeed(int speed)
    {
        Speed = speed;
    }

    /// <summary>
    /// Empêche la duplication du pathfinder.
    /// </summary>
    public void BlockDuplication()
    {
        CanDuplicate = false;
    }
}
