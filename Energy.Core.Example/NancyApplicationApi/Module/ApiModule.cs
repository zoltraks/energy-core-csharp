namespace NancyApplicationApi.Module
{
    using Nancy;
    using System;
    using System.Text;

    public class ApiModule : NancyModule
    {
        public ApiModule() : base("api")
        {
            MakeRootRoute();

            MakeEchoRoute();

            MakeCurrencyRoute();
        }

        private void MakeRootRoute()
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

        private void MakeEchoRoute()
        {
            Get["/echo"] = _ =>
            {
                try
                {
                    System.Diagnostics.Process process = new System.Diagnostics.Process();
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.FileName = "bash1";
                    process.StartInfo.Arguments = "/mnt/c/WORK/17_09_16/ascript.sh";
                    process.StartInfo.CreateNoWindow = true;
                    process.StartInfo.RedirectStandardOutput = true;
                    process.Start();

                    StringBuilder responseText = new StringBuilder();
                    string line;
                    while ((line = process.StandardOutput.ReadLine()) != null)
                    {
                        responseText.AppendLine(line);
                    }

                    return Response.AsJson<object>(new
                    {
                        responseText = responseText.ToString()
                    });
                }
                catch (Exception x)
                {
                    return Response.AsJson<object>(new
                    {
                        exceptionMessage = x.Message
                    });
                }
            };
        }

        private void MakeCurrencyRoute()
        {
            string[] names = Energy.Base.Class.GetFieldsAndProperties(typeof(Energy.Base.Currency.Manager), false);
            foreach (string name in names)
            {
                string route = "/currency/" + name;
                Get[route] = _ =>
                {
                    object o = Energy.Base.Class.GetFieldOrPropertyValue(Energy.Base.Currency.Manager.Default, name, false, false);
                    return Response.AsJson<Energy.Base.Currency>(o as Energy.Base.Currency);
                };
            }
        }
    }
}