using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour{

    public RectTransform infoPanel;

    private bool infoPanelVisible;

    Animal animal;

    private void Start() {
        EventHandlerUI.OnActionChange += UpdateInfo;

        infoPanelVisible = false;

        infoPanel.gameObject.SetActive(infoPanelVisible);
    }

    private void Update() {
        if(infoPanelVisible){            
            UpdateNeeds();
        }
        Ray interactionRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit interactionInfo;
        if(Physics.Raycast(interactionRay, out interactionInfo, Mathf.Infinity)){
            GameObject interactedObject = interactionInfo.collider.gameObject;

            if(interactedObject.tag == "Passive"){
                if(Input.GetMouseButtonDown(0)){
                    animal = interactedObject.GetComponent<Animal>();
                    ToggleInfoPanel();
                }
                
            }
        }
    }
    
    private void UpdateInfo(Transform entity, string action){
        Text name = infoPanel.Find("CreatureName").GetComponent<Text>();
        Text currentAction = infoPanel.Find("CurrentAction").GetComponent<Text>();

        name.text = entity.name;
        currentAction.text = action;
    }
    private void UpdateNeeds(){
        Slider thirstBar = infoPanel.Find("Thirst").GetComponent<Slider>();
        Slider hungerBar = infoPanel.Find("Hunger").GetComponent<Slider>();
        
        thirstBar.value = animal.thirst;
        hungerBar.value = animal.hunger;

    }
   

    private void ToggleInfoPanel(){
        infoPanelVisible = !infoPanelVisible;
        infoPanel.gameObject.SetActive(infoPanelVisible);
    }
}
