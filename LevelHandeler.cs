using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelHandeler : MonoBehaviour {

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.name.Equals("SpikeMap"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            Destroy(gameObject);
        }
        if (col.gameObject.name.Equals("EndLevel"))
        {
            int sceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
            if (sceneIndex >= SceneManager.sceneCountInBuildSettings)
            {
                Debug.Log("Game has ended at " + sceneIndex);
                Application.Quit();
            }
            else
            {
                SceneManager.LoadScene(sceneIndex);
                Destroy(gameObject);
            }
        }
    }
}
