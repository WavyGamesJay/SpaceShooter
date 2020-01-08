using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    //handle to text
    [SerializeField] private Text _scoreText;
    [SerializeField] private Image _livesImage;
    [SerializeField] Sprite[] _livesSprites;
    [SerializeField] private Text _gameOverText;
    [SerializeField] private Text _restartText;

    [SerializeField] private GameManager _gameManager; 

    // Start is called before the first frame update
    void Start()
    {
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        _scoreText.text = "Score: " + 0;
        _gameOverText.gameObject.SetActive(false);
        _restartText.gameObject.SetActive(false);

        if(_gameManager == null) {
            Debug.LogError("Game Manager is NULL");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateScore(int playerScore) {
        _scoreText.text = "Score: " + playerScore.ToString();
    }

    public void UpdateLives(int currentLives) {
        _livesImage.sprite = _livesSprites[currentLives];
    }

    public void GameOver(bool gameOver) {
        _gameOverText.gameObject.SetActive(gameOver);
        _restartText.gameObject.SetActive(gameOver);
        StartCoroutine(GameOverFlicker());
        _gameManager.GameOver(); 
    }

    IEnumerator GameOverFlicker() {
        while(true) {
            _gameOverText.text = "GAME OVER";
            yield return new WaitForSeconds(0.5f);
            _gameOverText.text = "";
            yield return new WaitForSeconds(0.5f);
        }
    }
}
