namespace Yag2048.Core.Infrastructure;

public delegate Task PipelineDelegate<in T>(T context);