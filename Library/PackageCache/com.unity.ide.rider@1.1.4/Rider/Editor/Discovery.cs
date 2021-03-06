                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                folderObject = subkey?.GetValue("InstallLocation");
            if (folderObject == null) continue;
            var folder = folderObject.ToString();
            var possiblePath = Path.Combine(folder, @"bin\rider64.exe");
            if (File.Exists(possiblePath))
              installPaths.Add(possiblePath);
          }
        }
      }
    }

    private static string[] CollectPathsFromToolbox(string toolboxRiderRootPath, string dirName, string searchPattern,
      bool isMac)
    {
      if (!Directory.Exists(toolboxRiderRootPath))
        return new string[0];

      var channelDirs = Directory.GetDirectories(toolboxRiderRootPath);
      var paths = channelDirs.SelectMany(channelDir =>
        {
          try
          {
            // use history.json - last entry stands for the active build https://jetbrains.slack.com/archives/C07KNP99D/p1547807024066500?thread_ts=1547731708.057700&cid=C07KNP99D
            var historyFile = Path.Combine(channelDir, ".history.json");
            if (File.Exists(historyFile))
            {
              var json = File.ReadAllText(historyFile);
              var build = ToolboxHistory.GetLatestBuildFromJson(json);
              if (build != null)
              {
                var buildDir = Path.Combine(channelDir, build);
                var executablePaths = GetExecutablePaths(dirName, searchPattern, isMac, buildDir);
                if (executablePaths.Any())
                  return executablePaths;
              }
            }

            var channelFile = Path.Combine(channelDir, ".channel.settings.json");
            if (File.Exists(channelFile))
            {
              var json = File.ReadAllText(channelFile).Replace("active-application", "active_application");
              var build = ToolboxInstallData.GetLatestBuildFromJson(json);
              if (build != null)
              {
                var buildDir = Path.Combine(channelDir, build);
                var executablePaths = GetExecutablePaths(dirName, searchPattern, isMac, buildDir);
                if (executablePaths.Any())
                  return executablePaths;
              }
            }

            // changes in toolbox json files format may brake the logic above, so return all found Rider installations
            return Directory.GetDirectories(channelDir)
              .SelectMany(buildDir => GetExecutablePaths(dirName, searchPattern, isMac, buildDir));
          }
          catch (Exception e)
          {
            // do not write to Debug.Log, just log it.
            Logger.Warn($"Failed to get RiderPath from {channelDir}", e);
          }

          return new string[0];
        })
        .Where(c => !string.IsNullOrEmpty(c))
        .ToArray();
      return paths;
    }

    private static string[] GetExecutablePaths(string dirName, string searchPattern, bool isMac, string buildDir)
    {
      var folder = new DirectoryInfo(Path.Combine(buildDir, dirName));
      if (!folder.Exists)
        return new string[0];

      if (!isMac)
        return new[] {Path.Combine(folder.FullName, searchPattern)}.Where(File.Exists).ToArray();
      return folder.GetDirectories(searchPattern).Select(f => f.FullName)
        .Where(Directory.Exists).ToArray();
    }

    // Disable the "field is never assigned" compiler warning. We never assign it, but Unity does.
    // Note that Unity disable this warning in the generated C# projects
#pragma warning disable 0649

    [Serializable]
    class ToolboxHistory
    {
      public List<ItemNode> history;

      [CanBeNull]
      public static string GetLatestBuildFromJson(string json)
      {
        try
        {
#if UNITY_4_7 || UNITY_5_5
          return JsonConvert.DeserializeObject<ToolboxHistory>(json).history.LastOrDefault()?.item.build;
#else
          return JsonUtility.FromJson<ToolboxHistory>(json).history.LastOrDefault()?.item.build;
#endif
        }
        catch (Exception)
        {
          Logger.Warn($"Failed to get latest build from json {json}");
        }

        return null;
      }
    }

    [Serializable]
    class ItemNode
    {
      public BuildNode item;
    }

    [Serializable]
    class BuildNode
    {
      public string build;
    }

    // ReSharper disable once ClassNeverInstantiated.Global
    [Serializable]
    class ToolboxInstallData
    {
      // ReSharper disable once InconsistentNaming
      public ActiveApplication active_application;

      [CanBeNull]
      public static string GetLatestBuildFromJson(string json)
      {
        try
        {
#if UNITY_4_7 || UNITY_5_5
          var toolbox = JsonConvert.DeserializeObject<ToolboxInstallData>(json);
#else
          var toolbox = JsonUtility.FromJson<ToolboxInstallData>(json);
#endif
          var builds = toolbox.active_application.builds;
          if (builds != null && builds.Any())
            return builds.First();
        }
        catch (Exception)
        {
          Logger.Warn($"Failed to get latest build from json {json}");
        }

        return null;
      }
    }

    [Serializable]
    class ActiveApplication
    {
      // ReSharper disable once InconsistentNaming
      public List<string> builds;
    }

#pragma warning restore 0649

    public struct RiderInfo
    {
      public bool IsToolbox;
      public string Presentation;
      public string BuildVersion;
      public string Path;

      public RiderInfo(string path, bool isToolbox)
      {
        if (path == RiderScriptEditor.CurrentEditor)
        {
          RiderScriptEditorData.instance.Init();
          BuildVersion = RiderScriptEditorData.instance.currentEditorVersion;
        }
        else
          BuildVersion = GetBuildNumber(path);
        Path = new FileInfo(path).FullName; // normalize separators
        var presentation = "Rider " + BuildVersion;
        if (isToolbox)
          presentation += " (JetBrains Toolbox)";

        Presentation = presentation;
        IsToolbox = isToolbox;
      }
    }

    private static class Logger
    {
      internal static void Warn(string message, Exception e = null)
      {
#if RIDER_EDITOR_PLUGIN // can't be used in com.unity.ide.rider
        Log.GetLog(typeof(RiderPathLocator).Name).Warn(message);
        if (e != null) 
          Log.GetLog(typeof(RiderPathLocator).Name).Warn(e);
#else
        Debug.LogError(message);
        if (e != null)
          Debug.LogException(e);
#endif
      }
    }
  }
}