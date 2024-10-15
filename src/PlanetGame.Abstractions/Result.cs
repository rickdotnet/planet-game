namespace PlanetGame.Abstractions;

public static class Result
{
    public static Result<T> Success<T>(T value) => new Result<T>.Success(value);
    public static Result<T> Failure<T>(Exception error) => new Result<T>.Fail(error);
    
    public static Result<T> Try<T>(Func<T> func)
    {
        try
        {
            return Success(func());
        }
        catch (Exception e)
        {
            return Failure<T>(e);
        }
    }
    
    public static async ValueTask<Result<T>> Try<T>(Func<ValueTask<T>> func)
    {
        try
        {
            return Success(await func());
        }
        catch (Exception e)
        {
            return Failure<T>(e);
        }
    }
}

public abstract record Result<T>
{
    public sealed record Success(T Value) : Result<T>{}

    public sealed record Fail(Exception Error) : Result<T>;

    public static implicit operator bool(Result<T> result)
        => result is Success;
}

public static class ResultExtensions
{
    public static Result<TResult> Select<T, TResult>(this Result<T> result, Func<T, TResult> transform)
    {
        return result switch
        {
            Result<T>.Success success => Result.Success(transform(success.Value)),
            Result<T>.Fail fail => Result.Failure<TResult>(fail.Error),
            _ => throw new InvalidOperationException("Unknown StoreResult type.")
        };
    }

    public static async ValueTask<Result<TResult>> Select<T, TResult>(this ValueTask<Result<T>> task, Func<T, Task<TResult>> transform)
    {
        var result = await task;

        return result switch
        {
            Result<T>.Success success => Result.Success(await transform(success.Value)),
            Result<T>.Fail fail => Result.Failure<TResult>(fail.Error),
            _ => throw new InvalidOperationException("Unknown StoreResult type.")
        };
    }

    public static async ValueTask<Result<TResult>> Select<T, TResult>(this Result<T> result, Func<T, Task<TResult>> transform)
    {
        return result switch
        {
            Result<T>.Success success => Result.Success(await transform(success.Value)),
            Result<T>.Fail fail => Result.Failure<TResult>(fail.Error),
            _ => throw new InvalidOperationException("Unknown StoreResult type.")
        };
    }

    public static void OnSuccess<T>(this Result<T> result, Action<T> onSuccess)
    {
        if (result is Result<T>.Success success)
            onSuccess(success.Value);
    }

    public static async Task OnSuccess<T>(this Result<T> result, Func<T, Task> onSuccess)
    {
        if (result is Result<T>.Success success)
            await onSuccess(success.Value);
    }

    public static async Task OnSuccess<T>(this ValueTask<Result<T>> task, Func<T, Task> onSuccess)
    {
        var result = await task;
        if (result is Result<T>.Success success)
            await onSuccess(success.Value);
    }
    
    public static async Task OnSuccess<T>(this ValueTask<Result<T>> task, Action<T> onSuccess)
    {
        var result = await task;
        if (result is Result<T>.Success success)
            onSuccess(success.Value);
    }

    public static void OnFailure<T>(this Result<T> result, Action<Exception> onFailure)
    {
        if (result is Result<T>.Fail fail)
            onFailure(fail.Error);
    }


    public static async Task OnFailure<T>(this Result<T> result, Func<Exception, Task> onFailure)
    {
        if (result is Result<T>.Fail fail)
            await onFailure(fail.Error);
    }

    public static void Resolve<T>(this Result<T> result, Action<T> onSuccess, Action<Exception> onFailure)
    {
        switch (result)
        {
            case Result<T>.Success success:
                onSuccess(success.Value);
                break;
            case Result<T>.Fail fail:
                onFailure(fail.Error);
                break;
        }
    }

    public static async Task Resolve<T>(this Result<T> result, Func<T, Task> onSuccess, Func<Exception, Task> onFailure)
    {
        switch (result)
        {
            case Result<T>.Success success:
                await onSuccess(success.Value);
                break;
            case Result<T>.Fail fail:
                await onFailure(fail.Error);
                break;
        }
    }
}