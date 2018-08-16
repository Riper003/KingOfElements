using UnityEngine;
using System.Collections;

public class CustomCursor :MonoBehaviour {

    public Texture2D cursorTexture;


    public bool ccEnabled = false;

    void Start() {

        Invoke("SetCustomCursor",0.2f);
    }

    void OnDisable() {
        //Resets the cursor to the default 
        Cursor.SetCursor(null,Vector2.zero,CursorMode.Auto);
        //Set the _ccEnabled variable to false 
        this.ccEnabled = false;
    }

    private void SetCustomCursor() {
        //Replace the 'cursorTexture' with the cursor   
        Cursor.SetCursor(cursorTexture,Vector2.zero,CursorMode.Auto);        
        //Set the ccEnabled variable to true 
        this.ccEnabled = true;
    }
}


