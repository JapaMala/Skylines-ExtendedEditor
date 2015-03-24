using ColossalFramework;
using ColossalFramework.UI;
using ICities;

namespace ExtendedEditor
{
    public class ExtendedEditorLoadingExtension : LoadingExtensionBase
    {
        public override void OnLevelLoaded(LoadMode mode)
        {
            if (!Singleton<ToolManager>.exists)
                return;
            if (Singleton<ToolManager>.instance.m_properties.m_mode != ItemClass.Availability.AssetEditor)
                return;

            UIView view = UIView.GetAView();

            ExtendedEditorPanel editorPanel = (ExtendedEditorPanel)view.AddUIComponent(typeof(ExtendedEditorPanel));
        }
    }
}
