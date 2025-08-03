namespace MinPlatform.Schema.Migrators.Runner
{
    using System;

    public interface IStopWatch
    {
        void Start();
        void Stop();
        TimeSpan ElapsedTime();
        TimeSpan Time(Action action);
    }
}
