namespace Chip8.Core
{
    public class GraphicsUpdatedEventArgs : EventArgs
    {
        #region Public Constructors

        public GraphicsUpdatedEventArgs(Bit[,] data) => Data = data;

        #endregion Public Constructors

        #region Public Properties

        public Bit[,] Data { get; init; }

        #endregion Public Properties
    }
}