//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.11.2
//     from Assets/Jam/Input/Wagon.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @WagonInput: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @WagonInput()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""Wagon"",
    ""maps"": [
        {
            ""name"": ""Wagon"",
            ""id"": ""50930ac6-0176-44ba-b43b-d5f58e60aebb"",
            ""actions"": [
                {
                    ""name"": ""Left"",
                    ""type"": ""Button"",
                    ""id"": ""2fc77a09-f9b9-47c6-a2f1-1e72396d028c"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Right"",
                    ""type"": ""Button"",
                    ""id"": ""23a9a62d-43b7-492c-be34-50c4ed985759"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Drag"",
                    ""type"": ""Value"",
                    ""id"": ""b13969db-d4dd-4c62-87aa-57aaa033216c"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""EndDrag"",
                    ""type"": ""Button"",
                    ""id"": ""134146ff-a823-4a62-949d-c15a2e4d467b"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""9faaf399-b8ab-4bd3-9e20-fce516cefbd0"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Left"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b223ed80-d040-46ed-a28c-a5cc68347bf7"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Left"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c8421013-6465-44b1-9bc3-7250de80b03a"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Right"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0e1aa967-e057-4f35-b896-01cfa9d8d854"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Right"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2fb7395a-314b-4888-84cc-25fab50bf784"",
                    ""path"": ""<Mouse>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Drag"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f92607cf-0431-4b59-9942-753c72dd48fa"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""EndDrag"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Manual"",
            ""id"": ""5176732d-7fe3-4c60-82b1-7ec5af900a05"",
            ""actions"": [
                {
                    ""name"": ""NextPage"",
                    ""type"": ""Button"",
                    ""id"": ""3056051c-df81-4409-9284-777a45a97dc7"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""PrevPage"",
                    ""type"": ""Button"",
                    ""id"": ""3eb99cd6-b6d9-4048-93b1-48609fbfcf25"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""4908dfaf-8898-42a7-9247-2195c312a2a9"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""NextPage"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2b90193a-d317-4b1b-9bbc-785d73a47e96"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""NextPage"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""42122912-1c18-4cf2-b483-e67ee150e70d"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PrevPage"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5937707c-6ff1-47ff-984c-24b94bac954b"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PrevPage"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Wagon
        m_Wagon = asset.FindActionMap("Wagon", throwIfNotFound: true);
        m_Wagon_Left = m_Wagon.FindAction("Left", throwIfNotFound: true);
        m_Wagon_Right = m_Wagon.FindAction("Right", throwIfNotFound: true);
        m_Wagon_Drag = m_Wagon.FindAction("Drag", throwIfNotFound: true);
        m_Wagon_EndDrag = m_Wagon.FindAction("EndDrag", throwIfNotFound: true);
        // Manual
        m_Manual = asset.FindActionMap("Manual", throwIfNotFound: true);
        m_Manual_NextPage = m_Manual.FindAction("NextPage", throwIfNotFound: true);
        m_Manual_PrevPage = m_Manual.FindAction("PrevPage", throwIfNotFound: true);
    }

    ~@WagonInput()
    {
        UnityEngine.Debug.Assert(!m_Wagon.enabled, "This will cause a leak and performance issues, WagonInput.Wagon.Disable() has not been called.");
        UnityEngine.Debug.Assert(!m_Manual.enabled, "This will cause a leak and performance issues, WagonInput.Manual.Disable() has not been called.");
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }

    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // Wagon
    private readonly InputActionMap m_Wagon;
    private List<IWagonActions> m_WagonActionsCallbackInterfaces = new List<IWagonActions>();
    private readonly InputAction m_Wagon_Left;
    private readonly InputAction m_Wagon_Right;
    private readonly InputAction m_Wagon_Drag;
    private readonly InputAction m_Wagon_EndDrag;
    public struct WagonActions
    {
        private @WagonInput m_Wrapper;
        public WagonActions(@WagonInput wrapper) { m_Wrapper = wrapper; }
        public InputAction @Left => m_Wrapper.m_Wagon_Left;
        public InputAction @Right => m_Wrapper.m_Wagon_Right;
        public InputAction @Drag => m_Wrapper.m_Wagon_Drag;
        public InputAction @EndDrag => m_Wrapper.m_Wagon_EndDrag;
        public InputActionMap Get() { return m_Wrapper.m_Wagon; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(WagonActions set) { return set.Get(); }
        public void AddCallbacks(IWagonActions instance)
        {
            if (instance == null || m_Wrapper.m_WagonActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_WagonActionsCallbackInterfaces.Add(instance);
            @Left.started += instance.OnLeft;
            @Left.performed += instance.OnLeft;
            @Left.canceled += instance.OnLeft;
            @Right.started += instance.OnRight;
            @Right.performed += instance.OnRight;
            @Right.canceled += instance.OnRight;
            @Drag.started += instance.OnDrag;
            @Drag.performed += instance.OnDrag;
            @Drag.canceled += instance.OnDrag;
            @EndDrag.started += instance.OnEndDrag;
            @EndDrag.performed += instance.OnEndDrag;
            @EndDrag.canceled += instance.OnEndDrag;
        }

        private void UnregisterCallbacks(IWagonActions instance)
        {
            @Left.started -= instance.OnLeft;
            @Left.performed -= instance.OnLeft;
            @Left.canceled -= instance.OnLeft;
            @Right.started -= instance.OnRight;
            @Right.performed -= instance.OnRight;
            @Right.canceled -= instance.OnRight;
            @Drag.started -= instance.OnDrag;
            @Drag.performed -= instance.OnDrag;
            @Drag.canceled -= instance.OnDrag;
            @EndDrag.started -= instance.OnEndDrag;
            @EndDrag.performed -= instance.OnEndDrag;
            @EndDrag.canceled -= instance.OnEndDrag;
        }

        public void RemoveCallbacks(IWagonActions instance)
        {
            if (m_Wrapper.m_WagonActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IWagonActions instance)
        {
            foreach (var item in m_Wrapper.m_WagonActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_WagonActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public WagonActions @Wagon => new WagonActions(this);

    // Manual
    private readonly InputActionMap m_Manual;
    private List<IManualActions> m_ManualActionsCallbackInterfaces = new List<IManualActions>();
    private readonly InputAction m_Manual_NextPage;
    private readonly InputAction m_Manual_PrevPage;
    public struct ManualActions
    {
        private @WagonInput m_Wrapper;
        public ManualActions(@WagonInput wrapper) { m_Wrapper = wrapper; }
        public InputAction @NextPage => m_Wrapper.m_Manual_NextPage;
        public InputAction @PrevPage => m_Wrapper.m_Manual_PrevPage;
        public InputActionMap Get() { return m_Wrapper.m_Manual; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(ManualActions set) { return set.Get(); }
        public void AddCallbacks(IManualActions instance)
        {
            if (instance == null || m_Wrapper.m_ManualActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_ManualActionsCallbackInterfaces.Add(instance);
            @NextPage.started += instance.OnNextPage;
            @NextPage.performed += instance.OnNextPage;
            @NextPage.canceled += instance.OnNextPage;
            @PrevPage.started += instance.OnPrevPage;
            @PrevPage.performed += instance.OnPrevPage;
            @PrevPage.canceled += instance.OnPrevPage;
        }

        private void UnregisterCallbacks(IManualActions instance)
        {
            @NextPage.started -= instance.OnNextPage;
            @NextPage.performed -= instance.OnNextPage;
            @NextPage.canceled -= instance.OnNextPage;
            @PrevPage.started -= instance.OnPrevPage;
            @PrevPage.performed -= instance.OnPrevPage;
            @PrevPage.canceled -= instance.OnPrevPage;
        }

        public void RemoveCallbacks(IManualActions instance)
        {
            if (m_Wrapper.m_ManualActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IManualActions instance)
        {
            foreach (var item in m_Wrapper.m_ManualActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_ManualActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public ManualActions @Manual => new ManualActions(this);
    public interface IWagonActions
    {
        void OnLeft(InputAction.CallbackContext context);
        void OnRight(InputAction.CallbackContext context);
        void OnDrag(InputAction.CallbackContext context);
        void OnEndDrag(InputAction.CallbackContext context);
    }
    public interface IManualActions
    {
        void OnNextPage(InputAction.CallbackContext context);
        void OnPrevPage(InputAction.CallbackContext context);
    }
}
