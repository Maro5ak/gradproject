using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour{

    public Button startButton;
    public Slider populationSlider;
    public Slider treeSlider;
    public Slider plantSlider;
    public Slider bushSlider;

    public Text populationText;
    public Text treeDensityText;
    public Text bushDensityText;
    public Text plantDensityText;


    void Start(){
        startButton.onClick.AddListener(delegate {SimulationStart(); });
        populationSlider.onValueChanged.AddListener(delegate { UpdatePopulation(); });
        treeSlider.onValueChanged.AddListener(delegate { UpdateTreeDensity(); });
        plantSlider.onValueChanged.AddListener(delegate { UpdatePlantDensity(); });
        bushSlider.onValueChanged.AddListener(delegate { UpdateBushDensity(); });

        populationText.text = populationSlider.value + "";
        plantDensityText.text = plantSlider.value * 100 + "%";
        bushDensityText.text = bushSlider.value * 100 + "%";
        treeDensityText.text = treeSlider.value * 100+ "%";
    }

    private void SimulationStart(){
        
        ApplicationControl.maxPopulation = populationSlider.value;
        ApplicationControl.maxTrees = treeSlider.value;
        ApplicationControl.maxBushes = bushSlider.value;
        ApplicationControl.maxPlants = plantSlider.value;
        ApplicationControl.sceneSwitch = true;
        SceneManager.LoadScene("SampleScene");
        
    }

    private void UpdatePopulation(){
        populationText.text = populationSlider.value + "";
    }

    private void UpdateTreeDensity(){
        treeDensityText.text = Mathf.RoundToInt(treeSlider.value * 100) + "%";
    }

    private void UpdatePlantDensity(){
        plantDensityText.text = Mathf.RoundToInt(plantSlider.value * 100) + "%";
    }

    private void UpdateBushDensity(){
        bushDensityText.text = Mathf.RoundToInt(bushSlider.value * 100) + "%";
    }


}
