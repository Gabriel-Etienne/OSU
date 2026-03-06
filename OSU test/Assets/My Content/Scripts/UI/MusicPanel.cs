using UnityEngine;

public class MusicPanel : MonoBehaviour
{
    [Header("Music Panel System")]
    [SerializeField] private Animator _musicPanelAnimator;
    private bool _isPanelOpen = true;
    
    public void ChangeMusicPanelState()
    {
        _isPanelOpen = !_isPanelOpen;
        
        if (_isPanelOpen)
            _musicPanelAnimator.Play("OpenPanel");
        else
            _musicPanelAnimator.Play("RetractPanel");
    }
}
