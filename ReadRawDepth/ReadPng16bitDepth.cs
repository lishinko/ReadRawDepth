using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadRawDepth
{
    internal class ReadPng16bitDepth : IDisposable
    {
        struct color
        {
            public byte r; public byte g; public byte b; public byte a;
        }
        public ReadPng16bitDepth(string path, int offsetx, int offsety, int width, int height, bool gamma)
        {
            using (var original = new Mat(path, ImreadModes.Unchanged))
            {
                //var showing = original;
                //Cv2.ImShow("show", showing);
                //Cv2.WaitKey();
                BuildMat(offsetx, offsety, width, height, original);
            }
            _path = path;
        }

        private void BuildMat(int offsetx, int offsety, int width, int height, Mat original)
        {
            Rect rc = new Rect(offsetx, offsety, width, height);
            using (var depth = original[rc])
            {
                var m = new Mat(width * 2, height, MatType.CV_16UC1);
                for (int y = 0; y < depth.Cols; y++)
                {
                    for (int x = 0; x < depth.Rows; x++)
                    {
                        ref var e = ref depth.At<color>(x, y);
                        var m1 = (ushort)((e.r << (ushort)8) + (ushort)e.g);
                        m.Set<UInt16>(x, 2 * y, m1);

                        var m2 = (ushort)((e.b << (ushort)8) + (ushort)e.a);
                        m.Set<UInt16>(x, 2 * y + 1, m2);

                    }
                }
                _mat = m;

                _mat.MinMaxIdx(out var minIdx, out var maxIdx);
                Console.WriteLine($"min = {minIdx}, max = {maxIdx}");
            }
        }

        public void Show()
        {
            var showing = _mat;
            Cv2.ImShow("show", showing);
            Cv2.WaitKey();
        }
        public void Log()
        {
            var fullPath = Path.GetFullPath(_path);
            var mat = _mat;
            mat.MinMaxIdx(out var minIdx, out var maxIdx);
            Console.WriteLine($"min = {minIdx}, max = {maxIdx}");
        }
        private Mat _mat;
        private bool disposedValue;
        private readonly string _path;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: 释放托管状态(托管对象)
                    _mat.Dispose();
                }

                // TODO: 释放未托管的资源(未托管的对象)并重写终结器
                // TODO: 将大型字段设置为 null
                disposedValue = true;
            }
        }

        // // TODO: 仅当“Dispose(bool disposing)”拥有用于释放未托管资源的代码时才替代终结器
        // ~ReadPng16bitDepth()
        // {
        //     // 不要更改此代码。请将清理代码放入“Dispose(bool disposing)”方法中
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // 不要更改此代码。请将清理代码放入“Dispose(bool disposing)”方法中
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
