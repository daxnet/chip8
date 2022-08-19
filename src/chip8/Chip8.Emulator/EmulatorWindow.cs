using Chip8.Core;
using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System.ComponentModel;
using System.Drawing;

namespace Chip8.Emulator
{
    internal class EmulatorWindow : GameWindow
    {
        #region Private Fields

        private readonly Machine _machine = new();

        #endregion Private Fields

        #region Public Constructors

        public EmulatorWindow(
            GameWindowSettings gameWindowSettings,
            NativeWindowSettings nativeWindowSettings,
            string romFile)
            : base(gameWindowSettings, nativeWindowSettings)
        {
            _machine.LoadRom(romFile);
            _machine.Initialize();
            _machine.GraphicsUpdated += Machine_GraphicsUpdated;
        }

        #endregion Public Constructors

        #region Protected Methods

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            _machine.GraphicsUpdated -= Machine_GraphicsUpdated;
        }

        protected override void OnKeyDown(KeyboardKeyEventArgs e)
        {
            base.OnKeyDown(e);
            var consoleKey = MapKeyToConsoleKey(e.Key);
            if (consoleKey != ConsoleKey.NoName)
            {
                _machine.KeyDown(consoleKey);
            }
        }

        protected override void OnKeyUp(KeyboardKeyEventArgs e)
        {
            base.OnKeyUp(e);
            var consoleKey = MapKeyToConsoleKey(e.Key);
            if (consoleKey != ConsoleKey.NoName)
            {
                _machine.KeyUp(consoleKey);
            }
        }

        protected override void OnLoad()
        {
            base.OnLoad();
            GL.ClearColor(Color.Black);
            GL.Color3(Color.White);
            GL.Ortho(0, 64, 32, 0, -1, 1);
            GL.Clear(ClearBufferMask.ColorBufferBit);
            SwapBuffers();
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0, 0, e.Width, e.Height);
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);
            _machine?.EmulateCycle();
        }

        #endregion Protected Methods

        #region Private Methods

        private static ConsoleKey MapKeyToConsoleKey(Keys key) => key switch
        {
            >= Keys.A and <= Keys.Z or >= Keys.D0 and <= Keys.D9
                => (ConsoleKey)Enum.Parse(typeof(ConsoleKey),
                    Enum.GetName(typeof(Keys), key) ?? "NoName"),
            _ => ConsoleKey.NoName
        };

        private void Machine_GraphicsUpdated(object? sender, GraphicsUpdatedEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit);

            for (var x = 0; x < 64; x++)
                for (var y = 0; y < 32; y++)
                {
                    if (e.Data[x, y])
                    {
                        GL.Rect(x, y, x + 1, y + 1);
                    }
                }

            SwapBuffers();
        }

        #endregion Private Methods
    }
}