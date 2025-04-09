using System;
using Jam.Scripts.Dialogue.Runtime.Enums;
using Jam.Scripts.Dialogue.Runtime.SO;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace Jam.Scripts.Dialogue.Editor.Graph_view
{
    public class DialogueEditorWindow : EditorWindow
    {
        private DialogueContainerSO _currentDialogueContainer;
        private DialogueGraphView _graphView;
        private DialogueSaveAndLoad _saveAndLoad;

        private LanguageType _currentLanguage = LanguageType.English;                  
        private ToolbarMenu _languageDropdownMenu;
        private Label _nameOfDialogueContainer;
        private string _editorWindowStyleSheet = "USS/EditorWindow/EditorWindowStyleSheet";

        public LanguageType CurrentLanguage => _currentLanguage;

        // Callback attribute for opening an asset in Unity (e.g the callback is fired when double clicking an asset in the Project Browser).
        // Read More https://docs.unity3d.com/ScriptReference/Callbacks.OnOpenAssetAttribute.html
        [OnOpenAsset(0)]
        public static bool ShowWindow(int instanceId, int line)
        {
            Object dialogueContainer = EditorUtility.InstanceIDToObject(instanceId);

            if (dialogueContainer is DialogueContainerSO item)
            {
                DialogueEditorWindow window = (DialogueEditorWindow) GetWindow(typeof(DialogueEditorWindow));
                window.titleContent = new GUIContent("Dialogue Editor");
                window._currentDialogueContainer = item;
                window.minSize = new Vector2(500, 250);
                window.Load();
            }

            return false;
        }

        private void OnEnable()
        {
            ConstructGraphView();
            GenerateToolBar();
            Load();
        }

        private void OnDisable()
        {
            rootVisualElement.Remove(_graphView);
        }

        private void ConstructGraphView()
        {
            _graphView = new DialogueGraphView(this);
            _graphView.StretchToParentSize();
            rootVisualElement.Add(_graphView);
            _saveAndLoad = new DialogueSaveAndLoad(_graphView);
        }
        
        /// <summary>
        /// Generate the toolbar you will see in the top left of the dialogue editor window.
        /// </summary>
        private void GenerateToolBar()
        {
            
            // Find and load the styleSheet for graph view.
            StyleSheet styleSheet = Resources.Load<StyleSheet>(_editorWindowStyleSheet);
            // Add the styleSheet for graph view.
            rootVisualElement.styleSheets.Add(styleSheet);
            
            Toolbar toolbar = new Toolbar();

            //save button
            Button saveBtn = new Button
            {
                text = "Save"
            };
            saveBtn.clicked += Save;

            toolbar.Add(saveBtn);
            
            //load button
            Button loadBtn = new Button
            {
                text = "Load"
            };
            loadBtn.clicked += Load;
            
            toolbar.Add(loadBtn);
            
            //Dropdown menu for languages
            _languageDropdownMenu = new ToolbarMenu();
            foreach (LanguageType language in Enum.GetValues(typeof(LanguageType)))
            {
                // Here we go through each language and make a button with that language.
                // When you click on the language in the dropdown menu we tell it to run Language(language) method.
                _languageDropdownMenu.menu.AppendAction(language.ToString(), x => Language(language));
            }

            toolbar.Add(_languageDropdownMenu);
            
            //CharacterName of current DialogueContainer opened dialogue.
            _nameOfDialogueContainer = new Label("");
            toolbar.Add(_nameOfDialogueContainer);
            _nameOfDialogueContainer.AddToClassList("nameOfDialogueContainer");

            rootVisualElement.Add(toolbar);
        }

        private void Load()
        {
            if (_currentDialogueContainer != null)
            {
                Language(LanguageType.English);
                _nameOfDialogueContainer.text = "Container name:   " + _currentDialogueContainer.name;
                _saveAndLoad.Load(_currentDialogueContainer);
            }
        }

        private void Save()
        {
            if (_currentDialogueContainer != null)
                _saveAndLoad.Save(_currentDialogueContainer);
        }

        private void Language(LanguageType languageType)
        {
            _languageDropdownMenu.text = $"Language: {languageType.ToString()}";
            _currentLanguage = languageType;
            _graphView.ReloadLanguage();
        }
    }
}
