using System;

namespace Common
{
    public interface IAppExit
    {
        Action Exit { get; }
    }

    public class AppExit : IAppExit
    {
        public Action Exit { get; }

        public AppExit(Action Exit)
        {
            this.Exit = Exit;
        }
    }
}
