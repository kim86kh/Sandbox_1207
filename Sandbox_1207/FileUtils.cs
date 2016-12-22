using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Security.Cryptography;

namespace Sandbox_1207
{
    class FileUtils
    {
        static MD5 md5;
        static string infopath = ".drvinfo$$$";
        public FileUtils()
        {
            md5 = MD5.Create();
        }

        public void explore(string drv)
        {
            if (!Directory.Exists(drv + "\\" + infopath))
            {
                Directory.CreateDirectory(drv + "\\" + infopath);
            }
            List<string> pathlist = new List<string>();
            List<string> filelist = new List<string>();
            string hash = null;
            pathlist.Add(drv + "\\");
            //BufferedStream db = new BufferedStream(new FileStream(drv + "\\" + infopath + "\\filelist.txt", FileMode.Create));
            StreamWriter db = new StreamWriter(drv + "\\" + infopath + "\\filelist.txt");
            
            while (pathlist.Count > 0)
            {
                Console.Out.WriteLine(pathlist[0]);
                try
                {
                    pathlist.AddRange(Directory.GetDirectories(pathlist[0]));
                    foreach (string d in Directory.GetFiles(pathlist[0]))
                    {
                        Console.Out.Write("\t" + d);
                        int tick1 = Environment.TickCount;

                        FileInfo fi = new FileInfo(d);
                        if (fi.Length > (1048576 * 500))
                        {
                            hash = this.getMD5(getSnippetFromHugeFile(d, 1048576, 1048576 * 100));
                        }
                        else
                        if (fi.Length > (1048576 * 100))
                        {
                            hash = this.getMD5(getSnippetFromHugeFile(d, 1048576, 1048576 * 20));
                        }
                        else
                        if (fi.Length > (1048576 * 10))
                        {
                            hash = this.getMD5(getSnippetFromHugeFile(d, 1048576, 1048576 * 2));
                        }
                        else
                        {
                            hash = getMD5(new FileStream(d, FileMode.Open));
                        }
                        int tick2 = Environment.TickCount;
                        double elaspsed1 = (tick2 - tick1) / 1000.0;
                        Console.WriteLine("...." + elaspsed1.ToString() + " Sec");
                       // db.WriteLine(d + "\t"+ fi.Length+"\t" + hash);
                        db.Write(d);
                        db.Write("\t");
                        db.Write(fi.Length);
                        db.Write("\t");
                        db.Write(fi.CreationTime.ToBinary());
                        db.Write("\t");
                        db.Write(fi.LastWriteTime.ToBinary());
                        db.Write("\t");
                        db.Write(hash);

                        db.Write("\n");


                    }
                }
                catch (UnauthorizedAccessException e)
                {

                }
                catch(IOException e)
                {

                }
                pathlist.RemoveAt(0);

            }
            db.Close();
        }
        public string getMD5(Stream stream)
        {
            //return Encoding.ASCII.GetString(md5.ComputeHash(stream));
            byte[] md5result = md5.ComputeHash(stream);
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < md5result.Length; i++)
            {
                sBuilder.Append(md5result[i].ToString("x2"));
            }
            return sBuilder.ToString();

        }
        public Stream getSnippetFromHugeFile(string filename, int snippetSize, int interval)
        {
            byte[] b = new byte[snippetSize];
            BufferedStream bs = new BufferedStream(new FileStream(filename, FileMode.Open));
            BufferedStream combined = new BufferedStream(new MemoryStream());
            while (bs.Position < bs.Length)
            {
                bs.Read(b, 0, snippetSize);
                bs.Seek(interval - snippetSize, SeekOrigin.Current);
                combined.Write(b, 0, snippetSize);
            }
            combined.Seek(0, SeekOrigin.Begin);
            return combined;

            /*            StringBuilder sBuilder = new StringBuilder();
                        for (int i = 0; i < md5result.Length; i++)
                        {
                            sBuilder.Append(md5result[i].ToString("x2"));
                        }

                        Console.Out.WriteLine(sBuilder.ToString().Length);
                        Console.Out.WriteLine(md5result.Length);*/
        }
    }
}
