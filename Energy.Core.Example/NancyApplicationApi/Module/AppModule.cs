namespace NancyApplicationApi.Module
{
    using Nancy;

    public class AppModule : NancyModule
    {
        public AppModule()
        {
            Get["/app"] = parameters =>
            {
                return View["app"];
            };
        }
    }
}