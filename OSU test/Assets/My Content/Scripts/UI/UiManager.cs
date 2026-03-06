using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    [SerializeField] private SongManager _songManager;
    public Slider musicProgressBar;

    [Space(20)]
    [Header("Score System")]
    [SerializeField] private Animator _comboAnimator;
    public TMP_Text comboText;
    private int _combo;
    public TMP_Text missText;
    private int _miss;
    public TMP_Text lateText;
    private int _late;
    public TMP_Text goodText;
    private int _good;
    public TMP_Text perfectText;
    private int _perf;
    
    
    private void Awake()
    {
        musicProgressBar.maxValue = _songManager.MusicLength;
    }

    private void OnEnable()
    {
        NoteScript.OnNoteState += GetNoteState;
    }

    private void OnDisable()
    {
        NoteScript.OnNoteState -= GetNoteState;
    }

    private void GetNoteState(NoteHitState noteHitState, Vector3 position)
    {
        switch (noteHitState)
        {
            case NoteHitState.Miss:
                _miss++;
                missText.text = $": {_miss}";
                _combo = 0;
                comboText.text = $"{_combo} X";
                break;
            
            case NoteHitState.Late:
                _late++;
                _combo++;
                lateText.text = $": {_late}";
                comboText.text = $"{_combo} X";
                _comboAnimator.Play("AddNumber", 0, 0f);
                break;
            
            case NoteHitState.Good:
                _good++;
                _combo++;
                goodText.text = $": {_good}";
                comboText.text = $"{_combo} X";
                _comboAnimator.Play("AddNumber", 0, 0f);
                break;
            
            case NoteHitState.Perfect:
                _perf++;
                _combo++;
                perfectText.text = $": {_perf}";
                comboText.text = $"{_combo} X";
                _comboAnimator.Play("AddNumber", 0, 0f);
                break;
        }
    }

    private void Update()
    {
        musicProgressBar.value = _songManager.MusicTime;
    }


}
