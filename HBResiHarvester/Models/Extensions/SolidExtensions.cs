using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Bimorph.WebApi.Core.Models.Types;

namespace HBResiHarvester.Extensions
{
    public static class SolidExtensions
    {
        /// <summary>
        /// Converts the Revit <paramref name="solid"/> in to a
        /// <see cref="BimorphMesh"/>
        /// </summary>
        /// <param name="solid"></param>
        /// <param name="geometryInstance">
        /// The  <see cref="GeometryInstance"/> that was used to extract the <paramref name="solid"/>.
        /// </param>
        public static BimorphMesh ToBimorphMesh(this Solid solid, GeometryInstance geometryInstance)
        {
            var faceArray = solid.Faces;

            int totalFaces = faceArray.Size;

            var faces = new int[totalFaces][];

            var verticesTemp = new List<XYZ>();

            var instanceTransform = geometryInstance.Transform;


            int faceCounter = 0;

            foreach (Face face in faceArray)
            {
                var faceAsMesh = face.Triangulate();

                var faceAsMeshVertices = faceAsMesh.Vertices;

                int totalFacesInMeshFace = faceAsMesh.NumTriangles;


                for (int i = 0; i < totalFacesInMeshFace; i++)
                {
                    var triangle = faceAsMesh.get_Triangle(i);

                    var triangleIndices = new int[3];

                    for (int j = 0; j < 3; ++j)
                    {
                        int k = (int)triangle.get_Index(j);

                        triangleIndices[j] = k;

                    }

                    faces[faceCounter] = triangleIndices;

                }

                verticesTemp.AddRange(faceAsMeshVertices.Select(vertex => instanceTransform.OfPoint(vertex)));

                faceCounter++;
            }

            var vertices = new double[verticesTemp.Count][];

            for (int i = 0; i < verticesTemp.Count; i++)
            {
                var vertex = verticesTemp[i];

                vertices[i] = new[] { vertex.X, vertex.Y, vertex.Z };
            }

            return new BimorphMesh(vertices, faces);
        }
    }
}
