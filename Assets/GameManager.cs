using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour {

	bool gameStarted = false;
	public static GameManager Instance;
    public TextMeshProUGUI scoreTxt;
    public TextMeshProUGUI coinTxt;

    private void Awake()
    {
		Instance = this;
    }

    private void Start()
    {
        SoundManager.instance.PlaySoundLoop(SoundManager.Sounds.BGM);
    }

    void Update () {
		if (!gameStarted && Input.GetMouseButtonDown(0)) {
			FindObjectOfType<Text>().transform.parent.gameObject.SetActive(false);
			Ball.Move = gameStarted = true;
		}

        UpdateCoin();
        UpdateScore();
	}

    private void UpdateScore()
    {
        scoreTxt.text = ScoreManager.instance.GetScore().ToString();
    }

    private void UpdateCoin()
    {
        coinTxt.text = ScoreManager.instance.GetCoinCount().ToString();
    }

	public void Restart()
    {
		SceneManager.LoadScene(0);
    }
}
