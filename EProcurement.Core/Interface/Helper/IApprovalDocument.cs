using EProcurement.Common.RequestModel.Operational;
using EProcurement.Common.ResponseModel.Operational;
using EProcurement.DataObjects.Models.Operational;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace EProcurement.Core.Interface.Helper
{
    public interface IApprovalDocument
    {
        bool GenerateHotelAccommodationApproval(HotelAccommodationRequest hotel, string path, long requestedBy);
    }
}
