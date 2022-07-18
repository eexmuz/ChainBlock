using UnityEngine;

public class VisualEffect : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem _particleSystem;

    [SerializeField]
    private ParticleCallbackHandler _particleCallbackHandler;

    private System.Action _onEnd;

    private void Awake()
    {
        _particleCallbackHandler.OnSystemStopped += OnSystemStopped;
    }

    public void Play(System.Action onEnd = null)
    {
        _particleSystem.Play();
        _onEnd = onEnd;
    }

    public void PlayAndDestroy()
    {
        _particleSystem.Play();
        _onEnd = () =>
        {
            Destroy(gameObject);
        };
    }
    
    private void OnSystemStopped()
    {
        if (_onEnd != null)
        {
            _onEnd.Invoke();
            _onEnd = null;
        }
    }
}