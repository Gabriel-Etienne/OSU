using System.Collections;
using TMPro;
using UnityEngine;
using System;

public class NoteScript : MonoBehaviour
{
    GameManager _gameManager;
    
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private TMP_Text _nbText;
    
    
    private NoteHitState _noteState = NoteHitState.Miss;
    public bool isAlreadyPressed = false;
    
    
    public static Action<NoteHitState, Vector3> OnNoteState;
    
    private Coroutine _updateCoroutine; 
    
    [Header("Note Animation Variables")] // all the animation curve must be between 0 and 1 , both in time and value
    [SerializeField] private AnimationCurve _insideAlphaCurve;
    [SerializeField] private Vector2 _insideAlphaCurveMinMax;
    [SerializeField] private SpriteRenderer _spriteRenderCorner;
    [Space(10)]
    
    [SerializeField] private AnimationCurve _outsideAlphaCurve;
    [SerializeField] private Vector2 _outsideAlphaCurveMinMax;
    [SerializeField] private SpriteRenderer _spriteRenderOutside;
    [Space(10)]
    
    [SerializeField] private AnimationCurve _outsideScaleCurve;
    [SerializeField] private Vector2 _outsideScaleCurveeMinMax;
    [Space(10)]

    private float _spawnTime = 0f;
    private float _targetTime = 1f;
    private float percentage = 0f;

    public void SetGameManager(GameManager gameManager)
    {
        _gameManager = gameManager;
    }

    public void SpawnNote(string noteNb, Color noteColor, Vector3 position, float newSpawnTime, float newTargetTime)
    {
        _nbText.text = noteNb;
        _spriteRenderer.color = noteColor;
        transform.position = position;
        _spawnTime = newSpawnTime;
        _targetTime = newTargetTime;
        percentage = 0;
        
        if (_updateCoroutine != null)
            StopCoroutine(_updateCoroutine);

        _updateCoroutine = StartCoroutine(UpdateNoteStateOverTime());
    }

    public void DespawnNote()
    {
        //Debug.Log($"{_noteState}");
        OnNoteState?.Invoke(_noteState, transform.position);
        
        gameObject.SetActive(false);
        
        _gameManager.AddUsedNoteToList(gameObject);
        
        if (_updateCoroutine != null)
            StopCoroutine(_updateCoroutine);
    }


    IEnumerator UpdateNoteStateOverTime()
    {
        _noteState = NoteHitState.Miss;
        
        while (percentage < 1f)
        {
            percentage = Mathf.InverseLerp(_spawnTime, _targetTime, _gameManager.ActualTime);
            UpdateNoteHitState();
            UpdateNoteState();
            yield return null;
        }

        float waitTime = _targetTime;
        _noteState =  NoteHitState.Late;
        
        while (waitTime < _targetTime + _gameManager.toleranceLate)
        {
            waitTime = _gameManager.ActualTime;
            yield return null;
        }
        
        _noteState = NoteHitState.Miss;
        
        DespawnNote();
    }

    private void UpdateNoteHitState()
    {
        if (percentage <= 1f && percentage >= 1f - _gameManager.tolerancePerfect)
        {
            _noteState = NoteHitState.Perfect;
        }
        else
        {
            _noteState =  NoteHitState.Good;
        }
    }

    private void UpdateNoteState()
    {
        // Evaluate curves
        float insideAlpha = _insideAlphaCurve.Evaluate(percentage);
        float outsideAlpha = _outsideAlphaCurve.Evaluate(percentage);
        float outsideScale = _outsideScaleCurve.Evaluate(percentage);

        // Remap between min / max
        insideAlpha = Mathf.Lerp(_insideAlphaCurveMinMax.x, _insideAlphaCurveMinMax.y, insideAlpha);
        outsideAlpha = Mathf.Lerp(_outsideAlphaCurveMinMax.x, _outsideAlphaCurveMinMax.y, outsideAlpha);
        outsideScale = Mathf.Lerp(_outsideScaleCurveeMinMax.x, _outsideScaleCurveeMinMax.y, outsideScale);

        Color newColor; // to contains the color of the element we want to change
        
        // the colored part
        newColor = _spriteRenderer.color;
        newColor.a = insideAlpha;
        _spriteRenderer.color = newColor;
        
        // the white circle around the colored part
        newColor = _spriteRenderCorner.color;
        newColor.a = insideAlpha;
        _spriteRenderCorner.color =  newColor;
        
        // the text within the colored part
        newColor = _nbText.color;
        newColor.a = insideAlpha;
        _nbText.color =  newColor;
        
        // the circle outside the note
        newColor = _spriteRenderOutside.color;
        newColor.a = outsideAlpha;
        _spriteRenderOutside.color = newColor;
        
        // change the scale value of the outside circle
        _spriteRenderOutside.transform.localScale = new Vector3(outsideScale, outsideScale, outsideScale);
    }
    
}

public enum NoteHitState
{
    Miss,
    Good,
    Perfect,
    Late
}