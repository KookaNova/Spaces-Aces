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
                    ""type"": ""PassThrough"",
                    ""id"": ""39feed91-9511-46e5-ad51-1861b7e92efc"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Brake"",
                    ""type"": ""PassThrough"",
                    ""id"": ""3fc556eb-640e-42df-a3d5-da8bb14c2e19"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Yaw"",
                    ""type"": ""Value"",
                    ""id"": ""d1a91c8b-2259-4247-94b2-3818ad2383bb"",
                    ""expectedControlType"": """",
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
                    ""name"": ""CameraChange"",
                    ""type"": ""Button"",
                    ""id"": ""d9730d32-a327-48b3-9fc7-316b4c39a803"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""TargetModeAdd"",
                    ""type"": ""PassThrough"",
                    ""id"": ""d252cb79-d319-4f5e-9a1d-3b1ed2e5ba59"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""TargetModeSub"",
                    ""type"": ""PassThrough"",
                    ""id"": ""ebda4c52-725e-49ec-8885-8fc11c20b319"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""CycleTargets"",
                    ""type"": ""Button"",
                    ""id"": ""4f705ef8-9272-42d1-80eb-76a198b91983"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""MissileButton"",
                    ""type"": ""Button"",
                    ""id"": ""831ca93d-ed42-46b1-b749-770b1ee0df39"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Primary Ability"",
                    ""type"": ""PassThrough"",
                    ""id"": ""c794cb57-8f84-4362-88e9-d22889c3201c"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Secondary Ability"",
                    ""type"": ""PassThrough"",
                    ""id"": ""d2f5022b-a86b-448b-9422-99bd9d1fccd8"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Ace Ability"",
                    ""type"": ""PassThrough"",
                    ""id"": ""4003d615-b0dd-4507-a7a0-4954a08fdef2"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""CameraStick"",
                    ""type"": ""Value"",
                    ""id"": ""b43eafe0-6452-4c98-95ba-3e12b1fe2f1d"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""CameraTargetLock"",
                    ""type"": ""Button"",
                    ""id"": ""450277f3-9615-4df5-9e5d-a7bc680ed88a"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""CameraMouse"",
                    ""type"": ""Value"",
                    ""id"": ""64d68f33-742e-4614-8393-e7082a7f3c40"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""fac45eca-6e1d-4158-b987-bb92780711c8"",
                    ""path"": ""<Gamepad>/rightShoulder"",
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
                    ""path"": ""<Gamepad>/leftShoulder"",
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
                    ""path"": ""<Gamepad>/leftTrigger"",
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
                    ""path"": ""<Gamepad>/rightTrigger"",
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
                    ""id"": ""2b6cfe15-38b0-4cca-944a-8c69c06a7a5f"",
                    ""path"": ""<Gamepad>/dpad/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Gun Fire"",
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
                    ""id"": ""732e4333-0e93-479c-9ebe-9dad4be63849"",
                    ""path"": ""<Gamepad>/rightStickPress"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""CameraChange"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ef3ad97a-be8d-4247-b6d2-7c015cbce6cb"",
                    ""path"": ""<Gamepad>/dpad/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""TargetModeAdd"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""000042f7-a2da-489a-af15-ec83ed111368"",
                    ""path"": ""<Keyboard>/t"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""TargetModeAdd"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d66734c8-df53-47a7-a5a0-f07da6adda87"",
                    ""path"": ""<Mouse>/middleButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""CycleTargets"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""08a21d43-6bae-486f-b319-381eb408864f"",
                    ""path"": ""<Gamepad>/dpad/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""CycleTargets"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""774d2f15-15cd-4d5e-9a05-6fec8726617d"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MissileButton"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a46efc90-0aab-47af-8538-84afa8e2ec91"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MissileButton"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""89f395f1-8c0d-485c-ab2a-02e43613c4e4"",
                    ""path"": ""<Keyboard>/1"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Primary Ability"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""1d9e94c8-e688-474a-9fb1-9c1e5ae455bd"",
                    ""path"": ""<Gamepad>/buttonWest"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Primary Ability"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3bb4b7a1-272f-4b7a-b50c-603692a7a8b4"",
                    ""path"": ""<Keyboard>/2"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Secondary Ability"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7709b204-7d8f-4bd8-b050-9058c440e19c"",
                    ""path"": ""<Gamepad>/buttonNorth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Secondary Ability"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""39436436-6d52-447f-94ec-b6e144152dd6"",
                    ""path"": ""<Keyboard>/3"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Ace Ability"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""Button With One Modifier"",
                    ""id"": ""2d035d27-a511-409d-8f59-1ccc7789a77a"",
                    ""path"": ""ButtonWithOneModifier"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Ace Ability"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""modifier"",
                    ""id"": ""40d218b7-faaa-44cb-bf74-e4827b01e05e"",
                    ""path"": ""<Gamepad>/buttonWest"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Ace Ability"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""button"",
                    ""id"": ""79d6947c-9108-46fb-8716-1a8f54ab183b"",
                    ""path"": ""<Gamepad>/buttonNorth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Ace Ability"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""0a6de934-effa-4992-a769-e612fcfa262b"",
                    ""path"": ""<Gamepad>/rightStick"",
                    ""interactions"": """",
                    ""processors"": ""ScaleVector2"",
                    ""groups"": """",
                    ""action"": ""CameraStick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b5a32301-c0ce-4ffc-a625-9a3ce1624e42"",
                    ""path"": ""<Gamepad>/dpad/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""TargetModeSub"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4546626b-9269-467b-9a3f-b89c40c6d99a"",
                    ""path"": ""<Gamepad>/leftStickPress"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""CameraTargetLock"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4a134cef-cda5-4dfa-8e27-8973d476960c"",
                    ""path"": ""<Keyboard>/x"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""CameraTargetLock"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""88087199-83ec-4994-bba7-017febbded0a"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""CameraMouse"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""UI"",
            ""id"": ""9cff6dd0-fcf5-4e83-826c-e50c6167b0c5"",
            ""actions"": [
                {
                    ""name"": ""Navigate"",
                    ""type"": ""PassThrough"",
                    ""id"": ""91be01a6-80e0-4b62-ad30-8bc732524083"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Submit"",
                    ""type"": ""Button"",
                    ""id"": ""c7725986-1015-47a3-9b77-968659df9115"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Cancel"",
                    ""type"": ""Button"",
                    ""id"": ""95339349-bf50-4a82-bfc7-8a091f4868b3"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Point"",
                    ""type"": ""PassThrough"",
                    ""id"": ""b3f68b04-4141-4492-897c-28a7c0b8699e"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Click"",
                    ""type"": ""PassThrough"",
                    ""id"": ""a55dc20c-f11a-4f1d-816b-e078f6a868cf"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""ScrollWheel"",
                    ""type"": ""PassThrough"",
                    ""id"": ""aa55a057-d81b-4234-8d15-fdca928ab5d9"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""MiddleClick"",
                    ""type"": ""PassThrough"",
                    ""id"": ""63fb6804-afb2-4dc9-bbc6-ed2a9157636f"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""RightClick"",
                    ""type"": ""PassThrough"",
                    ""id"": ""38242aee-0b41-46bb-a911-92c8c382d9d2"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""TrackedDevicePosition"",
                    ""type"": ""PassThrough"",
                    ""id"": ""6bb62b04-73fa-402c-b803-baa993f01c93"",
                    ""expectedControlType"": ""Vector3"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""TrackedDeviceOrientation"",
                    ""type"": ""PassThrough"",
                    ""id"": ""60d42dc7-3d3a-4c93-8d8f-797eb7629819"",
                    ""expectedControlType"": ""Quaternion"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""Gamepad"",
                    ""id"": ""c68e11cd-7bde-47b3-a75d-f2f90aabf1f3"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Navigate"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""fc535f3e-19df-4aed-aac5-b4be5b2d0850"",
                    ""path"": ""<Gamepad>/leftStick/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Gamepad"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""up"",
                    ""id"": ""8f46084b-42e7-4ae8-b429-905c0f105443"",
                    ""path"": ""<Gamepad>/rightStick/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Gamepad"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""784deee1-c6ad-4715-953d-f28d56c0c121"",
                    ""path"": ""<Gamepad>/leftStick/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Gamepad"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""7afeb4d9-ecf8-4463-be3b-bbf98735b97f"",
                    ""path"": ""<Gamepad>/rightStick/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Gamepad"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""3da27f99-6a7d-4dfe-83bf-b0e555a9dc46"",
                    ""path"": ""<Gamepad>/leftStick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Gamepad"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""04f61204-0685-4c3b-9536-57109f593c8b"",
                    ""path"": ""<Gamepad>/rightStick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Gamepad"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""7a6ae045-210b-43b0-982d-fa7def096a23"",
                    ""path"": ""<Gamepad>/leftStick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Gamepad"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""2464272c-4236-45a4-8eb0-13e3a0586145"",
                    ""path"": ""<Gamepad>/rightStick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Gamepad"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""5d09947d-2ccb-4ef8-8269-4065a7d7dfbe"",
                    ""path"": ""<Gamepad>/dpad"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Gamepad"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""Joystick"",
                    ""id"": ""baf5aefd-93d3-4825-92af-6043c3452a12"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Navigate"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""5c7e0502-2cf8-43d5-b623-bb1c4a57e75a"",
                    ""path"": ""<Joystick>/stick/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Joystick"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""7fe360d0-8734-452f-a898-29501eb444a8"",
                    ""path"": ""<Joystick>/stick/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Joystick"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""c1ffaab5-bf9c-415a-98c1-db90dcc7ff0c"",
                    ""path"": ""<Joystick>/stick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Joystick"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""ac9f3766-2079-4ac2-95c2-1329bb446271"",
                    ""path"": ""<Joystick>/stick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Joystick"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Keyboard"",
                    ""id"": ""6772514c-b9ba-449a-bee2-8a14590088d1"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Navigate"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""0fa47856-d8fc-414a-9175-8724a042d537"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""up"",
                    ""id"": ""ba85bae0-12d2-4206-b25b-5614ac26592f"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""52988a38-645d-4c29-8823-dc6f18ebd521"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""33eed677-530a-4371-a2fb-7a9804f7e38b"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""169cb38d-f324-4f3e-8aa1-8ac7a7424d91"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""e0708533-a608-4c02-b309-201de6b68dea"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""1c02738e-e7a0-41f8-860d-439940c0794a"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""1b2d00a0-c4c7-401f-b8a4-fa35c6add949"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""00e15659-cd57-4130-831c-0a77a40493b6"",
                    ""path"": ""*/{Submit}"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Submit"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4b847492-cd48-4e8e-b93c-d4bfd7eb0bbf"",
                    ""path"": ""*/{Cancel}"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Cancel"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""eba97386-6c97-4875-a740-abc7ae2ed1e0"",
                    ""path"": ""<Mouse>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Point"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6409e417-c447-4b2b-aa38-d0981881550f"",
                    ""path"": ""<Pen>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Point"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8c915ef6-956d-452a-bb3e-4cd3920825cb"",
                    ""path"": ""<Touchscreen>/touch*/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Touch"",
                    ""action"": ""Point"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""375ca656-5c41-4bfd-bad2-0e99a975e018"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Keyboard&Mouse"",
                    ""action"": ""Click"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3df1980a-d8b2-455a-8085-906e5a0b70ef"",
                    ""path"": ""<Pen>/tip"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Keyboard&Mouse"",
                    ""action"": ""Click"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""900619f6-c7ae-43f5-9e52-13300e7d0edd"",
                    ""path"": ""<Touchscreen>/touch*/press"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Touch"",
                    ""action"": ""Click"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2cbbd652-c117-491b-9412-161c2691adc5"",
                    ""path"": ""<XRController>/trigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""XR"",
                    ""action"": ""Click"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""cf8bb695-dde0-443c-805e-c7fbcc94e49b"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Click"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""eb18591a-ef37-411b-b40c-c74ab3e26258"",
                    ""path"": ""<Mouse>/scroll"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Keyboard&Mouse"",
                    ""action"": ""ScrollWheel"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6932a894-9f3f-4cd2-87aa-137adea611e0"",
                    ""path"": ""<Mouse>/middleButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Keyboard&Mouse"",
                    ""action"": ""MiddleClick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""69a4ef9b-45cf-462b-b3be-82ab7f44f28c"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Keyboard&Mouse"",
                    ""action"": ""RightClick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""09ab9e82-771f-40c3-84d5-a192891e075c"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""RightClick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""47454b17-8184-41cb-9e13-78c700293716"",
                    ""path"": ""<XRController>/devicePosition"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""XR"",
                    ""action"": ""TrackedDevicePosition"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e233bc8d-73b2-4949-9f53-967ba07b54f7"",
                    ""path"": ""<XRController>/deviceRotation"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""XR"",
                    ""action"": ""TrackedDeviceOrientation"",
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
        m_Flight_CameraChange = m_Flight.FindAction("CameraChange", throwIfNotFound: true);
        m_Flight_TargetModeAdd = m_Flight.FindAction("TargetModeAdd", throwIfNotFound: true);
        m_Flight_TargetModeSub = m_Flight.FindAction("TargetModeSub", throwIfNotFound: true);
        m_Flight_CycleTargets = m_Flight.FindAction("CycleTargets", throwIfNotFound: true);
        m_Flight_MissileButton = m_Flight.FindAction("MissileButton", throwIfNotFound: true);
        m_Flight_PrimaryAbility = m_Flight.FindAction("Primary Ability", throwIfNotFound: true);
        m_Flight_SecondaryAbility = m_Flight.FindAction("Secondary Ability", throwIfNotFound: true);
        m_Flight_AceAbility = m_Flight.FindAction("Ace Ability", throwIfNotFound: true);
        m_Flight_CameraStick = m_Flight.FindAction("CameraStick", throwIfNotFound: true);
        m_Flight_CameraTargetLock = m_Flight.FindAction("CameraTargetLock", throwIfNotFound: true);
        m_Flight_CameraMouse = m_Flight.FindAction("CameraMouse", throwIfNotFound: true);
        // UI
        m_UI = asset.FindActionMap("UI", throwIfNotFound: true);
        m_UI_Navigate = m_UI.FindAction("Navigate", throwIfNotFound: true);
        m_UI_Submit = m_UI.FindAction("Submit", throwIfNotFound: true);
        m_UI_Cancel = m_UI.FindAction("Cancel", throwIfNotFound: true);
        m_UI_Point = m_UI.FindAction("Point", throwIfNotFound: true);
        m_UI_Click = m_UI.FindAction("Click", throwIfNotFound: true);
        m_UI_ScrollWheel = m_UI.FindAction("ScrollWheel", throwIfNotFound: true);
        m_UI_MiddleClick = m_UI.FindAction("MiddleClick", throwIfNotFound: true);
        m_UI_RightClick = m_UI.FindAction("RightClick", throwIfNotFound: true);
        m_UI_TrackedDevicePosition = m_UI.FindAction("TrackedDevicePosition", throwIfNotFound: true);
        m_UI_TrackedDeviceOrientation = m_UI.FindAction("TrackedDeviceOrientation", throwIfNotFound: true);
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
    private readonly InputAction m_Flight_CameraChange;
    private readonly InputAction m_Flight_TargetModeAdd;
    private readonly InputAction m_Flight_TargetModeSub;
    private readonly InputAction m_Flight_CycleTargets;
    private readonly InputAction m_Flight_MissileButton;
    private readonly InputAction m_Flight_PrimaryAbility;
    private readonly InputAction m_Flight_SecondaryAbility;
    private readonly InputAction m_Flight_AceAbility;
    private readonly InputAction m_Flight_CameraStick;
    private readonly InputAction m_Flight_CameraTargetLock;
    private readonly InputAction m_Flight_CameraMouse;
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
        public InputAction @CameraChange => m_Wrapper.m_Flight_CameraChange;
        public InputAction @TargetModeAdd => m_Wrapper.m_Flight_TargetModeAdd;
        public InputAction @TargetModeSub => m_Wrapper.m_Flight_TargetModeSub;
        public InputAction @CycleTargets => m_Wrapper.m_Flight_CycleTargets;
        public InputAction @MissileButton => m_Wrapper.m_Flight_MissileButton;
        public InputAction @PrimaryAbility => m_Wrapper.m_Flight_PrimaryAbility;
        public InputAction @SecondaryAbility => m_Wrapper.m_Flight_SecondaryAbility;
        public InputAction @AceAbility => m_Wrapper.m_Flight_AceAbility;
        public InputAction @CameraStick => m_Wrapper.m_Flight_CameraStick;
        public InputAction @CameraTargetLock => m_Wrapper.m_Flight_CameraTargetLock;
        public InputAction @CameraMouse => m_Wrapper.m_Flight_CameraMouse;
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
                @CameraChange.started -= m_Wrapper.m_FlightActionsCallbackInterface.OnCameraChange;
                @CameraChange.performed -= m_Wrapper.m_FlightActionsCallbackInterface.OnCameraChange;
                @CameraChange.canceled -= m_Wrapper.m_FlightActionsCallbackInterface.OnCameraChange;
                @TargetModeAdd.started -= m_Wrapper.m_FlightActionsCallbackInterface.OnTargetModeAdd;
                @TargetModeAdd.performed -= m_Wrapper.m_FlightActionsCallbackInterface.OnTargetModeAdd;
                @TargetModeAdd.canceled -= m_Wrapper.m_FlightActionsCallbackInterface.OnTargetModeAdd;
                @TargetModeSub.started -= m_Wrapper.m_FlightActionsCallbackInterface.OnTargetModeSub;
                @TargetModeSub.performed -= m_Wrapper.m_FlightActionsCallbackInterface.OnTargetModeSub;
                @TargetModeSub.canceled -= m_Wrapper.m_FlightActionsCallbackInterface.OnTargetModeSub;
                @CycleTargets.started -= m_Wrapper.m_FlightActionsCallbackInterface.OnCycleTargets;
                @CycleTargets.performed -= m_Wrapper.m_FlightActionsCallbackInterface.OnCycleTargets;
                @CycleTargets.canceled -= m_Wrapper.m_FlightActionsCallbackInterface.OnCycleTargets;
                @MissileButton.started -= m_Wrapper.m_FlightActionsCallbackInterface.OnMissileButton;
                @MissileButton.performed -= m_Wrapper.m_FlightActionsCallbackInterface.OnMissileButton;
                @MissileButton.canceled -= m_Wrapper.m_FlightActionsCallbackInterface.OnMissileButton;
                @PrimaryAbility.started -= m_Wrapper.m_FlightActionsCallbackInterface.OnPrimaryAbility;
                @PrimaryAbility.performed -= m_Wrapper.m_FlightActionsCallbackInterface.OnPrimaryAbility;
                @PrimaryAbility.canceled -= m_Wrapper.m_FlightActionsCallbackInterface.OnPrimaryAbility;
                @SecondaryAbility.started -= m_Wrapper.m_FlightActionsCallbackInterface.OnSecondaryAbility;
                @SecondaryAbility.performed -= m_Wrapper.m_FlightActionsCallbackInterface.OnSecondaryAbility;
                @SecondaryAbility.canceled -= m_Wrapper.m_FlightActionsCallbackInterface.OnSecondaryAbility;
                @AceAbility.started -= m_Wrapper.m_FlightActionsCallbackInterface.OnAceAbility;
                @AceAbility.performed -= m_Wrapper.m_FlightActionsCallbackInterface.OnAceAbility;
                @AceAbility.canceled -= m_Wrapper.m_FlightActionsCallbackInterface.OnAceAbility;
                @CameraStick.started -= m_Wrapper.m_FlightActionsCallbackInterface.OnCameraStick;
                @CameraStick.performed -= m_Wrapper.m_FlightActionsCallbackInterface.OnCameraStick;
                @CameraStick.canceled -= m_Wrapper.m_FlightActionsCallbackInterface.OnCameraStick;
                @CameraTargetLock.started -= m_Wrapper.m_FlightActionsCallbackInterface.OnCameraTargetLock;
                @CameraTargetLock.performed -= m_Wrapper.m_FlightActionsCallbackInterface.OnCameraTargetLock;
                @CameraTargetLock.canceled -= m_Wrapper.m_FlightActionsCallbackInterface.OnCameraTargetLock;
                @CameraMouse.started -= m_Wrapper.m_FlightActionsCallbackInterface.OnCameraMouse;
                @CameraMouse.performed -= m_Wrapper.m_FlightActionsCallbackInterface.OnCameraMouse;
                @CameraMouse.canceled -= m_Wrapper.m_FlightActionsCallbackInterface.OnCameraMouse;
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
                @CameraChange.started += instance.OnCameraChange;
                @CameraChange.performed += instance.OnCameraChange;
                @CameraChange.canceled += instance.OnCameraChange;
                @TargetModeAdd.started += instance.OnTargetModeAdd;
                @TargetModeAdd.performed += instance.OnTargetModeAdd;
                @TargetModeAdd.canceled += instance.OnTargetModeAdd;
                @TargetModeSub.started += instance.OnTargetModeSub;
                @TargetModeSub.performed += instance.OnTargetModeSub;
                @TargetModeSub.canceled += instance.OnTargetModeSub;
                @CycleTargets.started += instance.OnCycleTargets;
                @CycleTargets.performed += instance.OnCycleTargets;
                @CycleTargets.canceled += instance.OnCycleTargets;
                @MissileButton.started += instance.OnMissileButton;
                @MissileButton.performed += instance.OnMissileButton;
                @MissileButton.canceled += instance.OnMissileButton;
                @PrimaryAbility.started += instance.OnPrimaryAbility;
                @PrimaryAbility.performed += instance.OnPrimaryAbility;
                @PrimaryAbility.canceled += instance.OnPrimaryAbility;
                @SecondaryAbility.started += instance.OnSecondaryAbility;
                @SecondaryAbility.performed += instance.OnSecondaryAbility;
                @SecondaryAbility.canceled += instance.OnSecondaryAbility;
                @AceAbility.started += instance.OnAceAbility;
                @AceAbility.performed += instance.OnAceAbility;
                @AceAbility.canceled += instance.OnAceAbility;
                @CameraStick.started += instance.OnCameraStick;
                @CameraStick.performed += instance.OnCameraStick;
                @CameraStick.canceled += instance.OnCameraStick;
                @CameraTargetLock.started += instance.OnCameraTargetLock;
                @CameraTargetLock.performed += instance.OnCameraTargetLock;
                @CameraTargetLock.canceled += instance.OnCameraTargetLock;
                @CameraMouse.started += instance.OnCameraMouse;
                @CameraMouse.performed += instance.OnCameraMouse;
                @CameraMouse.canceled += instance.OnCameraMouse;
            }
        }
    }
    public FlightActions @Flight => new FlightActions(this);

    // UI
    private readonly InputActionMap m_UI;
    private IUIActions m_UIActionsCallbackInterface;
    private readonly InputAction m_UI_Navigate;
    private readonly InputAction m_UI_Submit;
    private readonly InputAction m_UI_Cancel;
    private readonly InputAction m_UI_Point;
    private readonly InputAction m_UI_Click;
    private readonly InputAction m_UI_ScrollWheel;
    private readonly InputAction m_UI_MiddleClick;
    private readonly InputAction m_UI_RightClick;
    private readonly InputAction m_UI_TrackedDevicePosition;
    private readonly InputAction m_UI_TrackedDeviceOrientation;
    public struct UIActions
    {
        private @ControlInputActions m_Wrapper;
        public UIActions(@ControlInputActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @Navigate => m_Wrapper.m_UI_Navigate;
        public InputAction @Submit => m_Wrapper.m_UI_Submit;
        public InputAction @Cancel => m_Wrapper.m_UI_Cancel;
        public InputAction @Point => m_Wrapper.m_UI_Point;
        public InputAction @Click => m_Wrapper.m_UI_Click;
        public InputAction @ScrollWheel => m_Wrapper.m_UI_ScrollWheel;
        public InputAction @MiddleClick => m_Wrapper.m_UI_MiddleClick;
        public InputAction @RightClick => m_Wrapper.m_UI_RightClick;
        public InputAction @TrackedDevicePosition => m_Wrapper.m_UI_TrackedDevicePosition;
        public InputAction @TrackedDeviceOrientation => m_Wrapper.m_UI_TrackedDeviceOrientation;
        public InputActionMap Get() { return m_Wrapper.m_UI; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(UIActions set) { return set.Get(); }
        public void SetCallbacks(IUIActions instance)
        {
            if (m_Wrapper.m_UIActionsCallbackInterface != null)
            {
                @Navigate.started -= m_Wrapper.m_UIActionsCallbackInterface.OnNavigate;
                @Navigate.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnNavigate;
                @Navigate.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnNavigate;
                @Submit.started -= m_Wrapper.m_UIActionsCallbackInterface.OnSubmit;
                @Submit.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnSubmit;
                @Submit.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnSubmit;
                @Cancel.started -= m_Wrapper.m_UIActionsCallbackInterface.OnCancel;
                @Cancel.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnCancel;
                @Cancel.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnCancel;
                @Point.started -= m_Wrapper.m_UIActionsCallbackInterface.OnPoint;
                @Point.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnPoint;
                @Point.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnPoint;
                @Click.started -= m_Wrapper.m_UIActionsCallbackInterface.OnClick;
                @Click.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnClick;
                @Click.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnClick;
                @ScrollWheel.started -= m_Wrapper.m_UIActionsCallbackInterface.OnScrollWheel;
                @ScrollWheel.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnScrollWheel;
                @ScrollWheel.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnScrollWheel;
                @MiddleClick.started -= m_Wrapper.m_UIActionsCallbackInterface.OnMiddleClick;
                @MiddleClick.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnMiddleClick;
                @MiddleClick.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnMiddleClick;
                @RightClick.started -= m_Wrapper.m_UIActionsCallbackInterface.OnRightClick;
                @RightClick.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnRightClick;
                @RightClick.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnRightClick;
                @TrackedDevicePosition.started -= m_Wrapper.m_UIActionsCallbackInterface.OnTrackedDevicePosition;
                @TrackedDevicePosition.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnTrackedDevicePosition;
                @TrackedDevicePosition.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnTrackedDevicePosition;
                @TrackedDeviceOrientation.started -= m_Wrapper.m_UIActionsCallbackInterface.OnTrackedDeviceOrientation;
                @TrackedDeviceOrientation.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnTrackedDeviceOrientation;
                @TrackedDeviceOrientation.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnTrackedDeviceOrientation;
            }
            m_Wrapper.m_UIActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Navigate.started += instance.OnNavigate;
                @Navigate.performed += instance.OnNavigate;
                @Navigate.canceled += instance.OnNavigate;
                @Submit.started += instance.OnSubmit;
                @Submit.performed += instance.OnSubmit;
                @Submit.canceled += instance.OnSubmit;
                @Cancel.started += instance.OnCancel;
                @Cancel.performed += instance.OnCancel;
                @Cancel.canceled += instance.OnCancel;
                @Point.started += instance.OnPoint;
                @Point.performed += instance.OnPoint;
                @Point.canceled += instance.OnPoint;
                @Click.started += instance.OnClick;
                @Click.performed += instance.OnClick;
                @Click.canceled += instance.OnClick;
                @ScrollWheel.started += instance.OnScrollWheel;
                @ScrollWheel.performed += instance.OnScrollWheel;
                @ScrollWheel.canceled += instance.OnScrollWheel;
                @MiddleClick.started += instance.OnMiddleClick;
                @MiddleClick.performed += instance.OnMiddleClick;
                @MiddleClick.canceled += instance.OnMiddleClick;
                @RightClick.started += instance.OnRightClick;
                @RightClick.performed += instance.OnRightClick;
                @RightClick.canceled += instance.OnRightClick;
                @TrackedDevicePosition.started += instance.OnTrackedDevicePosition;
                @TrackedDevicePosition.performed += instance.OnTrackedDevicePosition;
                @TrackedDevicePosition.canceled += instance.OnTrackedDevicePosition;
                @TrackedDeviceOrientation.started += instance.OnTrackedDeviceOrientation;
                @TrackedDeviceOrientation.performed += instance.OnTrackedDeviceOrientation;
                @TrackedDeviceOrientation.canceled += instance.OnTrackedDeviceOrientation;
            }
        }
    }
    public UIActions @UI => new UIActions(this);
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
        void OnCameraChange(InputAction.CallbackContext context);
        void OnTargetModeAdd(InputAction.CallbackContext context);
        void OnTargetModeSub(InputAction.CallbackContext context);
        void OnCycleTargets(InputAction.CallbackContext context);
        void OnMissileButton(InputAction.CallbackContext context);
        void OnPrimaryAbility(InputAction.CallbackContext context);
        void OnSecondaryAbility(InputAction.CallbackContext context);
        void OnAceAbility(InputAction.CallbackContext context);
        void OnCameraStick(InputAction.CallbackContext context);
        void OnCameraTargetLock(InputAction.CallbackContext context);
        void OnCameraMouse(InputAction.CallbackContext context);
    }
    public interface IUIActions
    {
        void OnNavigate(InputAction.CallbackContext context);
        void OnSubmit(InputAction.CallbackContext context);
        void OnCancel(InputAction.CallbackContext context);
        void OnPoint(InputAction.CallbackContext context);
        void OnClick(InputAction.CallbackContext context);
        void OnScrollWheel(InputAction.CallbackContext context);
        void OnMiddleClick(InputAction.CallbackContext context);
        void OnRightClick(InputAction.CallbackContext context);
        void OnTrackedDevicePosition(InputAction.CallbackContext context);
        void OnTrackedDeviceOrientation(InputAction.CallbackContext context);
    }
}
