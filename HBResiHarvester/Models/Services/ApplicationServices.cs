using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bimorph.WebApi.Core;
using Autodesk.Revit.DB;

namespace HBResiHarvester
{
    internal class ApplicationServices
    {
        public Document Document { get; }

        public BimorphAPIClientService WebClientService { get; }

        public ApplicationServices(Document document , BimorphAPIClientService webClientService)
        {
            this.Document = document;

            this.WebClientService = webClientService;
        }
    }
}
