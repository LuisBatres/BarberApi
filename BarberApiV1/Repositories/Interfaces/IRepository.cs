namespace BarberApiV1.Repositories.Interfaces;

public interface IRepository
{
    //INTERFAZ PARA LAS CONSULTAS A LA BD
    
    Task<TOutput> ExecuteStoredProcedureAsync<TOutput>(string storedProcedure, object parameters = null);
    
    Task<TOutput> ExecuteQueryAsync<TOutput>(string query, object parameters = null);
}