using Microsoft.Extensions.Configuration;
using Quartz;
using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using prometheus_net = Prometheus;

public class AggregationJob : IJob
{
    private readonly string _connectionString;
    private readonly prometheus_net.Gauge _taskCountMetric;

    public AggregationJob(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
        _taskCountMetric = prometheus_net.Metrics.CreateGauge("tasks_count", "Number of tasks with a specific status", new prometheus_net.GaugeConfiguration
        {
            LabelNames = new[] { "status" }
        });
    }

    public Task Execute(IJobExecutionContext context)
    {
        using var connection = new SqlConnection(_connectionString);
        connection.Open();

        // Perform SQL aggregation query
        string status = "InProgress";
        var startTime = DateTime.UtcNow.AddMinutes(-5); // Adjust to a 5-minute interval

        using var command = new SqlCommand(
            "SELECT COUNT(*) FROM YourTable WHERE Status = @Status AND Timestamp >= @StartTime",
            connection
        );
        command.Parameters.AddWithValue("@Status", status);
        command.Parameters.AddWithValue("@StartTime", startTime);

        var count = (int)command.ExecuteScalar();

        Console.WriteLine($"Number of tasks with status '{status}' in the last 5 minutes: {count}");

        // Set the gauge metric value
        _taskCountMetric.WithLabels(status).Set(count);

        return Task.CompletedTask;
    }
}
