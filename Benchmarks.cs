using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Environments;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class Program
{
	static void Main(string[] args)
	{
		var jobs =
			from runtime in new[] { /*CoreRuntime.Core70,*/ CoreRuntime.Core80 }
			select Job.Default.WithRuntime(runtime);

		BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args, ManualConfig.CreateMinimumViable()
			.HideColumns("Error", "StdDev", "Median", "RatioSD", "x", "y", "c")
			.AddDiagnoser(new DisassemblyDiagnoser(new DisassemblyDiagnoserConfig(
				printSource: true,
				exportHtml: true,
				exportCombinedDisassemblyReport: true,
				exportDiff: true)))
			.AddJob(jobs.ToArray()));
	}

	[Benchmark(Baseline = true), Arguments(true)]
	public bool Before(bool? b) => b == true;

	[Benchmark, Arguments(true)]
	public bool After(bool? b) => b.GetValueOrDefault();
}
