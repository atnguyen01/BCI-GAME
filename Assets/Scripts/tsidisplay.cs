using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace TSI2Unity
{
    public class tsidisplay : MonoBehaviour
    {
        public GameObject TSImanager;
        private TSINetworkInterface tSINetworkInterface;
        public TextMeshPro Hb;
        public TextMeshPro HbO;
        // Start is called before the first frame update
        void Start()
        {

            tSINetworkInterface = TSImanager.GetComponent<TSINetworkInterface>();
            InvokeRepeating("UpdateOxyDisplay", 1f, 1f);
            InvokeRepeating("UpdateDeOxyDisplay", 1f, 1f);
        }

        public void UpdateOxyDisplay()
        {

            // Create an empty string to store the content
            string HbO_txt = "Hbo:\n";



            // Iterate through the dictionary and add each key-value pair to the content string
            foreach (var kvp in tSINetworkInterface.DataOxy)
            {
                HbO_txt += $"Ch: {kvp.Key}, HbO: {kvp.Value}\n";
            }

            // Set the Text UI object's text to the content string
            HbO.text = HbO_txt;
            // James: This outputs the numerous/annoying 20/21 messages in console
            //Debug.Log(tSINetworkInterface.DataOxy.Keys.Count);
        }

        public void UpdateDeOxyDisplay()
        {
            // Create an empty string to store the content
            string Hb_txt = "Hb:\n";


            // Iterate through the dictionary and add each key-value pair to the content string
            foreach (var kvp in tSINetworkInterface.DataDeOxy)
            {
                Hb_txt += $"Ch: {kvp.Key}, Hb: {kvp.Value}\n";
            }


            Hb.text = Hb_txt;

        }



    }
}