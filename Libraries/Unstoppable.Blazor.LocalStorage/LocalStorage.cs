using System.Diagnostics.CodeAnalysis;
using System.Runtime.ExceptionServices;
using System.Text.Json;
using Microsoft.JSInterop;

namespace Unstoppable.Blazor;

public class LocalStorage(IJSInProcessRuntime runtime) : CoreStorage
{
  [RequiresUnreferencedCode("localStorage.getItem")]
  public virtual T? Get<T>(string key)
  {
    ExceptionDispatchInfo? exceptionDispatchInfo = null;
    T? result = default;

    try
    {
      string? value = runtime.Invoke<string?>("localStorage.getItem", key);
      result = ConvertGetItemResult<T>(value);
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
}