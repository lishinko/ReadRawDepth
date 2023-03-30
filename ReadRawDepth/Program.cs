using CommandLine;
using System.Diagnostics.CodeAnalysis;
using OpenCvSharp;

namespace ReadRawDepth
{
    public class Options
    {
        [Option('f', "file", Required = true, HelpText = "输入要读取的文件")]
        [NotNull]
        public string File { get; set; }
        [Option('s', "scale", Required = false, Default = 1, HelpText = "如果图片不明显,可以输入这个变的明显一点")]
        public int Scale { get; set; }
        [Option('g', "gamma", Required = false, Default = false, HelpText = "true = 对图片反向gamma矫正,同时忽略scale属性")]
        public bool Gamma { get; set; }
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            string path = null;
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
            var raw = File.ReadAllBytes(path);
            var fullPath = Path.GetFullPath(path);
            var fileName = Path.GetFileNameWithoutExtension(fullPath);
            var directory = Path.GetDirectoryName(fullPath);
            //注意,我们这个贴图是没有padding的
            using (Mat mat = new Mat(528, 528, MatType.CV_32FC1, raw, 0))
            {
                //Mat showing = new Mat();
                //Cv2.ConvertScaleAbs(mat, showing, 25500, 0);
                mat.MinMaxIdx(out var minIdx, out var maxIdx);
                Console.WriteLine($"min = {minIdx}, max = {maxIdx}");
                var saved = Path.Combine(directory, $"{fileName}.png");
                Console.WriteLine(saved);
                mat.SaveImage(saved);

                //using var showing = mat.Normalize(0, 255, NormTypes.Hamming2);
                Mat showing = mat;
                if (scale != 1.0f)
                {
                    showing = mat * (float)scale;
                }
                else if (gamma)
                {
                    for (int y = 0; y < showing.Rows; y++)
                    {
                        for (int x = 0; x < showing.Cols; x++)
                        {
                            ref var e = ref showing.At<float>(x, y);
                            e = MathF.Pow(e, 1f / 2.2f);
                        }
                    }
                }
                Cv2.ImShow("show", showing);
                Cv2.WaitKey();
            }
        }
    }
}