using UnityEngine;
using UnityEngine.UIElements;
using System.Collections;
using System.Collections.Generic;

public class Menu : MonoBehaviour
{
    // whether listening for user input 
    private bool _isListening;

    // UI document, assigned in Inspector 
    public UIDocument doc;

    // fade transition script, assigned in Inspector 
    public FadeUI fade;

    // awake 
    private void Awake()
    {

        // stop listening for input 
        _isListening = false;

        // register interaction events 
        RegisterEvents();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created 
    private void Start()
    {
        // AUDIO if not null 
        /*if (AudioManager.Instance != null) { 
            // play bgm 
            AudioManager.Instance.PlayClipFromSource( 
                AudioManger.Instance.bgmMenu, 
                AudioManager.Instance.bgmSource 
                ); 
 
            // fade music 
            AudioManager.Instance.ToggleFade(); 
        }*/

        // reveal scene 
        fade.Reveal();

        StartCoroutine(EnableInputWithDelay(fade.duration));
    }


    private void RegisterEvents()
    {
        /*The UI Toolkit is managed in code via a UI Document.  
        Every UI Document has a root, which can be accessed in code. 
        From there, a query system allows us to access each individual 
        UI element. Further, we can register callback functions that  
        execute whenever a specific user interaction occurs on a UI element.*/

        // retrieve UI Document root 
        VisualElement root = doc.rootVisualElement;

        // retrieve UI elements 
        //buttons 
        List<VisualElement> btns = root.Query(className: "ButtonStyle").ToList();

        // iterate through buttons 
        foreach (VisualElement aBtn in btns)
        {

            // register interaction events 
            aBtn.RegisterCallback<ClickEvent, VisualElement>(OnClickBtn, aBtn);
        }
    }

    private IEnumerator EnableInputWithDelay(float theDelay)
    {
        /*Attribute coroutine is used to allow the execution of the 
        function over multiple frames. 
 
        This allows us to easily use Unity's built-in function to display a logo, 
        fade in and out or play audio.*/

        // delay 
        yield return new WaitForSeconds(theDelay);

        // listen for input 
        _isListening = true;
    }

    // handle button clicks 
    private void OnClickBtn(ClickEvent theEvent, VisualElement theBtn)
    {
        /* this function is executed any time a button is clicked. 
        If we are listening for input, we proceed to determine 
        which button was clicked and then transition to the next scene 
        accordingly. */

        // if listening for input 
        if (_isListening == true)
        {

            // toggle flag 
            _isListening = false; // if input is allowed we stopped immediately to prevent multiple input 

            // audio if not null 
            /*if (AudioManager.Instance != null) 
            { 
                // fade bgm 
                if (AudioManager.Instance.bgmSource.clip != null) { 
                    AudioManager.Instance.ToggleFade(); 
                } 
            }*/

            // conceal scene 
            fade.Conceal(); // fade out the menu screen 

            // load button 
            if (theBtn.name == "BtnPlay")
            {
                // switch scene 
                StartCoroutine(StateManager.Instance.SwitchSceneTo("Play", fade.duration));
            }

        }
    }

    // Update is called once per frame 
    void Update()
    {

    }
}
