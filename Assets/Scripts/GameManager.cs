using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class GameManager : MonoBehaviour
{
    public enum LoseReason { HitTower, Crash, GoToSpace }
    public enum UIPanel { MainMenu, Gameplay, Win, Lose }

    [SerializeField] private BackgroundScroll backgroundScroll;
    [SerializeField] private Plane plane;
    [SerializeField] private TowerGenerator towerGenerator;
    [SerializeField] private Camera cam;
    [SerializeField] private Button tapButton;
    [SerializeField] private bool mobileInput;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI startGameText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI bestRecordText;
    [SerializeField] private GameObject mainMenuUI;
    [SerializeField] private GameObject gameplayUI;
    [SerializeField] private GameObject winUI;
    [SerializeField] private RawImage footageHolder;
    [SerializeField] private Texture[] finalImage;
    [SerializeField] private TextMeshProUGUI winMessage;
    [SerializeField, TextArea] private string[] winMessages;
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private VideoClip[] videoClips;
    [SerializeField] private GameObject loseUI;
    [SerializeField] private TextMeshProUGUI loseMessage;
    [SerializeField, TextArea] private string[] crashMessages;
    [SerializeField, TextArea] private string[] goToSpaceMessages;

    private int currentScore;
    public bool IsGameStarted { get; private set; }
    private bool isGameEnded;

    public Plane Plane { get => plane; }
    public Camera Cam { get => cam; }

    private const string RECORD_PREFS = "BEST_RECORD";

    private void Start()
    {
        Application.targetFrameRate = 60;

        mainMenuUI.SetActive(true);
        gameplayUI.SetActive(false);

        bestRecordText.text += PlayerPrefs.GetInt(RECORD_PREFS);

        tapButton.gameObject.SetActive(mobileInput);

        tapButton.onClick.AddListener(() =>
        {
            if (!IsGameStarted)
                StartGame();
            else
                plane?.Jump();

            if (isGameEnded)
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        });

        if (mobileInput)
            startGameText.text = "Touch screen to Start!";
        else
            startGameText.text = "Press 'Space' to Start!";
    }

    private void Update()
    {
        if (!IsGameStarted)
        {
            if (!mobileInput && Input.GetKeyDown(KeyCode.Space))
                StartGame();
        }
        else
        {
            if (!mobileInput && Input.GetKeyDown(KeyCode.Space))
                plane?.Jump();
        }

        if (isGameEnded)
            if (!mobileInput && Input.GetKeyDown(KeyCode.Space))
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void OnDisable()
    {
        videoPlayer.loopPointReached -= OnEndVideo;
    }

    private void StartGame()
    {
        IsGameStarted = true;
        gameplayUI.SetActive(true);
        mainMenuUI.SetActive(false);
        towerGenerator.StartGenerator();
    }

    public void AddScore()
    {
        currentScore++;
        scoreText.text = currentScore.ToString();
        AudioManager.Instance.ScoreUpSFX();
    }

    public void Lose(LoseReason reason)
    {
        backgroundScroll.Stop();
        towerGenerator.StopGenerator();
        isGameEnded = true;

        PlayerPrefs.SetInt(RECORD_PREFS, currentScore);

        if (reason == LoseReason.HitTower)
        {
            ShowUI(UIPanel.Win);

            winMessage.text = winMessages[Random.Range(0, winMessages.Length)];

            if (videoClips.Length > 0)
            {
                videoPlayer.clip = videoClips[Random.Range(0, videoClips.Length)];
                videoPlayer.Play();

                videoPlayer.loopPointReached += OnEndVideo;
            }
        }
        else if (reason == LoseReason.Crash)
        {
            ShowUI(UIPanel.Lose);
            loseMessage.text = crashMessages[Random.Range(0, crashMessages.Length)];
        }
        else if (reason == LoseReason.GoToSpace)
        {
            ShowUI(UIPanel.Lose);
            loseMessage.text = goToSpaceMessages[Random.Range(0, goToSpaceMessages.Length)];
        }
    }

    private void OnEndVideo(VideoPlayer source)
    {
        videoPlayer.clip = null;
        footageHolder.texture = finalImage[Random.Range(0, finalImage.Length)];

        AudioManager.Instance.WinSFX();
    }

    public void ShowUI(UIPanel uIPanel)
    {
        mainMenuUI.SetActive(false);
        gameplayUI.SetActive(false);
        winUI.SetActive(false);
        loseUI.SetActive(false);

        switch (uIPanel)
        {
            case UIPanel.MainMenu:
                mainMenuUI.SetActive(true);
                break;
            case UIPanel.Gameplay:
                gameplayUI.SetActive(true);
                break;
            case UIPanel.Win:
                winUI.SetActive(true);
                break;
            case UIPanel.Lose:
                loseUI.SetActive(true);
                break;
            default:
                break;
        }
    }
}