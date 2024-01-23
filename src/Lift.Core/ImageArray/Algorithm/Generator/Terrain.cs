using Lift.Core.Externals;

namespace Lift.Core.ImageArray.Algorithm;

/// <summary>
/// 
/// </summary>
public static partial class Algorithm
{
    ///// <summary>
    ///// 使用柏林噪音生成地形图
    ///// </summary>
    ///// <param name="width"></param>
    ///// <param name="height"></param>
    ///// <param name="scale">缩放因子，影响地形的细节程度</param>
    ///// <param name="octaves">渐变频率的数量，影响地形的复杂性</param>
    ///// <param name="persistence">振幅的衰减系数，影响地形的平滑程度</param>
    ///// <param name="lacunarity">每个渐变的频率倍增因子</param>
    ///// <returns></returns>
    //public static Mat TerrainFromPerlinNoise(
    //    int width,
    //    int height,
    //    double scale = 0.1,
    //    int octaves = 6,
    //    double persistence = 0.5,
    //    double lacunarity = 2)
    //{
    //    var terrain = new Mat(height, width, MatType.CV_8UC1);

    //    // 使用Perlin Noise填充地形数据
    //    for (var y = 0; y < height; y++)
    //    {
    //        for (var x = 0; x < width; x++)
    //        {
    //            var xCoord = x * scale;
    //            var yCoord = y * scale;

    //            var perlinValue = GeneratePerlinNoise(xCoord, yCoord, octaves, persistence, lacunarity);

    //            // 将Perlin Noise值映射到0-255范围
    //            var pixelValue = (byte) ((perlinValue + 1.0) * 0.5 * 255);

    //            // 设置地形数据像素值
    //            terrain.Set(y, x, new Scalar(pixelValue));
    //        }
    //    }

    //    return terrain;
    //}

    //static double GeneratePerlinNoise(double x, double y, int octaves, double persistence, double lacunarity)
    //{
    //    var total = 0.0;
    //    var frequency = 1.0;
    //    var amplitude = 1.0;
    //    var maxValue = 0.0;

    //    for (int i = 0; i < octaves; i++)
    //    {
    //        // 使用OpenSimplexNoise库获取Perlin Noise值
    //        total += OpenSimplexNoise.Evaluate(x * frequency, y * frequency) * amplitude;


    //        maxValue += amplitude;

    //        // 调整振幅和频率
    //        amplitude *= persistence;
    //        frequency *= lacunarity;
    //    }

    //    // 将Perlin Noise值归一化到[-1, 1]范围
    //    return total / maxValue;
    //}

    /// <summary>
    /// 
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="seed"></param>
    /// <returns></returns>
    [Obsolete("这个方法有问题，它是真的完全随机")]
    public static Mat TerrainFromSimplexNoise(int width, int height, int seed = 0)
    {

        SimplexNoise.Seed = seed;
        var noise = SimplexNoise.Calc2D(width, height, 1);

        var rows = noise.GetLength(0);
        var cols = noise.GetLength(1);

        var mat = new Mat(rows, cols, MatType.CV_32FC1);

        for (var i = 0; i < rows; i++)
        {
            for (var j = 0; j < cols; j++)
            {
                mat.Set<float>(i, j, noise[i, j]);
            }
        }

        mat.MinMaxLoc(out double min, out double max);

        mat = ((mat - min) / (max - min)).ToMat().ToU8();

        return mat;
    }
}
