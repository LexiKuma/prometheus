using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

public class Startup
{
    // ... other configurations ...

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        // ... other configurations ...

        app.UseMetricServer();
        app.UseHttpMetrics();

        // ... other configurations ...
    }
}
