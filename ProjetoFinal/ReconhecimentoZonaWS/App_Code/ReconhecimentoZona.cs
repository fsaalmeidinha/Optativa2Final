using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

// NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ReconhecimentoZona" in code, svc and config file together.
public class ReconhecimentoZona : IReconhecimentoZona
{
    public string DoWork(string name)
    {
        return "HI , " + name + " Welcome to simplest REST Service";
    }

    public string IdentificaZona(double latitude, double longitude)
    {
        ReconhecimentoZonaRN.ReconhecimentoZonaRN rn = new ReconhecimentoZonaRN.ReconhecimentoZonaRN();
        return rn.IdentificarZona(latitude, longitude);
    }

}
