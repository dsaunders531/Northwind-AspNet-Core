namespace tools.Models
{
    public enum ApiMethod
    {
        GET = 0,
        POST = 1,
        PUT = 2,
        PATCH = 3,
        DELETE = 4
    }

    public enum ApiActionParameterType
    {
        indeterminate = 0,
        jsonBody = 1,
        query = 2,
        route = 3,
        form = 4,
        header = 5
    }
}
