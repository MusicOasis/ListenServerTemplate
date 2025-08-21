// (c) 2024 Discord, Inc.

using System;
using System.IO;
using System.Reflection;
using UnrealBuildTool;

public partial class DiscordPartnerSDKLibrary : ModuleRules {
    string RuntimeBinariesPath = "$(PluginDir)/Binaries/ThirdParty/DiscordPartnerSDKLibrary/";

    public DiscordPartnerSDKLibrary(ReadOnlyTargetRules Target)
      : base(Target) {
        Type = ModuleType.External;

        PublicIncludePaths.AddRange(new string[] { Path.Combine(ModuleDirectory, "include") });

        var setupMethodName = $"Setup_{Target.Platform}";
        var setupMethod =
          GetType().GetMethod(setupMethodName, BindingFlags.NonPublic | BindingFlags.Instance);
        if (setupMethod != null) {
            setupMethod.Invoke(this, new object[] { Target });
        }

        if (Target.Platform == UnrealTargetPlatform.Win64) {
            // Add the import library
            PublicAdditionalLibraries.Add(
              Path.Combine(ModuleDirectory, "lib", "discord_partner_sdk.dll.lib"));

            // Delay-load the DLL, so we can load it from the right place first
            PublicDelayLoadDLLs.Add("discord_partner_sdk.dll");

            // Ensure that the DLL is staged along with the executable
            RuntimeDependencies.Add(RuntimeBinariesPath + "Win64/discord_partner_sdk.dll");
        } else if (Target.Platform == UnrealTargetPlatform.Linux) {
            string SharedLibPath =
              Path.Combine(RuntimeBinariesPath, "Linux", "libdiscord_partner_sdk.so");
            PublicAdditionalLibraries.Add(SharedLibPath);
            RuntimeDependencies.Add(SharedLibPath);
        } else if (Target.Platform == UnrealTargetPlatform.Mac) {
            string SharedLibPath =
              Path.Combine(RuntimeBinariesPath, "Mac", "libdiscord_partner_sdk.dylib");
            PublicAdditionalLibraries.Add(SharedLibPath);
            RuntimeDependencies.Add(SharedLibPath);
        } else if (Target.Platform == UnrealTargetPlatform.IOS) {
            string FrameworkPath = Path.Combine(PluginDirectory,
                                                "Binaries",
                                                "ThirdParty",
                                                "DiscordPartnerSDKLibrary",
                                                "IOS",
                                                "discord_partner_sdk.xcframework");
            PublicAdditionalFrameworks.Add(
              new Framework("discord_partner_sdk", FrameworkPath, bCopyFramework: true));
        } else if (Target.Platform == UnrealTargetPlatform.Android) {
            PublicAdditionalLibraries.Add(Path.Combine(
              RuntimeBinariesPath, "Android", "jni", "arm64-v8a", "libdiscord_partner_sdk.so"));
            PublicAdditionalLibraries.Add(Path.Combine(
              RuntimeBinariesPath, "Android", "jni", "armeabi-v7a", "libdiscord_partner_sdk.so"));
            PublicAdditionalLibraries.Add(Path.Combine(
              RuntimeBinariesPath, "Android", "jni", "x86", "libdiscord_partner_sdk.so"));
            PublicAdditionalLibraries.Add(Path.Combine(
              RuntimeBinariesPath, "Android", "jni", "x86_64", "libdiscord_partner_sdk.so"));
        }
    }
}
