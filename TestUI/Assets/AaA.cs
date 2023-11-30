using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class AaA : MonoBehaviour
{
    [SerializeField] private UIDocument _uiDocument;
    
    private VisualElement _root;
    
    private Button _Play;
    private Button _Options;
    private Button _Exit;
    
    private VisualElement _MainMenu;
    
    private Button _Back;
    
    private VisualElement _OptionsMenu;
    
    
    private void Awake()
    {
        _root = _uiDocument.rootVisualElement;
        
        _Play = _root.Q<Button>("Play");
        _Options = _root.Q<Button>("Options");
        _Exit = _root.Q<Button>("Quit");
        
        _MainMenu = _root.Q<VisualElement>("MainMenu");
        
        _Play.clicked += Play;
        _Options.clicked += Options;
        _Exit.clicked += Exit;
        
        _OptionsMenu = _root.Q<VisualElement>("OptionsMenu");
        
        _Back = _root.Q<Button>("Back");
        
        
        _Back.clicked += Back;
    }

    private void Back()
    {
        SwitchToOptions(false);
    }

    private void Play()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("A");
    }
    
    private void Options()
    {
        SwitchToOptions(true);
    }
    
    private void SwitchToOptions(bool switchToOptions)
    {
        _OptionsMenu.style.display = switchToOptions ? DisplayStyle.Flex : DisplayStyle.None;
        _MainMenu.style.display = switchToOptions ? DisplayStyle.None : DisplayStyle.Flex;
    }
    
    private void Exit()
    {
        Application.Quit();
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
