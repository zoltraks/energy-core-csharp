namespace NancyApplicationApi.Module
{
    using Nancy;
    using System;
    using System.Text;

    public class ApiModule : NancyModule
    {
        public ApiModule() : base("api")
        {
            Get["/"] = parameters =>
            {
                return Response.AsJson<object>(new
                {
                    list = "List1",
                    orderBy = "sort_by",
                });
            };

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
    }
}