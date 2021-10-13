using EProcurement.DataObjects.Models.MasterData;
using EProcurement.DataObjects.Models.Operational;
using EProcurement.Common.RequestModel.Operational;
using Microsoft.EntityFrameworkCore;
using EProcurement.Core.Interface.Helper;
using EProcurement.Common.Model;
using EProcurement.DataObjects;
using System.Collections.Generic;
using Newtonsoft.Json;
using SelectPdf;
using System;
using System.IO;
using System.Linq;

namespace EProcurement.Core.Service.Helper
{
    public class ApprovalDocumentService : IApprovalDocument
    {
        private readonly ISharePoint _sharePoint;
        private readonly IRepositoryBase<Person> _personRepository;
        private readonly IRepositoryBase<CostCenter> _costCenterRepository;
        private readonly IRepositoryBase<Station> _stationRepository;
        private readonly IRepositoryBase<Country> _countryRepository;
        private readonly IRepositoryBase<HARApprover> _harApproverRepository;
        private readonly IRepositoryBase<HARDelegateTeam> _harDelegateTeamRepository;
        public ApprovalDocumentService(
          IRepositoryBase<Person> personRepository,
          IRepositoryBase<CostCenter> costCenterRepository,
          IRepositoryBase<Station> stationRepository,
          IRepositoryBase<Country> countryRepository,
          IRepositoryBase<HARApprover> harApproverRepository,
          IRepositoryBase<HARDelegateTeam> harDelegateTeamRepository,
        ISharePoint sharePoint)
        {
            _sharePoint = sharePoint;
            _personRepository = personRepository;
            _costCenterRepository = costCenterRepository;
            _stationRepository = stationRepository;
            _countryRepository = countryRepository;
            _harApproverRepository = harApproverRepository;
            _harDelegateTeamRepository = harDelegateTeamRepository;
        }
        public bool GenerateHotelAccommodationApproval(HotelAccommodationRequest hotel, string path,long requestedBy)
        {
            var hotelTemplatePath = Directory.GetCurrentDirectory() + "\\Templates\\HotelAccomodationApproval.html";

            //
            var approversRequest = new List<HotelARApproversRequest>();
            if (!string.IsNullOrEmpty(hotel.Approvers))
            {
                hotel.Approvers = hotel.Approvers.Replace("\\", "");
                approversRequest = JsonConvert.DeserializeObject<List<HotelARApproversRequest>>(hotel.Approvers);
            }
            var approvers = GetApproverInformation(approversRequest);
            var costCenter = _costCenterRepository.Find(hotel.CostCenterId);
            var station = _stationRepository.Find(hotel.StationId);
            var country = _countryRepository.Find(hotel.CountryId);
            var requester = _personRepository.Find(requestedBy);

            var hotelTemplate = File.ReadAllText(hotelTemplatePath);
            hotelTemplate = hotelTemplate.Replace("``a", DateTime.Now.Date.ToString());
            hotelTemplate = hotelTemplate.Replace("``c", hotel.Section);
            hotelTemplate = hotelTemplate.Replace("``b", hotel.RequestName);
            hotelTemplate = hotelTemplate.Replace("``d", costCenter.CostCenterName);
            hotelTemplate = hotelTemplate.Replace("``e", hotel.RequestDate.Date.ToString());
            hotelTemplate = hotelTemplate.Replace("``f", hotel.HotelServiceType.ToString());
            hotelTemplate = hotelTemplate.Replace("``g", station.CityCode);
            hotelTemplate = hotelTemplate.Replace("``h", hotel.City);
            hotelTemplate = hotelTemplate.Replace("``i", country.CountryName);
            hotelTemplate = hotelTemplate.Replace("``j", hotel.Commencementdate.Date.ToString());
            hotelTemplate = hotelTemplate.Replace("``k", hotel.CrewPattern);
            hotelTemplate = hotelTemplate.Replace("``l", path);
            hotelTemplate = hotelTemplate.Replace("``m", requester.FirstName + " " + requester.MiddleName);
            hotelTemplate = hotelTemplate.Replace("``n", approvers);
            HtmlToPdf converter = new HtmlToPdf();
            PdfDocument doc = converter.ConvertHtmlString(hotelTemplate);
            doc.Save("HotelApproval.pdf");
            var filePath = @"G:\Development\Project\E-Procurement\API\EProcurement.Api\HotelApproval.pdf";
            _sharePoint.UploadFileToSharePointOnlineAsync(new FileConfig
            {
                FilePath = filePath,
                SiteUrl = @"https://azuredevelopersethiopianair.sharepoint.com/sites/eprocurement",
                FileName = filePath.Substring(filePath.LastIndexOf("\\") + 1)
            });
            return true;
        }
        public string GetApproverInformation(List<HotelARApproversRequest> approverRequests)
        {
            var approversText = "";
            try
            {
                var approvers = _harApproverRepository.Where(x => approverRequests.Select(x => x.PersonId).Contains(x.PersonId.Value))
                    .Include(x=>x.Person)
                    .ToList();
                if (approvers.Count() == 0)
                    return approversText;
                foreach (var approver in approvers)
                {
                    approversText += " <br> " + approver.Person.FirstName + " " + approver.Person.MiddleName + " - " + approver.Person.Position;
                }
                return approversText;

            }
            catch (Exception ex)
            {
                return approversText;
            }
        }
    }
}
