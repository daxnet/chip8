// See https://aka.ms/new-console-template for more information

using Chip8.Emulator;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

if (args.Length == 0)
{
    Console.WriteLine("Usage: chip8.exe <rom file>");
    return -1;
}

var gameWindowSettings = new GameWindowSettings
{
    UpdateFrequency = 600
};

var nativeWindowSettings = new NativeWindowSettings
{
    Size = new Vector2i(960, 480),
    Profile = ContextProfile.Compatability,
    Title = "CHIP-8 Emulator"
};

new EmulatorWindow(gameWindowSettings, nativeWindowSettings, args[0]).Run();

return 0;
