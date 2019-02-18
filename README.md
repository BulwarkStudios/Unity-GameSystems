# !!!IMPORTANT!!!

Developped with Unity 2019.1.0a14

- Require Odin Inspector (version used: 2.0.15): https://assetstore.unity.com/packages/tools/utilities/odin-inspector-and-serializer-89041

- Require TextMesh Pro: Package Manager TextMesh Pro 1.3.0

# How to test the project

- Install Odin Inspector
- Open the scene "Assets/Scenes/MainMenu.scene"
- Click on the play button

# Unity-GameSystems

Unity-GameSystems is a set of tools made after the release of Warhammer 40.000: Mechanicus (https://store.steampowered.com/app/673880/Warhammer_40000_Mechanicus/) to fix and clean design patterns used in the game.

# Contexts

In Mechanicus, when the game started to grow, it was hard to debug issues related to the state system. We have to log every things and we don't have a visual editor to see what's happening in real time. So I thought about a pattern with a more "data driven" approach to visualize the current state of the game. I choose to use what we can call a Singleton ScriptableObject pattern. 

A game can be describe and split in many states like "Introduction", "MainMenu", "Settings", "Credits", "NewGame", "Overworld", "BuildingManagement", "UnitManagement", "Inventory", etc...
In that pattern a game context is one of this state.

The GameContextSystem class will manage, update or change the current contexts.
Using a ScriptableObject and the magic of Odin Inspector, you can view and debug contexts.
The file is located in: Resources/GameSystems/Contexts/GameContextSystem.asset

A context has to extend GameContext<T>, the system will find that class automatically and will create an associated ScriptableObject located in: Resources/GameSystems/Contexts/List/YourContextName.asset
To have an access to a context just call: "YourContextName.Instance".

The system can have 4 contexts at the same time.
However the system can manage "layers". When a layer is added, the old contexts are saved and only the contexts in the new layer are active.
So when a layer is removed, the old contexts become active again.

Some API reference:

#### Load contexts: See the GameSetup class
```
GameContextSystem.Load();
```

#### Custom context: 
See the GameContextMainMenu class

#### Set a context: See the MainMenuInitialize class
```
GameContextSystem.SetContext(GameContextMainMenu.Instance, GameContextSystem.INDEX.MAIN);
GameContextSystem.SetContext(GameContextMainMenuCustom.Instance, GameContextSystem.INDEX.SUB1);
```

#### Remove a context:
```
GameContextSystem.SetContext(GameContextNone.Instance, GameContextSystem.INDEX.MAIN);
```

#### Set a context and add a layer: See the WindowSettings class
```
GameContextSystem.AddLayer(GameContextSettings.Instance, GameContextSystem.INDEX.MAIN);
or
GameContextSystem.AddLayer();
GameContextSystem.SetContext(GameContextSettings.Instance, GameContextSystem.INDEX.MAIN);
```

#### Remove context layer: See the WindowSettings class
```
GameContextSystem.RemoveLayer();
```

# Events

It began to be a nightmare to know which object listen to what. Events started to generate more and more spaghetti code despite a nice architecture. It was hard to locate issues. I use the same Singleton ScriptableObject pattern for the event to visualize their life in real time.

An event with no parameter has to extend GameEvent<T>. The system can manage events up to 4 parameters. It has to extend GameEvent<T, P1, P2, P3, P4>. As the GameContext system, this system will find that class automatically and will create an associated ScriptableObject located in: Resources/GameSystems/Events/List/YourEventName.asset

An object can listen, unlisten or trigger an event using a static method directly. The ScriptableObject associated with the event will display all objects than are listenning the event.

Additionally the system can constrain the listening of an event with one or more GameContext or a specific method.

Some API reference:

#### Load events: See the GameSetup class
```
GameEventSystem.Load();
```

#### Listen an event: See the MainMenu class
```
GameEventNewGame.Listen(this, NewGame).AddGameContextConstraint(GameContextMainMenu.Instance);
// NewGame is executed when the event "GameEventNewGame" is triggered but only if the context "GameContextMainMenu" is active
```

#### Unlisten an event: See the MainMenu class
```
GameEventNewGame.Unlisten(this, NewGame);
```

#### Trigger an event: See the MainMenu class
```
GameEventNewGame.Trigger();
```

# Libraries

In Mechanicus, we have a "Config" scene that contains a lot of data container objects. We can access to a data using the Service Locator pattern. The issue here was, if you or a member of team wanted to add a new data, the scene has to be open and saved. That cause sometimes conflicts or merge issue with Git.

As the GameContext system et the GameEvent system, I use the same Singleton ScriptableObject pattern. 
A library has to extend GameLibrary<T>, the system will find that class automatically and will create an associated ScriptableObject located in: Resources/GameSystems/Libraries/List/YourLibraryName.asset
  
The team members can now add data directly in the project window, without opening a scene and reducing the amount of conflicts in the source control system.

Some API reference:

#### Load libraries: See the GameSetup class
```
GameLibrarySystem.Load();
```

#### Get a library: See the MainMenu class
```
GameLibraryUi.Instance.testButton
```

# Logs

The Debug class in Unity is great, you can active or deactive all logs with a simple line of code:
```
Debug.unityLogger.logEnabled = false;
```
But there is no way (or I didn't find it) to prevent the compilation of Debug.Log calls. A simple Debug.Log("test") with a logEnabled = false will still allocate some memories.

So a solution to that is to wrap the Unity Debug log system with the [Conditional] attribute and compile the log calls only if the "Scripting Define Symbols" contains the "DEBUG_BUILD" constant.

Internally, we use Editor Console Pro (https://assetstore.unity.com/packages/tools/utilities/editor-console-pro-11889) and I wanted to take advantage of their custom filter feature. 
In this log system, you can tag (or not) every logs you want. This will add the tag in the console before the object you wanted to log.

Some API reference:

#### Initialize the log system with a specific config: See the GameSetup class
```
GameLogSystem.Initialize(new LogConfigTest());
```

#### Log something: See the MainMenu class
Log.Info(GameLibraryUi.Instance.testButton, LogConfigTest.TAG.TASK);
Log.Warning(GameLibraryUi.Instance.testButton, LogConfigTest.TAG.TASK);
Log.Exception(GameLibraryUi.Instance.testButton, LogConfigTest.TAG.TASK);
Log.Error(GameLibraryUi.Instance.testButton, LogConfigTest.TAG.TASK);

#### Setup the log system: See the LogConfigTest class
Add tags to the enum "TAG" and active or deactive those tags in the "SetupTags" method.

# UI

TODO

# Folder Architecture Convention
https://docs.google.com/document/d/1Vr-OTl4bF2BxqRWNAUzqmarMXYzwkCxpPjY25PfPKHo/edit?usp=sharing
