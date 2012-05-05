using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.ServiceModel.Web;

// NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IReconhecimentoZona" in both code and config file together.
[ServiceContract]
public interface IReconhecimentoZona
{
    [OperationContract]
    [WebGet(UriTemplate = "/ReconhecimentoZona?latitude={latitude}&longitude={longitude}", BodyStyle = WebMessageBodyStyle.Bare)]
    string IdentificaZona(double latitude, double longitude);
}
