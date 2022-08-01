using FrooxEngine;
using FrooxEngine.UIX;
using HarmonyLib;
using NeosModLoader;
using System.Collections.Generic;

namespace DefaultTooltips
{
    public class DefaultTooltips : NeosMod
    {
        public override string Name => "DefaultTooltips";
        public override string Author => "Psychpsyo";
        public override string Version => "1.0.0";
        public override string Link => "https://github.com/Psychpsyo/DefaultTooltips";

        private static Dictionary<string, string> createNewLabelDict = new Dictionary<string, string>()
        {
            {"", "Return to the previous menu."},
            {"/0", "Creates a new, empty slot."},
            {"/1", "Creates a new particle system."},
            {"/3DModel", "3D Models such as cubes, spheres and cylinders."},
            {"/3DModel/0", "Creates a new, resizeable box."},
            {"/3DModel/1", "Creates a new capsule. (pill)"},
            {"/3DModel/2", "Creates a new 3D cone."},
            {"/3DModel/3", "Creates a new cylinder."},
            {"/3DModel/4", "Creates a new, one-sided grid mesh."},
            {"/3DModel/5", "Creates a new, one-sided quad mesh."},
            {"/3DModel/6", "Creates a new, resizeable sphere."},
            {"/3DModel/7", "Creates a new torus. (donut)"},
            {"/3DModel/8", "Creates a new, one-sided triangle mesh."},
            {"/Collider", "Various types and shapes of colliders."},
            {"/Collider/0", "Creates a new box collider."},
            {"/Collider/1", "Creates a new capsule collider."},
            {"/Collider/2", "Creates a new cone collider."},
            {"/Collider/3", "Creates a new cylinder collider."},
            {"/Collider/4", "Creates a new mesh collider."},
            {"/Collider/5", "Creates a new sphere collider."},
            {"/Editor", "Various editing and optimization tools."},
            {"/Editor/0", "Creates a new asset optmization wizard."}, //TODO: Write what this is used for on a new line here
            {"/Editor/1", "Creates a new cubemap creator.\nThis can be used to turn 6 separate images into a cubemap."},
            {"/Editor/2", "Creates a new world light sources wizard."}, //TODO: Write what this is used for on a new line here
            {"/Editor/3", "Creates a new LogiX transfer wizard."}, //TODO: Write what this is used for on a new line here
            {"/Editor/4", "Creates a new reflection probe wizard."}, //TODO: Write what this is used for on a new line here
            {"/Editor/5", "Creates a new world text renderer wizard."}, //TODO: Write what this is used for on a new line here
            {"/Editor/6", "Creates a new user inspector.\nThis can be used inspect users and access detailed information about them."},
            {"/Light", "Directional, point and spot lights."},
            {"/Light/0", "Creates a new, directional light source."},
            {"/Light/1", "Creates a new light source that shines in every direction."},
            {"/Light/2", "Creates a new spot light."},
            {"/Materials", "Different materials (shaders) to apply to 3D objects."},
            {"/Object", "Various pre-setup objects such as UI Elements, Mirrors, Cameras..."},
            {"/Text", "Floating Text."},
            {"/Text/0", "Creates a new piece of floating text with no outline."},
            {"/Text/1", "Creates a new piece of floating text with outlines on the letters."}
        };

        private static Dictionary<string, string> inspectorLabelDict = new Dictionary<string, string>()
        {
            {"OnObjectRootPressed", "Go up to the next object root in the hierarchy."},
            {"OnRootUpPressed", "Go up one slot in the hierarchy."},
            {"OnDestroyPressed", "Destroys the selected slot and related assets."},
            {"OnDestroyPreservingAssetsPressed", "Destroys the selected slot while preserving related assets."},
            {"OnInsertParentPressed", "Inserts a new parent above the selected slot."},
            {"OnAddChildPressed", "Appends a new child to the selected slot."},
            {"OnDuplicatePressed", "Duplicates the selected slot."},
            {"OnSetRootPressed", "Focuses the hierarchy onto the selected slot."},
            {"OnAttachComponentPressed", "Attach a component to the selected slot."}
        };

        private static Dictionary<string, string> inventoryLabelDict = new Dictionary<string, string>()
        {
            {"ShowInventoryOwners", "Go to group inventories."},
            {"GenerateLink", "Spawn selected folder.\n(This will make it public.)"},
            {"MakePrivate", "Make selected folder private again."},
            {"DeleteItem", "Delete selected item.\n(double click)"},
            {"AddCurrentAvatar", "Save your equipped avatar to this folder."},
            {"AddNew", "Create a new folder or save your currently held item."},
            {"OnOpenWorld", "Open as private world."},
            {"OnEquipAvatar", "Equip selected avatar."},
            {"OnSetDefaultHome", "Set as your default home world."},
            {"OnSetDefaultAvatar", "Set as your default avatar."},
            {"OnSetDefaultKeyboard", "Set as your default keyboard."},
            {"OnSetDefaultCamera", "Set as your default camera."},
            {"OnSpawnFacet", "Spawn selected facet in local space.\n(Facets don't work properly in desktop mode)"}
        };

        private static Dictionary<VoiceMode, string> voiceFacetLabelDict = new Dictionary<VoiceMode, string>()
        {
            {VoiceMode.Whisper, "Open whisper bubble."},
            {VoiceMode.Normal, "Set your voice to 'normal'."},
            {VoiceMode.Shout, "Set your voice to 'shout'."},
            {VoiceMode.Broadcast, "Set your voice to 'broadcast'."},
        };

        private static Dictionary<CloudX.Shared.OnlineStatus, string> onlineStatusFacetLabelDict = new Dictionary<CloudX.Shared.OnlineStatus, string>()
        {
            {CloudX.Shared.OnlineStatus.Online, "Set your status to 'Online'."},
            {CloudX.Shared.OnlineStatus.Away, "Set your status to 'Away'."},
            {CloudX.Shared.OnlineStatus.Busy, "Set your status to 'Busy'."},
            {CloudX.Shared.OnlineStatus.Invisible, "Set your status to 'Invisible'. (You will appear as offline to others.)"},
        };

        private static Dictionary<string, string> fileBrowserLabelDict = new Dictionary<string, string>()
        {
            {"RunImport", "Import selected file or folder."},
            {"RunRawImport", "Import selected file as a raw file."},
            {"CreateNew", "Create a new Folder or export held item."},
            {"Reload", "Refresh the current view."}
        };

        private static Dictionary<string, string> imageImportLabelDict = new Dictionary<string, string>()
        {
            {"Preset_Image", "Import as regular image."},
            {"Preset_Screenshot", "Import as Neos screenshot with metadata."},
            {"Preset_360", "Import as a 360° photo."},
            {"Preset_StereoImage", "Sterep (3D) image import options..."},
            {"Preset_Stereo360", "360° Stereo (3D) image import options..."},
            {"Preset_180", "Import as a 180° photo."},
            {"Preset_Stereo180", "180° Stereo (3D) image import options..."},
            {"Preset_LUT", "Import as a LUT.\n(A LUT is a color lookup table)"},
            {"AsRawFile", "Import as a raw file."},
            {"Return", "Return to the previous menu."},
            {"Preset_HorizontalLR", "Import as side-by-side left-right 3D picture."},
            {"Preset_HorizontalRL", "Import as side-by-side right-left 3D picture."},
            {"Preset_VerticalLR", "Import as top-to-bottom left-right 3D picture."},
            {"Preset_VerticalRL", "Import as top-to-bottom right-left 3D picture."}
        };

        private static Dictionary<string, string> videoImportLabelDict = new Dictionary<string, string>()
        {
            {"Preset_Video", "Import as regular video."},
            {"Preset_360", "Import as a 360° video."},
            {"Preset_StereoVideo", "Sterep (3D) video import options..."},
            {"Preset_Stereo360", "360° Stereo (3D) video import options..."},
            {"Preset_Depth", "Depth video import options..."},
            {"Preset_180", "Import as a 180° video."},
            {"Preset_Stereo180", "180° Stereo (3D) video import options..."},
            {"AsRawFile", "Import as a raw file."},
            {"Return", "Return to the previous menu."},
            {"Preset_HorizontalLR", "Import as side-by-side left-right 3D video."},
            {"Preset_HorizontalRL", "Import as side-by-side right-left 3D video."},
            {"Preset_VerticalLR", "Import as top-to-bottom left-right 3D video."},
            {"Preset_VerticalRL", "Import as top-to-bottom right-left 3D video."},
            // TODO: What are these four?
            {"Preset_DepthDefault", ""},
            {"Preset_DepthPFCapture", ""},
            {"Preset_DepthPFCaptureHorizontal", ""},
            {"Preset_DepthHolofix", ""}
        };

        public override void OnEngineInit()
        {
            Tooltippery.Tooltippery.labelProviders.Add(inspectorLabels);
            Tooltippery.Tooltippery.labelProviders.Add(createNewLabels);
            Tooltippery.Tooltippery.labelProviders.Add(inventoryLabels);
            Tooltippery.Tooltippery.labelProviders.Add(voiceFacetLabels);
            Tooltippery.Tooltippery.labelProviders.Add(fileBrowserLabels);
            Tooltippery.Tooltippery.labelProviders.Add(imageImportLabels);
            Tooltippery.Tooltippery.labelProviders.Add(videoImportLabels);
            Tooltippery.Tooltippery.labelProviders.Add(onlineStatusFacetLabels);
        }

        private static string createNewLabels(IButton button, ButtonEventData eventData)
        {
            if (button.Slot.GetComponentInParents<DevCreateNewForm>() == null) return null;
            string target = button.Slot.GetComponent<ButtonRelay<string>>()?.Argument.Value;
            if (target == null) return null;
            if (createNewLabelDict.TryGetValue(target, out target)) return target;
            return null;
        }

        private static string inspectorLabels(IButton button, ButtonEventData eventData)
        {
            // only care for buttons on the UIX Canvas for now:
            if (button.GetType() != typeof(Button)) return null;

            // get the inspector and target method of the button
            SceneInspector inspector = button.Slot.GetComponentInParents<SceneInspector>();
            if (inspector == null) return null;

            WorldDelegate? buttonTarget = null;
            if (((Button)button).Pressed?.Target != null) buttonTarget = ((Button)button).Pressed.Value;
            if (button.Slot.GetComponent<ButtonRelay>()?.ButtonPressed?.Target != null) buttonTarget = button.Slot.GetComponent<ButtonRelay>()?.ButtonPressed?.Value;
            if (!buttonTarget.HasValue) return null;

            string methodName = buttonTarget.Value.method;

            // is this button is calling a function of the inspector itself?
            if (buttonTarget.Value.target == inspector.ReferenceID)
            {
                string retVal;
                if (inspectorLabelDict.TryGetValue(methodName, out retVal)) return retVal;
            }

            return null;
        }

        private static string inventoryLabels(IButton button, ButtonEventData eventData)
        {
            InventoryBrowser inventory = button.Slot.GetComponentInParents<InventoryBrowser>();
            if (inventory == null) return null;

            WorldDelegate? buttonTarget = null;
            if (((Button)button).Pressed?.Target != null)
            {
                buttonTarget = ((Button)button).Pressed.Value;
            }
            else if (button.Slot.GetComponent<ButtonRelay>()?.ButtonPressed?.Target != null)
            {
                buttonTarget = button.Slot.GetComponent<ButtonRelay>().ButtonPressed.Value;
            }
            // back buttons
            else if (button.Slot.GetComponent<ButtonRelay<int>>()?.ButtonPressed?.Target != null)
            {
                ButtonRelay<int> relay = button.Slot.GetComponent<ButtonRelay<int>>();
                buttonTarget = relay.ButtonPressed?.Value;
                if (!buttonTarget.HasValue) return null;
                if (buttonTarget.Value.method == "OnGoUp")
                {
                    string[] path = inventory.CurrentPath == "" ? new[] { "Inventory" } : ("Inventory\\" + inventory.CurrentPath).Split('\\');
                    return "Go back to '" + path[path.Length - 1 - relay.Argument] + "'.";
                }
                else
                {
                    return null;
                }
            }
            if (!buttonTarget.HasValue) return null;

            string methodName = buttonTarget.Value.method;

            // is this button is calling a function of the inspector itself?
            if (buttonTarget.Value.target == inventory.ReferenceID)
            {
                string retVal;
                if (inventoryLabelDict.TryGetValue(methodName, out retVal)) return retVal;
            }

            return null;
        }

        private static string voiceFacetLabels(IButton button, ButtonEventData eventData)
        {
            if (button.Slot.GetComponentInParents<VoiceFacetPreset>() == null) return null;
            VoiceMode? targetVoiceMode = button.Slot.GetComponent<ButtonValueSet<VoiceMode>>()?.SetValue.Value;
            if (!targetVoiceMode.HasValue)
            {
                IField<bool> targetToggle = button.Slot.GetComponent<ButtonToggle>()?.TargetValue.Target;
                if (targetToggle?.Name == "GlobalMute")
                {
                    return targetToggle.Value ? "Unmute your mic." : "Mute your mic.";
                }
            }

            string retVal = null;
            voiceFacetLabelDict.TryGetValue(targetVoiceMode.Value, out retVal);
            return retVal;
        }

        private static string onlineStatusFacetLabels(IButton button, ButtonEventData eventData)
        {
            if (button.Slot.GetComponentInParents<OnlineStatusFacetPreset>() == null) return null;
            CloudX.Shared.OnlineStatus? targetStatus = button.Slot.GetComponent<ButtonValueSet<CloudX.Shared.OnlineStatus>>()?.SetValue.Value;
            if (!targetStatus.HasValue) return null;

            string retVal = null;
            onlineStatusFacetLabelDict.TryGetValue(targetStatus.Value, out retVal);
            return retVal;
        }

        private static string fileBrowserLabels(IButton button, ButtonEventData eventData)
        {
            FileBrowser fileBrowser = button.Slot.GetComponentInParents<FileBrowser>();
            if (fileBrowser == null) return null;

            WorldDelegate? buttonTarget = null;
            if (((Button)button).Pressed?.Target != null)
            {
                buttonTarget = ((Button)button).Pressed.Value;
            }
            else if (button.Slot.GetComponent<ButtonRelay>()?.ButtonPressed?.Target != null)
            {
                buttonTarget = button.Slot.GetComponent<ButtonRelay>().ButtonPressed.Value;
            }
            // back buttons
            else if (button.Slot.GetComponent<ButtonRelay<int>>()?.ButtonPressed?.Target != null)
            {
                ButtonRelay<int> relay = button.Slot.GetComponent<ButtonRelay<int>>();
                buttonTarget = relay.ButtonPressed?.Value;
                if (!buttonTarget.HasValue) return null;
                if (buttonTarget.Value.method == "OnGoUp")
                {
                    string[] path = fileBrowser.CurrentPath == null ? new[] { "Computer" } : ("Computer\\" + fileBrowser.CurrentPath).Split('\\');
                    return "Go back to '" + path[path.Length - 1 - relay.Argument] + "'.";
                }
                else
                {
                    return null;
                }
            }
            if (!buttonTarget.HasValue) return null;

            string methodName = buttonTarget.Value.method;

            // is this button is calling a function of the inspector itself?
            if (buttonTarget.Value.target == fileBrowser.ReferenceID)
            {
                string retVal;
                if (fileBrowserLabelDict.TryGetValue(methodName, out retVal)) return retVal;
            }

            return null;
        }
        private static string imageImportLabels(IButton button, ButtonEventData eventData)
        {
            // only care for buttons on the UIX Canvas for now:
            if (button.GetType() != typeof(Button)) return null;

            if (button.Slot.GetComponentInParents<ImageImportDialog>() == null) return null;
            if (((Button)button).Pressed?.Target == null) return null;
            string target = ((Button)button).Pressed.Value.method;
            if (imageImportLabelDict.TryGetValue(target, out target)) return target;
            return null;
        }
        private static string videoImportLabels(IButton button, ButtonEventData eventData)
        {
            // only care for buttons on the UIX Canvas for now:
            if (button.GetType() != typeof(Button)) return null;

            if (button.Slot.GetComponentInParents<VideoImportDialog>() == null) return null;
            string target = null;
            if (((Button)button).Pressed?.Target != null) target = ((Button)button).Pressed.Value.method;
            if (button.Slot.GetComponent<ButtonRelay>() != null) target = button.Slot.GetComponent<ButtonRelay>().ButtonPressed?.Value.method;
            if (target == null) return null;
            if (videoImportLabelDict.TryGetValue(target, out target)) return target;
            return null;
        }
    }
}