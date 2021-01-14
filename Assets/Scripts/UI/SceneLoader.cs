using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour{

    public Button startButton;
    public Slider populationSlider;
    public Slider treeSlider;
    public Text populationText;
    public Text densityText;

    void Start(){
        startButton.onClick.AddListener(delegate {SimulationStart(); });
        populationSlider.onValueChanged.AddListener(delegate { UpdatePopulation(); });
        treeSlider.onValueChanged.AddListener(delegate { UpdateTreeDensity(); });

        populationText.text = populationSlider.value + "";
        densityText.text = treeSlider.value * 100+ "%";
    }

    private void SimulationStart(){
        SceneManager.LoadScene("SampleScene");
    }

    private void UpdatePopulation(){
        populationText.text = populationSlider.value + "";
    }

    private void UpdateTreeDensity(){
        densityText.text = Mathf.RoundToInt(treeSlider.value * 100) + "%";
    }


}
