Our namespace (13:00 19.03.2026):

Player
├── Movement
├── CameraBobbing
├── PlayerInteraction
└── CameraController

Infrastructure
├── GameData
│   ├── SettingsData
│   ├── HouseInstaller
│   ├── ProjectInstaller
│   └── MenuInstaller
│
├── Controllers
│   ├── SceneController
│   ├── SettingsManager
│   ├── AudioManager
│   └── GameEvents
│
├── SceneReference
│
└── SO
    ├── ImageStorageSO
    └── AudioStorageSO

UI
├── Buttons
│   ├── ButtonParent
│   ├── ActiveButton
│   ├── ThemeToggleButton
│   └── MenuSwitcher
│
├── Sliders
│   └── UISliderHandler
│
└── Panels
    ├── PauseMenu
    └── ImageDisplayManager

Interaction
├── BaseInteractable
└── Objects
    ├── GarbageItem
    └── ImageItem