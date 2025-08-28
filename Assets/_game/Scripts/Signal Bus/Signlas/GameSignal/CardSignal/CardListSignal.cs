using System.Collections.Generic;
public class CardListSignal
{
    public readonly List<Card> Cards = new();
    public readonly int Col;
    public readonly int Row;

    public CardListSignal(List<Card> cards, int col, int row)
    {
        Cards = cards;
        Col = col;
        Row = row;
    }
}
