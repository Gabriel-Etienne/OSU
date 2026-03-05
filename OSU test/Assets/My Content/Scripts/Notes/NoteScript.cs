using TMPro;
using UnityEngine;

public class NoteScript : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private TMP_Text _nbText;

    NoteScript(string noteNb, Color noteColor)
    {
        _nbText.text = noteNb;
        _spriteRenderer.color = noteColor;
    }
    
}
