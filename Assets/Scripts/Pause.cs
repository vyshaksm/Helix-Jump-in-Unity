using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Pause : MonoBehaviour
{
    public static Pause Instance;
    public GameObject muteImage;
    public GameObject unMuteImage;
    public GameObject vibrateImage;
    public GameObject vibrateOffImage;
    public GameObject pausePanel;
    public TextMeshProUGUI scoreTxt;
    public TextMeshProUGUI highScoreTxt;
    public TextMeshProUGUI coinTxt;
    public List<GameObject> lifeSymbols;


    private void Awake()
    {
        Instance = this;
    }
    public void PauseGame()
    {
        pausePanel.SetActive(true);
        Time.timeScale = 0;
    }

    public void Resume()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1;
    }

    public void Reload()
    {
        SceneManager.LoadScene(0);
    }

    public void SaveAndExit()
    {
        Bridge.GetInstance().SendScore(ScoreManager.instance.GetScore());
    }

    public void Mute()
    {
        SoundManager.instance.Unmute();
        muteImage.SetActive(true);
        unMuteImage.SetActive(false);
        PlayerPrefs.SetInt("SoundMuted", 0);
    }

    public void UnMute()
    {
        SoundManager.instance.Mute();
        unMuteImage.SetActive(true);
        muteImage.SetActive(false);
        PlayerPrefs.SetInt("SoundMuted", 1);
    }

    public void VibrateOn()
    {
        vibrateImage.SetActive(true);
        vibrateOffImage.SetActive(false);
    }

    public void VibrateOff()
    {
        vibrateOffImage.SetActive(true);
        vibrateImage.SetActive(false);
    }

    private void Score()
    {
        scoreTxt.text = ScoreManager.instance.GetScore().ToString();
    }

    private void Coin()
    {
        coinTxt.text = Bridge.GetInstance().thisPlayerInfo.coins.ToString();
    }

    private void HighScore()
    {
        highScoreTxt.text = Bridge.GetInstance().thisPlayerInfo.highScore.ToString();
    }

    private void Update()
    {
        Score();
        Coin();
        HighScore();
    }
}
