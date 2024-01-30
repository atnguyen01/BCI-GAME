using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;
using System.Linq;
using Unity.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using Microsoft.MixedReality.Toolkit.UI;
namespace TSI2Unity{
public class TextChanges : MonoBehaviour
{
        public GameObject TSImanager;
        private TSINetworkInterface tSINetworkInterface;
        public TMP_Text jim;
        public GameObject button1;
        public GameObject button2;
        public GameObject button3;
        public TMP_Text buttonlabel1;
        public TMP_Text buttonlabel2;
        public TMP_Text buttonlabel3;
        public int baker = 0;
        public int carpenter = 0;
        public int potter = 0;
        public int response_counter = 0; 
        public int button_num = 0;
        public int choice = 0; //set previous button num for choice 3s
        public bool isFocused = true;
        //public string Path;
        public string[][] csvValues;
        public string[] labelRow; // 1st row with commands
        string[] strsplit; //for point changes
        public bool tutorialOver = false;
    // Start is called before the first frame update

    public void LoadFile(string path){
    var data = File.ReadAllLines(path).Select(l => l.Split(';')).ToArray().ToArray();
    csvValues = data;
    /*
    for(int i=0; i <csvValues.GetLength(0); i++){
        labelRow[i] = csvValues[i][0];
    }*/
    }
    void Start()
    {
        isFocused = true;
        //var resources = Resources.Load("story_script");
        //Path = Application.dataPath + @"\Resources\story_script.csv";
        setButtonStatus();
        var basePath = Path.Combine(Application.streamingAssetsPath, "story_script.csv");
        csvValues = File.ReadAllLines(basePath).Select(l => l.Split(';')).ToArray().ToArray();
        //Debug.Log(Path);
        tSINetworkInterface = TSImanager.GetComponent<TSINetworkInterface>();

            //LoadFile(Path);
            //Debug.Log(csvValues.GetLength(0)); //getting number of rows
            for(int i=0; i <csvValues.GetLength(0); i++){
            //Debug.Log(csvValues[i][0]);
            labelRow[i] = csvValues[i][0];
            //Debug.Log(labelRow[i]);
            }   //shows row 1 for debugging and sanity purposes
    }


    public void setButton1(){
        button_num = 1;
    }
    public void setButton2(){
        button_num = 2;
    }

    public void setButton3(){
        button_num = 3;
    }


    // Update is called once per frame
    void Update()
    {
        
    }

   public void setButtonStatus(){
        if(buttonlabel1.text.Equals("")){
            button1.SetActive(false);
        } else {
            button1.SetActive(true);
        }
        if(buttonlabel2.text.Equals("")){
            button2.SetActive(false);
        } else {
            button2.SetActive(true);
        }
        if(buttonlabel3.text.Equals("")){
            button3.SetActive(false);
        } else {
            button3.SetActive(true);
        }
    }
//not done need to figure out point change for two characters at once
//lines 43-45 need to figure out (prob case for state choice) 
public void playTurn(){ 
    switch(labelRow[response_counter]){
        case "start_story":
            break;
        case "dialogue":
            jim.text = csvValues[response_counter][1];
                buttonlabel1.text = "";
                buttonlabel2.text = "Continue";
                buttonlabel3.text = "";
            if(labelRow[response_counter+1].Equals("choice_2")){
                buttonlabel1.text = csvValues[response_counter+1][1];
                buttonlabel2.text = "";
                buttonlabel3.text = csvValues[response_counter+1][2];
               response_counter += 1; // skip choice in csv to show at the same time
            }
            if(labelRow[response_counter+1].Equals("choice_3")){
                buttonlabel1.text = csvValues[response_counter+1][1];
                buttonlabel2.text = csvValues[response_counter+1][2];
                buttonlabel3.text = csvValues[response_counter+1][3];
                response_counter += 1; // skip choice in csv to show at the same time
            }


            if(labelRow[response_counter+1].Equals("comment") && labelRow[response_counter+2].Equals("state") && labelRow[response_counter+3].Equals("comment")){
            response_counter += 3; //skip comment state and coment
            }else if(labelRow[response_counter+1].Equals("comment") && labelRow[response_counter+2].Equals("state")){
            response_counter += 2; //skip comment and state
            }else if(labelRow[response_counter+1].Equals("comment")){
            response_counter += 1; //skip comment
            }
            if(labelRow[response_counter+1].Equals("state_choice_3") && isFocused && labelRow[response_counter+2].Equals("point_change")){
                buttonlabel1.text = csvValues[response_counter+1][1];
                buttonlabel2.text = csvValues[response_counter+1][2];
                buttonlabel3.text = csvValues[response_counter+1][3];
                response_counter += 2; //skip point change but still goes into affect 
            }  else if(labelRow[response_counter+1].Equals("state_choice_3") && !isFocused && labelRow[response_counter+2].Equals("point_change")){
                buttonlabel1.text = csvValues[response_counter+1][4];
                buttonlabel2.text = csvValues[response_counter+1][5];
                buttonlabel3.text = csvValues[response_counter+1][6];                    
                response_counter += 2; //skip point change but still goes into affect 
            }

            break;
        case "state_dialogue":
 if(isFocused && !csvValues[response_counter][1].Equals("skip")){
                jim.text = csvValues[response_counter][1];
            }else if(!isFocused && !csvValues[response_counter][2].Equals("skip")){
                jim.text = csvValues[response_counter][2];
            }

            if(!isFocused && csvValues[response_counter+1][2].Equals("skip") && csvValues[response_counter+2][2].Equals("skip")){
                response_counter += 1; 
            }
            if(!isFocused && csvValues[response_counter+1][2].Equals("skip")){
                response_counter += 1; 
            }

            if(isFocused && csvValues[response_counter+1][1].Equals("skip") && csvValues[response_counter+2][2].Equals("skip")){
                response_counter += 1; 
            }
            if(isFocused && csvValues[response_counter+1][1].Equals("skip")){
                response_counter += 1; 
            }            

            if(isFocused && labelRow[response_counter+1].Equals("comment") && labelRow[response_counter+2].Equals("state_choice_3") && labelRow[response_counter+3].Equals("point_change")){
                buttonlabel1.text = csvValues[response_counter+2][1];
                buttonlabel2.text = csvValues[response_counter+2][2];
                buttonlabel3.text = csvValues[response_counter+2][3];
                response_counter += 3; //skip point change but still goes into affect 
            }

            if(!isFocused && labelRow[response_counter+1].Equals("comment") && labelRow[response_counter+2].Equals("state_choice_3") && labelRow[response_counter+3].Equals("point_change")){
                buttonlabel1.text = csvValues[response_counter+2][4];
                buttonlabel2.text = csvValues[response_counter+2][5];
                buttonlabel3.text = csvValues[response_counter+2][6];
                response_counter += 3; //skip point change but still goes into affect 
            }

            
            if(isFocused && labelRow[response_counter+1].Equals("state_choice_3") && labelRow[response_counter+2].Equals("point_change")){
                buttonlabel1.text = csvValues[response_counter+1][1];
                buttonlabel2.text = csvValues[response_counter+1][2];
                buttonlabel3.text = csvValues[response_counter+1][3];
                response_counter += 2; //skip point change but still goes into affect 
            }
            
            if(!isFocused && labelRow[response_counter+1].Equals("state_choice_3") && labelRow[response_counter+2].Equals("point_change")){
                buttonlabel1.text = csvValues[response_counter+1][4];
                buttonlabel2.text = csvValues[response_counter+1][5];
                buttonlabel3.text = csvValues[response_counter+1][6];
                response_counter += 2; //skip point change but still goes into affect 
            }

            break;
        case "choice_3":
            if(button_num == 1){
                choice = 1;
            } else if(button_num == 2){
                choice = 2;
            }else if(button_num == 3){
                choice = 3;
            }
            break;
        case "path_2":
                switch(button_num){
                    case 1:
                    jim.text = csvValues[response_counter][1];
                    break;
                    case 2:
                    response_counter -=1; //so clicking will do nothing here
                    break;
                    case 3: 
                    jim.text = csvValues[response_counter][2];
                    break;
                }

                buttonlabel1.text = "";
                buttonlabel2.text = "Continue";
                buttonlabel3.text = "";
            break;
        case "state_path_3":
                if(labelRow[response_counter-1].Equals("choice_3")){
                if(button_num == 1){
                    choice = 1;
                } else if(button_num == 2){
                    choice = 2;
                }else if(button_num == 3){
                    choice = 3;
                }
            }
                buttonlabel1.text = "";
                buttonlabel2.text = "Continue";
                buttonlabel3.text = "";
            if(choice == 0){
            switch(button_num){
                case 1:
                if(isFocused){
                   strsplit = csvValues[response_counter-1][1].Split(' '); // if point change before
                   if(!csvValues[response_counter][1].Equals("skip") && !csvValues[response_counter][1].Equals("pass 4")){
                   jim.text = csvValues[response_counter][1]; 
                   }
                    if(csvValues[response_counter+3][1].Equals("skip") && csvValues[response_counter+2][1].Equals("skip") && csvValues[response_counter+1][1].Equals("skip")){
                        response_counter += 1;
                    }
                    if(csvValues[response_counter+2][1].Equals("skip") && csvValues[response_counter+1][1].Equals("skip")){
                        response_counter += 1;
                    }
                    if(csvValues[response_counter+1][1].Equals("skip")){
                        response_counter += 1;
                    }
                    if(csvValues[response_counter+1][1].Equals("pass 4")){
                        response_counter += 5;
                    }
                } else {
                  strsplit = csvValues[response_counter-1][4].Split(' ');
                    if(!csvValues[response_counter][4].Equals("skip") && !csvValues[response_counter][4].Equals("pass 4")){
                   jim.text = csvValues[response_counter][4]; 
                   } 
                    if(csvValues[response_counter+3][4].Equals("skip") && csvValues[response_counter+2][1].Equals("skip") && csvValues[response_counter+1][1].Equals("skip")){
                        response_counter += 1;
                    }
                    if(csvValues[response_counter+2][4].Equals("skip") && csvValues[response_counter+1][1].Equals("skip")){
                        response_counter += 1;
                    }
                    if(csvValues[response_counter+1][4].Equals("skip")){
                        response_counter += 1;
                    }
                    if(csvValues[response_counter+1][4].Equals("pass 4")){
                        response_counter += 5;
                    }
                }
                break;
                case 2:
                if(isFocused){
                   strsplit = csvValues[response_counter-1][2].Split(' ');
                    if(!csvValues[response_counter][2].Equals("skip")){
                   jim.text = csvValues[response_counter][2]; 
                   }
                    if(csvValues[response_counter+3][2].Equals("skip") && csvValues[response_counter+2][2].Equals("skip") && csvValues[response_counter+1][2].Equals("skip")){
                        response_counter += 1;
                    }
                    if(csvValues[response_counter+2][2].Equals("skip") && csvValues[response_counter+1][2].Equals("skip")){
                        response_counter += 1;
                    }
                    if(csvValues[response_counter+1][2].Equals("skip")){
                        response_counter += 1;
                    }
                    if(csvValues[response_counter+1][2].Equals("pass 4")){
                        response_counter += 5;
                    }
                } else {
                  strsplit = csvValues[response_counter-1][5].Split(' ');
                    if(!csvValues[response_counter][5].Equals("skip") && !csvValues[response_counter][5].Equals("pass 4")){
                   jim.text = csvValues[response_counter][5]; 
                   }
                   if(csvValues[response_counter+3][5].Equals("skip") && csvValues[response_counter+2][5].Equals("skip") && csvValues[response_counter+1][5].Equals("skip")){
                        response_counter += 1;
                    }
                    if(csvValues[response_counter+2][5].Equals("skip") && csvValues[response_counter+1][5].Equals("skip")){
                        response_counter += 1;
                    }
                    if(csvValues[response_counter+1][5].Equals("skip")){
                        response_counter += 1;
                    }
                    if(csvValues[response_counter+1][5].Equals("pass 4")){
                        response_counter += 5;
                    }
                }
                break;
                case 3:
                if(isFocused){
                   strsplit = csvValues[response_counter-1][3].Split(' ');
                    if(!csvValues[response_counter][3].Equals("skip")){
                   jim.text = csvValues[response_counter][3]; 
                   } 
                    if(csvValues[response_counter+3][3].Equals("skip") && csvValues[response_counter+2][3].Equals("skip") && csvValues[response_counter+1][3].Equals("skip")){
                        response_counter += 1;
                    }
                    if(csvValues[response_counter+2][3].Equals("skip") && csvValues[response_counter+1][3].Equals("skip")){
                        response_counter += 1;
                    }
                    if(csvValues[response_counter+1][3].Equals("skip")){
                        response_counter += 1;
                    }
                    if(csvValues[response_counter+1][3].Equals("pass 4")){
                        response_counter += 5;
                    }
                } else {
                  strsplit = csvValues[response_counter-1][6].Split(' ');  
                    if(!csvValues[response_counter][6].Equals("skip")){
                   jim.text = csvValues[response_counter][6]; 
                   } 
                    if(csvValues[response_counter+3][6].Equals("skip") && csvValues[response_counter+2][6].Equals("skip") && csvValues[response_counter+1][6].Equals("skip")){
                        response_counter += 1;
                    }
                    if(csvValues[response_counter+2][6].Equals("skip") && csvValues[response_counter+1][6].Equals("skip")){
                        response_counter += 1;
                    }
                    if(csvValues[response_counter+1][6].Equals("skip")){
                        response_counter += 1;
                    }
                    if(csvValues[response_counter+1][6].Equals("pass 4")){
                        response_counter += 5;
                    }
                } 
                break;
            }
    } else if(choice == 1){
        if(isFocused){
                    strsplit = csvValues[response_counter-1][1].Split(' '); // if point change before
                   if(!csvValues[response_counter][1].Equals("skip") && !csvValues[response_counter][1].Equals("pass 4")){
                        jim.text = csvValues[response_counter][1]; 
                   }
                    if(csvValues[response_counter+3][1].Equals("skip") && csvValues[response_counter+2][1].Equals("skip") && csvValues[response_counter+1][1].Equals("skip")){
                        response_counter += 1;
                    }
                    if(csvValues[response_counter+2][1].Equals("skip") && csvValues[response_counter+1][1].Equals("skip")){
                        response_counter += 1;
                    }
                    if(csvValues[response_counter+1][1].Equals("skip")){
                        response_counter += 1;
                    }
                    if(csvValues[response_counter+1][1].Equals("pass 4")){
                        response_counter += 5;
                    } 
                } else {
                  strsplit = csvValues[response_counter-1][4].Split(' ');
                    if(!csvValues[response_counter][4].Equals("skip") && !csvValues[response_counter][4].Equals("pass 4")){
                   jim.text = csvValues[response_counter][4]; 
                   } 
                    if(csvValues[response_counter+3][4].Equals("skip") && csvValues[response_counter+2][1].Equals("skip") && csvValues[response_counter+1][1].Equals("skip")){
                        response_counter += 1;
                    }
                    if(csvValues[response_counter+2][4].Equals("skip") && csvValues[response_counter+1][1].Equals("skip")){
                        response_counter += 1;
                    }
                    if(csvValues[response_counter+1][4].Equals("skip")){
                        response_counter += 1;
                    }
                    if(csvValues[response_counter+1][4].Equals("pass 4")){
                        response_counter += 5;
                    }
                }
        } else if(choice == 2){
        if(isFocused){
                   strsplit = csvValues[response_counter-1][2].Split(' ');
                    if(!csvValues[response_counter][2].Equals("skip")){
                   jim.text = csvValues[response_counter][2]; 
                   } 
                    if(csvValues[response_counter+3][2].Equals("skip") && csvValues[response_counter+2][2].Equals("skip") && csvValues[response_counter+1][2].Equals("skip")){
                        response_counter += 1;
                    }
                    if(csvValues[response_counter+2][2].Equals("skip") && csvValues[response_counter+1][2].Equals("skip")){
                        response_counter += 1;
                    }
                    if(csvValues[response_counter+1][2].Equals("skip")){
                        response_counter += 1;
                    }
                    if(csvValues[response_counter+1][2].Equals("pass 4")){
                        response_counter += 5;
                    }
                } else {
                  strsplit = csvValues[response_counter-1][5].Split(' ');
                    if(!csvValues[response_counter][5].Equals("skip") && !csvValues[response_counter][5].Equals("pass 4")){
                   jim.text = csvValues[response_counter][5]; 
                   }
                    if(csvValues[response_counter+3][5].Equals("skip") && csvValues[response_counter+2][5].Equals("skip") && csvValues[response_counter+1][5].Equals("skip")){
                        response_counter += 1;
                    }
                    if(csvValues[response_counter+2][5].Equals("skip") && csvValues[response_counter+1][5].Equals("skip")){
                        response_counter += 1;
                    }
                    if(csvValues[response_counter+1][5].Equals("skip")){
                        response_counter += 1;
                    }
                    if(csvValues[response_counter+1][5].Equals("pass 4")){
                        response_counter += 5;
                    }

        }
        } else if(choice == 3){
            if(isFocused){
                   strsplit = csvValues[response_counter-1][3].Split(' ');
                    if(!csvValues[response_counter][3].Equals("skip")){
                   jim.text = csvValues[response_counter][3]; 
                   }
                    if(csvValues[response_counter+3][3].Equals("skip") && csvValues[response_counter+2][3].Equals("skip") && csvValues[response_counter+1][3].Equals("skip")){
                        response_counter += 1;
                    }
                    if(csvValues[response_counter+2][3].Equals("skip") && csvValues[response_counter+1][3].Equals("skip")){
                        response_counter += 1;
                    }
                    if(csvValues[response_counter+1][3].Equals("skip")){
                        response_counter += 1;
                    }
                    if(csvValues[response_counter+1][3].Equals("pass 4")){
                        response_counter += 5;
                    }
                } else {
                  strsplit = csvValues[response_counter-1][6].Split(' ');  
                    if(!csvValues[response_counter][6].Equals("skip")){
                   jim.text = csvValues[response_counter][6]; 
                   }
                    if(csvValues[response_counter+3][6].Equals("skip") && csvValues[response_counter+2][6].Equals("skip") && csvValues[response_counter+1][6].Equals("skip")){
                        response_counter += 1;
                    }
                    if(csvValues[response_counter+2][6].Equals("skip") && csvValues[response_counter+1][6].Equals("skip")){
                        response_counter += 1;
                    }
                    if(csvValues[response_counter+1][6].Equals("skip")){
                        response_counter += 1;
                    }
                    if(csvValues[response_counter+1][6].Equals("pass 4")){
                        response_counter += 5;
                    } 
        }
        }
                if(strsplit[0].Equals("Carpenter")){
                    carpenter += Int32.Parse(strsplit[1]);
                } else if(strsplit[0].Equals("Baker")){
                    baker += Int32.Parse(strsplit[1]);
                } else if(strsplit[0].Equals("Potter")){
                    potter += Int32.Parse(strsplit[1]);
                }

                if(strsplit.Length > 3){
                    if(strsplit[2].Equals("Carpenter")){
                        carpenter += Int32.Parse(strsplit[3]);
                    } else if(strsplit[2].Equals("Baker")){
                        baker += Int32.Parse(strsplit[3]);
                    } else if(strsplit[2].Equals("Potter")){
                        potter += Int32.Parse(strsplit[3]);
                    }
                }
                for(int i = 0; i < strsplit.Length; i++){
                Debug.Log(strsplit[i]);
                }
                if(labelRow[response_counter+1].Equals("comment")){
                response_counter += 1; //skip comment
                }
            
                //Debug.Log(carpenter);
                //Debug.Log(baker);
                //Debug.Log(potter);

            break;

            break;
            case "wait":
                    jim.text = csvValues[response_counter][1];
                    buttonlabel1.text = csvValues[response_counter][2];
                    buttonlabel2.text = "";
                    buttonlabel3.text = csvValues[response_counter][3];
            break;
            case "wait_response":
                    if(button_num == 2 || button_num == 3){
                        jim.text = csvValues[response_counter][2];
                        response_counter -= 1; //stay where you are 
                    } else {
                        jim.text = csvValues[response_counter][1];
                    buttonlabel1.text = "";
                    buttonlabel2.text = "Continue";
                    buttonlabel3.text = "";
                        if(labelRow[response_counter+1].Equals("comment") && labelRow[response_counter+2].Equals("state")){
                         response_counter += 2; //skip past comment and state
                        }
                    }
            break;

            case "accuse":
                if(baker > carpenter && baker > potter){
                    jim.text = csvValues[response_counter][1];
                } else if(carpenter > baker && carpenter > potter){
                    jim.text = csvValues[response_counter][2];
                } else if(potter > baker && potter > carpenter){
                    jim.text = csvValues[response_counter][3];
                }
                if(labelRow[response_counter+1].Equals("comment")){
                    response_counter += 1; //skip comment
                    }
            break;

            case "path_3":
                buttonlabel1.text = "";
                buttonlabel2.text = "Continue";
                buttonlabel3.text = "";
                if(tutorialOver){
                    if(baker > carpenter && baker > potter && !csvValues[response_counter][1].Equals("skip")){
                        jim.text = csvValues[response_counter][1];
                    }
                    if(baker > carpenter && baker > potter && csvValues[response_counter+1][1].Equals("skip")){
                        response_counter += 1;
                    }

                    if(carpenter > baker && carpenter > potter && !csvValues[response_counter][2].Equals("skip")){
                        jim.text = csvValues[response_counter][2];
                    }
                    if(carpenter > baker && carpenter > potter && csvValues[response_counter+1][2].Equals("skip")){
                        response_counter += 1;
                    }

                    if(potter > baker && potter > carpenter && !csvValues[response_counter][3].Equals("skip")){
                        jim.text = csvValues[response_counter][3];
                    }
                    if(potter > baker && potter > carpenter && csvValues[response_counter+1][3].Equals("skip")){
                        response_counter += 1;
                    }
                    
                } else{
                    switch(button_num){
                        case 1:
                        jim.text = csvValues[response_counter][1];
                        break;
                        case 2:
                        jim.text = csvValues[response_counter][2];
                        break;
                        case 3:
                        jim.text = csvValues[response_counter][3];
                        break;
                    }
                    tutorialOver = true;
                }
                
            break;                   
    }
    setButtonStatus();
    response_counter +=1;
}

//old play turn
/*


    public void playTurn(){
       switch(response_counter){
          case 1:
            switch(button_num){
                case 1:
                if(isFocused){
                jim.text = "FNAF is the best movie ever";
                setButtonText(response_counter);
                }else {
                jim.text = "FNAF is the worst movie ever";
                setButtonText(response_counter);
                }
                break;
                case 2:
                jim.text = "Skyrim is the best game ever";
                setButtonText(response_counter);
                break;    
                case 3:
                jim.text = "Jagger is always correct";
                setButtonText(response_counter);
                break;
               }
           break;
            case 2:
                switch(button_num){
                    case 1:
                    jim.text = "Andrew is innocent";
                    setButtonText(response_counter);
                    break;
                    case 2:
                    jim.text = "james is a baker. He bakes many good things like breads,cakes, and pastries. I especially like bagels with cream cheese it's just sooooooooooooooooo good";
                    setButtonText(response_counter);
                    break;    
                    case 3:
                    jim.text = "Jagger did it";
                    setButtonText(response_counter);
                    break;
                    }
           break;
            case 3:
                switch(button_num){
                    case 1:
                    jim.text = "Andrew is a wizard";
                    setButtonText(response_counter);
                    break;
                    case 2:
                    jim.text = "james is a baker.";
                    setButtonText(response_counter);
                    break;    
                    case 3:
                    jim.text = "Jagger sus";
                    setButtonText(response_counter);
                    break;
                    }
           break;
            case 4:
                switch(button_num){
                    case 1:
                    jim.text = "Andrew is a potter";
                    setButtonText(response_counter);
                    break;
                    case 2:
                    jim.text = "james is just here.";
                    setButtonText(response_counter);
                    break;    
                    case 3:
                    jim.text = "Jagger is a gamer";
                    setButtonText(response_counter);
                    break;
                    }
           break;
            case 5:
                switch(button_num){
                    case 1:
                    jim.text = "Andrew is cool";
                    setButtonText(response_counter);
                    break;
                    case 2:
                    jim.text = "james is good with bread knives.";
                    setButtonText(response_counter);
                    break;    
                    case 3:
                    jim.text = "Jagger hates all of us";
                    setButtonText(response_counter);
                    break;
                    }
           break;
}
response_counter += 1;
Debug.Log(response_counter);
} */
}
}