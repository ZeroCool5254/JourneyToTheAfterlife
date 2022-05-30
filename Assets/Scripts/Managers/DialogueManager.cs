using System;
using System.Collections;
using System.Collections.Generic;
using Core;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

using TMPro;
using Ink.Runtime;
using ScriptableObjects;
using ScriptableObjects.Events;

public class DialogueManager : MonoSingleton<DialogueManager>
{
    [SerializeField, Header("HUD")] private GameObject _hudPanel;
    [SerializeField, Header("Dialogue")] private GameObject _dialoguePanel;
    [SerializeField] private TextMeshProUGUI _dialogueText;

    [SerializeField, Header("Choices")] private GameObject[] _choices;
    private TextMeshProUGUI[] _choicesText;

    [SerializeField, Header("Events")] private StartDialogueEvent _dialogueEvent;
    [SerializeField] private EndDialogueEvent _dialogueEndEvent;
    [SerializeField] private TogglePlayerInputEvent _playerInputEvent;

    private InputManager _inputManager;
    private Story _currentStory;

    private bool _isDialoguePlaying;

    private void Start()
    {
        _inputManager = new InputManager();
        _inputManager.UI.Submit.performed += ContinueButton;
        _dialoguePanel.SetActive(false);

        _choicesText = new TextMeshProUGUI[_choices.Length];
        int index = 0;
        foreach (GameObject choice in _choices)
        {
            _choicesText[index] = choice.GetComponentInChildren<TextMeshProUGUI>();
            index++;
        }
    }

    private void OnEnable()
    {
        _dialogueEvent.ActivateDialogueEvent.AddListener(EnterDialogueMode);
    }

    private void OnDisable()
    {
        _dialogueEvent.ActivateDialogueEvent.RemoveListener(EnterDialogueMode);
    }

    private void EnterDialogueMode(TextAsset inkJson)
    {
        _playerInputEvent.DisableInput();
        _inputManager.UI.Enable();
        _currentStory = new Story(inkJson.text);
        _isDialoguePlaying = true;
        _hudPanel.SetActive(false);
        _dialoguePanel.SetActive(true);
        ContinueStory();
    }

    //unfortunately since I am using the new Input system and the standard
    //CallbackContext I have to put this in a function of its own because other 
    //scripts cannot call a function with a CallbackContext parameter.
    private void ContinueButton(InputAction.CallbackContext context)
    {
        if (_currentStory.currentChoices.Count == 0)
        {
            ContinueStory();
        }
    }

    private void ContinueStory()
    {
        
        //return right away if dialogue is not playing
        if (!_isDialoguePlaying) return;
        //handle continuing to the next line in the dialogue when interact is pressed is pressed
        if (_currentStory.canContinue)
        {
            _dialogueText.text = _currentStory.Continue();
            DisplayChoices();
        }
        else
        {
            ExitDialogueMode();
        }
    }

    private void DisplayChoices()
    {
        List<Choice> currentChoices = _currentStory.currentChoices;
        //check to see if we can support all the choices in the list.
        if (currentChoices.Count > _choices.Length)
        {
            Debug.LogError("DialogueManager::MOre choices were given than supported. Number of choices given " + currentChoices.Count);
        }

        int index = 0;
        foreach (Choice choice in currentChoices)
        {
            _choices[index].gameObject.SetActive(true);
            _choicesText[index].text = choice.text;
            index++;
        }

        for (int i = index; i < _choices.Length; i++)
        {
            _choices[i].gameObject.SetActive(false);
        }

        StartCoroutine(SelectFirstChoice());
    }

    private IEnumerator SelectFirstChoice()
    {
        EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
        EventSystem.current.SetSelectedGameObject(_choices[0].gameObject);
    }

    public void MakeChoice(int choiceIndex)
    {
        _currentStory.ChooseChoiceIndex(choiceIndex);
        ContinueStory();
    }
    
    private void ExitDialogueMode()
    {
        _isDialoguePlaying = false;
        _hudPanel.SetActive(true);
        _dialoguePanel.SetActive(false);
        _dialogueText.text = String.Empty;
        _playerInputEvent.EnableInput();
        _inputManager.UI.Disable();
        _dialogueEndEvent.CompleteDialogueEvent.Invoke();
    }
}
