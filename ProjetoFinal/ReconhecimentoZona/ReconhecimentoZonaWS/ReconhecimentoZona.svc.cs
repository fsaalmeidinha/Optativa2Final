﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace ReconhecimentoZonaWS
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ReconhecimentoZona" in code, svc and config file together.
    public class ReconhecimentoZona : IReconhecimentoZona
    {
        [System.ServiceModel.OperationContract]
        [System.ServiceModel.Web.WebGet(UriTemplate = "cars/Carsearch?carName={theCarName}&color={theColor}&price={thePrice}")]
        string SearchCar(string theCarName, string theColor, string thePrice);
    }
}
