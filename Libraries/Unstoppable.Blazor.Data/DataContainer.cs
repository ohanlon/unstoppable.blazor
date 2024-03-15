using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components;

namespace Unstoppable.Blazor;

public class DataContainer : ComponentBase
{
  [Parameter] public bool HasData { get; set; }

  [Parameter] public RenderFragment Template { get; set; } = null!;

  [Parameter] public RenderFragment NoData { get; set; } = null!;

  protected override void BuildRenderTree(RenderTreeBuilder builder)
  {
    base.BuildRenderTree(builder);
    builder.AddContent(0, HasData ? Template : NoData);
  }
}