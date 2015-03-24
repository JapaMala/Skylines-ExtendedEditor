using ColossalFramework.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace ExtendedEditor
{
    class ExtendedEditorPanel : UIPanel
    {
        private UILabel buildingType;

        private UIDropDown buildingAISelection;

        List<string> buildingAINames;
        List<Type> buildingAITypes;

        List<string> itemClasses;

        public override void Start()
        {
            backgroundSprite = "GenericPanel";
            width = 400;
            height = 800;
            anchor = UIAnchorStyle.CenterHorizontal | UIAnchorStyle.CenterVertical;
            
            ////allow resizing. Don't know why it's not showing up.
            //UIResizeHandle resizeHandle = AddUIComponent<UIResizeHandle>();
            //resizeHandle.backgroundSprite = "buttonresize";

            //Container for the titlebar stuff.
            UISlicedSprite caption = AddUIComponent<UISlicedSprite>();
            caption.anchor = UIAnchorStyle.Top | UIAnchorStyle.Left | UIAnchorStyle.Right;
            caption.fillDirection = UIFillDirection.Horizontal;
            caption.fillAmount = 1;
            caption.relativePosition = new Vector3(0, 0, 0);
            caption.width = 400;
            caption.height = 40;

            //Window title
            UILabel label = caption.AddUIComponent<UILabel>();
            label.autoSize = true;
            label.textAlignment = UIHorizontalAlignment.Center;
            label.verticalAlignment = UIVerticalAlignment.Top;
            label.textScale = 1.3f;
            label.text = "Extended Properties";
            label.anchor = UIAnchorStyle.CenterVertical | UIAnchorStyle.CenterHorizontal;
            label.relativePosition = new Vector3(0, 0, 0);

            //Window dragging.
            UIDragHandle dragHandle = caption.AddUIComponent<UIDragHandle>();
            dragHandle.width = 400;
            dragHandle.height = 40;
            dragHandle.relativePosition = new Vector3(0, 0, 0);
            dragHandle.target = this;

            // Allow automated layout
            this.autoLayoutDirection = LayoutDirection.Vertical;
            this.autoLayoutStart = LayoutStart.TopLeft;
            this.autoLayoutPadding = new RectOffset(10, 10, 0, 10);
            this.autoLayout = true;

            //setup the building label
            buildingType = AddUIComponent<UILabel>();
            buildingAISelection = AddUIComponent<UIDropDown>();
            buildingAISelection.listBackground = "InfoDisplay";
            buildingAISelection.itemHover = "ListItemHover";
            buildingAISelection.itemHighlight = "ListItemHilight";
            buildingAISelection.normalBgSprite = "InfoDisplay";
            buildingAISelection.width = 300;
            buildingAISelection.height = 28;
            buildingAISelection.listWidth = 300;

            buildingAISelection.eventSelectedIndexChanged += buildingAISelection_eventSelectedIndexChanged;

            UIButton buildingAISelectionButton = buildingAISelection.AddUIComponent<UIButton>();
            buildingAISelectionButton.normalFgSprite = "IconUpArrow";
            buildingAISelectionButton.width = 47;
            buildingAISelectionButton.height = 26;
            buildingAISelectionButton.verticalAlignment = UIVerticalAlignment.Middle;
            buildingAISelectionButton.horizontalAlignment = UIHorizontalAlignment.Right;
            buildingAISelectionButton.relativePosition = new Vector3(1, 2);
            buildingAISelection.triggerButton = buildingAISelectionButton;

            RefreshBuildingAIs();
            RefreshItemClasses();
        }

        void buildingAISelection_eventSelectedIndexChanged(UIComponent component, int value)
        {
            if (ToolsModifierControl.toolController.m_editPrefabInfo is BuildingInfo)
            {
                BuildingInfo buildingInfo = (BuildingInfo)ToolsModifierControl.toolController.m_editPrefabInfo;
                if (buildingInfo.m_buildingAI.GetType() == buildingAITypes[value])
                    return; // nothing is changed.
                BuildingAI backup = buildingInfo.m_buildingAI;
                GameObject currentGameObject = buildingInfo.m_buildingAI.gameObject;
                currentGameObject.AddComponent(buildingAITypes[value]);
                buildingInfo.m_buildingAI = (BuildingAI)currentGameObject.GetComponent(buildingAITypes[value]);
                buildingInfo.m_buildingAI.m_info = backup.m_info;
                buildingInfo.m_buildingAI.m_info.m_buildingAI = buildingInfo.m_buildingAI;
                if (buildingInfo.m_buildingAI == null)
                    buildingInfo.m_buildingAI = backup;
                else
                    Destroy(backup);
            }
        }

        public override void Update()
        {
            PrefabInfo currentPrefab = ToolsModifierControl.toolController.m_editPrefabInfo;
            if (currentPrefab is BuildingInfo)
            {
                BuildingInfo buildingInfo = (BuildingInfo)currentPrefab;
                buildingType.text = buildingInfo.m_buildingAI.GetType().Name;

                buildingAISelection.selectedIndex = buildingAITypes.FindIndex(item => item == buildingInfo.m_buildingAI.GetType());
            }
        }

        void RefreshBuildingAIs()
        {
            if (buildingAINames == null)
                buildingAINames = new List<string>();
            if (buildingAITypes == null)
                buildingAITypes = new List<Type>();
            buildingAINames.Clear();
            buildingAITypes.Clear();

            AppDomain currentDomain = AppDomain.CurrentDomain;
            //Load the assembly from the application directory using a simple name. 

            //Create an assembly called CustomLibrary to run this sample.
            currentDomain.Load("ExtendedEditor");

            //Make an array for the list of assemblies.
            Assembly[] assems = currentDomain.GetAssemblies();

            foreach (var assembly in assems)
            {
                foreach (Type AIType in assembly.GetTypes().Where(t => t.IsSubclassOf(typeof(BuildingAI))))
                {
                    buildingAITypes.Add(AIType);
                    buildingAINames.Add(AIType.Name);
                }
            }

            buildingAISelection.items = buildingAINames.ToArray();
        }

        void RefreshItemClasses()
        {
            if (itemClasses == null)
                itemClasses = new List<string>();
            itemClasses.Clear();
            ItemClassCollection classCollection = FindObjectOfType<ItemClassCollection>();
            if (classCollection == null)
                return;
            foreach (var item in classCollection.m_classes)
            {
                itemClasses.Add(item.name);
                ModDebug.Log(item.name);
            }
        }
    }
}
