using TMPro;
using UnityEngine;
using System.Collections;

public class TextManager : MonoBehaviour
{
    public static TextManager Instance;

    #region Variables

    #region public
    /// <summary>
    /// For when you want to skip text you can assign a button to do it with.
    /// </summary>
    public KeyCode skipKey;

    /// <summary>
    /// This displays the text in the scene.
    /// </summary>
    public TextMeshProUGUI dialogueTextObject;

    public string[] dialogueCell1;
    public string[] dialogueCell2;
    public string[] dialogueCell3;
    
    /// <summary>
    /// The speed at which the text gets displayed.
    /// </summary>
    public float textSpeed = 0.05f;
    #endregion

    #region private

    /// <summary>
    /// The current text from the array.
    /// </summary>
    private int currentDialogueIndex = 0;

    /// <summary>
    ///  If the text is still being generated. 
    /// </summary>
    private bool isTyping = false;

    /// <summary>
    /// to check if the coroutine is displaying text.
    /// </summary>
    public Coroutine textDisplayCoroutine;

    /// <summary>
    /// The text you want to be displaying.
    /// </summary>
    [SerializeField] private string[] mainDialogue;

    /// <summary>
    /// The panel containing the dialogue text.
    /// </summary>
    [SerializeField] private GameObject dialoguePanel;

    #endregion Private

    #endregion Variables

    #region Functions

    #region Public

    #region DialogueOnOrOff(bool)
    /// <summary>
    /// Turns the dialogue panel on or off.
    /// </summary>
    /// <param name="isOn">True to show the panel, false to hide it.</param>
    public void DialogueOnOrOff(bool isOn) { dialoguePanel.SetActive(isOn); }
    #endregion

    public void WhatToDisplay(string whatToDisplay)
    {
        if (isTyping)
        {
            StopAllCoroutines();

            StartCoroutine(DisplayText(whatToDisplay));
        }
    }

    #region DisplayText(string)
    /// <summary>
    /// This coroutine displays the text it gets given.
    /// </summary>
    /// <param name="dialogue"></param>
    /// <returns></returns>
    private IEnumerator DisplayText(string dialogue)
    {
        Debug.Log(IsInvoking());
        DialogueOnOrOff(true);

        dialogueTextObject.text = "";

        TypingControl(true);

        // This types out the letters that it got from the array
        for (int i = 0; i < dialogue.Length; i++)
        {
            char letter = dialogue[i];
            dialogueTextObject.text += letter;
            yield return new WaitForSeconds(textSpeed);
        }

        TypingControl(false);
    }
    #endregion DisplayText(string)

    #endregion Public

    #region Private

    #region Awake()
    private void Awake()
    {
        // Checks for if there is already a singelton in the scene if so it removes itself.
        if (Instance == null) { Instance = this; }
        else { Destroy(gameObject); }

        // This checks if th skip key has a value or not and assigns it.
        if (skipKey == KeyCode.None) { skipKey = KeyCode.Mouse0; }

        StartCoroutine(DisplayText(mainDialogue[currentDialogueIndex]));
    }
    #endregion

    #region Update()
    private void Update()
    {
        if (Input.GetKeyDown(skipKey))
        {
            // This checks if the text is still putting it out and then completes it.
            if (isTyping) { CompleteTextDisplay(); }
            else if (currentDialogueIndex < mainDialogue.Length)
            {
                // Check if the current displayed text matches the dialogue line
                if (dialogueTextObject.text == mainDialogue[currentDialogueIndex])
                {
                    currentDialogueIndex++;

                    // Display the next dialogue line
                    if (currentDialogueIndex < mainDialogue.Length)
                    {
                        textDisplayCoroutine = StartCoroutine(DisplayText(mainDialogue[currentDialogueIndex]));
                    }
                    else
                    {
                        // if there is no more dialogue, it then hides the panel
                        DialogueOnOrOff(false);
                    }
                }
                else { CompleteTextDisplay(); }
            }
            else { DialogueOnOrOff(false); }
        }
    }
    #endregion

    #region TypingControl(bool)
    /// <summary>
    /// This is to control the state of typing.
    /// </summary>
    /// <param name="typing"></param>
    private void TypingControl(bool typing) { isTyping = typing; }
    #endregion TypingControl(bool)

    #region CompleteTextDisplay()
    /// <summary>
    /// For when you don't want to wait for the dialogue to finish you can skip it to fully display the text.
    /// </summary>
    private void CompleteTextDisplay()
    {
        // This checks if the coroutine is working and stops it so that it won't impede on the text.
        if (textDisplayCoroutine != null) { StopCoroutine(textDisplayCoroutine); }

        // This completes the text it was trying to output.
        dialogueTextObject.text = mainDialogue[currentDialogueIndex];
        TypingControl(false);
    }
    #endregion CompleteTextDisplay()

    #endregion Private
    #endregion Functions
}