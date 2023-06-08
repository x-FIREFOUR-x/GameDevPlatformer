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
using StatsSystem;

namespace UI
{
    public class UIContext : IDisposable
    {
        private const string LoadPath = "Prefabs/UI/";

        private readonly Dictionary<ScreenType, IScreenController> _controllers;
        private readonly Transform _uiContainer;
        private List<IWindowsInputSource> _inputSources;
        private Data _data;

        private IScreenController _currentController;

        private IScreenController _quickInventoryController;

        public UIContext()
        {
            _controllers = new Dictionary<ScreenType, IScreenController>();

            GameObject container = new GameObject()
            {
                name = nameof(UIContext)
            };
            _uiContainer = container.transform;

            MonoBehaviour.DontDestroyOnLoad(container);
        }

        public void SetData(List<IWindowsInputSource> inputSources, Data data)
        {
            _inputSources = inputSources;
            _data = data;

            foreach (IWindowsInputSource inputSource in _inputSources)
            {
                inputSource.InventoryRequested += HandleInventory;
            }

            if (_quickInventoryController == null)
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

                _quickInventoryController = screenControllers;
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
                    return new InventoryScreenPresenter((InventoryScreenView)GetView<ScreenView>(screenType), _data.Inventory, _data.RarityDescriptors, _data.StatsController);
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
            
            public StatsController StatsController { get; }

            public Data(Inventory inventory, List<RarityDescriptor> rarityDescriptors, StatsController statsController)
            {
                Inventory = inventory;
                RarityDescriptors = rarityDescriptors;
                StatsController = statsController;
            }
        }
    }
}
