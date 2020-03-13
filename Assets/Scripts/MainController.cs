using Managers;
using UnityEngine;
using Zenject;

public class MainController: IInitializable, ITickable, ITouchListener
{
    [Inject] private TileManager _tileManager;
    [Inject] private InputManager _inputManager;
    [Inject] private BallManager _ballManager;
    [Inject] private UIManager _uiManager;

    private Camera _mainCamera;
    private Vector2 _lastTilePosition;
    private float _minDistanceToEnd;
    private int _countFalls;
    private int _countPoints;

    private enum GameStatus
    {
        Pause,
        Play,
        Lose,
        WaitForRestart
    }

    private GameStatus _gameStatus = GameStatus.Pause;

    public void Tick()
    {
        if (_gameStatus == GameStatus.Play)
        {
            var ballPosition = _ballManager.Position;
            MoveCamera(ballPosition);
            var roundedPosition = new Vector2(Mathf.Round(ballPosition.x), Mathf.Round(ballPosition.y));
            BuildNewPathIfNeeded(roundedPosition);
            if (_tileManager.PresentTileOnPosition(roundedPosition))
            {
                BallOnPositionWithTile(roundedPosition);
            }
            else
            {
                BallOnPositionWithoutTile();
            }
        }
    }

    private void MoveCamera(Vector2 position)
    {
        var cameraTransform = _mainCamera.transform;
        cameraTransform.position = new Vector3(position.x, position.y, cameraTransform.position.z);
    }

    private void BallOnPositionWithoutTile()
    {
        _uiManager.ShowFalls(++_countFalls);
        _uiManager.ShowTapToPlay();
        _gameStatus = GameStatus.Lose;
        _ballManager.StopMoveAndDestroy(() => { _gameStatus = GameStatus.WaitForRestart; });
    }

    private void BallOnPositionWithTile(Vector2 roundedPosition)
    {
        if (!_lastTilePosition.Equals(roundedPosition))
        {
            _lastTilePosition = roundedPosition;
            _tileManager.BallNewCheckpoint(roundedPosition);
            if (_tileManager.CollectBonusOnTileIfExist(roundedPosition))
            {
                _uiManager.ShowPoints(++_countPoints);
            }
        }
    }

    private void BuildNewPathIfNeeded(Vector2 roundedPosition)
    {
        if (Vector2.Distance(roundedPosition, _tileManager.LastTilePosition) < _minDistanceToEnd)
        {
            _tileManager.BuildPath(10);
        }
    }

    public void Initialize()
    {
        _tileManager.CreateStartupArea();
        _tileManager.BuildPath(50);
        _minDistanceToEnd = Vector2.Distance(Vector2.zero, _tileManager.LastTilePosition);
        _inputManager.AddListener(this);
        _mainCamera = Camera.main;
        _uiManager.gameObject.SetActive(true);
    }

    public void TouchRegistered()
    {
        switch (_gameStatus)
        {
            case GameStatus.Pause:
                StartGame();
                break;
            case GameStatus.Play:
                _ballManager.ChangeDirection();
                break;
            case GameStatus.WaitForRestart:
                StartGame();
                _ballManager.Reset(_lastTilePosition);
                _ballManager.ChangeDirection();
                break;
        }
    }

    private void StartGame()
    {
        _uiManager.ShowTapToPlay(false);
        _gameStatus = GameStatus.Play;
        _ballManager.StartMove();
    }
}
