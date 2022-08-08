// See https://aka.ms/new-console-template for more information
using Chip8.Core;

var machine = new Machine();
machine.Initialize();
machine.LoadRom("IBM Logo.ch8");
machine.Run();