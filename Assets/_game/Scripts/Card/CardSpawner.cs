using System.Collections.Generic;
using UnityEngine;

public class CardSpawner : MonoBehaviour, IService
{
    [SerializeField] private Card _cardPrefab;
    [SerializeField] private Transform _parent;

    private SignalBus _signalBus;
    private List<Card> _cards = new();

    private int _id = 0;
    private int _col;
    private int _row;
    private int _suitCount;
    private int _tmp = 0;

    public void Init()
    {
        _signalBus = ServiceLocator.Current.Get<SignalBus>();

        _signalBus.Subscirbe<SpawnCardSignal>(Spawn);
    }

    private void Spawn(SpawnCardSignal signal)
    {
        _col = signal.DifficultData.Column;
        _row = signal.DifficultData.Row;
        _suitCount = signal.DifficultData.SuitCount;
        var suits = signal.SuitData.Get(_suitCount);
        for (int i = 0; i < (_col * _row)/2; i++)
        {
            if (_tmp >= suits.Count) _tmp = 0;
            var SpriteData = suits[_tmp];
            _tmp++;
            for(int j = 0; j < 2; j++)
            {
                var go = GameObject.Instantiate(_cardPrefab, _parent);
                go.Init(SpriteData.SuitSprite, signal.SuitData.SuitLogo, SpriteData.Id, _id, _signalBus);
                _cards.Add(go);
                _id++;
            }
        }
        _signalBus.Invoke(new CardListSignal(_cards, _col, _row));
    }

    private void OnDestroy()
    {
        _signalBus.Unsubscribe<SpawnCardSignal>(Spawn);
    }
}
