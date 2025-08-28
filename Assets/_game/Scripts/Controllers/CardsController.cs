using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CardsController : IService, IDisposable
{
    private IDifficultLoader _diffLoader;
    private ISuitLoader _suitLoader;
    private int _currentDifficultId;
    private int _currentSuitPack;
    private int _col;
    private int _row;
    private float _minSpace = 10f;
    private List<Card> _cards;
    private int _pairCount;
    private int number = 0;
    private Vector2 _windowResolution;
    private List<Card> _verifCard = new();

    private SignalBus _signalBus;
    public void Init()
    {
        _signalBus = ServiceLocator.Current.Get<SignalBus>();
        _signalBus.Subscirbe<CardListSignal>(PoolList);
        _signalBus.Subscirbe<ResolutionChangedSignal>(SizeBoard);
        _signalBus.Subscirbe<OpenCardSignal>(CheckId);
        _signalBus.Subscirbe<FindPairBonusSignal>(FindPair);


        _diffLoader = ServiceLocator.Current.Get<IDifficultLoader>();
        _currentDifficultId = PlayerPrefs.GetInt(StringConstant.CURRENT_DIFFICULT, 0);

        _suitLoader = ServiceLocator.Current.Get<ISuitLoader>();
        _currentSuitPack = PlayerPrefs.GetInt(StringConstant.SELECTED_SUIT, 0);
        
        OnInit();
    }

    private async void CheckId(OpenCardSignal signal)
    {
        _verifCard.Add(signal.Card);
        if (_verifCard.Count < 2) return;
        _signalBus.Invoke(new BlockCardSignal());
        int cardId1 = _verifCard[0].SuitId;
        int cardId2 = _verifCard[1].SuitId;
        var isPair = CompareId(cardId1, cardId2);
        foreach (var card in _verifCard)
        {
            if (card != null)
            {
                card.CheckComplete(isPair);
            }
        }

        _signalBus.Invoke(new RatioChangeSignal(isPair));
        if (!isPair) await UniTask.WaitForSeconds(2f);
        else
        {
            _pairCount--;
            _signalBus.Invoke(new AddScoreSignal());
            if (_pairCount == 0)
            {
                _signalBus.Invoke(new EndGameSignal());
            }
        }
        _verifCard = new();
        _signalBus.Invoke(new BlockCardSignal());
    }

    private bool CompareId(int id1, int id2)
    {
        return id1 == id2;
    }

    private async void OnInit()
    {
        await UniTask.WaitUntil(_diffLoader.IsLoaded);
        await UniTask.WaitUntil(_suitLoader.IsLoaded); 

        var diffData = _diffLoader.GetDifficultData().FirstOrDefault(x => x.Id == _currentDifficultId);

        if (diffData == null)
        {
            Debug.LogError($"Can't find difficult with id {_currentDifficultId}");
            return;
        }

        var suitData = _suitLoader.GetSuitData().ToList()[_currentSuitPack];
        if (suitData == null)
        {
            Debug.LogError($"Can't find difficult with id {_currentSuitPack}");
            return;
        }

        _signalBus.Invoke(new SpawnCardSignal(diffData, suitData));
        CardsDistribution(_windowResolution);
        _signalBus.Invoke(new StartGameSignal());
    }
    private void PoolList(CardListSignal signal)
    {
        number++;
        _col = signal.Col;
        _row = signal.Row;
        _cards = Shuffle(signal.Cards);
        _pairCount = _cards.Count / 2;
    }
    public static List<T> Shuffle<T>(List<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = UnityEngine.Random.Range(0, i + 1);
            (list[i], list[j]) = (list[j], list[i]);
        }
        return list;
    }
    private void SizeBoard(ResolutionChangedSignal signal)
    {
        _windowResolution = signal.CurrentResolution;
        if (_cards != null) CardsDistribution(_windowResolution);
    }
    private void CardsDistribution(Vector2 resolution)
    {
        float windowHeight = resolution.x;
        float windowWidth = resolution.y;

        float objHeight = _cards[0].GetSize().x;
        float objWidth = _cards[0].GetSize().y;

        float maxPossibleWidth = (windowWidth - (_col + 1) * _minSpace) / _col;
        float maxPossibleHeight = (windowHeight - (_row + 1) * _minSpace) / _row;

        float widthScale = maxPossibleWidth / objWidth;
        float heightScale = maxPossibleHeight / objHeight;
        float scale = Mathf.Min(widthScale, heightScale, 1.2f);

        objHeight *= scale;
        objWidth *= scale;

        float horizontalSpacing = Mathf.Max(_minSpace, (windowWidth - _col * objWidth) / (_col + 1));
        float vertivalSpacing = Mathf.Max(_minSpace, (windowHeight - _row * objHeight) / (_row + 1));

        float startX = horizontalSpacing + objWidth / 2;
        float startY = vertivalSpacing + objHeight / 2;

        int index = 0;
        for (int i = 0; i < _row; i++)
        {
            for (int j = 0; j < _col; j++)
            {
                float posX = startX + j * (objWidth + horizontalSpacing);
                float posY = startY + i * (objHeight + vertivalSpacing);
                _cards[index].PositionChange(scale, new(posY,-posX));
                index++;
            }
        }
    }
    private void FindPair(FindPairBonusSignal signal)
    {
        if (_verifCard.Count == 0)
        {
            _signalBus.Invoke(new NoOpenCardSignal());
            return;
        }
        var openCard = _verifCard[0];
        List<Card> correctCard = new List<Card>();
        foreach (var card in _cards)
        {
            if (openCard.SuitId == card.SuitId && openCard.Id != card.Id && !card.IsComplete)
            {
                correctCard.Add(card);
            }
        }
        int random = UnityEngine.Random.Range(0, correctCard.Count);
        correctCard[random].ShakeCard(signal.Duration);
    }
    public void Dispose()
    {
        _signalBus.Unsubscribe<CardListSignal>(PoolList);
        _signalBus.Unsubscribe<ResolutionChangedSignal>(SizeBoard);
    }
}
