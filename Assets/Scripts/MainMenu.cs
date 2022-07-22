using System.Collections;
using System.Collections.Generic;
//unity editor is defined only when we are running games on editor..
#if UNITY_EDITOR
    using UnityEditor;
#endif

using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    // takes game to Gameplay
    public void StartNewGame() {
        SceneManager.LoadScene(1);
    }

    // rules
    public void Rules()
    {
        SceneManager.LoadScene(3);
    }
    // exits game
    public void QuitGame() {
        #if UNITY_EDITOR
            EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }


    
}

