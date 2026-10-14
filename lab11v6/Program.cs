using System;
using System.Collections.Generic;

abstract class TestRunner
{
    public string Status { get; protected set; } = "Not started";
    public string Results { get; protected set; } = "";

    public void Run()
    {
        Status = "Running";

        try
        {
            SetUp();
            ExecuteTests();
            CollectResults();
            Status = "Completed";
        }
        catch (Exception ex)
        {
            Status = "Failed";
            Results = ex.Message;
            Console.WriteLine($"Error: {ex.Message}");
        }
        finally
        {
            TearDown();
            GenerateReport();
        }
    }

    protected abstract void SetUp();
    protected abstract void ExecuteTests();
    protected abstract void CollectResults();

    protected virtual void TearDown()
    {
        Console.WriteLine("Cleaning up");
    }

    protected virtual void GenerateReport()
    {
        Console.WriteLine($"Status: {Status}");
        Console.WriteLine($"Results: {Results}");
        Console.WriteLine();
    }
}

sealed class UnitTestRunner : TestRunner
{
    public int TestedMethods { get; set; } = 5;
    protected override void SetUp()
    {
        Console.WriteLine("Preparing unit tests");
    }
    protected override void ExecuteTests()
    {
        Console.WriteLine($"Testing {TestedMethods} methods");
    }

    protected override void CollectResults()
    {
        Results = $"Checked {TestedMethods} methods";
        Console.WriteLine(Results);
    }
}

sealed class IntegrationTestRunner : TestRunner
{
    public int IntegratedComponents { get; set; } = 3;

    protected override void SetUp()
    {
        Console.WriteLine("Preparing integration tests");
    }

    protected override void ExecuteTests()
    {
        Console.WriteLine(
            $"Testing integration of {IntegratedComponents} components");
    }

    protected override void CollectResults()
    {
        Results = $"Integrated {IntegratedComponents} components";
        Console.WriteLine(Results);
    }
}

sealed class E2ETestRunner : TestRunner
{
    public int UserScenarios { get; set; } = 4;

    protected override void SetUp()
    {
        Console.WriteLine("Preparing E2E tests");
    }

    protected override void ExecuteTests()
    {
        Console.WriteLine(
            $"Executing {UserScenarios} user scenarios");
    }

    protected override void CollectResults()
    {
        Results = $"Executed {UserScenarios} user scenarios";
        Console.WriteLine(Results);
    }
}

class Program
{
    static void Main()
    {
        List<TestRunner> runs = new List<TestRunner>();
        runs.Add(new UnitTestRunner());
        runs.Add(new IntegrationTestRunner());
        runs.Add(new E2ETestRunner());

        foreach (TestRunner runner in runs)
        {
            runner.Run();
        }
    }
}