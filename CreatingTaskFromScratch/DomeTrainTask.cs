using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;

namespace CreatingTaskFromScratch;

public class DomeTrainTask
{
    private readonly Lock _lock = new Lock();
    private bool _isComplete;
    private Exception? _exception;

    private Action? _action;
    private ExecutionContext? _context;

    public bool IsComplete
    {
        get
        {
            lock (_lock)
            {
                return _isComplete;
            }
        }
    }

    //public void SetResult()
    //{
    //    lock (_lock)
    //    {
    //        if (_isComplete)
    //            throw new InvalidOperationException("Task is already done. Can not set result of a completed dome task.");

    //        _isComplete = true;
    //    }
    //}

    //public void SetException(Exception exception)
    //{
    //    lock (_lock)
    //    {
    //        if (_isComplete)
    //            throw new InvalidOperationException("Task is already done. Can not set an exception for a completed dome task.");
    //    }

    //    _exception = exception;
    //}

    public void SetResult() => CompleteTask(null);
    public void SetException(Exception exception) => CompleteTask(exception);
    public void CompleteTask(Exception? exception)
    {
        lock(_lock)
        {
            if(_isComplete)
                throw new InvalidOperationException("Task is already done.");
        }

        _exception = exception;
        _isComplete = true;

        if(_action is not null)
        {
            if(_context is null)
            {
                _action.Invoke();
            }
            else
            {
                ExecutionContext.Run(_context, state => ((Action?)state)?.Invoke(), _action);
            }
        }
    }

    public static DomeTrainTask Run(Action action)
    {
        DomeTrainTask task = new DomeTrainTask();

        ThreadPool.QueueUserWorkItem(_ =>
        {
            try
            {
                action();
                task.SetResult();
            }
            catch (Exception e)
            {
                task.SetException(e);
            }
        });

        return task;
    }

    public DomeTrainTask ContinueWith(Action action)
    {
        DomeTrainTask task = new DomeTrainTask();

        lock(_lock)
        {
            if(_isComplete)
            {
                ThreadPool.QueueUserWorkItem(_ =>
                {
                    try
                    {
                        action();
                        task.SetResult();
                    }
                    catch (Exception e)
                    {
                        task.SetException(e);
                    }
                });
            }
            else
            {
                _action = action;
                _context = ExecutionContext.Capture();
            }
        }

        return task;
    }

    public void Wait()
    {
        ManualResetEventSlim resetEventSlim = null;

        lock (_lock)
        {
            if (!_isComplete)
            {
                resetEventSlim = new ManualResetEventSlim();
                this.ContinueWith(() => resetEventSlim.Set());

                //resetEventSlim.Wait(); //Cause deadlock
            }
        }

        resetEventSlim?.Wait();

        if (_exception is not null)
        {
            ExceptionDispatchInfo.Throw(_exception);
        }
    }

    public static DomeTrainTask Delay(TimeSpan delay)
    {
        DomeTrainTask task = new DomeTrainTask();

        new Timer(_ => task.SetResult()).Change(delay, Timeout.InfiniteTimeSpan);

        return task;
    }

    public DomeTrainTaskAwaiter GetAwaiter() => new DomeTrainTaskAwaiter(this);
}

public readonly struct DomeTrainTaskAwaiter : INotifyCompletion
{
    private readonly DomeTrainTask _task;

    internal DomeTrainTaskAwaiter(DomeTrainTask task)
    {
        _task = task;
    }

    public void OnCompleted(Action continuation)
    {
        _task.ContinueWith(continuation);
    }

    public bool IsCompleted => _task.IsComplete;
    //public DomeTrainTaskAwaiter GetAwaiter()
    //{
    //    return this;
    //}
    public DomeTrainTaskAwaiter GetAwaiter() => this;
    public void GetResult() => _task.Wait();
}
