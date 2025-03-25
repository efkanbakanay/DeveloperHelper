using Xunit;
using Xunit.Abstractions;

namespace DeveloperHelper.Testing;

public class TestRunner
{
    private readonly ITestOutputHelper _output;

    public TestRunner(ITestOutputHelper output)
    {
        _output = output;
    }

    public async Task RunAllTests()
    {
        _output.WriteLine("Starting all tests...");

        // Security Tests
        _output.WriteLine("\nRunning Security Tests...");
        var securityTests = new Security.SecurityHelperTests();
        await RunTest(() => securityTests.HashPassword_ShouldHashPassword());
        await RunTest(() => securityTests.VerifyPassword_WithValidPassword_ShouldReturnTrue());
        await RunTest(() => securityTests.VerifyPassword_WithInvalidPassword_ShouldReturnFalse());
        await RunTest(() => securityTests.GenerateJwtToken_ShouldGenerateValidToken());
        await RunTest(() => securityTests.ValidateJwtToken_WithValidToken_ShouldReturnClaimsPrincipal());
        await RunTest(() => securityTests.ValidateJwtToken_WithInvalidToken_ShouldReturnNull());
        await RunTest(() => securityTests.SanitizeHtml_ShouldRemoveXssContent());
        await RunTest(() => securityTests.SanitizeSql_ShouldRemoveSqlInjectionContent());
        await RunTest(() => securityTests.EncryptAndDecrypt_ShouldWorkCorrectly());

        // Logging Tests
        _output.WriteLine("\nRunning Logging Tests...");
        var loggingTests = new Logging.LoggerHelperTests();
        await RunTest(() => loggingTests.Configure_ShouldConfigureLogger());
        await RunTest(() => loggingTests.LogDebug_ShouldLogMessage());
        await RunTest(() => loggingTests.LogInformation_ShouldLogMessage());
        await RunTest(() => loggingTests.LogWarning_ShouldLogMessage());
        await RunTest(() => loggingTests.LogError_ShouldLogMessage());
        await RunTest(() => loggingTests.LogError_WithException_ShouldLogMessage());
        await RunTest(() => loggingTests.LogCritical_ShouldLogMessage());
        await RunTest(() => loggingTests.Log_WithLevel_ShouldLogMessage());
        await RunTest(() => loggingTests.Log_WithLevelAndException_ShouldLogMessage());

        // Cache Tests
        _output.WriteLine("\nRunning Cache Tests...");
        var cacheTests = new Cache.CacheHelperTests();
        await RunTest(() => cacheTests.Set_ShouldStoreValue());
        await RunTest(() => cacheTests.Get_WithExistingValue_ShouldReturnValue());
        await RunTest(() => cacheTests.Get_WithNonExistingValue_ShouldReturnDefault());
        await RunTest(() => cacheTests.GetOrSet_WithNonExistingValue_ShouldSetAndReturnValue());
        await RunTest(() => cacheTests.GetOrSet_WithExistingValue_ShouldReturnExistingValue());
        await RunTest(() => cacheTests.GetOrSetAsync_WithNonExistingValue_ShouldSetAndReturnValue());
        await RunTest(() => cacheTests.GetOrSetAsync_WithExistingValue_ShouldReturnExistingValue());
        await RunTest(() => cacheTests.Remove_ShouldRemoveValue());
        await RunTest(() => cacheTests.Clear_ShouldRemoveAllValues());
        await RunTest(() => cacheTests.Exists_WithExistingValue_ShouldReturnTrue());
        await RunTest(() => cacheTests.Exists_WithNonExistingValue_ShouldReturnFalse());
        await RunTest(() => cacheTests.Update_WithExistingValue_ShouldUpdateValue());
        await RunTest(() => cacheTests.Update_WithNonExistingValue_ShouldReturnFalse());
        await RunTest(() => cacheTests.SetOrUpdate_ShouldSetOrUpdateValue());

        // Database Tests
        _output.WriteLine("\nRunning Database Tests...");
        var databaseTests = new Database.DatabaseHelperTests();
        await RunTest(() => databaseTests.ExecuteStoredProcedureAsync_ShouldExecuteProcedure());
        await RunTest(() => databaseTests.ExecuteQueryAsync_ShouldExecuteQuery());
        await RunTest(() => databaseTests.ExecuteCommandAsync_ShouldExecuteCommand());
        await RunTest(() => databaseTests.ExecuteScalarAsync_ShouldExecuteScalar());
        await RunTest(() => databaseTests.ExecuteTransactionAsync_ShouldExecuteCommandsInTransaction());
        await RunTest(() => databaseTests.CreateDbContext_ShouldCreateContext());

        // HTTP Tests
        _output.WriteLine("\nRunning HTTP Tests...");
        var httpTests = new Http.HttpClientHelperTests();
        await RunTest(() => httpTests.GetAsync_ShouldReturnResponse());
        await RunTest(() => httpTests.GetAsync_WithType_ShouldReturnDeserializedResponse());
        await RunTest(() => httpTests.PostAsync_ShouldReturnResponse());
        await RunTest(() => httpTests.PostAsync_WithType_ShouldReturnDeserializedResponse());
        await RunTest(() => httpTests.PutAsync_ShouldReturnResponse());
        await RunTest(() => httpTests.PutAsync_WithType_ShouldReturnDeserializedResponse());
        await RunTest(() => httpTests.DeleteAsync_ShouldReturnResponse());

        _output.WriteLine("\nAll tests completed successfully!");
    }

    private async Task RunTest(Func<Task> testMethod)
    {
        try
        {
            await testMethod();
            _output.WriteLine($"✓ {testMethod.Method.Name}");
        }
        catch (Exception ex)
        {
            _output.WriteLine($"✗ {testMethod.Method.Name}: {ex.Message}");
            throw;
        }
    }

    private async Task RunTest(Action testMethod)
    {
        try
        {
            await Task.Run(testMethod);
            _output.WriteLine($"✓ {testMethod.Method.Name}");
        }
        catch (Exception ex)
        {
            _output.WriteLine($"✗ {testMethod.Method.Name}: {ex.Message}");
            throw;
        }
    }
} 