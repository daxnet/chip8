// See https://aka.ms/new-console-template for more information
using Chip8.Core;

var machine = new Machine();
machine.Initialize();
machine.GraphicsUpdated += Machine_GraphicsUpdated;

void Machine_GraphicsUpdated(object? sender, GraphicsUpdatedEventArgs e)
{
    Console.WriteLine("Graphics");
}

machine.LoadRom("IBM Logo.ch8");
machine.EmulateCycle();
Console.WriteLine("OK");