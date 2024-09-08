using UnityEngine;

public class Health : MonoBehaviour {
    public int MaxHealth {get; private set;}
    public int CurrentHealth {get; private set;}

    [SerializeField] private bool hasHealthBar = false;
    [SerializeField] private bool showsDamageText = false;

    [SerializeField] private bool isHealable = true;
    [SerializeField] private bool isInvincible = false;
    

    

    
    public void InitializeHealthSystem(int _maxHealth, bool _hasHealthBar, bool _showsDamageText, bool _isHealable){
        SetMaxHealth(_maxHealth, true);
        hasHealthBar = _hasHealthBar;
        if(hasHealthBar){
            //Assign/create health bar if one does not exist
        }
        showsDamageText = _showsDamageText;
        isHealable = _isHealable;
    }

    public void SetMaxHealth(int amount, bool setCurrentToMax = false){
        MaxHealth = amount;
        if(setCurrentToMax) CurrentHealth = MaxHealth;
    }

    public void TakeHeal(int amount){
        if(isHealable){
            CurrentHealth += amount;
            if(CurrentHealth > MaxHealth){
                CurrentHealth = MaxHealth;
            }
        }
    }

    public void TakeDamage(int amount, bool wasCrit = false){
        if(!isInvincible){
            CurrentHealth -= amount;
            if(CurrentHealth <= 0){
                Debug.Log($"{name} has died!");
            }
        }
    }


    public void ChangeInvincibilityStatus(){
        isInvincible = !isInvincible;
    }
}