# Build Environment Config
a Unity package to manage multiple environments with ease

### How to?

1. Create a class inherits from `BaseBuildEnvironment` with a `[CreateAssetMenu]` attribute
```cs
    [CreateAssetMenu(fileName = "Platform Environment", menuName = "Data/Platform Environment/Data")]
    public class BuildEnvironment : BaseBuildEnvironment
    {

    }
```

1. Define the properties to be included in the environment. e.g analytics, development build, in game cheat with the property getter too
```cs
    [CreateAssetMenu(fileName = "Platform Environment", menuName = "Data/Platform Environment/Data")]
    public class BuildEnvironment : BaseBuildEnvironment
    {
        [SerializeField] private bool _developmentBuild;
        [SerializeField] private bool _enableAnalytics;

        public bool DevelopmentBuild => _developmentBuild;
        public bool EnableAnalytics => _enableAnalytics;
    }
```


1. Now Let's Create the a editor window that we can use to toggle the environment. Create a class derives from `BaseBuildEnvironmentConfigWindow`

```cs
public class MainBuildEnvironmentConfigWindow : BaseBuildEnvironmentConfigWindow
{

}
```

1. In this class, we have to override several things such as the symbol definition list & adding the list manually
```CS
private const string DEVELOPMENT = "DEVELOPMENT";
private const string ANALYTICS = "ANALYTICS";

public override string[] SYMBOLS => new string[] {
    DEVELOPMENT,
    ANALYTICS
};
```

1. Now Override the Update List, this is where you want to add the active symbols
```cs

protected override void UpdateList(BaseBuildEnvironment activeEnvironment, List<string> symbolList)
{
    var env = activeEnvironment as BuildEnvironment;

    if (env.DevelopmentBuild)
        symbolList.Add(DEVELOPMENT);

    if (env.EnableAnalytics)
        symbolList.Add(ANALYTICS);
}
```

1. Let's also set where the editor should scan the asset path, you can customize the path by your liking
```
protected override string ENVIRONMENT_DATA_CONTAINER_PATH => "Assets/Data/Environment/";
```

1. Lastly, add the window activation method
```cs
[MenuItem(MENU_ITEM_PATH)]
public static void SetGameBuild()
    => GetWindow<MainBuildEnvironmentConfigWindow>("Game Build");

```

1. All the class has been setup, now lets try it up by going to create our environment data containe by right clicking on Project Folder, Data > Platform Environment > Container
![Data Creation](Docs\docs_datacreation.png)

1. Now also create the data by going to Data > Platform Environment > Data

1. Now add the data to the Platform Environment array
![Window](Docs\docs_window.png)

1. To test it out, head over to Tools > Set Release Target (Make sure your `PlatformDataContainer` is in the correct path!)


## Complete Class

```cs
 [CreateAssetMenu(fileName = "Platform Environment", menuName = "Data/Platform Environment/Data")]
    public class BuildEnvironment : BaseBuildEnvironment
    {
        [SerializeField] private bool _developmentBuild;
        [SerializeField] private bool _enableAnalytics;

        public bool DevelopmentBuild => _developmentBuild;
        public bool EnableAnalytics => _enableAnalytics;
    }
```

```cs 
public class MainBuildEnvironmentConfigWindow : BaseBuildEnvironmentConfigWindow
{
        private const string DEVELOPMENT_SCRIPT_SYMBOL = "DEVELOPMENT";
        private const string FTUE_SCRIPT_SYMBOL = "FTUE";

        public override string[] SYMBOLS => new string[] {
            DEVELOPMENT_SCRIPT_SYMBOL,
            FTUE_SCRIPT_SYMBOL,
        };

        protected override string ENVIRONMENT_DATA_CONTAINER_PATH => "Assets/Data/Environment/";

        [MenuItem(MENU_ITEM_PATH)]
        public static void SetGameBuild()
            => GetWindow<MainBuildEnvironmentConfigWindow>("Game Build");

        protected override void UpdateList(BaseBuildEnvironment activeEnvironment, List<string> symbolList)
        {
            var env = activeEnvironment as BuildEnvironment;

        if (env.DevelopmentBuild)
            symbolList.Add(DEVELOPMENT_SCRIPT_SYMBOL);

        if (env.EnableAnalytics)
            symbolList.Add(INGAME_CHEAT_SCRIPT_SYMBOL);
    }
}

```
