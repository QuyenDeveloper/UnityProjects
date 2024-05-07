using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InGameMenu : MonoBehaviour
{
    [SerializeField] private InputActionReference menu;
    [SerializeField] private GameObject inGameMenu, winPanel, losePanel, backGroundPanel;

    private bool isMenuOpen = false;
    private bool gameEnded = false;

    public Animator transition;
    public float transitionTime = 1f;
    float volume;
    [SerializeField] private Image _audioImg;

    private int score;
    public TextMeshProUGUI _scoreText;
    public int AddScore { get => score; set => score += value; }

    private Coroutine scoreReductionCoroutine = null; 
    public float reductionInterval = 1f;

    void Start()
    {
        StartScoreReduction();
    }
    private void OnEnable()
    {
        menu.action.performed += OpenInGameMenu;
    }

    private void OnDisable()
    {
        menu.action.performed -= OpenInGameMenu;
    }

    public void OpenInGameMenu(InputAction.CallbackContext context)
    {
        if (gameEnded) return;
        isMenuOpen = !isMenuOpen;
        inGameMenu.SetActive(isMenuOpen);
        Time.timeScale = isMenuOpen ? 0f : 1f; 
        VolumeSwith();
    }
    public void OnBackClick()
    {
        OpenInGameMenu(new InputAction.CallbackContext());
    }

    private IEnumerator OpenMainMenu()
    {
        OpenInGameMenu(new InputAction.CallbackContext());
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene("MenuScence");
    }

    public void OnMainMenuClicked()
    {
        StartCoroutine(OpenMainMenu());
    }

    public void OnQuitClicked()
    {
        Application.Quit();
    }

    private IEnumerator OpenEndGamePanel(bool winOrLose)
    {
        yield return new WaitForSeconds(2);
        Time.timeScale = 0f;
        gameEnded = true;
        backGroundPanel.SetActive(true);
        if (winOrLose)
        {
            winPanel.SetActive(true);
            losePanel.SetActive(false);
        }
        else
        {
            losePanel.SetActive(true);
            winPanel.SetActive(false);
        }
        StopScoreReduction();
    }

    public void GameEnd(bool winOrLose)
    {
        StartCoroutine(OpenEndGamePanel(winOrLose));
    }

    private IEnumerator OnNewGame()
    {
        Time.timeScale = 1f; 
        winPanel.SetActive(false); 
        losePanel.SetActive(false); 
        backGroundPanel.SetActive(false);

        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene("Level1");
    }

    public void OnNewGameClick()
    {
        StartCoroutine(OnNewGame());
    }
    public void VolumeSwith()
    {
        if (PlayerPrefs.GetFloat("masterVolume") == 0.5)
        {
            _audioImg.color = Color.red;
            volume = 0f;
        }
        else
        {
            _audioImg.color = Color.white;
            volume = 0.5f;
        }
        AudioListener.volume = volume;
        PlayerPrefs.SetFloat("masterVolume", volume);
    }

    public void UpdateScore()
    {
        _scoreText.text = score.ToString();
    }


    void StartScoreReduction()
    {
        Debug.Log(score.ToString());
        if (scoreReductionCoroutine == null)
        {
            scoreReductionCoroutine = StartCoroutine(ReduceScoreRoutine());
        }
    }

    void StopScoreReduction()
    {
        if (scoreReductionCoroutine != null)
        {
            StopCoroutine(scoreReductionCoroutine);
            scoreReductionCoroutine = null;
        }
    }

    private IEnumerator ReduceScoreRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(reductionInterval);
            ReduceScore();
            UpdateScoreUI();
        }
    }

    private void ReduceScore()
    {
        score--;
        if (score <= 0)
        {
            score = 0;
            //StopScoreReduction();
        }
    }

    private void UpdateScoreUI()
    {
        _scoreText.text = score.ToString();
    }
}
