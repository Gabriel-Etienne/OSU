using TMPro;
using UnityEngine;

public class NoteScript : MonoBehaviour
{
    GameManager _gameManager;
    
    [SerializeField] private Animator _animatorNote;
    [SerializeField] private Animator _animatorCircle;
    
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private TMP_Text _nbText;
    public bool isAlreadyPressed = false;

    public void SetGameManager(GameManager gameManager)
    {
        _gameManager = gameManager;
    }

    public void SpawnNote(string noteNb, Color noteColor, Vector3 position)
    {
        _nbText.text = noteNb;
        _spriteRenderer.color = noteColor;
        transform.position = position;
        
        _animatorNote.Play("OnSpawn");
        _animatorCircle.Play("CircleGathering");
    }

    public void DespawnNote()
    {
        // play disable function
        gameObject.SetActive(false);
        _gameManager.AddUsedNoteToList(gameObject);
    }
    
}
