using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeometryTasks;
using System.Drawing;

namespace GeometryPainting
{
    public static class SegmentExtension
    {
        public static Dictionary<Segment, Color> ColorDictionary = 
            new Dictionary<Segment, Color>();

        public static Color GetColor (this Segment segment)
        {
            if (ColorDictionary.ContainsKey(segment))
                return ColorDictionary[segment];
            return Color.Black;
        }

        public static void SetColor(this Segment segment, Color color)
        {
        	ColorDictionary[segment] = color;
        }
    }
}