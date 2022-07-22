using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
    using UnityEditor;
#endif

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    public Text scoreText = null;
    public Text demonsKilledText = null;
    public int demonsKilled = 0;
    public int score = 0;
    private void Awake()
    {
        updateScore();
    }

    private void updateScore()
    {
        demonsKilled = GamePlay.instance.demonsKilled;
        score = GamePlay.instance.score;
        scoreText.text = "Score : " + score.ToString();
        demonsKilledText.text = "Demons Killed : " + demonsKilled.ToString();
    }

    //goes to mainMenu
    public void GotoMainMenuScene()
    {
        SceneManager.LoadScene(0);
    }

    // exits game
    public void QuitGame()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }
}
