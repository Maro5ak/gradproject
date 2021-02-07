using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour{

    public RectTransform infoPanel;
    public RectTransform loadingScreen;


    private Button zoomButton;
    private bool infoPanelVisible;

    Animal animal;

    private void Start() {
        EventHandlerUI.OnActionChange += UpdatePanel;
        EventHandlerUI.OnAgeChange += UpdatePanel;
        EventHandlerUI.OnLoading += LoadingDone;

        zoomButton = infoPanel.Find("ZoomButton").GetComponent<Button>();
        zoomButton.onClick.AddListener(delegate {ZoomOnSelected(); });

        infoPanelVisible = false;

        loadingScreen.gameObject.SetActive(false);
        infoPanel.gameObject.SetActive(infoPanelVisible);
    }

    private void Update() {
        if(infoPanelVisible){            
            UpdateNeeds();
        }
        // A way to get mouse inputs from the user about info etc
        Ray interactionRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit interactionInfo;
        if(Physics.Raycast(interactionRay, out interactionInfo, Mathf.Infinity)){
            GameObject interactedObject = interactionInfo.collider.gameObject;
            if(infoPanelVisible){
                if(Input.GetMouseButtonDown(1)){
                    ToggleInfoPanel();
                }
            }
            if(interactedObject.tag == "Passive"){
                if(Input.GetMouseButtonDown(0)){
                    animal = interactedObject.GetComponent<Animal>();
                    UpdatePanel();
                    ToggleInfoPanel();
                }
                
            }
        }
    }
    // Method that updates the info that there is about the creature being viewed
    private void UpdatePanel(){
        Text name = infoPanel.Find("CreatureName").GetComponent<Text>();
        Text animalGender = infoPanel.Find("Gender").GetComponent<Text>();
        Text animalAge = infoPanel.Find("Age").GetComponent<Text>();
        Text currentAction = infoPanel.Find("CurrentAction").GetComponent<Text>();

        if(animal != null){
            currentAction.text = animal.GetComponent<Animal>().GetAction();
            animalAge.text = "Age: " + animal.GetComponent<Animal>().GetAge().ToString() + "yr(s)";
            name.text = animal.GetComponent<Animal>().GetName();
            animalGender.text = animal.GetComponent<Animal>().GetGender() ? "Male" : "Female";
        }
    }
    //updates the sliders for thirst and hunger
    private void UpdateNeeds(){
        Slider thirstBar = infoPanel.Find("Thirst").GetComponent<Slider>();
        Slider hungerBar = infoPanel.Find("Hunger").GetComponent<Slider>();
        
        thirstBar.value = animal.thirst;
        hungerBar.value = animal.hunger;

    }

    private void LoadingDone(){
        loadingScreen.gameObject.SetActive(false);
    }
    

    private void ZoomOnSelected(){
        if(infoPanelVisible){
            Debug.Log("Pepaaa");
        }
    }

    private void ToggleInfoPanel(){
        infoPanelVisible = !infoPanelVisible;
        infoPanel.gameObject.SetActive(infoPanelVisible);
    }
}
