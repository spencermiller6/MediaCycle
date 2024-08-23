using System;

namespace MediaCycle.Sources;

public interface ISource
{
    public void Connect();
    public void Disconnect();
    public void Sync();
}
