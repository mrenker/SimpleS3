using System.Globalization;
using System.IO;
using Genbox.SimpleS3.Abstracts;
using Genbox.SimpleS3.Abstracts.Constants;
using Genbox.SimpleS3.Abstracts.Marshal;
using Genbox.SimpleS3.Core.Enums;
using Genbox.SimpleS3.Core.Internal.Xml;
using Genbox.SimpleS3.Core.Network.Requests.Multipart;
using Genbox.SimpleS3.Core.Network.Requests.S3Types;
using JetBrains.Annotations;

namespace Genbox.SimpleS3.Core.Internal.Marshal.Request.Multipart
{
    [UsedImplicitly]
    internal class CompleteMultipartUploadRequestMarshal : IRequestMarshal<CompleteMultipartUploadRequest>
    {
        public Stream MarshalRequest(CompleteMultipartUploadRequest request, IS3Config config)
        {
            request.AddQueryParameter(AmzParameters.UploadId, request.UploadId);
            request.AddHeader(AmzHeaders.XAmzRequestPayer, request.RequestPayer == Payer.Requester ? "requester" : null);

            //build the XML required to describe each part
            FastXmlWriter xml = new FastXmlWriter(512);
            xml.WriteStartElement("CompleteMultipartUpload");

            foreach (S3PartInfo partInfo in request.UploadParts)
            {
                xml.WriteStartElement("Part");
                xml.WriteElement("ETag", partInfo.ETag.Trim('"'));
                xml.WriteElement("PartNumber", partInfo.PartNumber.ToString(NumberFormatInfo.InvariantInfo));
                xml.WriteEndElement("Part");
            }

            xml.WriteEndElement("CompleteMultipartUpload");

            return new MemoryStream(xml.GetBytes());
        }
    }
}