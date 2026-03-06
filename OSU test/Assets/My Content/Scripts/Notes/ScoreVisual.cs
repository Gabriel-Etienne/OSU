using System.Collections;
using TMPro;
using UnityEngine;

public class ScoreVisual : MonoBehaviour
{
    private Coroutine _coroutine;
    
    [SerializeField] TMP_Text _scoreText;
    
    [Space(30)]
    [SerializeField] Color _colorPerfect;
    [SerializeField] Color _colorGood;
    [SerializeField] Color _colorLate;
    [SerializeField] Color _colorMiss;
    
    
    [Space(30)] 
    [Header("Visualiser Animation Variables")] // all the animation curve must be between 0 and 1 , both in time and value
    [SerializeField] private AnimationCurve _scaleCurve;
    [SerializeField] private Vector2 _scaleMinMax =  new Vector2(0f, 1f);

    [Space(30)] 
    public float speed = 3f;
    public float fadeDuration = 0.5f;

    public void SpawnScore(NoteHitState hitState , Vector3 position)
    {
        transform.position = position;
        
        _scoreText.text = $"{hitState}";
        Color newColor = _colorMiss;

        switch (hitState)
        {
            case NoteHitState.Perfect:
                newColor = _colorPerfect;
                break;
            
            case NoteHitState.Good:
                newColor = _colorGood;
                break;
            
            case NoteHitState.Late:
                newColor = _colorLate;
                break;
            
            case NoteHitState.Miss:
                newColor = _colorMiss;
                break;
        }
        
        _scoreText.color = newColor;
        
        if (_coroutine != null)
            StopCoroutine(_coroutine);
        
        _coroutine = StartCoroutine(FadeOut());
        
    }

    public void DespawnScore()
    {
        // appelle la fonction qui la remet dans la pull
        
        
        gameObject.SetActive(false);
    }
    
    IEnumerator FadeOut()
    {
        float offset = Random.Range(-0.25f, 0.25f);
        Vector3 direction = new Vector3(offset, 1f, 0f);

        float timer = 0f;
        
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            
            Vector3 newPosition = transform.position + speed * Time.deltaTime * direction;
            transform.position = newPosition;
            
            
            float scaleValue = _scaleCurve.Evaluate(Mathf.InverseLerp(0, fadeDuration,  timer));
            
            float alphaValue = Mathf.InverseLerp(fadeDuration , 0,  timer);
            
            scaleValue = Mathf.Lerp(_scaleMinMax.x, _scaleMinMax.y, scaleValue);
            
            transform.localScale = new Vector3(scaleValue, scaleValue, scaleValue);
            
            Color newColor = _scoreText.color;
            newColor.a = alphaValue;
            _scoreText.color = newColor;
            
            yield return null;
        }
        
        _coroutine =  null;
        DespawnScore();
        
    }
    
    
}
