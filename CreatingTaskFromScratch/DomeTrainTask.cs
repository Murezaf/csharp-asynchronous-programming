namespace CreatingTaskFromScratch;

public class DomeTrainTask
{
    private readonly Lock _lock = new Lock();
    private bool _isComplete;
    private Exception? _exception; 

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

    public void SetResult()
    {
        lock (_lock)
        {
            if (_isComplete)
                throw new InvalidOperationException("Task is already done. Can not set result of a completed dome task.");

            _isComplete = true;
        }
    }

    public void SetException(Exception exception)
    {
        lock (_lock)
        {
            if (_isComplete)
                throw new InvalidOperationException("Task is already done. Can not set an exception for a completed dome task.");
        }

        _exception = exception;
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
}
