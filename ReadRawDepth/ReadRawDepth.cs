using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadRawDepth
{
    internal class ReadRawDepth : IDisposable
    {
        public ReadRawDepth(string path, int width, int height, float scale, bool gamma)
        {
            var raw = File.ReadAllBytes(path);
            _mat = new Mat(width, height, MatType.CV_32FC1, raw, 0);
            _path = path;
            if (scale != 1.0f)
            {
                _mat = _mat * scale;
            }
            else if (gamma)
            {
                var showing = _mat;
                for (int y = 0; y < showing.Rows; y++)
                {
                    for (int x = 0; x < showing.Cols; x++)
                    {
                        ref var e = ref showing.At<float>(x, y);
                        e = MathF.Pow(e, 1f / 2.2f);
                    }
                }
            }
        }
        public void Save()
        {
            var fullPath = Path.GetFullPath(_path);
            var fileName = Path.GetFileNameWithoutExtension(fullPath);
            var directory = Path.GetDirectoryName(fullPath);
            var mat = _mat;
            mat.MinMaxIdx(out var minIdx, out var maxIdx);
            Console.WriteLine($"min = {minIdx}, max = {maxIdx}");
            var saved = Path.Combine(directory, $"{fileName}.png");
            Console.WriteLine(saved);
            mat.SaveImage(saved);
        }
        public void Show()
        {
            var showing = _mat;
            Cv2.ImShow("show", showing);
            Cv2.WaitKey();
        }
        private Mat _mat;
        private string _path;
        private bool disposedValue;

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
        // ~ReadRawDepth()
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
