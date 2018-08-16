using System;
using UnityEngine;
using UnityEngine.UI;

public class Pause :MonoBehaviour {
    private bool paused = false;

    public GameObject upgradePowerCanvas;
    public GameObject choosePowerCanvas;
    public GameObject pauseMenu;
    public GameObject player;

    public Text dashToMouseText;

	public GameObject controlsCanvas;

    private void Start() {
        
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            PauseThis();
            choosePowerCanvas.SetActive(false);
            upgradePowerCanvas.SetActive(false);
			controlsCanvas.SetActive (false);
        }

    }

    public void PauseThis() {
        paused = TogglePause();
        pauseMenu.SetActive(paused);
        player.GetComponent<Character>().enabled = !paused;
        player.GetComponent<CastSkill>().enabled = !paused;
    }

    public bool TogglePause() {
        if (Time.timeScale == 0f) {
            Time.timeScale = 1f;

            return (false);
        } else {

            Time.timeScale = 0f;
            return (true);
        }
    }

    public void Resume() {
        pauseMenu.SetActive(false);
		controlsCanvas.SetActive (false);
        
        Time.timeScale = 1f;
        paused = false;
        player.GetComponent<Character>().enabled = !paused;
        player.GetComponent<CastSkill>().enabled = !paused;
        
    }

    public void MainMenu() {
        Time.timeScale = 1f;
        Application.LoadLevel(0);

    }

    public void ReturnToCheckpoint() {
        player.GetComponent<Player>().transform.position = player.GetComponent<Player>().startPos;
        Resume();
    }

	public void Controls(){
		controlsCanvas.SetActive (true);
	}

    public void DashToMouse() {
        
        GameManager.Instance.dashToMouse = !GameManager.Instance.dashToMouse;
        if (GameManager.Instance.dashToMouse)
            dashToMouseText.text = "Dash to mouse: " + "On";
        else
            dashToMouseText.text = "Dash to mouse: " + "Off";

    }

    public void Quit() {
        Application.Quit();
    }
}