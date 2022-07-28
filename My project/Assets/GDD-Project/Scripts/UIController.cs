using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public JetCont jetController;
    public Text speedText;
    public Text heightText;
    public Text throttleText;


    // Start is called before the first frame update
   
    void Update()
    {
        if(jetController==null)
        {
            Debug.LogError("Jet Controller is missing");
            return;
        }

        speedText.text= $"Speed: {jetController.speed:n0}";
        heightText.text = $"Height: {jetController.height:n0}";
        throttleText.text = $"Throttle:{jetController.throttle:n0}%";



    }
}
