using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour{

    public Button startButton;
    public Slider rabbitPopulationSlider;
    public Slider sheepPopulationSlider;
    public Slider treeSlider;
    public Slider plantSlider;
    public Slider bushSlider;
    public Slider rabbitGenderSlider;
    public Slider sheepGenderSlider;

    public Text rabbitPopulationText, sheepPopulationText;
    public Text treeDensityText;
    public Text bushDensityText;
    public Text plantDensityText;
    public Text rabbitMalePerc, rabbitFemalePerc, sheepMalePerc, sheepFemalePerc;

    private int rabbitMale, rabbitFemale, sheepMale, sheepFemale;


    void Start(){
        //button and entities
        startButton.onClick.AddListener(delegate {SimulationStart(); });
        treeSlider.onValueChanged.AddListener(delegate { UpdateTreeDensity(); });
        plantSlider.onValueChanged.AddListener(delegate { UpdatePlantDensity(); });
        bushSlider.onValueChanged.AddListener(delegate { UpdateBushDensity(); });
        //Species
        rabbitGenderSlider.onValueChanged.AddListener(delegate { UpdateRabbitGenderPercentages(); });
        rabbitPopulationSlider.onValueChanged.AddListener(delegate { UpdateRabbitPopulation(); });
        sheepGenderSlider.onValueChanged.AddListener(delegate { UpdateSheepGenderPercentages(); });
        sheepPopulationSlider.onValueChanged.AddListener(delegate { UpdateSheepPopulation(); });

        //entities
        plantDensityText.text = plantSlider.value * 100 + "%";
        bushDensityText.text = bushSlider.value * 100 + "%";
        treeDensityText.text = treeSlider.value * 100+ "%";
        //species
        rabbitPopulationText.text = rabbitPopulationSlider.value + "";
        sheepPopulationText.text = sheepPopulationSlider.value + "";
        //rabbit calculations
        rabbitMale = Mathf.RoundToInt(rabbitGenderSlider.value * 100);
        rabbitFemale = 100 - rabbitMale;
        rabbitMalePerc.text = rabbitMale + "%";
        rabbitFemalePerc.text = rabbitFemale + "%";
        //sheep calculations
        sheepMale = Mathf.RoundToInt(sheepGenderSlider.value * 100);
        sheepFemale = 100 - sheepMale;
        sheepMalePerc.text = sheepMale + "%";
        sheepFemalePerc.text = sheepFemale + "%";
    }

    private void SimulationStart(){
        
        
        ApplicationControl.maxTrees = treeSlider.value;
        ApplicationControl.maxBushes = bushSlider.value;
        ApplicationControl.maxPlants = plantSlider.value;
        //calcs for rabbit (again)
        rabbitMale = Mathf.RoundToInt(rabbitGenderSlider.value * rabbitPopulationSlider.value);
        rabbitFemale = Mathf.RoundToInt(rabbitPopulationSlider.value - rabbitMale);
        ApplicationControl.rabbitMalePop = rabbitMale;
        ApplicationControl.rabbitFemalePop = rabbitFemale;
        ApplicationControl.rabbitMaxPopulation = rabbitPopulationSlider.value;
        //calcs for sheeps, again ,_,
        sheepMale = Mathf.RoundToInt(sheepGenderSlider.value * sheepPopulationSlider.value);
        sheepFemale = Mathf.RoundToInt(sheepPopulationSlider.value - sheepMale);
        ApplicationControl.sheepMalePop = sheepMale;
        ApplicationControl.sheepFemalePop = sheepFemale;
        ApplicationControl.sheepMaxPopulation = sheepPopulationSlider.value;

        ApplicationControl.sceneSwitch = true;      
        //aaand go (that what the button does, you know)
        SceneManager.LoadScene("SampleScene");
        
    }

    private void UpdateRabbitPopulation(){
        rabbitPopulationText.text = rabbitPopulationSlider.value + "";
    }
    private void UpdateSheepPopulation(){
        sheepPopulationText.text = sheepPopulationSlider.value + "";
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
    private void UpdateRabbitGenderPercentages(){
        rabbitMale = Mathf.RoundToInt(rabbitGenderSlider.value * 100);
        rabbitFemale = 100 - rabbitMale;
        rabbitMalePerc.text = rabbitMale + "%";
        rabbitFemalePerc.text = rabbitFemale + "%";
    }
    private void UpdateSheepGenderPercentages(){
        sheepMale = Mathf.RoundToInt(sheepGenderSlider.value * 100);
        sheepFemale = 100 - sheepMale;
        sheepMalePerc.text = sheepMale + "%";
        sheepFemalePerc.text = sheepFemale + "%";
    }


}
