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
            {"/Editor/0", "Creates a new asset optmization wizard.\nTODO: Write what this is used for"},
            {"/Editor/1", "Creates a new cubemap creator.\nThis can be used to turn 6 separate images into a cubemap."},
            {"/Editor/2", "Creates a new world light sources wizard.\nTODO: Write what this is used for"},
            {"/Editor/3", "Creates a new LogiX transfer wizard.\nTODO: Write what this is used for"},
            {"/Editor/4", "Creates a new reflection probe wizard.\nTODO: Write what this is used for"},
            {"/Editor/5", "Creates a new world text renderer wizard.\nTODO: Write what this is used for"},
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
            {"OnInsertParentPressed", "Inserts a new parent slot above the selected slot."},
            {"OnAddChildPressed", "Appends a new child slot to the selected slot."},
            {"OnDuplicatePressed", "Duplicates the selected slot."},
            {"OnSetRootPressed", "Focuses the hierarchy onto the selected slot."},
            {"OnAttachComponentPressed", "Attach a component to the selected slot."}
        };

        public override void OnEngineInit()
        {
            Tooltippery.Tooltippery.labelProviders.Add(createNewLabels);
            Tooltippery.Tooltippery.labelProviders.Add(inspectorLabels);

            Harmony harmony = new Harmony("Psychpsyo.Tooltippery");
            harmony.PatchAll();
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
            if (buttonTarget == null) return null;

            string methodName = buttonTarget.Value.method;

            // is this button is calling a function of the inspector itself?
            if (buttonTarget.Value.target == inspector.ReferenceID)
            {
                string retVal;
                if (inspectorLabelDict.TryGetValue(methodName, out retVal)) return retVal;
            }

            return null;
        }
    }
}