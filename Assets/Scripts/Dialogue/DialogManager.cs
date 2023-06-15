using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ink.Runtime;
using UnityEngine.EventSystems;
using UnityEngine.Playables;

public class DialogManager : MonoBehaviour
{
    [Header("Dialogue UI")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private GameObject continueIcon;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private TextMeshProUGUI DisplayNameText;
    [SerializeField] private AudioClip audioClip;
    public Animator DialogPanelAnim,ImageAnim;
    public float typingSpeed;

    [Header("Choices UI")]
    [SerializeField] private GameObject[] choices;
    private TextMeshProUGUI[] choicesText;

    private Story currentStory;
    private int textassetsayi,directorablesayi;
    private Coroutine displayLineCoroutine;
    private AudioSource audioSource;
    private bool canContinueNextLine,isRichtext,Space = false;
    public bool dialogueIsPlaying { get; private set; }
    string sonuncumetn;
    public TextAsset[] InkCutscene;
    [Header("CutScenes")]
    public PlayableDirector[] CutScenes;
    [Header("Choices CutScenes")]
    public PlayableDirector[] ChoicesCutScenes;
    public static DialogManager Instance;
    private const string SPEAKER_TAG = "speaker";
    private const string PORTRAIT_TAG = "portrait";
    private const string PAUSE_TAG = "Pause";
    private const string PLAYCHOICES_TAG = "PlayChoices";

    private void Awake() 
    {
        if (Instance != null)
        {
            Debug.LogWarning("Found more than one Dialogue Manager in the scene");
        }
        Instance = this;
        audioSource = GetComponent<AudioSource>();
    }
    private void Start() 
    {
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);

        // get all of the choices text 
        choicesText = new TextMeshProUGUI[choices.Length];
        int index = 0;
        foreach (GameObject choice in choices) 
        {
            choicesText[index] = choice.GetComponentInChildren<TextMeshProUGUI>();
            index++;
        }
    }

    private void Update() 
    {
        // return right away if dialogue isn't playing
        if (!dialogueIsPlaying) 
        {
            return;
        }

        // handle continuing to the next line in the dialogue when submit is pressed
        if (canContinueNextLine && currentStory.currentChoices.Count == 0 && Input.GetKeyDown(KeyCode.Space) && !Input.GetKey(KeyCode.E) && Space)
        {
            ContinueStory();
        }
    }

    public void EnterDialogueMode(TextAsset inkJSON) 
    {
        DialogPanelAnim.Play("EnterPanel");
        currentStory = new Story(inkJSON.text);
        dialogueIsPlaying = true;
        dialoguePanel.SetActive(true);
        Space = true;

        ContinueStory();
    }

    private IEnumerator ExitDialogueMode() 
    {
        DialogPanelAnim.SetTrigger("Exit");
        yield return new WaitForSeconds(1f);
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
        dialogueText.text = "";
    }

    private void ContinueStory() 
    {
        if (currentStory.canContinue) 
        {
            if(displayLineCoroutine != null)
            {
                StopCoroutine(displayLineCoroutine);
            }
            displayLineCoroutine = StartCoroutine(DisplayLine(currentStory.Continue()));
            HandleTags(currentStory.currentTags);
        }
        else 
        {
            StartCoroutine(ExitDialogueMode());
        }
    }
    private void HandleTags(List<string> currentTags)
    {
        foreach (string tag in currentTags)
        {
            string[] splitTag = tag.Split(':');
            string tagKey = splitTag[0].Trim();
            string tagValue = splitTag[1].Trim();
            switch(tagKey)
            {
                case SPEAKER_TAG:
                DisplayNameText.text = tagValue;
                break;
                case PORTRAIT_TAG:
                ImageAnim.Play(tagValue);
                break;
                case PAUSE_TAG:
                if(tagValue == "CutSceneStop")
                {
                    CutScenes[directorablesayi].Pause();
                    DialogModeCutSceneActive();
                }
                else if(tagValue == "CutScene")
                {
                    CutScenes[directorablesayi].Pause();
                }
                else if(tagValue == "bos")
                {
                    StopCoroutine(displayLineCoroutine);
                    dialogueText.text = sonuncumetn;
                    dialogueText.maxVisibleCharacters = sonuncumetn.Length;
                    continueIcon.SetActive(true);
                    CutScenes[directorablesayi].Play();
                    StartCoroutine(ExitDialogueMode());
                }
                else if(tagValue == "Space")
                {
                    Space = false;
                }
                break;
                case PLAYCHOICES_TAG:
                foreach(PlayableDirector playableDirector in ChoicesCutScenes)
                {
                    if(playableDirector.name == tagValue)
                    {
                        playableDirector.Play();
                    }
                }
                break;
            }
        }
    }
    private IEnumerator DisplayLine(string line)
    {
        dialogueText.text = line;
        dialogueText.maxVisibleCharacters = 0;
        HideChoices();
        canContinueNextLine = false;
        continueIcon.SetActive(false);
        foreach(char letter in line.ToCharArray())
        {
            if(Input.GetKey(KeyCode.E))
            {
                dialogueText.maxVisibleCharacters = line.Length;
                audioSource.Stop();
                break;
            }
            if(letter == '<' || isRichtext)
            {
                isRichtext = true;
                if(letter == '>')
                {
                    isRichtext = false;
                }
            }
            else
            {
                dialogueText.maxVisibleCharacters++;
                yield return new WaitForSeconds(typingSpeed);
                audioSource.PlayOneShot(audioClip);
            }
        }
        DisplayChoices();
        sonuncumetn = line;
        continueIcon.SetActive(true);
        canContinueNextLine = true;
    }
    private void HideChoices()
    {
        foreach(GameObject choicebutton in choices)
        {
            choicebutton.SetActive(false);
        }
    }

    private void DisplayChoices() 
    {
        List<Choice> currentChoices = currentStory.currentChoices;

        if (currentChoices.Count > choices.Length)
        {
            Debug.LogError("More choices were given than the UI can support. Number of choices given: " 
                + currentChoices.Count);
        }

        int index = 0;
        // enable and initialize the choices up to the amount of choices for this line of dialogue
        foreach(Choice choice in currentChoices) 
        {
            choices[index].gameObject.SetActive(true);
            choicesText[index].text = choice.text;
            index++;
        }
        // go through the remaining choices the UI supports and make sure they're hidden
        for (int i = index; i < choices.Length; i++) 
        {
            choices[i].gameObject.SetActive(false);
        }

    }
    public void DialogModeCutSceme()
    {
        directorablesayi++;
        EnterDialogueMode(InkCutscene[textassetsayi]);
        textassetsayi ++;
    }
    public void DialogModeCutSceneActive()
    {
        dialogueIsPlaying = true;
        dialoguePanel.SetActive(true);

        ContinueStory();
    }

    public void MakeChoice(int choiceIndex)
    {
        if(canContinueNextLine)
        {
            currentStory.ChooseChoiceIndex(choiceIndex);
            ContinueStory();
        }
    }

}