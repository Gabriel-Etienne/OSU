using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    #region Variables

    public static Action ResetGameState;
    
    private Coroutine _coroutinePrimary;
    private Coroutine _coroutineSecondary;
    private Coroutine _coroutineUpdate;
    
    [Header("Game Settings")]
    [SerializeField] private GameObject _notePrefab;
    [SerializeField] private LayerMask _noteLayer;
    [SerializeField] private SongManager _songManager;
    public Vector2 gameLimits =  new Vector2(8, 8);
    
    
    public float timeBetweenSpawnAndHit = 1f;
    public float toleranceLate = 0.2f;
    public float tolerancePerfect = 0.2f;
    public float toleranceGood = 0.5f;
    
    
    private int _nbNoteDefault = 10;
    private List<GameObject> _listAvailableNode = new List<GameObject>(); // pulling system
    private Camera _cam;

    public float ActualTime => _songManager.MusicTime;
    public List<NoteToSpawn> notesToSpawn => _songManager.partition.notes;

    private int indexNoteToSpawn = 0;


    [Header("Song Editor Variables")]
    public bool isEditing = false;

    private int indexEditor = 1;
    public float timeBeforeLongNotes = 1f;
    public float registerFrequencyOnHold = 0.1f;
    
    private bool _isPrimaryPressed = false;
    private bool _isSecondaryPressed = false;
    
    public List<NoteToSpawn> _noteToSpawns = new List<NoteToSpawn>();

    #endregion
    
    private void Start()
    {
        _cam = Camera.main;
        _listAvailableNode.Clear();
        
        for (int i = 0; i < _nbNoteDefault; i++)
        {
            _listAvailableNode.Add(InstantiateNote());
        }
        
        _coroutineUpdate = StartCoroutine(UpdateCoroutine());
    }
    
    IEnumerator UpdateCoroutine()
    {
        while (true)
        {
            if (notesToSpawn.Count > indexNoteToSpawn && ActualTime >= notesToSpawn[indexNoteToSpawn].timeForPerfect - timeBetweenSpawnAndHit)
            {
                NoteToSpawn noteToSpawn = notesToSpawn[indexNoteToSpawn];
                
                SpawnNextNoteAt(noteToSpawn.position, noteToSpawn.color, notesToSpawn[indexNoteToSpawn].timeForPerfect);
                
                indexNoteToSpawn++;
            }
            yield return null;
        }
    }


    #region Event And Functions Linked
    private void OnEnable()
    {
        InputManager.OnButtonPressedEvent += StartCheck;
        InputManager.OnButtonReleasedEvent += EndCheck;
        InputManager.OnRestartEvent +=  ResetGame;
    }
    private void OnDisable()
    {
        InputManager.OnButtonPressedEvent -= StartCheck;
        InputManager.OnButtonReleasedEvent -= EndCheck;
        InputManager.OnRestartEvent -=  ResetGame;
    }

    private void ResetGame()
    {
        indexNoteToSpawn = 0;
        ResetGameState?.Invoke();
    }
    private void StartCheck(ButtonPressed buttonPressed)
    {
        if (!isEditing) StartCheckNonEditor(buttonPressed);
        else StartCheckEditor(buttonPressed);
    }
    private void EndCheck(ButtonPressed buttonPressed)
    {
        if (!isEditing) EndCheckNonEditor(buttonPressed);
        else EndCheckEditor(buttonPressed);
    }
    
    #endregion
    
    #region Editor

    private void StartCheckEditor(ButtonPressed buttonPressed)
    {
        if (buttonPressed == ButtonPressed.Primary)
        {
            if (_coroutinePrimary != null)
                StopCoroutine(_coroutinePrimary);
            
            _isPrimaryPressed = true;
            _coroutinePrimary = StartCoroutine(CheckCoroutineEditor(buttonPressed));
        }
        else 
        {
            if (_coroutineSecondary != null)
                StopCoroutine(_coroutineSecondary);
            
            _isSecondaryPressed = true;
            _coroutineSecondary = StartCoroutine(CheckCoroutineEditor(buttonPressed));
        }
    }
    
    private void EndCheckEditor(ButtonPressed buttonPressed)
    {
        if (buttonPressed == ButtonPressed.Primary)
        {
            _isPrimaryPressed =  false;
        }
        else 
        {
            _isSecondaryPressed = false;
        }
        
    }
    
    private bool IsButtonPressed(ButtonPressed button)
    {
        if (button == ButtonPressed.Primary)
            return _isPrimaryPressed;
        if (button == ButtonPressed.Secondary)
            return _isSecondaryPressed;

        return false;
    }
    
    private Vector3 GetMouseWorldPos()
    {
        Vector2 mouseScreenPos = Mouse.current.position.ReadValue();
        Vector3 pos = _cam.ScreenToWorldPoint(mouseScreenPos);
        pos.z = 0f;
        return pos;
    }
    
    IEnumerator CheckCoroutineEditor(ButtonPressed buttonPressed)
    {
        float timePressed = 0f;
        float timer = 0;
        float timerBetwewnRegister = 0f;

        int index = indexEditor;
        indexEditor++;
        
        bool isLongNote;
        
        List<PosAndTime> positionsAndTimes = new List<PosAndTime>();
        
        positionsAndTimes.Add(new PosAndTime(ActualTime, GetMouseWorldPos()));
        
        while (IsButtonPressed(buttonPressed))
        {
            timer += Time.deltaTime;
            timerBetwewnRegister += Time.deltaTime;

            if (timerBetwewnRegister >= registerFrequencyOnHold)
            {
                positionsAndTimes.Add(new PosAndTime(ActualTime, GetMouseWorldPos()));
                
                timerBetwewnRegister -= registerFrequencyOnHold;
            }

            
            yield return null;
        }

        isLongNote = timer >= timeBeforeLongNotes;
        
        _noteToSpawns.Add(new NoteToSpawn(positionsAndTimes[0].time, positionsAndTimes[0].position, Random.ColorHSV(), index));

    }
    
    #endregion
    
    #region Non Editor
    
    private GameObject InstantiateNote()
    {
        GameObject newNote = Instantiate(_notePrefab);
        newNote.GetComponent<NoteScript>().SetGameManager(this);
        newNote.gameObject.SetActive(false);
        return newNote;
    }

    private GameObject GetAvailableNote()
    {
        if (_listAvailableNode.Count == 0)
        {
            return InstantiateNote();
        }

        GameObject newNote = _listAvailableNode[0];
        _listAvailableNode.RemoveAt(0);
        
        return newNote;
    }

    public void SpawnNextNoteAt(Vector2 position , Color color, float targetTime)
    {
        GameObject newNote = GetAvailableNote(); // get une note
        
        newNote.gameObject.SetActive(true); // active la note
        
        newNote.GetComponent<NoteScript>().SpawnNote(1.ToString(), color,position, ActualTime, targetTime); // spawn la note
    }

    public void AddUsedNoteToList(GameObject note)
    {
        _listAvailableNode.Add(note);
    }
    
    private void StartCheckNonEditor(ButtonPressed buttonPressed)
    {
        if (buttonPressed == ButtonPressed.Primary)
        {
            if (_coroutinePrimary != null)
                StopCoroutine(_coroutinePrimary);
            _coroutinePrimary = StartCoroutine(CheckCoroutineNonEditor());
        }
        else 
        {
            if (_coroutineSecondary != null)
                StopCoroutine(_coroutineSecondary);
            _coroutineSecondary = StartCoroutine(CheckCoroutineNonEditor());
        }
    }
    
    private void EndCheckNonEditor(ButtonPressed buttonPressed)
    {
        if (buttonPressed == ButtonPressed.Primary)
        {
            if (_coroutinePrimary != null)
                StopCoroutine(_coroutinePrimary);
            _coroutinePrimary = null;
        }
        else 
        {
            if (_coroutineSecondary != null)
                StopCoroutine(_coroutineSecondary);
            _coroutineSecondary = null;
        }
    }
    
    IEnumerator CheckCoroutineNonEditor()
    {
        while (true)
        {
            Vector2 mouseScreenPos = Mouse.current.position.ReadValue();
            Vector3 mouseWorldPos = _cam.ScreenToWorldPoint(mouseScreenPos);
            mouseWorldPos.z = 0f;

            Collider2D hit = Physics2D.OverlapPoint(mouseWorldPos, _noteLayer);

            if (hit != null)
            {
                //Debug.Log("Note touchée : " + hit.name);
                hit.GetComponent<NoteScript>().DespawnNote();
                
                break;

            }
            
            yield return null;
        }
    }
    
    #endregion
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, gameLimits);
    }
}
