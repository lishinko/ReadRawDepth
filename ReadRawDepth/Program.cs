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
        [Option('w', "width", Required = true,  HelpText = "宽度")]
        public int Width { get; set; }
        [Option('h', "height", Required = true,  HelpText = "高度")]
        public int Height { get; set; }
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            var result = Parser.Default.ParseArguments<Options>(args);
            //.WithParsed<Options>(options =>
            //{
            //    path = options.File;
            //    scale = options.Scale;
            //    gamma = options.Gamma;
            //});
            if (result.Tag != ParserResultType.Parsed)
            {
                return;
            }
            var options = result.Value;
            if (options.File == null)
            {
                throw new NullReferenceException("no path to read");
            }
            using (var d = new ReadRawDepth(options.File, options.Width, options.Height, options.Scale, options.Gamma))
            {
                d.Show();
                //d.Save();
            }
            //using(var d = new ReadPng16bitDepth(path, 512,0, 512, 1024, true))
            //{
            //    d.Show();
            //}
        }
    }
}