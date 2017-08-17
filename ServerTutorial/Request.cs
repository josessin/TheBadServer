using System;

namespace ServerTutorial
{
    class Request
    {
        public String Type { get; set; }
        public String URL { get; set; }
        public String Host { get; set; }

        public String Referer { get; set; }


        public Request(String type, String url, String host, String referrer)
        {
            this.Type = type;
            this.URL = url;
            this.Host = host;
            this.Referer = referrer;
        }

        public static Request GetRequest(String request)
        {
            if (String.IsNullOrEmpty(request)) return null;

            String[] tokens = request.Split(' ','\n');
            String type = tokens[0];
            String url = tokens[1];
            String host = tokens[4];
            String referer = "";

            for(int i = 0; i < tokens.Length;i++)
            {
                if(tokens[i] == "Referer:")
                {
                    referer = tokens[i + 1];
                    break;
                }
            }

            Console.WriteLine("GET REQUEST: ");
            Console.WriteLine(type);
            Console.WriteLine(url);
            Console.WriteLine(host);
            Console.WriteLine(referer);

            return new Request(type,url,host,referer);
        }
    }
}
