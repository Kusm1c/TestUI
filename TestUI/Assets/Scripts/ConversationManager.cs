using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using Button = UnityEngine.UIElements.Button;

public class ConversationManager : MonoBehaviour
{
    private VisualTreeAsset conversationItemTemplate;
    private VisualTreeAsset choiceButtonTemplate;
    private VisualElement conversationContainer;
    private VisualElement choicesContainer;

    [SerializeField] private UIDocument _uiDocument;
    [SerializeField] private TextAsset _jsonFile;

    public int currentIndex = 0;
    private string currentLanguage = "French"; // Default language
    
    private DropdownField _DropdownField;
    
    public static ConversationManager Instance { get; private set; }
    
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    [Serializable]
    public class ConversationItem
    {
        public string speaker;
        public MultilanguageText text;
        public Choice[] choices;
    }

    [Serializable]
    public class MultilanguageText
    {
        public string English;
        public string French;
        public string Japanese;
    }

    [Serializable]
    public class Choice
    {
        public MultilanguageText text;
        public string next;
    }

    public ConversationItem[] conversation;

    private void Start()
    {
        conversationContainer = _uiDocument.rootVisualElement.Q<VisualElement>("ConversationContainer");
        choicesContainer = _uiDocument.rootVisualElement.Q<VisualElement>("ChoicesContainer");
        _DropdownField = _uiDocument.rootVisualElement.Q<DropdownField>("Dropdown");
        _DropdownField.choices = new List<string>(new[] {"English", "French", "Japanese"});
        _DropdownField.RegisterValueChangedCallback(evt => ChangeLanguage(evt.newValue));

        conversation = LoadConversation(_jsonFile);
        DisplayCurrentItem();
    }

    private void ChangeLanguage(string language)
    {
        Debug.Log($"Changing language to {language}");
        currentLanguage = language;
        conversationContainer.Clear();
        choicesContainer.Clear();
        DisplayCurrentItem();
    }

    private bool _hasChoices;

    private void Update()
    {
        // Check for space key press
        if (Input.GetKeyDown(KeyCode.Space) && !_hasChoices)
        {
            if (currentIndex >= conversation.Length)
            {
                Debug.Log("End of conversation!");
                return;
            }
            if (conversation[currentIndex].choices != null && conversation[currentIndex].choices.Length > 0)
            {
                OnChoiceSelected(conversation[currentIndex].choices[0].next);
            }
            else
            {
                MoveToNextLine();
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            _DropdownField.value = "English";
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ChangeLanguage("French");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ChangeLanguage("Japanese");
        }
    }

    private void MoveToNextLine()
    {
        conversationContainer.Clear();
        choicesContainer.Clear();

        currentIndex++;

        DisplayCurrentItem();
    }

    private ConversationItem[] LoadConversation(TextAsset jsonFile)
    {
        if (jsonFile == null)
        {
            Debug.LogError("JSON file is not assigned!");
            return null;
        }

        string jsonText = jsonFile.text;

        ConversationWrapper wrapper = JsonUtility.FromJson<ConversationWrapper>(jsonText);

        if (wrapper is { conversation: not null } && wrapper.conversation.Length != 0)
            return wrapper.conversation;
        Debug.LogError("Failed to load conversation from JSON file!");
        return null;
    }

    public class ConversationWrapper
    {
        public ConversationItem[] conversation;
    }

    private void DisplayCurrentItem()
    {
        if (currentIndex >= conversation.Length)
        {
            Debug.Log("End of conversation!");
            return;
        }

        ConversationItem currentItem = conversation[currentIndex];

        Label speakerLabel = new Label(currentItem.speaker);
        conversationContainer.Add(speakerLabel);

        Label textLabel = new Label(GetTextForCurrentLanguage(currentItem.text));
        conversationContainer.Add(textLabel);
        textLabel.style.whiteSpace = WhiteSpace.Normal;

        if (currentItem.choices != null && currentItem.choices.Length > 0)
        {
            _hasChoices = true;
            DisplayChoices(currentItem.choices);
        }
    }

    private void DisplayChoices(Choice[] choices)
    {
        foreach (Choice choice in choices)
        {
            Button choiceButton = new Button(() => OnChoiceSelected(choice.next))
            {
                text = GetTextForCurrentLanguage(choice.text)
            };
            choicesContainer.Add(choiceButton);
        }
    }

    private string GetTextForCurrentLanguage(MultilanguageText text)
    {
        switch (currentLanguage)
        {
            case "English":
                return text.English;
            case "French":
                return text.French;
            case "Japanese":
                return text.Japanese;
            default:
                Debug.LogWarning($"Unsupported language: {currentLanguage}. Defaulting to English.");
                return text.English;
        }
    }

    private void OnChoiceSelected(string nextTextKey)
    {
        Choice selectedChoice = conversation[currentIndex].choices.FirstOrDefault(choice => choice.next == nextTextKey);

        if (selectedChoice == null)
        {
            Debug.LogError($"Invalid nextTextKey: {nextTextKey}");
            return;
        }

        conversationContainer.Clear();
        choicesContainer.Clear();

        DisplayChoiceResult(selectedChoice.text);

        _hasChoices = false;
    }


    private void DisplayChoiceResult(MultilanguageText nextText)
    {
        Label textLabel = new Label(GetTextForCurrentLanguage(nextText));
        conversationContainer.Add(textLabel);
        textLabel.style.whiteSpace = WhiteSpace.Normal;

        currentIndex++;
    }
}