using System.Data;
using BarberApiV1.Data;
using BarberApiV1.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Microsoft.Data.SqlClient;

namespace BarberApiV1.Repositories.Services;

public class RepositoryService : IRepository
{
    private readonly MyDbContext _context;
    
    public RepositoryService(MyDbContext context)
    {
        _context = context;
    }
    
    public async Task<TOutput> ExecuteStoredProcedureAsync<TOutput>(string storedProcedure, object parameters = null)
    {
        return await ExecuteSP<TOutput>(_context, storedProcedure, parameters);
    }

    public async Task<TOutput> ExecuteQueryAsync<TOutput>(string query, object parameters = null)
    {
        return await ExecuteQuery<TOutput>(_context, query, parameters);
    }
    
    private async Task<TOutput> ExecuteQuery<TOutput>(DbContext context, string query, object parameters = null)
    {
        try
        {
            var connection = context.Database.GetDbConnection();
            if (connection.State != ConnectionState.Open)
                await connection.OpenAsync();

            await using var command = new SqlCommand(query, (SqlConnection)connection)
            {
                CommandType = CommandType.Text
            };
        
            if (parameters != null)
            {
                foreach (var prop in parameters.GetType().GetProperties())
                {
                    var value = prop.GetValue(parameters);
                    if (value != null)
                    {
                        command.Parameters.AddWithValue($"@{prop.Name}", value);
                    }
                }
            }

            await using var reader = await command.ExecuteReaderAsync();
        
            if (typeof(TOutput).IsPrimitive || typeof(TOutput) == typeof(string) || typeof(TOutput) == typeof(DateTime))
            {
                if (await reader.ReadAsync())
                {
                    return (TOutput)Convert.ChangeType(reader[0], typeof(TOutput));
                }

                return default;
            }
        
            var result = await DataReaderToObject<TOutput>(reader);
            return result;
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    private async Task<TOutput> ExecuteSP<TOutput>(DbContext context, string storedProcedure,
        object parameters = null)
    {
        try
        {
            var connection = context.Database.GetDbConnection();
            if (connection.State != ConnectionState.Open)
                await connection.OpenAsync();

            await using var command = new SqlCommand(storedProcedure, (SqlConnection)connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            if (parameters != null)
            {
                foreach (var prop in parameters.GetType().GetProperties())
                {
                    var value = prop.GetValue(parameters);
                    if (value != null)
                    {
                        command.Parameters.AddWithValue($"@{prop.Name}", value);
                    }
                }
            }

            await using var reader = await command.ExecuteReaderAsync();
        
            if (typeof(TOutput).IsPrimitive || typeof(TOutput) == typeof(string) || typeof(TOutput) == typeof(DateTime))
            {
                if (await reader.ReadAsync())
                {
                    return (TOutput)Convert.ChangeType(reader[0], typeof(TOutput));
                }

                return default;
            }
        
            var result = await DataReaderToObject<TOutput>(reader);
            return result;
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    private async Task<TOutput> DataReaderToObject<TOutput>(SqlDataReader reader)
    {
        if (!await reader.ReadAsync())
            return default;
    
        var columns = Enumerable.Range(0, reader.FieldCount)
            .Select(i => reader.GetName(i))
            .ToList();
    
        var dataDict = new Dictionary<string, object>();
        foreach (var column in columns)
        {
            var ordinal = reader.GetOrdinal(column);
            dataDict[column] = reader.IsDBNull(ordinal) ? null : reader.GetValue(ordinal);
        }
    
        var json = JsonConvert.SerializeObject(dataDict);

        return JsonConvert.DeserializeObject<TOutput>(json);
    }
}