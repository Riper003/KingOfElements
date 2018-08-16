using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Upgrade :Interactable {

    private int specc;
    public GameObject upgradePowerCanvas;
    private GameObject ui;
    
    public int originalDamageCost;
    public int originalAoeCost;
    public int originalHealthCost;
    public int originalDashCost;
    public int originalUltiCost;

    private int damageCost;
    private int aoeCost;
    private int healthCost;
    private int dashCost;
    private int ultiCost;
    public Text aoeUpgradeText;
    public Text dashUpgradeText;
    public Text ultiUpgradeText;
    public Text damageCostText;
    public Text aoeCostText;
    public Text healthCostText;
    public Text dashCostText;
    public Text ultiCostText;


    public Text amountHealthUpgrade;
    public Text amountAoeUpgrade;
    public Text amountDmgUpgrade;
    public Text amountDashUpgrade;
    public Text amountUltiUpgrade;

    public override void InteractEffect() {

        ui = GameObject.FindGameObjectWithTag("UI");

        ui.GetComponent<Pause>().PauseThis();
        upgradePowerCanvas.SetActive(true);
        UpdateCosts();
       

        specc = GameManager.Instance.specc;
        switch (specc) {
            case 0:
                aoeUpgradeText.text = "kek";
                dashUpgradeText.text = "top kek";
                ultiUpgradeText.text = "topest of keks";
                break;
            case 1:
                aoeUpgradeText.text = "Increases the area of your fireball explosion by 100%";
                dashUpgradeText.text = "Reduce the cooldown of your fire dash by 1 second";
                ultiUpgradeText.text = "Increase the amount of fireballs by 2";
                break;
            case 2:
                aoeUpgradeText.text = "Increases the number of enemies the icicle can pierce through by 1";
                dashUpgradeText.text = "Reduce the cooldown of your frost dash by 1 second";
                ultiUpgradeText.text = "Increase the duration of your freeze by 1 second";
                break;
            case 3:
                aoeUpgradeText.text = "Increases the number of times your lightbolt will bounce by 1";
                dashUpgradeText.text = "Increase the duration of your lightning dash by 1 second";
                ultiUpgradeText.text = "Increase the duration of your lightning bolt stun by 1 second";
                break;
        }

        UpdateText();

    }

    public void SetDamageUpgrade() {
        if (GameManager.Instance.damageUpgradeCount < 11) {
            if (GameManager.Instance.currency >= damageCost) {
                GameManager.Instance.damageUpgrade += 0.1f;
                GameManager.Instance.currency -= damageCost;
                GameManager.Instance.damageUpgradeCount++;
                
            } else {
                print("Nat enuff manny, buddy");
            }

        } else {
            print("NäeDmg");
        }


        UpdateText();
    }
    public void SetAoe() {
        if (GameManager.Instance.aoeUpgradeCount < 6) {

            if (GameManager.Instance.currency >= aoeCost) {
                GameManager.Instance.aoeUpgrade += 1;
                GameManager.Instance.currency -= aoeCost;
                GameManager.Instance.aoeUpgradeCount++;
                
            } else {
                print("Nat enuff manny, buddy");
            }

        } else {
            print("NäeAoe");
        }

        UpdateText();
    }
    public void SetHealth() {
        if (GameManager.Instance.healthUpgradeCount < 11) {

            if (GameManager.Instance.currency >= healthCost) {
                GameManager.Instance.healthUpgrade += 0.1f;
                GameManager.Instance.currency -= healthCost;
                GameManager.Instance.healthUpgradeCount++;
                player.GetComponent<Player>().UpdateHealth();
                player.GetComponent<Character>().RestorHealth(50);
            } else {
                print("Nat enuff manny, buddy");
            }

        } else {
            print("NäeHealth");
        }

        UpdateText();
    }

    public void SetDash() {
        if (GameManager.Instance.dashUpgradeCount < 3) {

            if (GameManager.Instance.currency >= dashCost) {
                GameManager.Instance.dashUpgrade ++;
                GameManager.Instance.currency -= dashCost;
                GameManager.Instance.dashUpgradeCount++;
                
            } else {
                print("Nat enuff manny, buddy");
            }

        } else {
            print("NäeDash");
        }

        UpdateText();
    }
    public void SetUlti() {
        if (GameManager.Instance.ultiUpgradeCount < 3) {

            if (GameManager.Instance.currency >= ultiCost) {
                GameManager.Instance.ultiUpgrade ++;
                GameManager.Instance.currency -= ultiCost;
                GameManager.Instance.ultiUpgradeCount++;
                
            } else {
                print("Nat enuff manny, buddy");
            }

        } else {
            print("NäeUlti");
        }

        UpdateText();
    }


    private void UpdateCosts() {
        damageCost = originalDamageCost * GameManager.Instance.damageUpgradeCount;
        aoeCost = originalAoeCost * GameManager.Instance.aoeUpgradeCount;
        healthCost = originalHealthCost * GameManager.Instance.healthUpgradeCount;
        dashCost = originalDashCost * GameManager.Instance.dashUpgradeCount;
        ultiCost = originalUltiCost * GameManager.Instance.ultiUpgradeCount;
    }

    void UpdateText() {
        UpdateCosts();
        amountAoeUpgrade.text = GameManager.Instance.aoeUpgradeCount - 1 + "/5";
        amountDmgUpgrade.text = GameManager.Instance.damageUpgradeCount - 1 + "/10";
        amountHealthUpgrade.text = GameManager.Instance.healthUpgradeCount - 1 + "/10";
        amountDashUpgrade.text = GameManager.Instance.dashUpgradeCount - 1 + "/2";
        amountUltiUpgrade.text = GameManager.Instance.ultiUpgradeCount - 1 + "/2";

        if (GameManager.Instance.aoeUpgradeCount != 6)
            aoeCostText.text = aoeCost + " EP";
        else
            aoeCostText.text = "Max";

        if (GameManager.Instance.damageUpgradeCount != 11)
            damageCostText.text = damageCost + " EP";
        else
            damageCostText.text = "Max";

        if (GameManager.Instance.healthUpgradeCount != 11)
            healthCostText.text = healthCost + " EP";
        else
            healthCostText.text = "Max";
        if (GameManager.Instance.dashUpgradeCount != 3)
            dashCostText.text = dashCost + " EP";
        else
            dashCostText.text = "Max";
        if (GameManager.Instance.ultiUpgradeCount != 3)
            ultiCostText.text = ultiCost + " EP";
        else
            ultiCostText.text = "Max";



        player.GetComponent<Player>().UpdateCurrentHealthBar();
        GameManager.Instance.UpdateCurrencyText();


    }
    public void Exit() {
        ui.GetComponent<Pause>().Resume();
        upgradePowerCanvas.SetActive(false);
    }

}
