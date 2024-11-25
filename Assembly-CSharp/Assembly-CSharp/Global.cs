using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;
using Klei;
using KMod;
using ProcGenGame;
using STRINGS;
using UnityEngine;
using UnityEngine.U2D;
using Directory = System.IO.Directory;

public class Global : MonoBehaviour {
    private static         GameInputManager mInputManager;
    private static         string           saveFolderTestResult = "unknown";
    public static readonly string           LanguageModKey       = "LanguageMod";
    public static readonly string           LanguageCodeKey      = "LanguageCode";
    private readonly       DevToolManager   DevTools             = new DevToolManager();
    public                 SpriteAtlas[]    forcedAtlasInitializationList;
    public                 GameObject       globalCanvas;
    private                bool             gotKleiUserID;
    public                 GameObject       modErrorsPrefab;
    public                 Manager          modManager;
    private                bool             updated_with_initialized_distribution_platform;
    public static          Global           Instance { get; private set; }

    public static BindingEntry[] GenerateDefaultBindings(bool hotKeyBuildMenuPermitted = true) {
        var list = new List<BindingEntry> {
            new BindingEntry(null,   GamepadButton.Start,      KKeyCode.Escape, Modifier.None, Action.Escape, false),
            new BindingEntry("Root", GamepadButton.NumButtons, KKeyCode.W,      Modifier.None, Action.PanUp),
            new BindingEntry("Root", GamepadButton.NumButtons, KKeyCode.S,      Modifier.None, Action.PanDown),
            new BindingEntry("Root", GamepadButton.NumButtons, KKeyCode.A,      Modifier.None, Action.PanLeft),
            new BindingEntry("Root", GamepadButton.NumButtons, KKeyCode.D,      Modifier.None, Action.PanRight),
            new BindingEntry("Tool", GamepadButton.NumButtons, KKeyCode.O,      Modifier.None, Action.RotateBuilding),
            new BindingEntry("Management",
                             GamepadButton.NumButtons,
                             KKeyCode.L,
                             Modifier.None,
                             Action.ManagePriorities),
            new BindingEntry("Management",
                             GamepadButton.NumButtons,
                             KKeyCode.F,
                             Modifier.None,
                             Action.ManageConsumables),
            new BindingEntry("Management", GamepadButton.NumButtons, KKeyCode.V, Modifier.None, Action.ManageVitals),
            new BindingEntry("Management", GamepadButton.NumButtons, KKeyCode.R, Modifier.None, Action.ManageResearch),
            new BindingEntry("Management", GamepadButton.NumButtons, KKeyCode.E, Modifier.None, Action.ManageReport),
            new BindingEntry("Management", GamepadButton.NumButtons, KKeyCode.U, Modifier.None, Action.ManageDatabase),
            new BindingEntry("Management", GamepadButton.NumButtons, KKeyCode.J, Modifier.None, Action.ManageSkills),
            new BindingEntry("Management",
                             GamepadButton.NumButtons,
                             KKeyCode.Period,
                             Modifier.None,
                             Action.ManageSchedule),
            new BindingEntry("Management", GamepadButton.NumButtons, KKeyCode.Z, Modifier.None, Action.ManageStarmap),
            new BindingEntry("Root", GamepadButton.NumButtons, KKeyCode.G, Modifier.None, Action.Dig),
            new BindingEntry("Root", GamepadButton.NumButtons, KKeyCode.M, Modifier.None, Action.Mop),
            new BindingEntry("Root", GamepadButton.NumButtons, KKeyCode.K, Modifier.None, Action.Clear),
            new BindingEntry("Root", GamepadButton.NumButtons, KKeyCode.I, Modifier.None, Action.Disinfect),
            new BindingEntry("Root", GamepadButton.NumButtons, KKeyCode.T, Modifier.None, Action.Attack),
            new BindingEntry("Root", GamepadButton.NumButtons, KKeyCode.N, Modifier.None, Action.Capture),
            new BindingEntry("Root", GamepadButton.NumButtons, KKeyCode.Y, Modifier.None, Action.Harvest),
            new BindingEntry("Root", GamepadButton.NumButtons, KKeyCode.Insert, Modifier.None, Action.EmptyPipe),
            new BindingEntry("Root", GamepadButton.NumButtons, KKeyCode.D, Modifier.Shift, Action.Disconnect),
            new BindingEntry("Root", GamepadButton.NumButtons, KKeyCode.P, Modifier.None, Action.Prioritize),
            new BindingEntry("Root", GamepadButton.NumButtons, KKeyCode.S, Modifier.Alt, Action.ToggleScreenshotMode),
            new BindingEntry("Root", GamepadButton.NumButtons, KKeyCode.C, Modifier.None, Action.BuildingCancel),
            new BindingEntry("Root", GamepadButton.NumButtons, KKeyCode.X, Modifier.None, Action.BuildingDeconstruct),
            new BindingEntry("Root", GamepadButton.NumButtons, KKeyCode.Tab, Modifier.None, Action.CycleSpeed),
            new BindingEntry("Root", GamepadButton.NumButtons, KKeyCode.H, Modifier.None, Action.CameraHome),
            new BindingEntry("Root", GamepadButton.A, KKeyCode.Mouse0, Modifier.None, Action.MouseLeft, false),
            new BindingEntry("Root", GamepadButton.A, KKeyCode.Mouse0, Modifier.Shift, Action.ShiftMouseLeft, false),
            new BindingEntry("Root", GamepadButton.B, KKeyCode.Mouse1, Modifier.None, Action.MouseRight, false),
            new BindingEntry("Root",
                             GamepadButton.NumButtons,
                             KKeyCode.Mouse2,
                             Modifier.None,
                             Action.MouseMiddle,
                             false),
            new BindingEntry("Root", GamepadButton.NumButtons, KKeyCode.Alpha1, Modifier.None, Action.Plan1),
            new BindingEntry("Root", GamepadButton.NumButtons, KKeyCode.Alpha2, Modifier.None, Action.Plan2),
            new BindingEntry("Root", GamepadButton.NumButtons, KKeyCode.Alpha3, Modifier.None, Action.Plan3),
            new BindingEntry("Root", GamepadButton.NumButtons, KKeyCode.Alpha4, Modifier.None, Action.Plan4),
            new BindingEntry("Root", GamepadButton.NumButtons, KKeyCode.Alpha5, Modifier.None, Action.Plan5),
            new BindingEntry("Root", GamepadButton.NumButtons, KKeyCode.Alpha6, Modifier.None, Action.Plan6),
            new BindingEntry("Root", GamepadButton.NumButtons, KKeyCode.Alpha7, Modifier.None, Action.Plan7),
            new BindingEntry("Root", GamepadButton.NumButtons, KKeyCode.Alpha8, Modifier.None, Action.Plan8),
            new BindingEntry("Root", GamepadButton.NumButtons, KKeyCode.Alpha9, Modifier.None, Action.Plan9),
            new BindingEntry("Root", GamepadButton.NumButtons, KKeyCode.Alpha0, Modifier.None, Action.Plan10),
            new BindingEntry("Root", GamepadButton.NumButtons, KKeyCode.Minus, Modifier.None, Action.Plan11),
            new BindingEntry("Root", GamepadButton.NumButtons, KKeyCode.Equals, Modifier.None, Action.Plan12),
            new BindingEntry("Root", GamepadButton.NumButtons, KKeyCode.Minus, Modifier.Shift, Action.Plan13),
            new BindingEntry("Root", GamepadButton.NumButtons, KKeyCode.Equals, Modifier.Shift, Action.Plan14),
            new BindingEntry("Root", GamepadButton.NumButtons, KKeyCode.Backspace, Modifier.Shift, Action.Plan15),
            new BindingEntry("Root", GamepadButton.NumButtons, KKeyCode.B, Modifier.None, Action.CopyBuilding),
            new BindingEntry("Root", GamepadButton.RT,         KKeyCode.MouseScrollUp, Modifier.None, Action.ZoomIn),
            new BindingEntry("Root", GamepadButton.LT,         KKeyCode.MouseScrollDown, Modifier.None, Action.ZoomOut),
            new BindingEntry("Root", GamepadButton.NumButtons, KKeyCode.F1, Modifier.None, Action.Overlay1),
            new BindingEntry("Root", GamepadButton.NumButtons, KKeyCode.F2, Modifier.None, Action.Overlay2),
            new BindingEntry("Root", GamepadButton.NumButtons, KKeyCode.F3, Modifier.None, Action.Overlay3),
            new BindingEntry("Root", GamepadButton.NumButtons, KKeyCode.F4, Modifier.None, Action.Overlay4),
            new BindingEntry("Root", GamepadButton.NumButtons, KKeyCode.F5, Modifier.None, Action.Overlay5),
            new BindingEntry("Root", GamepadButton.NumButtons, KKeyCode.F6, Modifier.None, Action.Overlay6),
            new BindingEntry("Root", GamepadButton.NumButtons, KKeyCode.F7, Modifier.None, Action.Overlay7),
            new BindingEntry("Root", GamepadButton.NumButtons, KKeyCode.F8, Modifier.None, Action.Overlay8),
            new BindingEntry("Root", GamepadButton.NumButtons, KKeyCode.F9, Modifier.None, Action.Overlay9),
            new BindingEntry("Root", GamepadButton.NumButtons, KKeyCode.F10, Modifier.None, Action.Overlay10),
            new BindingEntry("Root", GamepadButton.NumButtons, KKeyCode.F11, Modifier.None, Action.Overlay11),
            new BindingEntry("Root", GamepadButton.NumButtons, KKeyCode.F1, Modifier.Shift, Action.Overlay12),
            new BindingEntry("Root", GamepadButton.NumButtons, KKeyCode.F2, Modifier.Shift, Action.Overlay13),
            new BindingEntry("Root", GamepadButton.NumButtons, KKeyCode.F3, Modifier.Shift, Action.Overlay14),
            new BindingEntry("Root",
                             GamepadButton.NumButtons,
                             KKeyCode.F4,
                             Modifier.Shift,
                             Action.Overlay15,
                             DlcManager.AVAILABLE_EXPANSION1_ONLY),
            new BindingEntry("Root", GamepadButton.NumButtons, KKeyCode.KeypadPlus,  Modifier.None, Action.SpeedUp),
            new BindingEntry("Root", GamepadButton.NumButtons, KKeyCode.KeypadMinus, Modifier.None, Action.SlowDown),
            new BindingEntry("Root", GamepadButton.Back,       KKeyCode.Space,       Modifier.None, Action.TogglePause),
            new BindingEntry("Navigation",
                             GamepadButton.NumButtons,
                             KKeyCode.Alpha1,
                             Modifier.Ctrl,
                             Action.SetUserNav1),
            new BindingEntry("Navigation",
                             GamepadButton.NumButtons,
                             KKeyCode.Alpha2,
                             Modifier.Ctrl,
                             Action.SetUserNav2),
            new BindingEntry("Navigation",
                             GamepadButton.NumButtons,
                             KKeyCode.Alpha3,
                             Modifier.Ctrl,
                             Action.SetUserNav3),
            new BindingEntry("Navigation",
                             GamepadButton.NumButtons,
                             KKeyCode.Alpha4,
                             Modifier.Ctrl,
                             Action.SetUserNav4),
            new BindingEntry("Navigation",
                             GamepadButton.NumButtons,
                             KKeyCode.Alpha5,
                             Modifier.Ctrl,
                             Action.SetUserNav5),
            new BindingEntry("Navigation",
                             GamepadButton.NumButtons,
                             KKeyCode.Alpha6,
                             Modifier.Ctrl,
                             Action.SetUserNav6),
            new BindingEntry("Navigation",
                             GamepadButton.NumButtons,
                             KKeyCode.Alpha7,
                             Modifier.Ctrl,
                             Action.SetUserNav7),
            new BindingEntry("Navigation",
                             GamepadButton.NumButtons,
                             KKeyCode.Alpha8,
                             Modifier.Ctrl,
                             Action.SetUserNav8),
            new BindingEntry("Navigation",
                             GamepadButton.NumButtons,
                             KKeyCode.Alpha9,
                             Modifier.Ctrl,
                             Action.SetUserNav9),
            new BindingEntry("Navigation",
                             GamepadButton.NumButtons,
                             KKeyCode.Alpha0,
                             Modifier.Ctrl,
                             Action.SetUserNav10),
            new BindingEntry("Navigation",
                             GamepadButton.NumButtons,
                             KKeyCode.Alpha1,
                             Modifier.Shift,
                             Action.GotoUserNav1),
            new BindingEntry("Navigation",
                             GamepadButton.NumButtons,
                             KKeyCode.Alpha2,
                             Modifier.Shift,
                             Action.GotoUserNav2),
            new BindingEntry("Navigation",
                             GamepadButton.NumButtons,
                             KKeyCode.Alpha3,
                             Modifier.Shift,
                             Action.GotoUserNav3),
            new BindingEntry("Navigation",
                             GamepadButton.NumButtons,
                             KKeyCode.Alpha4,
                             Modifier.Shift,
                             Action.GotoUserNav4),
            new BindingEntry("Navigation",
                             GamepadButton.NumButtons,
                             KKeyCode.Alpha5,
                             Modifier.Shift,
                             Action.GotoUserNav5),
            new BindingEntry("Navigation",
                             GamepadButton.NumButtons,
                             KKeyCode.Alpha6,
                             Modifier.Shift,
                             Action.GotoUserNav6),
            new BindingEntry("Navigation",
                             GamepadButton.NumButtons,
                             KKeyCode.Alpha7,
                             Modifier.Shift,
                             Action.GotoUserNav7),
            new BindingEntry("Navigation",
                             GamepadButton.NumButtons,
                             KKeyCode.Alpha8,
                             Modifier.Shift,
                             Action.GotoUserNav8),
            new BindingEntry("Navigation",
                             GamepadButton.NumButtons,
                             KKeyCode.Alpha9,
                             Modifier.Shift,
                             Action.GotoUserNav9),
            new BindingEntry("Navigation",
                             GamepadButton.NumButtons,
                             KKeyCode.Alpha0,
                             Modifier.Shift,
                             Action.GotoUserNav10),
            new BindingEntry("CinematicCamera",
                             GamepadButton.NumButtons,
                             KKeyCode.C,
                             Modifier.None,
                             Action.CinemaCamEnable,
                             true,
                             true),
            new BindingEntry("CinematicCamera",
                             GamepadButton.NumButtons,
                             KKeyCode.A,
                             Modifier.None,
                             Action.CinemaPanLeft,
                             true,
                             true),
            new BindingEntry("CinematicCamera",
                             GamepadButton.NumButtons,
                             KKeyCode.D,
                             Modifier.None,
                             Action.CinemaPanRight,
                             true,
                             true),
            new BindingEntry("CinematicCamera",
                             GamepadButton.NumButtons,
                             KKeyCode.W,
                             Modifier.None,
                             Action.CinemaPanUp,
                             true,
                             true),
            new BindingEntry("CinematicCamera",
                             GamepadButton.NumButtons,
                             KKeyCode.S,
                             Modifier.None,
                             Action.CinemaPanDown,
                             true,
                             true),
            new BindingEntry("CinematicCamera",
                             GamepadButton.NumButtons,
                             KKeyCode.I,
                             Modifier.None,
                             Action.CinemaZoomIn,
                             true,
                             true),
            new BindingEntry("CinematicCamera",
                             GamepadButton.NumButtons,
                             KKeyCode.O,
                             Modifier.None,
                             Action.CinemaZoomOut,
                             true,
                             true),
            new BindingEntry("CinematicCamera",
                             GamepadButton.NumButtons,
                             KKeyCode.Z,
                             Modifier.None,
                             Action.CinemaZoomSpeedPlus,
                             true,
                             true),
            new BindingEntry("CinematicCamera",
                             GamepadButton.NumButtons,
                             KKeyCode.Z,
                             Modifier.Shift,
                             Action.CinemaZoomSpeedMinus,
                             true,
                             true),
            new BindingEntry("CinematicCamera",
                             GamepadButton.NumButtons,
                             KKeyCode.P,
                             Modifier.None,
                             Action.CinemaUnpauseOnMove,
                             true,
                             true),
            new BindingEntry("CinematicCamera",
                             GamepadButton.NumButtons,
                             KKeyCode.T,
                             Modifier.None,
                             Action.CinemaToggleLock,
                             true,
                             true),
            new BindingEntry("CinematicCamera",
                             GamepadButton.NumButtons,
                             KKeyCode.E,
                             Modifier.None,
                             Action.CinemaToggleEasing,
                             true,
                             true),
            new BindingEntry("Building", GamepadButton.NumButtons, KKeyCode.Slash, Modifier.None, Action.ToggleOpen),
            new BindingEntry("Building",
                             GamepadButton.NumButtons,
                             KKeyCode.Return,
                             Modifier.None,
                             Action.ToggleEnabled),
            new BindingEntry("Building",
                             GamepadButton.NumButtons,
                             KKeyCode.Backslash,
                             Modifier.None,
                             Action.BuildingUtility1),
            new BindingEntry("Building",
                             GamepadButton.NumButtons,
                             KKeyCode.LeftBracket,
                             Modifier.None,
                             Action.BuildingUtility2),
            new BindingEntry("Building",
                             GamepadButton.NumButtons,
                             KKeyCode.RightBracket,
                             Modifier.None,
                             Action.BuildingUtility3),
            new BindingEntry("Root", GamepadButton.NumButtons, KKeyCode.LeftAlt,   Modifier.Alt, Action.AlternateView),
            new BindingEntry("Root", GamepadButton.NumButtons, KKeyCode.RightAlt,  Modifier.Alt, Action.AlternateView),
            new BindingEntry("Tool", GamepadButton.NumButtons, KKeyCode.LeftShift, Modifier.Shift, Action.DragStraight),
            new BindingEntry("Tool",
                             GamepadButton.NumButtons,
                             KKeyCode.RightShift,
                             Modifier.Shift,
                             Action.DragStraight),
            new BindingEntry("Debug", GamepadButton.NumButtons, KKeyCode.T,  Modifier.Ctrl, Action.DebugFocus),
            new BindingEntry("Debug", GamepadButton.NumButtons, KKeyCode.U,  Modifier.Ctrl, Action.DebugUltraTestMode),
            new BindingEntry("Debug", GamepadButton.NumButtons, KKeyCode.F1, Modifier.Alt,  Action.DebugToggleUI),
            new BindingEntry("Debug", GamepadButton.NumButtons, KKeyCode.F3, Modifier.Alt,  Action.DebugCollectGarbage),
            new BindingEntry("Debug", GamepadButton.NumButtons, KKeyCode.F7, Modifier.Alt,  Action.DebugInvincible),
            new BindingEntry("Debug",
                             GamepadButton.NumButtons,
                             KKeyCode.F10,
                             Modifier.Alt,
                             Action.DebugForceLightEverywhere),
            new BindingEntry("Debug", GamepadButton.NumButtons, KKeyCode.F10, Modifier.Shift, Action.DebugElementTest),
            new BindingEntry("Debug", GamepadButton.NumButtons, KKeyCode.F12, Modifier.Shift, Action.DebugTileTest),
            new BindingEntry("Debug", GamepadButton.NumButtons, KKeyCode.N, Modifier.Alt, Action.DebugRefreshNavCell),
            new BindingEntry("Debug", GamepadButton.NumButtons, KKeyCode.Q, Modifier.Ctrl, Action.DebugGotoTarget),
            new BindingEntry("Debug", GamepadButton.NumButtons, KKeyCode.S, Modifier.Ctrl, Action.DebugSelectMaterial),
            new BindingEntry("Debug", GamepadButton.NumButtons, KKeyCode.M, Modifier.Ctrl, Action.DebugToggleMusic),
            new BindingEntry("Debug",
                             GamepadButton.NumButtons,
                             KKeyCode.F,
                             Modifier.Ctrl,
                             Action.DebugToggleClusterFX,
                             DlcManager.AVAILABLE_EXPANSION1_ONLY),
            new BindingEntry("Debug", GamepadButton.NumButtons, KKeyCode.Backspace, Modifier.None, Action.DebugToggle),
            new BindingEntry("Debug",
                             GamepadButton.NumButtons,
                             KKeyCode.Backspace,
                             Modifier.Ctrl,
                             Action.DebugToggleFastWorkers),
            new BindingEntry("Debug", GamepadButton.NumButtons, KKeyCode.Q, Modifier.Alt, Action.DebugTeleport),
            new BindingEntry("Debug",
                             GamepadButton.NumButtons,
                             KKeyCode.F2,
                             Modifier.Alt,
                             Action.DebugSpawnMinionAtmoSuit),
            new BindingEntry("Debug", GamepadButton.NumButtons, KKeyCode.F2, Modifier.Ctrl, Action.DebugSpawnMinion),
            new BindingEntry("Debug", GamepadButton.NumButtons, KKeyCode.F3, Modifier.Ctrl, Action.DebugPlace),
            new BindingEntry("Debug",
                             GamepadButton.NumButtons,
                             KKeyCode.F4,
                             Modifier.Ctrl,
                             Action.DebugInstantBuildMode),
            new BindingEntry("Debug", GamepadButton.NumButtons, KKeyCode.F5, Modifier.Ctrl, Action.DebugSlowTestMode),
            new BindingEntry("Debug", GamepadButton.NumButtons, KKeyCode.F6, Modifier.Ctrl, Action.DebugDig),
            new BindingEntry("Debug", GamepadButton.NumButtons, KKeyCode.F8, Modifier.Ctrl, Action.DebugExplosion),
            new BindingEntry("Debug",
                             GamepadButton.NumButtons,
                             KKeyCode.F9,
                             Modifier.Ctrl,
                             Action.DebugDiscoverAllElements),
            new BindingEntry("Debug",
                             GamepadButton.NumButtons,
                             KKeyCode.T,
                             Modifier.Alt,
                             Action.DebugToggleSelectInEditor),
            new BindingEntry("Debug", GamepadButton.NumButtons, KKeyCode.P, Modifier.Alt, Action.DebugPathFinding),
            new BindingEntry("Debug", GamepadButton.NumButtons, KKeyCode.C, Modifier.Ctrl, Action.DebugCheerEmote),
            new BindingEntry("Debug", GamepadButton.NumButtons, KKeyCode.Z, Modifier.Alt, Action.DebugSuperSpeed),
            new BindingEntry("Debug", GamepadButton.NumButtons, KKeyCode.Equals, Modifier.Alt, Action.DebugGameStep),
            new BindingEntry("Debug", GamepadButton.NumButtons, KKeyCode.Minus, Modifier.Alt, Action.DebugSimStep),
            new BindingEntry("Debug", GamepadButton.NumButtons, KKeyCode.X, Modifier.Alt, Action.DebugNotification),
            new BindingEntry("Debug",
                             GamepadButton.NumButtons,
                             KKeyCode.C,
                             Modifier.Alt,
                             Action.DebugNotificationMessage),
            new BindingEntry("Debug",
                             GamepadButton.NumButtons,
                             KKeyCode.BackQuote,
                             Modifier.None,
                             Action.ToggleProfiler),
            new BindingEntry("Debug",
                             GamepadButton.NumButtons,
                             KKeyCode.BackQuote,
                             Modifier.Alt,
                             Action.ToggleChromeProfiler),
            new BindingEntry("Debug",
                             GamepadButton.NumButtons,
                             KKeyCode.F1,
                             Modifier.Ctrl,
                             Action.DebugDumpSceneParitionerLeakData),
            new BindingEntry("Debug",
                             GamepadButton.NumButtons,
                             KKeyCode.F12,
                             Modifier.Ctrl,
                             Action.DebugTriggerException),
            new BindingEntry("Debug", GamepadButton.NumButtons, KKeyCode.F12, (Modifier)6,   Action.DebugTriggerError),
            new BindingEntry("Debug", GamepadButton.NumButtons, KKeyCode.F10, Modifier.Ctrl, Action.DebugDumpGCRoots),
            new BindingEntry("Debug",
                             GamepadButton.NumButtons,
                             KKeyCode.F10,
                             (Modifier)3,
                             Action.DebugDumpGarbageReferences),
            new BindingEntry("Debug", GamepadButton.NumButtons, KKeyCode.F11, Modifier.Ctrl, Action.DebugDumpEventData),
            new BindingEntry("Debug", GamepadButton.NumButtons, KKeyCode.F7, (Modifier)3, Action.DebugCrashSim),
            new BindingEntry("Debug", GamepadButton.NumButtons, KKeyCode.Alpha9, Modifier.Alt, Action.DebugNextCall),
            new BindingEntry("Debug", GamepadButton.NumButtons, KKeyCode.Alpha1, Modifier.Alt, Action.SreenShot1x),
            new BindingEntry("Debug", GamepadButton.NumButtons, KKeyCode.Alpha2, Modifier.Alt, Action.SreenShot2x),
            new BindingEntry("Debug", GamepadButton.NumButtons, KKeyCode.Alpha3, Modifier.Alt, Action.SreenShot8x),
            new BindingEntry("Debug", GamepadButton.NumButtons, KKeyCode.Alpha4, Modifier.Alt, Action.SreenShot32x),
            new BindingEntry("Debug", GamepadButton.NumButtons, KKeyCode.Alpha5, Modifier.Alt, Action.DebugLockCursor),
            new BindingEntry("Debug",
                             GamepadButton.NumButtons,
                             KKeyCode.Alpha0,
                             Modifier.Alt,
                             Action.DebugTogglePersonalPriorityComparison),
            new BindingEntry("Root",
                             GamepadButton.NumButtons,
                             KKeyCode.Return,
                             Modifier.None,
                             Action.DialogSubmit,
                             false),
            new BindingEntry("Analog",
                             GamepadButton.NumButtons,
                             KKeyCode.None,
                             Modifier.None,
                             Action.AnalogCamera,
                             false),
            new BindingEntry("Analog",
                             GamepadButton.NumButtons,
                             KKeyCode.None,
                             Modifier.None,
                             Action.AnalogCursor,
                             false),
            new BindingEntry("BuildingsMenu",
                             GamepadButton.NumButtons,
                             KKeyCode.A,
                             Modifier.None,
                             Action.BuildMenuKeyA,
                             false,
                             true),
            new BindingEntry("BuildingsMenu",
                             GamepadButton.NumButtons,
                             KKeyCode.B,
                             Modifier.None,
                             Action.BuildMenuKeyB,
                             false,
                             true),
            new BindingEntry("BuildingsMenu",
                             GamepadButton.NumButtons,
                             KKeyCode.C,
                             Modifier.None,
                             Action.BuildMenuKeyC,
                             false,
                             true),
            new BindingEntry("BuildingsMenu",
                             GamepadButton.NumButtons,
                             KKeyCode.D,
                             Modifier.None,
                             Action.BuildMenuKeyD,
                             false,
                             true),
            new BindingEntry("BuildingsMenu",
                             GamepadButton.NumButtons,
                             KKeyCode.E,
                             Modifier.None,
                             Action.BuildMenuKeyE,
                             false,
                             true),
            new BindingEntry("BuildingsMenu",
                             GamepadButton.NumButtons,
                             KKeyCode.F,
                             Modifier.None,
                             Action.BuildMenuKeyF,
                             false,
                             true),
            new BindingEntry("BuildingsMenu",
                             GamepadButton.NumButtons,
                             KKeyCode.G,
                             Modifier.None,
                             Action.BuildMenuKeyG,
                             false,
                             true),
            new BindingEntry("BuildingsMenu",
                             GamepadButton.NumButtons,
                             KKeyCode.H,
                             Modifier.None,
                             Action.BuildMenuKeyH,
                             false,
                             true),
            new BindingEntry("BuildingsMenu",
                             GamepadButton.NumButtons,
                             KKeyCode.I,
                             Modifier.None,
                             Action.BuildMenuKeyI,
                             false,
                             true),
            new BindingEntry("BuildingsMenu",
                             GamepadButton.NumButtons,
                             KKeyCode.J,
                             Modifier.None,
                             Action.BuildMenuKeyJ,
                             false,
                             true),
            new BindingEntry("BuildingsMenu",
                             GamepadButton.NumButtons,
                             KKeyCode.K,
                             Modifier.None,
                             Action.BuildMenuKeyK,
                             false,
                             true),
            new BindingEntry("BuildingsMenu",
                             GamepadButton.NumButtons,
                             KKeyCode.L,
                             Modifier.None,
                             Action.BuildMenuKeyL,
                             false,
                             true),
            new BindingEntry("BuildingsMenu",
                             GamepadButton.NumButtons,
                             KKeyCode.M,
                             Modifier.None,
                             Action.BuildMenuKeyM,
                             false,
                             true),
            new BindingEntry("BuildingsMenu",
                             GamepadButton.NumButtons,
                             KKeyCode.N,
                             Modifier.None,
                             Action.BuildMenuKeyN,
                             false,
                             true),
            new BindingEntry("BuildingsMenu",
                             GamepadButton.NumButtons,
                             KKeyCode.O,
                             Modifier.None,
                             Action.BuildMenuKeyO,
                             false,
                             true),
            new BindingEntry("BuildingsMenu",
                             GamepadButton.NumButtons,
                             KKeyCode.P,
                             Modifier.None,
                             Action.BuildMenuKeyP,
                             false,
                             true),
            new BindingEntry("BuildingsMenu",
                             GamepadButton.NumButtons,
                             KKeyCode.Q,
                             Modifier.None,
                             Action.BuildMenuKeyQ,
                             false,
                             true),
            new BindingEntry("BuildingsMenu",
                             GamepadButton.NumButtons,
                             KKeyCode.R,
                             Modifier.None,
                             Action.BuildMenuKeyR,
                             false,
                             true),
            new BindingEntry("BuildingsMenu",
                             GamepadButton.NumButtons,
                             KKeyCode.S,
                             Modifier.None,
                             Action.BuildMenuKeyS,
                             false,
                             true),
            new BindingEntry("BuildingsMenu",
                             GamepadButton.NumButtons,
                             KKeyCode.T,
                             Modifier.None,
                             Action.BuildMenuKeyT,
                             false,
                             true),
            new BindingEntry("BuildingsMenu",
                             GamepadButton.NumButtons,
                             KKeyCode.U,
                             Modifier.None,
                             Action.BuildMenuKeyU,
                             false,
                             true),
            new BindingEntry("BuildingsMenu",
                             GamepadButton.NumButtons,
                             KKeyCode.V,
                             Modifier.None,
                             Action.BuildMenuKeyV,
                             false,
                             true),
            new BindingEntry("BuildingsMenu",
                             GamepadButton.NumButtons,
                             KKeyCode.W,
                             Modifier.None,
                             Action.BuildMenuKeyW,
                             false,
                             true),
            new BindingEntry("BuildingsMenu",
                             GamepadButton.NumButtons,
                             KKeyCode.X,
                             Modifier.None,
                             Action.BuildMenuKeyX,
                             false,
                             true),
            new BindingEntry("BuildingsMenu",
                             GamepadButton.NumButtons,
                             KKeyCode.Y,
                             Modifier.None,
                             Action.BuildMenuKeyY,
                             false,
                             true),
            new BindingEntry("BuildingsMenu",
                             GamepadButton.NumButtons,
                             KKeyCode.Z,
                             Modifier.None,
                             Action.BuildMenuKeyZ,
                             false,
                             true),
            new BindingEntry("Sandbox", GamepadButton.NumButtons, KKeyCode.B, Modifier.Shift, Action.SandboxBrush),
            new BindingEntry("Sandbox", GamepadButton.NumButtons, KKeyCode.N, Modifier.Shift, Action.SandboxSprinkle),
            new BindingEntry("Sandbox", GamepadButton.NumButtons, KKeyCode.F, Modifier.Shift, Action.SandboxFlood),
            new BindingEntry("Sandbox", GamepadButton.NumButtons, KKeyCode.K, Modifier.Shift, Action.SandboxSample),
            new BindingEntry("Sandbox", GamepadButton.NumButtons, KKeyCode.H, Modifier.Shift, Action.SandboxHeatGun),
            new BindingEntry("Sandbox", GamepadButton.NumButtons, KKeyCode.J, Modifier.Shift, Action.SandboxStressTool),
            new BindingEntry("Sandbox", GamepadButton.NumButtons, KKeyCode.C, Modifier.Shift, Action.SandboxClearFloor),
            new BindingEntry("Sandbox", GamepadButton.NumButtons, KKeyCode.X, Modifier.Shift, Action.SandboxDestroy),
            new BindingEntry("Sandbox",
                             GamepadButton.NumButtons,
                             KKeyCode.E,
                             Modifier.Shift,
                             Action.SandboxSpawnEntity),
            new BindingEntry("Sandbox",
                             GamepadButton.NumButtons,
                             KKeyCode.S,
                             Modifier.Shift,
                             Action.ToggleSandboxTools),
            new BindingEntry("Sandbox", GamepadButton.NumButtons, KKeyCode.R, Modifier.Shift, Action.SandboxReveal),
            new BindingEntry("Sandbox",
                             GamepadButton.NumButtons,
                             KKeyCode.Z,
                             Modifier.Shift,
                             Action.SandboxCritterTool),
            new BindingEntry("Sandbox",
                             GamepadButton.NumButtons,
                             KKeyCode.T,
                             Modifier.Shift,
                             Action.SandboxStoryTraitTool),
            new BindingEntry("Sandbox",
                             GamepadButton.NumButtons,
                             KKeyCode.Mouse0,
                             Modifier.Ctrl,
                             Action.SandboxCopyElement),
            new BindingEntry("SwitchActiveWorld",
                             GamepadButton.NumButtons,
                             KKeyCode.Alpha1,
                             Modifier.Backtick,
                             Action.SwitchActiveWorld1,
                             true,
                             false,
                             DlcManager.AVAILABLE_EXPANSION1_ONLY),
            new BindingEntry("SwitchActiveWorld",
                             GamepadButton.NumButtons,
                             KKeyCode.Alpha2,
                             Modifier.Backtick,
                             Action.SwitchActiveWorld2,
                             true,
                             false,
                             DlcManager.AVAILABLE_EXPANSION1_ONLY),
            new BindingEntry("SwitchActiveWorld",
                             GamepadButton.NumButtons,
                             KKeyCode.Alpha3,
                             Modifier.Backtick,
                             Action.SwitchActiveWorld3,
                             true,
                             false,
                             DlcManager.AVAILABLE_EXPANSION1_ONLY),
            new BindingEntry("SwitchActiveWorld",
                             GamepadButton.NumButtons,
                             KKeyCode.Alpha4,
                             Modifier.Backtick,
                             Action.SwitchActiveWorld4,
                             true,
                             false,
                             DlcManager.AVAILABLE_EXPANSION1_ONLY),
            new BindingEntry("SwitchActiveWorld",
                             GamepadButton.NumButtons,
                             KKeyCode.Alpha5,
                             Modifier.Backtick,
                             Action.SwitchActiveWorld5,
                             true,
                             false,
                             DlcManager.AVAILABLE_EXPANSION1_ONLY),
            new BindingEntry("SwitchActiveWorld",
                             GamepadButton.NumButtons,
                             KKeyCode.Alpha6,
                             Modifier.Backtick,
                             Action.SwitchActiveWorld6,
                             true,
                             false,
                             DlcManager.AVAILABLE_EXPANSION1_ONLY),
            new BindingEntry("SwitchActiveWorld",
                             GamepadButton.NumButtons,
                             KKeyCode.Alpha7,
                             Modifier.Backtick,
                             Action.SwitchActiveWorld7,
                             true,
                             false,
                             DlcManager.AVAILABLE_EXPANSION1_ONLY),
            new BindingEntry("SwitchActiveWorld",
                             GamepadButton.NumButtons,
                             KKeyCode.Alpha8,
                             Modifier.Backtick,
                             Action.SwitchActiveWorld8,
                             true,
                             false,
                             DlcManager.AVAILABLE_EXPANSION1_ONLY),
            new BindingEntry("SwitchActiveWorld",
                             GamepadButton.NumButtons,
                             KKeyCode.Alpha9,
                             Modifier.Backtick,
                             Action.SwitchActiveWorld9,
                             true,
                             false,
                             DlcManager.AVAILABLE_EXPANSION1_ONLY),
            new BindingEntry("SwitchActiveWorld",
                             GamepadButton.NumButtons,
                             KKeyCode.Alpha0,
                             Modifier.Backtick,
                             Action.SwitchActiveWorld10,
                             true,
                             false,
                             DlcManager.AVAILABLE_EXPANSION1_ONLY)
        };

        var list2 = (IList<BuildMenu.DisplayInfo>)BuildMenu.OrderedBuildings.data;
        if (BuildMenu.UseHotkeyBuildMenu() && hotKeyBuildMenuPermitted)
            foreach (var display_info in list2)
                AddBindings(HashedString.Invalid, display_info, list);

        return list.ToArray();
    }

    private static void AddBindings(HashedString          parent_category,
                                    BuildMenu.DisplayInfo display_info,
                                    List<BindingEntry>    bindings) {
        if (display_info.data != null) {
            var type = display_info.data.GetType();
            if (typeof(IList<BuildMenu.DisplayInfo>).IsAssignableFrom(type))
                using (var enumerator = ((IList<BuildMenu.DisplayInfo>)display_info.data).GetEnumerator()) {
                    while (enumerator.MoveNext()) {
                        var display_info2 = enumerator.Current;
                        AddBindings(display_info.category, display_info2, bindings);
                    }

                    return;
                }

            if (typeof(IList<BuildMenu.BuildingInfo>).IsAssignableFrom(type)) {
                var str   = HashCache.Get().Get(parent_category);
                var group = new CultureInfo("en-US", false).TextInfo.ToTitleCase(str) + " Menu";
                var item = new BindingEntry(group,
                                            GamepadButton.NumButtons,
                                            display_info.keyCode,
                                            Modifier.None,
                                            display_info.hotkey,
                                            true,
                                            true);

                bindings.Add(item);
            }
        }
    }

    private void Awake() {
        var crash_reporter = GetComponent<KCrashReporter>();
        if ((crash_reporter != null) & (SceneInitializerLoader.ReportDeferredError == null))
            SceneInitializerLoader.ReportDeferredError = delegate(SceneInitializerLoader.DeferredError deferred_error) {
                                                             crash_reporter.ShowDialog(deferred_error.msg,
                                                              deferred_error.stack_trace);
                                                         };

        globalCanvas = GameObject.Find("Canvas");
        DontDestroyOnLoad(globalCanvas.gameObject);
        OutputSystemInfo();
        Debug.Assert(Instance == null);
        Instance = this;
        Debug.Log("Initializing at " + System.DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff"));
        Debug.Log("Save path: "      + Util.RootFolder());
        MyCmp.Init();
        MySmi.Init();
        DevToolManager.Instance.Init();
        if (forcedAtlasInitializationList != null)
            foreach (var spriteAtlas in forcedAtlasInitializationList) {
                var array2 = new Sprite[spriteAtlas.spriteCount];
                spriteAtlas.GetSprites(array2);
                var array3 = array2;
                for (var j = 0; j < array3.Length; j++) {
                    var texture = array3[j].texture;
                    if (texture != null) {
                        texture.filterMode = FilterMode.Bilinear;
                        texture.anisoLevel = 4;
                        texture.mipMapBias = 0f;
                    }
                }
            }

        FileSystem.Initialize();
        Singleton<StateMachineUpdater>.CreateInstance();
        Singleton<StateMachineManager>.CreateInstance();
        Localization.RegisterForTranslation(typeof(UI));
        modManager = new Manager();
        modManager.LoadModDBAndInitialize();
        modManager.Load(Content.DLL);
        modManager.Load(Content.Strings);
        KSerialization.Manager.Initialize();
        InitializeGlobalInput();
        InitializeGlobalSound();
        InitializeGlobalAnimation();
        Localization.Initialize();
        modManager.Load(Content.Translation);
        modManager.distribution_platforms.Add(new Local("Local", Label.DistributionPlatform.Local, false));
        modManager.distribution_platforms.Add(new Local("Dev",   Label.DistributionPlatform.Dev,   true));
        KProfiler.main_thread = Thread.CurrentThread;
        RestoreLegacyMetricsSetting();
        TestDataLocations();
        DistributionPlatform.onExitRequest             += OnExitRequest;
        DistributionPlatform.onDlcAuthenticationFailed += OnDlcAuthenticationFailed;
        if (DistributionPlatform.Initialized) {
            if (!KPrivacyPrefs.instance.disableDataCollection) {
                var array4 = new string[6];
                array4[0] = "Logged into ";
                array4[1] = DistributionPlatform.Inst.Name;
                array4[2] = " with ID:";
                var num = 3;
                var id  = DistributionPlatform.Inst.LocalUser.Id;
                array4[num] = id != null ? id.ToString() : null;
                array4[4]   = ", NAME:";
                array4[5]   = DistributionPlatform.Inst.LocalUser.Name;
                Debug.Log(string.Concat(array4));
                ThreadedHttps<KleiAccount>.Instance.AuthenticateUser(OnGetUserIdKey);
            } else
                Debug.Log("Data collection disabled, account will not be used.");
        } else {
            Debug.LogWarning("Can't init " + DistributionPlatform.Inst.Name + " distribution platform...");
            OnGetUserIdKey();
        }

        ThreadedHttps<KleiItems>.Instance.LoadInventoryCache();
        modManager.Load(Content.LayerableFiles);
        WorldGen.LoadSettings(true);
        StartCoroutine(WorldGen.ListenForLoadSettingsErrorRoutine());
        GlobalResources.Instance();
    }

    private static void InitializeGlobalInput() {
        if (Game.IsQuitting()) return;

        mInputManager = new GameInputManager(GenerateDefaultBindings());
    }

    private static void InitializeGlobalSound() {
        Audio.Get();
        Singleton<SoundEventVolumeCache>.CreateInstance();
    }

    private static void InitializeGlobalAnimation() {
        KAnimBatchManager.CreateInstance();
        Singleton<AnimEventManager>.CreateInstance();
        Singleton<KBatchedAnimUpdater>.CreateInstance();
    }

    private void OnExitRequest() {
        var flag = true;
        if (Game.Instance != null) {
            var filename = SaveLoader.GetActiveSaveFilePath();
            if (!string.IsNullOrEmpty(filename) && File.Exists(filename)) {
                flag = false;
                var component = KScreenManager
                                .AddChild(globalCanvas, ScreenPrefabs.Instance.ConfirmDialogScreen.gameObject)
                                .GetComponent<KScreen>();

                component.Activate();
                component.GetComponent<ConfirmDialogScreen>()
                         .PopupConfirmDialog(string.Format(UI.FRONTEND.RAILFORCEQUIT.SAVE_EXIT,
                                                           Path.GetFileNameWithoutExtension(filename)),
                                             delegate {
                                                 SaveLoader.Instance.Save(filename);
                                                 App.Quit();
                                             },
                                             delegate { App.Quit(); });
            }
        }

        if (flag) {
            var component2 = KScreenManager
                             .AddChild(globalCanvas, ScreenPrefabs.Instance.ConfirmDialogScreen.gameObject)
                             .GetComponent<KScreen>();

            component2.Activate();
            component2.GetComponent<ConfirmDialogScreen>()
                      .PopupConfirmDialog(UI.FRONTEND.RAILFORCEQUIT.WARN_EXIT, delegate { App.Quit(); }, null);
        }
    }

    private void OnDlcAuthenticationFailed() {
        if (DlcManager.IsExpansion1Active()) {
            var component = KScreenManager.AddChild(globalCanvas, ScreenPrefabs.Instance.ConfirmDialogScreen.gameObject)
                                          .GetComponent<KScreen>();

            component.Activate();
            var component2 = component.GetComponent<ConfirmDialogScreen>();
            component2.deactivateOnCancelAction = false;
            component2.PopupConfirmDialog(UI.FRONTEND.RAILFORCEQUIT.DLC_NOT_PURCHASED, delegate { App.Quit(); }, null);
        }
    }

    private void RestoreLegacyMetricsSetting() {
        if (KPlayerPrefs.GetInt("ENABLE_METRICS", 1) == 0) {
            KPlayerPrefs.DeleteKey("ENABLE_METRICS");
            KPlayerPrefs.Save();
            KPrivacyPrefs.instance.disableDataCollection = true;
            KPrivacyPrefs.Save();
        }
    }

    private void TestDataLocations() {
        if (Application.platform == RuntimePlatform.WindowsPlayer ||
            Application.platform == RuntimePlatform.WindowsEditor)
            try {
                var text  = Util.RootFolder();
                var text2 = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                text2 = Path.Combine(text2, "Klei");
                text2 = Path.Combine(text2, Util.GetTitleFolderName());
                Debug.Log("Test Data Location / docs / "  + text);
                Debug.Log("Test Data Location / local / " + text2);
                if (!Directory.Exists(text2)) Directory.CreateDirectory(text2);
                if (!Directory.Exists(text)) Directory.CreateDirectory(text);
                var      text3  = Path.Combine(text,  "test");
                var      text4  = Path.Combine(text2, "test");
                string[] array  = { text3, text4 };
                var      array2 = new bool[2];
                var      array3 = new bool[2];
                var      array4 = new bool[2];
                for (var i = 0; i < array.Length; i++) {
                    try {
                        using (var fileStream
                               = File.Open(array[i], FileMode.Create, FileAccess.Write, FileShare.ReadWrite)) {
                            var bytes = Encoding.UTF8.GetBytes("test");
                            fileStream.Write(bytes, 0, bytes.Length);
                            array2[i] = true;
                        }
                    } catch (Exception ex) {
                        array2[i] = false;
                        DebugUtil.LogWarningArgs("Test Data Locations / failed to write " +
                                                 array[i]                                 +
                                                 ": "                                     +
                                                 ex.Message);
                    }

                    try {
                        using (var fileStream2 = File.Open(array[i], FileMode.Open, FileAccess.Read)) {
                            var utf    = Encoding.UTF8;
                            var array5 = new byte[fileStream2.Length];
                            if (fileStream2.Read(array5, 0, array5.Length) == fileStream2.Length) {
                                var @string = utf.GetString(array5);
                                if (@string == "test")
                                    array3[i] = true;
                                else {
                                    array3[i] = false;
                                    DebugUtil
                                        .LogWarningArgs(string
                                                            .Concat("Test Data Locations / failed to validate contents ",
                                                                    array[i],
                                                                    ", got: `",
                                                                    @string,
                                                                    "`"));
                                }
                            }
                        }
                    } catch (Exception ex2) {
                        array3[i] = false;
                        DebugUtil.LogWarningArgs("Test Data Locations / failed to read " +
                                                 array[i]                                +
                                                 ": "                                    +
                                                 ex2.Message);
                    }

                    try {
                        File.Delete(array[i]);
                        array4[i] = true;
                    } catch (Exception ex3) {
                        array4[i] = false;
                        DebugUtil.LogWarningArgs("Test Data Locations / failed to remove " +
                                                 array[i]                                  +
                                                 ": "                                      +
                                                 ex3.Message);
                    }
                }

                for (var j = 0; j < array.Length; j++)
                    Debug.Log(string.Concat("Test Data Locations / ",
                                            array[j],
                                            " / write ",
                                            array2[j].ToString(),
                                            " / read ",
                                            array3[j].ToString(),
                                            " / removed ",
                                            array4[j].ToString()));

                var flag  = array2[0] && array3[0];
                var flag2 = array2[1] && array3[1];
                if (flag && flag2)
                    saveFolderTestResult = "both";
                else if (flag && !flag2)
                    saveFolderTestResult = "docs_only";
                else if (!flag && flag2)
                    saveFolderTestResult = "local_only";
                else
                    saveFolderTestResult = "neither";
            } catch (Exception ex4) {
                KCrashReporter.Assert(false,
                                      "Test Data Locations / failed: " + ex4.Message,
                                      new[] { KCrashReporter.CRASH_CATEGORY.FILEIO });
            }
    }

    public static GameInputManager GetInputManager() {
        if (mInputManager == null) InitializeGlobalInput();
        return mInputManager;
    }

    private void OnApplicationFocus(bool focus) {
        if (mInputManager != null) mInputManager.OnApplicationFocus(focus);
    }

    private void OnGetUserIdKey() { gotKleiUserID = true; }

    private void Update() {
        var instance = ImGuiRenderer.GetInstance();
        if (instance) {
            DevTools.UpdateShouldShowTools();
            instance.gameObject.transform.parent.gameObject.SetActive(DevTools.Show);
            if (DevTools.Show) instance.NewFrame();
            DevTools.UpdateTools();
        }

        mInputManager.Update();
        if (Singleton<AnimEventManager>.Instance != null) Singleton<AnimEventManager>.Instance.Update();
        if (DistributionPlatform.Initialized && !updated_with_initialized_distribution_platform) {
            updated_with_initialized_distribution_platform = true;
            SteamUGCService.Initialize();
            var steam = new Steam();
            SteamUGCService.Instance.AddClient(steam);
            modManager.distribution_platforms.Add(steam);
            SteamAchievementService.Initialize();
        }

        if (gotKleiUserID) {
            gotKleiUserID = false;
            ThreadedHttps<KleiMetrics>.Instance.SetCallBacks(SetONIStaticSessionVariables,
                                                             SetONIDynamicSessionVariables);

            ThreadedHttps<KleiMetrics>.Instance.StartSession();
            KleiItems.AddRequestInventoryRefresh();
            KleiItems.AddRequestGetPricingInfo();
        }

        ThreadedHttps<KleiMetrics>.Instance.SetLastUserAction(KInputManager.lastUserActionTicks);
        Localization.VerifyTranslationModSubscription(globalCanvas);
        if (DistributionPlatform.Initialized) ThreadedHttps<KleiItems>.Instance.Update();
    }

    private void SetONIStaticSessionVariables() {
        ThreadedHttps<KleiMetrics>.Instance.SetStaticSessionVariable("Branch",              "release");
        ThreadedHttps<KleiMetrics>.Instance.SetStaticSessionVariable("Build",               642695U);
        ThreadedHttps<KleiMetrics>.Instance.SetStaticSessionVariable("SaveFolderWriteTest", saveFolderTestResult);
        if (KPlayerPrefs.HasKey(UnitConfigurationScreen.MassUnitKey))
            ThreadedHttps<KleiMetrics>.Instance.SetStaticSessionVariable(UnitConfigurationScreen.MassUnitKey,
                                                                         ((GameUtil.MassUnit)
                                                                             KPlayerPrefs.GetInt(UnitConfigurationScreen
                                                                                 .MassUnitKey)).ToString());

        if (KPlayerPrefs.HasKey(UnitConfigurationScreen.TemperatureUnitKey))
            ThreadedHttps<KleiMetrics>.Instance.SetStaticSessionVariable(UnitConfigurationScreen.TemperatureUnitKey,
                                                                         ((GameUtil.TemperatureUnit)
                                                                             KPlayerPrefs.GetInt(UnitConfigurationScreen
                                                                                 .TemperatureUnitKey)).ToString());

        var selectedLanguageType = (int)Localization.GetSelectedLanguageType();
        ThreadedHttps<KleiMetrics>.Instance.SetStaticSessionVariable(LanguageCodeKey,
                                                                     Localization.GetCurrentLanguageCode());

        if (selectedLanguageType == 2)
            ThreadedHttps<KleiMetrics>.Instance.SetStaticSessionVariable(LanguageModKey,
                                                                         LanguageOptionsScreen.GetSavedLanguageMod());
    }

    private void SetONIDynamicSessionVariables(Dictionary<string, object> data) {
        if (Game.Instance != null && GameClock.Instance != null) {
            data.Add("GameTimeSeconds",  (uint)GameClock.Instance.GetTime());
            data.Add("WasDebugEverUsed", Game.Instance.debugWasUsed);
            data.Add("IsSandboxEnabled", SaveGame.Instance.sandboxEnabled);
        }
    }

    private void LateUpdate() {
        StreamedTextures.UpdateRequests();
        Singleton<KBatchedAnimUpdater>.Instance.LateUpdate();
        if (DevTools.Show) {
            var instance = ImGuiRenderer.GetInstance();
            if (instance == null) return;

            instance.EndFrame();
        }
    }

    private void OnDestroy() {
        if (modManager != null) modManager.Shutdown();
        Instance = null;
        if (Singleton<AnimEventManager>.Instance != null) Singleton<AnimEventManager>.Instance.FreeResources();
        Singleton<KBatchedAnimUpdater>.DestroyInstance();
    }

    private void OnApplicationQuit() {
        KGlobalAnimParser.DestroyInstance();
        ThreadedHttps<KleiMetrics>.Instance.EndSession();
    }

    private void OutputSystemInfo() {
        try {
            Console.WriteLine("SYSTEM INFO:");
            foreach (var keyValuePair in KleiMetrics.GetHardwareStats())
                try { Console.WriteLine("    {0}={1}", keyValuePair.Key, keyValuePair.Value); } catch { }

            Console.WriteLine("    {0}={1}", "System Language", Application.systemLanguage.ToString());
        } catch { }
    }
}