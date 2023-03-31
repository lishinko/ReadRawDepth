using CommandLine;
using System.Diagnostics.CodeAnalysis;
using OpenCvSharp;

namespace ReadRawDepth
{
    public class Options
    {
        [Option('f', "file", Required = true, HelpText = "输入要读取的文件")]
        [NotNull]
        public string File { get; set; } = string.Empty;
        [Option('s', "scale", Required = false, Default = 1, HelpText = "如果图片不明显,可以输入这个变的明显一点")]
        public int Scale { get; set; }
        [Option('g', "gamma", Required = false, Default = false, HelpText = "true = 对图片反向gamma矫正,同时忽略scale属性")]
        public bool Gamma { get; set; }
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            string path = string.Empty;
            int scale = 1;
            bool gamma = false;
            Parser.Default.ParseArguments<Options>(args)
                .WithParsed<Options>(options =>
                {
                    path = options.File;
                    scale = options.Scale;
                    gamma = options.Gamma;
                });
            if (path == null)
            {
                throw new NullReferenceException("no path to read");
            }
            using (var d = new ReadRawDepth(path, 528, 528, 1.0f, true))
            {
                d.Show();
            }
        }
    }
}