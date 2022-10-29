using System;

namespace Tournament.Football.Impls.Internal;

internal class OnStartedInvoker<TStageResult>
{

    private object _startedEventLocker = new object();
    private bool _isStarted;

    public OnStartedInvoker(FootballStageBase<TStageResult> stage, Action invoke)
    {
        foreach (var game in stage.Schedule)
        {
            game.OnResultSet += g =>
           {
               lock (_startedEventLocker)
               {
                   if (!_isStarted)
                   {
                       _isStarted = true;
                       invoke();
                   }
               }
           };
        }
    }

}
