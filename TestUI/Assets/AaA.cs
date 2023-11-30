using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class AaA : MonoBehaviour
{
    [SerializeField] private UIDocument _uiDocument;
    
    // rootVisualElement is the main container for each VisualElement in a hierarchy
    private VisualElement _root;
    
    
    // Main Menu
    private Button _Play;
    private Button _Options;
    private Button _Exit;
    
    private VisualElement _MainMenu;
    
    
    // Options Menu
    private MinMaxSlider _MinMaxSlider;
    private Slider _Slider;
    private DropdownField _DropdownField;
    private Toggle _Toggle;
    private RadioButtonGroup _RadioButtonGroup;
    
    private List<Label> _Labels = new();
    
    private Button _Back;
    
    private VisualElement _OptionsMenu;
    
    // Pause Menu
    private Button _Pause;
    
    private VisualElement _GameMenu;
    
    // Game Paused Menu
    private Button _Resume;
    private Button _Settings;
    private Button _MainMenuButton;
    
    private VisualElement _GamePausedMenu;
    
    public bool _isInGame;
    
    public void Awake()
    {
        _root = _uiDocument.rootVisualElement;
        
        _Play = _root.Q<Button>("Play");
        _Options = _root.Q<Button>("Options");
        _Exit = _root.Q<Button>("Quit");
        
        _MainMenu = _root.Q<VisualElement>("MainMenu");
        
        _Play.clicked += Play;
        _Options.clicked += Options;
        _Exit.clicked += Exit;
        
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        
        _OptionsMenu = _root.Q<VisualElement>("OptionsMenu");
        
        _MinMaxSlider = _root.Q<MinMaxSlider>("MinMaxSlider");
        _Slider = _root.Q<Slider>("Slider");
        _DropdownField = _root.Q<DropdownField>("Dropdown");
        _Toggle = _root.Q<Toggle>("Toggle");
        _RadioButtonGroup = _root.Q<RadioButtonGroup>("RadioButtonGroup");
        _Back = _root.Q<Button>("Back");
        
        _Labels.Add(_root.Q<Label>("Label1"));
        _Labels.Add(_root.Q<Label>("Label2"));
        _Labels.Add(_root.Q<Label>("Label3"));
        _Labels.Add(_root.Q<Label>("Label4"));
        
        _DropdownField.choices = DropDownEnumExtensions.GetDropDownEnumList();
        
        _RadioButtonGroup.choices = DropDownEnumExtensions.GetDropDownEnumList();
        
        _MinMaxSlider.RegisterValueChangedCallback(evt => _Labels[0].text = $"Min: {evt.newValue.x} Max: {evt.newValue.y}");
        _Slider.RegisterValueChangedCallback(evt => _Labels[1].text = $"Slider: {evt.newValue}");
        _DropdownField.RegisterValueChangedCallback(evt => _Labels[2].text = $"Dropdown: {evt.newValue}");
        _Toggle.RegisterValueChangedCallback(evt => _Labels[3].text = $"Toggle: {evt.newValue}");
        
        _Back.clicked += Back;
        
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        
        _GameMenu = _root.Q<VisualElement>("GameMenu");
        
        _Pause = _root.Q<Button>("Pause");
        
        _Pause.clicked += Pause;
        
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        
        _GamePausedMenu = _root.Q<VisualElement>("GamePausedMenu");
        
        _Resume = _root.Q<Button>("Resume");
        _Settings = _root.Q<Button>("Settings");
        _MainMenuButton = _root.Q<Button>("MainMenuButton");
        
        _Resume.clicked += Resume;
        _Settings.clicked += Settings;
        _MainMenuButton.clicked += MainMenuButton;
        
        if (!_isInGame) SwitchToOptions(false);
        else
        {
            _MainMenu.style.display = DisplayStyle.None;
            _OptionsMenu.style.display = DisplayStyle.None;
            _GameMenu.style.display = DisplayStyle.Flex;
        }
    }
    
    private  void Resume()
    {
        _GamePausedMenu.style.display = DisplayStyle.None;
        _GameMenu.style.display = DisplayStyle.Flex;
    }
    
    private  void Settings()
    {
        SwitchToOptions(true);
    }

    private  void MainMenuButton()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("SampleScene");
    }

    private  void Pause()
    {
        _GamePausedMenu.style.display = DisplayStyle.Flex;
        _GameMenu.style.display = DisplayStyle.None;
    }

    private  void Back()
    { 
        SwitchToOptions(false);
    }

    private  void Play()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("A");
    }
    
    private  void Options()
    {
        SwitchToOptions(true);
    }
    
    private  void SwitchToOptions(bool switchToOptions)
    {
        if (_isInGame)
        {
            _GamePausedMenu.style.display = switchToOptions ? DisplayStyle.None : DisplayStyle.Flex;
        }
        else
        {
            _MainMenu.style.display = switchToOptions ? DisplayStyle.None : DisplayStyle.Flex;
        }
        
        _OptionsMenu.style.display = switchToOptions ? DisplayStyle.Flex : DisplayStyle.None;
    }
    
    private  void Exit()
    {
        Application.Quit();
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
