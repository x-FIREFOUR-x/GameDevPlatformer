using UnityEngine;

public class GameStateController : MonoBehaviour
{
    enum StateGame
    {
        StartLevel, 
        RunGame,
        FinishGame
    }

    [SerializeField] InputReader _inputReader;

    private StateGame _stateGame = StateGame.StartLevel;

    public void SetStateRunGame()
    {
        _inputReader.enabled = true;
        _stateGame = StateGame.RunGame;
    }
}
