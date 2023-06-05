using System;
using System.Collections.Generic;
using UnityEngine; 

using UI.Enum;
using UI.Core;
using UI.InventoryUI.InventoryUI;
using UI.InventoryUI.QuickInventoryUI;
using InputReader;
using Items;
using Items.Data;

namespace UI
{
    public class UIContext : IDisposable
    {
        private const string LoadPath = "Prefabs/UI/";

        private readonly Dictionary<ScreenType, IScreenController> _controllers;
        private readonly Transform _uiContainer;
        private readonly List<IWindowsInputSource> _inputSources;
        private readonly Data _data;

        private IScreenController _currentController;


        public UIContext(List<IWindowsInputSource> inputSources, Data data)
        {
            _controllers = new Dictionary<ScreenType, IScreenController>();
            _inputSources = inputSources;
            _data = data;
            foreach (IWindowsInputSource inputSource in _inputSources)
            {
                inputSource.InventoryRequested += HandleInventory;
            }

            GameObject container = new GameObject()
            {
                name = nameof(UIContext)
            };
            _uiContainer = container.transform;

            OpenQuickInventory();
        }
    
        public void CloseCurrentScreen()
        {
            _currentController?.Complete();
            _currentController = null;
        }

        public void Dispose()
        {
            foreach (IWindowsInputSource inputSource in _inputSources)
            {
                inputSource.InventoryRequested -= HandleInventory;
            }

            foreach (IScreenController screenControllers in _controllers.Values)
            {
                screenControllers.OpenScreenRequested -= OpenScreen;
                screenControllers.CloseScreenRequested -= CloseCurrentScreen;
            }
        }

        private void HandleInventory()
        {
            _controllers.TryGetValue(ScreenType.Inventory, out IScreenController screenController);
            if (_currentController != null && screenController == _currentController)
            {
                CloseCurrentScreen();
                return;
            }
            
            OpenScreen(ScreenType.Inventory);
        }

        private void OpenQuickInventory()
        {
            if (!_controllers.TryGetValue(ScreenType.QuickInventory, out IScreenController screenControllers))
            {
                screenControllers = GetController(ScreenType.QuickInventory);
                _controllers.Add(ScreenType.QuickInventory, screenControllers);
                screenControllers.Initialize();
            }
        }

        private void OpenScreen(ScreenType screenType)
        {
            _currentController?.Complete();

            if(!_controllers.TryGetValue(screenType, out IScreenController screenControllers))
            {
                screenControllers = GetController(screenType);
                screenControllers.CloseScreenRequested += CloseCurrentScreen;
                screenControllers.OpenScreenRequested += OpenScreen;
                _controllers.Add(screenType, screenControllers);
            }

            _currentController = screenControllers;
            _currentController.Initialize();
        }

        private IScreenController GetController(ScreenType screenType)
        {
            
            switch (screenType)
            {
                case ScreenType.Inventory:
                    return new InventoryScreenPresenter((InventoryScreenView)GetView<ScreenView>(screenType), _data.Inventory, _data.RarityDescriptors);
                case ScreenType.QuickInventory:
                    return new QuickInventoryScreenPresenter((QuickInventoryScreenView)GetView<ScreenView>(screenType), _data.Inventory, _data.RarityDescriptors);
                default:
                    throw new NullReferenceException();
            };
        }

        private TView GetView<TView>(ScreenType screenType) where TView : ScreenView
        {
            TView prefab = Resources.Load<TView>($"{LoadPath}{screenType.ToString()}");
            return UnityEngine.Object.Instantiate(prefab, _uiContainer);
        }

        public struct Data
        {
            public Inventory Inventory { get; }
            public List<RarityDescriptor> RarityDescriptors { get; }

            public Data(Inventory inventory, List<RarityDescriptor> rarityDescriptors)
            {
                Inventory = inventory;
                RarityDescriptors = rarityDescriptors;
            }
        }
    }
}
