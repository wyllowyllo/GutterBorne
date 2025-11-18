using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("UI Panels")]
    [SerializeField] private GameObject _titlePanel;
    [SerializeField] private GameObject _inGamePanel;
    [SerializeField] private GameObject _gameOverPanel;

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

    public void GameOver()
    {
        if (_isGameOver) return;
        _isGameOver = true;

        Time.timeScale = 0f;

        if (_gameOverPanel != null) _gameOverPanel.SetActive(true);
        if (_inGamePanel != null) _inGamePanel.SetActive(false);
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
