using System;

public class ScoreManager : IScoreManager
{
    private ScoreRound _currentRound;
    private Action<int> OnUpdateScore;

    public void OnIntialize(Action<int> onUpdateScore)
    {
        OnUpdateScore = onUpdateScore;
    }

    public void StartScoreRound()
    {
        _currentRound = new ScoreRound();
    }

    public void AddScore(int score)
    {
        _currentRound.CurrentScore += score;
        OnUpdateScore?.Invoke(_currentRound.CurrentScore);

    }

    public void EndScoreRound()
    {
        //save round
    }
}