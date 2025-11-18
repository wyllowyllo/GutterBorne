using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("UI Panels")]
    [SerializeField] private GameObject _titlePanel;
    [SerializeField] private GameObject _inGamePanel;
    [SerializeField] private GameObject _gameOverPanel;
    [SerializeField] private GameObject _clearPanel;

    [Header("플레이어")]
    [SerializeField] private PlayerBody _playerBody;
    private bool _isGameOver = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        
    }

    private void Start()
    {
        _playerBody.OnDeathEvent.AddListener(PlayerDeath);
        
        ShowTitle();
    }

    private void ShowTitle()
    {
        Time.timeScale = 0f;
        _isGameOver = false;

        if (_titlePanel != null) _titlePanel.SetActive(true);
        if (_inGamePanel != null) _inGamePanel.SetActive(false);
        if (_gameOverPanel != null) _gameOverPanel.SetActive(false);
    }

    public void StartGame()
    {
        Time.timeScale = 1f;
        _isGameOver = false;

        if (_titlePanel != null) _titlePanel.SetActive(false);
        if (_inGamePanel != null) _inGamePanel.SetActive(true);
        if (_gameOverPanel != null) _gameOverPanel.SetActive(false);
    }

    private void GameOver()
    {
        if (_isGameOver) return;
        _isGameOver = true;

        Time.timeScale = 0f;

        if (_gameOverPanel != null) _gameOverPanel.SetActive(true);
        if (_inGamePanel != null) _inGamePanel.SetActive(false);
    }

    public void GameClear()
    {
        Time.timeScale = 0f;
        
        if (_gameOverPanel != null) _clearPanel.SetActive(true);
        if (_inGamePanel != null) _inGamePanel.SetActive(false);
    }
    private void PlayerDeath()
    {
        Invoke("GameOver", 1f);
    }
    public void RestartGame()
    {
        SceneManager.LoadScene(0);
    }
    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
