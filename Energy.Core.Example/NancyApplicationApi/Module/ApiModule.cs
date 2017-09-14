namespace NancyApplicationApi.Module
{
    using Nancy;

    public class ApiModule : NancyModule
    {
        public ApiModule(): base("api")
        {
            Get["/"] = parameters =>
            {
                return Response.AsJson<object>(new
                {
                    list = "List1",
                    orderBy = "sort_by",
                });
            };
        }
    }
}