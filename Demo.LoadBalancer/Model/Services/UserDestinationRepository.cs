using System.Data.SQLite;

namespace Demo.LoadBalancer.Model.Services;

public class UserDestinationRepository : IUserDestinationRepository
{
    private readonly IConfiguration configuration;

    public UserDestinationRepository(IConfiguration configuration)
    {
        this.configuration = configuration;
    }
    
    public async Task<string?> FindUserDestination(string email)
    {
        using var conn = await OpenConnectionAsync();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT DestinationId FROM UserDestinations WHERE Email=@Email";
        cmd.Parameters.AddWithValue("Email", email);
        string? destination = (await cmd.ExecuteScalarAsync()) as string;
        return destination;
    }

    public async Task UpsertUserDestination(string email, string destinationId)
    {
        using var conn = await OpenConnectionAsync();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = "REPLACE INTO UserDestinations (Email, DestinationId) VALUES (@Email, @DestinationId)";
        cmd.Parameters.AddWithValue("Email", email);
        cmd.Parameters.AddWithValue("DestinationId", destinationId);
        await cmd.ExecuteNonQueryAsync();
    }

    private async Task<SQLiteConnection> OpenConnectionAsync()
    {
        SQLiteConnection connection = new(configuration.GetConnectionString("DefaultConnection"));
        await connection.OpenAsync();
        return connection;
    }
}