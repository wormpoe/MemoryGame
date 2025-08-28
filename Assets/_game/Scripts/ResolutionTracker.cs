using UnityEngine;

public class ResolutionTracker : MonoBehaviour, IService
{
    [SerializeField] private RectTransform _rect;
    private SignalBus _signalBus;
    private Vector2 currentResolution = Vector2.zero;

    public void Init()
    {
        _signalBus = ServiceLocator.Current.Get<SignalBus>();
    }

    private void Update()
    {
        Vector2 newResolution = new(_rect.rect.width, _rect.rect.height);

        if (newResolution != currentResolution)
        {
            ChangeResolution(newResolution);
        }
    }
    private void ChangeResolution(Vector2 res)
    {
        currentResolution = res;
        _signalBus.Invoke(new ResolutionChangedSignal(currentResolution));
    }
}
