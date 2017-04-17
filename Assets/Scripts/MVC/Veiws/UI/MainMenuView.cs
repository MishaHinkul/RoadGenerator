using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuView : BaseView
{
    [Header("Tabs:")]
    [SerializeField]
    private GameObject tabMainMenu;
    [SerializeField]
    private GameObject tabSettings;

    [Header("Buttons:")]
    [SerializeField]
    private Button generionButton;

    [SerializeField]
    private Button settingsButton;

    [SerializeField]
    private Button exitButton;

    [SerializeField]
    private Button settingsBackButton;

    [Header("Sliders:")]
    [SerializeField]
    private Slider spawnTimeSlide;
    [SerializeField]
    private Slider stopTimeSlider;

    [Header("Text:")]
    [SerializeField]
    private Text spawnText;
    [SerializeField]
    private Text stopText;

    private string spawnTitle;
    private string stopTitle;

    private const string PLAYERS_PREFS_TIME_SPAWN = "TimeSpawn";
    private const string PLAYERS_PREFS_TIEME_STOP = "TimeStop";

    [Inject]
    public SettingsModel settingsModel { get; private set; }

    public void LoadView()
    {
        generionButton.onClick.RemoveAllListeners();
        settingsButton.onClick.RemoveAllListeners();
        exitButton.onClick.RemoveAllListeners();
        settingsBackButton.onClick.RemoveAllListeners();
        spawnTimeSlide.onValueChanged.RemoveAllListeners();
        stopTimeSlider.onValueChanged.RemoveAllListeners();


        generionButton.onClick.AddListener(Generation);
        settingsButton.onClick.AddListener(Settings);
        exitButton.onClick.AddListener(Exit);
        settingsBackButton.onClick.AddListener(Back);

        spawnTimeSlide.onValueChanged.AddListener(ChangeValueSpawnTime);
        stopTimeSlider.onValueChanged.AddListener(ChangeValueStopTime);

        stopTitle = stopText.text;
        spawnTitle = spawnText.text;

        float startValueSpawn = PlayerPrefs.GetInt(PLAYERS_PREFS_TIME_SPAWN, 4);
        float startValueStop = PlayerPrefs.GetInt(PLAYERS_PREFS_TIEME_STOP, 4);

        ChangeValueSpawnTime(startValueSpawn);
        ChangeValueStopTime(startValueStop);
    }

    public void RemoveView()
    {
        generionButton.onClick.RemoveAllListeners();
        settingsButton.onClick.RemoveAllListeners();
        exitButton.onClick.RemoveAllListeners();
        settingsBackButton.onClick.RemoveAllListeners();
        spawnTimeSlide.onValueChanged.RemoveAllListeners();
        stopTimeSlider.onValueChanged.RemoveAllListeners();
    }

    //Buttons
    private void Generation()
    {
        dispatcher.Dispatch(EventGlobal.E_GeneradeRoads);
    }

    private void Settings()
    {
        tabMainMenu.SetActive(false);
        tabSettings.SetActive(true);
    }

    private void Exit()
    {
        Application.Quit();
    }

    private void Back()
    {
        tabMainMenu.SetActive(true);
        tabSettings.SetActive(false);

        PlayerPrefs.SetInt(PLAYERS_PREFS_TIME_SPAWN, (int)settingsModel.carSpawnTime);
        PlayerPrefs.SetInt(PLAYERS_PREFS_TIEME_STOP, (int)settingsModel.stopGassStationTime);
    }

    //Sliders
    private void ChangeValueSpawnTime(float value)
    {
        spawnText.text = spawnTitle + "<b> " + value + " </b> с.";
        settingsModel.carSpawnTime = value;
    }

    private void ChangeValueStopTime(float value)
    {
        stopText.text = stopTitle + "<b> " + value + " </b> с.";
        settingsModel.stopGassStationTime = value;
    }
}