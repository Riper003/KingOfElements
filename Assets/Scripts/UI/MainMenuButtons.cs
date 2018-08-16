using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuButtons : MonoBehaviour {

   

    void Start() {
        //Cursor.lockState = CursorLockMode.None;        
    }

   
    public void LoadLevel() {
        Application.LoadLevel(1);
    }

    public void StartButton() {
        //Invoke("LoadLevel", 1f);
        LoadLevel();
    }

    public void ExitButton() {
        Application.Quit();
    }



}