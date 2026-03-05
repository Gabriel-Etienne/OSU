using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class GameManager : MonoBehaviour
{
    #region Variables
    
    private Coroutine _coroutinePrimary;
    private Coroutine _coroutineSecondary;
    private Coroutine _coroutineUpdate;
    public Vector2 limit;
    
    [SerializeField] private GameObject _notePrefab;

    [SerializeField] private int _nbNoteDefault = 10;
    private List<GameObject> _listAvailableNode = new List<GameObject>(); // pulling system
    private List<GameObject> _listNoteToCheck = new List<GameObject>(); // already in gameplay
    
    
    private Camera _cam;
    [SerializeField] private LayerMask _noteLayer;

    #endregion


    public Vector2 randomTimerLimit = new Vector2(0.25f,2f);
    
    public float timerMax = 1f;
    private float timer = 0f;

    IEnumerator Update()
    {
        while (true)
        {
            timer +=  Time.deltaTime;
            if (timer >= timerMax)
            {
                float x = Random.Range(-(limit.x/2), limit.x/2);
                float y = Random.Range(-(limit.y/2), limit.y/2);
                SpawnNextNoteAt(new Vector2(x, y));
                timer -= timerMax;
                timerMax = Random.Range(randomTimerLimit.x , randomTimerLimit.y);
            }
            yield return null;
            
        }
    }


    private void Start()
    {
        _cam = Camera.main;
        _listAvailableNode.Clear();
        
        for (int i = 0; i < _nbNoteDefault; i++)
        {
            _listAvailableNode.Add(InstantiateNote());
        }
        
        _coroutineUpdate = StartCoroutine(Update());
    }

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

    public void SpawnNextNoteAt(Vector2 position)
    {
        GameObject newNote = GetAvailableNote(); // get une note
        
        newNote.gameObject.SetActive(true); // active la note
        
        
        
        newNote.GetComponent<NoteScript>().SpawnNote(1.ToString(), Random.ColorHSV(),position); // spawn la note
    }

    public void AddUsedNoteToList(GameObject note)
    {
        _listAvailableNode.Add(note);
    }

    private void OnEnable()
    {
        InputManager.OnButtonPressedEvent += StartCheck;
        InputManager.OnButtonReleasedEvent += EndCheck;
    }

    private void OnDisable()
    {
        InputManager.OnButtonPressedEvent -= StartCheck;
        InputManager.OnButtonReleasedEvent -= EndCheck;
        
    }
    
    private void StartCheck(ButtonPressed buttonPressed)
    {
        // start coroutine of check 

        if (buttonPressed == ButtonPressed.Primary)
        {
            if (_coroutinePrimary != null)
                StopCoroutine(_coroutinePrimary);
            _coroutinePrimary = StartCoroutine(CheckCoroutine());
        }
        else 
        {
            if (_coroutineSecondary != null)
                StopCoroutine(_coroutineSecondary);
            _coroutineSecondary = StartCoroutine(CheckCoroutine());
        }
        
        Debug.Log($"{buttonPressed} Button pressed");
    }

    private void EndCheck(ButtonPressed buttonPressed)
    {
        // end coroutine of check 
        
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
        
        Debug.Log($"{buttonPressed} Button released");
    }

    IEnumerator CheckCoroutine()
    {
        while (true)
        {
            Vector2 mouseScreenPos = Mouse.current.position.ReadValue();
            Vector3 mouseWorldPos = _cam.ScreenToWorldPoint(mouseScreenPos);
            mouseWorldPos.z = 0f;

            Collider2D hit = Physics2D.OverlapPoint(mouseWorldPos, _noteLayer);

            if (hit != null)
            {
                Debug.Log("Note touchée : " + hit.name);
                hit.GetComponent<NoteScript>().DespawnNote();
                
                break;

            }
            
            yield return null;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, limit);
    }
}
