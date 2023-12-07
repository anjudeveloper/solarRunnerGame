using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject player;
    public GameObject gameOverPanel;

    public SolarSpawnManager solarSpawnManager;
    public SolarSpawnManager solarSpawnManagerRoad;
    public ObstacleManager obstacleManager;
    public CoinManager coinManager;

    public BackgroundScroll backgroundScrollL1;
     public BackgroundScroll backgroundScrollL2;
      public BackgroundScroll backgroundScrollL3;

        public BackgroundScroll backgroundScrollR;

    public int perLifePoint;
    private bool isGameOver = false;

    void Start(){

            gameOverPanel.SetActive(false); // Hide game over panel

    }

    void Update()
    {

    }



    public void GameOver()
    {
        isGameOver = true;
        player.SetActive(false); // Deactivate player on game over
        gameOverPanel.SetActive(true); // Show game over panel
        solarSpawnManager.StopSpawning();
        obstacleManager.StopObstacleSpawning();
        solarSpawnManagerRoad.StopSpawning();
        backgroundScrollL1.isActive=false;
         backgroundScrollL2.isActive=false;
          backgroundScrollL3.isActive=false;
          backgroundScrollR.isActive=false;

        // Stop background scrolling or animations
        // Implement logic to handle the game over state
    }


    public void ContinueGame()
    {

        if (coinManager.GetCurrentPoint() >= perLifePoint)
        {
            coinManager.SpendPoint(100);

            isGameOver = false;
            player.SetActive(true); // Reactivate player
            gameOverPanel.SetActive(false); // Hide game over panel
            solarSpawnManager.ContinueSpawning();
            obstacleManager.ContinueObstacleSpawning();
             solarSpawnManagerRoad.ContinueSpawning();
            backgroundScrollL1.isActive=true;
             backgroundScrollL2.isActive=true;
              backgroundScrollL3.isActive=true;
                 backgroundScrollR.isActive=true;

        }
        else
        {

            Debug.Log("Not Engjoy Point");
        }


    }

    public void MainMenu()
    {
        SceneManager.LoadScene("Main");
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
