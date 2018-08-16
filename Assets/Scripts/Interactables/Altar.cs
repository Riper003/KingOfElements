using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Altar :Interactable {

    public Sprite[] melee;
    public Sprite[] ranged;
    public Sprite[] dash;

    public GameObject choosePowerCanvas;
    private GameObject ui;

    public Image meleeOnCdIcon;
    public Image meleeActiveIcon;
    public Image rangedOnCdIcon;
    public Image rangedActiveIcon;
    public Image dashOnCdIcon;
    public Image dashActiveIcon;
    CastSkill skill;
    Text textField;

    public override void InteractEffect() {

        ui = GameObject.FindGameObjectWithTag("UI");
        
        ui.GetComponent<Pause>().PauseThis();
        choosePowerCanvas.SetActive(true);


        skill = player.GetComponent<CastSkill>();
        textField = player.GetComponent<Player>().feedbackText;
  

    }

    public void SetFire() {
        GameManager.Instance.specc = 1;        
        textField.text = "You obtained Fire Powers!";
        textField.color = new Color(1,0.3f,0);
        AfterPress();
    }
    public void SetFrost() {
        GameManager.Instance.specc = 2;
        textField.text = "You obtained Frost Powers!";
        textField.color = Color.cyan;
        
        AfterPress();
    }
    public void SetLightning() {
        GameManager.Instance.specc = 3;
        textField.text = "You obtained Lightning Powers!";
        textField.color = new Color(216,0,255);
        AfterPress();
    }
    public void SetNoMagic() {
        GameManager.Instance.specc = 0;
        textField.text = "You obtained no power!";
        textField.color = Color.gray;

        AfterPress();
        GameManager.Instance.fireWay.SetActive(false);
        GameManager.Instance.frostWay.SetActive(false);
        GameManager.Instance.lightningWay.SetActive(false);
    }

    void AfterPress() {
        ui.GetComponent<Pause>().Resume();
        choosePowerCanvas.SetActive(false);
        skill.UpdatePlayerSpecc();
        SetUiIcons(skill.specc);        
        skill.SetGlobalCooldown();
        player.GetComponent<Player>().feedbackCanvas.SetActive(true);
        GameManager.Instance.UpdateSkillIcons();
        StartCoroutine(player.GetComponent<Player>().TextDuration());
        GameManager.Instance.OpenRightWay();

    }

    void SetUiIcons(int specc) {        
        GameManager.Instance.meleeCDIcon.GetComponent<Image>().sprite = melee[specc];        
        GameManager.Instance.meleeActiveIcon.GetComponent<Image>().sprite = melee[specc];
        GameManager.Instance.rangedCDIcon.GetComponent<Image>().sprite = ranged[specc];
        GameManager.Instance.rangedActiveIcon.GetComponent<Image>().sprite = ranged[specc];
        GameManager.Instance.dashCDIcon.GetComponent<Image>().sprite = dash[specc];
        GameManager.Instance.dashActiveIcon.GetComponent<Image>().sprite = dash[specc];
    }


}
