using System;
using System.IO;
using System.Net.Sockets;


namespace ServerTutorial
{
    class Response
    {

        private Byte[] data = null;
        private String status;
        private String mime;

        private Response(String status, String mime, Byte[] data)
        {
            this.data = data;
            this.status = status;
            this.mime = mime;
        }

        public static Response From(Request request)
        {
            if(request == null)
            {
                return MakeErrorFile(ErrorType.BadRequest);
            }

            if(request.Type=="GET")
            {
                
                String file = Environment.CurrentDirectory + HTTPServer.WEB_DIR + request.URL;
                FileInfo fi = new FileInfo(file);
                if(fi.Exists && fi.Extension.Contains("."))
                {
                    Console.WriteLine("Page Found!");
                    return MakeFromFile(fi);
                }
                else
                {
                    DirectoryInfo di = new DirectoryInfo(fi + "/");
                    if (!di.Exists)
                    {
                        return MakeErrorFile(ErrorType.NotFound);
                    }
                    FileInfo[] files = di.GetFiles();
                    foreach(FileInfo ff in files)
                    {
                        String s = ff.Name;
                        if (s.Contains("default.html") || s.Contains("default.htm") || s.Contains("index.html") || s.Contains("index.htm"))
                        {
                            return MakeFromFile(ff);
                        }
                    }
                }

            }
            else
            {
                return MakeErrorFile(ErrorType.NotAllowed);
            }

            return MakeErrorFile(ErrorType.NotFound);
        }

        private static Response MakeFromFile(FileInfo file)
        {
            FileStream fs = file.OpenRead();
            BinaryReader reader = new BinaryReader(fs);
            Byte[] b = new Byte[fs.Length];
            reader.Read(b, 0, b.Length);
            fs.Close();
            return new Response("200 OK", "html/text", b);
        }

        
        private static Response MakeErrorFile(ErrorType type)
        {
            int name = (int)type;
            String file = Environment.CurrentDirectory + HTTPServer.MSG_DIR + "/" + name + ".html";
            FileInfo fi = new FileInfo(file);
            FileStream fs = fi.OpenRead();
            BinaryReader reader = new BinaryReader(fs);
            Byte[] b = new Byte[fs.Length];
            reader.Read(b, 0, b.Length);

            String status = "";

            switch (type)
            {
                case ErrorType.BadRequest:
                    status = "400 Bad Request";
                    break;
                case ErrorType.NotAllowed:
                    status = "405 Method Not Allowed";
                    break;
                case ErrorType.NotFound:
                    status = "404 Not Found";
                    break;
                    
            }

            fs.Close();
            return new Response(status, "html/text", b);
        }

        public void Post(NetworkStream stream)
        {
            StreamWriter writer = new StreamWriter(stream);
            String line = String.Format("{0} {1}\r\nServer: {2}\r\nContent-Type: {3}\r\nAccept-Ranges: bytes\r\nContent-Length: {4}\r\n"
                , HTTPServer.VERSION, status, HTTPServer.NAME, mime, data.Length);
            Console.WriteLine("POST: " + line);
            writer.WriteLine(line);
            stream.Write(data,0,data.Length);
        }
 
    }

    public enum ErrorType
    {
        BadRequest = 400,
        NotFound = 404,
        NotAllowed = 405,

    };

}
