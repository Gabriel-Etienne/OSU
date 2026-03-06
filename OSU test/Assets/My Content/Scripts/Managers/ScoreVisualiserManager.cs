using System;
using UnityEngine;
using System.Collections.Generic;

public class ScoreVisualiserManager : MonoBehaviour
{
    [SerializeField] private GameObject _scoreVisualiserPrefab;
    private int _nbNoteDefault = 10;
    private List<GameObject> _listAvailableVisualiser = new List<GameObject>(); // pulling system
    
    private void Start()
    {
        _listAvailableVisualiser.Clear();
        
        for (int i = 0; i < _nbNoteDefault; i++)
        {
            _listAvailableVisualiser.Add(InstantiateNote());
        }
    }

    private GameObject InstantiateNote()
    {
        GameObject newNote = Instantiate(_scoreVisualiserPrefab);
        newNote.gameObject.SetActive(false);
        return newNote;
    }

    private GameObject GetAvailableVisualiser()
    {
        if (_listAvailableVisualiser.Count == 0)
        {
            return InstantiateNote();
        }

        GameObject newNote = _listAvailableVisualiser[0];
        _listAvailableVisualiser.RemoveAt(0);
        
        return newNote;
    }
    
    public void SpawnNextVisualiserAt(NoteHitState hitState, Vector3 position)
    {
        GameObject newNote = GetAvailableVisualiser(); // get une note
        
        newNote.gameObject.SetActive(true); // active la note
        
        newNote.GetComponent<ScoreVisual>().SpawnScore(hitState, position);
    }

    private void OnEnable()
    {
        NoteScript.OnNoteState += SpawnNextVisualiserAt;
    }

    private void OnDisable()
    {
        NoteScript.OnNoteState -= SpawnNextVisualiserAt;
        
    }

}
