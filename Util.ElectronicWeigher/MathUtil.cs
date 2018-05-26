using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Util.ElectronicWeigher
{
    public class MathUtil
    {
        public static decimal GetTargetWeight(decimal originWeight, string originUnitOfWeight, string targetUnitOfWeight)
        {
            decimal r = -9876M;
            switch (targetUnitOfWeight.ToUpper())
            {
                case "G": r = GetG(originWeight, originUnitOfWeight); break;
                case "KG": r = GetKG(originWeight, originUnitOfWeight); break;
                case "LBS": r = GetLBS(originWeight, originUnitOfWeight); break;
            }

            return r;
        }

        public static decimal GetG(decimal data, string unitOfWeight)
        {
            decimal r = -9876M;
            switch (unitOfWeight.ToUpper())
            {
                case "G": r = data; break;
                case "KG": r = data / 1000M; break;
                case "LBS": r = data * 453.59237M; break;
            }

            return r;
        }

        public static decimal GetKG(decimal data, string unitOfWeight)
        {
            decimal r = -9876M;
            switch (unitOfWeight.ToUpper())
            {
                case "G": r = data / 1000M; break;
                case "KG": r = data; break;
                case "LBS": r = data * 0.4535924M; break;
            }

            return r;
        }

        public static decimal GetLBS(decimal data, string unitOfWeight) // lb 则表示某力的大小, lbs 则表示质量
        {
            decimal r = -9876M;
            switch (unitOfWeight.ToUpper())
            {
                case "G": r = data * 0.0022046M; break;
                case "KG": r = data * 2.2046226M; break;
                case "LBS": r = data; break;
            }

            return r;
        }


    }
}
