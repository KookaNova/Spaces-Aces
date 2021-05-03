// GENERATED AUTOMATICALLY FROM 'Assets/Controller/ControlInputActions.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @ControlInputActions : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @ControlInputActions()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""ControlInputActions"",
    ""maps"": [
        {
            ""name"": ""Flight"",
            ""id"": ""333f7330-3c33-4c2e-a054-b2625a7a9b71"",
            ""actions"": [
                {
                    ""name"": ""MenuButton"",
                    ""type"": ""Button"",
                    ""id"": ""cc0bce3e-3366-4aff-9e47-3f802c411806"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Thrust"",
                    ""type"": ""Value"",
                    ""id"": ""39feed91-9511-46e5-ad51-1861b7e92efc"",
                    ""expectedControlType"": ""Analog"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Brake"",
                    ""type"": ""Button"",
                    ""id"": ""3fc556eb-640e-42df-a3d5-da8bb14c2e19"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Yaw"",
                    ""type"": ""Button"",
                    ""id"": ""d1a91c8b-2259-4247-94b2-3818ad2383bb"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Torque"",
                    ""type"": ""Value"",
                    ""id"": ""30f87051-4450-490b-a645-54055514bcf6"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": ""InvertVector2(invertY=false)"",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Gun Fire"",
                    ""type"": ""PassThrough"",
                    ""id"": ""c608c628-d07a-4850-afe5-190bd92bcf65"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": ""InvertVector2(invertY=false)"",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""Aim Gun"",
                    ""type"": ""PassThrough"",
                    ""id"": ""611c7b84-62e4-4476-9d1a-85822f47935b"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""CameraChange"",
                    ""type"": ""Button"",
                    ""id"": ""d9730d32-a327-48b3-9fc7-316b4c39a803"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=2)""
                },
                {
                    ""name"": ""ChangeTargetMode"",
                    ""type"": ""Button"",
                    ""id"": ""d252cb79-d319-4f5e-9a1d-3b1ed2e5ba59"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""fac45eca-6e1d-4158-b987-bb92780711c8"",
                    ""path"": ""<Gamepad>/rightTrigger"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": ""Basic Controls"",
                    ""action"": ""Thrust"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2fc076f5-50f2-4f83-b952-3fc255d6cd31"",
                    ""path"": ""<Keyboard>/shift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Basic Controls"",
                    ""action"": ""Thrust"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""de0a9a32-9db3-44a2-97e1-868296276cd6"",
                    ""path"": ""<Gamepad>/leftTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Basic Controls"",
                    ""action"": ""Brake"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""362ec941-f4b8-490e-8724-5db39dcf73af"",
                    ""path"": ""<Keyboard>/ctrl"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Basic Controls"",
                    ""action"": ""Brake"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c8b779b3-ad93-4231-ad1c-c14165025dd6"",
                    ""path"": ""<Gamepad>/start"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Basic Controls"",
                    ""action"": ""MenuButton"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""41c2f2da-e427-4029-a735-f8524f8d0d77"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Basic Controls"",
                    ""action"": ""MenuButton"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""Gamepad Yaw Axis"",
                    ""id"": ""80446557-a95a-41e8-96dc-411054cccb23"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Yaw"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""e21a66bd-c442-426a-bf78-e211ed60cc2b"",
                    ""path"": ""<Gamepad>/leftShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Basic Controls"",
                    ""action"": ""Yaw"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""0551144f-1705-4813-84ea-5ad1f57d7a22"",
                    ""path"": ""<Gamepad>/rightShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Basic Controls"",
                    ""action"": ""Yaw"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Keyboard Yaw Axis"",
                    ""id"": ""6954eea3-734b-4da9-a9c5-c352250e54dd"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Yaw"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""68386f12-983e-475e-8cd7-a3a4fccd1030"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Basic Controls"",
                    ""action"": ""Yaw"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""62d52197-e995-48ec-9990-6decbff48d50"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Basic Controls"",
                    ""action"": ""Yaw"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""2a9738b9-2b00-464d-bf1d-f74b44de707f"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Torque"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""Keyboard Torque"",
                    ""id"": ""954c352f-1c73-42a6-be9b-0b59ed9093bf"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Torque"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""0b2bd09a-8a97-46f5-82c0-b79808f309c4"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Torque"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""c6db8b3b-de9f-4d12-9abd-5c2e833c94a3"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Torque"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""1af661ce-d248-4b96-bda3-834f9e71ea4a"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Torque"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""72159607-ee21-44ca-8000-02de259bc587"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Torque"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""f8e2b713-84c8-408f-aa02-7d7e7053bd72"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Basic Controls"",
                    ""action"": ""Gun Fire"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a9dd8ff5-c3aa-4096-bec8-b9947d1d5c38"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Gun Fire"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""fe3afac4-0ebd-4f3e-bceb-0202c4962cdc"",
                    ""path"": ""<Mouse>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Aim Gun"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""db551c3e-7d6d-4946-a50d-7cd523d3fcb4"",
                    ""path"": ""<Joystick>/stick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Aim Gun"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""cef75e7c-c2bb-49aa-ba1a-18eee0f2995f"",
                    ""path"": ""<Keyboard>/c"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Basic Controls"",
                    ""action"": ""CameraChange"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ef3ad97a-be8d-4247-b6d2-7c015cbce6cb"",
                    ""path"": ""<Keyboard>/t"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ChangeTargetMode"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Menu"",
            ""id"": ""2721e964-e3a0-4624-89a4-b3bbc6e4eb41"",
            ""actions"": [
                {
                    ""name"": ""Escape Button"",
                    ""type"": ""Button"",
                    ""id"": ""97a723eb-ba74-4a02-bf16-88979aa79ae3"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""dbf9a980-ef76-48e1-b0f8-3d6a42e09c86"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Escape Button"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Basic Controls"",
            ""bindingGroup"": ""Basic Controls"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Gamepad>"",
                    ""isOptional"": true,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // Flight
        m_Flight = asset.FindActionMap("Flight", throwIfNotFound: true);
        m_Flight_MenuButton = m_Flight.FindAction("MenuButton", throwIfNotFound: true);
        m_Flight_Thrust = m_Flight.FindAction("Thrust", throwIfNotFound: true);
        m_Flight_Brake = m_Flight.FindAction("Brake", throwIfNotFound: true);
        m_Flight_Yaw = m_Flight.FindAction("Yaw", throwIfNotFound: true);
        m_Flight_Torque = m_Flight.FindAction("Torque", throwIfNotFound: true);
        m_Flight_GunFire = m_Flight.FindAction("Gun Fire", throwIfNotFound: true);
        m_Flight_AimGun = m_Flight.FindAction("Aim Gun", throwIfNotFound: true);
        m_Flight_CameraChange = m_Flight.FindAction("CameraChange", throwIfNotFound: true);
        m_Flight_ChangeTargetMode = m_Flight.FindAction("ChangeTargetMode", throwIfNotFound: true);
        // Menu
        m_Menu = asset.FindActionMap("Menu", throwIfNotFound: true);
        m_Menu_EscapeButton = m_Menu.FindAction("Escape Button", throwIfNotFound: true);
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

    // Flight
    private readonly InputActionMap m_Flight;
    private IFlightActions m_FlightActionsCallbackInterface;
    private readonly InputAction m_Flight_MenuButton;
    private readonly InputAction m_Flight_Thrust;
    private readonly InputAction m_Flight_Brake;
    private readonly InputAction m_Flight_Yaw;
    private readonly InputAction m_Flight_Torque;
    private readonly InputAction m_Flight_GunFire;
    private readonly InputAction m_Flight_AimGun;
    private readonly InputAction m_Flight_CameraChange;
    private readonly InputAction m_Flight_ChangeTargetMode;
    public struct FlightActions
    {
        private @ControlInputActions m_Wrapper;
        public FlightActions(@ControlInputActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @MenuButton => m_Wrapper.m_Flight_MenuButton;
        public InputAction @Thrust => m_Wrapper.m_Flight_Thrust;
        public InputAction @Brake => m_Wrapper.m_Flight_Brake;
        public InputAction @Yaw => m_Wrapper.m_Flight_Yaw;
        public InputAction @Torque => m_Wrapper.m_Flight_Torque;
        public InputAction @GunFire => m_Wrapper.m_Flight_GunFire;
        public InputAction @AimGun => m_Wrapper.m_Flight_AimGun;
        public InputAction @CameraChange => m_Wrapper.m_Flight_CameraChange;
        public InputAction @ChangeTargetMode => m_Wrapper.m_Flight_ChangeTargetMode;
        public InputActionMap Get() { return m_Wrapper.m_Flight; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(FlightActions set) { return set.Get(); }
        public void SetCallbacks(IFlightActions instance)
        {
            if (m_Wrapper.m_FlightActionsCallbackInterface != null)
            {
                @MenuButton.started -= m_Wrapper.m_FlightActionsCallbackInterface.OnMenuButton;
                @MenuButton.performed -= m_Wrapper.m_FlightActionsCallbackInterface.OnMenuButton;
                @MenuButton.canceled -= m_Wrapper.m_FlightActionsCallbackInterface.OnMenuButton;
                @Thrust.started -= m_Wrapper.m_FlightActionsCallbackInterface.OnThrust;
                @Thrust.performed -= m_Wrapper.m_FlightActionsCallbackInterface.OnThrust;
                @Thrust.canceled -= m_Wrapper.m_FlightActionsCallbackInterface.OnThrust;
                @Brake.started -= m_Wrapper.m_FlightActionsCallbackInterface.OnBrake;
                @Brake.performed -= m_Wrapper.m_FlightActionsCallbackInterface.OnBrake;
                @Brake.canceled -= m_Wrapper.m_FlightActionsCallbackInterface.OnBrake;
                @Yaw.started -= m_Wrapper.m_FlightActionsCallbackInterface.OnYaw;
                @Yaw.performed -= m_Wrapper.m_FlightActionsCallbackInterface.OnYaw;
                @Yaw.canceled -= m_Wrapper.m_FlightActionsCallbackInterface.OnYaw;
                @Torque.started -= m_Wrapper.m_FlightActionsCallbackInterface.OnTorque;
                @Torque.performed -= m_Wrapper.m_FlightActionsCallbackInterface.OnTorque;
                @Torque.canceled -= m_Wrapper.m_FlightActionsCallbackInterface.OnTorque;
                @GunFire.started -= m_Wrapper.m_FlightActionsCallbackInterface.OnGunFire;
                @GunFire.performed -= m_Wrapper.m_FlightActionsCallbackInterface.OnGunFire;
                @GunFire.canceled -= m_Wrapper.m_FlightActionsCallbackInterface.OnGunFire;
                @AimGun.started -= m_Wrapper.m_FlightActionsCallbackInterface.OnAimGun;
                @AimGun.performed -= m_Wrapper.m_FlightActionsCallbackInterface.OnAimGun;
                @AimGun.canceled -= m_Wrapper.m_FlightActionsCallbackInterface.OnAimGun;
                @CameraChange.started -= m_Wrapper.m_FlightActionsCallbackInterface.OnCameraChange;
                @CameraChange.performed -= m_Wrapper.m_FlightActionsCallbackInterface.OnCameraChange;
                @CameraChange.canceled -= m_Wrapper.m_FlightActionsCallbackInterface.OnCameraChange;
                @ChangeTargetMode.started -= m_Wrapper.m_FlightActionsCallbackInterface.OnChangeTargetMode;
                @ChangeTargetMode.performed -= m_Wrapper.m_FlightActionsCallbackInterface.OnChangeTargetMode;
                @ChangeTargetMode.canceled -= m_Wrapper.m_FlightActionsCallbackInterface.OnChangeTargetMode;
            }
            m_Wrapper.m_FlightActionsCallbackInterface = instance;
            if (instance != null)
            {
                @MenuButton.started += instance.OnMenuButton;
                @MenuButton.performed += instance.OnMenuButton;
                @MenuButton.canceled += instance.OnMenuButton;
                @Thrust.started += instance.OnThrust;
                @Thrust.performed += instance.OnThrust;
                @Thrust.canceled += instance.OnThrust;
                @Brake.started += instance.OnBrake;
                @Brake.performed += instance.OnBrake;
                @Brake.canceled += instance.OnBrake;
                @Yaw.started += instance.OnYaw;
                @Yaw.performed += instance.OnYaw;
                @Yaw.canceled += instance.OnYaw;
                @Torque.started += instance.OnTorque;
                @Torque.performed += instance.OnTorque;
                @Torque.canceled += instance.OnTorque;
                @GunFire.started += instance.OnGunFire;
                @GunFire.performed += instance.OnGunFire;
                @GunFire.canceled += instance.OnGunFire;
                @AimGun.started += instance.OnAimGun;
                @AimGun.performed += instance.OnAimGun;
                @AimGun.canceled += instance.OnAimGun;
                @CameraChange.started += instance.OnCameraChange;
                @CameraChange.performed += instance.OnCameraChange;
                @CameraChange.canceled += instance.OnCameraChange;
                @ChangeTargetMode.started += instance.OnChangeTargetMode;
                @ChangeTargetMode.performed += instance.OnChangeTargetMode;
                @ChangeTargetMode.canceled += instance.OnChangeTargetMode;
            }
        }
    }
    public FlightActions @Flight => new FlightActions(this);

    // Menu
    private readonly InputActionMap m_Menu;
    private IMenuActions m_MenuActionsCallbackInterface;
    private readonly InputAction m_Menu_EscapeButton;
    public struct MenuActions
    {
        private @ControlInputActions m_Wrapper;
        public MenuActions(@ControlInputActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @EscapeButton => m_Wrapper.m_Menu_EscapeButton;
        public InputActionMap Get() { return m_Wrapper.m_Menu; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(MenuActions set) { return set.Get(); }
        public void SetCallbacks(IMenuActions instance)
        {
            if (m_Wrapper.m_MenuActionsCallbackInterface != null)
            {
                @EscapeButton.started -= m_Wrapper.m_MenuActionsCallbackInterface.OnEscapeButton;
                @EscapeButton.performed -= m_Wrapper.m_MenuActionsCallbackInterface.OnEscapeButton;
                @EscapeButton.canceled -= m_Wrapper.m_MenuActionsCallbackInterface.OnEscapeButton;
            }
            m_Wrapper.m_MenuActionsCallbackInterface = instance;
            if (instance != null)
            {
                @EscapeButton.started += instance.OnEscapeButton;
                @EscapeButton.performed += instance.OnEscapeButton;
                @EscapeButton.canceled += instance.OnEscapeButton;
            }
        }
    }
    public MenuActions @Menu => new MenuActions(this);
    private int m_BasicControlsSchemeIndex = -1;
    public InputControlScheme BasicControlsScheme
    {
        get
        {
            if (m_BasicControlsSchemeIndex == -1) m_BasicControlsSchemeIndex = asset.FindControlSchemeIndex("Basic Controls");
            return asset.controlSchemes[m_BasicControlsSchemeIndex];
        }
    }
    public interface IFlightActions
    {
        void OnMenuButton(InputAction.CallbackContext context);
        void OnThrust(InputAction.CallbackContext context);
        void OnBrake(InputAction.CallbackContext context);
        void OnYaw(InputAction.CallbackContext context);
        void OnTorque(InputAction.CallbackContext context);
        void OnGunFire(InputAction.CallbackContext context);
        void OnAimGun(InputAction.CallbackContext context);
        void OnCameraChange(InputAction.CallbackContext context);
        void OnChangeTargetMode(InputAction.CallbackContext context);
    }
    public interface IMenuActions
    {
        void OnEscapeButton(InputAction.CallbackContext context);
    }
}
