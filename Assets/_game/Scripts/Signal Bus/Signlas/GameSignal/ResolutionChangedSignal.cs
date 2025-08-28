using UnityEngine;

public class ResolutionChangedSignal
{
    public readonly Vector2 CurrentResolution;

    public ResolutionChangedSignal(Vector2 currentResolution)
    {
        CurrentResolution = currentResolution;
    }
}
