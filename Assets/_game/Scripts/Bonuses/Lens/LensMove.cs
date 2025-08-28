using UnityEngine;

public class LensMove : MonoBehaviour, IService
{
    [SerializeField] private Vector3 _centering = Vector3.zero;
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private GameObject _lensObject;

    private SignalBus _signalBus;

    public void Init()
    {
        _signalBus = ServiceLocator.Current.Get<SignalBus>();
        _signalBus.Subscirbe<ChangeActiveLensSignal>(ActivateLens);
    }

    private void ActivateLens(ChangeActiveLensSignal signal)
    {
        _lensObject.SetActive(!_lensObject.activeSelf);
    }

    private void Update()
    {
        Vector3 mousePosition = Input.mousePosition + _centering;
        Vector3 worldPosition = _mainCamera.ScreenToWorldPoint(mousePosition);
        transform.position = worldPosition;
    }
}
