using UnityEngine;
using DG.Tweening;
using Cysharp.Threading.Tasks;
using UnityEditor;

public class Card : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _suit;
    [SerializeField] private SpriteRenderer _suitLogo;
    [SerializeField] private RectTransform _rectCard;
    [SerializeField] private BoxCollider _boxCollider;
    [SerializeField] private Renderer _renderer;


    private Vector3 _startPos;
    private Vector3 _rotate = new(90f, 0, 90f);
    private Sequence _sequence;

    private int _id;
    public int Id => _id;

    private int _suitId;
    public int SuitId => _suitId;

    private SignalBus _signalBus;
    private bool _gameActive = true;
    private bool _isChecing = false;
    private bool _isOpen = false;
    private bool _isComplete = false;
    public bool IsComplete => _isComplete;

    private bool _isAlive = true;
    private bool _isLensBonusActive = false;

    public void Init(Sprite sprite, Sprite logo, int suitId, int id, SignalBus bus)
    {
        _suitId = suitId;
        _id = id;
        _suit.sprite = sprite;
        _suitLogo.sprite = logo;
        _signalBus = bus;
        _suit.transform.localScale = SpriteSizeChange(sprite);
        _suitLogo.transform.localScale = SpriteSizeChange(logo);
        _signalBus.Subscirbe<BlockCardSignal>(Cheking);
        _signalBus.Subscirbe<LensBonusSignal>(RenderQueueChanging);
        _signalBus.Subscirbe<StopGameSignal>(StopGame);
        _signalBus.Subscirbe<StartGameSignal>(StartGame);
    }

    public async void CheckComplete(bool isPair)
    {
        if (!isPair)
        {
            await UniTask.WaitForSeconds(1.5f);
            if (_isAlive) CardFlip();
            return;
        }
        _isComplete = true;
        _isOpen = false;
    }

    private void OnMouseDown()
    {
        if (BoolChecer()) return;
        CardFlip();
        _signalBus.Invoke(new OpenCardSignal(this));
    }

    public void OnMouseOver()
    {
        if (BoolChecer()) return;
        _rectCard.DOAnchorPos3DZ(-80f, .5f);
    }

    public void OnMouseExit()
    {
        if (BoolChecer()) return;
        _rectCard.DOAnchorPos3D(_startPos, .5f);
    }
    private async void CardFlip()
    {
        _rotate.z = !_isOpen ? -90f : 90f;
        _sequence = DOTween.Sequence();
        _sequence
            .Append(transform.DOLocalRotate(_rotate, .5f))
            .Append(_rectCard.DOAnchorPos3D(_startPos, .5f))
            .SetLink(gameObject);
        if (_isOpen) await _sequence.AsyncWaitForCompletion();
        _isOpen = !_isOpen;
    }
    public void Cheking(BlockCardSignal signal)
    {
        _isChecing = !_isChecing;
    }
    public Vector2 GetSize()
    {

        Vector3 size = _boxCollider.size;

        return new(size.y, size.z);
    }
    public void PositionChange(float scale, Vector2 pos)
    {
        _rectCard.anchoredPosition = new(pos.x, pos.y);
        _rectCard.localScale = new(scale, scale, scale);
        _startPos = _rectCard.anchoredPosition3D;

    }
    private Vector3 SpriteSizeChange(Sprite sprite)
    {
        var size = GetSize();
        float cardWidth = size.x;
        float cardHeight = size.y;
        float spriteWidth = sprite.bounds.size.x;
        float spriteHeight = sprite.bounds.size.y;
        Debug.Log($"card:width{cardWidth}/height{cardHeight} | sprite: width{spriteWidth}, height{spriteHeight}");
        float scale = spriteWidth > spriteHeight ? (cardWidth * 2 / 3) / spriteWidth : (cardHeight * 2 / 3) / spriteHeight;
        return new(scale, scale, 1f);

    }
    private async void RenderQueueChanging(LensBonusSignal signal)
    {
        var startTime = Time.time;
        _isLensBonusActive = true;
        while (Time.time - startTime < signal.BonusDuration)
        {
            if (!_isComplete && !_isOpen)
            {
                _renderer.material.renderQueue = 3002;
                _suitLogo.material.renderQueue = 3003;
            }
            await UniTask.Yield();
        }
        _isLensBonusActive = false;
        _renderer.material.renderQueue = 2500;
        _suitLogo.material.renderQueue = 3000;
    }

    private void StopGame(StopGameSignal signal)
    {
        _gameActive = false;
    }
    private void StartGame(StartGameSignal signal)
    {
        _gameActive = true;
    }
    private bool BoolChecer()
    {
        return (!_gameActive || _isLensBonusActive || _isChecing || _isComplete || _isOpen);
    }

    public void ShakeCard(float duration)
    {
        Debug.Log(_id);
        _rectCard.DOShakeAnchorPos(duration, 30, 5);
    }

    private void OnDestroy()
    {
        _isAlive = false;
        _signalBus.Unsubscribe<BlockCardSignal>(Cheking);
        _signalBus.Unsubscribe<LensBonusSignal>(RenderQueueChanging);
        _signalBus.Unsubscribe<StopGameSignal>(StopGame);
        _signalBus.Unsubscribe<StartGameSignal>(StartGame);
    }
}
