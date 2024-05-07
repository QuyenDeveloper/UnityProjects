using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    [Header("Setting")]
    [SerializeField] private Image _audioImg;

    [Header("Level to Load")]
    public string _newGameLevel;
    private string _levelToLoad;
    [SerializeField] private Button _loadGameButton = null;
    public Animator transition;
    public float transitionTime = 1f;
    float volume;
    private void Awake()
    {
        CheckSaveGameOnGameLoad();
    }
    public void NewGameDialogYes()
    {
        StartCoroutine(LoadLevel());
    }
    IEnumerator LoadLevel()
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(_newGameLevel);
    }
    public void LoadGameDialogYes()
    {
        _levelToLoad = PlayerPrefs.GetString("SavedLevel");
        SceneManager.LoadScene(_levelToLoad);
    }

    public void CheckSaveGameOnGameLoad()
    {
        if (PlayerPrefs.HasKey("SavedLevel"))
        {
            _loadGameButton.interactable = true;
        }
        else
        {
            _loadGameButton.interactable = false;
        }

        //Ckeck save volume

        volume = PlayerPrefs.GetFloat("masterVolume");
        if (volume == 0.5)
        {
            _audioImg.color = Color.white;
        }
        else
        {
            _audioImg.color = Color.red;
        }
        AudioListener.volume = volume;
    }

    public void ExitButton()
    {
        Application.Quit();
    }

    public void VolumeSwith()
    {
        if (volume == 0.5) {
            _audioImg.color = Color.red;
            volume = 0f;
        }else
        {
            _audioImg.color = Color.white;
            volume = 0.5f;
        }
        AudioListener.volume = volume;
        PlayerPrefs.SetFloat("masterVolume", volume);
    } 
}
