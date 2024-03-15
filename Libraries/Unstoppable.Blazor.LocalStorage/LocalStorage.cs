﻿using System.Runtime.ExceptionServices;
using System.Text.Json;
using Microsoft.JSInterop;

namespace Unstoppable.Blazor;

public class LocalStorage(IJSRuntime runtime)
{
  public virtual async ValueTask<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default)
  {
    ExceptionDispatchInfo? exceptionDispatchInfo = null;
    Type type = typeof(T);
    T? result = default;
    try
    {
      string? value = await runtime.InvokeAsync<string>("localStorage.getItem", cancellationToken, key);

      if (type.IsPrimitive || type == typeof(string))
      {
        if (value is not null)
        {
          result = (T)Convert.ChangeType(value, type);
        }
      }
      else if (type == typeof(DateTime))
      {
        System.Diagnostics.Debug.WriteLine(value);
        if (value is not null)
        {
          if (DateTime.TryParse(value.Replace("\"", string.Empty), out DateTime dt))
          {
            result = (T)Convert.ChangeType(dt, type);
          }
        }
      }
      else
      {
        result = value is null ? default : JsonSerializer.Deserialize<T>(value);
      }
    }
    catch (Exception e)
    {
      exceptionDispatchInfo = ExceptionDispatchInfo.Capture(e);
    }
    finally
    {
      exceptionDispatchInfo?.Throw();
    }

    return result;
  }

  public virtual async Task SetAsync<T>(string key, T value, CancellationToken cancellationToken = default)
  {
    if (value is null)
    {
      await RemoveAsync(key, cancellationToken);
      return;
    }

    ExceptionDispatchInfo? exceptionDispatchInfo = null;
    try
    {
      Type type = typeof(T);
      string? writeableItem;
      if (type.IsPrimitive || type == typeof(string))
      {
        writeableItem = value.ToString();
      }
      else if (type == typeof(DateTime))
      {
        writeableItem = value.ToString();
      }
      else
      {
        writeableItem = JsonSerializer.Serialize(value);
      }

      await runtime.InvokeVoidAsync("localStorage.setItem", cancellationToken, key, writeableItem);
    }
    catch (Exception e)
    {
      exceptionDispatchInfo = ExceptionDispatchInfo.Capture(e);
    }
    finally
    {
      exceptionDispatchInfo?.Throw();
    }
  }

  public virtual async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
  {
    ExceptionDispatchInfo? exceptionDispatchInfo = null;
    try
    {
      await runtime.InvokeVoidAsync("localStorage.removeItem", cancellationToken, key);
    }
    catch (Exception e)
    {
      exceptionDispatchInfo = ExceptionDispatchInfo.Capture(e);
    }
    finally
    {
      exceptionDispatchInfo?.Throw();
    }
  }

  public async Task ClearStoreAsync(CancellationToken? cancellationToken = null)
  {
    ExceptionDispatchInfo? exceptionDispatchInfo = null;
    try
    {
      await runtime.InvokeVoidAsync("localStorage.clear", cancellationToken);
    }
    catch (Exception e)
    {
      exceptionDispatchInfo = ExceptionDispatchInfo.Capture(e);
    }
    finally
    {
      exceptionDispatchInfo?.Throw();
    }
  }
}