using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private Button _classic;
    [SerializeField]
    private Button _playAgain;
    [SerializeField]
    private GameObject _titleScreen;
    [SerializeField]
    private GameObject _resultScreen;
    [SerializeField]
    private GameObject _gameScreen;
    [SerializeField]
    private PlayerController _player;
    [Inject]
    private IScoreManager _scoreManager;
    [Inject]
    private CursorManager _cursorManager;
    [SerializeField]
    private Text _scoreText;
    private int _timeLeft = 30;
    [SerializeField]
    private Text _timeText;
    [SerializeField]
    private TriggerBehavior _firstPass;
    [SerializeField]
    private TriggerBehavior _secondPass;
    private bool _firstPassComplete;
    private bool _ballScoreDone;

    private void Awake()
    {
        _classic.onClick.AddListener(OnClickClassic);
        _scoreManager.OnIntialize(OnUpdateScore);
        _cursorManager.EnableCursor();
        _player.AllowPlayerMovement(false);
        _firstPass.SetOnPassTrigger(OnPassFirst);
        _secondPass.SetOnPassTrigger(OnPassSecond);
        _playAgain.onClick.AddListener(OnClickPlayAgain);
    }



    private void OnPassFirst(GameObject firstPassObject)
    {
        Debug.Log("FIRST PASS!");
        _firstPassComplete = true;
    }

    private void OnPassSecond(GameObject secondPassObject)
    {
        Debug.Log("SECOND PASS!");
        if (_ballScoreDone)
        {
            return;
        }
        _ballScoreDone = true;
        _scoreManager.AddScore(2);
    }

    private void OnUpdateScore(int score)
    {
        _scoreText.text = score.ToString();
        _scoreText.transform.localScale = Vector3.zero;
        _scoreText.transform.DOScale(Vector3.one, 0.15f).SetEase(Ease.OutBack);
    }

    private void OnClickClassic()
    {
        StartClassicGame();
    }

    public void StartClassicGame()
    {
        _titleScreen.gameObject.SetActive(false);
        _gameScreen.gameObject.SetActive(true);
        _scoreManager.StartScoreRound();
        _cursorManager.DisableCursor();
        _player.AllowPlayerMovement(true);
        _timeLeft = 60;
        StartCoroutine(StartGameTimer());
    }

    public void CheckBall()
    {
        _firstPassComplete = false;
        _ballScoreDone = false;
    }

    private void CompleteGame()
    {
        _resultScreen.gameObject.SetActive(true);
        _player.AllowPlayerMovement(false);
        _cursorManager.EnableCursor();

    }

    private IEnumerator StartGameTimer()
    {
        while(_timeLeft > 0)
        {
            _timeText.text = _timeLeft.ToString();
            yield return new WaitForSeconds(1f);
            _timeLeft--;
            
        }
        CompleteGame();
    }


    private void OnClickPlayAgain()
    {
        SceneManager.LoadScene(0);
    }

}