using IdentityModel.Client;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Plugins;
using System.Collections.Concurrent;

namespace Northwind.React.UI.Controllers;

public class OidcConfigurationController : Controller
{
    private readonly ILogger<OidcConfigurationController> _logger;

    public OidcConfigurationController(
        IClientRequestParametersProvider clientRequestParametersProvider,
        ILogger<OidcConfigurationController> logger)
    {
        ClientRequestParametersProvider = clientRequestParametersProvider;
        _logger = logger;

        if (OidcConfigurationController.ClientParameters == default 
            || (OidcConfigurationController.DictionaryRefreshedLast ?? DateTime.UtcNow.AddHours(-2)) < DateTime.UtcNow.AddHours(-1) )
        {
            OidcConfigurationController.ClientParameters = new ConcurrentDictionary<string, IDictionary<string, string>>();
            OidcConfigurationController.DictionaryRefreshedLast = DateTime.UtcNow;
        }
    }

    public IClientRequestParametersProvider ClientRequestParametersProvider { get; }

    // These things are needed for speed. Calling database each time is slow.
    private static ConcurrentDictionary<string, IDictionary<string, string>> ClientParameters { get; set; }

    private static object DictionaryLock = new object();

    private static DateTime? DictionaryRefreshedLast { get;set;}

    [HttpGet("_configuration/{clientId}")]
    public IActionResult GetClientRequestParameters([FromRoute] string clientId)
    {
        try
        {
            IDictionary<string, string> result;

            if (OidcConfigurationController.ClientParameters.TryGetValue(clientId, out result))
            {                
            }
            else
            {
                result = ClientRequestParametersProvider.GetClientParameters(HttpContext, clientId);

                if (Monitor.TryEnter(OidcConfigurationController.DictionaryLock,TimeSpan.FromSeconds(3)))
                {
                    _ = OidcConfigurationController.ClientParameters.TryAdd(clientId, result);

                    Monitor.Exit(OidcConfigurationController.DictionaryLock);
                }
                else
                {
                    // does not matter - we have the result object.
                }                
            }

            return Ok(result);
        }
        catch (Exception ex)
        {
            this._logger.LogError($"Error getting parameters for '{clientId}'", ex);
            throw;
        }        
    }
}
