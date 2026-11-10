using GuestBook.Web.Models;
using Microsoft.Data.SqlClient;

namespace GuestBook.Web.Services;

public class SqlStorageService
{
    private readonly string _connStr;

    public SqlStorageService(IConfiguration config)
    {
        _connStr = config.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string is missing.");
        EnsureTable();
    }

    private void EnsureTable()
    {
        const string sql = @"IF OBJECT_ID('dbo.GuestBookEntries','U') IS NULL
               CREATE TABLE dbo.GuestBookEntries(
                 Id INT IDENTITY PRIMARY KEY,
                 GuestName NVARCHAR(200) NOT NULL,
                 Message NVARCHAR(MAX) NOT NULL,
                 PhotoUrl NVARCHAR(500) NULL,
                 CreatedAt DATETIME2 NOT NULL);";
        using var conn = new SqlConnection(_connStr);
        conn.Open();
        using var cmd = new SqlCommand(sql, conn);
        cmd.ExecuteNonQuery();
    }

    public async Task AddEntryAsync(GuestBookEntry e)
    {
        const string sql = @"INSERT INTO dbo.GuestBookEntries (GuestName, Message, PhotoUrl, CreatedAt)
               VALUES (@n, @m, @p, @c);";
        await using var conn = new SqlConnection(_connStr);
        await conn.OpenAsync();
        await using var cmd = new SqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@n", e.GuestName);
        cmd.Parameters.AddWithValue("@m", e.Message);
        cmd.Parameters.AddWithValue("@p", (object?)e.PhotoUrl ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@c", e.CreatedAt);
        await cmd.ExecuteNonQueryAsync();
    }

    public async Task<List<GuestBookEntry>> GetTodayEntriesAsync()
    {
        var list = new List<GuestBookEntry>();
        await using var conn = new SqlConnection(_connStr);
        await conn.OpenAsync();
        await using var cmd = new SqlCommand(
            "SELECT Id, GuestName, Message, PhotoUrl, CreatedAt FROM dbo.GuestBookEntries ORDER BY CreatedAt DESC", conn);
        await using var r = await cmd.ExecuteReaderAsync();
        while (await r.ReadAsync())
            list.Add(new GuestBookEntry
            {
                Id = r.GetInt32(0),
                GuestName = r.GetString(1),
                Message = r.GetString(2),
                PhotoUrl = r.IsDBNull(3) ? null : r.GetString(3),
                CreatedAt = r.GetDateTime(4)
            });
        return list;
    }

    public async Task DeleteEntryAsync(int id)
    {
        const string sql = "DELETE FROM dbo.GuestBookEntries WHERE Id = @id;";
        await using var conn = new SqlConnection(_connStr);
        await conn.OpenAsync();
        await using var cmd = new SqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@id", id);
        await cmd.ExecuteNonQueryAsync();
    }

    public async Task<List<GuestBookEntry>> GetAllEntriesAsync()
    {
        var list = new List<GuestBookEntry>();
        await using var conn = new SqlConnection(_connStr);
        await conn.OpenAsync();
        await using var cmd = new SqlCommand(
            "SELECT Id, GuestName, Message, PhotoUrl, CreatedAt FROM dbo.GuestBookEntries ORDER BY CreatedAt DESC", conn);
        await using var r = await cmd.ExecuteReaderAsync();
        while (await r.ReadAsync())
            list.Add(new GuestBookEntry
            {
                Id = r.GetInt32(0),
                GuestName = r.GetString(1),
                Message = r.GetString(2),
                PhotoUrl = r.IsDBNull(3) ? null : r.GetString(3),
                CreatedAt = r.GetDateTime(4)
            });
        return list;
    }
}