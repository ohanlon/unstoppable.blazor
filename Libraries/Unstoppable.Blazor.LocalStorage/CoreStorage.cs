using System.Runtime.ExceptionServices;
using System.Text.Json;

namespace Unstoppable.Blazor;

public abstract class CoreStorage
{
  protected T? ConvertGetItemResult<T>(string? value)
  {
    ExceptionDispatchInfo? exceptionDispatchInfo = null;
    Type type = typeof(T);
    T? result = default;
    try
    {
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
}