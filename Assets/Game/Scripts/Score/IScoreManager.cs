using System;

public interface IScoreManager
{
    public void OnIntialize(Action<int> onUpdateScore);
    public void StartScoreRound();

    public void AddScore(int score);
}