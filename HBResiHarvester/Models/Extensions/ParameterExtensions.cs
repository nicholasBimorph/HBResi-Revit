using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;

namespace HBResiHarvester.Extensions
{
    public static class ParameterExtensions
    {

        /// <summary>
        /// Return the corresponding Storage Type value of a given Revit Parameter as a string representation
        /// </summary>
        /// <param name="p"></param>
        /// <returns>Returns null if input parameter is also null </returns>
        public static string GetParameterValueAsString(this Parameter p)
        {
            string output = "";
            if (p == null) return output;

            switch (p.StorageType)
            {

                case StorageType.Double:
                {
                    double value = Math.Round(p.AsDouble(),2);
                    string data = value.ToString();

                    if (data != null) 
                        output = data;

                    break;
                }
                case StorageType.Integer:
                {
                    string data = p.AsInteger().ToString();

                    if (data != null)
                        output = data;

                    break;
                }
                case StorageType.String:
                {
                    string data = p.AsString();

                    if (data != null)
                        output = data;
                    break;
                }
                case StorageType.ElementId:
                {
                    string data = p.AsElementId().IntegerValue.ToString();
                    if (data != null)
                        output = data;

                    break;
                }
                case StorageType.None:
                {
                    output = "none";
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return output;
        }
    }
}
